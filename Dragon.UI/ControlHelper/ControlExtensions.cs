using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dragon.ControlHelper
{
    public static class ControlExtensions
    {
        public static Task InvokeAsync(this Control control, Action action)
        {
            var tcs = new TaskCompletionSource();

            if (control.IsHandleCreated && !control.IsDisposed)
            {
                control.BeginInvoke(new MethodInvoker(() =>
                {
                    try { action(); tcs.SetResult(); }
                    catch (Exception ex) { tcs.SetException(ex); }
                }));
            }
            else
            {
                tcs.SetException(new InvalidOperationException("[ControlExtensions] Control handle not created."));
            }

            return tcs.Task;
        }
    }


}
