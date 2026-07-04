
using Dragon.ControlHelper.ScrcpyNet.Services;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl.ScrcpyNet.Services;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;


namespace Dragon.ControlHelper.ScrcpyNet.EventHandle
{

    public sealed class MouseFakeUhidHandlers : IDisposable
    {
        private readonly Control _hostControl;
        private readonly IScrcpyService _scrcpyService;
        private readonly string _deviceID;
        private readonly ushort _uhidId = 2;
        private bool _disposed;
        private Point _currentMousePosition;
        private const int SC_HID_MOUSE_INPUT_SIZE = 4;
        private const int MAX_DELTA = 127;
        private int physicalWith = 0;
        private int physicalHeight = 0;
        public MouseFakeUhidHandlers(Control hostControl, IScrcpyService scrcpyService, string deviceID)
        {
            _hostControl = hostControl ?? throw new ArgumentNullException(nameof(hostControl));
            _scrcpyService = scrcpyService ?? throw new ArgumentNullException(nameof(scrcpyService));
            _deviceID = deviceID;
            _currentMousePosition = Point.Empty;
            var phone = PhoneRepository.FindOneByDeviceID(deviceID);
            if (phone == null) return;
            physicalWith = phone.PhysicalWidth;
            physicalHeight = phone.PhysicalHeight;
            _hostControl.MouseClick += OnMouseClick;
            _ = StartUhidMouseAsync();
        }

        private async Task StartUhidMouseAsync() => await CreateMouseAsync();

        private async Task CreateMouseAsync()
        {
            try
            {
                byte[] reportDesc = CreateMouseReportDescriptor();
                var msg = new UhidCreateControlMessage(_uhidId, 0, 0, "FakeVirtualMouse", reportDesc);
                await _scrcpyService.SendSyncDongBoCommand(msg);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create UHID mouse", ex);
            }
        }

        private static byte[] CreateMouseReportDescriptor() => new byte[]
        {
            0x05, 0x01, 0x09, 0x02, 0xA1, 0x01, 0x09, 0x01, 0xA1, 0x00,
            0x05, 0x09, 0x19, 0x01, 0x29, 0x05, 0x15, 0x00, 0x25, 0x01,
            0x95, 0x05, 0x75, 0x01, 0x81, 0x02, 0x95, 0x01, 0x75, 0x03,
            0x81, 0x01, 0x05, 0x01, 0x09, 0x30, 0x09, 0x31, 0x09, 0x38,
            0x15, 0x81, 0x25, 0x7F, 0x75, 0x08, 0x95, 0x03, 0x81, 0x06,
            0xC0, 0xC0
        };

        // ----- Di chuyển về (0,0) -----
        public async Task MoveToOriginAsync(int delayMs = 50)
        {
            var size = _scrcpyService._videoSize;
            int steps = 35;
            byte[] moveData = new byte[SC_HID_MOUSE_INPUT_SIZE];
            moveData[1] = unchecked((byte)-MAX_DELTA);
            moveData[2] = unchecked((byte)-MAX_DELTA);
            for (int i = 0; i < steps; i++)
            {
                await _scrcpyService.SendSyncDongBoCommand(new UhidInputControlMessage(_uhidId, moveData));
                await Task.Delay(delayMs);
            }
            _currentMousePosition = Point.Empty;
        }

        public async Task<ExecutionStatus> MoveToOriginAsync(int delayMs, CancellationToken token, string deviceId)
        {
            try
            {
                var size = _scrcpyService._videoSize;
                int steps = 35;
                byte[] moveData = new byte[SC_HID_MOUSE_INPUT_SIZE];
                moveData[1] = unchecked((byte)-MAX_DELTA);
                moveData[2] = unchecked((byte)-MAX_DELTA);
                for (int i = 0; i < steps; i++)
                {
                    await _scrcpyService.SendSyncDongBoCommand(new UhidInputControlMessage(_uhidId, moveData));
                    if (await Logger.DelayAsync(delayMs, token, deviceId) == ExecutionStatus.Stop)
                        return ExecutionStatus.Stop;
                }
                _currentMousePosition = Point.Empty;
                return ExecutionStatus.Continue;
            }
            catch { return ExecutionStatus.Stop; }
        }

        // ----- Click vào panel sẽ chuyển sang chuột thật -----
        private async void OnMouseClick(object? sender, MouseEventArgs e)
        {
            // Hủy fake mouse, rồi yêu cầu InputModeManager chuyển sang real mouse
            await DisposeAllAsync();
            await _scrcpyService.RequetRealMouseFakeUHDI();
        }

        // ----- Di chuyển đến tọa độ tuyệt đối trên màn hình vật lý -----
        public async Task MoveToAsync(int deviceX, int deviceY, int delayMs = 100)
        {
            var size = _scrcpyService._videoSize;
            deviceX = Math.Clamp(deviceX, 0, size.Width);
            deviceY = Math.Clamp(deviceY, 0, size.Height);
            int dxTotal = deviceX - _currentMousePosition.X;
            int dyTotal = deviceY - _currentMousePosition.Y;
            while (dxTotal != 0 || dyTotal != 0)
            {
                int dx = Math.Clamp(dxTotal, -MAX_DELTA, MAX_DELTA);
                int dy = Math.Clamp(dyTotal, -MAX_DELTA, MAX_DELTA);
                byte[] data = new byte[SC_HID_MOUSE_INPUT_SIZE];
                data[1] = unchecked((byte)dx);
                data[2] = unchecked((byte)dy);
                await _scrcpyService.SendSyncDongBoCommand(new UhidInputControlMessage(_uhidId, data));
                _currentMousePosition.X = Math.Clamp(_currentMousePosition.X + dx, 0, size.Width);
                _currentMousePosition.Y = Math.Clamp(_currentMousePosition.Y + dy, 0, size.Height);
                dxTotal = deviceX - _currentMousePosition.X;
                dyTotal = deviceY - _currentMousePosition.Y;
                await Task.Delay(delayMs);
            }
        }

        public async Task ClickAtAsync(int x, int y)
        {
            await MoveToAsync(x, y);
            byte[] down = new byte[SC_HID_MOUSE_INPUT_SIZE]; down[0] = 0x01;
            byte[] up = new byte[SC_HID_MOUSE_INPUT_SIZE];
            await _scrcpyService.SendSyncDongBoCommand(new UhidInputControlMessage(_uhidId, down));
            await Task.Delay(50);
            await _scrcpyService.SendSyncDongBoCommand(new UhidInputControlMessage(_uhidId, up));
        }


        // ----- Dọn dẹp -----
        private async Task DisposeAllAsync()
        {
            if (_disposed) return;
            _disposed = true;
            _hostControl.MouseClick -= OnMouseClick;
            try
            {
                await SendControlMessageAsync(new UhidDestroyControlMessage(_uhidId));
            }
            catch { }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _ = DisposeAllAsync(); // fire-and-forget trong Dispose đồng bộ, đủ an toàn
            GC.SuppressFinalize(this);
        }



        private async Task SendControlMessageAsync(IControlMessage msg)
        {
            await _scrcpyService.SendControlCommand(msg);
        }

        public async Task<NodeResult> ClickAtAsync(int x, int y, int MiniSecond, CancellationToken token, string deviceId)
        {
            if (token.IsCancellationRequested)
                return NodeResult.Stop();

            int deviceX = Math.Clamp(x, 0, physicalWith);
            int deviceY = Math.Clamp(y, 0, physicalHeight);

            var kq = await MoveToAsync(deviceX, deviceY, MiniSecond, token, deviceId);
            if (kq.Status != ExecutionStatus.Continue)
                return kq;

            byte[] downData = new byte[SC_HID_MOUSE_INPUT_SIZE];
            downData[0] = 0x01;
            downData[1] = 0;
            downData[2] = 0;
            downData[3] = 0;

            var downMsg = new UhidInputControlMessage(_uhidId, downData);
            await SendControlMessageAsync(downMsg);

            await Task.Delay(200);

            byte[] upData = new byte[SC_HID_MOUSE_INPUT_SIZE];
            upData[0] = 0x00;
            upData[1] = 0;
            upData[2] = 0;
            upData[3] = 0;

            var upMsg = new UhidInputControlMessage(_uhidId, upData);
            await SendControlMessageAsync(upMsg);

            return NodeResult.Ok();
        }

        public async Task<NodeResult> MoveToAsync(int deviceX, int deviceY, int DelayMiniSecond, CancellationToken token, string deviceId, byte buttonState = 0x00, int Delta = -1)
        {
            return await MoveInternalAsync(deviceX, deviceY, DelayMiniSecond, token, deviceId, buttonState, Delta);
        }

        public async Task<NodeResult> MoveWithButtonAsync(int deviceX, int deviceY, int DelayMiniSecond, CancellationToken token, string deviceId, byte buttonState = 0x01, int Delta = -1)
        {
            return await MoveInternalAsync(deviceX, deviceY, DelayMiniSecond, token, deviceId, buttonState, Delta);
        }

        private int CalculateStepCount(int startX, int startY, int endX, int endY, int Delta = -1)
        {
            int dxTotal = endX - startX;
            int dyTotal = endY - startY;

            int maxDelta = Delta != -1 ? Delta : MAX_DELTA;

            int stepX = (int)Math.Ceiling(Math.Abs(dxTotal) / (double)maxDelta);
            int stepY = (int)Math.Ceiling(Math.Abs(dyTotal) / (double)maxDelta);

            return Math.Max(stepX, stepY);
        }

        private async Task<NodeResult> MoveInternalAsync(int deviceX, int deviceY, int DelayMiniSecond, CancellationToken token, string deviceId, byte buttonState, int Delta = -1)
        {
            if (token.IsCancellationRequested)
                return NodeResult.Stop();

            deviceX = Math.Clamp(deviceX, 0, physicalWith);
            deviceY = Math.Clamp(deviceY, 0, physicalHeight);

            int dxTotal = deviceX - _currentMousePosition.X;
            int dyTotal = deviceY - _currentMousePosition.Y;

            int stepCount = CalculateStepCount(_currentMousePosition.X, _currentMousePosition.Y, deviceX, deviceY, Delta);
            int maxDelta = Delta != -1 ? Delta : MAX_DELTA;

            for (int i = 0; i < stepCount; i++)
            {
                int dx = Math.Clamp(dxTotal, -maxDelta, maxDelta);
                int dy = Math.Clamp(dyTotal, -maxDelta, maxDelta);

                byte[] moveData = new byte[SC_HID_MOUSE_INPUT_SIZE];
                moveData[0] = buttonState;
                moveData[1] = (byte)(sbyte)dx;
                moveData[2] = (byte)(sbyte)dy;
                moveData[3] = 0;

                await SendControlMessageAsync(new UhidInputControlMessage(_uhidId, moveData));

                _currentMousePosition.X = Math.Clamp(_currentMousePosition.X + dx, 0, physicalWith);
                _currentMousePosition.Y = Math.Clamp(_currentMousePosition.Y + dy, 0, physicalHeight);

                dxTotal = deviceX - _currentMousePosition.X;
                dyTotal = deviceY - _currentMousePosition.Y;

                var kq = await Logger.DelayAsync(DelayMiniSecond, token, deviceId);
                if (kq == ExecutionStatus.Stop)
                    return NodeResult.Stop();
            }

            return NodeResult.Ok();
        }

        public async Task<NodeResult> Click(DLoopContext ctx, ClickArg click)
        {
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (token.IsCancellationRequested) return NodeResult.Stop();

            int deviceX = (int)(physicalWith * (click.x / 100.0));
            int deviceY = (int)(physicalHeight * (click.y / 100.0));
            int clickCount = click.ClickMode == ClickMode.DoubleClick ? 2 : 1;

            for (int i = 0; i < clickCount; i++)
            {
                var r = await ClickAtAsync(deviceX, deviceY, 50, token, logKey);
                if (r.Status != ExecutionStatus.Continue) return r;

                if (click.ClickMode == ClickMode.DoubleClick && i == 0)
                {
                    if (await Logger.DelayAsync(50, token, logKey) == ExecutionStatus.Stop)
                        return NodeResult.Stop();
                }
            }

            Logger.Notify(logKey, $"UHID {(click.ClickMode == ClickMode.DoubleClick ? "DoubleClick" : "Click")} ({deviceX},{deviceY})", Logger.Icon.Information);
            return NodeResult.Ok();
        }

        public async Task<NodeResult> LongPress(DLoopContext ctx, LongPressArg press)
        {
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (token.IsCancellationRequested) return NodeResult.Stop();

            int x = (int)(physicalWith * (press.x / 100.0));
            int y = (int)(physicalHeight * (press.y / 100.0));

            var m = await MoveToAsync(x, y, 50, token, logKey);
            if (m.Status != ExecutionStatus.Continue) return m;

            await SendControlMessageAsync(new UhidInputControlMessage(_uhidId, new byte[4] { 0x01, 0, 0, 0 }));

            if (await Logger.DelayAsync(press.Duration, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();

            await SendControlMessageAsync(new UhidInputControlMessage(_uhidId, new byte[4] { 0x00, 0, 0, 0 }));

            Logger.Notify(logKey, $"UHID LongPress ({x},{y}) {press.Duration}ms", Logger.Icon.Information);
            return NodeResult.Ok();
        }

        public async Task<NodeResult> Swipe(DLoopContext ctx, SwipeArg swipe)
        {
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (token.IsCancellationRequested) return NodeResult.Stop();

            int loopCount = swipe.loopTime;
            if (swipe.SwipeMode == SwipeMode.Random)
                loopCount = Random.Shared.Next(swipe.randMin, swipe.randMax + 1);

            for (int loop = 0; loop < loopCount; loop++)
            {
                if (token.IsCancellationRequested) return NodeResult.Stop();

                int x1, y1, x2, y2;
                bool isMoveOrigin = false;
                var r = Random.Shared;

                switch (swipe.Direction)
                {
                    case Direction.Up:
                        x1 = x2 = (int)(physicalWith * r.Next((int)swipe.Left, (int)swipe.Right + 1) / 100.0);
                        y1 = (int)(physicalHeight * swipe.Top / 100.0);
                        y2 = (int)(physicalHeight * swipe.Bottom / 100.0); break;
                    case Direction.Down:
                        x1 = x2 = (int)(physicalWith * r.Next((int)swipe.Left, (int)swipe.Right + 1) / 100.0);
                        y1 = (int)(physicalHeight * swipe.Bottom / 100.0);
                        y2 = (int)(physicalHeight * swipe.Top / 100.0); break;
                    case Direction.Left:
                        y1 = y2 = (int)(physicalHeight * r.Next((int)swipe.Top, (int)swipe.Bottom + 1) / 100.0);
                        x1 = (int)(physicalWith * swipe.Left / 100.0);
                        x2 = (int)(physicalWith * swipe.Right / 100.0);
                        isMoveOrigin = true; break;
                    case Direction.Right:
                        y1 = y2 = (int)(physicalHeight * r.Next((int)swipe.Top, (int)swipe.Bottom + 1) / 100.0);
                        x1 = (int)(physicalWith * swipe.Right / 100.0);
                        x2 = (int)(physicalWith * swipe.Left / 100.0);
                        isMoveOrigin = true; break;
                    default: return NodeResult.Fail("Invalid direction");
                }

                var m1 = await MoveToAsync(x1, y1, swipe.UHDIMoveDelay, token, logKey);
                if (m1.Status != ExecutionStatus.Continue) return m1;
                if (await Logger.DelayAsync(200, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();

                await SendControlMessageAsync(new UhidInputControlMessage(_uhidId, new byte[4] { 0x01, 0, 0, 0 }));

                var m2 = await MoveWithButtonAsync(x2, y2, swipe.DelayStep, token, logKey, 0x01, swipe.PixelPerStep);
                if (m2.Status != ExecutionStatus.Continue) return m2;

                await SendControlMessageAsync(new UhidInputControlMessage(_uhidId, new byte[4] { 0x00, 0, 0, 0 }));

                if (await Logger.DelayAsync(200, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();

                if (isMoveOrigin) await MoveToOriginAsync(5);

                if (loop < loopCount - 1)
                    if (await Logger.DelayAsync(200, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();

                Logger.Notify(logKey, $"UHID Swipe {swipe.Direction} {loop + 1}/{loopCount}", Logger.Icon.Information);
            }
            return NodeResult.Ok();
        }

        public async Task<NodeResult> DragAndDrop(DLoopContext ctx, DragArg drag)
        {
            var token = ctx.Token;
            var logKey = ctx.LogKey;

            if (token.IsCancellationRequested) return NodeResult.Stop();
            if (drag.Points == null || drag.Points.Count < 2) return NodeResult.Fail("Invalid drag data");

            var realPoints = drag.Points.Select(p => new Point(
                (int)(physicalWith * p.X / 100.0),
                (int)(physicalHeight * p.Y / 100.0))).ToList();

            var start = realPoints[0];
            var s = await MoveToAsync(start.X, start.Y, 50, token, logKey);
            if (s.Status != ExecutionStatus.Continue) return s;
            if (await Logger.DelayAsync(250, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();

            await SendControlMessageAsync(new UhidInputControlMessage(_uhidId, new byte[4] { 0x01, 0, 0, 0 }));
            if (await Logger.DelayAsync(500, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();

            for (int i = 1; i < realPoints.Count; i++)
            {
                if (token.IsCancellationRequested) return NodeResult.Stop();
                var p = realPoints[i];
                var m = await MoveWithButtonAsync(p.X, p.Y, drag.DelayPerStep, token, logKey, 0x01, drag.PixelsPerStep);
                if (m.Status != ExecutionStatus.Continue) return m;
            }

            if (await Logger.DelayAsync(500, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();
            await SendControlMessageAsync(new UhidInputControlMessage(_uhidId, new byte[4] { 0x00, 0, 0, 0 }));

            Logger.Notify(logKey, $"UHID Drag {drag.Points.Count} điểm", Logger.Icon.Information);
            return NodeResult.Ok();
        }

    }

}