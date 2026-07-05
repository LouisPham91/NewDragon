using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using AdvancedSharpAdbClient.Receivers;
using Dragon.Controller.DeviceControl;
using FFmpeg.AutoGen;
using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;

namespace Dragon.Controller.DeviceControl.ScrcpyNet.Services
{
    public class Scrcpy : IDisposable
    {
        public string DeviceID { get; }
        /// <summary>
        /// Cổng Free Port được gán cho thiết bị này
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Đối tượng nhận output từ adb shell khi khởi động scrcpy-server
        /// </summary>
        ConsoleOutputReceiver ReceiverServer = new ConsoleOutputReceiver();

        /// <summary>
        /// Đây là một singleton pool (bộ nhớ đệm dùng chung) do .NET cung cấp.
        /// Nó được thiết kế để tái sử dụng mảng byte nhằm giảm chi phí cấp phát/thu hồi bộ nhớ khi làm việc với stream, socket, video…
        /// Các hàm liên quan: ReadDeviceInfo, VideoMain
        /// </summary>
        private static readonly ArrayPool<byte> pool = ArrayPool<byte>.Shared;

        /// <summary>
        /// Scrcpy-Sever Gửi : Chiều rộng của thiết bị, sau khi được Resize
        /// </summary>
        public int Width { get; internal set; }
        /// <summary>
        /// Scrcpy-Sever Gửi : Chiều dài của thiết bị, sau khi được Resize
        /// </summary>
        public int Height { get; internal set; }
        /// <summary>
        /// Scrcpy-Sever Gửi : Tên thiết bị
        /// </summary>
        public string DeviceName { get; private set; } = "";

        /// <summary>
        /// Từ Adb Shell : Chiều rộng gốc của thiết bị, lấy từ scrcpy-server
        /// </summary>
        public int PhysicalWidth = 0;
        /// <summary>
        /// Từ Adb Shell : Chiều dài gốc của thiết bị, lấy từ scrcpy-server
        /// </summary>
        public int PhysicalHeight = 0;

        /// <summary>
        /// Setting Value : Bitrate Video Stream (mặc định 8Mbps)
        /// </summary>
        public long Bitrate { get; set; } = 8000000;


        /// <summary>
        /// Setting Value : Frame Rate Video Stream (mặc định 60fps)
        /// </summary>
        public int Fps { get; set; } = 60;
        /// <summary>
        /// Setting Value : Kích thước lớn nhất của chiều dài hoặc chiều rộng sau khi Resize (mặc định 960px)
        /// </summary>
        public int MaxSize { get; set; } = 960;

        /// <summary>
        /// Đường dẫn tới file scrcpy-server trên máy tính host
        /// </summary>
        public string ScrcpyServerFile { get; set; } = Path.Combine(AppContext.BaseDirectory, "Extension", "ScrcpyNet", "scrcpy-server");

        /// <summary>
        /// Trạng thái kết nối hiện tại
        /// </summary>
        public volatile bool Connected = false;
        /// <summary>
        /// Scrcpy Server ID, dùng để tạo forward/reverse port
        /// </summary>
        private int SCID = new Random().Next(10000000, 12345678);

        /// <summary>
        /// Bộ giải mã luồng video
        /// </summary>
        public VideoStreamDecoder VideoStreamDecoder { get; }
        /// <summary>
        /// Các đối tượng TcpClient và TcpListener để quản lý kết nối mạng
        /// </summary>
        private TcpClient? _videoClient;
        /// <summary>
        /// Đối tượng TcpClient để quản lý kết nối điều khiển
        /// </summary>
        private TcpClient? _controlClient;
        /// <summary>
        /// Đối tượng TcpListener để lắng nghe kết nối đến
        /// </summary>
        private TcpListener? _listener;
        /// <summary>
        /// CancellationTokenSource để quản lý hủy bỏ các tác vụ bất đồng bộ
        /// </summary>
        private CancellationToken _cts;

        /// <summary>
        /// Đối tượng AdbClient để tương tác với thiết bị Android qua ADB
        /// </summary>
        private AdbClient _adb;
        /// <summary>
        /// Dữ liệu thiết bị Android được kết nối
        /// </summary>
        //private readonly DeviceData _device;
        private readonly DeviceData _device;
        /// <summary>
        /// Biến theo dõi trạng thái Dispose của đối tượng
        /// </summary>
        private bool _disposed;
        /// <summary>
        /// Chế độ sử dụng reverse port forwarding (mặc định true cho android 7 trở lên ) và ngược lại cho Android 6 trở xuống
        /// </summary>
        private volatile bool _useReverse = true;

        /// <summary>
        /// Event được kích hoạt khi nhận được kích thước tải từ scrcpy-server, kick thước sẽ đc resize, Height = MaxSize
        /// </summary>
        public event Action<int, int>? OnLoadSizeEvent;

        public event Action<bool>? OnConnecting;


        public event Action<AVFrame> OnFrame
        {
            add
            {
                if (VideoStreamDecoder != null)
                {
                    VideoStreamDecoder.NewFrameEvent += value;
                }
            }
            remove
            {
                if (VideoStreamDecoder != null)
                {
                    VideoStreamDecoder.NewFrameEvent -= value;
                }
            }
        }
        // Chúng ta thêm các trường để giữ lại Task nền
        /// <summary>
        /// Các Task nền để xử lý buffer đẩy data thành video frames
        /// </summary>
        private Task? _bufferTask;
        /// <summary>
        /// Các Task nền để xử lý video điều khiển
        /// </summary>
        private Task? _videoTask;
        /// <summary>
        /// Các Task nền để xử lý điều khiển
        /// </summary>
        private Task? _controllerTask;

        // Đặt hằng số cho header và giới hạn kích thước packet
        /// <summary>
        /// Kích thước của meta header trong luồng video (12 byte)
        /// </summary>
        private const int MetaHeaderSize = 12;
        /// <summary>
        /// Kích thước tối đa của một packet video (2 MB)
        /// </summary>
        private const int MaxPacketSize = 2 * 1024 * 1024; // 2 MB, điều chỉnh nếu cần



        // Cấu hình bounded channel cho Buffer (video packets) và Control messages:
        /// <summary>
        /// Kênh bounded channel để lưu trữ các AVPacket video, 256 là số lượng tối đa trong kênh và sẽ drop oldest khi đầy
        /// </summary>
        // thay đổi khai báo channel
        //private readonly Channel<AVPacket> _bufferChannel = Channel.CreateBounded<AVPacket>(
        //    new BoundedChannelOptions(capacity: 256)
        //    {
        //        FullMode = BoundedChannelFullMode.DropOldest,
        //        SingleReader = true,
        //        SingleWriter = false
        //    }
        //);

        private readonly Channel<AVPacket> _bufferChannel = Channel.CreateBounded<AVPacket>(
            new BoundedChannelOptions(capacity: 256)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                SingleWriter = false
            }
        );
        // ngay sau dòng 198
        private byte[]? _pendingConfig = null;

        /// <summary>
        /// Kênh bounded channel để lưu trữ các control messages, 64 là số lượng tối đa trong kênh và sẽ drop oldest khi đầy
        /// </summary>
        private readonly Channel<IControlMessage> _controlChannel = Channel.CreateBounded<IControlMessage>(
            new BoundedChannelOptions(capacity: 64)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                SingleWriter = true
            }
        );


        /// <summary>
        /// Khởi tạo một thể hiện mới của lớp Scrcpy cho thiết bị cụ thể
        /// </summary>
        /// <param name="device"></param>
        /// <param name="videoStreamDecoder"></param>
        public Scrcpy(string deviceID, DeviceData device, VideoStreamDecoder? videoStreamDecoder = null)
        {
            DeviceID = deviceID;

            Port = ScrcpyPort.GetNextAvailablePort();

            //Debug.WriteLine($"{device.Serial} : Port is {Port}");
            _adb = new AdbClient();
            _device = device;
            // Thường dùng H.264 làm mặc định
            VideoStreamDecoder = videoStreamDecoder ?? new VideoStreamDecoder();

        }
        public void SetPhySicalSize(int width, int height)
        {
            PhysicalWidth = width;
            PhysicalHeight = height;
        }

        /// <summary>
        /// Bắt đầu kết nối và khởi động Scrcpy
        /// </summary>
        /// <param name="timeoutMs"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task StartAsync(long timeoutMs = 20000, CancellationToken cancellationToken = default)
        {
            // Kiểm tra trạng thái kết nối hiện tại
            if (Connected)
                throw new InvalidOperationException("Already connected.");

            // Tổng Cộng có 3 bước chính
            // 1. Thiết lập kết nối (SetupConnection)
            // 2. Đọc thông tin thiết bị (ReadDeviceInfo)
            // 3. Khởi động các luồng xử lý (StartThreads)

            try
            {
                // Tạo CancellationTokenSource liên kết
                _cts = cancellationToken;
                VideoStreamDecoder.SetToken(cancellationToken);
                if (_cts.IsCancellationRequested) return;

                for (int attempt = 0; attempt < 5; attempt++)
                {
                    try
                    {
                        if (_cts.IsCancellationRequested) return;
                        await SetupConnection(timeoutMs);
                        // //log.Information("Connection setup completed for device {DeviceId}", _device.Serial);
                        break;
                    }
                    catch (Exception ex)
                    {
                        ////log.Warning("Connection attempt {Attempt} failed for device {DeviceId}: {Message}", attempt + 1, _device.Serial, ex.Message);
                        if (attempt == 4)
                        {
                            //log.Error("Failed to connect after 5 attempts for device {DeviceId}: {Message}", _device.Serial, ex.Message);
                            throw new Exception("CloseDevicePanel", ex);
                        }
                        await Task.Delay(1000 * (attempt + 1), cancellationToken);
                    }
                }

                ReadDeviceInfo();

                //log.Information("Starting threads for device {DeviceId}", _device.Serial);
                StartThreads();

                Connected = true;
                OnConnecting?.Invoke(true);
                //log.Information("Scrcpy started successfully for device {DeviceId}", _device.Serial);
            }
            catch (Exception)
            {
                //log.Error("Start failed for device {DeviceId}: {Message}", _device.Serial, ex.Message);
                CleanupConnection();
                Connected = false;
                throw;
            }
        }

        /// <summary>
        /// Thiết lập kết nối với thiết bị Android qua ADB và Scrcpy server
        /// </summary>
        /// <param name="timeoutMs"></param>
        /// <returns></returns>
        /// <exception cref="TimeoutException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        private async Task SetupConnection(long timeoutMs)
        {
            try
            {
                if (_cts.IsCancellationRequested) return;
                // Nếu _useReverse = true → dùng Reverse (adb reverse)
                // Reverse: Device mở cổng → ánh xạ tới Host
                //   - Thiết bị Android mở một cổng cố định.
                //   - Cổng đó được ánh xạ tới một cổng trên máy tính host.
                //   - Nghĩa là: device → host.
                //   - Ví dụ: adb reverse tcp:8080 tcp:8080
                //     => App trên thiết bị kết nối localhost:8080 sẽ forward ngược về PC.

                if (_useReverse)
                {
                    // tại sao phải tạo 1 hàm MobileServerSetupAsync() riêng để làm gì?
                    // vì tớ cần biết nó sẽ throw ngay nên khi throw _useReverse = false vì android 6 trở xuống
                    var isReverseSuccess = await MobileServerSetupAsync();
                    if (!isReverseSuccess)
                        throw new Exception("Failed to set up reverse forward.");

                    // Bắt đầu lắng nghe kết nối từ scrcpy-server trên thiết bị Android
                    _listener = new TcpListener(IPAddress.Loopback, Port);
                    _listener.Start();
                    //log.Information("Listener started on port {Port} for device {DeviceId}", Port, _device.Serial);

                    // Khởi động scrcpy-server trên thiết bị Android
                    MobileServerStart();

                    // Chờ kết nối từ scrcpy-server cho cả video và control sockets
                    for (int i = 0; i < 10; i++)
                    {
                        ReceiverServer.Flush();
                        await Task.Delay(100);
                        //Debug.WriteLine(ReceiverServer.ToString());
                    }

                    // Kết nối video và control sockets với timeout
                    var timeoutTask = Task.Delay((int)timeoutMs);
                    var acceptTask = _listener.AcceptTcpClientAsync();
                    if (await Task.WhenAny(acceptTask, timeoutTask) == timeoutTask)
                        throw new TimeoutException("Timeout waiting for video socket.");

                    // Kết nối video socket
                    _videoClient = await acceptTask;
                    //log.Information("Video socket connected for device {DeviceId}", _device.Serial);

                    // Kết nối control socket
                    timeoutTask = Task.Delay((int)timeoutMs);
                    var acceptControlTask = _listener.AcceptTcpClientAsync();
                    if (await Task.WhenAny(acceptControlTask, timeoutTask) == timeoutTask)
                        throw new TimeoutException("Timeout waiting for control socket.");

                    _controlClient = await acceptControlTask;
                    //log.Information("Control socket connected for device {DeviceId}", _device.Serial);
                }
                else // dạng forward port 
                {
                    if (_cts.IsCancellationRequested) return;
                    // Đặc điểm dạng forward port:
                    // - Phải đảm bảo port trên host (máy tính) không bị chiếm dụng
                    // - Nếu port bị chiếm dụng, cần đóng các kết nối hiện tại và xóa tất cả forward/reverse trước khi tiếp tục
                    // - Nếu port vẫn bị chiếm dụng, tiến hành kill port và lấy port mới
                    // - Cuối cùng, tạo forward port và kết nối tới scrcpy-server


                    // Đóng kết nối và xóa forward/reverse nếu port bị chiếm dụng
                    if (ScrcpyPort.isPortRunInUse(Port))
                    {
                        // đóng tất cả kết nối TCP client hiện tại trên port này
                        CloseTcpClientsForPort(Port);
                        // Xóa tất cả forward và reverse trên thiết bị này
                        await MobileServerCleanup();

                        // Kiểm tra lại, nếu vẫn bị chiếm dụng thì tiến hành kill port và lấy port mới
                        if (ScrcpyPort.IsPortInUse(Port))
                        {
                            ScrcpyPort.KillPort(Port);
                            Port = ScrcpyPort.GetNextAvailablePort();
                        }
                    }

                    // Xóa tất cả forward và reverse trên thiết bị này 1 lần nữa để chắc chắn
                    await MobileServerCleanup();

                    // B2: Tạo SCID mới


                    // B3: Tạo đối tượng AdbClient mới để tránh lỗi kết nối
                    _adb = new AdbClient(new IPEndPoint(IPAddress.Loopback, 5037));

                    // B4: Đẩy Scrcpy Server và khởi động Forward Port
                    await UploadMobileServer();

                    // B5: Đây là bước quan trọng nhất nhìn vào code dưới đây trong hàm MobileServerStart()
                    /*     if (!_useReverse)
                    /*     cmds.Add("tunnel_forward=true");                         
                    // Nếu _useReverse = false → dùng Forward (adb forward)
                    // Forward: Host mở cổng → ánh xạ tới Device
                    //   - Máy tính host mở một cổng cố định.
                    //   - Cổng đó được ánh xạ tới một cổng trên thiết bị Android.
                    //   - Nghĩa là: host → device.
                    //   - Ví dụ: adb forward tcp:8080 tcp:8080
                    //     => Kết nối localhost:8080 trên PC sẽ forward tới cổng 8080 trên thiết bị.
                    */
                    MobileServerStart();

                    // khi này nếu chúng ta kiểm tra bằng ReceiverServer.Flush() thì sẽ thấy đc giá trị chỉ cần check nó khác null, empty là đc
                    bool serverReady = false;
                    for (int i = 0; i < 20; i++)
                    {
                        ReceiverServer.Flush();
                        if (!string.IsNullOrEmpty(ReceiverServer.ToString()))
                        {
                            serverReady = true;
                            await Task.Delay(100);
                            break;
                        }
                        await Task.Delay(100);
                    }
                    if (!serverReady)
                        throw new TimeoutException("Scrcpy server did not start in time.");

                    // B6: Tạo Forward Port từ Host → Device
                    var port = await _adb.CreateForwardAsync(_device, $"tcp:{Port}", $"localabstract:scrcpy_{SCID}", true);
                    if (port == 0)
                        throw new Exception($"Failed to create forward port for device {_device.Serial}.");

                    // B7: Kết nối tới scrcpy-server qua TcpClient
                    _videoClient = new TcpClient();
                    var connectTask = _videoClient.ConnectAsync(IPAddress.Loopback, Port);
                    var timeoutTask = Task.Delay((int)timeoutMs);
                    if (await Task.WhenAny(connectTask, timeoutTask) == timeoutTask)
                        throw new TimeoutException("Timeout connecting to video socket.");
                    await connectTask;

                    // B8: Đọc dummy byte từ video stream để đảm bảo kết nối ổn định
                    // Đây là do scrcpy họ nói sẽ trả về dumpy byte khi kết nối thành công
                    var stream = _videoClient.GetStream();
                    int dummy = -1;
                    for (int i = 0; i < 30; i++)
                    {
                        // Đọc byte giả (dummy byte) để xác nhận kết nối
                        if (stream.DataAvailable)
                        {
                            dummy = stream.ReadByte();
                            break;
                        }
                        await Task.Delay(100);
                    }
                    if (dummy != 0)
                        throw new InvalidOperationException("Invalid dummy byte received.");

                    // B9: Kết nối tới control socket
                    _controlClient = new TcpClient();
                    var controlConnectTask = _controlClient.ConnectAsync(IPAddress.Loopback, Port);
                    var controlTimeoutTask = Task.Delay((int)timeoutMs);
                    if (await Task.WhenAny(controlConnectTask, controlTimeoutTask) == controlTimeoutTask)
                        throw new TimeoutException("Timeout connecting to control socket.");
                    await controlConnectTask;
                }

                if (_controlClient == null || !_controlClient.Connected || _cts.IsCancellationRequested)
                {
                    throw new InvalidOperationException("Failed to establish control client.");
                }
            }
            catch (Exception ex)
            {
                //log.Error("SetupConnection failed for device {DeviceId}: {Message}", _device.Serial, ex.Message);
                CleanupConnection();
                Debug.WriteLine($"[SetupConnection] Error: không thể kết nối với scrcpy_server , _useReverse = {_useReverse}! \n Thông tin lỗi : {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        private void ReadDeviceInfo()
        {
            if (_videoClient == null || !_videoClient.Connected || _cts.IsCancellationRequested)
                throw new InvalidOperationException("Video client is null or not connected.");

            var stream = _videoClient.GetStream();
            stream.ReadTimeout = 5000;

            // 1. Đọc Device Name (64 byte) – server luôn gửi nếu không bị tắt
            byte[] deviceNameBuf = pool.Rent(64);
            try
            {
                int read = ReadExact(stream, deviceNameBuf, 0, 64);
                if (read == 64)
                {
                    DeviceName = Encoding.UTF8.GetString(deviceNameBuf).TrimEnd('\0');
                    Debug.WriteLine($"[DeviceInfo] Device Name: {DeviceName}");
                }
                else
                {
                    Debug.WriteLine($"[DeviceInfo] Device name read {read}/64 bytes – maybe not sent?");
                    DeviceName = _device.Model ?? "Unknown";
                }
            }
            finally { pool.Return(deviceNameBuf); }

            // 2. Đọc Codec ID (4 byte)
            byte[] codecBuf = pool.Rent(4);
            try
            {
                int read = ReadExact(stream, codecBuf, 0, 4);
                if (read != 4) throw new IOException($"Codec ID: expected 4, got {read}");
                int codecId = BinaryPrimitives.ReadInt32BigEndian(codecBuf);
                Debug.WriteLine($"[DeviceInfo] Codec: 0x{codecId:X8} ({GetCodecName(codecId)})");
            }
            finally { pool.Return(codecBuf); }

            // 3. Đọc Session Meta (12 byte)
            byte[] sessionBuf = pool.Rent(12);
            try
            {
                int read = ReadExact(stream, sessionBuf, 0, 12);
                if (read != 12) throw new IOException($"Session meta: expected 12, got {read}");

                int flags = BinaryPrimitives.ReadInt32BigEndian(sessionBuf.AsSpan(0, 4));
                int width = BinaryPrimitives.ReadInt32BigEndian(sessionBuf.AsSpan(4, 4));
                int height = BinaryPrimitives.ReadInt32BigEndian(sessionBuf.AsSpan(8, 4));

                // Bit 31 phải = 1
                if ((flags & 0x80000000) == 0)
                    Debug.WriteLine($"[DeviceInfo] WARNING: Session flag not set! flags=0x{flags:X8}");

                Debug.WriteLine($"[DeviceInfo] Session {width}x{height}, flags=0x{flags:X8}");
                Width = width;
                Height = height;
            }
            finally { pool.Return(sessionBuf); }

            // 4. 
            VideoStreamDecoder.Initialize(Width, Height);

            OnLoadSizeEvent?.Invoke(Width, Height);
        }

        // Phương thức phụ trợ (giữ nguyên logic đọc)
        private int ReadExact(Stream stream, byte[] buffer, int offset, int count)
        {
            int total = 0;
            while (total < count)
            {
                int r = stream.Read(buffer, offset + total, count - total);
                if (r == 0) break;
                total += r;
            }
            return total;
        }


        private string GetCodecName(int id)
        {
            return id switch
            {
                0x68323634 => "H.264",
                0x68323635 => "H.265",
                _ => $"0x{id:X8}"
            };
        }

        /// <summary>
        /// Khởi động các luồng xử lý nền cho buffer, video và controller
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void StartThreads()
        {
            if (_cts.IsCancellationRequested) return;


            // Khởi task trực tiếp, không dùng ContinueWith để tránh giữ reference
            _bufferTask = Task.Run(() => BufferMain(_cts), _cts);
            _videoTask = Task.Run(() => VideoMain(_cts), _cts);
            _controllerTask = Task.Run(() => ControllerMain(_cts), _cts);
        }

        public async Task SendControlCommand(IControlMessage msg)
        {
            if (_controlClient == null || !_controlClient.Connected || _cts.IsCancellationRequested)
            {
                //log.Warning("SendControlCommand called but controlClient is null or not connected for device {DeviceId}", _device.Serial);
                return;
            }

            await _controlChannel.Writer.WriteAsync(msg);

        }
        /// <summary>
        /// Task chính để xử lý buffer, đọc các packet từ kênh và giải mã chúng thành khung hình video
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task BufferMain(CancellationToken cancellationToken)
        {

            await foreach (var nativePkt in _bufferChannel.Reader.ReadAllAsync(cancellationToken))
            {
                VideoStreamDecoder.DecodePacket(nativePkt);
            }
        }

        private static readonly Regex RectRegex = new(@"Rect\(\s*\d+\s*,\s*\d+\s*-\s*(\d+)\s*,\s*(\d+)\s*\)", RegexOptions.Compiled);
        private static readonly Regex CurRegex = new(@"cur=(\d+)x(\d+)", RegexOptions.Compiled);
        public async Task<(int w, int h)> GetPhysicalSizeAsync(DeviceData device, IAdbClient adb, CancellationToken token = default)
        {
            // 1. ưu tiên mCurrentDisplayRect – Samsung/Xiaomi/Oppo đều có
            var out1 = await ShellAsync(device.Serial, device, adb, "dumpsys display | grep mCurrentDisplayRect", token);
            // ví dụ: mCurrentDisplayRect=Rect(0, 0 - 1080, 2400)
            var m = RectRegex.Match(out1);
            if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));

            // 2. fallback AOSP cũ
            var out2 = await ShellAsync(device.Serial, device, adb, "dumpsys window displays | grep cur=", token);
            m = CurRegex.Match(out2);
            if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));


            var out3 = await ShellAsync(device.Serial, device, adb, "dumpsys display | grep mCurrentLayerStackRect", token);
            m = CurRegex.Match(out3);
            if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));

            var out4 = await ShellAsync(device.Serial, device, adb, "dumpsys display | grep physicalFrame", token);
            m = CurRegex.Match(out4);
            if (m.Success) return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));

            return (0, 0);
        }
        public async Task<string> ShellAsync(string DeviceID, DeviceData Device, IAdbClient _adb, string command, CancellationToken ct = default)
        {
            var receiver = new ConsoleOutputReceiver();
            try
            {
                await _adb.ExecuteRemoteCommandAsync(command, Device, receiver, ct);
                return receiver.ToString().Trim();
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"[{DeviceID}] shell timeout: {command}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DeviceID}] shell error: {ex.Message}");
                return string.Empty;
            }
        }
        // Hàm tiện ích để đọc đúng số byte cần thiết từ stream
        /// <summary>
        /// Đọc đúng số byte cần thiết từ stream bất đồng bộ
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<int> ReadExactAsync(Stream stream, byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                if (_cts.IsCancellationRequested)
                    break;
                int bytesRead = await stream.ReadAsync(buffer, offset + totalRead, count - totalRead, cancellationToken);
                if (bytesRead == 0)
                {
                    // Hết dữ liệu hoặc sự cố xảy ra
                    break;
                }
                totalRead += bytesRead;
            }
            return totalRead;
        }
        public event Action? OnScrcpyVideoSizeChanged;
        private async Task VideoMain(CancellationToken cancellationToken)
        {
            if (_videoClient == null || !_videoClient.Connected) return;

            const ulong FLAG_CONFIG = 0x8000000000000000UL; // 1<<63
            const ulong FLAG_KEYFRAME = 0x4000000000000000UL; // 1<<62
            const ulong PTS_MASK = 0x3FFFFFFFFFFFFFFFUL;

            try
            {
                var s = _videoClient.GetStream();
                int n = 0;
                while (!cancellationToken.IsCancellationRequested)
                {
                    var h = pool.Rent(12);
                    if (await ReadExactAsync(s, h, 0, 12, cancellationToken) != 12) { pool.Return(h); break; }
                    ulong f = BinaryPrimitives.ReadUInt64BigEndian(h.AsSpan(0, 8));
                    int sz = BinaryPrimitives.ReadInt32BigEndian(h.AsSpan(8, 4));
                    pool.Return(h);

                    bool isCfg = (f & FLAG_CONFIG) != 0;
                    bool isKey = (f & FLAG_KEYFRAME) != 0;
                    long pts = (long)(f & PTS_MASK);
                    n++;

                    //Debug.WriteLine($"[VideoMain] #{n} f=0x{f:X16} sz={sz} cfg={isCfg} key={isKey}");

                    if (sz <= 0 || sz > MaxPacketSize)
                    {
                        Debug.WriteLine($"[VideoMain] Bad size {sz}");
                        var doc = (Width < Height);
                        var ngang = (Width > Height);
                        var sizeMax = await GetPhysicalSizeAsync(_device, new AdbClient(), _cts);
                        bool IsOk = doc && (sizeMax.h == PhysicalHeight) || ngang && (sizeMax.h == PhysicalWidth);
                        if (!IsOk)
                        {
                            OnScrcpyVideoSizeChanged?.Invoke();
                        }

                        return;
                    }

                    var buf = pool.Rent(sz);
                    if (await ReadExactAsync(s, buf, 0, sz, cancellationToken) != sz) { pool.Return(buf); break; }

                    if (isCfg)
                    {
                        _pendingConfig = new byte[sz];
                        Buffer.BlockCopy(buf, 0, _pendingConfig, 0, sz);
                        Debug.WriteLine($"[VideoMain] CONFIG saved {sz}");

                        pool.Return(buf);
                        continue;
                    }

                    byte[] data;
                    if (_pendingConfig != null) // <--- KHÔNG check isKey nữa
                    {
                        data = new byte[_pendingConfig.Length + sz];
                        Buffer.BlockCopy(_pendingConfig, 0, data, 0, _pendingConfig.Length);
                        Buffer.BlockCopy(buf, 0, data, _pendingConfig.Length, sz);
                        //Debug.WriteLine($"[VideoMain] MERGE cfg{_pendingConfig.Length}+pkt{sz}={data.Length}");
                        _pendingConfig = null;
                    }
                    else
                    {
                        data = new byte[sz];
                        Buffer.BlockCopy(buf, 0, data, 0, sz);
                    }
                    pool.Return(buf);

                    foreach (var pkt in VideoStreamDecoder.Decode(data, data.Length, pts))
                        _bufferChannel.Writer.TryWrite(pkt);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[VideoMain] {ex}");
            }
        }

        /// <summary>
        /// Task chính để xử lý điều khiển, đọc các lệnh từ kênh và gửi chúng qua kết nối điều khiển
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private async Task ControllerMain(CancellationToken cancellationToken)
        {
            //log.Information("ControllerMain started for device {DeviceId}", _device.Serial);
            if (_controlClient == null || !_controlClient.Connected || _cts.IsCancellationRequested)
            {
                return;
            }

            var stream = _controlClient.GetStream();
            await foreach (var cmd in _controlChannel.Reader.ReadAllAsync(cancellationToken))
            {
                //log.Debug("Sending control message {Type} for device {DeviceId}", cmd.Type, _device.Serial);
                byte[] bytes = cmd.GetBytes();
                await stream.WriteAsync(bytes, cancellationToken);
            }
        }

        /// <summary>
        /// Dùng riêng cho _useReverse == True. Để dọn dẹp dữ liệu cũ trên thiết bị và upload lại scrcpy-server.jar, sau đó tạo SCID mới
        /// </summary>
        public async Task CleanUpDeviceData_UploadScrcpyServer()
        {
            if (_cts.IsCancellationRequested) return;

            await MobileServerCleanup();
            await UploadMobileServer();

        }

        /// <summary>
        /// Dùng riêng cho _useReverse == True. Thiết lập reverse port forwarding từ thiết bị về host
        /// </summary>
        /// <returns></returns>
        private async Task<bool> MobileServerSetupAsync()
        {
            try
            {
                if (_cts.IsCancellationRequested) return false;

                await CleanUpDeviceData_UploadScrcpyServer();
                await _adb.CreateReverseForwardAsync(_device, $"localabstract:scrcpy_{SCID}", $"tcp:{Port}", true);
                //log.Information("Reverse forward set up for device {DeviceId} on port {Port}", _device.Serial, Port);
                return true;
            }
            catch (Exception)
            {
                //log.Warning("Reverse forward failed for device {DeviceId}: {Message}", _device.Serial, ex.Message);
                _useReverse = false;
                return false;
            }
        }

        /// <summary>
        /// Đây là hàm quan trọng nhất để khởi động scrcpy-server trên thiết bị Android
        /// trong đó : 
        /// - Nếu _useReverse = false → thêm tham số "tunnel_forward=true" để dùng forward port
        /// ServerVersion là phiên bản scrcpy-server
        /// SCID là ID kết nối duy nhất
        /// Bitrate là bitrate video
        /// Fps là khung hình trên giây
        /// MaxSize là kích thước tối đa của chiều dài hoặc chiều rộng video
        /// </summary>
        private void MobileServerStart()
        {
            //Task task = new Task(async () =>
            //{
            const string ServerVersion = "4.0";
            //log.Information("Starting scrcpy server for device {DeviceId}", _device.Serial);
            ReceiverServer = new ConsoleOutputReceiver();

            var cmds = new List<string>
                {
                    "CLASSPATH=/data/local/tmp/scrcpy-server.jar",
                    "app_process",
                    "/",
                    "com.genymobile.scrcpy.Server",
                    ServerVersion,
                    $"scid={SCID}",
                    "log_level=info",
                    //"video=true",
                    "audio=false",
                    //"video_codec=h264",
                    $"video_bit_rate={Bitrate}",
                    $"max_fps={Fps}",
                    $"max_size={MaxSize}",
                    //"video_encoder=OMX.Exynos.HEVC.Encoder",
                    "control=true",
                    "display_id=0",
                    "show_touches=false",
                    "stay_awake=false",
                    "power_off_on_close=false",
                    "downsize_on_error=true",
                    "cleanup=true",
                    "power_on=true",
                     "send_dummy_byte=true",
                    "clipboard_autosync=true"
                    //$"capture_orientation=0" // lock chieu nao
                    // Không thêm lock_video_orientation để giữ Unlocked
                    // Nếu muốn rõ ràng, thêm: "lock_video_orientation=-1"
                };

            if (!_useReverse)
                cmds.Add("tunnel_forward=true");

            string command = string.Join(" ", cmds);

            // Muốn test debug thay đổi thành await đọc receiver, nếu đơ mà không thấy thì server khởi động

            if (_cts.IsCancellationRequested == false)
                _ = _adb.ExecuteRemoteCommandAsync(command, _device, ReceiverServer, _cts);
        }

        /// <summary>
        /// Dọn dẹp các forward và reverse port trên thiết bị Android
        /// </summary>
        private async Task MobileServerCleanup()
        {
            try
            {
                await _adb.ExecuteRemoteCommandAsync("pkill -f scrcpy", _device, new ConsoleOutputReceiver(), _cts);
                await AdbService.RemoveScrcpyForwardsAsync(_device, _adb, _cts);
                await AdbService.RemoveScrcpyReversesAsync(_device, _adb, _cts);
                //log.Information("Cleaned up ADB forwards for device {DeviceId}", _device.Serial);
            }
            catch (Exception)
            {
                //log.Error("Failed to clean up ADB forwards for device {DeviceId}: {Message}", _device.Serial, ex.Message);
            }
        }
        /// <summary>
        /// Đẩy tệp scrcpy-server.jar lên thiết bị Android
        /// </summary>
        /// <exception cref="Exception"></exception>
        private async Task UploadMobileServer()
        {
            //log.Information("Checking device {DeviceId} before uploading scrcpy-server.jar", _device.Serial);
            try
            {
                //log.Information("Uploading scrcpy-server.jar for device {DeviceId}", _device.Serial);
                using var service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), _device);
                using var stream = File.OpenRead(ScrcpyServerFile);
                await service.PushAsync(stream, "/data/local/tmp/scrcpy-server.jar", UnixFileStatus.UserRead | UnixFileStatus.GroupRead | UnixFileStatus.OtherRead, DateTime.Now, null);
            }
            catch (Exception ex)
            {
                //log.Error("Failed to upload scrcpy-server.jar for device {DeviceId}: {Message}", _device.Serial, ex.Message);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Đóng các kết nối TcpClient hiện tại trên cổng cụ thể
        /// </summary>
        /// <param name="port"></param>
        public void CloseTcpClientsForPort(int port)
        {
            if (_videoClient != null)
            {
                if (_videoClient.Client?.LocalEndPoint is IPEndPoint ep && ep.Port == port)
                {
                    //log.Information("Closing video client on port {Port} for device {DeviceId}", port, _device.Serial);
                    _videoClient.Close();
                    _videoClient = null;
                }
            }

            if (_controlClient != null)
            {
                if (_controlClient.Client?.LocalEndPoint is IPEndPoint ep && ep.Port == port)
                {
                    //log.Information("Closing control client on port {Port} for device {DeviceId}", port, _device.Serial);
                    _controlClient.Close();
                    _controlClient = null;
                }
            }
        }


        /// <summary>
        /// Dọn dẹp kết nối TCP và các tài nguyên liên quan
        /// </summary>
        private void CleanupConnection()
        {
            _listener?.Stop();
            _videoClient?.Close();
            _controlClient?.Close();
            _listener = null;
            _videoClient = null;
            _controlClient = null;

        }



        /// <summary>
        /// Dừng kết nối và các task liên quan
        /// </summary>
        /// <returns></returns>
        public async Task StopAsync()
        {
            if (!Connected) return;
            Connected = false;


            try { await MobileServerCleanup(); } catch { }

            CleanupConnection();

            try { VideoStreamDecoder.Dispose(); } catch { }
        }

        /// <summary>
        /// Hủy và dọn dẹp tài nguyên
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _ = StopAsync();
            _disposed = true;
        }

        public async Task<string?> ReceiveClipboardDataAsync(CancellationToken ct = default)
        {
            if (_cts.IsCancellationRequested) return null;
            if (_controlClient?.Connected != true) return null;
            var stream = _controlClient.GetStream();
            var type = new byte[1];
            if (await ReadExactAsync(stream, type, 0, 1, ct) != 1) return null;
            if (type[0] != 0) return null; // TYPE_CLIPBOARD

            var lenBuf = new byte[4];
            if (await ReadExactAsync(stream, lenBuf, 0, 4, ct) != 4) return null;
            if (BitConverter.IsLittleEndian) Array.Reverse(lenBuf);
            int len = BitConverter.ToInt32(lenBuf, 0);
            if (len < 0 || len > 262144) return null; // 256KB max

            var data = new byte[len];
            if (await ReadExactAsync(stream, data, 0, len, ct) != len) return null;
            return Encoding.UTF8.GetString(data);
        }

        public async Task<long?> ReceiveAckClipboardAsync(CancellationToken ct = default)
        {
            if (_controlClient?.Connected != true) return null;
            var stream = _controlClient.GetStream();
            var type = new byte[1];
            if (await ReadExactAsync(stream, type, 0, 1, ct) != 1) return null;
            if (type[0] != 1) return null; // TYPE_ACK_CLIPBOARD

            var seqBuf = new byte[8];
            if (await ReadExactAsync(stream, seqBuf, 0, 8, ct) != 8) return null;
            if (BitConverter.IsLittleEndian) Array.Reverse(seqBuf);
            return BitConverter.ToInt64(seqBuf, 0);
        }

        public async Task<(int id, byte[] data)?> ReceiveUhidOutputAsync(CancellationToken ct = default)
        {
            if (_cts.IsCancellationRequested) return null;
            if (_controlClient?.Connected != true) return null;
            var stream = _controlClient.GetStream();
            var type = new byte[1];
            if (await ReadExactAsync(stream, type, 0, 1, ct) != 1) return null;
            if (type[0] != 2) return null; // TYPE_UHID_OUTPUT

            var idBuf = new byte[2];
            var lenBuf = new byte[2];
            if (await ReadExactAsync(stream, idBuf, 0, 2, ct) != 2) return null;
            if (await ReadExactAsync(stream, lenBuf, 0, 2, ct) != 2) return null;
            if (BitConverter.IsLittleEndian) { Array.Reverse(idBuf); Array.Reverse(lenBuf); }
            ushort id = BitConverter.ToUInt16(idBuf, 0);
            ushort len = BitConverter.ToUInt16(lenBuf, 0);
            if (len > 65535) return null;

            var data = new byte[len];
            if (await ReadExactAsync(stream, data, 0, len, ct) != len) return null;
            return (id, data);
        }

        public Task<string?> GetClipboardDataAsync(CancellationToken ct = default)
        {
            if (_cts.IsCancellationRequested) return Task.FromResult<string?>("");
            return ReceiveClipboardDataAsync(ct);
        }

        public Task<long?> GetClipboardAckAsync(CancellationToken ct = default)
        {
            if (_cts.IsCancellationRequested)
                return Task.FromResult<long?>(0);
            return ReceiveAckClipboardAsync(ct);
        }

        public Task<(int id, byte[] data)?> GetUhidOutputAsync(CancellationToken ct = default)
        {
            if (_cts.IsCancellationRequested) return Task.FromResult<(int id, byte[] data)?>(null);
            return ReceiveUhidOutputAsync(ct);
        }
    }
}