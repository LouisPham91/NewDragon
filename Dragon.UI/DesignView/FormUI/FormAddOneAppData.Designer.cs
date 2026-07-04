
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormAddOneAppData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddOneAppData));
            panelMain = new Panel();
            tableLayoutPanelBottomBordern1 = new TableLayoutPanelBottomBorderN();
            labelNum = new LabelNormalN();
            comboxPackageName = new ComboBoxCustomFlatN();
            labelNormaln4 = new LabelNormalN();
            labelModel = new LabelNormalN();
            labelDeviceID = new LabelNormalN();
            comboxSocialNetwork = new ComboBoxCustomFlatN();
            labelNormaln3 = new LabelNormalN();
            panelRoundn1 = new PanelRoundN();
            buttonAddOne_AppData = new ButtomRound();
            labelFile = new LabelNormalN();
            panel1 = new Panel();
            PictureBoxCloseForm = new PictureBoxBrightN();
            labelNormaln1 = new LabelNormalN();
            panelMain.SuspendLayout();
            tableLayoutPanelBottomBordern1.SuspendLayout();
            panelRoundn1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.WhiteSmoke;
            panelMain.Controls.Add(tableLayoutPanelBottomBordern1);
            panelMain.Controls.Add(panelRoundn1);
            panelMain.Controls.Add(panel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(1, 1);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(294, 392);
            panelMain.TabIndex = 0;
            // 
            // tableLayoutPanelBottomBordern1
            // 
            tableLayoutPanelBottomBordern1.BackColor = Color.FromArgb(245, 245, 245);
            tableLayoutPanelBottomBordern1.ColumnCount = 1;
            tableLayoutPanelBottomBordern1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelBottomBordern1.Controls.Add(labelNum, 0, 0);
            tableLayoutPanelBottomBordern1.Controls.Add(comboxPackageName, 0, 6);
            tableLayoutPanelBottomBordern1.Controls.Add(labelNormaln4, 0, 5);
            tableLayoutPanelBottomBordern1.Controls.Add(labelModel, 0, 1);
            tableLayoutPanelBottomBordern1.Controls.Add(labelDeviceID, 0, 2);
            tableLayoutPanelBottomBordern1.Controls.Add(comboxSocialNetwork, 0, 4);
            tableLayoutPanelBottomBordern1.Controls.Add(labelNormaln3, 0, 3);
            tableLayoutPanelBottomBordern1.DG_BorderColor = Color.FromArgb(168, 168, 168);
            tableLayoutPanelBottomBordern1.DG_BorderThickness = 1;
            tableLayoutPanelBottomBordern1.DG_IsCellSingleBorder = false;
            tableLayoutPanelBottomBordern1.Location = new Point(22, 68);
            tableLayoutPanelBottomBordern1.Margin = new Padding(0);
            tableLayoutPanelBottomBordern1.Name = "tableLayoutPanelBottomBordern1";
            tableLayoutPanelBottomBordern1.RowCount = 7;
            tableLayoutPanelBottomBordern1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanelBottomBordern1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanelBottomBordern1.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
            tableLayoutPanelBottomBordern1.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
            tableLayoutPanelBottomBordern1.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutPanelBottomBordern1.RowStyles.Add(new RowStyle(SizeType.Absolute, 17F));
            tableLayoutPanelBottomBordern1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanelBottomBordern1.Size = new Size(250, 208);
            tableLayoutPanelBottomBordern1.TabIndex = 102;
            // 
            // labelNum
            // 
            labelNum.AutoSize = true;
            labelNum.BackColor = Color.Transparent;
            labelNum.DG_BackColor = Color.Transparent;
            labelNum.DG_IsBrightBack = false;
            labelNum.DG_IsColorMode = false;
            labelNum.DG_IsGrayImage = false;
            labelNum.DG_LightenPercent = 80;
            labelNum.DG_SVGImageColor = Color.Transparent;
            labelNum.DG_SVGString = "";
            labelNum.ForeColor = Color.Black;
            labelNum.Location = new Point(3, 0);
            labelNum.Name = "labelNum";
            labelNum.Size = new Size(51, 15);
            labelNum.TabIndex = 99;
            labelNum.Text = "Number";
            // 
            // comboxPackageName
            // 
            comboxPackageName.BackColor = Color.DimGray;
            comboxPackageName.DG_ArrowColor = Color.Black;
            comboxPackageName.DG_ArrowHeight = 1.5F;
            comboxPackageName.DG_ArrowWidth = 3F;
            comboxPackageName.DG_BorderColor = Color.Black;
            comboxPackageName.DG_BorderSize = 0;
            comboxPackageName.DrawMode = DrawMode.OwnerDrawFixed;
            comboxPackageName.DropDownStyle = ComboBoxStyle.DropDownList;
            comboxPackageName.FlatStyle = FlatStyle.Flat;
            comboxPackageName.ForeColor = Color.Black;
            comboxPackageName.FormattingEnabled = true;
            comboxPackageName.Location = new Point(3, 168);
            comboxPackageName.Name = "comboxPackageName";
            comboxPackageName.Size = new Size(244, 24);
            comboxPackageName.TabIndex = 96;
            // 
            // labelNormaln4
            // 
            labelNormaln4.AutoSize = true;
            labelNormaln4.BackColor = Color.Transparent;
            labelNormaln4.DG_BackColor = Color.Transparent;
            labelNormaln4.DG_IsBrightBack = false;
            labelNormaln4.DG_IsColorMode = false;
            labelNormaln4.DG_IsGrayImage = false;
            labelNormaln4.DG_LightenPercent = 80;
            labelNormaln4.DG_SVGImageColor = Color.Transparent;
            labelNormaln4.DG_SVGString = "";
            labelNormaln4.ForeColor = Color.Black;
            labelNormaln4.Location = new Point(3, 148);
            labelNormaln4.Name = "labelNormaln4";
            labelNormaln4.Size = new Size(83, 15);
            labelNormaln4.TabIndex = 97;
            labelNormaln4.Text = "PackageName";
            // 
            // labelModel
            // 
            labelModel.AutoSize = true;
            labelModel.BackColor = Color.Transparent;
            labelModel.DG_BackColor = Color.Transparent;
            labelModel.DG_IsBrightBack = false;
            labelModel.DG_IsColorMode = false;
            labelModel.DG_IsGrayImage = false;
            labelModel.DG_LightenPercent = 80;
            labelModel.DG_SVGImageColor = Color.Transparent;
            labelModel.DG_SVGString = "";
            labelModel.ForeColor = Color.Black;
            labelModel.Location = new Point(3, 30);
            labelModel.Name = "labelModel";
            labelModel.Size = new Size(41, 15);
            labelModel.TabIndex = 101;
            labelModel.Text = "Model";
            // 
            // labelDeviceID
            // 
            labelDeviceID.AutoSize = true;
            labelDeviceID.BackColor = Color.Transparent;
            labelDeviceID.DG_BackColor = Color.Transparent;
            labelDeviceID.DG_IsBrightBack = false;
            labelDeviceID.DG_IsColorMode = false;
            labelDeviceID.DG_IsGrayImage = false;
            labelDeviceID.DG_LightenPercent = 80;
            labelDeviceID.DG_SVGImageColor = Color.Transparent;
            labelDeviceID.DG_SVGString = "";
            labelDeviceID.ForeColor = Color.Black;
            labelDeviceID.Location = new Point(3, 60);
            labelDeviceID.Name = "labelDeviceID";
            labelDeviceID.Size = new Size(53, 15);
            labelDeviceID.TabIndex = 5;
            labelDeviceID.Text = "DeviceID";
            // 
            // comboxSocialNetwork
            // 
            comboxSocialNetwork.BackColor = Color.DimGray;
            comboxSocialNetwork.DG_ArrowColor = Color.Black;
            comboxSocialNetwork.DG_ArrowHeight = 1.5F;
            comboxSocialNetwork.DG_ArrowWidth = 3F;
            comboxSocialNetwork.DG_BorderColor = Color.Black;
            comboxSocialNetwork.DG_BorderSize = 0;
            comboxSocialNetwork.DrawMode = DrawMode.OwnerDrawFixed;
            comboxSocialNetwork.DropDownStyle = ComboBoxStyle.DropDownList;
            comboxSocialNetwork.FlatStyle = FlatStyle.Flat;
            comboxSocialNetwork.ForeColor = Color.Black;
            comboxSocialNetwork.FormattingEnabled = true;
            comboxSocialNetwork.Location = new Point(3, 113);
            comboxSocialNetwork.Name = "comboxSocialNetwork";
            comboxSocialNetwork.Size = new Size(244, 24);
            comboxSocialNetwork.TabIndex = 95;
            // 
            // labelNormaln3
            // 
            labelNormaln3.AutoSize = true;
            labelNormaln3.BackColor = Color.Transparent;
            labelNormaln3.DG_BackColor = Color.Transparent;
            labelNormaln3.DG_IsBrightBack = false;
            labelNormaln3.DG_IsColorMode = false;
            labelNormaln3.DG_IsGrayImage = false;
            labelNormaln3.DG_LightenPercent = 80;
            labelNormaln3.DG_SVGImageColor = Color.Transparent;
            labelNormaln3.DG_SVGString = "";
            labelNormaln3.ForeColor = Color.Black;
            labelNormaln3.Location = new Point(3, 91);
            labelNormaln3.Name = "labelNormaln3";
            labelNormaln3.Size = new Size(86, 15);
            labelNormaln3.TabIndex = 7;
            labelNormaln3.Text = "Social Network";
            // 
            // panelRoundn1
            // 
            panelRoundn1.Controls.Add(buttonAddOne_AppData);
            panelRoundn1.Controls.Add(labelFile);
            panelRoundn1.Dock = DockStyle.Bottom;
            panelRoundn1.GD_Radius = 15F;
            panelRoundn1.Location = new Point(0, 315);
            panelRoundn1.Name = "panelRoundn1";
            panelRoundn1.Size = new Size(294, 77);
            panelRoundn1.TabIndex = 3;
            // 
            // buttonAddOne_AppData
            // 
            buttonAddOne_AppData.BackColor = Color.FromArgb(225, 225, 225);
            buttonAddOne_AppData.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonAddOne_AppData.DG_ForceColor = Color.Empty;
            buttonAddOne_AppData.DG_LightenPercent = 0.2F;
            buttonAddOne_AppData.DG_SVGString = "";
            buttonAddOne_AppData.FlatAppearance.BorderSize = 0;
            buttonAddOne_AppData.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonAddOne_AppData.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonAddOne_AppData.FlatStyle = FlatStyle.Flat;
            buttonAddOne_AppData.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonAddOne_AppData.ForeColor = Color.FromArgb(255, 70, 70);
            buttonAddOne_AppData.GD_Radius = 5F;
            buttonAddOne_AppData.ImageAlign = ContentAlignment.MiddleLeft;
            buttonAddOne_AppData.Location = new Point(56, 24);
            buttonAddOne_AppData.Name = "buttonAddOne_AppData";
            buttonAddOne_AppData.Padding = new Padding(25, 0, 20, 0);
            buttonAddOne_AppData.Size = new Size(161, 30);
            buttonAddOne_AppData.TabIndex = 18;
            buttonAddOne_AppData.TabStop = false;
            buttonAddOne_AppData.Text = "Add Now";
            buttonAddOne_AppData.UseVisualStyleBackColor = false;
            buttonAddOne_AppData.Click += buttonAddOne_AppData_Click;
            // 
            // labelFile
            // 
            labelFile.AutoSize = true;
            labelFile.BackColor = Color.Transparent;
            labelFile.DG_BackColor = Color.Transparent;
            labelFile.DG_IsBrightBack = true;
            labelFile.DG_IsColorMode = false;
            labelFile.DG_IsGrayImage = false;
            labelFile.DG_LightenPercent = 80;
            labelFile.DG_SVGImageColor = Color.Transparent;
            labelFile.DG_SVGString = "";
            labelFile.Font = new Font("Segoe UI", 9.75F);
            labelFile.ForeColor = Color.Black;
            labelFile.Location = new Point(22, 14);
            labelFile.Name = "labelFile";
            labelFile.Size = new Size(0, 17);
            labelFile.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(PictureBoxCloseForm);
            panel1.Controls.Add(labelNormaln1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(294, 40);
            panel1.TabIndex = 0;
            // 
            // PictureBoxCloseForm
            // 
            PictureBoxCloseForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PictureBoxCloseForm.BackColor = Color.FromArgb(245, 245, 245);
            PictureBoxCloseForm.DG_ImageColor = Color.Black;
            PictureBoxCloseForm.DG_ImageSize = new Size(18, 18);
            PictureBoxCloseForm.DG_IsBackTransparent = false;
            PictureBoxCloseForm.DG_IsBrightBack = true;
            PictureBoxCloseForm.DG_IsGrayImage = false;
            PictureBoxCloseForm.DG_IsWhiteImage = false;
            PictureBoxCloseForm.DG_LightenPercent = 50;
            PictureBoxCloseForm.DG_SVGString = resources.GetString("PictureBoxCloseForm.DG_SVGString");
            PictureBoxCloseForm.DG_UseMode = UseMode.ThemeMode;
            PictureBoxCloseForm.ForeColor = Color.Black;
            PictureBoxCloseForm.Image = (Image)resources.GetObject("PictureBoxCloseForm.Image");
            PictureBoxCloseForm.Location = new Point(256, 10);
            PictureBoxCloseForm.Name = "PictureBoxCloseForm";
            PictureBoxCloseForm.Padding = new Padding(2);
            PictureBoxCloseForm.Size = new Size(18, 18);
            PictureBoxCloseForm.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBoxCloseForm.TabIndex = 8;
            PictureBoxCloseForm.TabStop = false;
            PictureBoxCloseForm.Click += PictureBoxCloseForm_Click;
            // 
            // labelNormaln1
            // 
            labelNormaln1.AutoSize = true;
            labelNormaln1.BackColor = Color.Transparent;
            labelNormaln1.DG_BackColor = Color.Transparent;
            labelNormaln1.DG_IsBrightBack = false;
            labelNormaln1.DG_IsColorMode = false;
            labelNormaln1.DG_IsGrayImage = false;
            labelNormaln1.DG_LightenPercent = 80;
            labelNormaln1.DG_SVGImageColor = Color.Transparent;
            labelNormaln1.DG_SVGString = "";
            labelNormaln1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNormaln1.ForeColor = Color.Black;
            labelNormaln1.Location = new Point(22, 10);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(136, 20);
            labelNormaln1.TabIndex = 0;
            labelNormaln1.Text = "Add One AppData";
            // 
            // FormAddOneAppData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(168, 168, 168);
            ClientSize = new Size(296, 394);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormAddOneAppData";
            Opacity = 0.97D;
            Padding = new Padding(1);
            Text = "FormInstallAPK";
            Load += FormAndroidExplorer_Load;
            panelMain.ResumeLayout(false);
            tableLayoutPanelBottomBordern1.ResumeLayout(false);
            tableLayoutPanelBottomBordern1.PerformLayout();
            panelRoundn1.ResumeLayout(false);
            panelRoundn1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private Panel panel1;
        private PictureBoxBrightN PictureBoxCloseForm;
        private LabelNormalN labelFile;
        private PanelRoundN panelRoundn1;
        private ButtomRound buttonAddOne_AppData;
        private LabelNormalN labelNormaln1;
        private LabelNormalN labelDeviceID;
        private LabelNormalN labelNormaln3;
        private LabelNormalN labelNormaln4;
        private ComboBoxCustomFlatN comboxPackageName;
        private ComboBoxCustomFlatN comboxSocialNetwork;
        private LabelNormalN labelNum;
        private LabelNormalN labelModel;
        private TableLayoutPanelBottomBorderN tableLayoutPanelBottomBordern1;
    }
}