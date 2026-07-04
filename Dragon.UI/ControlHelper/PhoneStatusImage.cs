using AdvancedSharpAdbClient.Models;
using Dragon.ControlHelper;
using Dragon.DesignView.Public;

namespace Dragon.UI.ControlHelper
{
    public static class PhoneStatusImage
    {
        static string phoneOnline = "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 384 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M80 16C53.5 16 32 37.5 32 64l0 384c0 26.5 21.5 48 48 48l224 0c26.5 0 48-21.5 48-48l0-384c0-26.5-21.5-48-48-48L80 16zM16 64C16 28.7 44.7 0 80 0L304 0c35.3 0 64 28.7 64 64l0 384c0 35.3-28.7 64-64 64L80 512c-35.3 0-64-28.7-64-64L16 64zM152 432l80 0c4.4 0 8 3.6 8 8s-3.6 8-8 8l-80 0c-4.4 0-8-3.6-8-8s3.6-8 8-8z\"/></svg>";
        static string phoneOffline = "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 576 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M2.3-29.8c2.7-2.7 7-3.1 10.1-1l1.3 1 98.3 98.3 0-4.6c0-33.1 25.2-60.4 57.5-63.7l6.5-.3 224 0 6.5 .3C438.8 3.6 464 30.9 464 64l0 356.6 109.7 109.7 1 1.2c2.1 3.1 1.7 7.3-1 10.1s-7 3.1-10.1 1l-1.2-1-560-560-1-1.3c-2-3.1-1.7-7.3 1-10.1zM128 175.2L128 448c0 26.5 21.5 48 48 48l224 0c13.5 0 25.6-5.6 34.3-14.5l11.3 11.3c-10.2 10.4-23.8 17.3-39.1 18.9l-6.5 .3-224 0-6.5-.3C137.2 508.4 112 481.1 112 448l0-288.8 16 16zM328 432c4.4 0 8 3.6 8 8s-3.6 8-8 8l-80 0c-4.4 0-8-3.6-8-8s3.6-8 8-8l80 0zM176 16c-26.5 0-48 21.5-48 48l0 20.6 320 320 0-340.6c0-26.5-21.5-48-48-48L176 16z\"/></svg>";
        static string phoneIsRuning = "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 576 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M328-32c137 0 248 111 248 248 0 4.4-3.6 8-8 8s-8-3.6-8-8c0-128.1-103.9-232-232-232-4.4 0-8-3.6-8-8s3.6-8 8-8zm40 224a16 16 0 1 0 -32 0 16 16 0 1 0 32 0zm-48 0a32 32 0 1 1 64 0 32 32 0 1 1 -64 0zm0-120c0-4.4 3.6-8 8-8 83.9 0 152 68.1 152 152 0 4.4-3.6 8-8 8s-8-3.6-8-8c0-75.1-60.9-136-136-136-4.4 0-8-3.6-8-8zM96 0l176 0 0 16-176 0C69.5 16 48 37.5 48 64l0 384c0 26.5 21.5 48 48 48l224 0c26.5 0 48-21.5 48-48l0-176 16 0 0 176c0 35.3-28.7 64-64 64L96 512c-35.3 0-64-28.7-64-64L32 64C32 28.7 60.7 0 96 0zm72 432l80 0c4.4 0 8 3.6 8 8s-3.6 8-8 8l-80 0c-4.4 0-8-3.6-8-8s3.6-8 8-8z\"/></svg>";

        static Bitmap? phoneOnlineDark = null;
        static Bitmap? phoneOnlineLight = null;

        static Bitmap? phoneOfflineDark = null;
        static Bitmap? phoneOfflineLight = null;

        static Bitmap? phoneIsRuningDark = null;
        static Bitmap? phoneIsRuningLight = null;

        public static void LoadImage()
        {
            phoneOnlineLight = SvgRenderer.RenderSvgFromString(phoneOnline, 25, 25, Color.Black);
            phoneOnlineDark = SvgRenderer.RenderSvgFromString(phoneOnline, 25, 25, Color.White);

            phoneOfflineLight = SvgRenderer.RenderSvgFromString(phoneOffline, 25, 25, Color.Black);
            phoneOfflineDark = SvgRenderer.RenderSvgFromString(phoneOffline, 25, 25, Color.White);

            phoneIsRuningLight = SvgRenderer.RenderSvgFromString(phoneIsRuning, 25, 25, Color.Black);
            phoneIsRuningDark = SvgRenderer.RenderSvgFromString(phoneIsRuning, 25, 25, Color.White);

        }

        public static Bitmap? GetImage(DeviceState DeviceState, bool IsRuning)
        {
            if (IsRuning)
                return (ThemeHelper.CurrentMode == ThemeMode.Dark) ? phoneIsRuningDark : phoneIsRuningLight;
            else if (DeviceState == DeviceState.Online)
                return (ThemeHelper.CurrentMode == ThemeMode.Dark) ? phoneOnlineDark : phoneOnlineLight;
            else
                return (ThemeHelper.CurrentMode == ThemeMode.Dark) ? phoneOfflineDark : phoneOfflineLight;
        }
    }
}
