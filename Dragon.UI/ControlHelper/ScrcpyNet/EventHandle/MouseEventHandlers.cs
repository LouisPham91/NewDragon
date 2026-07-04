using Dragon.ControlHelper.ScrcpyNet.Services;
using Dragon.Controller.DeviceControl.ScrcpyNet.AndroidKeyBroad;
using Dragon.Controller.DeviceControl.ScrcpyNet.Services;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using System.Diagnostics;

namespace Dragon.ControlHelper.ScrcpyNet.EventHandle
{
    public sealed class MouseEventHandlers : IDisposable
    {
        public string DeviceId { get; }

        private readonly Control _hostControl;
        private readonly IScrcpyService _scrcpyService;

        // Dùng property công khai thay vì truy cập trường private _videoSize
        private Size VideoSize => _scrcpyService._videoSize;

        private bool _disposed;

        public MouseEventHandlers(Control hostControl, IScrcpyService scrcpyService, string deviceID)
        {
            DeviceId = deviceID;
            _hostControl = hostControl ?? throw new ArgumentNullException(nameof(hostControl));
            _scrcpyService = scrcpyService ?? throw new ArgumentNullException(nameof(scrcpyService));

            _hostControl.MouseDown += OnMouseDown;
            _hostControl.MouseMove += OnMouseMove;
            _hostControl.MouseUp += OnMouseUp;
            _hostControl.MouseWheel += OnMouseWheel;
            _hostControl.MouseEnter += OnMouseEnter;
        }

        // ---------- Core event handlers ----------
        private async void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var pos = GetTouchPosition(e.Location);
            if (pos.X == 1 && pos.Y == 1) return;

            var msg = new TouchEventControlMessage(
                AndroidMotionEventAction.AMOTION_EVENT_ACTION_DOWN,
                -1L, pos, 1.0f, 0, 0);
            await _scrcpyService.SendSyncDongBoCommand(msg);
        }

        private async void OnMouseMove(object? sender, MouseEventArgs e)
        {
            var pos = GetTouchPosition(e.Location);
            if (pos.X == 1 && pos.Y == 1) return;
            var msg = new TouchEventControlMessage(
                AndroidMotionEventAction.AMOTION_EVENT_ACTION_MOVE,
                -1L, pos, 1.0f, 0, 0);
            await _scrcpyService.SendSyncDongBoCommand(msg);
        }

        private async void OnMouseUp(object? sender, MouseEventArgs e)
        {
            var pos = GetTouchPosition(e.Location);
            if (pos.X == 1 && pos.Y == 1) return;
            var msg = new TouchEventControlMessage(
                AndroidMotionEventAction.AMOTION_EVENT_ACTION_UP,
                -1L, pos, 0.0f, 0, 0);
            await _scrcpyService.SendSyncDongBoCommand(msg);
        }

        private async void OnMouseWheel(object? sender, MouseEventArgs e)
        {
            var pos = GetTouchPosition(e.Location);
            if (pos.X == 1 && pos.Y == 1) return;

            int step = e.Delta / 120;
            float scroll = step * 0.4f;
            if (scroll == 0) scroll = step > 0 ? 1f : -1f;

            var msg = new ScrollEventControlMessage(pos, 0f, scroll, 0);
            await _scrcpyService.SendSyncDongBoCommand(msg);
        }

        private async void OnMouseEnter(object? sender, EventArgs e)
        {
            var msg = new BackOrScreenOnControlMessage(AndroidKeyEventAction.AKEY_EVENT_ACTION_DOWN);
            await _scrcpyService.SendSyncDongBoCommand(msg);
        }

        // ---------- Tính vị trí chạm (đã sửa để tính offset letterbox) ----------
        private Position GetTouchPosition(Point mousePt)
        {
            var videoSize = VideoSize; // Property lấy kích thước video từ service
            if (videoSize.Width <= 0 || videoSize.Height <= 0)
                return new Position(1, 1, 0, 0);

            int controlW = _hostControl.ClientSize.Width;
            int controlH = _hostControl.ClientSize.Height;

            // Tỉ lệ để giữ nguyên tỉ lệ video trong control
            float scaleX = (float)controlW / videoSize.Width;
            float scaleY = (float)controlH / videoSize.Height;
            float scale = Math.Min(scaleX, scaleY); // chọn tỉ lệ nhỏ hơn để vừa khít

            // Kích thước video thực tế hiển thị
            int displayWidth = (int)(videoSize.Width * scale);
            int displayHeight = (int)(videoSize.Height * scale);

            // Tính offset do căn giữa (letterbox)
            int offsetX = (controlW - displayWidth) / 2;
            int offsetY = (controlH - displayHeight) / 2;

            // Tọa độ chuột sau khi trừ viền
            int xIn = mousePt.X - offsetX;
            int yIn = mousePt.Y - offsetY;

            // Kiểm tra nếu chuột nằm ngoài vùng video
            if (xIn < 0 || yIn < 0 || xIn > displayWidth || yIn > displayHeight)
                return new Position(1, 1, 0, 0); // vị trí không hợp lệ

            // Map ngược về tọa độ gốc của video
            int rawX = (int)(xIn / scale);
            int rawY = (int)(yIn / scale);

            return new Position(rawX, rawY, (ushort)videoSize.Width, (ushort)videoSize.Height);
        }

        // ---------- Các thao tác tự động ----------
        public async Task<NodeResult> Click(DLoopContext ctx, ClickArg click)
        {
            var token = ctx.Token;
            var logKey = ctx.LogKey;
            if (token.IsCancellationRequested) return NodeResult.Stop();

            var size = VideoSize;
            int x = (int)(size.Width * (click.x / 100.0));
            int y = (int)(size.Height * (click.y / 100.0));
            var pos = new Position(x, y, (ushort)size.Width, (ushort)size.Height);

            int count = click.ClickMode == ClickMode.DoubleClick ? 2 : 1;
            for (int i = 0; i < count; i++)
            {
                await _scrcpyService.SendControlCommand(new TouchEventControlMessage(AndroidMotionEventAction.AMOTION_EVENT_ACTION_DOWN, -1L, pos, 1f, 0, 0));
                await _scrcpyService.SendControlCommand(new TouchEventControlMessage(AndroidMotionEventAction.AMOTION_EVENT_ACTION_UP, -1L, pos, 0f, 0, 0));
                if (i == 0 && count == 2)
                {
                    if (await Logger.DelayAsync(100, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();
                }
            }
            return NodeResult.Ok();
        }

        // ---------- ĐÃ THÊM LẠI LONG PRESS ----------
        public async Task<NodeResult> LongPress(DLoopContext ctx, LongPressArg press)
        {
            var token = ctx.Token;
            var logKey = ctx.LogKey;
            if (token.IsCancellationRequested) return NodeResult.Stop();

            var size = VideoSize;
            int x = (int)(size.Width * (press.x / 100.0));
            int y = (int)(size.Height * (press.y / 100.0));
            var pos = new Position(x, y, (ushort)size.Width, (ushort)size.Height);

            await _scrcpyService.SendControlCommand(new TouchEventControlMessage(AndroidMotionEventAction.AMOTION_EVENT_ACTION_DOWN, -1L, pos, 1.0f, 0, 0));

            if (await Logger.DelayAsync(press.Duration, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();

            await _scrcpyService.SendControlCommand(new TouchEventControlMessage(AndroidMotionEventAction.AMOTION_EVENT_ACTION_UP, -1L, pos, 0.0f, 0, 0));

            Logger.Notify(logKey, $"Scrcpy LongPress ({x},{y}) {press.Duration}ms", Logger.Icon.Information);
            return NodeResult.Ok();
        }

        // ---------- SWIPE ĐÃ SỬA LẠI CHẾ ĐỘ RANDOM ----------
        public async Task<NodeResult> Swipe(DLoopContext ctx, SwipeArg swipe)
        {
            var token = ctx.Token;
            var logKey = ctx.LogKey;
            var size = VideoSize;

            int loopCount = swipe.loopTime;
            if (swipe.SwipeMode == SwipeMode.Random)
                loopCount = Random.Shared.Next(swipe.randMin, swipe.randMax + 1);

            for (int loop = 0; loop < loopCount; loop++)
            {
                if (token.IsCancellationRequested) return NodeResult.Stop();

                int x1, y1, x2, y2;
                var r = Random.Shared;
                switch (swipe.Direction)
                {
                    case Direction.Down:
                        x1 = x2 = (int)(size.Width * r.Next((int)swipe.Left, (int)swipe.Right + 1) / 100.0);
                        y1 = (int)(size.Height * swipe.Bottom / 100.0);
                        y2 = (int)(size.Height * swipe.Top / 100.0); break;
                    case Direction.Up:
                        x1 = x2 = (int)(size.Width * r.Next((int)swipe.Left, (int)swipe.Right + 1) / 100.0);
                        y1 = (int)(size.Height * swipe.Top / 100.0);
                        y2 = (int)(size.Height * swipe.Bottom / 100.0); break;
                    case Direction.Right:
                        y1 = y2 = (int)(size.Height * r.Next((int)swipe.Top, (int)swipe.Bottom + 1) / 100.0);
                        x1 = (int)(size.Width * swipe.Right / 100.0);
                        x2 = (int)(size.Width * swipe.Left / 100.0); break;
                    case Direction.Left:
                        y1 = y2 = (int)(size.Height * r.Next((int)swipe.Top, (int)swipe.Bottom + 1) / 100.0);
                        x1 = (int)(size.Width * swipe.Left / 100.0);
                        x2 = (int)(size.Width * swipe.Right / 100.0); break;
                    default: return NodeResult.Fail("Invalid direction");
                }

                int distance = (int)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                int steps = Math.Max(10, distance / Math.Max(1, swipe.PixelPerStep));

                var pos1 = new Position(x1, y1, (ushort)size.Width, (ushort)size.Height);
                var pos2 = new Position(x2, y2, (ushort)size.Width, (ushort)size.Height);

                await _scrcpyService.SendControlCommand(new TouchEventControlMessage(AndroidMotionEventAction.AMOTION_EVENT_ACTION_DOWN, -1L, pos1, 1f, 0, 0));

                for (int i = 1; i <= steps; i++)
                {
                    int xi = x1 + (x2 - x1) * i / steps;
                    int yi = y1 + (y2 - y1) * i / steps;
                    var pos = new Position(xi, yi, (ushort)size.Width, (ushort)size.Height);
                    await _scrcpyService.SendControlCommand(new TouchEventControlMessage(AndroidMotionEventAction.AMOTION_EVENT_ACTION_MOVE, -1L, pos, 1f, 0, 0));
                    if (await Logger.DelayAsync(swipe.duration / steps, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();
                }

                await _scrcpyService.SendControlCommand(new TouchEventControlMessage(AndroidMotionEventAction.AMOTION_EVENT_ACTION_UP, -1L, pos2, 0f, 0, 0));
                Logger.Notify(logKey, $"Scrcpy Swipe {swipe.Direction} {loop + 1}/{loopCount}", Logger.Icon.Information);

                if (loop < loopCount - 1)
                    if (await Logger.DelayAsync(200, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();
            }
            return NodeResult.Ok();
        }

        public async Task<NodeResult> DragAndDrop(DLoopContext ctx, DragArg drag)
        {
            var token = ctx.Token;
            var logKey = ctx.LogKey;
            if (token.IsCancellationRequested) return NodeResult.Stop();
            if (drag.Points == null || drag.Points.Count < 2) return NodeResult.Fail("Invalid drag data");

            var size = VideoSize;
            var sw = Stopwatch.StartNew();
            int segCount = drag.Points.Count - 1;
            int durPerSeg = drag.Duration / Math.Max(1, segCount);

            var start = drag.Points[0];
            int sx = (int)(size.Width * start.X / 100.0);
            int sy = (int)(size.Height * start.Y / 100.0);
            var posStart = new Position(sx, sy, (ushort)size.Width, (ushort)size.Height);

            await _scrcpyService.SendControlCommand(new TouchEventControlMessage(AndroidMotionEventAction.AMOTION_EVENT_ACTION_DOWN, -1L, posStart, 1f, 0, 0));
            if (await Logger.DelayAsync(1000, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();

            for (int seg = 0; seg < segCount; seg++)
            {
                var p1 = drag.Points[seg];
                var p2 = drag.Points[seg + 1];
                int steps = 10;
                for (int i = 1; i <= steps; i++)
                {
                    if (token.IsCancellationRequested) return NodeResult.Stop();
                    double xiP = p1.X + (p2.X - p1.X) * i / (double)steps;
                    double yiP = p1.Y + (p2.Y - p1.Y) * i / (double)steps;
                    int xi = (int)(size.Width * xiP / 100.0);
                    int yi = (int)(size.Height * yiP / 100.0);
                    var pos = new Position(xi, yi, (ushort)size.Width, (ushort)size.Height);
                    await _scrcpyService.SendControlCommand(new TouchEventControlMessage(AndroidMotionEventAction.AMOTION_EVENT_ACTION_MOVE, -1L, pos, 1f, 0, 0));
                    if (await Logger.DelayAsync(Math.Max(1, durPerSeg / steps), token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();
                }
            }

            var end = drag.Points.Last();
            int ex = (int)(size.Width * end.X / 100.0);
            int ey = (int)(size.Height * end.Y / 100.0);
            var posEnd = new Position(ex, ey, (ushort)size.Width, (ushort)size.Height);

            if (await Logger.DelayAsync(1000, token, logKey) == ExecutionStatus.Stop) return NodeResult.Stop();
            await _scrcpyService.SendControlCommand(new TouchEventControlMessage(AndroidMotionEventAction.AMOTION_EVENT_ACTION_UP, -1L, posEnd, 0f, 0, 0));

            sw.Stop();
            Logger.Notify(logKey, $"Scrcpy Drag {drag.Points.Count} điểm, {sw.ElapsedMilliseconds}ms", Logger.Icon.Information);
            return NodeResult.Ok();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _hostControl.MouseDown -= OnMouseDown;
            _hostControl.MouseMove -= OnMouseMove;
            _hostControl.MouseUp -= OnMouseUp;
            _hostControl.MouseWheel -= OnMouseWheel;
            _hostControl.MouseEnter -= OnMouseEnter;
        }
    }
}