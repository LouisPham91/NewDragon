using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Dragon.UI.ControlHelper
{
    public static class NativeBootstrap
    {
        [ModuleInitializer]
        public static void Init()
        {
            //C:\Users\phamh\Desktop\NewDragon\Dragon.UI\runtimes\win-x64\native\WebView2Loader.dll
            var native = Path.Combine(AppContext.BaseDirectory, "Extension", "ScrcpyNet");
            if (!Directory.Exists(native)) return;

            // 1. cho Windows loader (SDL3, SDL3_ttf)
            SetDllDirectory(native);

            // 2. preload để chắc ăn
            var sdl3 = Path.Combine(native, "SDL3.dll");
            var sdl3ttf = Path.Combine(native, "SDL3_ttf.dll");
            var sqlite = Path.Combine(native, "e_sqlite3.dll");
            // var libwdi = Path.Combine(native, "libwdi.dll");
            //var webview2 = Path.Combine(native, "WebView2Loader.dll");
            //WebView2Loader.dll
            if (File.Exists(sdl3)) NativeLibrary.Load(sdl3);
            if (File.Exists(sdl3ttf)) NativeLibrary.Load(sdl3ttf);
            if (File.Exists(sqlite)) NativeLibrary.Load(sqlite);
            //if (File.Exists(sqlite)) NativeLibrary.Load(libwdi);
            //if (File.Exists(webview2)) NativeLibrary.Load(webview2);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);
    }
}
