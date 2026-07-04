
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;

namespace Dragon.DesignView.ControlUI.Private
{
    partial class UserPCRecognized
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserPCRecognized));
            panelRoundedWithBorder1 = new PanelRoundedWithBorder();
            tableLayoutPanel1 = new TableLayoutPanel();
            labelNormaln1 = new LabelNormalN();
            label1 = new Label();
            pictureBoxsvg1 = new PictureBoxSVG();
            panelRoundedWithBorder1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxsvg1).BeginInit();
            SuspendLayout();
            // 
            // panelRoundedWithBorder1
            // 
            panelRoundedWithBorder1.Controls.Add(tableLayoutPanel1);
            panelRoundedWithBorder1.Controls.Add(pictureBoxsvg1);
            panelRoundedWithBorder1.DG_BorderColor = Color.FromArgb(255, 128, 0);
            panelRoundedWithBorder1.DG_BorderDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            panelRoundedWithBorder1.DG_BorderRadius = 20;
            panelRoundedWithBorder1.DG_BorderThickness = 3;
            panelRoundedWithBorder1.Location = new Point(35, 19);
            panelRoundedWithBorder1.Name = "panelRoundedWithBorder1";
            panelRoundedWithBorder1.Padding = new Padding(15, 11, 15, 11);
            panelRoundedWithBorder1.Size = new Size(277, 70);
            panelRoundedWithBorder1.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(labelNormaln1, 0, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(15, 11);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(199, 48);
            tableLayoutPanel1.TabIndex = 1;
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
            labelNormaln1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNormaln1.ForeColor = Color.WhiteSmoke;
            labelNormaln1.Location = new Point(5, 5);
            labelNormaln1.Margin = new Padding(5, 5, 3, 0);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(170, 17);
            labelNormaln1.TabIndex = 0;
            labelNormaln1.Text = "Computer Not Recognized";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.Gray;
            label1.Location = new Point(5, 29);
            label1.Margin = new Padding(5, 5, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(135, 15);
            label1.TabIndex = 1;
            label1.Text = "this PC is not authorized";
            // 
            // pictureBoxsvg1
            // 
            pictureBoxsvg1.DG_SVGColor = SystemColors.ActiveBorder;
            pictureBoxsvg1.DG_SVGString = resources.GetString("pictureBoxsvg1.DG_SVGString");
            pictureBoxsvg1.DG_Zoom = 0.4F;
            pictureBoxsvg1.Dock = DockStyle.Right;
            pictureBoxsvg1.Image = (Image)resources.GetObject("pictureBoxsvg1.Image");
            pictureBoxsvg1.Location = new Point(214, 11);
            pictureBoxsvg1.Name = "pictureBoxsvg1";
            pictureBoxsvg1.Size = new Size(48, 48);
            pictureBoxsvg1.TabIndex = 0;
            pictureBoxsvg1.TabStop = false;
            // 
            // UserPCRecognized
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(panelRoundedWithBorder1);
            Name = "UserPCRecognized";
            Size = new Size(346, 108);
            Load += UserPCRecognized_Load;
            panelRoundedWithBorder1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxsvg1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PanelRoundedWithBorder panelRoundedWithBorder1;
        private PictureBoxSVG pictureBoxsvg1;
        private TableLayoutPanel tableLayoutPanel1;
        private LabelNormalN labelNormaln1;
        private Label label1;
    }
}