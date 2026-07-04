using System.Drawing;
using System.Drawing.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Runtime.InteropServices;


namespace Dragon.Controller.Controller.GlobalControl.Helper
{
   
    public static class BitmapExtensions
    {
        public unsafe static Image<Rgba32> ToImageSharp(this Bitmap bmp)
        {
            var img = new Image<Rgba32>(bmp.Width, bmp.Height);
            var rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            var data = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                img.ProcessPixelRows(accessor =>
                {
                    for (int y = 0; y < accessor.Height; y++)
                    {
                        var row = accessor.GetRowSpan(y);
                        IntPtr ptr = data.Scan0 + y * data.Stride;
                        MemoryMarshal.Cast<byte, BGRA>(new Span<byte>(ptr.ToPointer(), data.Stride))
                            .Slice(0, bmp.Width)
                            .CopyTo(MemoryMarshal.Cast<Rgba32, BGRA>(row));
                    }
                });
            }
            finally { bmp.UnlockBits(data); }
            return img;
        }

        // struct để map BGRA (GDI+) sang RGBA (ImageSharp)
        [StructLayout(LayoutKind.Sequential)]
        private struct BGRA { public byte B, G, R, A; }
    }
}
