
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;

namespace Dragon.DesignView.ControlUI.Private
{
    partial class UserThisPC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserThisPC));
            panelRoundedWithBorder1 = new PanelRoundedWithBorder();
            tableLayoutPanel1 = new TableLayoutPanel();
            LabelExpire = new LabelNormalN();
            labelStatus = new LabelNormalN();
            labelPCName = new LabelNormalN();
            panelNormaln1 = new PanelNormalN();
            pictureBoxsvg1 = new PictureBoxSVG();
            panelRoundedWithBorder1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panelNormaln1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxsvg1).BeginInit();
            SuspendLayout();
            // 
            // panelRoundedWithBorder1
            // 
            panelRoundedWithBorder1.BackColor = Color.Transparent;
            panelRoundedWithBorder1.Controls.Add(tableLayoutPanel1);
            panelRoundedWithBorder1.Controls.Add(panelNormaln1);
            panelRoundedWithBorder1.DG_BorderColor = Color.Gray;
            panelRoundedWithBorder1.DG_BorderDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            panelRoundedWithBorder1.DG_BorderRadius = 20;
            panelRoundedWithBorder1.DG_BorderThickness = 2;
            panelRoundedWithBorder1.Location = new Point(45, 7);
            panelRoundedWithBorder1.Name = "panelRoundedWithBorder1";
            panelRoundedWithBorder1.Padding = new Padding(11);
            panelRoundedWithBorder1.Size = new Size(257, 96);
            panelRoundedWithBorder1.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(LabelExpire, 0, 2);
            tableLayoutPanel1.Controls.Add(labelStatus, 0, 1);
            tableLayoutPanel1.Controls.Add(labelPCName, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(80, 11);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel1.Size = new Size(166, 74);
            tableLayoutPanel1.TabIndex = 6;
            // 
            // LabelExpire
            // 
            LabelExpire.AutoSize = true;
            LabelExpire.BackColor = Color.Transparent;
            LabelExpire.DG_BackColor = Color.Transparent;
            LabelExpire.DG_IsBrightBack = false;
            LabelExpire.DG_IsColorMode = false;
            LabelExpire.DG_IsGrayImage = false;
            LabelExpire.DG_LightenPercent = 80;
            LabelExpire.DG_SVGImageColor = Color.Transparent;
            LabelExpire.DG_SVGString = "";
            LabelExpire.ForeColor = Color.WhiteSmoke;
            LabelExpire.Location = new Point(6, 47);
            LabelExpire.Margin = new Padding(6, 3, 3, 0);
            LabelExpire.Name = "LabelExpire";
            LabelExpire.Size = new Size(103, 15);
            LabelExpire.TabIndex = 2;
            LabelExpire.Text = "Expire: 28/02/2026";
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.BackColor = Color.Transparent;
            labelStatus.DG_BackColor = Color.Transparent;
            labelStatus.DG_IsBrightBack = false;
            labelStatus.DG_IsColorMode = false;
            labelStatus.DG_IsGrayImage = false;
            labelStatus.DG_LightenPercent = 80;
            labelStatus.DG_SVGImageColor = Color.Transparent;
            labelStatus.DG_SVGString = "";
            labelStatus.ForeColor = Color.WhiteSmoke;
            labelStatus.Location = new Point(6, 25);
            labelStatus.Margin = new Padding(6, 3, 3, 0);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(38, 15);
            labelStatus.TabIndex = 1;
            labelStatus.Text = "active";
            // 
            // labelPCName
            // 
            labelPCName.AutoSize = true;
            labelPCName.BackColor = Color.Transparent;
            labelPCName.DG_BackColor = Color.Transparent;
            labelPCName.DG_IsBrightBack = false;
            labelPCName.DG_IsColorMode = false;
            labelPCName.DG_IsGrayImage = false;
            labelPCName.DG_LightenPercent = 80;
            labelPCName.DG_SVGImageColor = Color.Transparent;
            labelPCName.DG_SVGString = "";
            labelPCName.ForeColor = Color.WhiteSmoke;
            labelPCName.Location = new Point(6, 3);
            labelPCName.Margin = new Padding(6, 3, 3, 0);
            labelPCName.Name = "labelPCName";
            labelPCName.Size = new Size(77, 15);
            labelPCName.TabIndex = 0;
            labelPCName.Text = "Laptop-Work";
            // 
            // panelNormaln1
            // 
            panelNormaln1.BackColor = Color.FromArgb(245, 245, 245);
            panelNormaln1.Controls.Add(pictureBoxsvg1);
            panelNormaln1.Dock = DockStyle.Left;
            panelNormaln1.ForeColor = Color.Black;
            panelNormaln1.Location = new Point(11, 11);
            panelNormaln1.Name = "panelNormaln1";
            panelNormaln1.Padding = new Padding(3);
            panelNormaln1.Size = new Size(69, 74);
            panelNormaln1.TabIndex = 4;
            // 
            // pictureBoxsvg1
            // 
            pictureBoxsvg1.DG_SVGColor = Color.DarkGray;
            pictureBoxsvg1.DG_SVGString = resources.GetString("pictureBoxsvg1.DG_SVGString");
            pictureBoxsvg1.DG_Zoom = 0.7F;
            pictureBoxsvg1.Dock = DockStyle.Fill;
            pictureBoxsvg1.Image = (Image)resources.GetObject("pictureBoxsvg1.Image");
            pictureBoxsvg1.Location = new Point(3, 3);
            pictureBoxsvg1.Name = "pictureBoxsvg1";
            pictureBoxsvg1.Size = new Size(63, 68);
            pictureBoxsvg1.TabIndex = 0;
            pictureBoxsvg1.TabStop = false;
            // 
            // UserThisPC
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(panelRoundedWithBorder1);
            Name = "UserThisPC";
            Size = new Size(346, 108);
            panelRoundedWithBorder1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panelNormaln1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxsvg1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PanelRoundedWithBorder panelRoundedWithBorder1;
        private TableLayoutPanel tableLayoutPanel1;
        private LabelNormalN LabelExpire;
        private LabelNormalN labelStatus;
        private LabelNormalN labelPCName;
        private PanelNormalN panelNormaln1;
        private PictureBoxSVG pictureBoxsvg1;
    }
}