using Dragon.ControlHelper;
using Dragon.Controller.Database.Services;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Database.Models;
using Dragon.DesignView.Public;
using System.Drawing.Drawing2D;

namespace Dragon.DesignView.Private
{
    public partial class UserPhoneStatus : UserControl
    {
        public string DeviceID { get; }
        private Phone phone => PhoneRepository.FindOneByDeviceID(DeviceID) ?? throw new InvalidOperationException($"Phone {DeviceID} not found");
        private readonly Size _design = new(540, 960);
        private class M { public float cx, cy, w, h, fs; }
        private readonly Dictionary<Control, M> _map = new();

        public UserPhoneStatus(string deviceID, PhoneStatus phoneStatus)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            DeviceID = deviceID;
            InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.None;
            CaptureMetrics(phoneStatus);
            this.SizeChanged += UserPhoneStatus_SizeChanged;
        }

        // ===== LAYOUT TỰ ĐỘNG =====
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (Width == 0 || Height == 0) return;

            bool isHorizontal = Width > Height;

            if (isHorizontal)
            {
                // ---- NGANG: bỏ 2 label, căn giữa pictureBox ----
                labelTagNumber.Visible = false;
                labelSerialOrModel.Visible = false;

                int pb = (int)(Math.Min(Width, Height) * 0.41f);
                pictureBox1.Size = new Size(pb, pb);
                pictureBox1.Left = (Width - pb) / 2;
                pictureBox1.Top = (int)(Height * 0.22f);

                labelDragonStatus.Visible = true;
                labelDragonStatus.Left = 0;
                labelDragonStatus.Width = Width;
                labelDragonStatus.Top = pictureBox1.Bottom + Height / 25;
                labelDragonStatus.Height = Height / 12;

                var oldFont = labelDragonStatus.Font;
                labelDragonStatus.Font = new Font("Arial", Height * 0.052f, FontStyle.Bold);
 
            }
            else
            {
                // ---- DỌC: trả lại visible, để ApplyMetrics lo ----
                labelTagNumber.Visible = true;
                labelSerialOrModel.Visible = true;
            }
        }

        private void UserPhoneStatus_SizeChanged(object? sender, EventArgs e)
        {
            // chỉ scale theo % khi dọc, ngang đã xử lý ở OnResize
            if (Width <= Height)
            {
                ApplyMetrics();
            }
        }

        // ===== CODE GỐC CỦA BẠN - GIỮ NGUYÊN =====
        public void CaptureMetrics(PhoneStatus phoneStatus)
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

            if (phoneStatus == PhoneStatus.Loading)
            {
                pictureBox1.Image = ImagePhoneStatus.LoadingBitmap;
                labelDragonStatus.Text = "Passionately Loading...";
            }
            else if (phoneStatus == PhoneStatus.Resizing)
            {
                pictureBox1.Image = ImagePhoneStatus.LoadingBitmap;
                labelDragonStatus.Text = "RESIZE PENDING...";
            }
            else if (phoneStatus == PhoneStatus.Disconnect)
            {
                pictureBox1.Image = ImagePhoneStatus.PhoneStatusDict[(PhoneStatus.Disconnect, ThemeHelper.CurrentStyle)];
                labelDragonStatus.Text = "DEVICE OFFLINE";
            }
            else if (phoneStatus == PhoneStatus.Controller)
            {
                pictureBox1.Image = ImagePhoneStatus.PhoneStatusDict[(PhoneStatus.Controller, ThemeHelper.CurrentStyle)];
                labelDragonStatus.Text = "CONTROLLING";
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
                var old = c.Font;
                c.Font = new Font(c.Font.FontFamily, newFs, c.Font.Style);

                if (c is Label lbl) FitText(lbl);
            }
        }

        private void FitText(Label lbl)
        {
            float fs = lbl.Font.Size;
            while (fs > 6)
            {
                var sz = TextRenderer.MeasureText(lbl.Text, lbl.Font);
                if (sz.Width <= lbl.Width && sz.Height <= lbl.Height) break;
                fs -= 0.5f;
                var old = lbl.Font;
                lbl.Font = new Font(lbl.Font.FontFamily, fs, lbl.Font.Style);
        
            }
        }

        public void SetRegion(int BorderRadius, int BorderThickness)
        {
            if (Width > 0 && Height > 0)
            {
                var innerRect = ClientRectangle;
                using (GraphicsPath innerPath = GetRoundedRectanglePath(innerRect, BorderRadius - BorderThickness))
                {
                    Region?.Dispose();
                    Region = new Region(innerPath);
                }
            }
        }

        public static GraphicsPath GetRoundedRectanglePath(RectangleF rect, float radius)
        {
            float d = radius * 2f;
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0f) { path.AddRectangle(rect); return path; }
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
            path.CloseFigure();
            return path;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.SizeChanged -= UserPhoneStatus_SizeChanged;
                Region?.Dispose();
                _map.Clear();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}