using Dragon.DesignView.Public.ColorMode;

namespace Dragon.DesignView.FormUI
{
    partial class MouseSelect
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
            panelLineBorder1 = new PanelLineBorderNC();
            SuspendLayout();
            // 
            // panelLineBorder1
            // 
            panelLineBorder1.DG_BackGroundColor1 = Color.FromArgb(0, 173, 232);
            panelLineBorder1.DG_BackGroundColor2 = Color.FromArgb(207, 66, 51);
            panelLineBorder1.DG_BackGroundGradientBool = true;
            panelLineBorder1.DG_BackGroundLinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            panelLineBorder1.DG_BorderAlignment = System.Drawing.Drawing2D.PenAlignment.Inset;
            panelLineBorder1.DG_BorderThickness = 2;
            panelLineBorder1.DG_DrawBottom = false;
            panelLineBorder1.DG_DrawBottomColor = Color.FromArgb(61, 61, 61);
            panelLineBorder1.DG_DrawLeft = false;
            panelLineBorder1.DG_DrawLeftColor = Color.FromArgb(61, 61, 61);
            panelLineBorder1.DG_DrawRight = false;
            panelLineBorder1.DG_DrawRightColor = Color.FromArgb(61, 61, 61);
            panelLineBorder1.DG_DrawTop = false;
            panelLineBorder1.DG_DrawTopColor = Color.FromArgb(61, 61, 61);
            panelLineBorder1.Location = new Point(12, 12);
            panelLineBorder1.Name = "panelLineBorder1";
            panelLineBorder1.Size = new Size(107, 46);
            panelLineBorder1.TabIndex = 0;
            // 
            // MouseSelect
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(259, 135);
            Controls.Add(panelLineBorder1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MouseSelect";
            Text = "MouseSelect";
            Load += MouseSelect_Load;
            ResumeLayout(false);
        }

        #endregion

        private PanelLineBorderNC panelLineBorder1;
    }
}