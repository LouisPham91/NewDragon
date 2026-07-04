using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.DesignView.Public.NormalMode
{
    public class PanelTransparent : Panel, IThemeable
    {
        public PanelTransparent()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);

            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;
            BackColor = Color.Transparent;
        }

    }
}
