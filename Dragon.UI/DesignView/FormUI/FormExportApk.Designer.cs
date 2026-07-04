
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormExportApk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportApk));
            panelMain = new Panel();
            tree1 = new AntdUI.Tree();
            panelRoundn1 = new PanelRoundN();
            buttomApkFolder = new ButtomRound();
            buttonExportFile = new ButtomRound();
            labelFileApk = new LabelNormalN();
            panel1 = new Panel();
            textBoxSearch = new TextBoxNoborberN();
            pictureBoxBrightn1 = new PictureBoxBrightN();
            PictureBoxCloseForm = new PictureBoxBrightN();
            labelNormaln1 = new LabelNormalN();
            panelMain.SuspendLayout();
            panelRoundn1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.WhiteSmoke;
            panelMain.Controls.Add(tree1);
            panelMain.Controls.Add(panelRoundn1);
            panelMain.Controls.Add(panel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(1, 1);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(839, 768);
            panelMain.TabIndex = 0;
            // 
            // tree1
            // 
            tree1.Dock = DockStyle.Fill;
            tree1.Gap = 4;
            tree1.Location = new Point(0, 40);
            tree1.Name = "tree1";
            tree1.Size = new Size(839, 611);
            tree1.TabIndex = 4;
            tree1.Text = "tree1";
            // 
            // panelRoundn1
            // 
            panelRoundn1.Controls.Add(buttomApkFolder);
            panelRoundn1.Controls.Add(buttonExportFile);
            panelRoundn1.Controls.Add(labelFileApk);
            panelRoundn1.Dock = DockStyle.Bottom;
            panelRoundn1.GD_Radius = 15F;
            panelRoundn1.Location = new Point(0, 651);
            panelRoundn1.Name = "panelRoundn1";
            panelRoundn1.Size = new Size(839, 117);
            panelRoundn1.TabIndex = 3;
            // 
            // buttomApkFolder
            // 
            buttomApkFolder.BackColor = Color.FromArgb(225, 225, 225);
            buttomApkFolder.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttomApkFolder.DG_ForceColor = Color.Empty;
            buttomApkFolder.DG_LightenPercent = 0.2F;
            buttomApkFolder.DG_SVGString = "";
            buttomApkFolder.FlatAppearance.BorderSize = 0;
            buttomApkFolder.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttomApkFolder.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttomApkFolder.FlatStyle = FlatStyle.Flat;
            buttomApkFolder.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttomApkFolder.ForeColor = Color.FromArgb(255, 70, 70);
            buttomApkFolder.GD_Radius = 5F;
            buttomApkFolder.ImageAlign = ContentAlignment.MiddleLeft;
            buttomApkFolder.Location = new Point(646, 60);
            buttomApkFolder.Name = "buttomApkFolder";
            buttomApkFolder.Padding = new Padding(25, 0, 20, 0);
            buttomApkFolder.Size = new Size(173, 30);
            buttomApkFolder.TabIndex = 19;
            buttomApkFolder.TabStop = false;
            buttomApkFolder.Text = "Apk Export Folder";
            buttomApkFolder.UseVisualStyleBackColor = false;
            buttomApkFolder.Click += buttomApkFolder_Click;
            // 
            // buttonExportFile
            // 
            buttonExportFile.BackColor = Color.FromArgb(225, 225, 225);
            buttonExportFile.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonExportFile.DG_ForceColor = Color.Empty;
            buttonExportFile.DG_LightenPercent = 0.2F;
            buttonExportFile.DG_SVGString = "";
            buttonExportFile.FlatAppearance.BorderSize = 0;
            buttonExportFile.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonExportFile.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonExportFile.FlatStyle = FlatStyle.Flat;
            buttonExportFile.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonExportFile.ForeColor = Color.FromArgb(255, 70, 70);
            buttonExportFile.GD_Radius = 5F;
            buttonExportFile.ImageAlign = ContentAlignment.MiddleLeft;
            buttonExportFile.Location = new Point(452, 60);
            buttonExportFile.Name = "buttonExportFile";
            buttonExportFile.Padding = new Padding(25, 0, 20, 0);
            buttonExportFile.Size = new Size(173, 30);
            buttonExportFile.TabIndex = 18;
            buttonExportFile.TabStop = false;
            buttonExportFile.Text = "Export Apk";
            buttonExportFile.UseVisualStyleBackColor = false;
            buttonExportFile.Click += buttonExportFile_Click;
            // 
            // labelFileApk
            // 
            labelFileApk.AutoSize = true;
            labelFileApk.BackColor = Color.Transparent;
            labelFileApk.DG_BackColor = Color.Transparent;
            labelFileApk.DG_LightenPercent = 80;
            labelFileApk.DG_SVGString = "";
            labelFileApk.Font = new Font("Segoe UI", 9.75F);
            labelFileApk.DG_IsBrightBack = true;
            labelFileApk.DG_IsColorMode = false;
            labelFileApk.DG_IsGrayImage = false;
            labelFileApk.Location = new Point(22, 14);
            labelFileApk.Name = "labelFileApk";
            labelFileApk.Size = new Size(0, 17);
            labelFileApk.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(textBoxSearch);
            panel1.Controls.Add(pictureBoxBrightn1);
            panel1.Controls.Add(PictureBoxCloseForm);
            panel1.Controls.Add(labelNormaln1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(839, 40);
            panel1.TabIndex = 0;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxSearch.BackColor = SystemColors.Window;
            textBoxSearch.BorderStyle = BorderStyle.FixedSingle;
            textBoxSearch.DG_BorderThickness = 1;
            textBoxSearch.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxSearch.Location = new Point(552, 9);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(224, 23);
            textBoxSearch.TabIndex = 10;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            // 
            // pictureBoxBrightn1
            // 
            pictureBoxBrightn1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBoxBrightn1.DG_ImageColor = SystemColors.ControlText;
            pictureBoxBrightn1.DG_ImageSize = new Size(18, 18);
            pictureBoxBrightn1.DG_LightenPercent = 50;
            pictureBoxBrightn1.Image = (Image)resources.GetObject("pictureBoxBrightn1.Image");
            pictureBoxBrightn1.DG_IsBackTransparent = false;
            pictureBoxBrightn1.DG_IsBrightBack = true;
            pictureBoxBrightn1.DG_IsGrayImage = false;
            pictureBoxBrightn1.DG_IsWhiteImage = false;
            pictureBoxBrightn1.Location = new Point(528, 11);
            pictureBoxBrightn1.Name = "pictureBoxBrightn1";
            pictureBoxBrightn1.Padding = new Padding(2);
            pictureBoxBrightn1.Size = new Size(18, 18);
            pictureBoxBrightn1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxBrightn1.DG_SVGString = resources.GetString("pictureBoxBrightn1.SVGstring");
            pictureBoxBrightn1.TabIndex = 9;
            pictureBoxBrightn1.TabStop = false;
            pictureBoxBrightn1.DG_UseMode = UseMode.ThemeMode;
            // 
            // PictureBoxCloseForm
            // 
            PictureBoxCloseForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PictureBoxCloseForm.DG_ImageColor = SystemColors.ControlText;
            PictureBoxCloseForm.DG_ImageSize = new Size(18, 18);
            PictureBoxCloseForm.DG_LightenPercent = 50;
            PictureBoxCloseForm.Image = (Image)resources.GetObject("PictureBoxCloseForm.Image");
            PictureBoxCloseForm.DG_IsBackTransparent = false;
            PictureBoxCloseForm.DG_IsBrightBack = true;
            PictureBoxCloseForm.DG_IsGrayImage = false;
            PictureBoxCloseForm.DG_IsWhiteImage = false;
            PictureBoxCloseForm.Location = new Point(801, 10);
            PictureBoxCloseForm.Name = "PictureBoxCloseForm";
            PictureBoxCloseForm.Padding = new Padding(2);
            PictureBoxCloseForm.Size = new Size(18, 18);
            PictureBoxCloseForm.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBoxCloseForm.DG_SVGString = resources.GetString("PictureBoxCloseForm.SVGstring");
            PictureBoxCloseForm.TabIndex = 8;
            PictureBoxCloseForm.TabStop = false;
            PictureBoxCloseForm.DG_UseMode = UseMode.ThemeMode;
            PictureBoxCloseForm.Click += PictureBoxCloseForm_Click;
            // 
            // labelNormaln1
            // 
            labelNormaln1.AutoSize = true;
            labelNormaln1.BackColor = Color.Transparent;
            labelNormaln1.DG_BackColor = Color.Transparent;
            labelNormaln1.DG_LightenPercent = 80;
            labelNormaln1.DG_SVGString = "";
            labelNormaln1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNormaln1.DG_IsBrightBack = false;
            labelNormaln1.DG_IsColorMode = false;
            labelNormaln1.DG_IsGrayImage = false;
            labelNormaln1.Location = new Point(22, 10);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(162, 20);
            labelNormaln1.TabIndex = 0;
            labelNormaln1.Text = "Android Explorer APK";
            // 
            // FormExportApk
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(168, 168, 168);
            ClientSize = new Size(841, 770);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormExportApk";
            Opacity = 0.97D;
            Padding = new Padding(1);
            Text = "FormInstallAPK";
            panelMain.ResumeLayout(false);
            panelRoundn1.ResumeLayout(false);
            panelRoundn1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private Panel panel1;
        private PictureBoxBrightN PictureBoxCloseForm;
        private LabelNormalN labelFileApk;
        private PanelRoundN panelRoundn1;
        private ButtomRound buttonExportFile;
        private LabelNormalN labelNormaln1;
        private ButtomRound buttomApkFolder;
        private AntdUI.Tree tree1;
        private PictureBoxBrightN pictureBoxBrightn1;
        private TextBoxNoborberN textBoxSearch;
    }
}