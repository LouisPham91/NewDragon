using Dragon.Controller.GlobalControl.TaskDeviceManager.Runners;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Model.App;
using Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn;
using Dragon.Controller.TaskDeviceManager.Model.Emoji;
using Dragon.Controller.TaskDeviceManager.Model.File;
using Dragon.Controller.TaskDeviceManager.Model.HttpResponse;
using Dragon.Controller.TaskDeviceManager.Model.Input;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Controller.TaskDeviceManager.Model.Vision;
using Dragon.Controller.TaskDeviceManager.Runners;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure
{
    public class PhoneFarmHost
    {
        // Entry point cho 1 phone
        public async Task<NodeResult> ExecuteAsync(DLoop root, PhoneSession session)
        {
            // token riêng của phone, có thể cancel từ DragonPhoneManager.Stop()
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(session.Cts.Token);
            var ctx = new DLoopContext { Session = session, Token = cts.Token };

            root.Hydrate(); // deserialize ArgsJson 1 lần
            return await ExecuteNodeAsync(root, ctx);
        }

        // chạy 1 node + retry + timeout
        private async Task<NodeResult> ExecuteNodeAsync(DLoop node, DLoopContext ctx)
        {
            if (!node.Enabled) return NodeResult.Ok();

            for (int attempt = 1; attempt <= Math.Max(1, node.RetryCount); attempt++)
            {
                if (ctx.Token.IsCancellationRequested) return NodeResult.Stop();

                // timeout riêng cho node này
                using var timeoutCts = new CancellationTokenSource(node.TimeoutMs);
                using var linked = CancellationTokenSource.CreateLinkedTokenSource(ctx.Token, timeoutCts.Token);

                // tạo context mới chỉ đổi token, giữ nguyên Session và bag
                var runCtx = new DLoopContext
                {
                    Session = ctx.Session,
                    Token = linked.Token
                };
                // copy bag để node con vẫn đọc được kết quả trước
                runCtx.CopyBagFrom(ctx);

                NodeResult result;
                try
                {
                    result = await DispatchAsync(node, runCtx);
                }
                catch (OperationCanceledException)
                {
                    Logger.Notify(ctx.LogKey, $"Node '{node.Name}' timeout", Logger.Icon.Warning);
                    result = NodeResult.Stop();
                }
                catch (Exception ex)
                {
                    Logger.Notify(ctx.LogKey, $"Node lỗi: {ex.Message}", Logger.Icon.Error);
                    result = NodeResult.Fail(ex.Message);
                }

                // lưu kết quả vào bag của phone này
                if (result.Value != null) ctx.Set(node.Id.ToString(), result.Value);

                if (result.Status == ExecutionStatus.Continue)
                {
                    // chạy children nếu có
                    if (node.Children?.Count > 0)
                    {
                        var childResult = await ExecuteChildrenAsync(node, ctx);
                        if (childResult.Status != ExecutionStatus.Continue) return childResult;
                    }
                    return NodeResult.Ok(result.Value);
                }

                if (result.Status == ExecutionStatus.Retry && attempt < node.RetryCount)
                    continue; // thử lại

                // chính sách lỗi
                return node.OnError switch
                {
                    OnErrorMode.Continue => NodeResult.Ok(),
                    OnErrorMode.Skip => new NodeResult { Status = ExecutionStatus.Skip },
                    _ => result
                };
            }
            return NodeResult.Stop();
        }

        private async Task<NodeResult> ExecuteChildrenAsync(DLoop parent, DLoopContext ctx)
        {
            var list = parent.Children.Where(c => c.Enabled).ToList();

            if (parent.FlowMode == FlowMode.RandomOne && list.Count > 0)
                list = new() { list[Random.Shared.Next(list.Count)] };

            if (parent.FlowMode == FlowMode.PickIndex)
                list = list.Skip(parent.PickIndex).Take(1).ToList();

            foreach (var child in list)
            {
                var r = await ExecuteNodeAsync(child, ctx);
                if (r.Status == ExecutionStatus.Stop) return r;
                if (r.Status == ExecutionStatus.Break) break;
            }
            return NodeResult.Ok();
        }

        private Task<NodeResult> DispatchAsync(DLoop node, DLoopContext ctx) => node.Type switch
        {
            //AppArgs có thể là start/stop/install/uninstall/clear, tùy a[0].action
            NodeType.AppArgs => AppRunner.RunAsync(ctx, node.GetArgs<AppArgs>()),
            
            //GetColumnDataArgs dùng để lấy dữ liệu cột từ appdata trong database, và điền vào nó vào đâu tuỳ bạn
            NodeType.GetColumnDataArgs => DatabaseRunner.RunGetDatabaseColumn(ctx, node.GetArgs<GetColumnDataArgs>()),
            //SetColumnDataArgs dùng để set dữ liệu cột vào appdata trong database, dữ liệu này có thể dùng cho node khác hoặc cho chính node này ở lần chạy sau
            NodeType.SetColumnDataArgs => DatabaseRunner.RunSetDatabaseColumn(ctx, node.GetArgs<SetColumnDataArgs>()),
           
            // EmojiArgs dùng để click vào emoji, args có thể là text hoặc content-desc của emoji đó
            NodeType.EmojiArgs => EmojiRunner.RunAsync(ctx, node.GetArgs<EmojiArgs>()),
            
            // FileArgs dùng để copy file từ PC vào phone, hoặc ngược lại, hoặc xóa file trên phone
            NodeType.FileArgs => FileRunner.RunCopyFiles(ctx, node.GetArgs<FileArgs>()),
           
            // HttpRequestConfig dùng để gửi 1 HTTP request, args có thể config method/url/body/... và lưu kết quả vào bag
            NodeType.HttpRequestConfig => HttpRunner.RunAsync(ctx, node.GetArgs<HttpRequestConfig>()),
            
            //// ImeActionArgs dùng để thao tác với bàn phím ảo, như nhấn Enter, hoặc nhấn nút chuyển đổi giữa các layout bàn phím
            //NodeType.ImeActionArgs => InputRunner.RunImeAction(ctx, node.GetArgs<ImeActionArgs>()),

            // SendTextArgs dùng để gửi text, args có thể là text thường hoặc text dài, hoặc text lấy từ database column
            NodeType.SendTextArgs => InputRunner.Run_InputText_Test(ctx, node.GetArgs<SendTextArgs>()),
           
            // KeyPressArgs dùng để gửi 1 key event, args có thể là keycode hoặc editor action code
            NodeType.KeyPressArgs => IntentRunner.ExecutePressKeyAsync(ctx, node.GetArgs<KeyPressArgs>()),

            NodeType.Click => MouseRunner.Click(ctx, node.GetArgs<ClickArg>()),
            NodeType.LongPress => MouseRunner.LongPress(ctx, node.GetArgs<LongPressArg>()),
            NodeType.Swipe => MouseRunner.Swipe(ctx, node.GetArgs<SwipeArg>()),
            NodeType.DragDrop => MouseRunner.DragDrop(ctx, node.GetArgs<DragArg>()),

            NodeType.VisionScanArgs => VisionScanRunner.GetPositionAndClick(ctx, node.GetArgs<VisionScanArgs>()),

            NodeType.Delay => Logger.DelayAsync(node.GetArgs<int>(), ctx.Token, ctx.LogKey)
            .ContinueWith(t => new NodeResult
            {
                Status = t.Result, // ExecutionStatus từ Logger
            }),

            _ => Task.FromResult(NodeResult.Ok())
        };
    }
}