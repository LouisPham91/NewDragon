using Dragon.Controller.Controller.DeviceControl.HATX.Core.Model;
using Dragon.Controller.TaskDeviceManager.Model.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure.Services
{

    public static class NodeConverterServices
    {
        /// <summary>
        /// Convert NodeObj (từ XML dump) sang ATXNode dùng cho VisionScan
        /// </summary>
        public static ATXNode ToATXNode(NodeObj node, int screenWidth, int screenHeight)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var atx = new ATXNode
            {
                ClassName = node.ClassName ?? string.Empty,
                Text = node.Text ?? string.Empty,
                ResourceName = node.ResourceName ?? string.Empty,
                PackageName = node.PackageName ?? string.Empty,
                ContentDescription = node.ContentDescription ?? string.Empty,
                VisionAction = VisionAction.DetectOnly
            };

            // tính tâm bounds -> %
            var b = node.Bound;
            if (b != null && b.Width > 0 && b.Height > 0 && screenWidth > 0 && screenHeight > 0)
            {
                var center = b.CenterPos;
                float px = (float)Math.Round(center.X * 100.0 / screenWidth, 2);
                float py = (float)Math.Round(center.Y * 100.0 / screenHeight, 2);

                // clamp 0-100
                px = Math.Max(0f, Math.Min(100f, px));
                py = Math.Max(0f, Math.Min(100f, py));

                atx.PotisonClick = new PointCenter { X = px, Y = py };
                atx.SecialClickPoint = $"{px},{py}"; // "x%,y%" dùng cho ClickAtPoint
            }

            return atx;
        }

        /// <summary>
        /// Overload: tự lấy screen size từ Phone nếu bạn có
        /// </summary>
        public static ATXNode ToATXNode(NodeObj node, Size screenSize)
            => ToATXNode(node, screenSize.Width, screenSize.Height);
    }

}
