namespace Dragon.DesignView.FormUI
{
    partial class FormTransferComputer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTransferComputer));
            panelTranfer = new Panel();
            panelGetTransfer = new Panel();
            iconExchange = new AntdUI.Label();
            buttomTransferNow = new AntdUI.Button();
            label1 = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            label2 = new Label();
            flowCompList = new AntdUI.FlowPanel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panelTranfer
            // 
            panelTranfer.Location = new Point(314, 186);
            panelTranfer.Name = "panelTranfer";
            panelTranfer.Size = new Size(273, 108);
            panelTranfer.TabIndex = 1;
            // 
            // panelGetTransfer
            // 
            panelGetTransfer.Location = new Point(641, 186);
            panelGetTransfer.Name = "panelGetTransfer";
            panelGetTransfer.Size = new Size(273, 108);
            panelGetTransfer.TabIndex = 2;
            // 
            // iconExchange
            // 
            iconExchange.BadgeAlign = AntdUI.TAlign.Left;
            iconExchange.BadgeBack = Color.FromArgb(97, 53, 49);
            iconExchange.BadgeSize = 2F;
            iconExchange.BadgeSvg = resources.GetString("iconExchange.BadgeSvg");
            iconExchange.Location = new Point(597, 228);
            iconExchange.Name = "iconExchange";
            iconExchange.Size = new Size(33, 30);
            iconExchange.TabIndex = 5;
            iconExchange.Text = "";
            // 
            // buttomTransferNow
            // 
            buttomTransferNow.BadgeSvg = "";
            buttomTransferNow.DefaultBack = Color.FromArgb(70, 70, 70);
            buttomTransferNow.ForeColor = Color.White;
            buttomTransferNow.IconRatio = 0.1F;
            buttomTransferNow.IconSvg = "";
            buttomTransferNow.Location = new Point(535, 379);
            buttomTransferNow.Name = "buttomTransferNow";
            buttomTransferNow.Size = new Size(161, 40);
            buttomTransferNow.TabIndex = 6;
            buttomTransferNow.Text = "Transfer Now";
            buttomTransferNow.Click += buttomTransferNow_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(97, 53, 49);
            label1.Location = new Point(404, 84);
            label1.Name = "label1";
            label1.Size = new Size(421, 30);
            label1.TabIndex = 8;
            label1.Text = "Transfer other computer to this computer";
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(flowCompList);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(286, 554);
            panel1.TabIndex = 9;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(label2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(284, 51);
            panel2.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(97, 53, 49);
            label2.Location = new Point(38, 15);
            label2.Name = "label2";
            label2.Size = new Size(211, 21);
            label2.TabIndex = 10;
            label2.Text = "Choose Computer Transfer";
            // 
            // flowCompList
            // 
            flowCompList.Dock = DockStyle.Fill;
            flowCompList.Location = new Point(0, 51);
            flowCompList.Name = "flowCompList";
            flowCompList.Size = new Size(284, 501);
            flowCompList.TabIndex = 1;
            flowCompList.Text = "flowPanel1";
            // 
            // FormTransferComputer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            ClientSize = new Size(942, 554);
            Controls.Add(panel1);
            Controls.Add(label1);
            Controls.Add(buttomTransferNow);
            Controls.Add(iconExchange);
            Controls.Add(panelGetTransfer);
            Controls.Add(panelTranfer);
            Name = "FormTransferComputer";
            Text = "Transfer Comp";
            Load += FormTransferComputer_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panelTranfer;
        private Panel panelGetTransfer;
        private AntdUI.Label iconExchange;
        private AntdUI.Button buttomTransferNow;
        private Label label1;
        private Panel panel1;
        private Panel panel2;
        private Label label2;
        private AntdUI.FlowPanel flowCompList;
    }
}