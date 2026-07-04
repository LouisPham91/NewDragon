
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;
using System;
using System.Windows.Forms;

namespace Dragon.DesignView.ControlUI.Private
{
    public partial class UserPCRecognized : UserControl
    {
        // 🔥 Theme handler - protected để Designer file truy cập được
        

        public UserPCRecognized()
        {
            InitializeComponent();

            // 🔥 Khởi tạo theme handler NGAY ĐẦU
            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            

            // Cập nhật màu đặc biệt cho panel
            panelRoundedWithBorder1.DG_BorderColor = Color.FromArgb(255, 128, 0); // Màu cam cảnh báo
        }

        // ⚠️ KHÔNG CÓ method Dispose ở đây - để Designer file xử lý

        private void UserPCRecognized_Load(object sender, EventArgs e)
        {
            if (IsDisposed) return;
            pictureBoxsvg1.RenderSvg();
        }
    }
}