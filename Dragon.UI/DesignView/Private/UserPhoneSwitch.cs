

using Dragon.ControlHelper;
using Dragon.Controller.Database.Services;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Database.Models;
using Dragon.DesignView.Public;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace Dragon.DesignView.Private
{
    public partial class UserPhoneSwitch : UserControl
    {
        private readonly Size _design = new(540, 960);
        private class M { public float cx, cy, w, h, fs; }
        private readonly Dictionary<Control, M> _map = new();

        // gọn: hardcode luôn vì bạn không cần đổi runtime
        public const int _radius = 15;
        public const int _borderThickness = 4;

        public UserPhoneSwitch(Phone phone)
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            InitializeComponent();
            Padding = new Padding(_borderThickness);
            AutoSizeChange(phone);
            CaptureMetrics(phone);
            SizeChanged += UserPhoneSwitch_SizeChanged;
            UpdateRegion();
        }

        private void UserPhoneSwitch_SizeChanged(object? sender, EventArgs e)
        {
            UpdateRegion();
            ApplyMetrics();
        }
        public void AutoSizeChange(Phone phone)
        {
            var allsize = GetSettings.GetALLSize(phone);
            Size = new Size(allsize.min.w + _borderThickness, allsize.min.h + _borderThickness);
        }
        public void CaptureMetrics(Phone phone) // trước là Capture()
        {

            var IsModel = GetSettings.GetDisplayModelName();
            Save(labelTagNumber);
            Save(labelSerialOrModel);
            Save(labelDragonStatus);
            Save(pictureBox1);

            ApplyMetrics();

            var redcolor = ThemeHelper.ForceColofulFist;
            labelTagNumber.ForeColor = redcolor;
            labelSerialOrModel.ForeColor = redcolor;
            labelDragonStatus.ForeColor = redcolor;
            labelTagNumber.Text = (phone.PhoneTagNumber < 9) ? "0" + phone.PhoneTagNumber.ToString() : phone.PhoneTagNumber.ToString();
            labelSerialOrModel.Text = phone.PhoneMode.ToString() + " MODE";

            BackColor = ThemeHelper.BackNormalFirst;

            pictureBox1.Image = ImagePhoneStatus.PhoneStatusDict[(PhoneStatus.Controller, ThemeHelper.CurrentStyle)];
            labelDragonStatus.Text = "CONTROLLING";
        }
        private void ApplyMetrics()
        {
            if (_map.Count == 0) return;

            float cw = Width, ch = Height;

            foreach (var kv in _map)
            {
                var c = kv.Key; var m = kv.Value;

                int nw = (int)(m.w * cw);
                int nh = (int)(m.h * ch);
                int nx = (int)(m.cx * cw - nw / 2f);
                int ny = (int)(m.cy * ch - nh / 2f);

                c.Bounds = new Rectangle(nx, ny, nw, nh);

                float newFs = Math.Max(6f, m.fs * ch);
                c.Font = new Font(c.Font.FontFamily, newFs, c.Font.Style);

                if (c is Label lbl) FitText(lbl);
            }
        }
        private void Save(Control c)
        {
            _map[c] = new M
            {
                cx = (c.Left + c.Width / 2f) / _design.Width,
                cy = (c.Top + c.Height / 2f) / _design.Height,
                w = c.Width / (float)_design.Width,
                h = c.Height / (float)_design.Height,
                fs = c.Font.Size / _design.Height
            };
        }
        private void FitText(Label lbl)
        {
            float fs = lbl.Font.Size;
            while (fs > 6)
            {
                var sz = TextRenderer.MeasureText(lbl.Text, lbl.Font);
                if (sz.Width <= lbl.Width && sz.Height <= lbl.Height) break;
                fs -= 0.5f;
                lbl.Font = new Font(lbl.Font.FontFamily, fs, lbl.Font.Style);
            }
        }
        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0) return;
            // Region vẫn là hình bo góc ngoài, để control không còn góc vuông
            using var path = GetRoundedRectanglePath(ClientRectangle, _radius);
            Region?.Dispose();
            Region = new Region(path);
        }
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
        //    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        //    e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality; // quan trọng để không lệch 0.5px

        //    // Vẽ border HOÀN TOÀN bên trong Region
        //    float t = _borderThickness;
        //    // lùi vào nửa độ dày để pen không chạm mép Region
        //    var rect = new RectangleF(t / 2f, t / 2f, Width - t, Height - t);
        //    float r = Math.Max(0, _radius - t / 2f);

        //    using var path = GetRoundedRectanglePath(rect, r);
        //    using var pen = new Pen(ThemeHelper.BorderNormalFist, t)
        //    {
        //        Alignment = PenAlignment.Inset, // ép nét vẽ nằm trong, không bị cắt
        //        LineJoin = LineJoin.Round,
        //        StartCap = LineCap.Round,
        //        EndCap = LineCap.Round
        //    };
        //    e.Graphics.DrawPath(pen, path);
        //}
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var outer = new Rectangle(0, 0, Width, Height);
            var inner = Rectangle.Inflate(outer, -_borderThickness, -_borderThickness);

            using var outerPath = GetRoundedRectanglePath(outer, _radius);
            using var innerPath = GetRoundedRectanglePath(inner, Math.Max(0, _radius - _borderThickness));

            e.Graphics.SetClip(outerPath);
            e.Graphics.ExcludeClip(new Region(innerPath));

            using var brush = new SolidBrush(ThemeHelper.BorderNormalFist);
            e.Graphics.FillPath(brush, outerPath);
            e.Graphics.ResetClip();
        }
        public static GraphicsPath GetRoundedRectanglePath(RectangleF rect, float radius)
        {
            float d = radius * 2f;
            var path = new GraphicsPath();
            if (radius <= 0) { path.AddRectangle(rect); return path; }
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Region?.Dispose();
                _map.Clear();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
