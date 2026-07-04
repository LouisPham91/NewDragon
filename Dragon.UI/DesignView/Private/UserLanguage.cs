
using Dragon.DesignView.Public;

namespace Dragon.DesignView.ControlUI.Private
{
    public partial class UserLanguage : UserControl
    {
        // 🔥 Theme handler - protected để Designer file truy cập được


        public UserLanguage()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();

            // 🔥 Khởi tạo theme handler NGAY ĐẦU


            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;
            BackColor = ThemeHelper.BackNormalFirst;
            foreach (var ctrl in DesignHelper.GetAllControls(this))
            {
                ctrl.ForeColor = ThemeHelper.ForeNormalFirst;
            }


        }

        // ⚠️ KHÔNG CÓ method Dispose ở đây - để Designer file xử lý
    }
}