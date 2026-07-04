
using Dragon.ControlHelper;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Controller.Server.Model;
using Dragon.DesignView.Public;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dragon.DesignView.ControlUI.Private
{
    public partial class UserThisPC : UserControl
    {
        // 🔥 Theme handler - protected để Designer file truy cập được
        

        public UserThisPC(Computer computer, Color? color)
        {
            InitializeComponent();

            // 🔥 Khởi tạo theme handler NGAY ĐẦU
            
            
            ApplyTheme();

            Changecomputer(computer, color);
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            
        }

        // ⚠️ KHÔNG CÓ method Dispose ở đây - để Designer file xử lý

        public void SapXepControl()
        {
            if (IsDisposed) return;
            UI_Helper.SapXepCenter(panelRoundedWithBorder1);
        }

        public void Changecomputer(Computer computer, Color? colorIcon)
        {
            if (IsDisposed) return;

            if (colorIcon != null)
                this.pictureBoxsvg1.DG_SVGColor = colorIcon;

            labelPCName.Text = computer.MachineName;
            labelStatus.Text = computer.Status;
            LabelExpire.Text = $"Expire: {computer.Expires_At:dd/MM/yyyy}";

            // Sử dụng màu từ ThemeHelper
            if (computer.Status == "active")
            {
                labelStatus.ForeColor = Color.Lime;
            }
            else if (computer.Status == "expired")
            {
                labelStatus.ForeColor = ThemeHelper.ForceColofulFist;
            }
            else if (computer.Status == "trailer")
            {
                labelStatus.ForeColor = Color.Orange;
            }
        }
    }
}