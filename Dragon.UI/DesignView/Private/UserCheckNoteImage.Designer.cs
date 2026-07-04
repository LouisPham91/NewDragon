
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.ControlUI.Private
{
    partial class UserCheckNoteImage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserCheckNoteImage));
            tablePanelCheck = new TableLayoutPanel();
            lblDetectOnly = new Label();
            lblDetectAndClick = new Label();
            chkActiveCheck = new CheckBox();
            labelDeleteCheck = new LabelNormalN();
            tableLayoutPanel4 = new TableLayoutPanel();
            labelnumber4 = new LabelNormalN();
            numbericAccuracy = new NumericUpDownCustomN();
            pnlCrop = new TableLayoutPanel();
            lblCropRegion = new LabelNormalN();
            txtCropRegion = new TextBox();
            pnlText = new TableLayoutPanel();
            lblTextToFind = new LabelNormalN();
            txtTextToFind = new TextBox();
            pnlClick = new TableLayoutPanel();
            lblClickPoint = new LabelNormalN();
            txtPointCheck = new TextBox();
            picboxImageCheck = new PictureBox();
            lblDetectAtPoint = new Label();
            tablePanelCheck.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            pnlCrop.SuspendLayout();
            pnlText.SuspendLayout();
            pnlClick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picboxImageCheck).BeginInit();
            SuspendLayout();
            // 
            // tablePanelCheck
            // 
            tablePanelCheck.BackColor = Color.FromArgb(40, 40, 40);
            tablePanelCheck.ColumnCount = 2;
            tablePanelCheck.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36.6053162F));
            tablePanelCheck.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 63.3946838F));
            tablePanelCheck.Controls.Add(lblDetectOnly, 0, 1);
            tablePanelCheck.Controls.Add(lblDetectAndClick, 0, 2);
            tablePanelCheck.Controls.Add(chkActiveCheck, 0, 0);
            tablePanelCheck.Controls.Add(labelDeleteCheck, 1, 0);
            tablePanelCheck.Controls.Add(tableLayoutPanel4, 1, 1);
            tablePanelCheck.Controls.Add(pnlCrop, 1, 2);
            tablePanelCheck.Controls.Add(pnlText, 1, 3);
            tablePanelCheck.Controls.Add(pnlClick, 0, 4);
            tablePanelCheck.Controls.Add(picboxImageCheck, 0, 5);
            tablePanelCheck.Controls.Add(lblDetectAtPoint, 0, 3);
            tablePanelCheck.Dock = DockStyle.Fill;
            tablePanelCheck.Location = new Point(0, 0);
            tablePanelCheck.Name = "tablePanelCheck";
            tablePanelCheck.RowCount = 6;
            tablePanelCheck.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tablePanelCheck.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tablePanelCheck.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tablePanelCheck.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tablePanelCheck.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tablePanelCheck.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tablePanelCheck.Size = new Size(471, 240);
            tablePanelCheck.TabIndex = 1;
            // 
            // lblDetectOnly
            // 
            lblDetectOnly.AutoSize = true;
            lblDetectOnly.ForeColor = Color.White;
            lblDetectOnly.Location = new Point(6, 34);
            lblDetectOnly.Margin = new Padding(6, 6, 3, 0);
            lblDetectOnly.Name = "lblDetectOnly";
            lblDetectOnly.Size = new Size(69, 15);
            lblDetectOnly.TabIndex = 25;
            lblDetectOnly.Text = "Detect Only";
            // 
            // lblDetectAndClick
            // 
            lblDetectAndClick.AutoSize = true;
            lblDetectAndClick.ForeColor = Color.White;
            lblDetectAndClick.Location = new Point(6, 62);
            lblDetectAndClick.Margin = new Padding(6, 6, 3, 0);
            lblDetectAndClick.Name = "lblDetectAndClick";
            lblDetectAndClick.Size = new Size(83, 15);
            lblDetectAndClick.TabIndex = 24;
            lblDetectAndClick.Text = "Detect && Click";
            // 
            // chkActiveCheck
            // 
            chkActiveCheck.AutoSize = true;
            chkActiveCheck.ForeColor = Color.White;
            chkActiveCheck.Location = new Point(6, 4);
            chkActiveCheck.Margin = new Padding(6, 4, 3, 3);
            chkActiveCheck.Name = "chkActiveCheck";
            chkActiveCheck.Size = new Size(119, 19);
            chkActiveCheck.TabIndex = 0;
            chkActiveCheck.Text = "Active This Check";
            chkActiveCheck.UseVisualStyleBackColor = true;
            chkActiveCheck.CheckedChanged += chkActiveCheck_CheckedChanged;
            // 
            // labelDeleteCheck
            // 
            labelDeleteCheck.AutoSize = true;
            labelDeleteCheck.BackColor = Color.Transparent;
            labelDeleteCheck.DG_IsBrightBack = true;
            labelDeleteCheck.DG_SVGString = resources.GetString("labelDeleteCheck.DG_SVGString");
            labelDeleteCheck.ForeColor = Color.White;
            labelDeleteCheck.Image = (Image)resources.GetObject("labelDeleteCheck.Image");
            labelDeleteCheck.ImageAlign = ContentAlignment.MiddleLeft;
            labelDeleteCheck.Location = new Point(178, 6);
            labelDeleteCheck.Margin = new Padding(6, 6, 3, 0);
            labelDeleteCheck.Name = "labelDeleteCheck";
            labelDeleteCheck.Size = new Size(58, 15);
            labelDeleteCheck.TabIndex = 10;
            labelDeleteCheck.Text = "      Delete";
            labelDeleteCheck.Click += labelDeleteCheck_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.7234039F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71.2765961F));
            tableLayoutPanel4.Controls.Add(labelnumber4, 0, 0);
            tableLayoutPanel4.Controls.Add(numbericAccuracy, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(172, 28);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(299, 28);
            tableLayoutPanel4.TabIndex = 13;
            // 
            // labelnumber4
            // 
            labelnumber4.AutoSize = true;
            labelnumber4.BackColor = Color.Transparent;
            labelnumber4.ForeColor = Color.White;
            labelnumber4.Location = new Point(6, 6);
            labelnumber4.Margin = new Padding(6, 6, 3, 0);
            labelnumber4.Name = "labelnumber4";
            labelnumber4.Size = new Size(56, 15);
            labelnumber4.TabIndex = 11;
            labelnumber4.Text = "Accuracy";
            // 
            // numbericAccuracy
            // 
            numbericAccuracy.BackColor = Color.FromArgb(61, 61, 61);
            numbericAccuracy.DG_ArrowColor = Color.Black;
            numbericAccuracy.DG_ArrowPressedColor = Color.FromArgb(61, 61, 61);
            numbericAccuracy.DG_BorderColor = Color.FromArgb(168, 168, 168);
            numbericAccuracy.DG_BoxColor = Color.FromArgb(245, 245, 245);
            numbericAccuracy.DG_ButtonColor = Color.FromArgb(205, 205, 205);
            numbericAccuracy.DG_Minimum = 1;
            numbericAccuracy.DG_TextColor = Color.Black;
            numbericAccuracy.ForeColor = Color.White;
            numbericAccuracy.Location = new Point(88, 3);
            numbericAccuracy.Name = "numbericAccuracy";
            numbericAccuracy.Size = new Size(58, 22);
            numbericAccuracy.TabIndex = 10;
            numbericAccuracy.Value = 90;
            numbericAccuracy.TextChanged += numbericAccuracy_ValueChanged;
            // 
            // pnlCrop
            // 
            pnlCrop.ColumnCount = 2;
            pnlCrop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            pnlCrop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlCrop.Controls.Add(lblCropRegion, 0, 0);
            pnlCrop.Controls.Add(txtCropRegion, 1, 0);
            pnlCrop.Dock = DockStyle.Fill;
            pnlCrop.Location = new Point(172, 56);
            pnlCrop.Margin = new Padding(0);
            pnlCrop.Name = "pnlCrop";
            pnlCrop.RowCount = 1;
            pnlCrop.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlCrop.Size = new Size(299, 28);
            pnlCrop.TabIndex = 20;
            // 
            // lblCropRegion
            // 
            lblCropRegion.AutoSize = true;
            lblCropRegion.BackColor = Color.Transparent;
            lblCropRegion.ForeColor = Color.White;
            lblCropRegion.Location = new Point(3, 6);
            lblCropRegion.Margin = new Padding(3, 6, 3, 0);
            lblCropRegion.Name = "lblCropRegion";
            lblCropRegion.Size = new Size(70, 15);
            lblCropRegion.TabIndex = 0;
            lblCropRegion.Text = "CropRegion";
            // 
            // txtCropRegion
            // 
            txtCropRegion.BackColor = Color.FromArgb(40, 40, 40);
            txtCropRegion.BorderStyle = BorderStyle.FixedSingle;
            txtCropRegion.ForeColor = Color.White;
            txtCropRegion.Location = new Point(83, 3);
            txtCropRegion.Name = "txtCropRegion";
            txtCropRegion.Size = new Size(213, 23);
            txtCropRegion.TabIndex = 1;
            txtCropRegion.TextChanged += txtCropRegion_TextChanged;
            // 
            // pnlText
            // 
            pnlText.ColumnCount = 2;
            pnlText.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            pnlText.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlText.Controls.Add(lblTextToFind, 0, 0);
            pnlText.Controls.Add(txtTextToFind, 1, 0);
            pnlText.Dock = DockStyle.Fill;
            pnlText.Location = new Point(172, 84);
            pnlText.Margin = new Padding(0);
            pnlText.Name = "pnlText";
            pnlText.RowCount = 1;
            pnlText.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlText.Size = new Size(299, 28);
            pnlText.TabIndex = 21;
            // 
            // lblTextToFind
            // 
            lblTextToFind.AutoSize = true;
            lblTextToFind.BackColor = Color.Transparent;
            lblTextToFind.ForeColor = Color.White;
            lblTextToFind.Location = new Point(3, 6);
            lblTextToFind.Margin = new Padding(3, 6, 3, 0);
            lblTextToFind.Name = "lblTextToFind";
            lblTextToFind.Size = new Size(63, 15);
            lblTextToFind.TabIndex = 0;
            lblTextToFind.Text = "TextToFind";
            // 
            // txtTextToFind
            // 
            txtTextToFind.BackColor = Color.FromArgb(40, 40, 40);
            txtTextToFind.BorderStyle = BorderStyle.FixedSingle;
            txtTextToFind.ForeColor = Color.White;
            txtTextToFind.Location = new Point(83, 3);
            txtTextToFind.Name = "txtTextToFind";
            txtTextToFind.Size = new Size(213, 23);
            txtTextToFind.TabIndex = 1;
            txtTextToFind.TextChanged += txtTextToFind_TextChanged;
            // 
            // pnlClick
            // 
            pnlClick.ColumnCount = 2;
            tablePanelCheck.SetColumnSpan(pnlClick, 2);
            pnlClick.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            pnlClick.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlClick.Controls.Add(lblClickPoint, 0, 0);
            pnlClick.Controls.Add(txtPointCheck, 1, 0);
            pnlClick.Dock = DockStyle.Fill;
            pnlClick.Location = new Point(0, 112);
            pnlClick.Margin = new Padding(0);
            pnlClick.Name = "pnlClick";
            pnlClick.RowCount = 1;
            pnlClick.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlClick.Size = new Size(471, 30);
            pnlClick.TabIndex = 22;
            // 
            // lblClickPoint
            // 
            lblClickPoint.AutoSize = true;
            lblClickPoint.BackColor = Color.Transparent;
            lblClickPoint.ForeColor = Color.White;
            lblClickPoint.Location = new Point(6, 6);
            lblClickPoint.Margin = new Padding(6, 6, 3, 0);
            lblClickPoint.Name = "lblClickPoint";
            lblClickPoint.Size = new Size(77, 15);
            lblClickPoint.TabIndex = 0;
            lblClickPoint.Text = "Click Point %";
            // 
            // txtPointCheck
            // 
            txtPointCheck.BackColor = Color.FromArgb(40, 40, 40);
            txtPointCheck.BorderStyle = BorderStyle.FixedSingle;
            txtPointCheck.ForeColor = Color.White;
            txtPointCheck.Location = new Point(103, 3);
            txtPointCheck.Name = "txtPointCheck";
            txtPointCheck.Size = new Size(120, 23);
            txtPointCheck.TabIndex = 17;
            txtPointCheck.TextChanged += txtPointCheck_TextChanged;
            // 
            // picboxImageCheck
            // 
            picboxImageCheck.BackColor = Color.FromArgb(30, 30, 30);
            tablePanelCheck.SetColumnSpan(picboxImageCheck, 2);
            picboxImageCheck.Dock = DockStyle.Fill;
            picboxImageCheck.Location = new Point(3, 145);
            picboxImageCheck.Name = "picboxImageCheck";
            picboxImageCheck.Size = new Size(465, 92);
            picboxImageCheck.SizeMode = PictureBoxSizeMode.Zoom;
            picboxImageCheck.TabIndex = 5;
            picboxImageCheck.TabStop = false;
            // 
            // lblDetectAtPoint
            // 
            lblDetectAtPoint.AutoSize = true;
            lblDetectAtPoint.ForeColor = Color.White;
            lblDetectAtPoint.Location = new Point(6, 90);
            lblDetectAtPoint.Margin = new Padding(6, 6, 3, 0);
            lblDetectAtPoint.Name = "lblDetectAtPoint";
            lblDetectAtPoint.Size = new Size(154, 15);
            lblDetectAtPoint.TabIndex = 23;
            lblDetectAtPoint.Text = "Detect && Click On Point (%)";
            // 
            // UserCheckNoteImage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(tablePanelCheck);
            Name = "UserCheckNoteImage";
            Size = new Size(471, 240);
            tablePanelCheck.ResumeLayout(false);
            tablePanelCheck.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            pnlCrop.ResumeLayout(false);
            pnlCrop.PerformLayout();
            pnlText.ResumeLayout(false);
            pnlText.PerformLayout();
            pnlClick.ResumeLayout(false);
            pnlClick.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picboxImageCheck).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tablePanelCheck;
        private CheckBox chkActiveCheck;
        private PictureBox picboxImageCheck;
        private LabelNormalN labelDeleteCheck;
        private TableLayoutPanel tableLayoutPanel4;
        private LabelNormalN labelnumber4;
        private NumericUpDownCustomN numbericAccuracy;
        private TextBox txtPointCheck;
        private TableLayoutPanel pnlCrop;
        private LabelNormalN lblCropRegion;
        private TextBox txtCropRegion;
        private TableLayoutPanel pnlText;
        private LabelNormalN lblTextToFind;
        private TableLayoutPanel pnlClick;
        private LabelNormalN lblClickPoint;
        public TextBox txtTextToFind;
        private Label lblDetectAtPoint;
        private Label lblDetectOnly;
        private Label lblDetectAndClick;
    }
}
