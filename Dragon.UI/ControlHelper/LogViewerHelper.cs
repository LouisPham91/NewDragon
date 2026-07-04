
using Dragon.Controller.TaskDeviceManager.Infrastructure;

namespace Dragon.ControlHelper
{
    public static class LogViewerHelper
    {
        public static void Attach(RichTextBox box, string deviceId)
        {
            box.Clear();
            LogHub.Attach(deviceId, (time, msg, icon) =>
            {
                if (box.IsDisposed) return;
                var act = () => {
                    box.SelectionStart = box.TextLength;
                    box.SelectionColor = GetColor(icon);
                    box.AppendText($"{time:HH:mm:ss} {GetIcon(icon)} {msg}{Environment.NewLine}");
                    box.SelectionColor = box.ForeColor;
                };
                if (box.InvokeRequired) box.Invoke(act); else act();
            });
        }

        private static Color GetColor(Logger.Icon icon) => icon switch
        {
            Logger.Icon.Error => Color.Red,
            Logger.Icon.Warning => Color.OrangeRed,
            _ => Color.Black
        };
        private static string GetIcon(Logger.Icon icon) => icon switch
        {
            Logger.Icon.Error => "❌",
            Logger.Icon.Warning => "⚠️",
            _ => "ℹ️"
        };
    }
}