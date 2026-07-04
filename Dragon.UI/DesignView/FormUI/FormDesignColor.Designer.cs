using Dragon.DesignView.Public.ColorMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormDesignColor
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
            panel1 = new Panel();
            button1 = new Button();
            panel2 = new Panel();
            button2 = new Button();
            panelColorc1 = new PanelColorC();
            panelColorc2 = new PanelColorC();
            label1 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(button1);
            panel1.Location = new Point(26, 122);
            panel1.Name = "panel1";
            panel1.Size = new Size(420, 420);
            panel1.TabIndex = 0;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(97, 66, 51);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.FromArgb(215, 66, 51);
            button1.Location = new Point(166, 192);
            button1.Name = "button1";
            button1.Size = new Size(87, 34);
            button1.TabIndex = 2;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(40, 40, 40);
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(button2);
            panel2.Location = new Point(452, 122);
            panel2.Name = "panel2";
            panel2.Size = new Size(420, 420);
            panel2.TabIndex = 1;
            panel2.Paint += panel2_Paint;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(97, 66, 51);
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.ForeColor = Color.FromArgb(215, 66, 51);
            button2.Location = new Point(166, 192);
            button2.Name = "button2";
            button2.Size = new Size(87, 34);
            button2.TabIndex = 2;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = false;
            // 
            // panelColorc1
            // 
            panelColorc1.BorderStyle = BorderStyle.FixedSingle;
            panelColorc1.Location = new Point(39, 31);
            panelColorc1.Name = "panelColorc1";
            panelColorc1.Size = new Size(37, 31);
            panelColorc1.TabIndex = 2;
            panelColorc1.BackColorChanged += panelColorc1_BackColorChanged;
            // 
            // panelColorc2
            // 
            panelColorc2.BackColor = Color.FromArgb(40, 40, 40);
            panelColorc2.BorderStyle = BorderStyle.FixedSingle;
            panelColorc2.Location = new Point(93, 30);
            panelColorc2.Name = "panelColorc2";
            panelColorc2.Size = new Size(37, 31);
            panelColorc2.TabIndex = 3;
            panelColorc2.BackColorChanged += panelColorc2_BackColorChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(39, 13);
            label1.Name = "label1";
            label1.Size = new Size(34, 15);
            label1.TabIndex = 4;
            label1.Text = "Light";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(93, 13);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 5;
            label2.Text = "Dark";
            // 
            // FormDesignColor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 248, 248);
            ClientSize = new Size(1267, 820);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(panelColorc2);
            Controls.Add(panelColorc1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "FormDesignColor";
            Text = "FormDesignColor";
            Load += FormDesignColor_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Button button1;
        private Button button2;
        private PanelColorC panelColorc1;
        private PanelColorC panelColorc2;
        private Label label1;
        private Label label2;
    }
}