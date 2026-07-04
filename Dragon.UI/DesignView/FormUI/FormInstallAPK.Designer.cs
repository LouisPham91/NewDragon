
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormInstallAPK
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInstallAPK));
            panelMain = new Panel();
            flowLayoutPanel1 = new AntdUI.In.FlowLayoutPanel();
            panelRoundn1 = new PanelRoundN();
            buttonStartInstall = new ButtomRound();
            labelFileApk = new LabelNormalN();
            panel1 = new Panel();
            PictureBoxCloseForm = new PictureBoxBrightN();
            labelTotalPhone = new LabelNormalN();
            labelNormaln1 = new LabelNormalN();
            panelMain.SuspendLayout();
            panelRoundn1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.WhiteSmoke;
            panelMain.Controls.Add(flowLayoutPanel1);
            panelMain.Controls.Add(panelRoundn1);
            panelMain.Controls.Add(panel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(1, 1);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(853, 507);
            panelMain.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 40);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(20, 2, 20, 2);
            flowLayoutPanel1.Size = new Size(853, 380);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // panelRoundn1
            // 
            panelRoundn1.BackColor = Color.FromArgb(245, 245, 245);
            panelRoundn1.Controls.Add(buttonStartInstall);
            panelRoundn1.Controls.Add(labelFileApk);
            panelRoundn1.Dock = DockStyle.Bottom;
            panelRoundn1.ForeColor = Color.Black;
            panelRoundn1.GD_Radius = 15F;
            panelRoundn1.Location = new Point(0, 420);
            panelRoundn1.Name = "panelRoundn1";
            panelRoundn1.Size = new Size(853, 87);
            panelRoundn1.TabIndex = 3;
            // 
            // buttonStartInstall
            // 
            buttonStartInstall.BackColor = Color.FromArgb(225, 225, 225);
            buttonStartInstall.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonStartInstall.DG_ForceColor = Color.Empty;
            buttonStartInstall.FlatAppearance.BorderSize = 0;
            buttonStartInstall.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonStartInstall.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonStartInstall.FlatStyle = FlatStyle.Flat;
            buttonStartInstall.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonStartInstall.ForeColor = Color.FromArgb(255, 70, 70);
            buttonStartInstall.GD_Radius = 5F;
            buttonStartInstall.ImageAlign = ContentAlignment.MiddleLeft;
            buttonStartInstall.Location = new Point(647, 35);
            buttonStartInstall.Name = "buttonStartInstall";
            buttonStartInstall.Padding = new Padding(25, 0, 20, 0);
            buttonStartInstall.Size = new Size(173, 30);
            buttonStartInstall.TabIndex = 18;
            buttonStartInstall.TabStop = false;
            buttonStartInstall.Text = "Start Installation";
            buttonStartInstall.UseVisualStyleBackColor = false;
            buttonStartInstall.Click += buttonStartInstall_Click;
            // 
            // labelFileApk
            // 
            labelFileApk.AutoSize = true;
            labelFileApk.BackColor = Color.Transparent;
            labelFileApk.Font = new Font("Segoe UI", 9.75F);
            labelFileApk.ForeColor = Color.Black;
            labelFileApk.Location = new Point(22, 14);
            labelFileApk.Name = "labelFileApk";
            labelFileApk.Size = new Size(238, 17);
            labelFileApk.TabIndex = 0;
            labelFileApk.Text = "Extension\\App\\com.faceboo.katana.apk";
            // 
            // panel1
            // 
            panel1.Controls.Add(PictureBoxCloseForm);
            panel1.Controls.Add(labelTotalPhone);
            panel1.Controls.Add(labelNormaln1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(853, 40);
            panel1.TabIndex = 0;
            // 
            // PictureBoxCloseForm
            // 
            PictureBoxCloseForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PictureBoxCloseForm.BackColor = Color.FromArgb(245, 245, 245);
            PictureBoxCloseForm.DG_ImageColor = SystemColors.ControlText;
            PictureBoxCloseForm.DG_ImageSize = new Size(18, 18);
            PictureBoxCloseForm.DG_LightenPercent = 50;
            PictureBoxCloseForm.DG_SVGString = resources.GetString("PictureBoxCloseForm.DG_SVGString");
            PictureBoxCloseForm.ForeColor = Color.Black;
            PictureBoxCloseForm.Image = (Image)resources.GetObject("PictureBoxCloseForm.Image");
            PictureBoxCloseForm.Location = new Point(815, 10);
            PictureBoxCloseForm.Name = "PictureBoxCloseForm";
            PictureBoxCloseForm.Padding = new Padding(2);
            PictureBoxCloseForm.Size = new Size(18, 18);
            PictureBoxCloseForm.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBoxCloseForm.TabIndex = 8;
            PictureBoxCloseForm.TabStop = false;
            PictureBoxCloseForm.Click += PictureBoxCloseForm_Click;
            // 
            // labelTotalPhone
            // 
            labelTotalPhone.AutoSize = true;
            labelTotalPhone.BackColor = Color.Transparent;
            labelTotalPhone.DG_IsColorMode = true;
            labelTotalPhone.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTotalPhone.ForeColor = Color.FromArgb(207, 66, 51);
            labelTotalPhone.Location = new Point(144, 10);
            labelTotalPhone.Name = "labelTotalPhone";
            labelTotalPhone.Size = new Size(27, 20);
            labelTotalPhone.TabIndex = 1;
            labelTotalPhone.Text = "19";
            // 
            // labelNormaln1
            // 
            labelNormaln1.AutoSize = true;
            labelNormaln1.BackColor = Color.Transparent;
            labelNormaln1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNormaln1.ForeColor = Color.Black;
            labelNormaln1.Location = new Point(22, 10);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(204, 20);
            labelNormaln1.TabIndex = 0;
            labelNormaln1.Text = "Installing APK to 19 devices";
            // 
            // FormInstallAPK
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(168, 168, 168);
            ClientSize = new Size(855, 509);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormInstallAPK";
            Opacity = 0.97D;
            Padding = new Padding(1);
            Text = "FormInstallAPK";
            panelMain.ResumeLayout(false);
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
        private LabelNormalN labelNormaln1;
        private LabelNormalN labelTotalPhone;
        private PictureBoxBrightN PictureBoxCloseForm;
        private LabelNormalN labelFileApk;
        private AntdUI.In.FlowLayoutPanel flowLayoutPanel1;
        private PanelRoundN panelRoundn1;
        private ButtomRound buttonStartInstall;
    }
}