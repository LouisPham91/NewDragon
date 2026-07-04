using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Dragon.DesignView.Public.NormalMode
{
    public class TextBoxNoborberN : TextBox, IThemeable
    {
        private int _borderThickness = 1;

        [Category("Dragon")]
        [DisplayName("DG_BorderThickness")]
        [Description("Độ dày viền vẽ tay")]
        [DefaultValue(1)]
        public int DG_BorderThickness
        {
            get => _borderThickness;
            set { _borderThickness = Math.Max(1, value); Invalidate(); }
        }

        public TextBoxNoborberN()
        {
            // QUAN TRỌNG: bỏ viền mặc định, không dùng UserPaint
            BorderStyle = BorderStyle.None;
            // cho phép vẽ lại khi resize
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;
            // đảm bảo ThemeHelper đã init, nếu null thì dùng màu hệ thống
            try
            {
                BackColor = ThemeHelper.BackNormalFirst;
                ForeColor = ThemeHelper.ForeNormalFirst;
            }
            catch { /* bỏ qua nếu ThemeHelper chưa sẵn */ }
            Invalidate();
        }

        // VẼ VIỀN AN TOÀN - dùng WM_NCPAINT thay vì OnPaintBackground
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            const int WM_NCPAINT = 0x85;
            if (m.Msg == WM_NCPAINT && !IsDisposed && Width > 2 && Height > 2)
            {
                using var g = Graphics.FromHwnd(Handle);
                using var pen = new Pen(Enabled ? (Parent?.BackColor ?? SystemColors.ControlDark) : SystemColors.InactiveBorder, _borderThickness);
                // vẽ sát mép, không dùng -1
                var rect = new Rectangle(0, 0, Width - 1, Height - 1);
                if (rect.Width > 0 && rect.Height > 0)
                {
                    g.DrawRectangle(pen, rect);
                }
            }
        }

        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }
    }
}