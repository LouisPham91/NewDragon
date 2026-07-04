

namespace Dragon.DesignView.Public.ColorMode
{
    public class labelBox : Label, IThemeable
    {
        public bool IsDrawingLine = false;
        // thêm field
        public labelBox()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
            
             // dùng field
            ApplyTheme();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!IsDrawingLine) return;

            using (Pen pen = new Pen(ForeColor, 2)) // Độ dày và màu của đường kẻ
            {
                // Đường chéo từ góc trên trái đến góc dưới phải
                e.Graphics.DrawLine(pen, 0, 0, Width, Height);

                // Đường chéo từ góc trên phải đến góc dưới trái
                e.Graphics.DrawLine(pen, Width, 0, 0, Height);
            }
        }

        public void ApplyTheme()
        {
            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                BackColor = ThemeHelper.BackNormal3rd;
                if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeRed)
                {
                    ForeColor = ThemeHelper.ForceColofulFist;
                }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGreen)
                {
                    ForeColor = ThemeHelper.ForceColoful2nd;
                }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGold)
                {
                    ForeColor = ThemeHelper.ForceColofulFist;
                }
            }
            else
            {
                BackColor = ThemeHelper.BackColofulFirst;
                if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeRed)
                {
                    ForeColor = ThemeHelper.ForceColofulFist;
                }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGreen)
                {
                    ForeColor = ThemeHelper.ForceColoful2nd;
                }
                else if (ThemeHelper.CurrentStyle == ThemeStyle.ThemeGold)
                {
                    ForeColor = ThemeHelper.ForceColoful3rd;
                }
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            base.Dispose(disposing);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
        }
    }
}
