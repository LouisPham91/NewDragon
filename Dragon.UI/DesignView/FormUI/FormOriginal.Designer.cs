

using Dragon.DesignView.Public;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormOriginal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOriginal));
            panelMain = new PanelRoundN();
            panelBottomBordern1 = new PanelBottomBorderN();
            PictureBoxFullScreen = new PictureBoxBrightN();
            PictureBoxMiniminze = new PictureBoxBrightN();
            PictureBoxCloseForm = new PictureBoxBrightN();
            labelTitle = new LabelNormalN();
            pictureBoxIcon = new PictureBoxBrightN();
            panelMain.SuspendLayout();
            panelBottomBordern1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxFullScreen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxMiniminze).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.White;
            panelMain.Controls.Add(panelBottomBordern1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.ForeColor = Color.Black;
            panelMain.GD_Radius = 15F;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(736, 544);
            panelMain.TabIndex = 0;
            // 
            // panelBottomBordern1
            // 
            panelBottomBordern1.BackColor = Color.White;
            panelBottomBordern1.Controls.Add(PictureBoxFullScreen);
            panelBottomBordern1.Controls.Add(PictureBoxMiniminze);
            panelBottomBordern1.Controls.Add(PictureBoxCloseForm);
            panelBottomBordern1.Controls.Add(labelTitle);
            panelBottomBordern1.Controls.Add(pictureBoxIcon);
            panelBottomBordern1.DG_BackColor = Color.White;
            panelBottomBordern1.DG_BorderColor = Color.FromArgb(168, 168, 168);
            panelBottomBordern1.Dock = DockStyle.Top;
            panelBottomBordern1.ForeColor = Color.Black;
            panelBottomBordern1.Location = new Point(0, 0);
            panelBottomBordern1.Name = "panelBottomBordern1";
            panelBottomBordern1.Size = new Size(736, 34);
            panelBottomBordern1.TabIndex = 0;
            panelBottomBordern1.MouseDown += PanelMouseDown;
            // 
            // PictureBoxFullScreen
            // 
            PictureBoxFullScreen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PictureBoxFullScreen.BackColor = Color.Transparent;
            PictureBoxFullScreen.DG_ImageColor = Color.Black;
            PictureBoxFullScreen.DG_ImageSize = new Size(18, 18);
            PictureBoxFullScreen.DG_IsBackTransparent = true;
            PictureBoxFullScreen.DG_LightenPercent = 50;
            PictureBoxFullScreen.DG_SVGString = resources.GetString("PictureBoxFullScreen.DG_SVGString");
            PictureBoxFullScreen.ForeColor = Color.Black;
            PictureBoxFullScreen.Location = new Point(668, 8);
            PictureBoxFullScreen.Name = "PictureBoxFullScreen";
            PictureBoxFullScreen.Padding = new Padding(2);
            PictureBoxFullScreen.Size = new Size(18, 18);
            PictureBoxFullScreen.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBoxFullScreen.TabIndex = 8;
            PictureBoxFullScreen.TabStop = false;
            PictureBoxFullScreen.Click += PictureBoxFullScreen_Click;
            // 
            // PictureBoxMiniminze
            // 
            PictureBoxMiniminze.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PictureBoxMiniminze.BackColor = Color.Transparent;
            PictureBoxMiniminze.DG_ImageColor = Color.Black;
            PictureBoxMiniminze.DG_ImageSize = new Size(18, 18);
            PictureBoxMiniminze.DG_IsBackTransparent = true;
            PictureBoxMiniminze.DG_LightenPercent = 50;
            PictureBoxMiniminze.DG_SVGString = resources.GetString("PictureBoxMiniminze.DG_SVGString");
            PictureBoxMiniminze.ForeColor = Color.Black;
            PictureBoxMiniminze.Location = new Point(630, 8);
            PictureBoxMiniminze.Name = "PictureBoxMiniminze";
            PictureBoxMiniminze.Padding = new Padding(2);
            PictureBoxMiniminze.Size = new Size(18, 18);
            PictureBoxMiniminze.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBoxMiniminze.TabIndex = 9;
            PictureBoxMiniminze.TabStop = false;
            PictureBoxMiniminze.Click += PictureBoxMiniminze_Click;
            // 
            // PictureBoxCloseForm
            // 
            PictureBoxCloseForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PictureBoxCloseForm.BackColor = Color.Transparent;
            PictureBoxCloseForm.DG_ImageColor = Color.Black;
            PictureBoxCloseForm.DG_ImageSize = new Size(18, 18);
            PictureBoxCloseForm.DG_IsBackTransparent = true;
            PictureBoxCloseForm.DG_LightenPercent = 50;
            PictureBoxCloseForm.DG_SVGString = resources.GetString("PictureBoxCloseForm.DG_SVGString");
            PictureBoxCloseForm.ForeColor = Color.Black;
            PictureBoxCloseForm.Location = new Point(706, 8);
            PictureBoxCloseForm.Name = "PictureBoxCloseForm";
            PictureBoxCloseForm.Padding = new Padding(2);
            PictureBoxCloseForm.Size = new Size(18, 18);
            PictureBoxCloseForm.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBoxCloseForm.TabIndex = 7;
            PictureBoxCloseForm.TabStop = false;
            PictureBoxCloseForm.Click += PictureBoxCloseForm_Click;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.BackColor = Color.Transparent;
            labelTitle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitle.ForeColor = Color.Black;
            labelTitle.Location = new Point(52, 9);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(133, 17);
            labelTitle.TabIndex = 6;
            labelTitle.Text = "Title Template Form";
            // 
            // pictureBoxIcon
            // 
            pictureBoxIcon.BackColor = Color.Transparent;
            pictureBoxIcon.DG_ImageColor = Color.Black;
            pictureBoxIcon.DG_IsBackTransparent = true;
            pictureBoxIcon.DG_IsBrightBack = false;
            pictureBoxIcon.DG_LightenPercent = 1;
            pictureBoxIcon.ForeColor = Color.Black;
            pictureBoxIcon.Location = new Point(17, 6);
            pictureBoxIcon.Name = "pictureBoxIcon";
            pictureBoxIcon.Size = new Size(22, 22);
            pictureBoxIcon.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxIcon.TabIndex = 5;
            pictureBoxIcon.TabStop = false;
            // 
            // FormOriginal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(168, 168, 168);
            ClientSize = new Size(736, 544);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormOriginal";
            Text = "PanelOriginal";
            panelMain.ResumeLayout(false);
            panelBottomBordern1.ResumeLayout(false);
            panelBottomBordern1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxFullScreen).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxMiniminze).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).EndInit();
            ResumeLayout(false);
        }

        #endregion

        public PanelRoundN panelMain;
        private PictureBoxBrightN PictureBoxFullScreen;
        private PictureBoxBrightN PictureBoxMiniminze;
        private PictureBoxBrightN PictureBoxCloseForm;
        public LabelNormalN labelTitle;
        public PictureBoxBrightN pictureBoxIcon;
        public PanelBottomBorderN panelBottomBordern1;
    }
}