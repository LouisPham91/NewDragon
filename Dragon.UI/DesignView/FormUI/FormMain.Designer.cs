
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            panelNormaln1 = new PanelNormalN();
            panelLeftMainBack = new PanelRoundN();
            panelMenu = new PanelRoundN();
            panelUserControl = new PanelNormalN();
            tableLayoutPanelMain = new TableLayoutPanelBottomBorderN();
            labelKeepVerison = new LabelKeepForceColor();
            labelKeepCommon = new LabelKeepForceColor();
            labelKeepOTG = new LabelKeepForceColor();
            panelLoginVip = new PanelLineBorderNC();
            pictureBoxBrightn1 = new PictureBoxBrightN();
            labelLoginVip = new LabelNormalN();
            panelGradientMainForm = new PanelLineBorderNC();
            pictureLogo = new PictureBoxBrightN();
            label2 = new LabelNormalN();
            label1 = new LabelNormalN();
            PictureBox_Sun_Bright = new PictureBoxBrightN();
            PictureBox_Chevrons_left = new PictureBoxBrightN();
            flowLayoutMain = new DoubleBufferedFlowLayoutPanel();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).BeginInit();
            panelNormaln1.SuspendLayout();
            panelLeftMainBack.SuspendLayout();
            panelMenu.SuspendLayout();
            tableLayoutPanelMain.SuspendLayout();
            panelLoginVip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).BeginInit();
            panelGradientMainForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBox_Sun_Bright).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBox_Chevrons_left).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.Controls.Add(flowLayoutMain);
            panelMain.Controls.Add(panelNormaln1);
            panelMain.Location = new Point(1, 1);
            panelMain.Size = new Size(1698, 948);
            panelMain.Controls.SetChildIndex(panelNormaln1, 0);
            panelMain.Controls.SetChildIndex(flowLayoutMain, 0);
            // 
            // labelTitle
            // 
            labelTitle.ForeColor = SystemColors.ControlText;
            labelTitle.Size = new Size(132, 17);
            labelTitle.Text = "Dragon Phone Farm";
            // 
            // panelNormaln1
            // 
            panelNormaln1.BackColor = Color.FromArgb(245, 245, 245);
            panelNormaln1.Controls.Add(panelLeftMainBack);
            panelNormaln1.Dock = DockStyle.Left;
            panelNormaln1.ForeColor = Color.Black;
            panelNormaln1.Location = new Point(0, 34);
            panelNormaln1.Name = "panelNormaln1";
            panelNormaln1.Padding = new Padding(10, 5, 10, 0);
            panelNormaln1.Size = new Size(396, 914);
            panelNormaln1.TabIndex = 1;
            // 
            // panelLeftMainBack
            // 
            panelLeftMainBack.BackColor = Color.FromArgb(168, 168, 168);
            panelLeftMainBack.Controls.Add(panelMenu);
            panelLeftMainBack.Dock = DockStyle.Top;
            panelLeftMainBack.ForeColor = Color.Black;
            panelLeftMainBack.GD_Radius = 15F;
            panelLeftMainBack.Location = new Point(10, 5);
            panelLeftMainBack.Name = "panelLeftMainBack";
            panelLeftMainBack.Padding = new Padding(1);
            panelLeftMainBack.Size = new Size(376, 900);
            panelLeftMainBack.TabIndex = 0;
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.WhiteSmoke;
            panelMenu.Controls.Add(panelUserControl);
            panelMenu.Controls.Add(tableLayoutPanelMain);
            panelMenu.Controls.Add(panelLoginVip);
            panelMenu.Controls.Add(panelGradientMainForm);
            panelMenu.Dock = DockStyle.Fill;
            panelMenu.ForeColor = Color.Black;
            panelMenu.GD_Radius = 15F;
            panelMenu.Location = new Point(1, 1);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(374, 898);
            panelMenu.TabIndex = 0;
            // 
            // panelUserControl
            // 
            panelUserControl.BackColor = Color.FromArgb(245, 245, 245);
            panelUserControl.Dock = DockStyle.Fill;
            panelUserControl.ForeColor = Color.Black;
            panelUserControl.Location = new Point(0, 202);
            panelUserControl.Name = "panelUserControl";
            panelUserControl.Size = new Size(374, 696);
            panelUserControl.TabIndex = 5;
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.BackColor = Color.WhiteSmoke;
            tableLayoutPanelMain.ColumnCount = 3;
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanelMain.Controls.Add(labelKeepVerison, 2, 0);
            tableLayoutPanelMain.Controls.Add(labelKeepCommon, 0, 0);
            tableLayoutPanelMain.Controls.Add(labelKeepOTG, 1, 0);
            tableLayoutPanelMain.DG_BorderColor = Color.FromArgb(168, 168, 168);
            tableLayoutPanelMain.Dock = DockStyle.Top;
            tableLayoutPanelMain.Location = new Point(0, 173);
            tableLayoutPanelMain.Margin = new Padding(0);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 1;
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanelMain.Size = new Size(374, 29);
            tableLayoutPanelMain.TabIndex = 4;
            // 
            // labelKeepVerison
            // 
            labelKeepVerison.AutoSize = true;
            labelKeepVerison.BackColor = Color.FromArgb(245, 245, 245);
            labelKeepVerison.DG_SVGString = resources.GetString("labelKeepVerison.DG_SVGString");
            labelKeepVerison.Dock = DockStyle.Fill;
            labelKeepVerison.ForeColor = Color.Black;
            labelKeepVerison.Image = (Image)resources.GetObject("labelKeepVerison.Image");
            labelKeepVerison.ImageAlign = ContentAlignment.MiddleLeft;
            labelKeepVerison.Location = new Point(248, 0);
            labelKeepVerison.Margin = new Padding(0);
            labelKeepVerison.Name = "labelKeepVerison";
            labelKeepVerison.Padding = new Padding(25, 0, 25, 0);
            labelKeepVerison.Size = new Size(126, 29);
            labelKeepVerison.TabIndex = 4;
            labelKeepVerison.Text = "Version";
            labelKeepVerison.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelKeepCommon
            // 
            labelKeepCommon.AutoSize = true;
            labelKeepCommon.BackColor = Color.FromArgb(245, 245, 245);
            labelKeepCommon.DG_SVGString = resources.GetString("labelKeepCommon.DG_SVGString");
            labelKeepCommon.Dock = DockStyle.Fill;
            labelKeepCommon.ForeColor = Color.Black;
            labelKeepCommon.Image = (Image)resources.GetObject("labelKeepCommon.Image");
            labelKeepCommon.ImageAlign = ContentAlignment.MiddleLeft;
            labelKeepCommon.Location = new Point(0, 0);
            labelKeepCommon.Margin = new Padding(0);
            labelKeepCommon.Name = "labelKeepCommon";
            labelKeepCommon.Padding = new Padding(21, 0, 10, 0);
            labelKeepCommon.Size = new Size(124, 29);
            labelKeepCommon.TabIndex = 0;
            labelKeepCommon.Text = "Common";
            labelKeepCommon.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelKeepOTG
            // 
            labelKeepOTG.AutoSize = true;
            labelKeepOTG.BackColor = Color.FromArgb(245, 245, 245);
            labelKeepOTG.DG_SVGString = resources.GetString("labelKeepOTG.DG_SVGString");
            labelKeepOTG.Dock = DockStyle.Fill;
            labelKeepOTG.ForeColor = Color.Black;
            labelKeepOTG.Image = (Image)resources.GetObject("labelKeepOTG.Image");
            labelKeepOTG.ImageAlign = ContentAlignment.MiddleLeft;
            labelKeepOTG.Location = new Point(124, 0);
            labelKeepOTG.Margin = new Padding(0);
            labelKeepOTG.Name = "labelKeepOTG";
            labelKeepOTG.Padding = new Padding(29, 0, 29, 0);
            labelKeepOTG.Size = new Size(124, 29);
            labelKeepOTG.TabIndex = 3;
            labelKeepOTG.Text = "OTG";
            labelKeepOTG.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panelLoginVip
            // 
            panelLoginVip.Controls.Add(pictureBoxBrightn1);
            panelLoginVip.Controls.Add(labelLoginVip);
            panelLoginVip.DG_DrawBottom = true;
            panelLoginVip.DG_DrawBottomColor = Color.FromArgb(168, 168, 168);
            panelLoginVip.DG_IsSameLineColor = true;
            panelLoginVip.DG_IsUseBackNormal = true;
            panelLoginVip.Dock = DockStyle.Top;
            panelLoginVip.Location = new Point(0, 140);
            panelLoginVip.Name = "panelLoginVip";
            panelLoginVip.Padding = new Padding(2);
            panelLoginVip.Size = new Size(374, 33);
            panelLoginVip.TabIndex = 3;
            // 
            // pictureBoxBrightn1
            // 
            pictureBoxBrightn1.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxBrightn1.DG_ImageColor = Color.Black;
            pictureBoxBrightn1.DG_ImageSize = new Size(17, 17);
            pictureBoxBrightn1.DG_SVGString = resources.GetString("pictureBoxBrightn1.DG_SVGString");
            pictureBoxBrightn1.ForeColor = Color.Black;
            pictureBoxBrightn1.Location = new Point(23, 8);
            pictureBoxBrightn1.Name = "pictureBoxBrightn1";
            pictureBoxBrightn1.Size = new Size(17, 17);
            pictureBoxBrightn1.TabIndex = 4;
            pictureBoxBrightn1.TabStop = false;
            // 
            // labelLoginVip
            // 
            labelLoginVip.AutoSize = true;
            labelLoginVip.BackColor = Color.Transparent;
            labelLoginVip.DG_IsBrightBack = true;
            labelLoginVip.ForeColor = Color.Black;
            labelLoginVip.Location = new Point(54, 9);
            labelLoginVip.Name = "labelLoginVip";
            labelLoginVip.Size = new Size(152, 15);
            labelLoginVip.TabIndex = 3;
            labelLoginVip.Text = "Login VIP to use all features";
            // 
            // panelGradientMainForm
            // 
            panelGradientMainForm.Controls.Add(pictureLogo);
            panelGradientMainForm.Controls.Add(label2);
            panelGradientMainForm.Controls.Add(label1);
            panelGradientMainForm.Controls.Add(PictureBox_Sun_Bright);
            panelGradientMainForm.Controls.Add(PictureBox_Chevrons_left);
            panelGradientMainForm.DG_BackGroundColor1 = Color.FromArgb(32, 173, 232);
            panelGradientMainForm.DG_BackGroundColor2 = Color.FromArgb(255, 71, 51);
            panelGradientMainForm.DG_BackGroundGradientBool = true;
            panelGradientMainForm.DG_BackGroundLinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            panelGradientMainForm.DG_DrawBottom = true;
            panelGradientMainForm.DG_DrawBottomColor = Color.FromArgb(168, 168, 168);
            panelGradientMainForm.DG_IsSameLineColor = true;
            panelGradientMainForm.Dock = DockStyle.Top;
            panelGradientMainForm.Location = new Point(0, 0);
            panelGradientMainForm.Name = "panelGradientMainForm";
            panelGradientMainForm.Padding = new Padding(2);
            panelGradientMainForm.Size = new Size(374, 140);
            panelGradientMainForm.TabIndex = 1;
            // 
            // pictureLogo
            // 
            pictureLogo.BackColor = Color.Transparent;
            pictureLogo.DG_ImageColor = Color.Black;
            pictureLogo.DG_IsBackTransparent = true;
            pictureLogo.DG_IsBrightBack = false;
            pictureLogo.ForeColor = Color.Black;
            pictureLogo.Location = new Point(64, 22);
            pictureLogo.Name = "pictureLogo";
            pictureLogo.Size = new Size(100, 100);
            pictureLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureLogo.TabIndex = 3;
            pictureLogo.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(179, 62);
            label2.Name = "label2";
            label2.Size = new Size(216, 19);
            label2.TabIndex = 2;
            label2.Text = "dragonphonefarm.kozow.com ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(179, 22);
            label1.Name = "label1";
            label1.Size = new Size(100, 32);
            label1.TabIndex = 1;
            label1.Text = "Dragon";
            // 
            // PictureBox_Sun_Bright
            // 
            PictureBox_Sun_Bright.BackColor = Color.Transparent;
            PictureBox_Sun_Bright.DG_ImageColor = Color.Black;
            PictureBox_Sun_Bright.DG_IsBackTransparent = true;
            PictureBox_Sun_Bright.DG_SVGString = resources.GetString("PictureBox_Sun_Bright.DG_SVGString");
            PictureBox_Sun_Bright.ForeColor = Color.Black;
            PictureBox_Sun_Bright.Location = new Point(23, 83);
            PictureBox_Sun_Bright.Name = "PictureBox_Sun_Bright";
            PictureBox_Sun_Bright.Size = new Size(17, 17);
            PictureBox_Sun_Bright.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBox_Sun_Bright.TabIndex = 2;
            PictureBox_Sun_Bright.TabStop = false;
            PictureBox_Sun_Bright.Click += PictureBox_Sun_Bright_Click;
            // 
            // PictureBox_Chevrons_left
            // 
            PictureBox_Chevrons_left.BackColor = Color.Transparent;
            PictureBox_Chevrons_left.DG_ImageColor = Color.Black;
            PictureBox_Chevrons_left.DG_ImageSize = new Size(17, 17);
            PictureBox_Chevrons_left.DG_IsBackTransparent = true;
            PictureBox_Chevrons_left.DG_SVGString = resources.GetString("PictureBox_Chevrons_left.DG_SVGString");
            PictureBox_Chevrons_left.ForeColor = Color.Black;
            PictureBox_Chevrons_left.Location = new Point(23, 43);
            PictureBox_Chevrons_left.Name = "PictureBox_Chevrons_left";
            PictureBox_Chevrons_left.Padding = new Padding(1);
            PictureBox_Chevrons_left.Size = new Size(17, 17);
            PictureBox_Chevrons_left.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBox_Chevrons_left.TabIndex = 1;
            PictureBox_Chevrons_left.TabStop = false;
            PictureBox_Chevrons_left.Click += PictureBox_Chevrons_left_Click;
            // 
            // flowLayoutMain
            // 
            flowLayoutMain.AutoScroll = true;
            flowLayoutMain.BackColor = Color.FromArgb(245, 245, 245);
            flowLayoutMain.Dock = DockStyle.Fill;
            flowLayoutMain.ForeColor = Color.Black;
            flowLayoutMain.Location = new Point(396, 34);
            flowLayoutMain.Name = "flowLayoutMain";
            flowLayoutMain.Size = new Size(1302, 914);
            flowLayoutMain.TabIndex = 2;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(168, 168, 168);
            ClientSize = new Size(1700, 950);
            Name = "FormMain";
            Padding = new Padding(1);
            Load += FirstForm_Load;
            panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).EndInit();
            panelNormaln1.ResumeLayout(false);
            panelLeftMainBack.ResumeLayout(false);
            panelMenu.ResumeLayout(false);
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            panelLoginVip.ResumeLayout(false);
            panelLoginVip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).EndInit();
            panelGradientMainForm.ResumeLayout(false);
            panelGradientMainForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBox_Sun_Bright).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBox_Chevrons_left).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PanelNormalN panelNormaln1;
        private PanelRoundN panelLeftMainBack;
        private PanelRoundN panelMenu;
        private PanelLineBorderNC panelGradientMainForm;
        private PictureBoxBrightN PictureBox_Sun_Bright;
        private LabelNormalN label2;
        private LabelNormalN label1;
        private PictureBoxBrightN PictureBox_Chevrons_left;
        private PanelLineBorderNC panelLoginVip;
        private LabelNormalN labelLoginVip;
        private PictureBoxBrightN pictureBoxBrightn1;
        private TableLayoutPanelBottomBorderN tableLayoutPanelMain;
        private LabelKeepForceColor labelKeepCommon;
        private LabelKeepForceColor labelKeepOTG;
        private LabelKeepForceColor labelKeepVerison;
        private PanelNormalN panelUserControl;
        private DoubleBufferedFlowLayoutPanel flowLayoutMain;
        private PictureBoxBrightN pictureLogo;
    }
}