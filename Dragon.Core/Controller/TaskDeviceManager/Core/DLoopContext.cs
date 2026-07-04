using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using Dragon.Controller.DeviceControl.HATX;
using Dragon.Controller.DeviceControl.ScrcpyNet.InterFace;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Core
{
    public class DLoopContext
    {
        public PhoneSession Session { get; init; } = null!;
        public CancellationToken Token { get; init; }
        public string LogKey => Session.Phone.DeviceID;

        // shortcut — không lưu trùng
        public AdbClient AdbClient => Session.AdbClient;
        public DeviceData DeviceData => Session.DeviceData!;
        public AtxDevice? Atx => Session.Atx;
        public IDeviceScreen? Screen => Session.Screen;
        public IDeviceInput? Input => Session.Input;
        public IDeviceInput? InputUhid => Session.InputUhid;
        public IDeviceMouse? Mouse => Session.Mouse;
        public IDeviceMouse? MouseUhid => Session.MouseUhid;
        private readonly ConcurrentDictionary<string, object> _bag = new();
        public void Set(string k, object v) => _bag[k] = v;
        public T? Get<T>(string k) => _bag.TryGetValue(k, out var v) ? (T)v : default;
        public void CopyBagFrom(DLoopContext other) 
        { 
            foreach (var kv in other._bag)
                _bag[kv.Key] = kv.Value; 
        }
    }
}
