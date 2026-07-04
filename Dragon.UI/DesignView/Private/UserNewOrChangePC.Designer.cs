
using Dragon.DesignView.Public.NormalMode;
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;

namespace Dragon.DesignView.ControlUI.Private
{
    partial class UserNewOrChangePC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserNewOrChangePC));
            panel1 = new Panel();
            panelCreate = new Panel();
            iconComputer = new AntdUI.Label();
            iconPlus = new AntdUI.Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            buttonCreateNew = new AntdUI.Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            labelNormaln1 = new Dragon.DesignView.Public.NormalMode.LabelNormalN();
            panel2 = new Panel();
            panelTransfer = new Panel();
            iconExchange = new AntdUI.Label();
            iconUser = new AntdUI.Label();
            tableLayoutPanel4 = new TableLayoutPanel();
            buttonTransfer = new AntdUI.Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            labelNormaln2 = new Dragon.DesignView.Public.NormalMode.LabelNormalN();
            panel1.SuspendLayout();
            panelCreate.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            panelTransfer.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panelCreate);
            panel1.Controls.Add(tableLayoutPanel3);
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(173, 108);
            panel1.TabIndex = 0;
            // 
            // panelCreate
            // 
            panelCreate.Controls.Add(iconComputer);
            panelCreate.Controls.Add(iconPlus);
            panelCreate.Location = new Point(40, 32);
            panelCreate.Name = "panelCreate";
            panelCreate.Size = new Size(87, 36);
            panelCreate.TabIndex = 9;
            // 
            // iconComputer
            // 
            iconComputer.BadgeAlign = AntdUI.TAlign.Left;
            iconComputer.BadgeBack = Color.FromArgb(97, 53, 49);
            iconComputer.BadgeSize = 2F;
            iconComputer.BadgeSvg = resources.GetString("iconComputer.BadgeSvg");
            iconComputer.Location = new Point(51, 2);
            iconComputer.Name = "iconComputer";
            iconComputer.Size = new Size(33, 30);
            iconComputer.TabIndex = 3;
            iconComputer.Text = "";
            // 
            // iconPlus
            // 
            iconPlus.Badge = "";
            iconPlus.BadgeAlign = AntdUI.TAlign.Left;
            iconPlus.BadgeBack = Color.FromArgb(97, 53, 49);
            iconPlus.BadgeSize = 2F;
            iconPlus.BadgeSvg = resources.GetString("iconPlus.BadgeSvg");
            iconPlus.ColorExtend = "";
            iconPlus.Location = new Point(3, 2);
            iconPlus.Name = "iconPlus";
            iconPlus.Prefix = "";
            iconPlus.PrefixSvg = "";
            iconPlus.Size = new Size(33, 30);
            iconPlus.TabIndex = 2;
            iconPlus.Text = "";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(buttonCreateNew, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Bottom;
            tableLayoutPanel3.Location = new Point(0, 71);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.Padding = new Padding(18, 0, 18, 0);
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(171, 35);
            tableLayoutPanel3.TabIndex = 4;
            // 
            // buttonCreateNew
            // 
            buttonCreateNew.AutoEllipsis = true;
            buttonCreateNew.DefaultBack = Color.FromArgb(70, 70, 70);
            buttonCreateNew.Dock = DockStyle.Fill;
            buttonCreateNew.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonCreateNew.ForeColor = Color.WhiteSmoke;
            buttonCreateNew.Location = new Point(21, 3);
            buttonCreateNew.Name = "buttonCreateNew";
            buttonCreateNew.Size = new Size(129, 29);
            buttonCreateNew.TabIndex = 0;
            buttonCreateNew.Text = "Create";
            buttonCreateNew.Click += buttonCreateNew_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(labelNormaln1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(171, 24);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // labelNormaln1
            // 
            labelNormaln1.AutoSize = true;
            labelNormaln1.BackColor = Color.Transparent;
            labelNormaln1.DG_BackColor = Color.Transparent;
            labelNormaln1.DG_LightenPercent = 80;
            labelNormaln1.DG_SVGImageColor = Color.Transparent;
            labelNormaln1.DG_SVGString = "";
            labelNormaln1.Dock = DockStyle.Fill;
            labelNormaln1.Font = new Font("Segoe UI", 9F);
            labelNormaln1.ForeColor = Color.White;
            // 🔥 SỬA: Đổi tên property
            labelNormaln1.DG_IsBrightBack = false;
            labelNormaln1.DG_IsColorMode = false;
            labelNormaln1.DG_IsGrayImage = false;
            labelNormaln1.Location = new Point(3, 0);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(165, 24);
            labelNormaln1.TabIndex = 0;
            labelNormaln1.Text = "Create New Computer";
            labelNormaln1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(panelTransfer);
            panel2.Controls.Add(tableLayoutPanel4);
            panel2.Controls.Add(tableLayoutPanel2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(173, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(173, 108);
            panel2.TabIndex = 1;
            // 
            // panelTransfer
            // 
            panelTransfer.Controls.Add(iconExchange);
            panelTransfer.Controls.Add(iconUser);
            panelTransfer.Location = new Point(40, 33);
            panelTransfer.Name = "panelTransfer";
            panelTransfer.Size = new Size(87, 36);
            panelTransfer.TabIndex = 8;
            // 
            // iconExchange
            // 
            iconExchange.BadgeAlign = AntdUI.TAlign.Left;
            iconExchange.BadgeBack = Color.FromArgb(97, 53, 49);
            iconExchange.BadgeSize = 2F;
            iconExchange.BadgeSvg = resources.GetString("iconExchange.BadgeSvg");
            iconExchange.Location = new Point(4, 3);
            iconExchange.Name = "iconExchange";
            iconExchange.Size = new Size(33, 30);
            iconExchange.TabIndex = 4;
            iconExchange.Text = "";
            // 
            // iconUser
            // 
            iconUser.BadgeAlign = AntdUI.TAlign.Left;
            iconUser.BadgeBack = Color.FromArgb(97, 53, 49);
            iconUser.BadgeSize = 2F;
            iconUser.BadgeSvg = resources.GetString("iconUser.BadgeSvg");
            iconUser.Location = new Point(50, 2);
            iconUser.Name = "iconUser";
            iconUser.Size = new Size(33, 30);
            iconUser.TabIndex = 5;
            iconUser.Text = "";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(buttonTransfer, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Bottom;
            tableLayoutPanel4.Location = new Point(0, 71);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.Padding = new Padding(18, 0, 18, 0);
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(171, 35);
            tableLayoutPanel4.TabIndex = 7;
            // 
            // buttonTransfer
            // 
            buttonTransfer.AutoEllipsis = true;
            buttonTransfer.DefaultBack = Color.FromArgb(70, 70, 70);
            buttonTransfer.Dock = DockStyle.Fill;
            buttonTransfer.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonTransfer.ForeColor = Color.WhiteSmoke;
            buttonTransfer.Location = new Point(21, 3);
            buttonTransfer.Name = "buttonTransfer";
            buttonTransfer.Size = new Size(129, 29);
            buttonTransfer.TabIndex = 1;
            buttonTransfer.Text = "Transfer";
            buttonTransfer.Click += buttonTransfer_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(labelNormaln2, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Top;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(171, 24);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // labelNormaln2
            // 
            labelNormaln2.AutoSize = true;
            labelNormaln2.BackColor = Color.Transparent;
            labelNormaln2.DG_BackColor = Color.Transparent;
            labelNormaln2.DG_LightenPercent = 80;
            labelNormaln2.DG_SVGImageColor = Color.Transparent;
            labelNormaln2.DG_SVGString = "";
            labelNormaln2.Dock = DockStyle.Fill;
            labelNormaln2.Font = new Font("Segoe UI", 9F);
            labelNormaln2.ForeColor = Color.White;
            // 🔥 SỬA: Đổi tên property
            labelNormaln2.DG_IsBrightBack = false;
            labelNormaln2.DG_IsColorMode = false;
            labelNormaln2.DG_IsGrayImage = false;
            labelNormaln2.Location = new Point(3, 0);
            labelNormaln2.Name = "labelNormaln2";
            labelNormaln2.Size = new Size(165, 24);
            labelNormaln2.TabIndex = 1;
            labelNormaln2.Text = "Transfer Computer";
            labelNormaln2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // UserNewOrChangePC
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "UserNewOrChangePC";
            Size = new Size(346, 108);
            panel1.ResumeLayout(false);
            panelCreate.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel2.ResumeLayout(false);
            panelTransfer.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private LabelNormalN labelNormaln1;
        private LabelNormalN labelNormaln2;
        private AntdUI.Label iconPlus;
        private AntdUI.Label iconComputer;
        private AntdUI.Label iconUser;
        private AntdUI.Label iconExchange;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private AntdUI.Button buttonCreateNew;
        private AntdUI.Button buttonTransfer;
        private Panel panelTransfer;
        private Panel panelCreate;
    }
}