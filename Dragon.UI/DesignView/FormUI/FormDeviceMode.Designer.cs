
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormDeviceMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeviceMode));
            panelMain = new Panel();
            PanelMenuName = new LabelNormalN();
            tableLayoutPanel2 = new TableLayoutPanel();
            AlabelNormaln2XVE3 = new LabelNormalN();
            AlabelNormaln3df2 = new LabelNormalN();
            AlabelNormaln1111212s = new LabelNormalN();
            AlabelNormaln12sdfsd = new LabelNormalN();
            AlabelNormaln1411XD = new LabelNormalN();
            AlabelNormaln25 = new LabelNormalN();
            AlabelNormaln26 = new LabelNormalN();
            AlabelNormaln27 = new LabelNormalN();
            labelSafeMode = new LabelNormalN();
            labelDelaySync = new LabelNormalN();
            AlabelNormaln21 = new LabelNormalN();
            AlabelNormaln20 = new LabelNormalN();
            AlabelNormaln19 = new LabelNormalN();
            labelWOTPMode = new LabelNormalN();
            toggleHideTagPhone = new ToggleWithTextControl();
            labelUSBMode = new LabelNormalN();
            labelUOTPMode = new LabelNormalN();
            labelNormaln9 = new LabelNormalN();
            AlabelNormaln16 = new LabelNormalN();
            labelWIFIMode = new LabelNormalN();
            AlabelNormaln2 = new LabelNormalN();
            AlabelNormaln3 = new LabelNormalN();
            toggleAutoScreen = new ToggleWithTextControl();
            toggleCtrlSelect = new ToggleWithTextControl();
            labelLightScreen2 = new LabelNormalN();
            UHDIMode = new LabelNormalN();
            labelWHDIMode = new LabelNormalN();
            labelRestartADB = new LabelNormalN();
            toggleDisplayModel = new ToggleWithTextControl();
            toggleDisplayTitle = new ToggleWithTextControl();
            toggleDisplayIP = new ToggleWithTextControl();
            AlabelNormaln11111X = new LabelNormalN();
            labelNormaln1 = new LabelNormalN();
            toggleSerial = new ToggleWithTextControl();
            panel1 = new Panel();
            PictureBoxCloseForm = new PictureBoxBrightN();
            tableLayoutPanel1 = new TableLayoutPanel();
            AlabelNormaln15 = new LabelNormalN();
            labelNormaln8 = new LabelNormalN();
            AlabelNormaln14 = new LabelNormalN();
            AlabelNormaln11 = new LabelNormalN();
            AlabelNormaln12 = new LabelNormalN();
            labelLightScreen1 = new LabelNormalN();
            panelTitle = new LabelNormalN();
            panelMain.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(40, 40, 40);
            panelMain.Controls.Add(PanelMenuName);
            panelMain.Controls.Add(tableLayoutPanel2);
            panelMain.Controls.Add(panel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.ForeColor = Color.Gainsboro;
            panelMain.Location = new Point(1, 1);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(770, 487);
            panelMain.TabIndex = 0;
            // 
            // PanelMenuName
            // 
            PanelMenuName.AutoSize = true;
            PanelMenuName.BackColor = Color.Transparent;
            PanelMenuName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            PanelMenuName.ForeColor = Color.Black;
            PanelMenuName.Location = new Point(22, 80);
            PanelMenuName.Name = "PanelMenuName";
            PanelMenuName.Size = new Size(109, 21);
            PanelMenuName.TabIndex = 15;
            PanelMenuName.Text = "Hide in more";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 11;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            tableLayoutPanel2.Controls.Add(AlabelNormaln2XVE3, 9, 2);
            tableLayoutPanel2.Controls.Add(AlabelNormaln3df2, 9, 1);
            tableLayoutPanel2.Controls.Add(AlabelNormaln1111212s, 5, 3);
            tableLayoutPanel2.Controls.Add(AlabelNormaln12sdfsd, 5, 2);
            tableLayoutPanel2.Controls.Add(AlabelNormaln1411XD, 5, 1);
            tableLayoutPanel2.Controls.Add(AlabelNormaln25, 1, 10);
            tableLayoutPanel2.Controls.Add(AlabelNormaln26, 1, 9);
            tableLayoutPanel2.Controls.Add(AlabelNormaln27, 1, 8);
            tableLayoutPanel2.Controls.Add(labelSafeMode, 0, 10);
            tableLayoutPanel2.Controls.Add(labelDelaySync, 0, 8);
            tableLayoutPanel2.Controls.Add(AlabelNormaln21, 1, 6);
            tableLayoutPanel2.Controls.Add(AlabelNormaln20, 1, 5);
            tableLayoutPanel2.Controls.Add(AlabelNormaln19, 1, 4);
            tableLayoutPanel2.Controls.Add(labelWOTPMode, 0, 4);
            tableLayoutPanel2.Controls.Add(toggleHideTagPhone, 4, 1);
            tableLayoutPanel2.Controls.Add(labelUSBMode, 0, 1);
            tableLayoutPanel2.Controls.Add(labelUOTPMode, 0, 3);
            tableLayoutPanel2.Controls.Add(labelNormaln9, 0, 0);
            tableLayoutPanel2.Controls.Add(AlabelNormaln16, 1, 1);
            tableLayoutPanel2.Controls.Add(labelWIFIMode, 0, 2);
            tableLayoutPanel2.Controls.Add(AlabelNormaln2, 1, 2);
            tableLayoutPanel2.Controls.Add(AlabelNormaln3, 1, 3);
            tableLayoutPanel2.Controls.Add(toggleAutoScreen, 4, 2);
            tableLayoutPanel2.Controls.Add(toggleCtrlSelect, 4, 3);
            tableLayoutPanel2.Controls.Add(labelLightScreen2, 0, 7);
            tableLayoutPanel2.Controls.Add(UHDIMode, 0, 5);
            tableLayoutPanel2.Controls.Add(labelWHDIMode, 0, 6);
            tableLayoutPanel2.Controls.Add(labelRestartADB, 0, 9);
            tableLayoutPanel2.Controls.Add(toggleDisplayModel, 8, 2);
            tableLayoutPanel2.Controls.Add(toggleDisplayTitle, 8, 1);
            tableLayoutPanel2.Controls.Add(toggleDisplayIP, 8, 3);
            tableLayoutPanel2.Controls.Add(AlabelNormaln11111X, 9, 3);
            tableLayoutPanel2.Controls.Add(labelNormaln1, 9, 4);
            tableLayoutPanel2.Controls.Add(toggleSerial, 8, 4);
            tableLayoutPanel2.Location = new Point(22, 112);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 11;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.Size = new Size(733, 356);
            tableLayoutPanel2.TabIndex = 14;
            // 
            // AlabelNormaln2XVE3
            // 
            AlabelNormaln2XVE3.AutoSize = true;
            AlabelNormaln2XVE3.BackColor = Color.Transparent;
            AlabelNormaln2XVE3.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln2XVE3.DG_SVGString = resources.GetString("AlabelNormaln2XVE3.DG_SVGString");
            AlabelNormaln2XVE3.Enabled = false;
            AlabelNormaln2XVE3.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln2XVE3.ForeColor = Color.Black;
            AlabelNormaln2XVE3.Image = (Image)resources.GetObject("AlabelNormaln2XVE3.Image");
            AlabelNormaln2XVE3.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln2XVE3.Location = new Point(671, 65);
            AlabelNormaln2XVE3.Margin = new Padding(3, 1, 3, 0);
            AlabelNormaln2XVE3.Name = "AlabelNormaln2XVE3";
            AlabelNormaln2XVE3.Size = new Size(13, 30);
            AlabelNormaln2XVE3.TabIndex = 18;
            AlabelNormaln2XVE3.Text = "     ";
            AlabelNormaln2XVE3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln3df2
            // 
            AlabelNormaln3df2.AutoSize = true;
            AlabelNormaln3df2.BackColor = Color.Transparent;
            AlabelNormaln3df2.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln3df2.DG_SVGString = resources.GetString("AlabelNormaln3df2.DG_SVGString");
            AlabelNormaln3df2.Enabled = false;
            AlabelNormaln3df2.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln3df2.ForeColor = Color.Black;
            AlabelNormaln3df2.Image = (Image)resources.GetObject("AlabelNormaln3df2.Image");
            AlabelNormaln3df2.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln3df2.Location = new Point(671, 33);
            AlabelNormaln3df2.Margin = new Padding(3, 1, 3, 0);
            AlabelNormaln3df2.Name = "AlabelNormaln3df2";
            AlabelNormaln3df2.Size = new Size(13, 30);
            AlabelNormaln3df2.TabIndex = 19;
            AlabelNormaln3df2.Text = "     ";
            AlabelNormaln3df2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln1111212s
            // 
            AlabelNormaln1111212s.AutoSize = true;
            AlabelNormaln1111212s.BackColor = Color.Transparent;
            AlabelNormaln1111212s.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln1111212s.DG_SVGString = resources.GetString("AlabelNormaln1111212s.DG_SVGString");
            AlabelNormaln1111212s.Enabled = false;
            AlabelNormaln1111212s.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln1111212s.ForeColor = Color.Black;
            AlabelNormaln1111212s.Image = (Image)resources.GetObject("AlabelNormaln1111212s.Image");
            AlabelNormaln1111212s.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln1111212s.Location = new Point(427, 97);
            AlabelNormaln1111212s.Margin = new Padding(3, 1, 3, 0);
            AlabelNormaln1111212s.Name = "AlabelNormaln1111212s";
            AlabelNormaln1111212s.Size = new Size(13, 30);
            AlabelNormaln1111212s.TabIndex = 20;
            AlabelNormaln1111212s.Text = "     ";
            AlabelNormaln1111212s.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln12sdfsd
            // 
            AlabelNormaln12sdfsd.AutoSize = true;
            AlabelNormaln12sdfsd.BackColor = Color.Transparent;
            AlabelNormaln12sdfsd.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln12sdfsd.DG_SVGString = resources.GetString("AlabelNormaln12sdfsd.DG_SVGString");
            AlabelNormaln12sdfsd.Enabled = false;
            AlabelNormaln12sdfsd.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln12sdfsd.ForeColor = Color.Black;
            AlabelNormaln12sdfsd.Image = (Image)resources.GetObject("AlabelNormaln12sdfsd.Image");
            AlabelNormaln12sdfsd.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln12sdfsd.Location = new Point(427, 65);
            AlabelNormaln12sdfsd.Margin = new Padding(3, 1, 3, 0);
            AlabelNormaln12sdfsd.Name = "AlabelNormaln12sdfsd";
            AlabelNormaln12sdfsd.Size = new Size(13, 30);
            AlabelNormaln12sdfsd.TabIndex = 21;
            AlabelNormaln12sdfsd.Text = "     ";
            AlabelNormaln12sdfsd.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln1411XD
            // 
            AlabelNormaln1411XD.AutoSize = true;
            AlabelNormaln1411XD.BackColor = Color.Transparent;
            AlabelNormaln1411XD.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln1411XD.DG_SVGString = resources.GetString("AlabelNormaln1411XD.DG_SVGString");
            AlabelNormaln1411XD.Enabled = false;
            AlabelNormaln1411XD.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln1411XD.ForeColor = Color.Black;
            AlabelNormaln1411XD.Image = (Image)resources.GetObject("AlabelNormaln1411XD.Image");
            AlabelNormaln1411XD.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln1411XD.Location = new Point(427, 33);
            AlabelNormaln1411XD.Margin = new Padding(3, 1, 3, 0);
            AlabelNormaln1411XD.Name = "AlabelNormaln1411XD";
            AlabelNormaln1411XD.Size = new Size(13, 30);
            AlabelNormaln1411XD.TabIndex = 22;
            AlabelNormaln1411XD.Text = "     ";
            AlabelNormaln1411XD.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln25
            // 
            AlabelNormaln25.AutoSize = true;
            AlabelNormaln25.BackColor = Color.Transparent;
            AlabelNormaln25.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln25.DG_SVGString = resources.GetString("AlabelNormaln25.DG_SVGString");
            AlabelNormaln25.Enabled = false;
            AlabelNormaln25.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln25.ForeColor = Color.Black;
            AlabelNormaln25.Image = (Image)resources.GetObject("AlabelNormaln25.Image");
            AlabelNormaln25.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln25.Location = new Point(153, 322);
            AlabelNormaln25.Margin = new Padding(3, 2, 3, 0);
            AlabelNormaln25.Name = "AlabelNormaln25";
            AlabelNormaln25.Size = new Size(13, 30);
            AlabelNormaln25.TabIndex = 21;
            AlabelNormaln25.Text = "     ";
            AlabelNormaln25.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln26
            // 
            AlabelNormaln26.AutoSize = true;
            AlabelNormaln26.BackColor = Color.Transparent;
            AlabelNormaln26.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln26.DG_SVGString = resources.GetString("AlabelNormaln26.DG_SVGString");
            AlabelNormaln26.Enabled = false;
            AlabelNormaln26.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln26.ForeColor = Color.Black;
            AlabelNormaln26.Image = (Image)resources.GetObject("AlabelNormaln26.Image");
            AlabelNormaln26.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln26.Location = new Point(153, 290);
            AlabelNormaln26.Margin = new Padding(3, 2, 3, 0);
            AlabelNormaln26.Name = "AlabelNormaln26";
            AlabelNormaln26.Size = new Size(13, 30);
            AlabelNormaln26.TabIndex = 22;
            AlabelNormaln26.Text = "     ";
            AlabelNormaln26.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln27
            // 
            AlabelNormaln27.AutoSize = true;
            AlabelNormaln27.BackColor = Color.Transparent;
            AlabelNormaln27.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln27.DG_SVGString = resources.GetString("AlabelNormaln27.DG_SVGString");
            AlabelNormaln27.Enabled = false;
            AlabelNormaln27.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln27.ForeColor = Color.Black;
            AlabelNormaln27.Image = (Image)resources.GetObject("AlabelNormaln27.Image");
            AlabelNormaln27.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln27.Location = new Point(153, 258);
            AlabelNormaln27.Margin = new Padding(3, 2, 3, 0);
            AlabelNormaln27.Name = "AlabelNormaln27";
            AlabelNormaln27.Size = new Size(13, 30);
            AlabelNormaln27.TabIndex = 23;
            AlabelNormaln27.Text = "     ";
            AlabelNormaln27.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelSafeMode
            // 
            labelSafeMode.AutoSize = true;
            labelSafeMode.BackColor = Color.Transparent;
            labelSafeMode.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelSafeMode.DG_SVGString = resources.GetString("labelSafeMode.DG_SVGString");
            labelSafeMode.Font = new Font("Segoe UI", 10F);
            labelSafeMode.ForeColor = Color.Black;
            labelSafeMode.Image = (Image)resources.GetObject("labelSafeMode.Image");
            labelSafeMode.ImageAlign = ContentAlignment.MiddleLeft;
            labelSafeMode.Location = new Point(3, 327);
            labelSafeMode.Margin = new Padding(3, 7, 3, 0);
            labelSafeMode.Name = "labelSafeMode";
            labelSafeMode.Size = new Size(102, 19);
            labelSafeMode.TabIndex = 26;
            labelSafeMode.Text = "       Safe Mode";
            labelSafeMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelDelaySync
            // 
            labelDelaySync.AutoSize = true;
            labelDelaySync.BackColor = Color.Transparent;
            labelDelaySync.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelDelaySync.DG_SVGString = resources.GetString("labelDelaySync.DG_SVGString");
            labelDelaySync.Font = new Font("Segoe UI", 10F);
            labelDelaySync.ForeColor = Color.Black;
            labelDelaySync.Image = (Image)resources.GetObject("labelDelaySync.Image");
            labelDelaySync.ImageAlign = ContentAlignment.MiddleLeft;
            labelDelaySync.Location = new Point(3, 263);
            labelDelaySync.Margin = new Padding(3, 7, 3, 0);
            labelDelaySync.Name = "labelDelaySync";
            labelDelaySync.Size = new Size(103, 19);
            labelDelaySync.TabIndex = 15;
            labelDelaySync.Text = "       Delay Sync";
            labelDelaySync.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln21
            // 
            AlabelNormaln21.AutoSize = true;
            AlabelNormaln21.BackColor = Color.Transparent;
            AlabelNormaln21.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln21.DG_SVGString = resources.GetString("AlabelNormaln21.DG_SVGString");
            AlabelNormaln21.Enabled = false;
            AlabelNormaln21.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln21.ForeColor = Color.Black;
            AlabelNormaln21.Image = (Image)resources.GetObject("AlabelNormaln21.Image");
            AlabelNormaln21.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln21.Location = new Point(153, 194);
            AlabelNormaln21.Margin = new Padding(3, 2, 3, 0);
            AlabelNormaln21.Name = "AlabelNormaln21";
            AlabelNormaln21.Size = new Size(13, 30);
            AlabelNormaln21.TabIndex = 20;
            AlabelNormaln21.Text = "     ";
            AlabelNormaln21.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln20
            // 
            AlabelNormaln20.AutoSize = true;
            AlabelNormaln20.BackColor = Color.Transparent;
            AlabelNormaln20.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln20.DG_SVGString = resources.GetString("AlabelNormaln20.DG_SVGString");
            AlabelNormaln20.Enabled = false;
            AlabelNormaln20.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln20.ForeColor = Color.Black;
            AlabelNormaln20.Image = (Image)resources.GetObject("AlabelNormaln20.Image");
            AlabelNormaln20.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln20.Location = new Point(153, 162);
            AlabelNormaln20.Margin = new Padding(3, 2, 3, 0);
            AlabelNormaln20.Name = "AlabelNormaln20";
            AlabelNormaln20.Size = new Size(13, 30);
            AlabelNormaln20.TabIndex = 20;
            AlabelNormaln20.Text = "     ";
            AlabelNormaln20.TextAlign = ContentAlignment.MiddleCenter;
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
            AlabelNormaln19.Location = new Point(153, 130);
            AlabelNormaln19.Margin = new Padding(3, 2, 3, 0);
            AlabelNormaln19.Name = "AlabelNormaln19";
            AlabelNormaln19.Size = new Size(13, 30);
            AlabelNormaln19.TabIndex = 20;
            AlabelNormaln19.Text = "     ";
            AlabelNormaln19.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelWOTPMode
            // 
            labelWOTPMode.AutoSize = true;
            labelWOTPMode.BackColor = Color.Transparent;
            labelWOTPMode.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelWOTPMode.DG_SVGString = resources.GetString("labelWOTPMode.DG_SVGString");
            labelWOTPMode.Font = new Font("Segoe UI", 10F);
            labelWOTPMode.ForeColor = Color.Black;
            labelWOTPMode.Image = (Image)resources.GetObject("labelWOTPMode.Image");
            labelWOTPMode.ImageAlign = ContentAlignment.MiddleLeft;
            labelWOTPMode.Location = new Point(3, 135);
            labelWOTPMode.Margin = new Padding(3, 7, 3, 0);
            labelWOTPMode.Name = "labelWOTPMode";
            labelWOTPMode.Size = new Size(115, 19);
            labelWOTPMode.TabIndex = 20;
            labelWOTPMode.Text = "       WOTP Mode";
            labelWOTPMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // toggleHideTagPhone
            // 
            toggleHideTagPhone.BackColor = Color.Transparent;
            toggleHideTagPhone.DG_BackgroundColor = Color.Empty;
            toggleHideTagPhone.DG_IsToggled = true;
            toggleHideTagPhone.DG_KnobColor = Color.White;
            toggleHideTagPhone.DG_LabelColor = Color.Gainsboro;
            toggleHideTagPhone.DG_LabelText = "Hide Tag Phone";
            toggleHideTagPhone.DG_ToggleColorClick = Color.FromArgb(207, 66, 51);
            toggleHideTagPhone.DG_ToggleColorNormal = Color.FromArgb(120, 120, 120);
            toggleHideTagPhone.Location = new Point(277, 35);
            toggleHideTagPhone.Name = "toggleHideTagPhone";
            toggleHideTagPhone.Size = new Size(144, 25);
            toggleHideTagPhone.TabIndex = 12;
            // 
            // labelUSBMode
            // 
            labelUSBMode.AutoSize = true;
            labelUSBMode.BackColor = Color.Transparent;
            labelUSBMode.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelUSBMode.DG_SVGString = resources.GetString("labelUSBMode.DG_SVGString");
            labelUSBMode.Font = new Font("Segoe UI", 10F);
            labelUSBMode.ForeColor = Color.Black;
            labelUSBMode.Image = (Image)resources.GetObject("labelUSBMode.Image");
            labelUSBMode.ImageAlign = ContentAlignment.MiddleLeft;
            labelUSBMode.Location = new Point(3, 39);
            labelUSBMode.Margin = new Padding(3, 7, 3, 0);
            labelUSBMode.Name = "labelUSBMode";
            labelUSBMode.Size = new Size(102, 19);
            labelUSBMode.TabIndex = 15;
            labelUSBMode.Text = "       USB Mode";
            labelUSBMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelUOTPMode
            // 
            labelUOTPMode.AutoSize = true;
            labelUOTPMode.BackColor = Color.Transparent;
            labelUOTPMode.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelUOTPMode.DG_SVGString = resources.GetString("labelUOTPMode.DG_SVGString");
            labelUOTPMode.Font = new Font("Segoe UI", 10F);
            labelUOTPMode.ForeColor = Color.Black;
            labelUOTPMode.Image = (Image)resources.GetObject("labelUOTPMode.Image");
            labelUOTPMode.ImageAlign = ContentAlignment.MiddleLeft;
            labelUOTPMode.Location = new Point(3, 103);
            labelUOTPMode.Margin = new Padding(3, 7, 3, 0);
            labelUOTPMode.Name = "labelUOTPMode";
            labelUOTPMode.Size = new Size(112, 19);
            labelUOTPMode.TabIndex = 8;
            labelUOTPMode.Text = "       UOTP Mode";
            labelUOTPMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelNormaln9
            // 
            labelNormaln9.AutoSize = true;
            labelNormaln9.BackColor = Color.Transparent;
            labelNormaln9.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelNormaln9.DG_SVGString = resources.GetString("labelNormaln9.DG_SVGString");
            labelNormaln9.Font = new Font("Segoe UI", 10F);
            labelNormaln9.ForeColor = Color.Black;
            labelNormaln9.Image = (Image)resources.GetObject("labelNormaln9.Image");
            labelNormaln9.ImageAlign = ContentAlignment.MiddleLeft;
            labelNormaln9.Location = new Point(3, 7);
            labelNormaln9.Margin = new Padding(3, 7, 3, 0);
            labelNormaln9.Name = "labelNormaln9";
            labelNormaln9.Size = new Size(64, 19);
            labelNormaln9.TabIndex = 10;
            labelNormaln9.Text = "       ADB";
            labelNormaln9.TextAlign = ContentAlignment.MiddleCenter;
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
            AlabelNormaln16.Location = new Point(153, 34);
            AlabelNormaln16.Margin = new Padding(3, 2, 3, 0);
            AlabelNormaln16.Name = "AlabelNormaln16";
            AlabelNormaln16.Size = new Size(13, 30);
            AlabelNormaln16.TabIndex = 16;
            AlabelNormaln16.Text = "     ";
            AlabelNormaln16.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelWIFIMode
            // 
            labelWIFIMode.AutoSize = true;
            labelWIFIMode.BackColor = Color.Transparent;
            labelWIFIMode.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelWIFIMode.DG_SVGString = resources.GetString("labelWIFIMode.DG_SVGString");
            labelWIFIMode.Font = new Font("Segoe UI", 10F);
            labelWIFIMode.ForeColor = Color.Black;
            labelWIFIMode.Image = (Image)resources.GetObject("labelWIFIMode.Image");
            labelWIFIMode.ImageAlign = ContentAlignment.MiddleLeft;
            labelWIFIMode.Location = new Point(3, 71);
            labelWIFIMode.Margin = new Padding(3, 7, 3, 0);
            labelWIFIMode.Name = "labelWIFIMode";
            labelWIFIMode.Size = new Size(105, 19);
            labelWIFIMode.TabIndex = 6;
            labelWIFIMode.Text = "       WIFI Mode";
            labelWIFIMode.TextAlign = ContentAlignment.MiddleCenter;
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
            AlabelNormaln2.Location = new Point(153, 66);
            AlabelNormaln2.Margin = new Padding(3, 2, 3, 0);
            AlabelNormaln2.Name = "AlabelNormaln2";
            AlabelNormaln2.Size = new Size(13, 30);
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
            AlabelNormaln3.Location = new Point(153, 98);
            AlabelNormaln3.Margin = new Padding(3, 2, 3, 0);
            AlabelNormaln3.Name = "AlabelNormaln3";
            AlabelNormaln3.Size = new Size(13, 30);
            AlabelNormaln3.TabIndex = 18;
            AlabelNormaln3.Text = "     ";
            AlabelNormaln3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // toggleAutoScreen
            // 
            toggleAutoScreen.BackColor = Color.Transparent;
            toggleAutoScreen.DG_BackgroundColor = Color.Empty;
            toggleAutoScreen.DG_KnobColor = Color.White;
            toggleAutoScreen.DG_LabelColor = Color.Gainsboro;
            toggleAutoScreen.DG_LabelText = "Auto Screen";
            toggleAutoScreen.DG_ToggleColorClick = Color.FromArgb(207, 66, 51);
            toggleAutoScreen.DG_ToggleColorNormal = Color.FromArgb(120, 120, 120);
            toggleAutoScreen.Location = new Point(277, 67);
            toggleAutoScreen.Name = "toggleAutoScreen";
            toggleAutoScreen.Size = new Size(144, 25);
            toggleAutoScreen.TabIndex = 19;
            // 
            // toggleCtrlSelect
            // 
            toggleCtrlSelect.BackColor = Color.Transparent;
            toggleCtrlSelect.DG_BackgroundColor = Color.Empty;
            toggleCtrlSelect.DG_KnobColor = Color.White;
            toggleCtrlSelect.DG_LabelColor = Color.Gainsboro;
            toggleCtrlSelect.DG_LabelText = "Ctrl Select";
            toggleCtrlSelect.DG_ToggleColorClick = Color.FromArgb(207, 66, 51);
            toggleCtrlSelect.DG_ToggleColorNormal = Color.FromArgb(120, 120, 120);
            toggleCtrlSelect.Location = new Point(277, 99);
            toggleCtrlSelect.Name = "toggleCtrlSelect";
            toggleCtrlSelect.Size = new Size(144, 25);
            toggleCtrlSelect.TabIndex = 20;
            // 
            // labelLightScreen2
            // 
            labelLightScreen2.AutoSize = true;
            labelLightScreen2.BackColor = Color.Transparent;
            labelLightScreen2.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelLightScreen2.DG_SVGString = resources.GetString("labelLightScreen2.DG_SVGString");
            labelLightScreen2.Font = new Font("Segoe UI", 10F);
            labelLightScreen2.ForeColor = Color.Black;
            labelLightScreen2.Image = (Image)resources.GetObject("labelLightScreen2.Image");
            labelLightScreen2.ImageAlign = ContentAlignment.MiddleLeft;
            labelLightScreen2.Location = new Point(3, 231);
            labelLightScreen2.Margin = new Padding(3, 7, 3, 0);
            labelLightScreen2.Name = "labelLightScreen2";
            labelLightScreen2.Size = new Size(112, 19);
            labelLightScreen2.TabIndex = 11;
            labelLightScreen2.Text = "       Light Screen";
            labelLightScreen2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // UHDIMode
            // 
            UHDIMode.AutoSize = true;
            UHDIMode.BackColor = Color.Transparent;
            UHDIMode.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            UHDIMode.DG_SVGString = resources.GetString("UHDIMode.DG_SVGString");
            UHDIMode.Font = new Font("Segoe UI", 10F);
            UHDIMode.ForeColor = Color.Black;
            UHDIMode.Image = (Image)resources.GetObject("UHDIMode.Image");
            UHDIMode.ImageAlign = ContentAlignment.MiddleLeft;
            UHDIMode.Location = new Point(3, 167);
            UHDIMode.Margin = new Padding(3, 7, 3, 0);
            UHDIMode.Name = "UHDIMode";
            UHDIMode.Size = new Size(111, 19);
            UHDIMode.TabIndex = 23;
            UHDIMode.Text = "       UHDI Mode";
            UHDIMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelWHDIMode
            // 
            labelWHDIMode.AutoSize = true;
            labelWHDIMode.BackColor = Color.Transparent;
            labelWHDIMode.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelWHDIMode.DG_SVGString = resources.GetString("labelWHDIMode.DG_SVGString");
            labelWHDIMode.Font = new Font("Segoe UI", 10F);
            labelWHDIMode.ForeColor = Color.Black;
            labelWHDIMode.Image = (Image)resources.GetObject("labelWHDIMode.Image");
            labelWHDIMode.ImageAlign = ContentAlignment.MiddleLeft;
            labelWHDIMode.Location = new Point(3, 199);
            labelWHDIMode.Margin = new Padding(3, 7, 3, 0);
            labelWHDIMode.Name = "labelWHDIMode";
            labelWHDIMode.Size = new Size(114, 19);
            labelWHDIMode.TabIndex = 24;
            labelWHDIMode.Text = "       WHDI Mode";
            labelWHDIMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelRestartADB
            // 
            labelRestartADB.AutoSize = true;
            labelRestartADB.BackColor = Color.Transparent;
            labelRestartADB.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelRestartADB.DG_SVGString = resources.GetString("labelRestartADB.DG_SVGString");
            labelRestartADB.Font = new Font("Segoe UI", 10F);
            labelRestartADB.ForeColor = Color.Black;
            labelRestartADB.Image = (Image)resources.GetObject("labelRestartADB.Image");
            labelRestartADB.ImageAlign = ContentAlignment.MiddleLeft;
            labelRestartADB.Location = new Point(3, 295);
            labelRestartADB.Margin = new Padding(3, 7, 3, 0);
            labelRestartADB.Name = "labelRestartADB";
            labelRestartADB.Size = new Size(111, 19);
            labelRestartADB.TabIndex = 25;
            labelRestartADB.Text = "       Restart ADB";
            labelRestartADB.TextAlign = ContentAlignment.MiddleCenter;
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
            toggleDisplayModel.Location = new Point(521, 67);
            toggleDisplayModel.Name = "toggleDisplayModel";
            toggleDisplayModel.Size = new Size(144, 25);
            toggleDisplayModel.TabIndex = 15;
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
            toggleDisplayTitle.Location = new Point(521, 35);
            toggleDisplayTitle.Name = "toggleDisplayTitle";
            toggleDisplayTitle.Size = new Size(144, 25);
            toggleDisplayTitle.TabIndex = 28;
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
            toggleDisplayIP.Location = new Point(521, 99);
            toggleDisplayIP.Name = "toggleDisplayIP";
            toggleDisplayIP.Size = new Size(144, 25);
            toggleDisplayIP.TabIndex = 18;
            // 
            // AlabelNormaln11111X
            // 
            AlabelNormaln11111X.AutoSize = true;
            AlabelNormaln11111X.BackColor = Color.Transparent;
            AlabelNormaln11111X.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln11111X.DG_SVGString = resources.GetString("AlabelNormaln11111X.DG_SVGString");
            AlabelNormaln11111X.Enabled = false;
            AlabelNormaln11111X.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln11111X.ForeColor = Color.Black;
            AlabelNormaln11111X.Image = (Image)resources.GetObject("AlabelNormaln11111X.Image");
            AlabelNormaln11111X.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln11111X.Location = new Point(671, 97);
            AlabelNormaln11111X.Margin = new Padding(3, 1, 3, 0);
            AlabelNormaln11111X.Name = "AlabelNormaln11111X";
            AlabelNormaln11111X.Size = new Size(13, 30);
            AlabelNormaln11111X.TabIndex = 17;
            AlabelNormaln11111X.Text = "     ";
            AlabelNormaln11111X.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelNormaln1
            // 
            labelNormaln1.AutoSize = true;
            labelNormaln1.BackColor = Color.Transparent;
            labelNormaln1.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelNormaln1.DG_SVGString = resources.GetString("labelNormaln1.DG_SVGString");
            labelNormaln1.Enabled = false;
            labelNormaln1.Font = new Font("Segoe UI", 8.3F);
            labelNormaln1.ForeColor = Color.Black;
            labelNormaln1.Image = (Image)resources.GetObject("labelNormaln1.Image");
            labelNormaln1.ImageAlign = ContentAlignment.MiddleLeft;
            labelNormaln1.Location = new Point(671, 129);
            labelNormaln1.Margin = new Padding(3, 1, 3, 0);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(13, 30);
            labelNormaln1.TabIndex = 27;
            labelNormaln1.Text = "     ";
            labelNormaln1.TextAlign = ContentAlignment.MiddleCenter;
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
            toggleSerial.Location = new Point(521, 131);
            toggleSerial.Name = "toggleSerial";
            toggleSerial.Size = new Size(144, 25);
            toggleSerial.TabIndex = 17;
            // 
            // panel1
            // 
            panel1.Controls.Add(PictureBoxCloseForm);
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Controls.Add(panelTitle);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(770, 77);
            panel1.TabIndex = 0;
            // 
            // PictureBoxCloseForm
            // 
            PictureBoxCloseForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PictureBoxCloseForm.BackColor = Color.FromArgb(245, 245, 245);
            PictureBoxCloseForm.DG_ImageColor = SystemColors.ControlText;
            PictureBoxCloseForm.DG_ImageSize = new Size(18, 18);
            PictureBoxCloseForm.DG_IsBrightBack = false;
            PictureBoxCloseForm.DG_LightenPercent = 50;
            PictureBoxCloseForm.DG_SVGString = resources.GetString("PictureBoxCloseForm.DG_SVGString");
            PictureBoxCloseForm.ForeColor = Color.Black;
            PictureBoxCloseForm.Image = (Image)resources.GetObject("PictureBoxCloseForm.Image");
            PictureBoxCloseForm.Location = new Point(732, 10);
            PictureBoxCloseForm.Name = "PictureBoxCloseForm";
            PictureBoxCloseForm.Padding = new Padding(2);
            PictureBoxCloseForm.Size = new Size(18, 18);
            PictureBoxCloseForm.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBoxCloseForm.TabIndex = 8;
            PictureBoxCloseForm.TabStop = false;
            PictureBoxCloseForm.Click += PictureBoxCloseForm_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 8;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 8F));
            tableLayoutPanel1.Controls.Add(AlabelNormaln15, 6, 0);
            tableLayoutPanel1.Controls.Add(labelNormaln8, 0, 0);
            tableLayoutPanel1.Controls.Add(AlabelNormaln14, 5, 0);
            tableLayoutPanel1.Controls.Add(AlabelNormaln11, 1, 0);
            tableLayoutPanel1.Controls.Add(AlabelNormaln12, 2, 0);
            tableLayoutPanel1.Controls.Add(labelLightScreen1, 4, 0);
            tableLayoutPanel1.Location = new Point(22, 42);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tableLayoutPanel1.Size = new Size(733, 32);
            tableLayoutPanel1.TabIndex = 13;
            // 
            // AlabelNormaln15
            // 
            AlabelNormaln15.AutoSize = true;
            AlabelNormaln15.BackColor = Color.Transparent;
            AlabelNormaln15.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln15.DG_SVGString = resources.GetString("AlabelNormaln15.DG_SVGString");
            AlabelNormaln15.Enabled = false;
            AlabelNormaln15.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln15.ForeColor = Color.Black;
            AlabelNormaln15.Image = (Image)resources.GetObject("AlabelNormaln15.Image");
            AlabelNormaln15.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln15.Location = new Point(449, 9);
            AlabelNormaln15.Margin = new Padding(3, 9, 3, 0);
            AlabelNormaln15.Name = "AlabelNormaln15";
            AlabelNormaln15.Size = new Size(16, 15);
            AlabelNormaln15.TabIndex = 14;
            AlabelNormaln15.Text = "   ";
            AlabelNormaln15.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelNormaln8
            // 
            labelNormaln8.AutoSize = true;
            labelNormaln8.BackColor = Color.Transparent;
            labelNormaln8.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelNormaln8.DG_SVGString = resources.GetString("labelNormaln8.DG_SVGString");
            labelNormaln8.Font = new Font("Segoe UI", 10F);
            labelNormaln8.ForeColor = Color.Black;
            labelNormaln8.Image = (Image)resources.GetObject("labelNormaln8.Image");
            labelNormaln8.ImageAlign = ContentAlignment.MiddleLeft;
            labelNormaln8.Location = new Point(3, 7);
            labelNormaln8.Margin = new Padding(3, 7, 3, 0);
            labelNormaln8.Name = "labelNormaln8";
            labelNormaln8.Size = new Size(64, 19);
            labelNormaln8.TabIndex = 11;
            labelNormaln8.Text = "       ADB";
            labelNormaln8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln14
            // 
            AlabelNormaln14.AutoSize = true;
            AlabelNormaln14.BackColor = Color.Transparent;
            AlabelNormaln14.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln14.DG_SVGString = resources.GetString("AlabelNormaln14.DG_SVGString");
            AlabelNormaln14.Enabled = false;
            AlabelNormaln14.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln14.ForeColor = Color.Black;
            AlabelNormaln14.Image = (Image)resources.GetObject("AlabelNormaln14.Image");
            AlabelNormaln14.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln14.Location = new Point(427, 9);
            AlabelNormaln14.Margin = new Padding(3, 9, 3, 0);
            AlabelNormaln14.Name = "AlabelNormaln14";
            AlabelNormaln14.Size = new Size(16, 15);
            AlabelNormaln14.TabIndex = 13;
            AlabelNormaln14.Text = "   ";
            AlabelNormaln14.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln11
            // 
            AlabelNormaln11.AutoSize = true;
            AlabelNormaln11.BackColor = Color.Transparent;
            AlabelNormaln11.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln11.DG_SVGString = resources.GetString("AlabelNormaln11.DG_SVGString");
            AlabelNormaln11.Enabled = false;
            AlabelNormaln11.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln11.ForeColor = Color.Black;
            AlabelNormaln11.Image = (Image)resources.GetObject("AlabelNormaln11.Image");
            AlabelNormaln11.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln11.Location = new Point(153, 9);
            AlabelNormaln11.Margin = new Padding(3, 9, 3, 0);
            AlabelNormaln11.Name = "AlabelNormaln11";
            AlabelNormaln11.Size = new Size(16, 15);
            AlabelNormaln11.TabIndex = 12;
            AlabelNormaln11.Text = "   ";
            AlabelNormaln11.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlabelNormaln12
            // 
            AlabelNormaln12.AutoSize = true;
            AlabelNormaln12.BackColor = Color.Transparent;
            AlabelNormaln12.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            AlabelNormaln12.DG_SVGString = resources.GetString("AlabelNormaln12.DG_SVGString");
            AlabelNormaln12.Enabled = false;
            AlabelNormaln12.Font = new Font("Segoe UI", 8.3F);
            AlabelNormaln12.ForeColor = Color.Black;
            AlabelNormaln12.Image = (Image)resources.GetObject("AlabelNormaln12.Image");
            AlabelNormaln12.ImageAlign = ContentAlignment.MiddleLeft;
            AlabelNormaln12.Location = new Point(175, 9);
            AlabelNormaln12.Margin = new Padding(3, 9, 3, 0);
            AlabelNormaln12.Name = "AlabelNormaln12";
            AlabelNormaln12.Size = new Size(16, 15);
            AlabelNormaln12.TabIndex = 13;
            AlabelNormaln12.Text = "   ";
            AlabelNormaln12.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelLightScreen1
            // 
            labelLightScreen1.AutoSize = true;
            labelLightScreen1.BackColor = Color.Transparent;
            labelLightScreen1.DG_SVGImageColor = Color.FromArgb(207, 66, 51);
            labelLightScreen1.DG_SVGString = resources.GetString("labelLightScreen1.DG_SVGString");
            labelLightScreen1.Font = new Font("Segoe UI", 10F);
            labelLightScreen1.ForeColor = Color.Black;
            labelLightScreen1.Image = (Image)resources.GetObject("labelLightScreen1.Image");
            labelLightScreen1.ImageAlign = ContentAlignment.MiddleLeft;
            labelLightScreen1.Location = new Point(277, 7);
            labelLightScreen1.Margin = new Padding(3, 7, 3, 0);
            labelLightScreen1.Name = "labelLightScreen1";
            labelLightScreen1.Size = new Size(112, 19);
            labelLightScreen1.TabIndex = 14;
            labelLightScreen1.Text = "       Light Screen";
            labelLightScreen1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelTitle
            // 
            panelTitle.AutoSize = true;
            panelTitle.BackColor = Color.Transparent;
            panelTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            panelTitle.ForeColor = Color.Black;
            panelTitle.Location = new Point(22, 10);
            panelTitle.Name = "panelTitle";
            panelTitle.Size = new Size(118, 21);
            panelTitle.TabIndex = 0;
            panelTitle.Text = "Show in Panel";
            // 
            // FormDeviceMode
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            ClientSize = new Size(772, 489);
            Controls.Add(panelMain);
            ForeColor = Color.Gainsboro;
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormDeviceMode";
            Opacity = 0.97D;
            Padding = new Padding(1);
            Text = "FormInstallAPK";
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private Panel panel1;
        private PictureBoxBrightN PictureBoxCloseForm;
        private LabelNormalN panelTitle;
        private LabelNormalN labelWIFIMode;
        private LabelNormalN labelUOTPMode;
        private LabelNormalN labelNormaln9;
        private LabelNormalN labelLightScreen2;
        private LabelNormalN labelNormaln8;
        private LabelNormalN AlabelNormaln12;
        private LabelNormalN AlabelNormaln11;
        private LabelNormalN AlabelNormaln15;
        private LabelNormalN AlabelNormaln14;
        private LabelNormalN labelLightScreen1;
        private ToggleWithTextControl toggleHideTagPhone;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private LabelNormalN labelUSBMode;
        private LabelNormalN AlabelNormaln16;
        private LabelNormalN AlabelNormaln2;
        private LabelNormalN AlabelNormaln3;
        private ToggleWithTextControl toggleDisplayIP;
        private ToggleWithTextControl toggleAutoScreen;
        private ToggleWithTextControl toggleCtrlSelect;
        private ToggleWithTextControl toggleDisplayModel;
        private ToggleWithTextControl toggleSerial;
        private LabelNormalN labelWOTPMode;
        private LabelNormalN UHDIMode;
        private LabelNormalN AlabelNormaln21;
        private LabelNormalN AlabelNormaln20;
        private LabelNormalN AlabelNormaln19;
        private LabelNormalN labelWHDIMode;
        private LabelNormalN labelSafeMode;
        private LabelNormalN labelDelaySync;
        private LabelNormalN labelRestartADB;
        private LabelNormalN AlabelNormaln25;
        private LabelNormalN AlabelNormaln26;
        private LabelNormalN AlabelNormaln27;
        private LabelNormalN PanelMenuName;
        private LabelNormalN AlabelNormaln12sdfsd;
        private LabelNormalN AlabelNormaln1111212s;
        private LabelNormalN AlabelNormaln3df2;
        private LabelNormalN AlabelNormaln2XVE3;
        private LabelNormalN AlabelNormaln11111X;
        private LabelNormalN AlabelNormaln1411XD;
        private LabelNormalN labelNormaln1;
        private ToggleWithTextControl toggleDisplayTitle;
    }
}