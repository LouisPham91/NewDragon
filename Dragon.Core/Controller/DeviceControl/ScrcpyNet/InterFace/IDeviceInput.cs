using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.DeviceControl.ScrcpyNet.InterFace
{
    public interface IDeviceInput
    {
        Task SendTextAsync(string text, CancellationToken token, string logKey);
        Task SetClipboardAsync(string text, CancellationToken token, string logKey);
    }
}
