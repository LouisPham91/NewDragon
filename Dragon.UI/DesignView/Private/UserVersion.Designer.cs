
using Dragon.DesignView.Public.NormalMode;
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;

namespace Dragon.DesignView.ControlUI.Private
{
    partial class UserVersion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserVersion));
            panelNormaln1 = new PanelNormalN();
            panel1 = new Panel();
            panelNormaln3 = new PanelNormalN();
            picboxLogo = new PictureBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            labelVersion = new Label();
            labelDragon = new Label();
            labelStatus = new Label();
            buttonLoginGmail = new AntdUI.Button();
            panelNormaln2 = new PanelNormalN();
            panelThisPC = new PanelNormalN();
            tableLayoutPanelBottomBordern2 = new TableLayoutPanelBottomBorderN();
            labelNormaln2 = new LabelNormalN();
            tableLayoutPanelBottomBordern1 = new TableLayoutPanelBottomBorderN();
            labelNormaln1 = new LabelNormalN();
            flowOtherComputer = new AntdUI.FlowPanel();
            panelNormaln1.SuspendLayout();
            panel1.SuspendLayout();
            panelNormaln3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picboxLogo).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            panelNormaln2.SuspendLayout();
            tableLayoutPanelBottomBordern2.SuspendLayout();
            tableLayoutPanelBottomBordern1.SuspendLayout();
            SuspendLayout();
            // 
            // panelNormaln1
            // 
            panelNormaln1.BackColor = Color.FromArgb(40, 40, 40);
            panelNormaln1.Controls.Add(panel1);
            panelNormaln1.Controls.Add(labelStatus);
            panelNormaln1.Controls.Add(buttonLoginGmail);
            panelNormaln1.Dock = DockStyle.Top;
            panelNormaln1.ForeColor = Color.Black;
            panelNormaln1.Location = new Point(0, 0);
            panelNormaln1.Name = "panelNormaln1";
            panelNormaln1.Size = new Size(346, 216);
            panelNormaln1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(panelNormaln3);
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Location = new Point(89, 24);
            panel1.Name = "panel1";
            panel1.Size = new Size(152, 67);
            panel1.TabIndex = 3;
            // 
            // panelNormaln3
            // 
            panelNormaln3.BackColor = Color.FromArgb(40, 40, 40);
            panelNormaln3.Controls.Add(picboxLogo);
            panelNormaln3.Dock = DockStyle.Fill;
            panelNormaln3.ForeColor = Color.Black;
            panelNormaln3.Location = new Point(0, 0);
            panelNormaln3.Name = "panelNormaln3";
            panelNormaln3.Size = new Size(68, 67);
            panelNormaln3.TabIndex = 4;
            // 
            // picboxLogo
            // 
            picboxLogo.Location = new Point(10, 10);
            picboxLogo.Name = "picboxLogo";
            picboxLogo.Size = new Size(46, 46);
            picboxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picboxLogo.TabIndex = 3;
            picboxLogo.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(labelVersion, 0, 1);
            tableLayoutPanel1.Controls.Add(labelDragon, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Right;
            tableLayoutPanel1.Location = new Point(68, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 43.28358F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 56.71642F));
            tableLayoutPanel1.Size = new Size(84, 67);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // labelVersion
            // 
            labelVersion.AutoSize = true;
            labelVersion.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelVersion.ForeColor = Color.FromArgb(255, 128, 128);
            labelVersion.Location = new Point(0, 34);
            labelVersion.Margin = new Padding(0, 5, 3, 0);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(56, 21);
            labelVersion.TabIndex = 2;
            labelVersion.Text = "V1.1.2";
            labelVersion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelDragon
            // 
            labelDragon.AutoSize = true;
            labelDragon.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDragon.ForeColor = Color.FromArgb(255, 128, 128);
            labelDragon.Location = new Point(0, 8);
            labelDragon.Margin = new Padding(0, 8, 3, 0);
            labelDragon.Name = "labelDragon";
            labelDragon.Size = new Size(67, 21);
            labelDragon.TabIndex = 1;
            labelDragon.Text = "Dragon";
            labelDragon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.ForeColor = Color.FromArgb(128, 255, 128);
            labelStatus.Location = new Point(84, 169);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(175, 15);
            labelStatus.TabIndex = 2;
            labelStatus.Text = "Login Succsess! Hello Mr. Phạm";
            // 
            // buttonLoginGmail
            // 
            buttonLoginGmail.BadgeSvg = "";
            buttonLoginGmail.DefaultBack = Color.FromArgb(70, 70, 70);
            buttonLoginGmail.ForeColor = Color.White;
            buttonLoginGmail.IconRatio = 1.3F;
            buttonLoginGmail.IconSvg = resources.GetString("buttonLoginGmail.IconSvg");
            buttonLoginGmail.Location = new Point(89, 112);
            buttonLoginGmail.Name = "buttonLoginGmail";
            buttonLoginGmail.Size = new Size(161, 40);
            buttonLoginGmail.TabIndex = 1;
            buttonLoginGmail.Text = "Login with Google";
            buttonLoginGmail.Click += buttonLoginGmail_Click;
            // 
            // panelNormaln2
            // 
            panelNormaln2.BackColor = Color.FromArgb(245, 245, 245);
            panelNormaln2.Controls.Add(panelThisPC);
            panelNormaln2.Controls.Add(tableLayoutPanelBottomBordern2);
            panelNormaln2.Controls.Add(tableLayoutPanelBottomBordern1);
            panelNormaln2.Dock = DockStyle.Top;
            panelNormaln2.ForeColor = Color.Black;
            panelNormaln2.Location = new Point(0, 216);
            panelNormaln2.Name = "panelNormaln2";
            panelNormaln2.Size = new Size(346, 190);
            panelNormaln2.TabIndex = 1;
            // 
            // panelThisPC
            // 
            panelThisPC.BackColor = Color.FromArgb(40, 40, 40);
            panelThisPC.Dock = DockStyle.Fill;
            panelThisPC.ForeColor = Color.Black;
            panelThisPC.Location = new Point(0, 40);
            panelThisPC.Name = "panelThisPC";
            panelThisPC.Size = new Size(346, 110);
            panelThisPC.TabIndex = 3;
            // 
            // tableLayoutPanelBottomBordern2
            // 
            tableLayoutPanelBottomBordern2.BackColor = Color.FromArgb(40, 40, 40);
            tableLayoutPanelBottomBordern2.ColumnCount = 1;
            tableLayoutPanelBottomBordern2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanelBottomBordern2.Controls.Add(labelNormaln2, 0, 0);
            tableLayoutPanelBottomBordern2.DG_BorderColor = Color.FromArgb(168, 168, 168);
            tableLayoutPanelBottomBordern2.DG_BorderThickness = 2;
            tableLayoutPanelBottomBordern2.Dock = DockStyle.Bottom;
            tableLayoutPanelBottomBordern2.Location = new Point(0, 150);
            tableLayoutPanelBottomBordern2.Margin = new Padding(0);
            tableLayoutPanelBottomBordern2.Name = "tableLayoutPanelBottomBordern2";
            tableLayoutPanelBottomBordern2.RowCount = 1;
            tableLayoutPanelBottomBordern2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanelBottomBordern2.Size = new Size(346, 40);
            tableLayoutPanelBottomBordern2.TabIndex = 1;
            // 
            // labelNormaln2
            // 
            labelNormaln2.AutoSize = true;
            labelNormaln2.BackColor = Color.Transparent;
            labelNormaln2.DG_SVGImageColor = Color.White;
            labelNormaln2.DG_SVGString = resources.GetString("labelNormaln2.DG_SVGString");
            labelNormaln2.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNormaln2.ForeColor = Color.White;
            labelNormaln2.Image = (Image)resources.GetObject("labelNormaln2.Image");
            labelNormaln2.ImageAlign = ContentAlignment.MiddleLeft;
            labelNormaln2.Location = new Point(10, 12);
            labelNormaln2.Margin = new Padding(10, 12, 0, 0);
            labelNormaln2.Name = "labelNormaln2";
            labelNormaln2.Size = new Size(164, 17);
            labelNormaln2.TabIndex = 1;
            labelNormaln2.Text = "      Your Other Computer";
            labelNormaln2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelBottomBordern1
            // 
            tableLayoutPanelBottomBordern1.BackColor = Color.FromArgb(40, 40, 40);
            tableLayoutPanelBottomBordern1.ColumnCount = 1;
            tableLayoutPanelBottomBordern1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanelBottomBordern1.Controls.Add(labelNormaln1, 0, 0);
            tableLayoutPanelBottomBordern1.DG_BorderColor = Color.FromArgb(168, 168, 168);
            tableLayoutPanelBottomBordern1.DG_BorderThickness = 2;
            tableLayoutPanelBottomBordern1.Dock = DockStyle.Top;
            tableLayoutPanelBottomBordern1.Location = new Point(0, 0);
            tableLayoutPanelBottomBordern1.Margin = new Padding(0);
            tableLayoutPanelBottomBordern1.Name = "tableLayoutPanelBottomBordern1";
            tableLayoutPanelBottomBordern1.RowCount = 1;
            tableLayoutPanelBottomBordern1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanelBottomBordern1.Size = new Size(346, 40);
            tableLayoutPanelBottomBordern1.TabIndex = 0;
            // 
            // labelNormaln1
            // 
            labelNormaln1.AutoSize = true;
            labelNormaln1.BackColor = Color.Transparent;
            labelNormaln1.DG_SVGImageColor = Color.White;
            labelNormaln1.DG_SVGString = resources.GetString("labelNormaln1.DG_SVGString");
            labelNormaln1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNormaln1.ForeColor = Color.White;
            labelNormaln1.Image = (Image)resources.GetObject("labelNormaln1.Image");
            labelNormaln1.ImageAlign = ContentAlignment.MiddleLeft;
            labelNormaln1.Location = new Point(10, 12);
            labelNormaln1.Margin = new Padding(10, 12, 0, 0);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(78, 17);
            labelNormaln1.TabIndex = 0;
            labelNormaln1.Text = "      This PC";
            labelNormaln1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowOtherComputer
            // 
            flowOtherComputer.Dock = DockStyle.Fill;
            flowOtherComputer.Location = new Point(0, 406);
            flowOtherComputer.Name = "flowOtherComputer";
            flowOtherComputer.Size = new Size(346, 242);
            flowOtherComputer.TabIndex = 2;
            flowOtherComputer.Text = "flowPanel1";
            // 
            // UserVersion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            Controls.Add(flowOtherComputer);
            Controls.Add(panelNormaln2);
            Controls.Add(panelNormaln1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "UserVersion";
            Size = new Size(346, 648);
            Load += UserVersion_Load;
            panelNormaln1.ResumeLayout(false);
            panelNormaln1.PerformLayout();
            panel1.ResumeLayout(false);
            panelNormaln3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picboxLogo).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panelNormaln2.ResumeLayout(false);
            tableLayoutPanelBottomBordern2.ResumeLayout(false);
            tableLayoutPanelBottomBordern2.PerformLayout();
            tableLayoutPanelBottomBordern1.ResumeLayout(false);
            tableLayoutPanelBottomBordern1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PanelNormalN panelNormaln1;
        private AntdUI.Button buttonLoginGmail;
        private Label labelStatus;
        private PanelNormalN panelNormaln2;
        private AntdUI.FlowPanel flowOtherComputer;
        private LabelNormalN labelNormaln1;
        private LabelNormalN labelNormaln2;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private PanelNormalN panelNormaln3;
        private PictureBox picboxLogo;
        private Label labelVersion;
        private Label labelDragon;
        // 🔥 SỬA: Tên class đúng
        private TableLayoutPanelBottomBorderN tableLayoutPanelBottomBordern2;
        private TableLayoutPanelBottomBorderN tableLayoutPanelBottomBordern1;
        private PanelNormalN panelThisPC;
    }
}