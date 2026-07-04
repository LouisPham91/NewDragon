

namespace Dragon.DesignView.Public.NormalMode
{
    public class DoubleBufferedFlowLayoutPanel : FlowLayoutPanel, IThemeable
    {
        // 🔥 Theme handler
        

        public DoubleBufferedFlowLayoutPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            UpdateStyles();      

            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            // NormalMode: CHỈ dùng BackNormalFirst và ForeNormalFirst
            BackColor = ThemeHelper.BackNormalFirst;
            ForeColor = ThemeHelper.ForeNormalFirst;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                
            base.Dispose(disposing);
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            if (IsDisposed) return;
            Invalidate();
            base.OnScroll(se);
        }
    }
}