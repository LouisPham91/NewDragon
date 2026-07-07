using System.Text.Json;
using System.Text.Json.Serialization;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Model.App;
using Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn;
using Dragon.Controller.TaskDeviceManager.Model.Emoji;
using Dragon.Controller.TaskDeviceManager.Model.File;
using Dragon.Controller.TaskDeviceManager.Model.HttpResponse;
using Dragon.Controller.TaskDeviceManager.Model.Input;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Controller.TaskDeviceManager.Model.Vision;

namespace Dragon.Controller.TaskDeviceManager.Core
{
    public class DLoop
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public bool IsSingleLoop { get; set; } = false; // nếu true, chỉ chạy 1 lần, không lặp lại
        public ControlMode ControlMode { get; set; }
        public VisionMode VisionMode { get; set; }
        public ConnectionType connectionType { get; set; } = ConnectionType.Auto;
        public string UserGmail { get; set; } = string.Empty; // ví dụ: "dragonfarm@gmail.com"
        public string PhoneModel { get; set; } = string.Empty;
        public NodeType Type { get; set; }
        public FlowMode FlowMode { get; set; } = FlowMode.Sequential;

        // --- AOT-safe args ---
        public string ArgsJson { get; set; } = "{}";
        [JsonIgnore] public object? Payload { get; set; }

        // --- điều khiển ---
        public List<DLoop> Children { get; set; } = new();
        public bool Enabled { get; set; } = true;
        public int RetryCount { get; set; } = 1;
        public int TimeoutMs { get; set; } = 30000;
        public OnErrorMode OnError { get; set; } = OnErrorMode.Stop;
        public int PickIndex { get; set; } = 0;
        public int SchemaVersion { get; set; } = 1;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DLoop() { }
        public DLoop(string name) => Name = name;

        // --- helper AOT ---
        public void SetArgs<T>(T obj) where T : notnull
        {
            Payload = obj;
            // dùng overload có context, không cần JsonTypeInfo<T>
            ArgsJson = JsonSerializer.Serialize(obj, typeof(T), DLoopJsonContext.Default);
        }

        public T GetArgs<T>() where T : notnull
        {
            if (Payload is T t) return t;

            var val = JsonSerializer.Deserialize(ArgsJson, typeof(T), DLoopJsonContext.Default);
            if (val is null)
                throw new InvalidOperationException($"Không deserialize được {typeof(T).Name}");

            Payload = val;
            return (T)val;
        }

        public void Hydrate(DLoop? parent = null)
        {
            if (parent != null)
            {
                if (ControlMode == default) ControlMode = parent.ControlMode;
                if (VisionMode == default) VisionMode = parent.VisionMode;
            }

            var payload = GetNodePayload();
            if (payload != null) ApplyModes(payload);

            foreach (var c in Children) c.Hydrate(this);
        }
        private void ApplyModes(object payload)
        {
            switch (payload)
            {
                case AppArgs a:
                    a.ControlMode = ControlMode;
                    SetArgs(a);
                    break;

                case GetColumnDataArgs g:
                    g.ControlMode = ControlMode;
                    SetArgs(g);
                    break;

                case SetColumnDataArgs s:
                    s.ControlMode = ControlMode;
                    SetArgs(s);
                    break;

                case EmojiArgs e:
                    e.ControlMode = ControlMode;
                    SetArgs(e);
                    break;

                case HttpRequestConfig h:
                    h.ControlMode = ControlMode;
                    SetArgs(h);
                    break;

                case ImeActionArgs ia:
                    ia.ControlMode = ControlMode;
                    SetArgs(ia);
                    break;

                case KeyPressArgs k:
                    k.ControlMode = ControlMode;
                    SetArgs(k);
                    break;

                case SendTextArgs st:
                    st.ControlMode = ControlMode;
                    SetArgs(st);
                    break;

                case ClickArg c:
                    c.ControlMode = ControlMode;
                    SetArgs(c);
                    break;

                case SwipeArg sw:
                    sw.ControlMode = ControlMode;
                    SetArgs(sw);
                    break;

                case LongPressArg lp:
                    lp.ControlMode = ControlMode;
                    SetArgs(lp);
                    break;

                case DragArg d:
                    d.ControlMode = ControlMode;
                    SetArgs(d);
                    break;

                case VisionScanArgs v: // <-- duy nhất có VisionMode
                    v.ControlMode = ControlMode;
                    v.VisionMode = VisionMode;
                    SetArgs(v);
                    break;

                case FileArgs f:
                    // FileArgs không có mode, giữ nguyên
                    SetArgs(f);
                    break;
            }
        }
        private object? GetNodePayload() => Type switch
        {
            //app 
            NodeType.AppArgs => GetArgs<AppArgs>(),
            // database
            NodeType.SetColumnDataArgs => GetArgs<SetColumnDataArgs>(),
            NodeType.GetColumnDataArgs => GetArgs<GetColumnDataArgs>(),
            // emoji
            NodeType.EmojiArgs => GetArgs<EmojiArgs>(),
            // file
            NodeType.FileArgs => GetArgs<FileArgs>(),
            // http
            NodeType.HttpRequestConfig => GetArgs<HttpRequestConfig>(),
            // input
            NodeType.SendTextArgs => GetArgs<SendTextArgs>(),
            NodeType.KeyPressArgs => GetArgs<KeyPressArgs>(),
            NodeType.ImeActionArgs => GetArgs<ImeActionArgs>(),
            // mouse
            NodeType.Click => GetArgs<ClickArg>(),
            NodeType.DragDrop => GetArgs<DragArg>(),
            NodeType.LongPress => GetArgs<LongPressArg>(),
            NodeType.Swipe => GetArgs<SwipeArg>(),
            // Vision
            NodeType.VisionScanArgs => GetArgs<VisionScanArgs>(),
            NodeType.Delay => GetArgs<int>(),
            _ => null
        };
    }

    public enum FlowMode { Sequential, RandomOne, PickIndex }

    public enum OnErrorMode { Stop, Continue, Skip }

    public enum NodeType
    {
        Title,
        //App
        AppArgs,
        // Database
        SetColumnDataArgs, GetColumnDataArgs,
        // Emoji
        EmojiArgs,
        // File
        FileArgs,
        // Http
        HttpRequestConfig,
        // Input
        SendTextArgs, ImeActionArgs,
        // Mouse
        Click, Swipe, LongPress, DragDrop,
        // Keyboard
        KeyPressArgs,
        // Vision
        VisionScanArgs,
        // System
        Delay
    }
}