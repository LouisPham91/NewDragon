

using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;


namespace Dragon.DesignView.FormUI
{

    [Designer(typeof(System.Windows.Forms.Design.ParentControlDesigner))] // Cho phép kéo thả control vào FormRound khi kế thừa
    public partial class FormRound : Form
    {
        public Size originalSize; // Lưu kích thước ban đầu
        public Point originalLocation; // Lưu vị trí ban đầu
        public bool isMaximized = false; // Cờ trạng thái

        // Các tham số cấu hình border
        private int borderRadius = 15;
        private int borderWidth = 2;

        private Color borderColor = Color.FromArgb(61, 61, 61);
        protected Panel innerPanel;

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        [Category("Appearance")]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = Math.Max(0, value);
                Padding = new Padding(borderWidth);
                if (innerPanel != null)
                    innerPanel.Margin = new Padding(borderWidth);
                SetRoundedRegion();
                UpdateInnerPanelRegion();
                if (IsHandleCreated) Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Empty")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                BackColor = value; // dùng value thay vì BorderColor để tránh gọi getter
                if (IsHandleCreated) Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius
        {
            get => borderRadius;
            set
            {
                if (borderRadius == value) return;
                borderRadius = Math.Max(0, value);
                Padding = new Padding(borderWidth);
                if (innerPanel != null)
                    innerPanel.Margin = new Padding(borderWidth);
                SetRoundedRegion();
                UpdateInnerPanelRegion();
                if (IsHandleCreated) Invalidate();
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color InnerPanelBackColor
        {
            get => innerPanel?.BackColor ?? Color.Empty;
            set
            {
                if (innerPanel == null) return;
                if (innerPanel.BackColor == value) return;
                innerPanel.BackColor = value;
                innerPanel.Invalidate();
            }
        }
        public Panel getInnerPanel()
        {
            return innerPanel;
        }
        public FormRound()
        {
            SetStyle(
              
              ControlStyles.DoubleBuffer |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();


            InitializeComponent();


            // Thiết lập High DPI
            this.AutoScaleMode = AutoScaleMode.Dpi;
            // Loại bỏ viền mặc định để hiển thị border custom
            this.FormBorderStyle = FormBorderStyle.None;
            // Đảm bảo nội dung không che mất border vẽ sẵn
            this.Padding = new Padding(borderWidth);
            this.StartPosition = FormStartPosition.Manual;

            // Khởi tạo Region ban đầu với border bo tròn
            SetRoundedRegion();

            // Tạo innerPanel dựa theo tiêu chí từ BorderedPanel:
            innerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(borderWidth),
            };

            // Khi innerPanel được nhấn chuột, gọi hàm kéo Form
            innerPanel.MouseDown += InnerPanel_MouseDown;
            innerPanel.SizeChanged += InnerPanel_SizeChanged;

            base.Controls.Add(innerPanel);

            UpdateInnerPanelRegion();
            this.LocationChanged += FormRound_LocationChanged;
            this.SizeChanged += FormRound_SizeChanged;

        }

        private void FormRound_SizeChanged(object? sender, EventArgs e)
        {
            if (!isMaximized && this.WindowState == FormWindowState.Normal)
            {
                UpdateOriginalState();
            }
        }

        private void FormRound_LocationChanged(object? sender, EventArgs e)
        {
            if (!isMaximized && this.WindowState == FormWindowState.Normal)
            {
                UpdateOriginalState();
            }
        }

        public void UpdateOriginalState()
        {
            originalSize = this.Size;
            originalLocation = this.Location;
        }

        private void FormRound_Load(object sender, EventArgs e)
        {
            var screen = Screen.PrimaryScreen;
            if (screen is not null)
            {
                Rectangle workingArea = screen.WorkingArea;
                this.Location = new Point(
                    workingArea.Left + (workingArea.Width - this.Width) / 2,
                    workingArea.Top + (workingArea.Height - this.Height) / 2);
            }
        }

        private void InnerPanel_SizeChanged(object? sender, EventArgs e)
        {
            UpdateInnerPanelRegion();
        }

        private void UpdateInnerPanelRegion()
        {
            if (innerPanel.Width <= 0 || innerPanel.Height <= 0)
                return;

            int innerRadius = Math.Max(0, borderRadius - borderWidth);
            Rectangle rect = new Rectangle(0, 0, innerPanel.Width, innerPanel.Height);
            using (GraphicsPath path = GetRoundedRectanglePath(rect, innerRadius))
            {
                if (innerPanel.Region != null)
                    innerPanel.Region.Dispose();

                innerPanel.Region = new Region(path);
            }
        }

        private void InnerPanel_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            using (GraphicsPath path = GetRoundedRectanglePath(rect, borderRadius))
            using (Pen pen = new Pen(borderColor, borderWidth) { Alignment = PenAlignment.Inset })
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetRoundedRegion();
        }
        private void SetRoundedRegion()
        {
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            using (GraphicsPath path = GetRoundedRectanglePath(rect, borderRadius))
            {
                if (this.Region != null)
                    this.Region.Dispose();
                this.Region = new Region(path);
            }
        }
        public void PanelMouseDown(object? sender, MouseEventArgs e)
        {
            if (sender != null && e.Button == MouseButtons.Left)
            {
                if (e.Clicks == 2) // Xử lý double-click
                {
                    if (!isMaximized)
                    {
                        UpdateOriginalState(); // Lưu trạng thái hiện tại
                        this.WindowState = FormWindowState.Maximized;
                        isMaximized = true;
                    }
                    else
                    {
                        this.WindowState = FormWindowState.Normal;
                        this.Size = originalSize;
                        this.Location = originalLocation;
                        isMaximized = false;
                    }
                }
                else if (e.Clicks == 1) // Xử lý click đơn (kéo form)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            }
        }



        public static GraphicsPath GetRoundedRectanglePath(RectangleF rect, float radius)
        {
            float d = radius * 2f;
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0f)
            {
                path.AddRectangle(rect);
                return path;
            }

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

        // Cơ chế resize khi chuột ở bốn góc
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            // Các giá trị hit test để báo cho hệ thống biết vùng resize
            const int HTLEFT = 10;
            const int HTRIGHT = 11;
            const int HTTOP = 12;
            const int HTTOPLEFT = 13;
            const int HTTOPRIGHT = 14;
            const int HTBOTTOM = 15;
            const int HTBOTTOMLEFT = 16;
            const int HTBOTTOMRIGHT = 17;

            // Độ rộng vùng border dùng để resize (có thể điều chỉnh)
            int resizeAreaSize = 10;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                if ((int)m.Result == 1) // 1 tương đương với HTCLIENT
                {
                    Point cursorPoint = PointToClient(Cursor.Position);

                    // Kiểm tra vùng bên trái
                    if (cursorPoint.X <= resizeAreaSize)
                    {
                        // Góc trên bên trái
                        if (cursorPoint.Y <= resizeAreaSize)
                        {
                            m.Result = (IntPtr)HTTOPLEFT;
                            return;
                        }
                        // Góc dưới bên trái
                        else if (cursorPoint.Y >= ClientSize.Height - resizeAreaSize)
                        {
                            m.Result = (IntPtr)HTBOTTOMLEFT;
                            return;
                        }
                        else
                        {
                            m.Result = (IntPtr)HTLEFT;
                            return;
                        }
                    }
                    // Kiểm tra vùng bên phải
                    if (cursorPoint.X >= ClientSize.Width - resizeAreaSize)
                    {
                        if (cursorPoint.Y <= resizeAreaSize)
                        {
                            m.Result = (IntPtr)HTTOPRIGHT;
                            return;
                        }
                        else if (cursorPoint.Y >= ClientSize.Height - resizeAreaSize)
                        {
                            m.Result = (IntPtr)HTBOTTOMRIGHT;
                            return;
                        }
                        else
                        {
                            m.Result = (IntPtr)HTRIGHT;
                            return;
                        }
                    }
                    // Kiểm tra vùng trên cùng
                    if (cursorPoint.Y <= resizeAreaSize)
                    {
                        m.Result = (IntPtr)HTTOP;
                        return;
                    }
                    // Kiểm tra vùng dưới cùng
                    if (cursorPoint.Y >= ClientSize.Height - resizeAreaSize)
                    {
                        m.Result = (IntPtr)HTBOTTOM;
                        return;
                    }
                }
                return;
            }

            base.WndProc(ref m);
        }
    }


}
