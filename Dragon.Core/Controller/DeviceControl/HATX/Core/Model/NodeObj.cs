
using System.Drawing;
using System.Text.Json.Serialization;
using System.Xml;

namespace Dragon.Controller.DeviceControl.HATX.Core.Model
{
    public class NodeObj
    {
        [JsonPropertyName("bounds")]
        public Bounds Bound { get; set; } = Bounds.Empty;

        [JsonPropertyName("childCount")]
        public int ChildCount { get; set; } = 0;

        [JsonPropertyName("className")]
        public string ClassName { get; set; } = string.Empty;

        [JsonPropertyName("contentDescription")]
        public string ContentDescription { get; set; } = string.Empty;

        [JsonPropertyName("packageName")]
        public string PackageName { get; set; } = string.Empty;

        [JsonPropertyName("resourceName")]
        public string ResourceName { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("visibleBounds")]
        public Bounds VisibleBounds { get; set; } = Bounds.Empty;

        [JsonPropertyName("checkable")]
        public bool Checkable { get; set; } = false;

        [JsonPropertyName("checked")]
        public bool Checked { get; set; } = false;

        [JsonPropertyName("clickable")]
        public bool Clickable { get; set; } = false;

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;

        [JsonPropertyName("focusable")]
        public bool Focusable { get; set; } = false;

        [JsonPropertyName("focused")]
        public bool Focused { get; set; } = false;

        [JsonPropertyName("longClickable")]
        public bool LongClickable { get; set; } = false;

        [JsonPropertyName("scrollable")]
        public bool Scrollable { get; set; } = false;

        [JsonPropertyName("selected")]
        public bool Selected { get; set; } = false;

        [JsonPropertyName("index")]
        public int Index { get; set; } = 0;

        [JsonPropertyName("password")]
        public bool Password { get; set; } = false;

        [JsonPropertyName("visibleToUser")]
        public bool VisibleToUser { get; set; } = false;

        [JsonPropertyName("drawingOrder")]
        public int DrawingOrder { get; set; } = 0;

        [JsonPropertyName("hint")]
        public string Hint { get; set; } = string.Empty;

        [JsonPropertyName("naf")]
        public bool Naf { get; set; } = false;

        public static NodeObj? Create(XmlNode? node)
        {
            if (node == null)
            {
                return null;
            }

            var indexAttr = node.Attributes?["index"];
            var boundsAttr = node.Attributes?["bounds"];
            var classAttr = node.Attributes?["class"];
            var contentDescAttr = node.Attributes?["content-desc"];
            var packageAttr = node.Attributes?["package"];
            var resourceIdAttr = node.Attributes?["resource-id"];
            var textAttr = node.Attributes?["text"];
            var checkableAttr = node.Attributes?["checkable"];
            var checkedAttr = node.Attributes?["checked"];
            var clickableAttr = node.Attributes?["clickable"];
            var enabledAttr = node.Attributes?["enabled"];
            var focusableAttr = node.Attributes?["focusable"];
            var focusedAttr = node.Attributes?["focused"];
            var longClickableAttr = node.Attributes?["long-clickable"];
            var scrollableAttr = node.Attributes?["scrollable"];
            var selectedAttr = node.Attributes?["selected"];
            var passwordAttr = node.Attributes?["password"];
            var visibleAttr = node.Attributes?["visible-to-user"];
            var drawingAttr = node.Attributes?["drawing-order"];
            var hintAttr = node.Attributes?["hint"];
            var nafAttr = node.Attributes?["NAF"];

            var obj = new NodeObj
            {
                Index = indexAttr != null && int.TryParse(indexAttr.Value, out int idx) ? idx : 0,
                Bound = boundsAttr != null ? Bounds.CreateSafe(boundsAttr.Value) : Bounds.Empty,
                ChildCount = node.ChildNodes != null ? node.ChildNodes.Count : 0,
                ClassName = classAttr?.Value ?? string.Empty,
                ContentDescription = contentDescAttr?.Value ?? string.Empty,
                PackageName = packageAttr?.Value ?? string.Empty,
                ResourceName = resourceIdAttr?.Value ?? string.Empty,
                Text = textAttr?.Value ?? string.Empty,
                Checkable = checkableAttr != null && bool.TryParse(checkableAttr.Value, out bool cb) ? cb : false,
                Checked = checkedAttr != null && bool.TryParse(checkedAttr.Value, out bool ch) ? ch : false,
                Clickable = clickableAttr != null && bool.TryParse(clickableAttr.Value, out bool cl) ? cl : false,
                Enabled = enabledAttr != null && bool.TryParse(enabledAttr.Value, out bool en) ? en : false,
                Focusable = focusableAttr != null && bool.TryParse(focusableAttr.Value, out bool fo) ? fo : false,
                Focused = focusedAttr != null && bool.TryParse(focusedAttr.Value, out bool fd) ? fd : false,
                LongClickable = longClickableAttr != null && bool.TryParse(longClickableAttr.Value, out bool lg) ? lg : false,
                Scrollable = scrollableAttr != null && bool.TryParse(scrollableAttr.Value, out bool sc) ? sc : false,
                Selected = selectedAttr != null && bool.TryParse(selectedAttr.Value, out bool se) ? se : false,
                Password = passwordAttr != null && bool.TryParse(passwordAttr.Value, out bool pw) ? pw : false,
                VisibleToUser = visibleAttr != null && bool.TryParse(visibleAttr.Value, out bool vu) ? vu : false,
                DrawingOrder = drawingAttr != null && int.TryParse(drawingAttr.Value, out int dr) ? dr : 0,
                Hint = hintAttr?.Value ?? string.Empty,
                Naf = nafAttr != null && bool.TryParse(nafAttr.Value, out bool nf) ? nf : false,
            };

            obj.VisibleBounds = obj.Bound;
            return obj;
        }


        public class Bounds
        {
            public static readonly Bounds Empty = new Bounds(0, 0, 0, 0);

            [JsonPropertyName("top")]
            public int Top { get; set; }

            [JsonPropertyName("left")]
            public int Left { get; set; }

            [JsonPropertyName("bottom")]
            public int Bottom { get; set; }

            [JsonPropertyName("right")]
            public int Right { get; set; }

            [JsonIgnore] public int X => Left;
            [JsonIgnore] public int Y => Top;
            [JsonIgnore] public int Width => Right - Left;
            [JsonIgnore] public int Height => Bottom - Top;
            [JsonIgnore] public Point CenterPos => new Point(Width / 2 + X, Height / 2 + Y);

            // === THÊM 2 METHOD NÀY ===
            public bool Contains(int x, int y) => x >= Left && x < Right && y >= Top && y < Bottom;
            public bool Contains(Point p) => Contains(p.X, p.Y);

            public Bounds() : this(0, 0, 0, 0) { }

            public Bounds(int left, int top, int right, int bottom)
            {
                Left = left; Top = top; Right = right; Bottom = bottom;
            }

            public static Bounds CreateSafe(string value)
            { /* giữ nguyên code cũ của bạn */
                try
                {
                    var sp = value.Split(new string[] { "][" }, StringSplitOptions.None);
                    if (sp.Length != 2) return Empty;
                    var tl = sp[0].TrimStart('[').Split(',');
                    var br = sp[1].TrimEnd(']').Split(',');
                    if (tl.Length != 2 || br.Length != 2) return Empty;
                    if (!int.TryParse(tl[0], out int tx)) return Empty;
                    if (!int.TryParse(tl[1], out int ty)) return Empty;
                    if (!int.TryParse(br[0], out int bx)) return Empty;
                    if (!int.TryParse(br[1], out int by)) return Empty;
                    return new Bounds(tx, ty, bx, by);
                }
                catch { return Empty; }
            }
        }

    }
}
