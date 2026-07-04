using Dragon.DesignView.Public;

namespace Dragon.ControlHelper
{
    public enum PhoneStatus
    {
        Loading,
        Connecting,
        Resizing,
        Disconnect,
        Controller,      
    }

    public class ImagePhoneStatus
    {
      
        public static readonly Dictionary<(PhoneStatus, ThemeStyle), Bitmap> PhoneStatusDict = new Dictionary<(PhoneStatus, ThemeStyle), Bitmap>();
        static string SVGDisConnect = "<svg width=\"800\" height=\"800\" viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><g id=\"grid_system\"/><g id=\"_icons\"><path d=\"M20.7 19.3l-1-1c-.4-.4-1-.4-1.4 0s-.4 1 0 1.4l1 1c.2.2.5.3.7.3s.5-.1.7-.3c.4-.4.4-1 0-1.4z\"/><path d=\"M14 22c0 .6.4 1 1 1s1-.4 1-1v-2c0-.6-.4-1-1-1s-1 .4-1 1v2z\"/><path d=\"M22 14h-2c-.6 0-1 .4-1 1s.4 1 1 1h2c.6 0 1-.4 1-1s-.4-1-1-1z\"/><path d=\"M20.7 8.4c0-1.4-.5-2.6-1.5-3.6-1-1-2.2-1.5-3.6-1.5s-2.6.5-3.6 1.5L9.8 7c-.4.4-.4 1 0 1.4s1 .4 1.4 0l2.2-2.2c1.2-1.2 3.2-1.2 4.4 0 .6.6.9 1.4.9 2.2 0 .8-.3 1.6-.9 2.2l-2.2 2.2c-.4.4-.4 1 0 1.4.2.2.5.3.7.3s.5-.1.7-.3l2.2-2.2c1-1 1.5-2.2 1.5-3.6z\"/><path d=\"M3.3 15.6c0 1.4.5 2.6 1.5 3.6 1 1 2.2 1.5 3.6 1.5s2.6-.5 3.6-1.5l2.2-2.2c.4-.4.4-1 0-1.4s-1-.4-1.4 0l-2.2 2.2c-1.2 1.2-3.2 1.2-4.4 0-.6-.6-.9-1.4-.9-2.2 0-.8.3-1.6.9-2.2l2.2-2.2c.4-.4.4-1 0-1.4s-1-.4-1.4 0L4.8 12c-1 1-1.5 2.2-1.5 3.6z\"/><path d=\"M5.7 4.3l-1-1c-.4-.4-1-.4-1.4 0s-.4 1 0 1.4l1 1c.2.2.4.3.7.3s.5-.1.7-.3c.4-.4.4-1 0-1.4z\"/><path d=\"M10 4V2c0-.6-.4-1-1-1S8 1.4 8 2v2c0 .6.4 1 1 1s1-.4 1-1z\"/><path d=\"M4 10c.6 0 1-.4 1-1S4.6 8 4 8H2c-.6 0-1 .4-1 1s.4 1 1 1h2z\"/></g></svg>";
        static string SVGController = "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\"><path d=\"M9 3.5V2M5.06 5.06L4 4M5.06 13L4 14.06M13 5.06L14.06 4M3.5 9H2\" stroke=\"#000\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\"/><path d=\"M15.86 16.19l-2.49 4.63c-.28.53-.42.79-.61.86-.15.06-.32.04-.46-.08-.15-.1-.24-.39-.4-.93L8.45 9.45c-.14-.47-.21-.71-.15-.87.05-.14.16-.25.3-.3.16-.06.4.01.87.15l11.23 3.46c.55.17.84.26.93.41.12.14.14.31.08.46-.07.19-.33.33-.86.61l-4.63 2.49c-.08.04-.12.07-.16.11-.04.04-.07.08-.11.12-.04.04-.07.08-.11.16z\" fill=\"#000\"/></svg>";
        static string pathDragonLoading = Path.Combine("Extension", "Gif", "DragonLoading.gif");
        public static readonly Bitmap LoadingBitmap;
        static ImagePhoneStatus()
        {
            // Disconnect với 3 màu
            PhoneStatusDict[(PhoneStatus.Disconnect, ThemeStyle.ThemeRed)] =
                SvgRenderer.RenderSvgFromString(SVGDisConnect, 200, 200, ThemeRed.ForceColofulFist);
            PhoneStatusDict[(PhoneStatus.Disconnect, ThemeStyle.ThemeGold)] =
                SvgRenderer.RenderSvgFromString(SVGDisConnect, 200, 200, ThemeGold.ForceColofulFist);
            PhoneStatusDict[(PhoneStatus.Disconnect, ThemeStyle.ThemeGreen)] =
                SvgRenderer.RenderSvgFromString(SVGDisConnect, 200, 200, ThemeGreen.ForceColofulFist);

            // Controller với 3 màu
            PhoneStatusDict[(PhoneStatus.Controller, ThemeStyle.ThemeRed)] =
                SvgRenderer.RenderSvgFromString(SVGController, 200, 200, ThemeRed.ForceColofulFist);
            PhoneStatusDict[(PhoneStatus.Controller, ThemeStyle.ThemeGold)] =
                SvgRenderer.RenderSvgFromString(SVGController, 200, 200, ThemeGold.ForceColofulFist);
            PhoneStatusDict[(PhoneStatus.Controller, ThemeStyle.ThemeGreen)] =
                SvgRenderer.RenderSvgFromString(SVGController, 200, 200, ThemeGreen.ForceColofulFist);
            Bitmap bmpLoading = (Bitmap)Image.FromFile(pathDragonLoading);
            LoadingBitmap = (Bitmap)Image.FromFile(pathDragonLoading);

            /*
             * // Lấy Disconnect màu đỏ
            Bitmap bmp1 = ImagePhoneStatus.PhoneStatusDict[(PhoneStatus.Disconnect, StatusColor.Red)];

            // Lấy Controller màu xanh
            Bitmap bmp2 = ImagePhoneStatus.PhoneStatusDict[(PhoneStatus.Controller, StatusColor.Green)];

            // Lấy Loading (chỉ 1 ảnh)
            Bitmap bmp3 = ImagePhoneStatus.LoadingBitmap;

             */
        }

    }
}
