using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace Dragon.ControlHelper.Interrop
{
    public static partial class SDL
    {
        [LibraryImport("SDL3", EntryPoint = "SDL_QueryTexture")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
        [return: MarshalAs(UnmanagedType.I1)]
        public static partial bool QueryTexture(nint texture, out uint format, out int access, out int w, out int h);
    }
}
