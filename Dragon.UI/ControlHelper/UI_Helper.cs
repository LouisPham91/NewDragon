using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.ControlHelper
{
    public class UI_Helper
    {
        public static void SapXepCenter(Control control)
        {
            if (control.Parent != null)
            {
                int x = (control.Parent.Width - control.Width) / 2;
                int y = (control.Parent.Width - control.Height) / 2;
                control.Location = new Point(x, control.Location.Y);
            }
        }
    }
}
