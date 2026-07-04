
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;

namespace Dragon.DesignView.FormUI
{
    partial class FormDeviceControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeviceControl));
            panel1 = new PanelTransparent();
            panelMenuRight = new PanelTransparent();
            panelRound1 = new PanelRoundN();
            panelRound2 = new PanelRoundN();
            panel3 = new PanelTransparent();
            flowPanel1 = new AntdUI.FlowPanel();
            tableLayoutPanelControl = new TableLayoutPanel();
            labelImportFileFolder = new LabelHoverN();
            pictureBoxBrightn3 = new PictureBoxBrightN();
            labelInstallApkFolder = new LabelHoverN();
            pictureBoxInstallFolder = new PictureBoxBrightN();
            pictureBoxInstallAPK = new PictureBoxBrightN();
            labelADB = new LabelHoverN();
            labelInstallApk = new LabelHoverN();
            pictureBoxADB = new PictureBoxBrightN();
            pictureBoxImportFile = new PictureBoxBrightN();
            labelImportFile = new LabelHoverN();
            pictureBoxShutDown = new PictureBoxBrightN();
            labelShutDown = new LabelHoverN();
            pictureBoxTurnOnScreen = new PictureBoxBrightN();
            labelTurnOnScreen = new LabelHoverN();
            pictureBoxRotateRight = new PictureBoxBrightN();
            labelRotateRight = new LabelHoverN();
            pictureBoxReStart = new PictureBoxBrightN();
            labelRestart = new LabelHoverN();
            pictureBoxBrightn1 = new PictureBoxBrightN();
            labelVolumnUp = new LabelHoverN();
            pictureBoxBrightn2 = new PictureBoxBrightN();
            labelVolumnDown = new LabelHoverN();
            labelExportFile = new LabelHoverN();
            pictureBoxExportFile = new PictureBoxBrightN();
            labelExportApk = new LabelHoverN();
            pictureBoxExportApk = new PictureBoxBrightN();
            labelScreenShot = new LabelHoverN();
            pictureBoxScreenShot = new PictureBoxBrightN();
            pictureBoxScreenShootFolder = new PictureBoxBrightN();
            labelScreenShotFolder = new LabelHoverN();
            pictureBoxActionRecord = new PictureBoxBrightN();
            labelActionRecord = new LabelHoverN();
            pictureBoxExcuteAction = new PictureBoxBrightN();
            labelExcuteAction = new LabelHoverN();
            pictureBoxDesignFlowCode = new PictureBoxBrightN();
            labelDesignFlowAutoCode = new LabelHoverN();
            pictureBoxSwitchMode = new PictureBoxBrightN();
            labelSwithToMode = new LabelHoverN();
            panelRoundCustom1 = new PanelRoundCustomC();
            tableLayoutPanel2 = new TableLayoutPanel();
            labelAppSwitch = new LabelHoverN();
            labelHome = new LabelHoverN();
            labelBack = new LabelHoverN();
            tableLayoutPanel4 = new TableLayoutPanel();
            label3dot = new LabelHoverN();
            labelPrint = new LabelHoverN();
            labelRotate = new LabelHoverN();
            tableLayoutPanel3 = new TableLayoutPanel();
            labelStatusMode = new Label();
            panel2 = new PanelTransparent();
            tableLayoutPanel1 = new TableLayoutPanel();
            labelModelName = new Label();
            labelPhoneTagNumber = new Label();
            PictureBoxCloseForm = new PictureBoxBrightN();
            panelAuto = new PanelTransparent();
            panelMenuRight.SuspendLayout();
            panelRound1.SuspendLayout();
            panelRound2.SuspendLayout();
            panel3.SuspendLayout();
            flowPanel1.SuspendLayout();
            tableLayoutPanelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxInstallFolder).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxInstallAPK).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxADB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImportFile).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxShutDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTurnOnScreen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxRotateRight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxReStart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxExportFile).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxExportApk).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenShot).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenShootFolder).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxActionRecord).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxExcuteAction).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDesignFlowCode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSwitchMode).BeginInit();
            panelRoundCustom1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(688, 964);
            panel1.TabIndex = 0;
            // 
            // panelMenuRight
            // 
            panelMenuRight.BackColor = Color.Transparent;
            panelMenuRight.Controls.Add(panelRound1);
            panelMenuRight.Dock = DockStyle.Right;
            panelMenuRight.Location = new Point(706, 0);
            panelMenuRight.Name = "panelMenuRight";
            panelMenuRight.Size = new Size(190, 964);
            panelMenuRight.TabIndex = 2;
            // 
            // panelRound1
            // 
            panelRound1.BackColor = Color.FromArgb(97, 53, 49);
            panelRound1.Controls.Add(panelRound2);
            panelRound1.Dock = DockStyle.Fill;
            panelRound1.ForeColor = Color.Black;
            panelRound1.GD_Radius = 15F;
            panelRound1.Location = new Point(0, 0);
            panelRound1.Margin = new Padding(0);
            panelRound1.Name = "panelRound1";
            panelRound1.Padding = new Padding(2);
            panelRound1.Size = new Size(190, 964);
            panelRound1.TabIndex = 0;
            // 
            // panelRound2
            // 
            panelRound2.BackColor = Color.FromArgb(40, 40, 40);
            panelRound2.Controls.Add(panel3);
            panelRound2.Controls.Add(panelRoundCustom1);
            panelRound2.Controls.Add(tableLayoutPanel4);
            panelRound2.Controls.Add(tableLayoutPanel3);
            panelRound2.Controls.Add(panel2);
            panelRound2.Dock = DockStyle.Fill;
            panelRound2.ForeColor = Color.Black;
            panelRound2.GD_Radius = 15F;
            panelRound2.Location = new Point(2, 2);
            panelRound2.Margin = new Padding(0);
            panelRound2.Name = "panelRound2";
            panelRound2.Size = new Size(186, 960);
            panelRound2.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Transparent;
            panel3.Controls.Add(flowPanel1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 114);
            panel3.Name = "panel3";
            panel3.Size = new Size(186, 810);
            panel3.TabIndex = 5;
            // 
            // flowPanel1
            // 
            flowPanel1.AutoScroll = true;
            flowPanel1.BackColor = Color.WhiteSmoke;
            flowPanel1.Controls.Add(tableLayoutPanelControl);
            flowPanel1.Dock = DockStyle.Fill;
            flowPanel1.Location = new Point(0, 0);
            flowPanel1.Name = "flowPanel1";
            flowPanel1.Size = new Size(186, 810);
            flowPanel1.TabIndex = 0;
            flowPanel1.Text = "flowPanel1";
            // 
            // tableLayoutPanelControl
            // 
            tableLayoutPanelControl.ColumnCount = 2;
            tableLayoutPanelControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelControl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 131F));
            tableLayoutPanelControl.Controls.Add(labelImportFileFolder, 1, 10);
            tableLayoutPanelControl.Controls.Add(pictureBoxBrightn3, 0, 10);
            tableLayoutPanelControl.Controls.Add(labelInstallApkFolder, 1, 2);
            tableLayoutPanelControl.Controls.Add(pictureBoxInstallFolder, 0, 2);
            tableLayoutPanelControl.Controls.Add(pictureBoxInstallAPK, 0, 1);
            tableLayoutPanelControl.Controls.Add(labelADB, 1, 0);
            tableLayoutPanelControl.Controls.Add(labelInstallApk, 1, 1);
            tableLayoutPanelControl.Controls.Add(pictureBoxADB, 0, 0);
            tableLayoutPanelControl.Controls.Add(pictureBoxImportFile, 0, 9);
            tableLayoutPanelControl.Controls.Add(labelImportFile, 1, 9);
            tableLayoutPanelControl.Controls.Add(pictureBoxShutDown, 0, 8);
            tableLayoutPanelControl.Controls.Add(labelShutDown, 1, 8);
            tableLayoutPanelControl.Controls.Add(pictureBoxTurnOnScreen, 0, 7);
            tableLayoutPanelControl.Controls.Add(labelTurnOnScreen, 1, 7);
            tableLayoutPanelControl.Controls.Add(pictureBoxRotateRight, 0, 6);
            tableLayoutPanelControl.Controls.Add(labelRotateRight, 1, 6);
            tableLayoutPanelControl.Controls.Add(pictureBoxReStart, 0, 5);
            tableLayoutPanelControl.Controls.Add(labelRestart, 1, 5);
            tableLayoutPanelControl.Controls.Add(pictureBoxBrightn1, 0, 3);
            tableLayoutPanelControl.Controls.Add(labelVolumnUp, 1, 3);
            tableLayoutPanelControl.Controls.Add(pictureBoxBrightn2, 0, 4);
            tableLayoutPanelControl.Controls.Add(labelVolumnDown, 1, 4);
            tableLayoutPanelControl.Controls.Add(labelExportFile, 1, 11);
            tableLayoutPanelControl.Controls.Add(pictureBoxExportFile, 0, 11);
            tableLayoutPanelControl.Controls.Add(labelExportApk, 1, 12);
            tableLayoutPanelControl.Controls.Add(pictureBoxExportApk, 0, 12);
            tableLayoutPanelControl.Controls.Add(labelScreenShot, 1, 13);
            tableLayoutPanelControl.Controls.Add(pictureBoxScreenShot, 0, 13);
            tableLayoutPanelControl.Controls.Add(pictureBoxScreenShootFolder, 0, 14);
            tableLayoutPanelControl.Controls.Add(labelScreenShotFolder, 1, 14);
            tableLayoutPanelControl.Controls.Add(pictureBoxActionRecord, 0, 15);
            tableLayoutPanelControl.Controls.Add(labelActionRecord, 1, 15);
            tableLayoutPanelControl.Controls.Add(pictureBoxExcuteAction, 0, 16);
            tableLayoutPanelControl.Controls.Add(labelExcuteAction, 1, 16);
            tableLayoutPanelControl.Controls.Add(pictureBoxDesignFlowCode, 0, 17);
            tableLayoutPanelControl.Controls.Add(labelDesignFlowAutoCode, 1, 17);
            tableLayoutPanelControl.Controls.Add(pictureBoxSwitchMode, 0, 18);
            tableLayoutPanelControl.Controls.Add(labelSwithToMode, 1, 18);
            tableLayoutPanelControl.Location = new Point(3, 3);
            tableLayoutPanelControl.Name = "tableLayoutPanelControl";
            tableLayoutPanelControl.RowCount = 22;
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanelControl.Size = new Size(164, 746);
            tableLayoutPanelControl.TabIndex = 0;
            // 
            // labelImportFileFolder
            // 
            labelImportFileFolder.AutoSize = true;
            labelImportFileFolder.BackColor = Color.WhiteSmoke;
            labelImportFileFolder.ForeColor = Color.Black;
            labelImportFileFolder.ImageAlign = ContentAlignment.MiddleLeft;
            labelImportFileFolder.Location = new Point(36, 339);
            labelImportFileFolder.Margin = new Padding(3, 9, 3, 0);
            labelImportFileFolder.Name = "labelImportFileFolder";
            labelImportFileFolder.Size = new Size(100, 15);
            labelImportFileFolder.TabIndex = 44;
            labelImportFileFolder.Text = "Import File Folder";
            labelImportFileFolder.TextAlign = ContentAlignment.MiddleLeft;
            labelImportFileFolder.Click += labelImportFileFolder_Click;
            // 
            // pictureBoxBrightn3
            // 
            pictureBoxBrightn3.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxBrightn3.DG_ImageColor = SystemColors.ControlText;
            pictureBoxBrightn3.DG_IsBrightBack = false;
            pictureBoxBrightn3.DG_SVGString = resources.GetString("pictureBoxBrightn3.DG_SVGString");
            pictureBoxBrightn3.ForeColor = Color.Black;
            pictureBoxBrightn3.Image = (Image)resources.GetObject("pictureBoxBrightn3.Image");
            pictureBoxBrightn3.Location = new Point(8, 338);
            pictureBoxBrightn3.Margin = new Padding(8);
            pictureBoxBrightn3.Name = "pictureBoxBrightn3";
            pictureBoxBrightn3.Size = new Size(17, 17);
            pictureBoxBrightn3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxBrightn3.TabIndex = 43;
            pictureBoxBrightn3.TabStop = false;
            // 
            // labelInstallApkFolder
            // 
            labelInstallApkFolder.AutoSize = true;
            labelInstallApkFolder.BackColor = Color.WhiteSmoke;
            labelInstallApkFolder.ForeColor = Color.Black;
            labelInstallApkFolder.ImageAlign = ContentAlignment.MiddleLeft;
            labelInstallApkFolder.Location = new Point(36, 75);
            labelInstallApkFolder.Margin = new Padding(3, 9, 3, 0);
            labelInstallApkFolder.Name = "labelInstallApkFolder";
            labelInstallApkFolder.Size = new Size(64, 15);
            labelInstallApkFolder.TabIndex = 42;
            labelInstallApkFolder.Text = "Apk Folder";
            labelInstallApkFolder.TextAlign = ContentAlignment.MiddleLeft;
            labelInstallApkFolder.Click += labelInstallApkFolder_Click;
            // 
            // pictureBoxInstallFolder
            // 
            pictureBoxInstallFolder.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxInstallFolder.DG_ImageColor = SystemColors.ControlText;
            pictureBoxInstallFolder.DG_IsBrightBack = false;
            pictureBoxInstallFolder.DG_SVGString = resources.GetString("pictureBoxInstallFolder.DG_SVGString");
            pictureBoxInstallFolder.ForeColor = Color.Black;
            pictureBoxInstallFolder.Image = (Image)resources.GetObject("pictureBoxInstallFolder.Image");
            pictureBoxInstallFolder.Location = new Point(8, 74);
            pictureBoxInstallFolder.Margin = new Padding(8);
            pictureBoxInstallFolder.Name = "pictureBoxInstallFolder";
            pictureBoxInstallFolder.Size = new Size(17, 17);
            pictureBoxInstallFolder.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxInstallFolder.TabIndex = 41;
            pictureBoxInstallFolder.TabStop = false;
            // 
            // pictureBoxInstallAPK
            // 
            pictureBoxInstallAPK.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxInstallAPK.DG_ImageColor = SystemColors.ControlText;
            pictureBoxInstallAPK.DG_IsBrightBack = false;
            pictureBoxInstallAPK.DG_SVGString = resources.GetString("pictureBoxInstallAPK.DG_SVGString");
            pictureBoxInstallAPK.ForeColor = Color.Black;
            pictureBoxInstallAPK.Image = (Image)resources.GetObject("pictureBoxInstallAPK.Image");
            pictureBoxInstallAPK.Location = new Point(8, 41);
            pictureBoxInstallAPK.Margin = new Padding(8);
            pictureBoxInstallAPK.Name = "pictureBoxInstallAPK";
            pictureBoxInstallAPK.Size = new Size(17, 17);
            pictureBoxInstallAPK.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxInstallAPK.TabIndex = 22;
            pictureBoxInstallAPK.TabStop = false;
            // 
            // labelADB
            // 
            labelADB.AutoSize = true;
            labelADB.BackColor = Color.WhiteSmoke;
            labelADB.ForeColor = Color.Black;
            labelADB.ImageAlign = ContentAlignment.MiddleLeft;
            labelADB.Location = new Point(36, 9);
            labelADB.Margin = new Padding(3, 9, 3, 0);
            labelADB.Name = "labelADB";
            labelADB.Size = new Size(111, 15);
            labelADB.TabIndex = 0;
            labelADB.Text = "ADB                           ";
            labelADB.TextAlign = ContentAlignment.MiddleLeft;
            
            // 
            // labelInstallApk
            // 
            labelInstallApk.AutoSize = true;
            labelInstallApk.BackColor = Color.WhiteSmoke;
            labelInstallApk.ForeColor = Color.Black;
            labelInstallApk.ImageAlign = ContentAlignment.MiddleLeft;
            labelInstallApk.Location = new Point(36, 42);
            labelInstallApk.Margin = new Padding(3, 9, 3, 0);
            labelInstallApk.Name = "labelInstallApk";
            labelInstallApk.Size = new Size(111, 15);
            labelInstallApk.TabIndex = 1;
            labelInstallApk.Text = "Install APK                ";
            labelInstallApk.TextAlign = ContentAlignment.MiddleLeft;
            labelInstallApk.MouseClick += labelInstallApk_MouseClick;
         
            // 
            // pictureBoxADB
            // 
            pictureBoxADB.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxADB.DG_ImageColor = SystemColors.ControlText;
            pictureBoxADB.DG_IsBrightBack = false;
            pictureBoxADB.DG_SVGString = resources.GetString("pictureBoxADB.DG_SVGString");
            pictureBoxADB.ForeColor = Color.Black;
            pictureBoxADB.Image = (Image)resources.GetObject("pictureBoxADB.Image");
            pictureBoxADB.Location = new Point(8, 8);
            pictureBoxADB.Margin = new Padding(8);
            pictureBoxADB.Name = "pictureBoxADB";
            pictureBoxADB.Size = new Size(17, 17);
            pictureBoxADB.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxADB.TabIndex = 21;
            pictureBoxADB.TabStop = false;
            // 
            // pictureBoxImportFile
            // 
            pictureBoxImportFile.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxImportFile.DG_ImageColor = SystemColors.ControlText;
            pictureBoxImportFile.DG_IsBrightBack = false;
            pictureBoxImportFile.DG_SVGString = resources.GetString("pictureBoxImportFile.DG_SVGString");
            pictureBoxImportFile.ForeColor = Color.Black;
            pictureBoxImportFile.Image = (Image)resources.GetObject("pictureBoxImportFile.Image");
            pictureBoxImportFile.Location = new Point(8, 305);
            pictureBoxImportFile.Margin = new Padding(8);
            pictureBoxImportFile.Name = "pictureBoxImportFile";
            pictureBoxImportFile.Size = new Size(17, 17);
            pictureBoxImportFile.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImportFile.TabIndex = 27;
            pictureBoxImportFile.TabStop = false;
            // 
            // labelImportFile
            // 
            labelImportFile.AutoSize = true;
            labelImportFile.BackColor = Color.WhiteSmoke;
            labelImportFile.ForeColor = Color.Black;
            labelImportFile.ImageAlign = ContentAlignment.MiddleLeft;
            labelImportFile.Location = new Point(36, 306);
            labelImportFile.Margin = new Padding(3, 9, 3, 0);
            labelImportFile.Name = "labelImportFile";
            labelImportFile.Size = new Size(64, 15);
            labelImportFile.TabIndex = 4;
            labelImportFile.Text = "Import File";
            labelImportFile.TextAlign = ContentAlignment.MiddleLeft;
            labelImportFile.Click += labelImportFile_Click;
            // 
            // pictureBoxShutDown
            // 
            pictureBoxShutDown.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxShutDown.DG_ImageColor = SystemColors.ControlText;
            pictureBoxShutDown.DG_IsBrightBack = false;
            pictureBoxShutDown.DG_SVGString = resources.GetString("pictureBoxShutDown.DG_SVGString");
            pictureBoxShutDown.ForeColor = Color.Black;
            pictureBoxShutDown.Image = (Image)resources.GetObject("pictureBoxShutDown.Image");
            pictureBoxShutDown.Location = new Point(8, 272);
            pictureBoxShutDown.Margin = new Padding(8);
            pictureBoxShutDown.Name = "pictureBoxShutDown";
            pictureBoxShutDown.Size = new Size(17, 17);
            pictureBoxShutDown.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxShutDown.TabIndex = 26;
            pictureBoxShutDown.TabStop = false;
            // 
            // labelShutDown
            // 
            labelShutDown.AutoSize = true;
            labelShutDown.BackColor = Color.WhiteSmoke;
            labelShutDown.ForeColor = Color.Black;
            labelShutDown.ImageAlign = ContentAlignment.MiddleLeft;
            labelShutDown.Location = new Point(36, 273);
            labelShutDown.Margin = new Padding(3, 9, 3, 0);
            labelShutDown.Name = "labelShutDown";
            labelShutDown.Size = new Size(65, 15);
            labelShutDown.TabIndex = 18;
            labelShutDown.Text = "Shut Down";
            labelShutDown.TextAlign = ContentAlignment.MiddleLeft;
            labelShutDown.Click += labelShutDown_Click;
            // 
            // pictureBoxTurnOnScreen
            // 
            pictureBoxTurnOnScreen.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxTurnOnScreen.DG_ImageColor = SystemColors.ControlText;
            pictureBoxTurnOnScreen.DG_IsBrightBack = false;
            pictureBoxTurnOnScreen.DG_SVGString = resources.GetString("pictureBoxTurnOnScreen.DG_SVGString");
            pictureBoxTurnOnScreen.ForeColor = Color.Black;
            pictureBoxTurnOnScreen.Image = (Image)resources.GetObject("pictureBoxTurnOnScreen.Image");
            pictureBoxTurnOnScreen.Location = new Point(8, 239);
            pictureBoxTurnOnScreen.Margin = new Padding(8);
            pictureBoxTurnOnScreen.Name = "pictureBoxTurnOnScreen";
            pictureBoxTurnOnScreen.Size = new Size(17, 17);
            pictureBoxTurnOnScreen.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxTurnOnScreen.TabIndex = 25;
            pictureBoxTurnOnScreen.TabStop = false;
            // 
            // labelTurnOnScreen
            // 
            labelTurnOnScreen.AutoSize = true;
            labelTurnOnScreen.BackColor = Color.WhiteSmoke;
            labelTurnOnScreen.ForeColor = Color.Black;
            labelTurnOnScreen.ImageAlign = ContentAlignment.MiddleLeft;
            labelTurnOnScreen.Location = new Point(36, 240);
            labelTurnOnScreen.Margin = new Padding(3, 9, 3, 0);
            labelTurnOnScreen.Name = "labelTurnOnScreen";
            labelTurnOnScreen.Size = new Size(88, 15);
            labelTurnOnScreen.TabIndex = 15;
            labelTurnOnScreen.Text = "Turn On Screen";
            labelTurnOnScreen.TextAlign = ContentAlignment.MiddleLeft;
            labelTurnOnScreen.Click += labelTurnOnScreen_Click;
            // 
            // pictureBoxRotateRight
            // 
            pictureBoxRotateRight.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxRotateRight.DG_ImageColor = SystemColors.ControlText;
            pictureBoxRotateRight.DG_IsBrightBack = false;
            pictureBoxRotateRight.DG_SVGString = resources.GetString("pictureBoxRotateRight.DG_SVGString");
            pictureBoxRotateRight.ForeColor = Color.Black;
            pictureBoxRotateRight.Image = (Image)resources.GetObject("pictureBoxRotateRight.Image");
            pictureBoxRotateRight.Location = new Point(8, 206);
            pictureBoxRotateRight.Margin = new Padding(8);
            pictureBoxRotateRight.Name = "pictureBoxRotateRight";
            pictureBoxRotateRight.Size = new Size(17, 17);
            pictureBoxRotateRight.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxRotateRight.TabIndex = 24;
            pictureBoxRotateRight.TabStop = false;
            // 
            // labelRotateRight
            // 
            labelRotateRight.AutoSize = true;
            labelRotateRight.BackColor = Color.WhiteSmoke;
            labelRotateRight.ForeColor = Color.Black;
            labelRotateRight.ImageAlign = ContentAlignment.MiddleLeft;
            labelRotateRight.Location = new Point(36, 207);
            labelRotateRight.Margin = new Padding(3, 9, 3, 0);
            labelRotateRight.Name = "labelRotateRight";
            labelRotateRight.Size = new Size(72, 15);
            labelRotateRight.TabIndex = 3;
            labelRotateRight.Text = "Rotate Right";
            labelRotateRight.TextAlign = ContentAlignment.MiddleLeft;
            labelRotateRight.Click += labelRotateRight_Click;
            // 
            // pictureBoxReStart
            // 
            pictureBoxReStart.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxReStart.DG_ImageColor = SystemColors.ControlText;
            pictureBoxReStart.DG_IsBrightBack = false;
            pictureBoxReStart.DG_SVGString = resources.GetString("pictureBoxReStart.DG_SVGString");
            pictureBoxReStart.ForeColor = Color.Black;
            pictureBoxReStart.Image = (Image)resources.GetObject("pictureBoxReStart.Image");
            pictureBoxReStart.Location = new Point(8, 173);
            pictureBoxReStart.Margin = new Padding(8);
            pictureBoxReStart.Name = "pictureBoxReStart";
            pictureBoxReStart.Size = new Size(17, 17);
            pictureBoxReStart.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxReStart.TabIndex = 23;
            pictureBoxReStart.TabStop = false;
            // 
            // labelRestart
            // 
            labelRestart.AutoSize = true;
            labelRestart.BackColor = Color.WhiteSmoke;
            labelRestart.ForeColor = Color.Black;
            labelRestart.ImageAlign = ContentAlignment.MiddleLeft;
            labelRestart.Location = new Point(36, 174);
            labelRestart.Margin = new Padding(3, 9, 3, 0);
            labelRestart.Name = "labelRestart";
            labelRestart.Size = new Size(43, 15);
            labelRestart.TabIndex = 2;
            labelRestart.Text = "Restart";
            labelRestart.TextAlign = ContentAlignment.MiddleLeft;
            labelRestart.Click += labelRestart_Click;
            // 
            // pictureBoxBrightn1
            // 
            pictureBoxBrightn1.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxBrightn1.DG_ImageColor = SystemColors.ControlText;
            pictureBoxBrightn1.DG_IsBrightBack = false;
            pictureBoxBrightn1.DG_SVGString = resources.GetString("pictureBoxBrightn1.DG_SVGString");
            pictureBoxBrightn1.ForeColor = Color.Black;
            pictureBoxBrightn1.Image = (Image)resources.GetObject("pictureBoxBrightn1.Image");
            pictureBoxBrightn1.Location = new Point(8, 107);
            pictureBoxBrightn1.Margin = new Padding(8);
            pictureBoxBrightn1.Name = "pictureBoxBrightn1";
            pictureBoxBrightn1.Size = new Size(17, 17);
            pictureBoxBrightn1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxBrightn1.TabIndex = 37;
            pictureBoxBrightn1.TabStop = false;
            // 
            // labelVolumnUp
            // 
            labelVolumnUp.AutoSize = true;
            labelVolumnUp.BackColor = Color.WhiteSmoke;
            labelVolumnUp.ForeColor = Color.Black;
            labelVolumnUp.ImageAlign = ContentAlignment.MiddleLeft;
            labelVolumnUp.Location = new Point(36, 108);
            labelVolumnUp.Margin = new Padding(3, 9, 3, 0);
            labelVolumnUp.Name = "labelVolumnUp";
            labelVolumnUp.Size = new Size(66, 15);
            labelVolumnUp.TabIndex = 38;
            labelVolumnUp.Text = "Volumn Up";
            labelVolumnUp.TextAlign = ContentAlignment.MiddleLeft;
            labelVolumnUp.Click += labelVolumnUp_Click;
            // 
            // pictureBoxBrightn2
            // 
            pictureBoxBrightn2.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxBrightn2.DG_ImageColor = SystemColors.ControlText;
            pictureBoxBrightn2.DG_IsBrightBack = false;
            pictureBoxBrightn2.DG_SVGString = resources.GetString("pictureBoxBrightn2.DG_SVGString");
            pictureBoxBrightn2.ForeColor = Color.Black;
            pictureBoxBrightn2.Image = (Image)resources.GetObject("pictureBoxBrightn2.Image");
            pictureBoxBrightn2.Location = new Point(8, 140);
            pictureBoxBrightn2.Margin = new Padding(8);
            pictureBoxBrightn2.Name = "pictureBoxBrightn2";
            pictureBoxBrightn2.Size = new Size(17, 17);
            pictureBoxBrightn2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxBrightn2.TabIndex = 39;
            pictureBoxBrightn2.TabStop = false;
            // 
            // labelVolumnDown
            // 
            labelVolumnDown.AutoSize = true;
            labelVolumnDown.BackColor = Color.WhiteSmoke;
            labelVolumnDown.ForeColor = Color.Black;
            labelVolumnDown.ImageAlign = ContentAlignment.MiddleLeft;
            labelVolumnDown.Location = new Point(36, 141);
            labelVolumnDown.Margin = new Padding(3, 9, 3, 0);
            labelVolumnDown.Name = "labelVolumnDown";
            labelVolumnDown.Size = new Size(82, 15);
            labelVolumnDown.TabIndex = 40;
            labelVolumnDown.Text = "Volumn Down";
            labelVolumnDown.TextAlign = ContentAlignment.MiddleLeft;
            labelVolumnDown.Click += labelVolumnDown_Click;
            // 
            // labelExportFile
            // 
            labelExportFile.AutoSize = true;
            labelExportFile.BackColor = Color.WhiteSmoke;
            labelExportFile.ForeColor = Color.Black;
            labelExportFile.ImageAlign = ContentAlignment.MiddleLeft;
            labelExportFile.Location = new Point(36, 372);
            labelExportFile.Margin = new Padding(3, 9, 3, 0);
            labelExportFile.Name = "labelExportFile";
            labelExportFile.Size = new Size(62, 15);
            labelExportFile.TabIndex = 6;
            labelExportFile.Text = "Export File";
            labelExportFile.TextAlign = ContentAlignment.MiddleLeft;
            labelExportFile.Click += labelExportFile_Click;
            // 
            // pictureBoxExportFile
            // 
            pictureBoxExportFile.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxExportFile.DG_ImageColor = SystemColors.ControlText;
            pictureBoxExportFile.DG_IsBrightBack = false;
            pictureBoxExportFile.DG_SVGString = resources.GetString("pictureBoxExportFile.DG_SVGString");
            pictureBoxExportFile.ForeColor = Color.Black;
            pictureBoxExportFile.Image = (Image)resources.GetObject("pictureBoxExportFile.Image");
            pictureBoxExportFile.Location = new Point(8, 371);
            pictureBoxExportFile.Margin = new Padding(8);
            pictureBoxExportFile.Name = "pictureBoxExportFile";
            pictureBoxExportFile.Size = new Size(17, 17);
            pictureBoxExportFile.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxExportFile.TabIndex = 29;
            pictureBoxExportFile.TabStop = false;
            // 
            // labelExportApk
            // 
            labelExportApk.AutoSize = true;
            labelExportApk.BackColor = Color.WhiteSmoke;
            labelExportApk.ForeColor = Color.Black;
            labelExportApk.ImageAlign = ContentAlignment.MiddleLeft;
            labelExportApk.Location = new Point(36, 405);
            labelExportApk.Margin = new Padding(3, 9, 3, 0);
            labelExportApk.Name = "labelExportApk";
            labelExportApk.Size = new Size(65, 15);
            labelExportApk.TabIndex = 19;
            labelExportApk.Text = "Export Apk";
            labelExportApk.TextAlign = ContentAlignment.MiddleLeft;
            labelExportApk.Click += labelExportApk_Click;
            // 
            // pictureBoxExportApk
            // 
            pictureBoxExportApk.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxExportApk.DG_ImageColor = SystemColors.ControlText;
            pictureBoxExportApk.DG_IsBrightBack = false;
            pictureBoxExportApk.DG_SVGString = resources.GetString("pictureBoxExportApk.DG_SVGString");
            pictureBoxExportApk.ForeColor = Color.Black;
            pictureBoxExportApk.Image = (Image)resources.GetObject("pictureBoxExportApk.Image");
            pictureBoxExportApk.Location = new Point(8, 404);
            pictureBoxExportApk.Margin = new Padding(8);
            pictureBoxExportApk.Name = "pictureBoxExportApk";
            pictureBoxExportApk.Size = new Size(17, 17);
            pictureBoxExportApk.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxExportApk.TabIndex = 30;
            pictureBoxExportApk.TabStop = false;
            // 
            // labelScreenShot
            // 
            labelScreenShot.AutoSize = true;
            labelScreenShot.BackColor = Color.WhiteSmoke;
            labelScreenShot.ForeColor = Color.Black;
            labelScreenShot.ImageAlign = ContentAlignment.MiddleLeft;
            labelScreenShot.Location = new Point(36, 438);
            labelScreenShot.Margin = new Padding(3, 9, 3, 0);
            labelScreenShot.Name = "labelScreenShot";
            labelScreenShot.Size = new Size(65, 15);
            labelScreenShot.TabIndex = 7;
            labelScreenShot.Text = "Screenshot";
            labelScreenShot.TextAlign = ContentAlignment.MiddleLeft;
            labelScreenShot.Click += labelScreenShot_Click;
            // 
            // pictureBoxScreenShot
            // 
            pictureBoxScreenShot.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxScreenShot.DG_ImageColor = SystemColors.ControlText;
            pictureBoxScreenShot.DG_IsBrightBack = false;
            pictureBoxScreenShot.DG_SVGString = resources.GetString("pictureBoxScreenShot.DG_SVGString");
            pictureBoxScreenShot.ForeColor = Color.Black;
            pictureBoxScreenShot.Image = (Image)resources.GetObject("pictureBoxScreenShot.Image");
            pictureBoxScreenShot.Location = new Point(8, 437);
            pictureBoxScreenShot.Margin = new Padding(8);
            pictureBoxScreenShot.Name = "pictureBoxScreenShot";
            pictureBoxScreenShot.Size = new Size(17, 17);
            pictureBoxScreenShot.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxScreenShot.TabIndex = 31;
            pictureBoxScreenShot.TabStop = false;
            // 
            // pictureBoxScreenShootFolder
            // 
            pictureBoxScreenShootFolder.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxScreenShootFolder.DG_ImageColor = SystemColors.ControlText;
            pictureBoxScreenShootFolder.DG_IsBrightBack = false;
            pictureBoxScreenShootFolder.DG_SVGString = resources.GetString("pictureBoxScreenShootFolder.DG_SVGString");
            pictureBoxScreenShootFolder.ForeColor = Color.Black;
            pictureBoxScreenShootFolder.Image = (Image)resources.GetObject("pictureBoxScreenShootFolder.Image");
            pictureBoxScreenShootFolder.Location = new Point(8, 470);
            pictureBoxScreenShootFolder.Margin = new Padding(8);
            pictureBoxScreenShootFolder.Name = "pictureBoxScreenShootFolder";
            pictureBoxScreenShootFolder.Size = new Size(17, 17);
            pictureBoxScreenShootFolder.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxScreenShootFolder.TabIndex = 32;
            pictureBoxScreenShootFolder.TabStop = false;
            // 
            // labelScreenShotFolder
            // 
            labelScreenShotFolder.AutoSize = true;
            labelScreenShotFolder.BackColor = Color.WhiteSmoke;
            labelScreenShotFolder.ForeColor = Color.Black;
            labelScreenShotFolder.ImageAlign = ContentAlignment.MiddleLeft;
            labelScreenShotFolder.Location = new Point(36, 471);
            labelScreenShotFolder.Margin = new Padding(3, 9, 3, 0);
            labelScreenShotFolder.Name = "labelScreenShotFolder";
            labelScreenShotFolder.Size = new Size(108, 15);
            labelScreenShotFolder.TabIndex = 8;
            labelScreenShotFolder.Text = "Screenshoot Folder";
            labelScreenShotFolder.TextAlign = ContentAlignment.MiddleLeft;
            labelScreenShotFolder.Click += labelScreenShotFolder_Click;
            // 
            // pictureBoxActionRecord
            // 
            pictureBoxActionRecord.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxActionRecord.DG_ImageColor = SystemColors.ControlText;
            pictureBoxActionRecord.DG_IsBrightBack = false;
            pictureBoxActionRecord.DG_SVGString = resources.GetString("pictureBoxActionRecord.DG_SVGString");
            pictureBoxActionRecord.ForeColor = Color.Black;
            pictureBoxActionRecord.Image = (Image)resources.GetObject("pictureBoxActionRecord.Image");
            pictureBoxActionRecord.Location = new Point(8, 503);
            pictureBoxActionRecord.Margin = new Padding(8);
            pictureBoxActionRecord.Name = "pictureBoxActionRecord";
            pictureBoxActionRecord.Size = new Size(17, 17);
            pictureBoxActionRecord.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxActionRecord.TabIndex = 33;
            pictureBoxActionRecord.TabStop = false;
            // 
            // labelActionRecord
            // 
            labelActionRecord.AutoSize = true;
            labelActionRecord.BackColor = Color.WhiteSmoke;
            labelActionRecord.ForeColor = Color.Black;
            labelActionRecord.ImageAlign = ContentAlignment.MiddleLeft;
            labelActionRecord.Location = new Point(36, 504);
            labelActionRecord.Margin = new Padding(3, 9, 3, 0);
            labelActionRecord.Name = "labelActionRecord";
            labelActionRecord.Size = new Size(82, 15);
            labelActionRecord.TabIndex = 13;
            labelActionRecord.Text = "Action Record";
            labelActionRecord.TextAlign = ContentAlignment.MiddleLeft;
            labelActionRecord.Click += labelActionRecord_Click;
            // 
            // pictureBoxExcuteAction
            // 
            pictureBoxExcuteAction.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxExcuteAction.DG_ImageColor = SystemColors.ControlText;
            pictureBoxExcuteAction.DG_IsBrightBack = false;
            pictureBoxExcuteAction.DG_SVGString = resources.GetString("pictureBoxExcuteAction.DG_SVGString");
            pictureBoxExcuteAction.ForeColor = Color.Black;
            pictureBoxExcuteAction.Image = (Image)resources.GetObject("pictureBoxExcuteAction.Image");
            pictureBoxExcuteAction.Location = new Point(8, 536);
            pictureBoxExcuteAction.Margin = new Padding(8);
            pictureBoxExcuteAction.Name = "pictureBoxExcuteAction";
            pictureBoxExcuteAction.Size = new Size(17, 17);
            pictureBoxExcuteAction.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxExcuteAction.TabIndex = 34;
            pictureBoxExcuteAction.TabStop = false;
            // 
            // labelExcuteAction
            // 
            labelExcuteAction.AutoSize = true;
            labelExcuteAction.BackColor = Color.WhiteSmoke;
            labelExcuteAction.ForeColor = Color.Black;
            labelExcuteAction.ImageAlign = ContentAlignment.MiddleLeft;
            labelExcuteAction.Location = new Point(36, 537);
            labelExcuteAction.Margin = new Padding(3, 9, 3, 0);
            labelExcuteAction.Name = "labelExcuteAction";
            labelExcuteAction.Size = new Size(86, 15);
            labelExcuteAction.TabIndex = 14;
            labelExcuteAction.Text = "Execute Action";
            labelExcuteAction.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pictureBoxDesignFlowCode
            // 
            pictureBoxDesignFlowCode.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxDesignFlowCode.DG_ImageColor = SystemColors.ControlText;
            pictureBoxDesignFlowCode.DG_IsBrightBack = false;
            pictureBoxDesignFlowCode.DG_SVGString = resources.GetString("pictureBoxDesignFlowCode.DG_SVGString");
            pictureBoxDesignFlowCode.ForeColor = Color.Black;
            pictureBoxDesignFlowCode.Image = (Image)resources.GetObject("pictureBoxDesignFlowCode.Image");
            pictureBoxDesignFlowCode.Location = new Point(8, 569);
            pictureBoxDesignFlowCode.Margin = new Padding(8);
            pictureBoxDesignFlowCode.Name = "pictureBoxDesignFlowCode";
            pictureBoxDesignFlowCode.Size = new Size(17, 17);
            pictureBoxDesignFlowCode.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxDesignFlowCode.TabIndex = 35;
            pictureBoxDesignFlowCode.TabStop = false;
            // 
            // labelDesignFlowAutoCode
            // 
            labelDesignFlowAutoCode.AutoSize = true;
            labelDesignFlowAutoCode.BackColor = Color.WhiteSmoke;
            labelDesignFlowAutoCode.ForeColor = Color.Black;
            labelDesignFlowAutoCode.ImageAlign = ContentAlignment.MiddleLeft;
            labelDesignFlowAutoCode.Location = new Point(36, 570);
            labelDesignFlowAutoCode.Margin = new Padding(3, 9, 3, 0);
            labelDesignFlowAutoCode.Name = "labelDesignFlowAutoCode";
            labelDesignFlowAutoCode.Size = new Size(125, 15);
            labelDesignFlowAutoCode.TabIndex = 20;
            labelDesignFlowAutoCode.Text = "DesignFlow AutoCode";
            labelDesignFlowAutoCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pictureBoxSwitchMode
            // 
            pictureBoxSwitchMode.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxSwitchMode.DG_ImageColor = SystemColors.ControlText;
            pictureBoxSwitchMode.DG_IsBrightBack = false;
            pictureBoxSwitchMode.DG_SVGString = resources.GetString("pictureBoxSwitchMode.DG_SVGString");
            pictureBoxSwitchMode.ForeColor = Color.Black;
            pictureBoxSwitchMode.Image = (Image)resources.GetObject("pictureBoxSwitchMode.Image");
            pictureBoxSwitchMode.Location = new Point(8, 602);
            pictureBoxSwitchMode.Margin = new Padding(8);
            pictureBoxSwitchMode.Name = "pictureBoxSwitchMode";
            pictureBoxSwitchMode.Size = new Size(17, 17);
            pictureBoxSwitchMode.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSwitchMode.TabIndex = 36;
            pictureBoxSwitchMode.TabStop = false;
            // 
            // labelSwithToMode
            // 
            labelSwithToMode.AutoSize = true;
            labelSwithToMode.BackColor = Color.WhiteSmoke;
            labelSwithToMode.ForeColor = Color.Black;
            labelSwithToMode.ImageAlign = ContentAlignment.MiddleLeft;
            labelSwithToMode.Location = new Point(36, 603);
            labelSwithToMode.Margin = new Padding(3, 9, 3, 0);
            labelSwithToMode.Name = "labelSwithToMode";
            labelSwithToMode.Size = new Size(114, 15);
            labelSwithToMode.TabIndex = 12;
            labelSwithToMode.Text = "Switch to Mode        ";
            labelSwithToMode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelRoundCustom1
            // 
            panelRoundCustom1.BackColor = Color.WhiteSmoke;
            panelRoundCustom1.Controls.Add(tableLayoutPanel2);
            panelRoundCustom1.DG_AddLine = LineBorder.Top;
            panelRoundCustom1.DG_AddLineColor = Color.FromArgb(207, 66, 51);
            panelRoundCustom1.DG_BorderColor = Color.WhiteSmoke;
            panelRoundCustom1.DG_BorderLine = LineBorder.Bottom;
            panelRoundCustom1.DG_BorderThickness = 1;
            panelRoundCustom1.DG_Radius = 13F;
            panelRoundCustom1.Dock = DockStyle.Bottom;
            panelRoundCustom1.Location = new Point(0, 924);
            panelRoundCustom1.Margin = new Padding(0);
            panelRoundCustom1.Name = "panelRoundCustom1";
            panelRoundCustom1.Padding = new Padding(1);
            panelRoundCustom1.Size = new Size(186, 36);
            panelRoundCustom1.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.WhiteSmoke;
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5F));
            tableLayoutPanel2.Controls.Add(labelAppSwitch, 1, 0);
            tableLayoutPanel2.Controls.Add(labelHome, 2, 0);
            tableLayoutPanel2.Controls.Add(labelBack, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(1, 1);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(184, 34);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // labelAppSwitch
            // 
            labelAppSwitch.AutoSize = true;
            labelAppSwitch.BackColor = Color.WhiteSmoke;
            labelAppSwitch.DG_SVGString = resources.GetString("labelAppSwitch.DG_SVGString");
            labelAppSwitch.Dock = DockStyle.Fill;
            labelAppSwitch.ForeColor = Color.Black;
            labelAppSwitch.Image = (Image)resources.GetObject("labelAppSwitch.Image");
            labelAppSwitch.Location = new Point(12, 0);
            labelAppSwitch.Name = "labelAppSwitch";
            labelAppSwitch.Size = new Size(49, 34);
            labelAppSwitch.TabIndex = 0;
            labelAppSwitch.TextAlign = ContentAlignment.MiddleLeft;
            labelAppSwitch.Click += labelAppSwitch_Click;
            // 
            // labelHome
            // 
            labelHome.AutoSize = true;
            labelHome.BackColor = Color.WhiteSmoke;
            labelHome.DG_SVGString = resources.GetString("labelHome.DG_SVGString");
            labelHome.Dock = DockStyle.Fill;
            labelHome.Font = new Font("Segoe UI", 12F);
            labelHome.ForeColor = Color.Black;
            labelHome.Image = (Image)resources.GetObject("labelHome.Image");
            labelHome.Location = new Point(67, 0);
            labelHome.Name = "labelHome";
            labelHome.Size = new Size(49, 34);
            labelHome.TabIndex = 1;
            labelHome.TextAlign = ContentAlignment.MiddleCenter;
            labelHome.Click += labelHome_Click;
            // 
            // labelBack
            // 
            labelBack.AutoSize = true;
            labelBack.BackColor = Color.WhiteSmoke;
            labelBack.DG_SVGString = resources.GetString("labelBack.DG_SVGString");
            labelBack.Dock = DockStyle.Fill;
            labelBack.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelBack.ForeColor = Color.Black;
            labelBack.Image = (Image)resources.GetObject("labelBack.Image");
            labelBack.Location = new Point(122, 0);
            labelBack.Name = "labelBack";
            labelBack.Size = new Size(49, 34);
            labelBack.TabIndex = 2;
            labelBack.TextAlign = ContentAlignment.MiddleRight;
            labelBack.Click += labelBack_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = Color.WhiteSmoke;
            tableLayoutPanel4.ColumnCount = 4;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.Controls.Add(label3dot, 3, 0);
            tableLayoutPanel4.Controls.Add(labelPrint, 2, 0);
            tableLayoutPanel4.Controls.Add(labelRotate, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Top;
            tableLayoutPanel4.Location = new Point(0, 88);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(186, 26);
            tableLayoutPanel4.TabIndex = 3;
            // 
            // label3dot
            // 
            label3dot.AutoSize = true;
            label3dot.BackColor = Color.WhiteSmoke;
            label3dot.DG_SVGString = resources.GetString("label3dot.DG_SVGString");
            label3dot.Dock = DockStyle.Fill;
            label3dot.ForeColor = Color.Black;
            label3dot.Image = (Image)resources.GetObject("label3dot.Image");
            label3dot.Location = new Point(151, 0);
            label3dot.Name = "label3dot";
            label3dot.Size = new Size(32, 26);
            label3dot.TabIndex = 3;
            label3dot.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelPrint
            // 
            labelPrint.AutoSize = true;
            labelPrint.BackColor = Color.WhiteSmoke;
            labelPrint.DG_SVGString = resources.GetString("labelPrint.DG_SVGString");
            labelPrint.Dock = DockStyle.Fill;
            labelPrint.ForeColor = Color.Black;
            labelPrint.Image = (Image)resources.GetObject("labelPrint.Image");
            labelPrint.Location = new Point(114, 0);
            labelPrint.Name = "labelPrint";
            labelPrint.Size = new Size(31, 26);
            labelPrint.TabIndex = 2;
            labelPrint.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelRotate
            // 
            labelRotate.AutoSize = true;
            labelRotate.BackColor = Color.WhiteSmoke;
            labelRotate.DG_SVGString = resources.GetString("labelRotate.DG_SVGString");
            labelRotate.Dock = DockStyle.Fill;
            labelRotate.ForeColor = Color.White;
            labelRotate.Image = (Image)resources.GetObject("labelRotate.Image");
            labelRotate.Location = new Point(77, 0);
            labelRotate.Name = "labelRotate";
            labelRotate.Size = new Size(31, 26);
            labelRotate.TabIndex = 1;
            labelRotate.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Controls.Add(labelStatusMode, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Top;
            tableLayoutPanel3.Location = new Point(0, 60);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(186, 28);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // labelStatusMode
            // 
            labelStatusMode.AutoSize = true;
            labelStatusMode.BackColor = Color.FromArgb(54, 46, 76);
            labelStatusMode.Dock = DockStyle.Fill;
            labelStatusMode.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelStatusMode.ForeColor = Color.Gray;
            labelStatusMode.Location = new Point(0, 0);
            labelStatusMode.Margin = new Padding(0);
            labelStatusMode.Name = "labelStatusMode";
            labelStatusMode.Size = new Size(186, 28);
            labelStatusMode.TabIndex = 0;
            labelStatusMode.Text = "Individual Control Mode";
            labelStatusMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(tableLayoutPanel1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(186, 60);
            panel2.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(97, 53, 49);
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44.08602F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55.91398F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(labelModelName, 1, 0);
            tableLayoutPanel1.Controls.Add(labelPhoneTagNumber, 0, 0);
            tableLayoutPanel1.Controls.Add(PictureBoxCloseForm, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(186, 60);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // labelModelName
            // 
            labelModelName.AutoSize = true;
            labelModelName.Dock = DockStyle.Fill;
            labelModelName.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            labelModelName.ForeColor = Color.FromArgb(207, 66, 51);
            labelModelName.Location = new Point(73, 3);
            labelModelName.Margin = new Padding(0, 3, 0, 0);
            labelModelName.Name = "labelModelName";
            labelModelName.Size = new Size(92, 57);
            labelModelName.TabIndex = 1;
            labelModelName.Text = "SM-G124G";
            // 
            // labelPhoneTagNumber
            // 
            labelPhoneTagNumber.AutoSize = true;
            labelPhoneTagNumber.Dock = DockStyle.Fill;
            labelPhoneTagNumber.Font = new Font("Segoe UI", 21F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelPhoneTagNumber.ForeColor = Color.FromArgb(207, 66, 51);
            labelPhoneTagNumber.Location = new Point(0, 2);
            labelPhoneTagNumber.Margin = new Padding(0, 2, 0, 0);
            labelPhoneTagNumber.Name = "labelPhoneTagNumber";
            labelPhoneTagNumber.Size = new Size(73, 58);
            labelPhoneTagNumber.TabIndex = 2;
            labelPhoneTagNumber.Text = "01";
            // 
            // PictureBoxCloseForm
            // 
            PictureBoxCloseForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PictureBoxCloseForm.BackColor = Color.Transparent;
            PictureBoxCloseForm.DG_ImageColor = Color.FromArgb(245, 245, 245);
            PictureBoxCloseForm.DG_ImageSize = new Size(18, 18);
            PictureBoxCloseForm.DG_IsBackTransparent = true;
            PictureBoxCloseForm.DG_IsBrightBack = false;
            PictureBoxCloseForm.DG_IsWhiteImage = true;
            PictureBoxCloseForm.DG_LightenPercent = 50;
            PictureBoxCloseForm.DG_SVGString = resources.GetString("PictureBoxCloseForm.DG_SVGString");
            PictureBoxCloseForm.DG_UseMode = UseMode.ThemeStyle;
            PictureBoxCloseForm.ForeColor = Color.White;
            PictureBoxCloseForm.Image = (Image)resources.GetObject("PictureBoxCloseForm.Image");
            PictureBoxCloseForm.Location = new Point(166, 2);
            PictureBoxCloseForm.Margin = new Padding(0, 2, 2, 0);
            PictureBoxCloseForm.Name = "PictureBoxCloseForm";
            PictureBoxCloseForm.Size = new Size(18, 18);
            PictureBoxCloseForm.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBoxCloseForm.TabIndex = 9;
            PictureBoxCloseForm.TabStop = false;
            PictureBoxCloseForm.Click += PictureBoxCloseForm_Click;
            // 
            // panelAuto
            // 
            panelAuto.BackColor = Color.Transparent;
            panelAuto.Dock = DockStyle.Fill;
            panelAuto.Location = new Point(688, 0);
            panelAuto.Name = "panelAuto";
            panelAuto.Size = new Size(18, 964);
            panelAuto.TabIndex = 3;
            // 
            // FormDeviceControl
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(896, 964);
            Controls.Add(panelAuto);
            Controls.Add(panelMenuRight);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FormDeviceControl";
            StartPosition = FormStartPosition.Manual;
            Text = "FormDeviceMain";
            panelMenuRight.ResumeLayout(false);
            panelRound1.ResumeLayout(false);
            panelRound2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            flowPanel1.ResumeLayout(false);
            tableLayoutPanelControl.ResumeLayout(false);
            tableLayoutPanelControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxInstallFolder).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxInstallAPK).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxADB).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImportFile).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxShutDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTurnOnScreen).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxRotateRight).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxReStart).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxExportFile).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxExportApk).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenShot).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenShootFolder).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxActionRecord).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxExcuteAction).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDesignFlowCode).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSwitchMode).EndInit();
            panelRoundCustom1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            panel2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).EndInit();
            ResumeLayout(false);
        }

       

        #endregion
        private PanelRoundN panelRound1;
        private PanelRoundN panelRound2;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private PanelRoundCustomC panelRoundCustom1;
        private AntdUI.FlowPanel flowPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private LabelHoverN labelAppSwitch;
        private LabelHoverN labelHome;
        private LabelHoverN labelBack;
        private TableLayoutPanel tableLayoutPanelControl;
        private LabelHoverN labelRotateRight;
        private LabelHoverN labelRestart;
        private LabelHoverN labelInstallApk;
        private LabelHoverN labelADB;
        private LabelHoverN label3dot;
        private LabelHoverN labelPrint;
        private LabelHoverN labelRotate;
        public Label labelStatusMode;
        private LabelHoverN labelScreenShotFolder;
        private LabelHoverN labelScreenShot;
        private LabelHoverN labelExportFile;
        private LabelHoverN labelImportFile;
        private LabelHoverN labelSwithToMode;
        private LabelHoverN labelExcuteAction;
        private LabelHoverN labelActionRecord;
        private LabelHoverN labelTurnOnScreen;
        private LabelHoverN labelShutDown;
        private LabelHoverN labelExportApk;
        private LabelHoverN labelDesignFlowAutoCode;
        private PictureBoxBrightN pictureBoxADB;
        private PictureBoxBrightN pictureBoxSwitchMode;
        private PictureBoxBrightN pictureBoxDesignFlowCode;
        private PictureBoxBrightN pictureBoxExcuteAction;
        private PictureBoxBrightN pictureBoxActionRecord;
        private PictureBoxBrightN pictureBoxScreenShootFolder;
        private PictureBoxBrightN pictureBoxScreenShot;
        private PictureBoxBrightN pictureBoxExportApk;
        private PictureBoxBrightN pictureBoxExportFile;
        private PictureBoxBrightN pictureBoxImportFile;
        private PictureBoxBrightN pictureBoxShutDown;
        private PictureBoxBrightN pictureBoxTurnOnScreen;
        private PictureBoxBrightN pictureBoxRotateRight;
        private PictureBoxBrightN pictureBoxReStart;
        private PictureBoxBrightN pictureBoxInstallAPK;
        public Label labelModelName;
        public Label labelPhoneTagNumber;
        private LabelHoverN labelVolumnUp;
        private PictureBoxBrightN pictureBoxBrightn1;
        private LabelHoverN labelVolumnDown;
        private PictureBoxBrightN pictureBoxBrightn2;
        private LabelHoverN labelInstallApkFolder;
        private PictureBoxBrightN pictureBoxInstallFolder;
        private PictureBoxBrightN PictureBoxCloseForm;
        private LabelHoverN labelImportFileFolder;
        private PictureBoxBrightN pictureBoxBrightn3;
        public PanelTransparent panel1;
        public PanelTransparent panelMenuRight;
        private PanelTransparent panel2;
        private PanelTransparent panel3;
        public PanelTransparent panelAuto;
    }
}