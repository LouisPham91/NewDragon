
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;
using System.Xml.Linq;

namespace Dragon.DesignView.ControlUI.Private
{
    partial class UserCommon
    {
        /// <summary> 
        /// Required designer variable.a
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserCommon));
            tableLayoutForButtom = new TableLayoutPanelBottomBorderN();
            buttomSocialNetwork = new ButtomRound();
            buttonDevice = new ButtomRound();
            buttomDarkMode = new ButtomRound();
            buttonADB = new ButtomRound();
            buttomLoad = new ButtomRound();
            buttonMobileRotate = new ButtomRound();
            buttomSetting = new ButtomRound();
            buttonMore = new ButtomRound();
            toggleControlScreen = new ToggleWithTextControl();
            tableLayoutPanel2 = new TableLayoutPanelN();
            labeln4 = new LabelN();
            labeln3 = new LabelN();
            SliderMaxScreenSize = new SliderCustomC();
            SliderSmallScreenSize = new SliderCustomC();
            SilderFPS = new SliderCustomC();
            SliderBitrate = new SliderCustomC();
            labeln1 = new LabelN();
            labeln2 = new LabelN();
            panelLineBorder1 = new PanelLineBorderNC();
            pictureBoxHome = new PictureBoxBrightN();
            tableLayoutPanel3 = new TableLayoutPanelN();
            labelKeepWATX = new LabelKeepForceColor();
            labelKeepWIFI = new LabelKeepForceColor();
            labelKeepUSB = new LabelKeepForceColor();
            labelKeepALL = new LabelKeepForceColor();
            labelKeepACC = new LabelKeepForceColor();
            labelKeepUATX = new LabelKeepForceColor();
            labelKeepUHDI = new LabelKeepForceColor();
            labelKeepWHDI = new LabelKeepForceColor();
            tableLayoutPanel4 = new TableLayoutPanelN();
            numbericPhonePage = new NumericUpDownCustomN();
            label5 = new LabelNormalN();
            labelNoneBox = new LabelNormalN();
            labelPhoneTags = new LabelNormalN();
            tableLayoutPhoneTag = new TableLayoutPanelBottomBorderN();
            tableLayoutPanel6 = new TableLayoutPanelN();
            numbericBoxPage = new NumericUpDownCustomN();
            label7 = new LabelNormalN();
            toggleControlBox = new ToggleWithTextControl();
            labelPhoneBox = new LabelNormalN();
            tableLayoutPhoneBox = new TableLayoutPanelBottomBorderN();
            tableLayoutForButtom.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panelLineBorder1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHome).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutForButtom
            // 
            tableLayoutForButtom.BackColor = Color.FromArgb(245, 245, 245);
            tableLayoutForButtom.ColumnCount = 3;
            tableLayoutForButtom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tableLayoutForButtom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutForButtom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutForButtom.Controls.Add(buttomSocialNetwork, 2, 3);
            tableLayoutForButtom.Controls.Add(buttonDevice, 2, 1);
            tableLayoutForButtom.Controls.Add(buttomDarkMode, 1, 3);
            tableLayoutForButtom.Controls.Add(buttonADB, 0, 3);
            tableLayoutForButtom.Controls.Add(buttomLoad, 1, 2);
            tableLayoutForButtom.Controls.Add(buttonMobileRotate, 0, 2);
            tableLayoutForButtom.Controls.Add(buttomSetting, 1, 1);
            tableLayoutForButtom.Controls.Add(buttonMore, 0, 1);
            tableLayoutForButtom.Controls.Add(toggleControlScreen, 2, 2);
            tableLayoutForButtom.DG_BorderColor = Color.Empty;
            tableLayoutForButtom.Dock = DockStyle.Top;
            tableLayoutForButtom.Location = new Point(0, 0);
            tableLayoutForButtom.Margin = new Padding(0);
            tableLayoutForButtom.Name = "tableLayoutForButtom";
            tableLayoutForButtom.RowCount = 4;
            tableLayoutForButtom.RowStyles.Add(new RowStyle(SizeType.Absolute, 5F));
            tableLayoutForButtom.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutForButtom.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutForButtom.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutForButtom.Size = new Size(372, 108);
            tableLayoutForButtom.TabIndex = 12;
            // 
            // buttomSocialNetwork
            // 
            buttomSocialNetwork.BackColor = Color.FromArgb(225, 225, 225);
            buttomSocialNetwork.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttomSocialNetwork.DG_ForceColor = Color.Empty;
            buttomSocialNetwork.DG_SVGString = resources.GetString("buttomSocialNetwork.DG_SVGString");
            buttomSocialNetwork.FlatAppearance.BorderSize = 0;
            buttomSocialNetwork.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttomSocialNetwork.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttomSocialNetwork.FlatStyle = FlatStyle.Flat;
            buttomSocialNetwork.ForeColor = Color.FromArgb(255, 70, 70);
            buttomSocialNetwork.GD_Radius = 5F;
            buttomSocialNetwork.Image = (Image)resources.GetObject("buttomSocialNetwork.Image");
            buttomSocialNetwork.ImageAlign = ContentAlignment.MiddleLeft;
            buttomSocialNetwork.Location = new Point(250, 74);
            buttomSocialNetwork.Name = "buttomSocialNetwork";
            buttomSocialNetwork.Padding = new Padding(10, 0, 10, 0);
            buttomSocialNetwork.Size = new Size(116, 27);
            buttomSocialNetwork.TabIndex = 25;
            buttomSocialNetwork.TabStop = false;
            buttomSocialNetwork.Text = "Network";
            buttomSocialNetwork.TextAlign = ContentAlignment.MiddleRight;
            buttomSocialNetwork.UseVisualStyleBackColor = false;
            buttomSocialNetwork.Click += buttomSocialNetwork_Click;
            // 
            // buttonDevice
            // 
            buttonDevice.BackColor = Color.FromArgb(225, 225, 225);
            buttonDevice.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonDevice.DG_ForceColor = Color.Empty;
            buttonDevice.DG_SVGString = resources.GetString("buttonDevice.DG_SVGString");
            buttonDevice.FlatAppearance.BorderSize = 0;
            buttonDevice.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonDevice.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonDevice.FlatStyle = FlatStyle.Flat;
            buttonDevice.ForeColor = Color.FromArgb(255, 70, 70);
            buttonDevice.GD_Radius = 5F;
            buttonDevice.Image = (Image)resources.GetObject("buttonDevice.Image");
            buttonDevice.ImageAlign = ContentAlignment.MiddleLeft;
            buttonDevice.Location = new Point(250, 8);
            buttonDevice.Name = "buttonDevice";
            buttonDevice.Padding = new Padding(19, 0, 19, 0);
            buttonDevice.Size = new Size(116, 27);
            buttonDevice.TabIndex = 19;
            buttonDevice.TabStop = false;
            buttonDevice.Text = "Devices";
            buttonDevice.TextAlign = ContentAlignment.MiddleRight;
            buttonDevice.UseVisualStyleBackColor = false;
            buttonDevice.Click += buttonDevice_Click;
            // 
            // buttomDarkMode
            // 
            buttomDarkMode.BackColor = Color.FromArgb(225, 225, 225);
            buttomDarkMode.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttomDarkMode.DG_ForceColor = Color.Empty;
            buttomDarkMode.DG_SVGString = resources.GetString("buttomDarkMode.DG_SVGString");
            buttomDarkMode.FlatAppearance.BorderSize = 0;
            buttomDarkMode.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttomDarkMode.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttomDarkMode.FlatStyle = FlatStyle.Flat;
            buttomDarkMode.ForeColor = Color.FromArgb(255, 70, 70);
            buttomDarkMode.GD_Radius = 5F;
            buttomDarkMode.Image = (Image)resources.GetObject("buttomDarkMode.Image");
            buttomDarkMode.ImageAlign = ContentAlignment.MiddleLeft;
            buttomDarkMode.Location = new Point(126, 74);
            buttomDarkMode.Name = "buttomDarkMode";
            buttomDarkMode.Padding = new Padding(10, 3, 10, 0);
            buttomDarkMode.Size = new Size(116, 27);
            buttomDarkMode.TabIndex = 14;
            buttomDarkMode.TabStop = false;
            buttomDarkMode.Text = "Light Mode";
            buttomDarkMode.TextAlign = ContentAlignment.MiddleRight;
            buttomDarkMode.UseVisualStyleBackColor = false;
            buttomDarkMode.Click += buttomDarkMode_Click;
            // 
            // buttonADB
            // 
            buttonADB.BackColor = Color.FromArgb(225, 225, 225);
            buttonADB.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonADB.DG_ForceColor = Color.Empty;
            buttonADB.DG_SVGString = resources.GetString("buttonADB.DG_SVGString");
            buttonADB.FlatAppearance.BorderSize = 0;
            buttonADB.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonADB.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonADB.FlatStyle = FlatStyle.Flat;
            buttonADB.ForeColor = Color.FromArgb(255, 70, 70);
            buttonADB.GD_Radius = 5F;
            buttonADB.Image = (Image)resources.GetObject("buttonADB.Image");
            buttonADB.ImageAlign = ContentAlignment.MiddleLeft;
            buttonADB.Location = new Point(3, 74);
            buttonADB.Name = "buttonADB";
            buttonADB.Padding = new Padding(25, 0, 30, 0);
            buttonADB.Size = new Size(116, 27);
            buttonADB.TabIndex = 15;
            buttonADB.TabStop = false;
            buttonADB.Text = "ADB";
            buttonADB.TextAlign = ContentAlignment.MiddleRight;
            buttonADB.UseVisualStyleBackColor = false;
            buttonADB.Click += buttonADB_Click;
            // 
            // buttomLoad
            // 
            buttomLoad.BackColor = Color.FromArgb(225, 225, 225);
            buttomLoad.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttomLoad.DG_ForceColor = Color.Empty;
            buttomLoad.DG_SVGString = resources.GetString("buttomLoad.DG_SVGString");
            buttomLoad.FlatAppearance.BorderSize = 0;
            buttomLoad.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttomLoad.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttomLoad.FlatStyle = FlatStyle.Flat;
            buttomLoad.ForeColor = Color.FromArgb(255, 70, 70);
            buttomLoad.GD_Radius = 5F;
            buttomLoad.Image = (Image)resources.GetObject("buttomLoad.Image");
            buttomLoad.ImageAlign = ContentAlignment.MiddleLeft;
            buttomLoad.Location = new Point(126, 41);
            buttomLoad.Name = "buttomLoad";
            buttomLoad.Padding = new Padding(22, 0, 30, 0);
            buttomLoad.Size = new Size(116, 27);
            buttomLoad.TabIndex = 16;
            buttomLoad.TabStop = false;
            buttomLoad.Text = "Load";
            buttomLoad.TextAlign = ContentAlignment.MiddleRight;
            buttomLoad.UseVisualStyleBackColor = false;
            buttomLoad.Click += buttomLoad_Click;
            // 
            // buttonMobileRotate
            // 
            buttonMobileRotate.BackColor = Color.FromArgb(225, 225, 225);
            buttonMobileRotate.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonMobileRotate.DG_ForceColor = Color.Empty;
            buttonMobileRotate.DG_SVGString = resources.GetString("buttonMobileRotate.DG_SVGString");
            buttonMobileRotate.FlatAppearance.BorderSize = 0;
            buttonMobileRotate.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonMobileRotate.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonMobileRotate.FlatStyle = FlatStyle.Flat;
            buttonMobileRotate.ForeColor = Color.FromArgb(255, 70, 70);
            buttonMobileRotate.GD_Radius = 5F;
            buttonMobileRotate.Image = (Image)resources.GetObject("buttonMobileRotate.Image");
            buttonMobileRotate.ImageAlign = ContentAlignment.MiddleLeft;
            buttonMobileRotate.Location = new Point(3, 41);
            buttonMobileRotate.Name = "buttonMobileRotate";
            buttonMobileRotate.Padding = new Padding(25, 0, 20, 0);
            buttonMobileRotate.Size = new Size(116, 27);
            buttonMobileRotate.TabIndex = 17;
            buttonMobileRotate.TabStop = false;
            buttonMobileRotate.Text = "Rotate";
            buttonMobileRotate.TextAlign = ContentAlignment.MiddleRight;
            buttonMobileRotate.UseVisualStyleBackColor = false;
            buttonMobileRotate.Click += buttonMobileRotate_Click;
            // 
            // buttomSetting
            // 
            buttomSetting.BackColor = Color.FromArgb(225, 225, 225);
            buttomSetting.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttomSetting.DG_ForceColor = Color.Empty;
            buttomSetting.DG_SVGString = resources.GetString("buttomSetting.DG_SVGString");
            buttomSetting.FlatAppearance.BorderSize = 0;
            buttomSetting.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttomSetting.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttomSetting.FlatStyle = FlatStyle.Flat;
            buttomSetting.ForeColor = Color.FromArgb(255, 70, 70);
            buttomSetting.GD_Radius = 5F;
            buttomSetting.Image = (Image)resources.GetObject("buttomSetting.Image");
            buttomSetting.ImageAlign = ContentAlignment.MiddleLeft;
            buttomSetting.Location = new Point(126, 8);
            buttomSetting.Name = "buttomSetting";
            buttomSetting.Padding = new Padding(22, 0, 22, 0);
            buttomSetting.Size = new Size(116, 27);
            buttomSetting.TabIndex = 18;
            buttomSetting.TabStop = false;
            buttomSetting.Text = "Setting";
            buttomSetting.TextAlign = ContentAlignment.MiddleRight;
            buttomSetting.UseVisualStyleBackColor = false;
            buttomSetting.Click += ButtomSetting_Click;
            // 
            // buttonMore
            // 
            buttonMore.BackColor = Color.FromArgb(225, 225, 225);
            buttonMore.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonMore.DG_ForceColor = Color.Empty;
            buttonMore.DG_SVGString = resources.GetString("buttonMore.DG_SVGString");
            buttonMore.FlatAppearance.BorderSize = 0;
            buttonMore.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonMore.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonMore.FlatStyle = FlatStyle.Flat;
            buttonMore.ForeColor = Color.FromArgb(255, 70, 70);
            buttonMore.GD_Radius = 4F;
            buttonMore.Image = (Image)resources.GetObject("buttonMore.Image");
            buttonMore.ImageAlign = ContentAlignment.MiddleLeft;
            buttonMore.Location = new Point(3, 8);
            buttonMore.Name = "buttonMore";
            buttonMore.Padding = new Padding(26, 0, 26, 0);
            buttonMore.Size = new Size(116, 27);
            buttonMore.TabIndex = 13;
            buttonMore.TabStop = false;
            buttonMore.Text = "More";
            buttonMore.TextAlign = ContentAlignment.MiddleRight;
            buttonMore.UseVisualStyleBackColor = false;
            buttonMore.Click += buttomMore_Click;
            // 
            // toggleControlScreen
            // 
            toggleControlScreen.DG_BackgroundColor = Color.FromArgb(225, 225, 225);
            toggleControlScreen.DG_BackGroundRadius = 5F;
            toggleControlScreen.DG_KnobColor = Color.FromArgb(35, 190, 35);
            toggleControlScreen.DG_LabelColor = Color.FromArgb(255, 70, 70);
            toggleControlScreen.DG_LabelText = "Control Screen";
            toggleControlScreen.DG_ToggleColorClick = Color.FromArgb(165, 165, 165);
            toggleControlScreen.DG_ToggleColorNormal = Color.FromArgb(205, 205, 205);
            toggleControlScreen.Dock = DockStyle.Fill;
            toggleControlScreen.Location = new Point(250, 41);
            toggleControlScreen.Name = "toggleControlScreen";
            toggleControlScreen.Size = new Size(119, 27);
            toggleControlScreen.TabIndex = 17;
            toggleControlScreen.Click += toggleControlScreen_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56F));
            tableLayoutPanel2.Controls.Add(labeln4, 0, 3);
            tableLayoutPanel2.Controls.Add(labeln3, 0, 2);
            tableLayoutPanel2.Controls.Add(SliderMaxScreenSize, 1, 0);
            tableLayoutPanel2.Controls.Add(SliderSmallScreenSize, 1, 1);
            tableLayoutPanel2.Controls.Add(SilderFPS, 1, 3);
            tableLayoutPanel2.Controls.Add(SliderBitrate, 1, 2);
            tableLayoutPanel2.Controls.Add(labeln1, 0, 0);
            tableLayoutPanel2.Controls.Add(labeln2, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Top;
            tableLayoutPanel2.Location = new Point(0, 108);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(372, 140);
            tableLayoutPanel2.TabIndex = 14;
            // 
            // labeln4
            // 
            labeln4.AutoSize = true;
            labeln4.Dock = DockStyle.Fill;
            labeln4.Location = new Point(3, 105);
            labeln4.Margin = new Padding(3);
            labeln4.Name = "labeln4";
            labeln4.Size = new Size(157, 32);
            labeln4.TabIndex = 28;
            labeln4.Text = "Small Frame Rate";
            labeln4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labeln3
            // 
            labeln3.AutoSize = true;
            labeln3.Dock = DockStyle.Fill;
            labeln3.Location = new Point(3, 71);
            labeln3.Margin = new Padding(3);
            labeln3.Name = "labeln3";
            labeln3.Size = new Size(157, 28);
            labeln3.TabIndex = 27;
            labeln3.Text = "Small Screen Quanlity";
            labeln3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SliderMaxScreenSize
            // 
            SliderMaxScreenSize.BackColor = Color.FromArgb(245, 245, 245);
            SliderMaxScreenSize.DG_FilledColor = Color.FromArgb(227, 66, 51);
            SliderMaxScreenSize.DG_Maximum = 960;
            SliderMaxScreenSize.DG_Minimum = 300;
            SliderMaxScreenSize.DG_Step = 2;
            SliderMaxScreenSize.DG_TextValueColor = Color.Black;
            SliderMaxScreenSize.DG_ThumbColor = Color.FromArgb(227, 66, 51);
            SliderMaxScreenSize.DG_TrackColor = Color.FromArgb(225, 225, 225);
            SliderMaxScreenSize.Dock = DockStyle.Fill;
            SliderMaxScreenSize.Location = new Point(166, 3);
            SliderMaxScreenSize.Name = "SliderMaxScreenSize";
            SliderMaxScreenSize.Size = new Size(203, 28);
            SliderMaxScreenSize.TabIndex = 20;
            SliderMaxScreenSize.Text = "SliderMaxScreenSize";
            // 
            // SliderSmallScreenSize
            // 
            SliderSmallScreenSize.BackColor = Color.FromArgb(245, 245, 245);
            SliderSmallScreenSize.DG_FilledColor = Color.FromArgb(227, 66, 51);
            SliderSmallScreenSize.DG_Maximum = 768;
            SliderSmallScreenSize.DG_Minimum = 100;
            SliderSmallScreenSize.DG_Step = 2;
            SliderSmallScreenSize.DG_TextValueColor = Color.Black;
            SliderSmallScreenSize.DG_ThumbColor = Color.FromArgb(227, 66, 51);
            SliderSmallScreenSize.DG_TrackColor = Color.FromArgb(225, 225, 225);
            SliderSmallScreenSize.Dock = DockStyle.Fill;
            SliderSmallScreenSize.Location = new Point(166, 37);
            SliderSmallScreenSize.Name = "SliderSmallScreenSize";
            SliderSmallScreenSize.Size = new Size(203, 28);
            SliderSmallScreenSize.TabIndex = 21;
            SliderSmallScreenSize.Text = "customSlider2";
            // 
            // SilderFPS
            // 
            SilderFPS.BackColor = Color.FromArgb(245, 245, 245);
            SilderFPS.DG_FilledColor = Color.FromArgb(227, 66, 51);
            SilderFPS.DG_Maximum = 60;
            SilderFPS.DG_Minimum = 1;
            SilderFPS.DG_TextValueColor = Color.Black;
            SilderFPS.DG_ThumbColor = Color.FromArgb(227, 66, 51);
            SilderFPS.DG_TrackColor = Color.FromArgb(225, 225, 225);
            SilderFPS.Dock = DockStyle.Fill;
            SilderFPS.Location = new Point(166, 105);
            SilderFPS.Name = "SilderFPS";
            SilderFPS.Size = new Size(203, 32);
            SilderFPS.TabIndex = 23;
            SilderFPS.Text = "customSlider4";
            // 
            // SliderBitrate
            // 
            SliderBitrate.BackColor = Color.FromArgb(245, 245, 245);
            SliderBitrate.DG_FilledColor = Color.FromArgb(227, 66, 51);
            SliderBitrate.DG_Maximum = 10000000;
            SliderBitrate.DG_Minimum = 100000;
            SliderBitrate.DG_Step = 100000;
            SliderBitrate.DG_TextValueColor = Color.Black;
            SliderBitrate.DG_ThumbColor = Color.FromArgb(227, 66, 51);
            SliderBitrate.DG_TrackColor = Color.FromArgb(225, 225, 225);
            SliderBitrate.Dock = DockStyle.Fill;
            SliderBitrate.Location = new Point(166, 71);
            SliderBitrate.Name = "SliderBitrate";
            SliderBitrate.Size = new Size(203, 28);
            SliderBitrate.TabIndex = 24;
            SliderBitrate.Text = "customSlider1";
            // 
            // labeln1
            // 
            labeln1.AutoSize = true;
            labeln1.Dock = DockStyle.Fill;
            labeln1.Location = new Point(3, 3);
            labeln1.Margin = new Padding(3);
            labeln1.Name = "labeln1";
            labeln1.Size = new Size(157, 28);
            labeln1.TabIndex = 25;
            labeln1.Text = "Large Screen Size";
            labeln1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labeln2
            // 
            labeln2.AutoSize = true;
            labeln2.Dock = DockStyle.Fill;
            labeln2.Location = new Point(3, 37);
            labeln2.Margin = new Padding(3);
            labeln2.Name = "labeln2";
            labeln2.Size = new Size(157, 28);
            labeln2.TabIndex = 26;
            labeln2.Text = "Small Screen Size";
            labeln2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelLineBorder1
            // 
            panelLineBorder1.Controls.Add(pictureBoxHome);
            panelLineBorder1.DG_DrawBottom = true;
            panelLineBorder1.DG_DrawBottomColor = Color.FromArgb(168, 168, 168);
            panelLineBorder1.DG_IsSameLineColor = true;
            panelLineBorder1.DG_IsUseBackNormal = true;
            panelLineBorder1.Dock = DockStyle.Top;
            panelLineBorder1.Location = new Point(0, 248);
            panelLineBorder1.Name = "panelLineBorder1";
            panelLineBorder1.Padding = new Padding(2);
            panelLineBorder1.Size = new Size(372, 20);
            panelLineBorder1.TabIndex = 16;
            // 
            // pictureBoxHome
            // 
            pictureBoxHome.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxHome.DG_ImageColor = Color.Black;
            pictureBoxHome.DG_IsBrightBack = false;
            pictureBoxHome.DG_SVGString = resources.GetString("pictureBoxHome.DG_SVGString");
            pictureBoxHome.ForeColor = Color.Black;
            pictureBoxHome.Image = (Image)resources.GetObject("pictureBoxHome.Image");
            pictureBoxHome.Location = new Point(7, -1);
            pictureBoxHome.Name = "pictureBoxHome";
            pictureBoxHome.Size = new Size(20, 20);
            pictureBoxHome.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxHome.TabIndex = 0;
            pictureBoxHome.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 8;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.Controls.Add(labelKeepWATX, 4, 0);
            tableLayoutPanel3.Controls.Add(labelKeepWIFI, 2, 0);
            tableLayoutPanel3.Controls.Add(labelKeepUSB, 1, 0);
            tableLayoutPanel3.Controls.Add(labelKeepALL, 0, 0);
            tableLayoutPanel3.Controls.Add(labelKeepACC, 7, 0);
            tableLayoutPanel3.Controls.Add(labelKeepUATX, 3, 0);
            tableLayoutPanel3.Controls.Add(labelKeepUHDI, 5, 0);
            tableLayoutPanel3.Controls.Add(labelKeepWHDI, 6, 0);
            tableLayoutPanel3.Dock = DockStyle.Top;
            tableLayoutPanel3.Location = new Point(0, 268);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(372, 34);
            tableLayoutPanel3.TabIndex = 17;
            // 
            // labelKeepWATX
            // 
            labelKeepWATX.AutoSize = true;
            labelKeepWATX.BackColor = Color.FromArgb(245, 245, 245);
            labelKeepWATX.DG_LineColorLeave = Color.FromArgb(61, 61, 61);
            labelKeepWATX.DG_TextColorEnter = Color.FromArgb(207, 66, 51);
            labelKeepWATX.DG_TextColorLeave = Color.White;
            labelKeepWATX.Dock = DockStyle.Fill;
            labelKeepWATX.ForeColor = Color.Black;
            labelKeepWATX.ImageAlign = ContentAlignment.MiddleLeft;
            labelKeepWATX.Location = new Point(184, 0);
            labelKeepWATX.Margin = new Padding(0);
            labelKeepWATX.Name = "labelKeepWATX";
            labelKeepWATX.Size = new Size(46, 34);
            labelKeepWATX.TabIndex = 9;
            labelKeepWATX.Text = "WATX";
            labelKeepWATX.TextAlign = ContentAlignment.MiddleCenter;
            labelKeepWATX.Click += labelKeepWATX_Click;
            // 
            // labelKeepWIFI
            // 
            labelKeepWIFI.AutoSize = true;
            labelKeepWIFI.BackColor = Color.FromArgb(245, 245, 245);
            labelKeepWIFI.DG_LineColorLeave = Color.FromArgb(61, 61, 61);
            labelKeepWIFI.DG_TextColorEnter = Color.FromArgb(207, 66, 51);
            labelKeepWIFI.DG_TextColorLeave = Color.White;
            labelKeepWIFI.Dock = DockStyle.Fill;
            labelKeepWIFI.ForeColor = Color.Black;
            labelKeepWIFI.ImageAlign = ContentAlignment.MiddleLeft;
            labelKeepWIFI.Location = new Point(92, 0);
            labelKeepWIFI.Margin = new Padding(0);
            labelKeepWIFI.Name = "labelKeepWIFI";
            labelKeepWIFI.Size = new Size(46, 34);
            labelKeepWIFI.TabIndex = 2;
            labelKeepWIFI.Text = "WIFI";
            labelKeepWIFI.TextAlign = ContentAlignment.MiddleCenter;
            labelKeepWIFI.Click += labelKeepWIFI_Click;
            // 
            // labelKeepUSB
            // 
            labelKeepUSB.AutoSize = true;
            labelKeepUSB.BackColor = Color.FromArgb(245, 245, 245);
            labelKeepUSB.DG_LineColorLeave = Color.FromArgb(61, 61, 61);
            labelKeepUSB.DG_TextColorEnter = Color.FromArgb(207, 66, 51);
            labelKeepUSB.DG_TextColorLeave = Color.White;
            labelKeepUSB.Dock = DockStyle.Fill;
            labelKeepUSB.ForeColor = Color.Black;
            labelKeepUSB.ImageAlign = ContentAlignment.MiddleLeft;
            labelKeepUSB.Location = new Point(46, 0);
            labelKeepUSB.Margin = new Padding(0);
            labelKeepUSB.Name = "labelKeepUSB";
            labelKeepUSB.Size = new Size(46, 34);
            labelKeepUSB.TabIndex = 1;
            labelKeepUSB.Text = "USB";
            labelKeepUSB.TextAlign = ContentAlignment.MiddleCenter;
            labelKeepUSB.Click += labelKeepUSB_Click;
            // 
            // labelKeepALL
            // 
            labelKeepALL.AutoSize = true;
            labelKeepALL.BackColor = Color.FromArgb(245, 245, 245);
            labelKeepALL.DG_LineColorLeave = Color.FromArgb(61, 61, 61);
            labelKeepALL.DG_TextColorEnter = Color.FromArgb(207, 66, 51);
            labelKeepALL.DG_TextColorLeave = Color.White;
            labelKeepALL.Dock = DockStyle.Fill;
            labelKeepALL.ForeColor = Color.Black;
            labelKeepALL.ImageAlign = ContentAlignment.MiddleLeft;
            labelKeepALL.Location = new Point(0, 0);
            labelKeepALL.Margin = new Padding(0);
            labelKeepALL.Name = "labelKeepALL";
            labelKeepALL.Size = new Size(46, 34);
            labelKeepALL.TabIndex = 0;
            labelKeepALL.Text = "All";
            labelKeepALL.TextAlign = ContentAlignment.MiddleCenter;
            labelKeepALL.Click += labelKeepALL_Click;
            // 
            // labelKeepACC
            // 
            labelKeepACC.AutoSize = true;
            labelKeepACC.BackColor = Color.FromArgb(245, 245, 245);
            labelKeepACC.DG_LineColorLeave = Color.FromArgb(61, 61, 61);
            labelKeepACC.DG_TextColorEnter = Color.FromArgb(207, 66, 51);
            labelKeepACC.DG_TextColorLeave = Color.White;
            labelKeepACC.Dock = DockStyle.Fill;
            labelKeepACC.ForeColor = Color.Black;
            labelKeepACC.ImageAlign = ContentAlignment.MiddleLeft;
            labelKeepACC.Location = new Point(322, 0);
            labelKeepACC.Margin = new Padding(0);
            labelKeepACC.Name = "labelKeepACC";
            labelKeepACC.Size = new Size(50, 34);
            labelKeepACC.TabIndex = 7;
            labelKeepACC.Text = "ACC";
            labelKeepACC.TextAlign = ContentAlignment.MiddleCenter;
            labelKeepACC.Click += labelKeepACC_Click;
            // 
            // labelKeepUATX
            // 
            labelKeepUATX.AutoSize = true;
            labelKeepUATX.BackColor = Color.FromArgb(245, 245, 245);
            labelKeepUATX.DG_LineColorLeave = Color.FromArgb(61, 61, 61);
            labelKeepUATX.DG_TextColorEnter = Color.FromArgb(207, 66, 51);
            labelKeepUATX.DG_TextColorLeave = Color.White;
            labelKeepUATX.Dock = DockStyle.Fill;
            labelKeepUATX.ForeColor = Color.Black;
            labelKeepUATX.ImageAlign = ContentAlignment.MiddleLeft;
            labelKeepUATX.Location = new Point(138, 0);
            labelKeepUATX.Margin = new Padding(0);
            labelKeepUATX.Name = "labelKeepUATX";
            labelKeepUATX.Size = new Size(46, 34);
            labelKeepUATX.TabIndex = 3;
            labelKeepUATX.Text = "UATX";
            labelKeepUATX.TextAlign = ContentAlignment.MiddleCenter;
            labelKeepUATX.Click += labelKeepUATXClick;
            // 
            // labelKeepUHDI
            // 
            labelKeepUHDI.AutoSize = true;
            labelKeepUHDI.BackColor = Color.FromArgb(245, 245, 245);
            labelKeepUHDI.DG_LineColorLeave = Color.FromArgb(61, 61, 61);
            labelKeepUHDI.DG_TextColorEnter = Color.FromArgb(207, 66, 51);
            labelKeepUHDI.DG_TextColorLeave = Color.White;
            labelKeepUHDI.Dock = DockStyle.Fill;
            labelKeepUHDI.ForeColor = Color.Black;
            labelKeepUHDI.ImageAlign = ContentAlignment.MiddleLeft;
            labelKeepUHDI.Location = new Point(230, 0);
            labelKeepUHDI.Margin = new Padding(0);
            labelKeepUHDI.Name = "labelKeepUHDI";
            labelKeepUHDI.Size = new Size(46, 34);
            labelKeepUHDI.TabIndex = 5;
            labelKeepUHDI.Text = "UHDI";
            labelKeepUHDI.TextAlign = ContentAlignment.MiddleCenter;
            labelKeepUHDI.Click += labelKeepUHDI_Click;
            // 
            // labelKeepWHDI
            // 
            labelKeepWHDI.AutoSize = true;
            labelKeepWHDI.BackColor = Color.FromArgb(245, 245, 245);
            labelKeepWHDI.DG_LineColorLeave = Color.FromArgb(61, 61, 61);
            labelKeepWHDI.DG_TextColorEnter = Color.FromArgb(207, 66, 51);
            labelKeepWHDI.DG_TextColorLeave = Color.White;
            labelKeepWHDI.Dock = DockStyle.Fill;
            labelKeepWHDI.ForeColor = Color.Black;
            labelKeepWHDI.ImageAlign = ContentAlignment.MiddleLeft;
            labelKeepWHDI.Location = new Point(276, 0);
            labelKeepWHDI.Margin = new Padding(0);
            labelKeepWHDI.Name = "labelKeepWHDI";
            labelKeepWHDI.Size = new Size(46, 34);
            labelKeepWHDI.TabIndex = 8;
            labelKeepWHDI.Text = "WHDI";
            labelKeepWHDI.TextAlign = ContentAlignment.MiddleCenter;
            labelKeepWHDI.Click += labelKeepWHDI_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 4;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.Controls.Add(numbericPhonePage, 3, 0);
            tableLayoutPanel4.Controls.Add(label5, 2, 0);
            tableLayoutPanel4.Controls.Add(labelNoneBox, 1, 0);
            tableLayoutPanel4.Controls.Add(labelPhoneTags, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Top;
            tableLayoutPanel4.Location = new Point(0, 302);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(372, 30);
            tableLayoutPanel4.TabIndex = 18;
            // 
            // numbericPhonePage
            // 
            numbericPhonePage.BackColor = Color.FromArgb(225, 225, 225);
            numbericPhonePage.DG_ArrowColor = Color.FromArgb(40, 40, 40);
            numbericPhonePage.DG_ArrowPressedColor = Color.FromArgb(61, 61, 61);
            numbericPhonePage.DG_BorderColor = Color.FromArgb(168, 168, 168);
            numbericPhonePage.DG_BoxColor = Color.FromArgb(225, 225, 225);
            numbericPhonePage.DG_ButtonColor = Color.FromArgb(225, 225, 225);
            numbericPhonePage.DG_Minimum = 1;
            numbericPhonePage.DG_TextColor = Color.Black;
            numbericPhonePage.Dock = DockStyle.Fill;
            numbericPhonePage.ForeColor = Color.Black;
            numbericPhonePage.Location = new Point(299, 3);
            numbericPhonePage.Name = "numbericPhonePage";
            numbericPhonePage.Padding = new Padding(1);
            numbericPhonePage.Size = new Size(70, 24);
            numbericPhonePage.TabIndex = 2;
            numbericPhonePage.Text = "numbericPhonePage";
            numbericPhonePage.Value = 1;
            numbericPhonePage.KeyUp += numbericPhonePage_KeyUp;
            numbericPhonePage.MouseDoubleClick += numbericPhonePage_MouseClick;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Dock = DockStyle.Fill;
            label5.ForeColor = Color.Black;
            label5.Location = new Point(225, 3);
            label5.Margin = new Padding(3);
            label5.Name = "label5";
            label5.Size = new Size(68, 24);
            label5.TabIndex = 1;
            label5.Text = "Pages";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelNoneBox
            // 
            labelNoneBox.AutoSize = true;
            labelNoneBox.BackColor = Color.Transparent;
            labelNoneBox.DG_IsBrightBack = true;
            labelNoneBox.Dock = DockStyle.Fill;
            labelNoneBox.ForeColor = Color.Black;
            labelNoneBox.Location = new Point(151, 3);
            labelNoneBox.Margin = new Padding(3);
            labelNoneBox.Name = "labelNoneBox";
            labelNoneBox.Size = new Size(68, 24);
            labelNoneBox.TabIndex = 3;
            labelNoneBox.Text = "None Box";
            labelNoneBox.TextAlign = ContentAlignment.MiddleRight;
            labelNoneBox.Click += labelNoneBox_Click;
            // 
            // labelPhoneTags
            // 
            labelPhoneTags.AutoSize = true;
            labelPhoneTags.BackColor = Color.Transparent;
            labelPhoneTags.DG_SVGString = resources.GetString("labelPhoneTags.DG_SVGString");
            labelPhoneTags.Dock = DockStyle.Fill;
            labelPhoneTags.ForeColor = Color.Black;
            labelPhoneTags.Image = (Image)resources.GetObject("labelPhoneTags.Image");
            labelPhoneTags.ImageAlign = ContentAlignment.MiddleLeft;
            labelPhoneTags.Location = new Point(3, 3);
            labelPhoneTags.Margin = new Padding(3);
            labelPhoneTags.Name = "labelPhoneTags";
            labelPhoneTags.Size = new Size(142, 24);
            labelPhoneTags.TabIndex = 4;
            labelPhoneTags.Text = "Phone Tags";
            labelPhoneTags.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPhoneTag
            // 
            tableLayoutPhoneTag.BackColor = Color.FromArgb(245, 245, 245);
            tableLayoutPhoneTag.ColumnCount = 10;
            tableLayoutPhoneTag.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneTag.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneTag.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneTag.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneTag.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.408602F));
            tableLayoutPhoneTag.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.4838705F));
            tableLayoutPhoneTag.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneTag.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneTag.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneTag.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneTag.DG_BorderColor = Color.FromArgb(168, 168, 168);
            tableLayoutPhoneTag.DG_IsCellSingleBorder = true;
            tableLayoutPhoneTag.Dock = DockStyle.Top;
            tableLayoutPhoneTag.Location = new Point(0, 332);
            tableLayoutPhoneTag.Margin = new Padding(0);
            tableLayoutPhoneTag.Name = "tableLayoutPhoneTag";
            tableLayoutPhoneTag.RowCount = 5;
            tableLayoutPhoneTag.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPhoneTag.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPhoneTag.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPhoneTag.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPhoneTag.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPhoneTag.Size = new Size(372, 155);
            tableLayoutPhoneTag.TabIndex = 21;
            tableLayoutPhoneTag.MouseClick += TableLayoutPhoneTag_MouseClick;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 4;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19.6236553F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20.1612911F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.Controls.Add(numbericBoxPage, 3, 0);
            tableLayoutPanel6.Controls.Add(label7, 2, 0);
            tableLayoutPanel6.Controls.Add(toggleControlBox, 1, 0);
            tableLayoutPanel6.Controls.Add(labelPhoneBox, 0, 0);
            tableLayoutPanel6.Dock = DockStyle.Top;
            tableLayoutPanel6.Location = new Point(0, 487);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Size = new Size(372, 30);
            tableLayoutPanel6.TabIndex = 22;
            // 
            // numbericBoxPage
            // 
            numbericBoxPage.BackColor = Color.FromArgb(225, 225, 225);
            numbericBoxPage.DG_ArrowColor = Color.FromArgb(40, 40, 40);
            numbericBoxPage.DG_ArrowPressedColor = Color.FromArgb(61, 61, 61);
            numbericBoxPage.DG_BorderColor = Color.FromArgb(168, 168, 168);
            numbericBoxPage.DG_BoxColor = Color.FromArgb(225, 225, 225);
            numbericBoxPage.DG_ButtonColor = Color.FromArgb(225, 225, 225);
            numbericBoxPage.DG_Minimum = 1;
            numbericBoxPage.DG_TextColor = Color.Black;
            numbericBoxPage.ForeColor = Color.Black;
            numbericBoxPage.Location = new Point(300, 3);
            numbericBoxPage.Name = "numbericBoxPage";
            numbericBoxPage.Size = new Size(69, 24);
            numbericBoxPage.TabIndex = 3;
            numbericBoxPage.Value = 1;
            numbericBoxPage.Click += numbericBoxPage_Click;
            numbericBoxPage.KeyUp += numbericBoxPage_KeyUp;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Dock = DockStyle.Fill;
            label7.ForeColor = Color.Black;
            label7.Location = new Point(225, 3);
            label7.Margin = new Padding(3);
            label7.Name = "label7";
            label7.Size = new Size(69, 24);
            label7.TabIndex = 2;
            label7.Text = "Pages";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // toggleControlBox
            // 
            toggleControlBox.BackColor = Color.Transparent;
            toggleControlBox.DG_BackgroundColor = Color.FromArgb(40, 40, 40);
            toggleControlBox.DG_BackGroundRadius = 5F;
            toggleControlBox.DG_IsModeWhiteBlack = true;
            toggleControlBox.DG_KnobColor = Color.FromArgb(136, 136, 136);
            toggleControlBox.DG_LabelColor = Color.FromArgb(35, 130, 35);
            toggleControlBox.DG_LabelText = "";
            toggleControlBox.DG_ToggleColorClick = Color.FromArgb(185, 185, 185);
            toggleControlBox.DG_ToggleColorNormal = Color.FromArgb(225, 225, 225);
            toggleControlBox.DG_ToggleWidth = 45;
            toggleControlBox.Dock = DockStyle.Right;
            toggleControlBox.ForeColor = Color.Black;
            toggleControlBox.Location = new Point(156, 3);
            toggleControlBox.Name = "toggleControlBox";
            toggleControlBox.Size = new Size(63, 24);
            toggleControlBox.TabIndex = 18;
            toggleControlBox.MouseUp += toggleControlBox_MouseUp;
            // 
            // labelPhoneBox
            // 
            labelPhoneBox.AutoSize = true;
            labelPhoneBox.BackColor = Color.Transparent;
            labelPhoneBox.DG_SVGString = resources.GetString("labelPhoneBox.DG_SVGString");
            labelPhoneBox.Dock = DockStyle.Fill;
            labelPhoneBox.ForeColor = Color.Black;
            labelPhoneBox.Image = (Image)resources.GetObject("labelPhoneBox.Image");
            labelPhoneBox.ImageAlign = ContentAlignment.MiddleLeft;
            labelPhoneBox.Location = new Point(3, 3);
            labelPhoneBox.Margin = new Padding(3);
            labelPhoneBox.Name = "labelPhoneBox";
            labelPhoneBox.Size = new Size(143, 24);
            labelPhoneBox.TabIndex = 19;
            labelPhoneBox.Text = "Phone Boxs";
            labelPhoneBox.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPhoneBox
            // 
            tableLayoutPhoneBox.BackColor = Color.FromArgb(245, 245, 245);
            tableLayoutPhoneBox.ColumnCount = 10;
            tableLayoutPhoneBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.2150536F));
            tableLayoutPhoneBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.67742F));
            tableLayoutPhoneBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPhoneBox.DG_BorderColor = Color.FromArgb(168, 168, 168);
            tableLayoutPhoneBox.DG_IsCellSingleBorder = true;
            tableLayoutPhoneBox.Dock = DockStyle.Top;
            tableLayoutPhoneBox.Location = new Point(0, 517);
            tableLayoutPhoneBox.Margin = new Padding(0);
            tableLayoutPhoneBox.Name = "tableLayoutPhoneBox";
            tableLayoutPhoneBox.RowCount = 5;
            tableLayoutPhoneBox.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPhoneBox.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPhoneBox.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPhoneBox.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPhoneBox.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPhoneBox.Size = new Size(372, 155);
            tableLayoutPhoneBox.TabIndex = 24;
            // 
            // UserCommon
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            Controls.Add(tableLayoutPhoneBox);
            Controls.Add(tableLayoutPanel6);
            Controls.Add(tableLayoutPhoneTag);
            Controls.Add(tableLayoutPanel4);
            Controls.Add(tableLayoutPanel3);
            Controls.Add(panelLineBorder1);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(tableLayoutForButtom);
            Name = "UserCommon";
            Size = new Size(372, 714);
            tableLayoutForButtom.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            panelLineBorder1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxHome).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            ResumeLayout(false);
        }


        #endregion

        private TableLayoutPanelBottomBorderN tableLayoutForButtom;
        private ToggleWithTextControl toggleControlScreen;
        private ButtomRound buttonMore;
        private ButtomRound buttomDarkMode;
        private ButtomRound buttonADB;
        private ButtomRound buttomLoad;
        private ButtomRound buttonMobileRotate;
        private ButtomRound buttomSetting;
        private ButtomRound buttonDevice;
        private SliderCustomC SliderMaxScreenSize;
        private SliderCustomC SliderSmallScreenSize;
        private SliderCustomC SilderFPS;
        private SliderCustomC SliderBitrate;
        private PanelLineBorderNC panelLineBorder1;
        private PictureBoxBrightN pictureBoxHome;
        private NumericUpDownCustomN numbericPhonePage;
        private LabelNormalN label5;
        private LabelNormalN labelNoneBox;
        private TableLayoutPanelBottomBorderN tableLayoutPhoneTag;
        private NumericUpDownCustomN numbericBoxPage;
        private LabelNormalN label7;
        private ToggleWithTextControl toggleControlBox;
        private TableLayoutPanelBottomBorderN tableLayoutPhoneBox;
        private LabelNormalN labelPhoneTags;
        private LabelNormalN labelPhoneBox;
        private LabelKeepForceColor labelKeepALL;
        private LabelKeepForceColor labelKeepACC;
        private LabelKeepForceColor labelKeepUHDI;
        private LabelKeepForceColor labelKeepUATX;
        private LabelKeepForceColor labelKeepWIFI;
        private LabelKeepForceColor labelKeepUSB;
        private LabelKeepForceColor labelKeepWHDI;
        private LabelKeepForceColor labelKeepWATX;
        private ButtomRound buttomSocialNetwork;
        private LabelN labeln1;
        private LabelN labeln4;
        private LabelN labeln3;
        private LabelN labeln2;
        private TableLayoutPanelN tableLayoutPanel2;
        private TableLayoutPanelN tableLayoutPanel3;
        private TableLayoutPanelN tableLayoutPanel4;
        private TableLayoutPanelN tableLayoutPanel6;
    }
}