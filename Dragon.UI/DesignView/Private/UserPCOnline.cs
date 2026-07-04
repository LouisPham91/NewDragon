
using Dragon.Controller.GlobalControl.Property;
using Dragon.Controller.Server.Model;
using Dragon.DesignView.Public;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dragon.DesignView.ControlUI.Private
{
    public partial class UserPCOnline : UserControl
    {
        public bool IsSelected { get; private set; } = false;
        public ComputerOnline? computerOnline = null;

        // 🔥 Theme handler - protected để Designer file truy cập được
        

        public UserPCOnline(ComputerOnline computerOnline, Color? color)
        {
            InitializeComponent();

            // 🔥 Khởi tạo theme handler NGAY ĐẦU
            
            
            ApplyTheme();

            this.computerOnline = computerOnline;
            Changecomputer(computerOnline, color);
            DoubleBuffered = true;
            tableLayoutPanel1.Click += TableLayoutPanel1_Click;
            panelRoundedWithBorder1.Click += TableLayoutPanel1_Click;
            this.Click += TableLayoutPanel1_Click;
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            

            // Cập nhật màu border cho panel khi được chọn
            if (IsSelected)
            {
                panelRoundedWithBorder1.DG_BorderColor = ThemeHelper.ForceColofulFist;
            }
            else
            {
                panelRoundedWithBorder1.DG_BorderColor = ThemeHelper.BorderNormalFist;
            }
        }

        // ⚠️ KHÔNG CÓ method Dispose ở đây - để Designer file xử lý

        public event EventHandler? Selected;

        private void TableLayoutPanel1_Click(object? sender, EventArgs e)
        {
            Selected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Đặt trạng thái selected
        /// </summary>
        public void SetSelected(bool selected)
        {
            IsSelected = selected;

            // Cập nhật màu border
            if (IsSelected)
            {
                panelRoundedWithBorder1.DG_BorderColor = ThemeHelper.ForceColofulFist;
            }
            else
            {
                panelRoundedWithBorder1.DG_BorderColor = ThemeHelper.BorderNormalFist;
            }

            Invalidate();
        }

        /// <summary>
        /// Vẽ border khi control được chọn
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDisposed) return;
            base.OnPaint(e);

            // Border đã được vẽ bởi PanelRoundedWithBorder, không cần vẽ thêm
        }

        /// <summary>
        /// Cập nhật thông tin máy tính
        /// </summary>
        public void Changecomputer(ComputerOnline computer, Color? colorIcon)
        {
            if (IsDisposed) return;

            if (colorIcon != null)
                this.pictureBoxsvg1.DG_SVGColor = colorIcon;

            labelPCName.Text = computer.MachineName;
            labelStatus.Text = computer.IsOnline ? "Online" : "Offline";
            LabelExpire.Text = $"Expire: {computer.Expires_At:dd/MM/yyyy}";

            // Sử dụng màu từ ThemeHelper
            labelStatus.ForeColor = computer.IsOnline
                ? Color.Lime
                : ThemeHelper.ForceColofulFist;
        }
    }
}