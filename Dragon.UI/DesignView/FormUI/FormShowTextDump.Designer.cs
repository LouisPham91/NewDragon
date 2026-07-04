namespace Dragon.DesignView.FormUI
{
    partial class FormShowTextDump
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            richTextBox1 = new RichTextBox();
            panelMain.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.Controls.Add(richTextBox1);
            panelMain.Location = new Point(1, 1);
            panelMain.Size = new Size(796, 533);
            panelMain.Controls.SetChildIndex(richTextBox1, 0);
            // 
            // labelTitle
            // 
            labelTitle.ForeColor = SystemColors.ControlText;
            // 
            // richTextBox1
            // 
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Location = new Point(0, 34);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(796, 499);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // FormShowTextDump
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(798, 535);
            Name = "FormShowTextDump";
            Padding = new Padding(1);
            Text = "FormShowTextDump";
            panelMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
    }
}