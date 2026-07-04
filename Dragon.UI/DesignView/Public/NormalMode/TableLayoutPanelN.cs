using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.DesignView.Public.NormalMode
{
    public class TableLayoutPanelN : TableLayoutPanel, IThemeable
    {
        public void ApplyTheme()
        {
            if (IsDisposed) return;

            BackColor = ThemeHelper.BackNormalFirst;

            Invalidate();
        }
    }
}
