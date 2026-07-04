
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;
using System;
using System.Windows.Forms;

namespace Dragon.DesignView.ControlUI.Private
{
    public partial class UserOTP : UserControl
    {
        // 🔥 Theme handler - protected để Designer file truy cập được
        

        public UserOTP()
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |  ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            // 🔥 Khởi tạo theme handler NGAY ĐẦU
            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;
            BackColor = ThemeHelper.BackNormalFirst;
            
        }

        // ⚠️ KHÔNG CÓ method Dispose ở đây - để Designer file xử lý
    }
}