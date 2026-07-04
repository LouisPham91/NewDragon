
using Dragon.DesignView.Public.ColorMode;
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;

namespace Dragon.DesignView.ControlUI.Private
{
    partial class UserSettingColor
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
            label1 = new Label();
            panelRed = new PanelColorC();
            panelGreen = new PanelColorC();
            panelGold = new PanelColorC();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(42, 20);
            label1.Name = "label1";
            label1.Size = new Size(156, 15);
            label1.TabIndex = 1;
            label1.Text = "Choose Theme Color Stype :";
            // 
            // panelRed
            // 
            panelRed.BackColor = Color.FromArgb(255, 71, 51);
            panelRed.DG_BottomLineColor = Color.FromArgb(255, 70, 70);
            panelRed.Location = new Point(48, 44);
            panelRed.Margin = new Padding(3, 2, 3, 2);
            panelRed.Name = "panelRed";
            panelRed.Size = new Size(26, 22);
            panelRed.TabIndex = 2;
            // 
            // panelGreen
            // 
            panelGreen.BackColor = Color.FromArgb(90, 151, 90);
            panelGreen.DG_BottomLineColor = Color.FromArgb(255, 70, 70);
            panelGreen.Location = new Point(92, 44);
            panelGreen.Margin = new Padding(3, 2, 3, 2);
            panelGreen.Name = "panelGreen";
            panelGreen.Size = new Size(26, 22);
            panelGreen.TabIndex = 3;
            // 
            // panelGold
            // 
            panelGold.BackColor = Color.DarkGoldenrod;
            panelGold.DG_BottomLineColor = Color.FromArgb(255, 70, 70);
            panelGold.Location = new Point(136, 44);
            panelGold.Margin = new Padding(3, 2, 3, 2);
            panelGold.Name = "panelGold";
            panelGold.Size = new Size(26, 22);
            panelGold.TabIndex = 4;
            // 
            // UserSettingColor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            Controls.Add(panelGold);
            Controls.Add(panelGreen);
            Controls.Add(panelRed);
            Controls.Add(label1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "UserSettingColor";
            Size = new Size(706, 716);
            Load += UserSettingColor_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private PanelColorC panelRed;
        private PanelColorC panelGreen;
        private PanelColorC panelGold;
    }
}