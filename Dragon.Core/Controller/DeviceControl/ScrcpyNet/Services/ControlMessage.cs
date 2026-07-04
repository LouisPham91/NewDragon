using Dragon.Controller.DeviceControl.ScrcpyNet.AndroidKeyBroad;
using System.Text;

namespace Dragon.Controller.DeviceControl.ScrcpyNet.Services
{
    // Struct to represent Position (x, y, screenWidth, screenHeight)
    public readonly struct Position
    {
        public int X { get; }
        public int Y { get; }
        public ushort ScreenWidth { get; }
        public ushort ScreenHeight { get; }

        public Position(int x, int y, ushort screenWidth, ushort screenHeight)
        {
            X = x;
            Y = y;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
        }
    }

    public interface IControlMessage
    {
        int Type { get; }
        byte[] GetBytes(); // Renamed from ToBytes for clarity
    }

    public static class ControlMessage
    {
        // Message types
        public const int TYPE_INJECT_KEYCODE = 0;
        public const int TYPE_INJECT_TEXT = 1;
        public const int TYPE_INJECT_TOUCH_EVENT = 2;
        public const int TYPE_INJECT_SCROLL_EVENT = 3;
        public const int TYPE_BACK_OR_SCREEN_ON = 4;
        public const int TYPE_EXPAND_NOTIFICATION_PANEL = 5;
        public const int TYPE_EXPAND_SETTINGS_PANEL = 6;
        public const int TYPE_COLLAPSE_PANELS = 7;
        public const int TYPE_GET_CLIPBOARD = 8;
        public const int TYPE_SET_CLIPBOARD = 9;
        public const int TYPE_SET_DISPLAY_POWER = 10;
        public const int TYPE_ROTATE_DEVICE = 11;
        public const int TYPE_UHID_CREATE = 12;
        public const int TYPE_UHID_INPUT = 13;
        public const int TYPE_UHID_DESTROY = 14;
        public const int TYPE_OPEN_HARD_KEYBOARD_SETTINGS = 15;
        public const int TYPE_START_APP = 16;
        public const int TYPE_RESET_VIDEO = 17;
        public const int TYPE_CAMERA_SET_TORCH = 18;
        public const int TYPE_CAMERA_ZOOM_IN = 19;
        public const int TYPE_CAMERA_ZOOM_OUT = 20;
        public const int TYPE_RESIZE_DISPLAY = 21;

        // Constants
        public const int INJECT_TEXT_MAX_LENGTH = 300;
        public const int CLIPBOARD_TEXT_MAX_LENGTH = (1 << 18) - 14; // 256KB - 14 bytes

        public const int COPY_KEY_NONE = 0;
        public const int COPY_KEY_COPY = 1;
        public const int COPY_KEY_CUT = 2;

        public static readonly long SEQUENCE_INVALID = 0;
    }

    public class InjectKeycodeControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_INJECT_KEYCODE;
        public AndroidKeyEventAction Action { get; }
        public AndroidKeycode Keycode { get; }
        public int Repeat { get; }
        public AndroidMetastate MetaState { get; }

        public InjectKeycodeControlMessage(AndroidKeyEventAction action, AndroidKeycode keycode, int repeat, AndroidMetastate metaState)
        {
            Action = action;
            Keycode = keycode;
            Repeat = repeat;
            MetaState = metaState;
        }

        // Format: type(1), action(1), keycode(4), repeat(4), metaState(4)
        public byte[] GetBytes()
        {
            using var ms = new MemoryStream();
            using var writer = new BigEndianBinaryWriter(ms);
            writer.Write((byte)Type);
            writer.Write((byte)Action);
            writer.Write((int)Keycode);
            writer.Write(Repeat);
            writer.Write((int)MetaState);
            return ms.ToArray();
        }
    }

    public class InjectTextControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_INJECT_TEXT;
        public string Text { get; }

        public InjectTextControlMessage(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Text cannot be null or empty.");
            if (Encoding.UTF8.GetByteCount(text) > ControlMessage.INJECT_TEXT_MAX_LENGTH)
                throw new ArgumentException($"Text exceeds max length of {ControlMessage.INJECT_TEXT_MAX_LENGTH} bytes.");
            Text = text;
        }

        // Format: type(1), length(4), text(UTF-8 bytes)
        public byte[] GetBytes()
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(Text);
            using var ms = new MemoryStream();
            using var writer = new BigEndianBinaryWriter(ms);
            writer.Write((byte)Type);
            writer.Write(textBytes.Length);
            writer.Write(textBytes);
            return ms.ToArray();
        }
    }

    public class TouchEventControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_INJECT_TOUCH_EVENT;
        public AndroidMotionEventAction Action { get; }
        public long PointerId { get; }
        public Position Position { get; }
        public float Pressure { get; }
        public AndroidMotionEventButtons ActionButton { get; }
        public AndroidMotionEventButtons Buttons { get; }

        public TouchEventControlMessage(AndroidMotionEventAction action, long pointerId, Position position, float pressure, AndroidMotionEventButtons actionButton, AndroidMotionEventButtons buttons)
        {
            Action = action;
            PointerId = pointerId;
            Position = position;
            Pressure = pressure;
            ActionButton = actionButton;
            Buttons = buttons;
        }

        // Format: type(1), action(1), pointerId(8), x(4), y(4), screenWidth(2), screenHeight(2), pressure(2, fixed-point), actionButton(4), buttons(4)
        public byte[] GetBytes()
        {
            using var ms = new MemoryStream();
            using var writer = new BigEndianBinaryWriter(ms);
            writer.Write((byte)Type);
            writer.Write((byte)Action);
            writer.Write(PointerId);
            writer.Write(Position.X);
            writer.Write(Position.Y);
            writer.Write(Position.ScreenWidth);
            writer.Write(Position.ScreenHeight);
            writer.Write((ushort)(Pressure * 65535)); // Convert float to 16-bit fixed-point
            writer.Write((int)ActionButton);
            writer.Write((int)Buttons);
            return ms.ToArray();
        }
    }

    public class ScrollEventControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_INJECT_SCROLL_EVENT;
        public Position Position { get; }
        public float HScroll { get; }
        public float VScroll { get; }
        public AndroidMotionEventButtons Buttons { get; }

        public ScrollEventControlMessage(Position position, float hScroll, float vScroll, AndroidMotionEventButtons buttons)
        {
            Position = position;
            HScroll = hScroll;
            VScroll = vScroll;
            Buttons = buttons;
        }

        // Format: type(1), x(4), y(4), screenWidth(2), screenHeight(2), hScroll(2, fixed-point), vScroll(2, fixed-point), buttons(4)
        public byte[] GetBytes()
        {
            using var ms = new MemoryStream();
            using var writer = new BigEndianBinaryWriter(ms);
            writer.Write((byte)Type);
            writer.Write(Position.X);
            writer.Write(Position.Y);
            writer.Write(Position.ScreenWidth);
            writer.Write(Position.ScreenHeight);
            writer.Write((short)(HScroll * 2048f));
            writer.Write((short)(VScroll * 2048f));
            writer.Write((int)Buttons);
            return ms.ToArray();
        }
    }

    public class BackOrScreenOnControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_BACK_OR_SCREEN_ON;
        public AndroidKeyEventAction Action { get; }

        public BackOrScreenOnControlMessage(AndroidKeyEventAction action)
        {
            Action = action;
        }

        // Format: type(1), action(1)
        public byte[] GetBytes()
        {
            using var ms = new MemoryStream();
            using var writer = new BigEndianBinaryWriter(ms);
            writer.Write((byte)Type);
            writer.Write((byte)Action);
            return ms.ToArray();
        }
    }

    public class ExpandNotificationPanelControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_EXPAND_NOTIFICATION_PANEL;

        // Format: type(1)
        public byte[] GetBytes()
        {
            return new byte[] { (byte)Type };
        }
    }

    public class ExpandSettingsPanelControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_EXPAND_SETTINGS_PANEL;

        // Format: type(1)
        public byte[] GetBytes()
        {
            return new byte[] { (byte)Type };
        }
    }

    public class CollapsePanelsControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_COLLAPSE_PANELS;

        // Format: type(1)
        public byte[] GetBytes()
        {
            return new byte[] { (byte)Type };
        }
    }

    public class GetClipboardControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_GET_CLIPBOARD;
        public int CopyKey { get; }

        public GetClipboardControlMessage(int copyKey)
        {
            CopyKey = copyKey;
        }

        // Format: type(1), copyKey(1)
        public byte[] GetBytes()
        {
            using var ms = new MemoryStream();
            using var writer = new BigEndianBinaryWriter(ms);
            writer.Write((byte)Type);
            writer.Write((byte)CopyKey);
            return ms.ToArray();
        }
    }

    public class SetClipboardControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_SET_CLIPBOARD;
        public long Sequence { get; }
        public string Text { get; }
        public bool Paste { get; }

        public SetClipboardControlMessage(long sequence, string text, bool paste)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Text cannot be null or empty.");
            if (Encoding.UTF8.GetByteCount(text) > ControlMessage.CLIPBOARD_TEXT_MAX_LENGTH)
                throw new ArgumentException($"Text exceeds max length of {ControlMessage.CLIPBOARD_TEXT_MAX_LENGTH} bytes.");
            Sequence = sequence;
            Text = text;
            Paste = paste;
        }

        // Format: type(1), sequence(8), paste(1), length(4), text(UTF-8 bytes)
        public byte[] GetBytes()
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(Text);
            using var ms = new MemoryStream();
            using var writer = new BigEndianBinaryWriter(ms);
            writer.Write((byte)Type);
            writer.Write(Sequence);
            writer.Write((byte)(Paste ? 1 : 0));
            writer.Write(textBytes.Length);
            writer.Write(textBytes);
            return ms.ToArray();
        }
    }

    public class SetDisplayPowerControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_SET_DISPLAY_POWER;
        public bool On { get; }

        public SetDisplayPowerControlMessage(bool on)
        {
            On = on;
        }

        // Format: type(1), on(1)
        public byte[] GetBytes()
        {
            using var ms = new MemoryStream();
            using var writer = new BigEndianBinaryWriter(ms);
            writer.Write((byte)Type);
            writer.Write((byte)(On ? 1 : 0));
            return ms.ToArray();
        }
    }

    public class RotateDeviceControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_ROTATE_DEVICE;

        // Format: type(1)
        public byte[] GetBytes()
        {
            return new byte[] { (byte)Type };
        }
    }

    public class UhidCreateControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_UHID_CREATE;
        public ushort Id { get; }
        public ushort VendorId { get; }
        public ushort ProductId { get; }
        public string Name { get; }
        public byte[] Data { get; }

        public UhidCreateControlMessage(ushort id, ushort vendorId, ushort productId, string name, byte[] data)
        {
            Id = id;
            VendorId = vendorId;
            ProductId = productId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        // Format: type(1), id(2), vendorId(2), productId(2), nameLength(1), name(UTF-8 bytes), dataLength(2), data(bytes)
        public byte[] GetBytes()
        {
            byte[] nameBytes = Encoding.UTF8.GetBytes(Name);
            if (nameBytes.Length > 127)
                throw new ArgumentException("Name length exceeds 127 bytes.");

            using var ms = new MemoryStream();
            using var writer = new BigEndianBinaryWriter(ms);
            writer.Write((byte)Type);
            writer.Write(Id);
            writer.Write(VendorId);
            writer.Write(ProductId);
            writer.Write((byte)nameBytes.Length);
            writer.Write(nameBytes);
            writer.Write((ushort)Data.Length);
            writer.Write(Data);
            return ms.ToArray();
        }
    }

    public class UhidInputControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_UHID_INPUT;
        public ushort Id { get; }
        public byte[] Data { get; }

        public UhidInputControlMessage(ushort id, byte[] data)
        {
            Id = id;
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        // Format: type(1), id(2), length(2), data(bytes)
        public byte[] GetBytes()
        {
            using var ms = new MemoryStream();
            using var writer = new BigEndianBinaryWriter(ms);
            writer.Write((byte)Type);
            writer.Write(Id);
            writer.Write((ushort)Data.Length);
            writer.Write(Data);
            return ms.ToArray();
        }
    }

    public class UhidDestroyControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_UHID_DESTROY;
        public ushort Id { get; }

        public UhidDestroyControlMessage(ushort id)
        {
            Id = id;
        }

        // Format: type(1), id(2)
        public byte[] GetBytes()
        {
            using var ms = new MemoryStream();
            using var writer = new BigEndianBinaryWriter(ms);
            writer.Write((byte)Type);
            writer.Write(Id);
            return ms.ToArray();
        }
    }

    public class OpenHardKeyboardSettingsControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_OPEN_HARD_KEYBOARD_SETTINGS;

        // Format: type(1)
        public byte[] GetBytes()
        {
            return new byte[] { (byte)Type };
        }
    }

    public class StartAppControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_START_APP;
        public string Name { get; }

        public StartAppControlMessage(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        // Format: type(1), nameLength(1), name(UTF-8 bytes)
        public byte[] GetBytes()
        {
            byte[] nameBytes = Encoding.UTF8.GetBytes(Name);
            if (nameBytes.Length > 127)
                throw new ArgumentException("Name length exceeds 127 bytes.");

            using var ms = new MemoryStream();
            using var writer = new BigEndianBinaryWriter(ms);
            writer.Write((byte)Type);
            writer.Write((byte)nameBytes.Length);
            writer.Write(nameBytes);
            return ms.ToArray();
        }
    }

    public class ResetVideoControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_RESET_VIDEO;

        // Format: type(1)
        public byte[] GetBytes()
        {
            return new byte[] { (byte)Type };
        }
    }

    public class CameraSetTorchControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_CAMERA_SET_TORCH;
        public bool On { get; }
        public CameraSetTorchControlMessage(bool on) => On = on;
        public byte[] GetBytes() => new[] { (byte)Type, (byte)(On ? 1 : 0) };
    }

    public class CameraZoomInControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_CAMERA_ZOOM_IN;
        public byte[] GetBytes() => new[] { (byte)Type };
    }

    public class CameraZoomOutControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_CAMERA_ZOOM_OUT;
        public byte[] GetBytes() => new[] { (byte)Type };
    }

    public class ResizeDisplayControlMessage : IControlMessage
    {
        public int Type => ControlMessage.TYPE_RESIZE_DISPLAY;
        public ushort Width { get; }
        public ushort Height { get; }
        public ResizeDisplayControlMessage(ushort w, ushort h) { Width = w; Height = h; }
        public byte[] GetBytes()
        {
            using var ms = new MemoryStream();
            using var wtr = new BigEndianBinaryWriter(ms);
            wtr.Write((byte)Type);
            wtr.Write(Width);
            wtr.Write(Height);
            return ms.ToArray();
        }
    }
}