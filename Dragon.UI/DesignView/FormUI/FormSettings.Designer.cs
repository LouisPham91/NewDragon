
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormSettings
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
                ThemeHelper.ThemeChanged -= OnGlobalThemeChanged;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            panelLineBordernc1 = new PanelLineBorderNC();
            buttomLanguage = new ButtomKeep();
            buttomThemeColor = new ButtomKeep();
            panelRightMain = new PanelNormalN();
            buttomSetting = new ButtomKeep();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).BeginInit();
            panelLineBordernc1.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.Controls.Add(panelRightMain);
            panelMain.Controls.Add(panelLineBordernc1);
            panelMain.Location = new Point(1, 1);
            panelMain.Size = new Size(896, 765);
            panelMain.Controls.SetChildIndex(panelLineBordernc1, 0);
            panelMain.Controls.SetChildIndex(panelRightMain, 0);
            // 
            // labelTitle
            // 
            labelTitle.ForeColor = SystemColors.ControlText;
            labelTitle.Size = new Size(58, 17);
            labelTitle.Text = "Settings";
            // 
            // panelLineBordernc1
            // 
            panelLineBordernc1.BackColor = Color.WhiteSmoke;
            panelLineBordernc1.Controls.Add(buttomSetting);
            panelLineBordernc1.Controls.Add(buttomLanguage);
            panelLineBordernc1.Controls.Add(buttomThemeColor);
            panelLineBordernc1.DG_DrawRight = true;
            panelLineBordernc1.DG_IsSameLineColor = true;
            panelLineBordernc1.DG_IsUseBackNormal = true;
            panelLineBordernc1.Dock = DockStyle.Left;
            panelLineBordernc1.Location = new Point(0, 34);
            panelLineBordernc1.Name = "panelLineBordernc1";
            panelLineBordernc1.Padding = new Padding(2);
            panelLineBordernc1.Size = new Size(146, 731);
            panelLineBordernc1.TabIndex = 1;
            // 
            // buttomLanguage
            // 
            buttomLanguage.BackColor = Color.WhiteSmoke;
            buttomLanguage.DG_ForceColorNormal = Color.FromArgb(40, 40, 40);
            buttomLanguage.DG_SVGString = resources.GetString("buttomLanguage.DG_SVGString");
            buttomLanguage.Dock = DockStyle.Top;
            buttomLanguage.FlatAppearance.BorderSize = 0;
            buttomLanguage.FlatStyle = FlatStyle.Flat;
            buttomLanguage.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttomLanguage.ForeColor = Color.FromArgb(40, 40, 40);
            buttomLanguage.Image = (Image)resources.GetObject("buttomLanguage.Image");
            buttomLanguage.ImageAlign = ContentAlignment.MiddleLeft;
            buttomLanguage.Location = new Point(2, 25);
            buttomLanguage.Name = "buttomLanguage";
            buttomLanguage.Size = new Size(142, 23);
            buttomLanguage.TabIndex = 4;
            buttomLanguage.TabStop = false;
            buttomLanguage.Text = "Language";
            buttomLanguage.UseVisualStyleBackColor = false;
            buttomLanguage.Click += buttomLanguage_Click;
            // 
            // buttomThemeColor
            // 
            buttomThemeColor.BackColor = Color.WhiteSmoke;
            buttomThemeColor.DG_ForceColorNormal = Color.FromArgb(40, 40, 40);
            buttomThemeColor.DG_SVGString = resources.GetString("buttomThemeColor.DG_SVGString");
            buttomThemeColor.Dock = DockStyle.Top;
            buttomThemeColor.FlatAppearance.BorderSize = 0;
            buttomThemeColor.FlatStyle = FlatStyle.Flat;
            buttomThemeColor.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttomThemeColor.ForeColor = Color.FromArgb(40, 40, 40);
            buttomThemeColor.Image = (Image)resources.GetObject("buttomThemeColor.Image");
            buttomThemeColor.ImageAlign = ContentAlignment.MiddleLeft;
            buttomThemeColor.Location = new Point(2, 2);
            buttomThemeColor.Name = "buttomThemeColor";
            buttomThemeColor.Size = new Size(142, 23);
            buttomThemeColor.TabIndex = 3;
            buttomThemeColor.TabStop = false;
            buttomThemeColor.Text = "Theme Color";
            buttomThemeColor.UseVisualStyleBackColor = false;
            buttomThemeColor.Click += buttomThemeColor_Click;
            // 
            // panelRightMain
            // 
            panelRightMain.BackColor = Color.WhiteSmoke;
            panelRightMain.Dock = DockStyle.Fill;
            panelRightMain.ForeColor = Color.Black;
            panelRightMain.Location = new Point(146, 34);
            panelRightMain.Name = "panelRightMain";
            panelRightMain.Size = new Size(750, 731);
            panelRightMain.TabIndex = 2;
            // 
            // buttomSetting
            // 
            buttomSetting.BackColor = Color.WhiteSmoke;
            buttomSetting.DG_ForceColorNormal = Color.FromArgb(40, 40, 40);
            buttomSetting.DG_SVGString = resources.GetString("buttomSetting.DG_SVGString");
            buttomSetting.Dock = DockStyle.Top;
            buttomSetting.FlatAppearance.BorderSize = 0;
            buttomSetting.FlatStyle = FlatStyle.Flat;
            buttomSetting.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttomSetting.ForeColor = Color.FromArgb(40, 40, 40);
            buttomSetting.Image = (Image)resources.GetObject("buttomSetting.Image");
            buttomSetting.ImageAlign = ContentAlignment.MiddleLeft;
            buttomSetting.Location = new Point(2, 48);
            buttomSetting.Name = "buttomSetting";
            buttomSetting.Size = new Size(142, 23);
            buttomSetting.TabIndex = 5;
            buttomSetting.TabStop = false;
            buttomSetting.Text = "Settings";
            buttomSetting.UseVisualStyleBackColor = false;
            buttomSetting.Click += buttomSetting_Click;
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(898, 767);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "FormSettings";
            Padding = new Padding(1);
            Text = "FormSettings";
            Load += FormSettings_Load;
            panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).EndInit();
            panelLineBordernc1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion


        private PanelLineBorderNC panelLineBordernc1;
        private PanelNormalN panelRightMain;
        private ButtomKeep buttomThemeColor;
        private ButtomKeep buttomLanguage;
        private ButtomKeep buttomSetting;
    }
}