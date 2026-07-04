
using Dragon.ControlHelper.ScrcpyNet.DesignDevice;
using Dragon.ControlHelper.UIController;
using Dragon.Controller.GlobalControl.Setting;
using Dragon.DesignView.ControlUI.Private;
using Dragon.DesignView.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dragon.DesignView.FormUI
{
    public partial class FormOriginal : Form
    {
        private System.Windows.Forms.Timer _resizeTimer;
        public Size originalSize;
        public Point originalLocation;
        public bool isMaximized = false;

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private Color _backColor = Color.White;
        private Color _borderColor = Color.FromArgb(168, 168, 168);
        private float _roundRadius = 15f;
        private int _padding = 1;

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BackColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                this.panelMain.BackColor = value;
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                this.BackColor = value;
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_RoundRadius
        {
            get => _roundRadius;
            set
            {
                _roundRadius = value;
                SetRoundedRegion();
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_Padding
        {
            get => _padding;
            set => UpdatePadding(value);
        }

        private void UpdatePadding(int value)
        {
            _padding = value;
            Padding = new Padding(value);
            SetPanelMainRoundedRegion();
        }
        public FormOriginal()
        {

            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            InitializeComponent();

            _resizeTimer = new System.Windows.Forms.Timer { Interval = 150 };
            _resizeTimer.Tick += (s, e) => {
                _resizeTimer.Stop();
                // gọi helper
                FormManager.Instance.virtualFlowHelper?.HandleScrollFinished();
            };
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _resizeTimer?.Stop();
                _resizeTimer?.Dispose();
            }
            if (disposing && (components != null))
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                
                LoadPicture();
            }
            base.OnLoad(e);
        }
        public void LoadPicture()
        {
            pictureBoxIcon.Image = DesignHelper.GetRandomLogo();
            this.Icon = GetIcon.GetIconByName("DragonHappy");
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Padding = new Padding(_padding);
            SetRoundedRegion();
            SetPanelMainRoundedRegion();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            panelMain.Size = new Size(ClientSize.Width - _padding * 2, ClientSize.Height - _padding * 2);
            SetRoundedRegion();
            SetPanelMainRoundedRegion();

            // <-- THÊM
            if (!DesignMode && IsHandleCreated)
            {
                _resizeTimer.Stop();
                _resizeTimer.Start(); // đợi user thả chuột xong mới update
            }
        }
        private void SetRoundedRegion()
        {
            using (var path = GetRoundedRectanglePath(ClientRectangle, _roundRadius))
            {
                var region = new Region(path);
                var old = Region;

                Region = region;
                old?.Dispose();
            }
        }
        private void SetPanelMainRoundedRegion()
        {
            using (var path = GetRoundedRectanglePath(panelMain.ClientRectangle, _roundRadius))
            {
                var region = new Region(path);
                var old = panelMain.Region;

                panelMain.Region = region;
                old?.Dispose();
            }
        }
        public static GraphicsPath GetRoundedRectanglePath(RectangleF rect, float radius)
        {
            float d = radius * 2f;
            var path = new GraphicsPath();
            if (radius <= 0) { path.AddRectangle(rect); return path; }

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

        public void UpdateOriginalState()
        {
            originalSize = this.Size;
            originalLocation = this.Location;
        }
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            if (!isMaximized && this.WindowState == FormWindowState.Normal)
            {
                UpdateOriginalState();
            }

            // <-- THÊM
            if (!DesignMode && IsHandleCreated)
            {
                _resizeTimer.Stop();
                _resizeTimer.Start(); // đợi user thả chuột xong mới update
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

        protected virtual void OnCloseFormClicked(EventArgs e)
        {
            this.Close();
        }

        protected virtual void OnMinimizeFormClicked(EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        protected virtual void OnFullScreenFormClicked(EventArgs e)
        {
            // Ví dụ chuyển đổi giữa full screen và normal
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        // Các event handler được đăng ký sẵn, chỉ việc chuyển tiếp tới các phương thức ảo ở trên.
        private void PictureBoxCloseForm_Click(object sender, EventArgs e)
        {
            OnCloseFormClicked(e);
        }

        private void PictureBoxMiniminze_Click(object sender, EventArgs e)
        {
            OnMinimizeFormClicked(e);
        }

        private void PictureBoxFullScreen_Click(object sender, EventArgs e)
        {
            OnFullScreenFormClicked(e);
        }


    }
}
