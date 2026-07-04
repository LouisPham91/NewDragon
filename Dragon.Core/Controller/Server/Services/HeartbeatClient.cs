using Dragon.Controller.GlobalControl.Security;
using Dragon.Controller.Server.Model;
using System.Diagnostics;
using System.Text.Json;

namespace Dragon.Controller.Server.Services
{
    public sealed class HeartbeatClient
    {
        private static readonly Lazy<HeartbeatClient> _instance = new(() => new HeartbeatClient());
        public static HeartbeatClient Instance => _instance.Value;

        private CancellationTokenSource? _cts;
        private Task? _heartbeatTask;

        private HeartbeatClient() { }

        public void Start(Guid computerId, string token, TimeSpan interval)
        {
            // Hủy heartbeat cũ nếu đang chạy
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var ct = _cts.Token;

            _heartbeatTask = Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        var payload = new EncryptedPayload
                        {
                            Data = await RsaKeyPair.MaHoa(
                                JsonSerializer.Serialize(computerId, JsonServer.Default.Guid))
                        };

                        // Dùng HttpServices thay vì _httpClient
                        var response = await HttpServices.PostJsonAuthAsync(
                            payload,
                            JsonServer.Default.EncryptedPayload,
                            token,
                            endpoint: "/api/computer/heartbeat" // giữ đúng route của bạn
                        );

                        Debug.WriteLine(await response.Content.ReadAsStringAsync(ct));
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Heartbeat error: {ex.Message}");
                    }

                    try
                    {
                        await Task.Delay(interval, ct);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }, ct);
        }

        public void Stop()
        {
            _cts?.Cancel();
        }
    }
}