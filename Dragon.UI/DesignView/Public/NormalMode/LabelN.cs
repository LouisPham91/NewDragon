using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.DesignView.Public.NormalMode
{
    public class LabelN : Label, IThemeable
    {
        public void ApplyTheme()
        {
            if (IsDisposed) return;

            BackColor = Color.Transparent;
            ForeColor = ThemeHelper.ForeNormalFirst;

            Invalidate();
        }
    }
}
