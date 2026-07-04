
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormAppData
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAppData));
            panelMain = new Panel();
            aotSafeDataGridView1 = new AotSafeDataGridView();
            aotSafeDataGridView2 = new AotSafeDataGridView();
            panelRoundn1 = new PanelRoundN();
            buttomDeleteAppData = new ButtomRound();
            panelRoundedWithBorder1 = new PanelRoundedWithBorder();
            labelSeletedPhone = new LabelNormalN();
            buttonAddOne_Appdata = new ButtomRound();
            labelFile = new LabelNormalN();
            panel1 = new Panel();
            textBoxSearch = new TextBoxNoborberN();
            pictureBoxBrightn1 = new PictureBoxBrightN();
            PictureBoxCloseForm = new PictureBoxBrightN();
            labelNormaln1 = new LabelNormalN();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)aotSafeDataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)aotSafeDataGridView2).BeginInit();
            panelRoundn1.SuspendLayout();
            panelRoundedWithBorder1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.WhiteSmoke;
            panelMain.Controls.Add(aotSafeDataGridView1);
            panelMain.Controls.Add(aotSafeDataGridView2);
            panelMain.Controls.Add(panelRoundn1);
            panelMain.Controls.Add(panel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(1, 1);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1319, 868);
            panelMain.TabIndex = 0;
            // 
            // aotSafeDataGridView1
            // 
            aotSafeDataGridView1.BackgroundColor = Color.White;
            aotSafeDataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            aotSafeDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            aotSafeDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            aotSafeDataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            aotSafeDataGridView1.Dock = DockStyle.Fill;
            aotSafeDataGridView1.EnableHeadersVisualStyles = false;
            aotSafeDataGridView1.GridColor = Color.Gray;
            aotSafeDataGridView1.Location = new Point(240, 40);
            aotSafeDataGridView1.Name = "aotSafeDataGridView1";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            aotSafeDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            aotSafeDataGridView1.RowHeadersVisible = false;
            aotSafeDataGridView1.Size = new Size(1079, 731);
            aotSafeDataGridView1.TabIndex = 6;
            // 
            // aotSafeDataGridView2
            // 
            aotSafeDataGridView2.BackgroundColor = Color.FromArgb(245, 245, 245);
            aotSafeDataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(245, 245, 245);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(200, 220, 240);
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            aotSafeDataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            aotSafeDataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(245, 245, 245);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(200, 220, 240);
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            aotSafeDataGridView2.DefaultCellStyle = dataGridViewCellStyle5;
            aotSafeDataGridView2.Dock = DockStyle.Left;
            aotSafeDataGridView2.EnableHeadersVisualStyles = false;
            aotSafeDataGridView2.GridColor = Color.FromArgb(200, 200, 200);
            aotSafeDataGridView2.Location = new Point(0, 40);
            aotSafeDataGridView2.Name = "aotSafeDataGridView2";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(245, 245, 245);
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(200, 220, 240);
            dataGridViewCellStyle6.SelectionForeColor = Color.Black;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            aotSafeDataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            aotSafeDataGridView2.RowHeadersVisible = false;
            aotSafeDataGridView2.Size = new Size(240, 731);
            aotSafeDataGridView2.TabIndex = 5;
            // 
            // panelRoundn1
            // 
            panelRoundn1.Controls.Add(buttomDeleteAppData);
            panelRoundn1.Controls.Add(panelRoundedWithBorder1);
            panelRoundn1.Controls.Add(buttonAddOne_Appdata);
            panelRoundn1.Controls.Add(labelFile);
            panelRoundn1.Dock = DockStyle.Bottom;
            panelRoundn1.GD_Radius = 15F;
            panelRoundn1.Location = new Point(0, 771);
            panelRoundn1.Name = "panelRoundn1";
            panelRoundn1.Size = new Size(1319, 97);
            panelRoundn1.TabIndex = 3;
            // 
            // buttomDeleteAppData
            // 
            buttomDeleteAppData.BackColor = Color.FromArgb(225, 225, 225);
            buttomDeleteAppData.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttomDeleteAppData.DG_ForceColor = Color.Empty;
            buttomDeleteAppData.DG_LightenPercent = 0.2F;
            buttomDeleteAppData.DG_SVGString = "";
            buttomDeleteAppData.FlatAppearance.BorderSize = 0;
            buttomDeleteAppData.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttomDeleteAppData.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttomDeleteAppData.FlatStyle = FlatStyle.Flat;
            buttomDeleteAppData.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttomDeleteAppData.ForeColor = Color.FromArgb(255, 70, 70);
            buttomDeleteAppData.GD_Radius = 5F;
            buttomDeleteAppData.ImageAlign = ContentAlignment.MiddleLeft;
            buttomDeleteAppData.Location = new Point(1093, 37);
            buttomDeleteAppData.Name = "buttomDeleteAppData";
            buttomDeleteAppData.Padding = new Padding(25, 0, 20, 0);
            buttomDeleteAppData.Size = new Size(182, 30);
            buttomDeleteAppData.TabIndex = 20;
            buttomDeleteAppData.TabStop = false;
            buttomDeleteAppData.Text = "Delete One AppData";
            buttomDeleteAppData.UseVisualStyleBackColor = false;
            buttomDeleteAppData.Click += buttomDeleteAppData_Click;
            // 
            // panelRoundedWithBorder1
            // 
            panelRoundedWithBorder1.Controls.Add(labelSeletedPhone);
            panelRoundedWithBorder1.DG_BorderColor = Color.Gray;
            panelRoundedWithBorder1.DG_BorderDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            panelRoundedWithBorder1.DG_BorderRadius = 20;
            panelRoundedWithBorder1.DG_BorderThickness = 2;
            panelRoundedWithBorder1.Location = new Point(3, 14);
            panelRoundedWithBorder1.Name = "panelRoundedWithBorder1";
            panelRoundedWithBorder1.Size = new Size(404, 35);
            panelRoundedWithBorder1.TabIndex = 19;
            // 
            // labelSeletedPhone
            // 
            labelSeletedPhone.AutoSize = true;
            labelSeletedPhone.BackColor = Color.Transparent;
            labelSeletedPhone.DG_BackColor = Color.Transparent;
            labelSeletedPhone.DG_IsBrightBack = false;
            labelSeletedPhone.DG_IsColorMode = false;
            labelSeletedPhone.DG_IsGrayImage = false;
            labelSeletedPhone.DG_LightenPercent = 80;
            labelSeletedPhone.DG_SVGImageColor = Color.Transparent;
            labelSeletedPhone.DG_SVGString = "";
            labelSeletedPhone.ForeColor = Color.Black;
            labelSeletedPhone.Location = new Point(37, 11);
            labelSeletedPhone.Name = "labelSeletedPhone";
            labelSeletedPhone.Size = new Size(60, 15);
            labelSeletedPhone.TabIndex = 0;
            labelSeletedPhone.Text = "Selected : ";
            // 
            // buttonAddOne_Appdata
            // 
            buttonAddOne_Appdata.BackColor = Color.FromArgb(225, 225, 225);
            buttonAddOne_Appdata.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonAddOne_Appdata.DG_ForceColor = Color.Empty;
            buttonAddOne_Appdata.DG_LightenPercent = 0.2F;
            buttonAddOne_Appdata.DG_SVGString = "";
            buttonAddOne_Appdata.FlatAppearance.BorderSize = 0;
            buttonAddOne_Appdata.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonAddOne_Appdata.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonAddOne_Appdata.FlatStyle = FlatStyle.Flat;
            buttonAddOne_Appdata.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonAddOne_Appdata.ForeColor = Color.FromArgb(255, 70, 70);
            buttonAddOne_Appdata.GD_Radius = 5F;
            buttonAddOne_Appdata.ImageAlign = ContentAlignment.MiddleLeft;
            buttonAddOne_Appdata.Location = new Point(888, 37);
            buttonAddOne_Appdata.Name = "buttonAddOne_Appdata";
            buttonAddOne_Appdata.Padding = new Padding(25, 0, 20, 0);
            buttonAddOne_Appdata.Size = new Size(182, 30);
            buttonAddOne_Appdata.TabIndex = 18;
            buttonAddOne_Appdata.TabStop = false;
            buttonAddOne_Appdata.Text = "Add One AppData";
            buttonAddOne_Appdata.UseVisualStyleBackColor = false;
            buttonAddOne_Appdata.Click += buttonAddOne_Appdata_Click;
            // 
            // labelFile
            // 
            labelFile.AutoSize = true;
            labelFile.BackColor = Color.Transparent;
            labelFile.DG_BackColor = Color.Transparent;
            labelFile.DG_IsBrightBack = true;
            labelFile.DG_IsColorMode = false;
            labelFile.DG_IsGrayImage = false;
            labelFile.DG_LightenPercent = 80;
            labelFile.DG_SVGImageColor = Color.Transparent;
            labelFile.DG_SVGString = "";
            labelFile.Font = new Font("Segoe UI", 9.75F);
            labelFile.ForeColor = Color.Black;
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
            panel1.Size = new Size(1319, 40);
            panel1.TabIndex = 0;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxSearch.BackColor = SystemColors.Window;
            textBoxSearch.BorderStyle = BorderStyle.FixedSingle;
            textBoxSearch.DG_BorderThickness = 1;
            textBoxSearch.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxSearch.ForeColor = Color.Black;
            textBoxSearch.Location = new Point(1034, 10);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(224, 23);
            textBoxSearch.TabIndex = 12;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            // 
            // pictureBoxBrightn1
            // 
            pictureBoxBrightn1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBoxBrightn1.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxBrightn1.DG_ImageColor = Color.Black;
            pictureBoxBrightn1.DG_ImageSize = new Size(18, 18);
            pictureBoxBrightn1.DG_IsBackTransparent = false;
            pictureBoxBrightn1.DG_IsBrightBack = false;
            pictureBoxBrightn1.DG_IsGrayImage = false;
            pictureBoxBrightn1.DG_IsWhiteImage = false;
            pictureBoxBrightn1.DG_LightenPercent = 50;
            pictureBoxBrightn1.DG_SVGString = resources.GetString("pictureBoxBrightn1.DG_SVGString");
            pictureBoxBrightn1.DG_UseMode = UseMode.ThemeMode;
            pictureBoxBrightn1.ForeColor = Color.Black;
            pictureBoxBrightn1.Image = (Image)resources.GetObject("pictureBoxBrightn1.Image");
            pictureBoxBrightn1.Location = new Point(1010, 12);
            pictureBoxBrightn1.Name = "pictureBoxBrightn1";
            pictureBoxBrightn1.Padding = new Padding(2);
            pictureBoxBrightn1.Size = new Size(18, 18);
            pictureBoxBrightn1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxBrightn1.TabIndex = 11;
            pictureBoxBrightn1.TabStop = false;
            // 
            // PictureBoxCloseForm
            // 
            PictureBoxCloseForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PictureBoxCloseForm.BackColor = Color.FromArgb(245, 245, 245);
            PictureBoxCloseForm.DG_ImageColor = Color.Black;
            PictureBoxCloseForm.DG_ImageSize = new Size(18, 18);
            PictureBoxCloseForm.DG_IsBackTransparent = false;
            PictureBoxCloseForm.DG_IsBrightBack = true;
            PictureBoxCloseForm.DG_IsGrayImage = false;
            PictureBoxCloseForm.DG_IsWhiteImage = false;
            PictureBoxCloseForm.DG_LightenPercent = 50;
            PictureBoxCloseForm.DG_SVGString = resources.GetString("PictureBoxCloseForm.DG_SVGString");
            PictureBoxCloseForm.DG_UseMode = UseMode.ThemeMode;
            PictureBoxCloseForm.ForeColor = Color.Black;
            PictureBoxCloseForm.Image = (Image)resources.GetObject("PictureBoxCloseForm.Image");
            PictureBoxCloseForm.Location = new Point(1281, 10);
            PictureBoxCloseForm.Name = "PictureBoxCloseForm";
            PictureBoxCloseForm.Padding = new Padding(2);
            PictureBoxCloseForm.Size = new Size(18, 18);
            PictureBoxCloseForm.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBoxCloseForm.TabIndex = 8;
            PictureBoxCloseForm.TabStop = false;
            PictureBoxCloseForm.Click += PictureBoxCloseForm_Click;
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
            labelNormaln1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNormaln1.ForeColor = Color.Black;
            labelNormaln1.Location = new Point(22, 10);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(127, 20);
            labelNormaln1.TabIndex = 0;
            labelNormaln1.Text = "Control AppData";
            // 
            // FormAppData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(168, 168, 168);
            ClientSize = new Size(1321, 870);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormAppData";
            Opacity = 0.97D;
            Padding = new Padding(1);
            Text = "FormInstallAPK";
            Load += FormAppData_Load;
            panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)aotSafeDataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)aotSafeDataGridView2).EndInit();
            panelRoundn1.ResumeLayout(false);
            panelRoundn1.PerformLayout();
            panelRoundedWithBorder1.ResumeLayout(false);
            panelRoundedWithBorder1.PerformLayout();
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
        private ButtomRound buttonAddOne_Appdata;
        private LabelNormalN labelNormaln1;
        private TextBoxNoborberN textBoxSearch;
        private PictureBoxBrightN pictureBoxBrightn1;
        private AotSafeDataGridView aotSafeDataGridView2;
        private AotSafeDataGridView aotSafeDataGridView1;
        private ButtomRound buttomDeleteAppData;
        private PanelRoundedWithBorder panelRoundedWithBorder1;
        private LabelNormalN labelSeletedPhone;
    }
}