
using SDL3;
using static SDL3.SDL;

namespace Dragon.ControlHelper.SDL2Helper
{
    
    public class SdlManager
    {
        private static bool _initialized = false;
        private static readonly object _lock = new();

        public static void Init()
        {
            lock (_lock)
            {
                if (_initialized) return;

                // SDL3: Init nhận InitFlags, trả về bool
                if (!SDL.Init(InitFlags.Video | InitFlags.Gamepad | InitFlags.Joystick))
                {
                    throw new Exception("[SdlManager] SDL3 init failed: " + SDL.GetError());
                }

                _initialized = true;
            }
        }

        // check nhanh subsystem đã init chưa
        public static bool IsInit(InitFlags flag)
        {
            return (SDL.WasInit(flag) & flag) == flag;
        }

        public static void Quit()
        {
            lock (_lock)
            {
                if (!_initialized) return;

                // SDL3 khuyến nghị quit từng subsystem
                SDL.QuitSubSystem(InitFlags.Gamepad | InitFlags.Joystick | InitFlags.Video);
                SDL.Quit(); // dọn phần còn lại

                _initialized = false;
            }
        }
    }

}
