
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormKeybroadSetting
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormKeybroadSetting));
            panelMain = new Panel();
            aotSafeDataGridView1 = new AotSafeDataGridView();
            panelRoundn1 = new PanelRoundN();
            buttonUpdateRow = new ButtomRound();
            buttonDeleteRow = new ButtomRound();
            labelFile = new LabelNormalN();
            panel1 = new Panel();
            textBoxSearch = new TextBoxNoborberN();
            pictureBoxBrightn1 = new PictureBoxBrightN();
            PictureBoxCloseForm = new PictureBoxBrightN();
            labelNormaln1 = new LabelNormalN();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)aotSafeDataGridView1).BeginInit();
            panelRoundn1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.WhiteSmoke;
            panelMain.Controls.Add(aotSafeDataGridView1);
            panelMain.Controls.Add(panelRoundn1);
            panelMain.Controls.Add(panel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(1, 1);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(727, 486);
            panelMain.TabIndex = 0;
            // 
            // aotSafeDataGridView1
            // 
            aotSafeDataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            aotSafeDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            aotSafeDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle5.SelectionForeColor = Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            aotSafeDataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            aotSafeDataGridView1.Dock = DockStyle.Fill;
            aotSafeDataGridView1.EnableHeadersVisualStyles = false;
            aotSafeDataGridView1.GridColor = Color.Gray;
            aotSafeDataGridView1.Location = new Point(0, 40);
            aotSafeDataGridView1.Name = "aotSafeDataGridView1";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle6.SelectionForeColor = Color.White;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            aotSafeDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            aotSafeDataGridView1.RowHeadersVisible = false;
            aotSafeDataGridView1.Size = new Size(727, 362);
            aotSafeDataGridView1.TabIndex = 4;
            // 
            // panelRoundn1
            // 
            panelRoundn1.Controls.Add(buttonUpdateRow);
            panelRoundn1.Controls.Add(buttonDeleteRow);
            panelRoundn1.Controls.Add(labelFile);
            panelRoundn1.Dock = DockStyle.Bottom;
            panelRoundn1.GD_Radius = 15F;
            panelRoundn1.Location = new Point(0, 402);
            panelRoundn1.Name = "panelRoundn1";
            panelRoundn1.Size = new Size(727, 84);
            panelRoundn1.TabIndex = 3;
            // 
            // buttonUpdateRow
            // 
            buttonUpdateRow.BackColor = Color.FromArgb(225, 225, 225);
            buttonUpdateRow.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonUpdateRow.DG_ForceColor = Color.Empty;
            buttonUpdateRow.DG_LightenPercent = 0.2F;
            buttonUpdateRow.DG_SVGString = "";
            buttonUpdateRow.FlatAppearance.BorderSize = 0;
            buttonUpdateRow.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonUpdateRow.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonUpdateRow.FlatStyle = FlatStyle.Flat;
            buttonUpdateRow.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonUpdateRow.ForeColor = Color.FromArgb(255, 70, 70);
            buttonUpdateRow.GD_Radius = 5F;
            buttonUpdateRow.ImageAlign = ContentAlignment.MiddleLeft;
            buttonUpdateRow.Location = new Point(201, 34);
            buttonUpdateRow.Name = "buttonUpdateRow";
            buttonUpdateRow.Padding = new Padding(25, 0, 20, 0);
            buttonUpdateRow.Size = new Size(173, 30);
            buttonUpdateRow.TabIndex = 19;
            buttonUpdateRow.TabStop = false;
            buttonUpdateRow.Text = "Update";
            buttonUpdateRow.UseVisualStyleBackColor = false;
            buttonUpdateRow.Click += buttonUpdateRow_Click;
            // 
            // buttonDeleteRow
            // 
            buttonDeleteRow.BackColor = Color.FromArgb(225, 225, 225);
            buttonDeleteRow.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonDeleteRow.DG_ForceColor = Color.Empty;
            buttonDeleteRow.DG_LightenPercent = 0.2F;
            buttonDeleteRow.DG_SVGString = "";
            buttonDeleteRow.FlatAppearance.BorderSize = 0;
            buttonDeleteRow.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonDeleteRow.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonDeleteRow.FlatStyle = FlatStyle.Flat;
            buttonDeleteRow.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonDeleteRow.ForeColor = Color.FromArgb(255, 70, 70);
            buttonDeleteRow.GD_Radius = 5F;
            buttonDeleteRow.ImageAlign = ContentAlignment.MiddleLeft;
            buttonDeleteRow.Location = new Point(22, 34);
            buttonDeleteRow.Name = "buttonDeleteRow";
            buttonDeleteRow.Padding = new Padding(25, 0, 20, 0);
            buttonDeleteRow.Size = new Size(173, 30);
            buttonDeleteRow.TabIndex = 18;
            buttonDeleteRow.TabStop = false;
            buttonDeleteRow.Text = "Delete";
            buttonDeleteRow.UseVisualStyleBackColor = false;
            buttonDeleteRow.Click += buttonDeleteRow_Click;
            // 
            // labelFile
            // 
            labelFile.AutoSize = true;
            labelFile.BackColor = Color.Transparent;
            labelFile.DG_BackColor = Color.Transparent;
            labelFile.DG_LightenPercent = 80;
            labelFile.DG_SVGImageColor = Color.Transparent;
            labelFile.DG_SVGString = "";
            labelFile.Font = new Font("Segoe UI", 9.75F);
            labelFile.DG_IsBrightBack = true;
            labelFile.DG_IsColorMode = false;
            labelFile.DG_IsGrayImage = false;
            labelFile.Location = new Point(22, 14);
            labelFile.Name = "labelFile";
            labelFile.Size = new Size(0, 17);
            labelFile.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(textBoxSearch);
            panel1.Controls.Add(pictureBoxBrightn1);
            panel1.Controls.Add(PictureBoxCloseForm);
            panel1.Controls.Add(labelNormaln1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(727, 40);
            panel1.TabIndex = 0;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxSearch.BackColor = SystemColors.Window;
            textBoxSearch.BorderStyle = BorderStyle.FixedSingle;
            textBoxSearch.DG_BorderThickness = 1;
            textBoxSearch.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxSearch.Location = new Point(442, 10);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(224, 23);
            textBoxSearch.TabIndex = 12;
            // 
            // pictureBoxBrightn1
            // 
            pictureBoxBrightn1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBoxBrightn1.DG_ImageColor = SystemColors.ControlText;
            pictureBoxBrightn1.DG_ImageSize = new Size(18, 18);
            pictureBoxBrightn1.DG_LightenPercent = 50;
            pictureBoxBrightn1.Image = (Image)resources.GetObject("pictureBoxBrightn1.Image");
            pictureBoxBrightn1.DG_IsBackTransparent = false;
            pictureBoxBrightn1.DG_IsBrightBack = true;
            pictureBoxBrightn1.DG_IsGrayImage = false;
            pictureBoxBrightn1.DG_IsWhiteImage = false;
            pictureBoxBrightn1.Location = new Point(418, 12);
            pictureBoxBrightn1.Name = "pictureBoxBrightn1";
            pictureBoxBrightn1.Padding = new Padding(2);
            pictureBoxBrightn1.Size = new Size(18, 18);
            pictureBoxBrightn1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxBrightn1.DG_SVGString = resources.GetString("pictureBoxBrightn1.SVGstring");
            pictureBoxBrightn1.TabIndex = 11;
            pictureBoxBrightn1.TabStop = false;
            pictureBoxBrightn1.DG_UseMode = UseMode.ThemeMode;
            // 
            // PictureBoxCloseForm
            // 
            PictureBoxCloseForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PictureBoxCloseForm.DG_ImageColor = SystemColors.ControlText;
            PictureBoxCloseForm.DG_ImageSize = new Size(18, 18);
            PictureBoxCloseForm.DG_LightenPercent = 50;
            PictureBoxCloseForm.Image = (Image)resources.GetObject("PictureBoxCloseForm.Image");
            PictureBoxCloseForm.DG_IsBackTransparent = false;
            PictureBoxCloseForm.DG_IsBrightBack = true;
            PictureBoxCloseForm.DG_IsGrayImage = false;
            PictureBoxCloseForm.DG_IsWhiteImage = false;
            PictureBoxCloseForm.Location = new Point(689, 10);
            PictureBoxCloseForm.Name = "PictureBoxCloseForm";
            PictureBoxCloseForm.Padding = new Padding(2);
            PictureBoxCloseForm.Size = new Size(18, 18);
            PictureBoxCloseForm.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBoxCloseForm.DG_SVGString = resources.GetString("PictureBoxCloseForm.SVGstring");
            PictureBoxCloseForm.TabIndex = 8;
            PictureBoxCloseForm.TabStop = false;
            PictureBoxCloseForm.DG_UseMode = UseMode.ThemeMode;
            PictureBoxCloseForm.Click += PictureBoxCloseForm_Click;
            // 
            // labelNormaln1
            // 
            labelNormaln1.AutoSize = true;
            labelNormaln1.BackColor = Color.Transparent;
            labelNormaln1.DG_BackColor = Color.Transparent;
            labelNormaln1.DG_LightenPercent = 80;
            labelNormaln1.DG_SVGImageColor = Color.Transparent;
            labelNormaln1.DG_SVGString = "";
            labelNormaln1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNormaln1.DG_IsBrightBack = false;
            labelNormaln1.DG_IsColorMode = false;
            labelNormaln1.DG_IsGrayImage = false;
            labelNormaln1.Location = new Point(22, 10);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(131, 20);
            labelNormaln1.TabIndex = 0;
            labelNormaln1.Text = "KeyBroad Setting";
            // 
            // FormKeybroadSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(168, 168, 168);
            ClientSize = new Size(729, 488);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormKeybroadSetting";
            Opacity = 0.97D;
            Padding = new Padding(1);
            Text = "FormInstallAPK";
            Load += FormAndroidExplorer_Load;
            panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)aotSafeDataGridView1).EndInit();
            panelRoundn1.ResumeLayout(false);
            panelRoundn1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private Panel panel1;
        private PictureBoxBrightN PictureBoxCloseForm;
        private LabelNormalN labelFile;
        private PanelRoundN panelRoundn1;
        private ButtomRound buttonDeleteRow;
        private LabelNormalN labelNormaln1;
        private ButtomRound buttonUpdateRow;
        private TextBoxNoborberN textBoxSearch;
        private PictureBoxBrightN pictureBoxBrightn1;
        private AotSafeDataGridView aotSafeDataGridView1;
    }
}