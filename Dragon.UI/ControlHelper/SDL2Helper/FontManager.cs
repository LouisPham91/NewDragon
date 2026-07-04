using System.Collections.Concurrent;
using SDL3;

namespace Dragon.ControlHelper.ScrcpyNet.Services
{
    public static class FontManagerSDL3
    {
        private static nint _baseFont = nint.Zero;
        // Dùng font Windows, không cần mang file
        private static readonly string _basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
        private static readonly ConcurrentDictionary<int, nint> _cache = new();

        static FontManagerSDL3()
        {
           
            if (SDL.WasInit(SDL.InitFlags.Video) == 0) SDL.Init(SDL.InitFlags.Video);
            if (TTF.WasInit() == 0) TTF.Init();
        }

        public static void LoadBase(int baseSize = 48)
        {
            if (_baseFont != nint.Zero) return;
            _baseFont = TTF.OpenFont(_basePath, baseSize);
            if (_baseFont == nint.Zero)
                throw new Exception($"TTF.OpenFont failed: {SDL.GetError()}");
        }

        public static void Warmup()
        {
            LoadBase(48);
            for (int s = 8; s <= 48; s += 2)
                GetFont(s);
        }

        public static nint GetFont(int pixelSize)
        {
            return _cache.GetOrAdd(pixelSize, size =>
            {
                uint props = SDL.CreateProperties();
                SDL.SetPointerProperty(props, "SDL_ttf.font.create.existing_font", _baseFont);
                SDL.SetFloatProperty(props, "SDL_ttf.font.create.size", size);
                nint f = TTF.OpenFontWithProperties(props);
                SDL.DestroyProperties(props);
                return f;
            });
        }

        public static void ReleaseAll()
        {
            foreach (var f in _cache.Values)
                if (f != nint.Zero && f != _baseFont) TTF.CloseFont(f);
            _cache.Clear();
            if (_baseFont != nint.Zero) { TTF.CloseFont(_baseFont); _baseFont = nint.Zero; }
            TTF.Quit();
        }
    }
}