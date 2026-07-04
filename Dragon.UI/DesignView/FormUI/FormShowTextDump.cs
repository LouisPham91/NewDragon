using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dragon.DesignView.FormUI
{
    public partial class FormShowTextDump : FormOriginal
    {
        public FormShowTextDump()
        {
            InitializeComponent();
        }

        public void SetText(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(SetText), text);
                return;
            }

            this.richTextBox1.Text = text;
        }
    }
}
