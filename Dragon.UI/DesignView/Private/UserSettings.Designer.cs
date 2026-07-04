
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.ControlUI.Private
{
    partial class UserSettings
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
            if (disposing)
            {
                // 🔥 SỬA: Unsubscribe đúng cách
                

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserSettings));
            tableLayoutPanel2 = new TableLayoutPanel();
            AlabelNormaln19 = new LabelNormalN();
            AlabelNormaln16 = new LabelNormalN();
            AlabelNormaln2 = new LabelNormalN();
            AlabelNormaln3 = new LabelNormalN();
            toggleDisplayTitle = new Dragon.DesignView.Public.ColorMode.ToggleWithTextControl();
            toggleDisplayModel = new Dragon.DesignView.Public.ColorMode.ToggleWithTextControl();
            toggleDisplayIP = new Dragon.DesignView.Public.ColorMode.ToggleWithTextControl();
            toggleSerial = new Dragon.DesignView.Public.ColorMode.ToggleWithTextControl();
            labelTitle = new LabelNormalN();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 192F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 23F));
            tableLayoutPanel2.Controls.Add(AlabelNormaln19, 1, 4);
            tableLayoutPanel2.Controls.Add(AlabelNormaln16, 1, 1);
            tableLayoutPanel2.Controls.Add(AlabelNormaln2, 1, 2);
            tableLayoutPanel2.Controls.Add(AlabelNormaln3, 1, 3);
            tableLayoutPanel2.Controls.Add(toggleDisplayTitle, 0, 1);
            tableLayoutPanel2.Controls.Add(toggleDisplayModel, 0, 2);
            tableLayoutPanel2.Controls.Add(toggleDisplayIP, 0, 3);
            tableLayoutPanel2.Controls.Add(toggleSerial, 0, 4);
            tableLayoutPanel2.Controls.Add(labelTitle, 0, 0);
            tableLayoutPanel2.Location = new Point(28, 29);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 5;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.Size = new Size(220, 167);
            tableLayoutPanel2.TabIndex = 15;
            // 
            // AlabelNormaln19
            // 
            AlabelNormaln19.AutoSize = true;
            AlabelNormaln19.BackColor = Color.Transparent;
            AlabelNormaln19.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln19.DG_SVGString = resources.GetString("AlabelNormaln19.DG_SVGString");
            AlabelNormaln19.Enabled = false;
            AlabelNormaln19.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln19.ForeColor = Color.Black;
            AlabelNormaln19.Image = (Image)resources.GetObject("AlabelNormaln19.Image");
            AlabelNormaln19.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln19.Location = new Point(195, 133);
            AlabelNormaln19.Margin = new Padding(3, 5, 3, 0);
            AlabelNormaln19.Name = "AlabelNormaln19";
            AlabelNormaln19.Size = new Size(22, 15);
            AlabelNormaln19.TabIndex = 20;
            AlabelNormaln19.Text = "     ";
            AlabelNormaln19.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln16
            // 
            AlabelNormaln16.AutoSize = true;
            AlabelNormaln16.BackColor = Color.Transparent;
            AlabelNormaln16.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln16.DG_SVGString = resources.GetString("AlabelNormaln16.DG_SVGString");
            AlabelNormaln16.Enabled = false;
            AlabelNormaln16.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln16.ForeColor = Color.Black;
            AlabelNormaln16.Image = (Image)resources.GetObject("AlabelNormaln16.Image");
            AlabelNormaln16.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln16.Location = new Point(195, 37);
            AlabelNormaln16.Margin = new Padding(3, 5, 3, 0);
            AlabelNormaln16.Name = "AlabelNormaln16";
            AlabelNormaln16.Size = new Size(22, 15);
            AlabelNormaln16.TabIndex = 16;
            AlabelNormaln16.Text = "     ";
            AlabelNormaln16.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln2
            // 
            AlabelNormaln2.AutoSize = true;
            AlabelNormaln2.BackColor = Color.Transparent;
            AlabelNormaln2.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln2.DG_SVGString = resources.GetString("AlabelNormaln2.DG_SVGString");
            AlabelNormaln2.Enabled = false;
            AlabelNormaln2.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln2.ForeColor = Color.Black;
            AlabelNormaln2.Image = (Image)resources.GetObject("AlabelNormaln2.Image");
            AlabelNormaln2.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln2.Location = new Point(195, 69);
            AlabelNormaln2.Margin = new Padding(3, 5, 3, 0);
            AlabelNormaln2.Name = "AlabelNormaln2";
            AlabelNormaln2.Size = new Size(22, 15);
            AlabelNormaln2.TabIndex = 17;
            AlabelNormaln2.Text = "     ";
            AlabelNormaln2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln3
            // 
            AlabelNormaln3.AutoSize = true;
            AlabelNormaln3.BackColor = Color.Transparent;
            AlabelNormaln3.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln3.DG_SVGString = resources.GetString("AlabelNormaln3.DG_SVGString");
            AlabelNormaln3.Enabled = false;
            AlabelNormaln3.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln3.ForeColor = Color.Black;
            AlabelNormaln3.Image = (Image)resources.GetObject("AlabelNormaln3.Image");
            AlabelNormaln3.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln3.Location = new Point(195, 101);
            AlabelNormaln3.Margin = new Padding(3, 5, 3, 0);
            AlabelNormaln3.Name = "AlabelNormaln3";
            AlabelNormaln3.Size = new Size(22, 15);
            AlabelNormaln3.TabIndex = 18;
            AlabelNormaln3.Text = "     ";
            AlabelNormaln3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // toggleDisplayTitle
            // 
            toggleDisplayTitle.BackColor = Color.Transparent;
            toggleDisplayTitle.DG_BackgroundColor = Color.Empty;
            toggleDisplayTitle.DG_KnobColor = Color.White;
            toggleDisplayTitle.DG_LabelColor = Color.Gainsboro;
            toggleDisplayTitle.DG_LabelText = "Display Title";
            toggleDisplayTitle.DG_ToggleColorClick = Color.FromArgb(207, 66, 51);
            toggleDisplayTitle.DG_ToggleColorNormal = Color.FromArgb(120, 120, 120);
            toggleDisplayTitle.Location = new Point(3, 35);
            toggleDisplayTitle.Name = "toggleDisplayTitle";
            toggleDisplayTitle.Size = new Size(144, 25);
            toggleDisplayTitle.TabIndex = 28;
            // 
            // toggleDisplayModel
            // 
            toggleDisplayModel.BackColor = Color.Transparent;
            toggleDisplayModel.DG_BackgroundColor = Color.Empty;
            toggleDisplayModel.DG_KnobColor = Color.White;
            toggleDisplayModel.DG_LabelColor = Color.Gainsboro;
            toggleDisplayModel.DG_LabelText = "Display Model";
            toggleDisplayModel.DG_ToggleColorClick = Color.FromArgb(207, 66, 51);
            toggleDisplayModel.DG_ToggleColorNormal = Color.FromArgb(120, 120, 120);
            toggleDisplayModel.Location = new Point(3, 67);
            toggleDisplayModel.Name = "toggleDisplayModel";
            toggleDisplayModel.Size = new Size(144, 25);
            toggleDisplayModel.TabIndex = 15;
            // 
            // toggleDisplayIP
            // 
            toggleDisplayIP.BackColor = Color.Transparent;
            toggleDisplayIP.DG_BackgroundColor = Color.Empty;
            toggleDisplayIP.DG_KnobColor = Color.White;
            toggleDisplayIP.DG_LabelColor = Color.Gainsboro;
            toggleDisplayIP.DG_LabelText = "Display IP";
            toggleDisplayIP.DG_ToggleColorClick = Color.FromArgb(207, 66, 51);
            toggleDisplayIP.DG_ToggleColorNormal = Color.FromArgb(120, 120, 120);
            toggleDisplayIP.Location = new Point(3, 99);
            toggleDisplayIP.Name = "toggleDisplayIP";
            toggleDisplayIP.Size = new Size(144, 25);
            toggleDisplayIP.TabIndex = 18;
            // 
            // toggleSerial
            // 
            toggleSerial.BackColor = Color.Transparent;
            toggleSerial.DG_BackgroundColor = Color.Empty;
            toggleSerial.DG_KnobColor = Color.White;
            toggleSerial.DG_LabelColor = Color.Gainsboro;
            toggleSerial.DG_LabelText = "Display Serial";
            toggleSerial.DG_ToggleColorClick = Color.FromArgb(207, 66, 51);
            toggleSerial.DG_ToggleColorNormal = Color.FromArgb(120, 120, 120);
            toggleSerial.Location = new Point(3, 131);
            toggleSerial.Name = "toggleSerial";
            toggleSerial.Size = new Size(144, 25);
            toggleSerial.TabIndex = 17;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.BackColor = Color.Transparent;
            labelTitle.DG_SVGString = resources.GetString("labelTitle.DG_SVGString");
            labelTitle.Dock = DockStyle.Fill;
            labelTitle.ForeColor = Color.White;
            labelTitle.Image = (Image)resources.GetObject("labelTitle.Image");
            labelTitle.ImageAlign = ContentAlignment.MiddleLeft;
            labelTitle.Location = new Point(3, 3);
            labelTitle.Margin = new Padding(3);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(186, 26);
            labelTitle.TabIndex = 29;
            labelTitle.Text = "Display Phone Title";
            labelTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // UserSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            Controls.Add(tableLayoutPanel2);
            Margin = new Padding(3, 2, 3, 2);
            Name = "UserSettings";
            Size = new Size(930, 543);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel2;
        private LabelNormalN AlabelNormaln19;
        private LabelNormalN AlabelNormaln16;
        private LabelNormalN AlabelNormaln2;
        private LabelNormalN AlabelNormaln3;
        private Public.ColorMode.ToggleWithTextControl toggleDisplayTitle;
        private Public.ColorMode.ToggleWithTextControl toggleDisplayModel;
        private Public.ColorMode.ToggleWithTextControl toggleDisplayIP;
        private Public.ColorMode.ToggleWithTextControl toggleSerial;
        private LabelNormalN labelTitle;
    }
}