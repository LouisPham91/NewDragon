
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormSocicalNetwork
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
                ThemeHelper.ThemeChanged -= OnGlobalThemeChanged;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSocicalNetwork));
            panelMain = new PanelNormalN();
            aotSafeDataGridView1 = new AotSafeDataGridView();
            panelNormaln2 = new PanelNormalN();
            panelNormaln1 = new PanelNormalN();
            panelRoundn1 = new PanelRoundN();
            panelRoundedWithBorder1 = new PanelRoundedWithBorder();
            labelNormaln3 = new LabelNormalN();
            labelNetworkSeleted = new LabelNormalN();
            buttonDeleteNetwork = new ButtomRound();
            labelNormaln2 = new LabelNormalN();
            txt_NetworkName = new TextBoxNoborberN();
            buttonUpdateNetwork = new ButtomRound();
            buttonAddNetwork = new ButtomRound();
            labelFile = new LabelNormalN();
            panel1 = new PanelNormalN();
            textBoxSearch = new TextBoxNoborberN();
            pictureBoxBrightn1 = new PictureBoxBrightN();
            PictureBoxCloseForm = new PictureBoxBrightN();
            labelNormaln1 = new LabelNormalN();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)aotSafeDataGridView1).BeginInit();
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
            panelMain.Controls.Add(panelNormaln2);
            panelMain.Controls.Add(panelNormaln1);
            panelMain.Controls.Add(panelRoundn1);
            panelMain.Controls.Add(panel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.ForeColor = Color.White;
            panelMain.Location = new Point(1, 1);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(431, 574);
            panelMain.TabIndex = 0;
            // 
            // aotSafeDataGridView1
            // 
            aotSafeDataGridView1.BackgroundColor = Color.FromArgb(40, 40, 40);
            aotSafeDataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 245, 245);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(200, 220, 240);
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            aotSafeDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            aotSafeDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(245, 245, 245);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(200, 220, 240);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            aotSafeDataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            aotSafeDataGridView1.Dock = DockStyle.Fill;
            aotSafeDataGridView1.EnableHeadersVisualStyles = false;
            aotSafeDataGridView1.GridColor = Color.FromArgb(200, 200, 200);
            aotSafeDataGridView1.Location = new Point(40, 40);
            aotSafeDataGridView1.Name = "aotSafeDataGridView1";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(245, 245, 245);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(200, 220, 240);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            aotSafeDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            aotSafeDataGridView1.RowHeadersVisible = false;
            aotSafeDataGridView1.Size = new Size(351, 366);
            aotSafeDataGridView1.TabIndex = 6;
            // 
            // panelNormaln2
            // 
            panelNormaln2.BackColor = Color.FromArgb(40, 40, 40);
            panelNormaln2.Dock = DockStyle.Right;
            panelNormaln2.ForeColor = Color.Black;
            panelNormaln2.Location = new Point(391, 40);
            panelNormaln2.Name = "panelNormaln2";
            panelNormaln2.Size = new Size(40, 366);
            panelNormaln2.TabIndex = 5;
            // 
            // panelNormaln1
            // 
            panelNormaln1.BackColor = Color.FromArgb(40, 40, 40);
            panelNormaln1.Dock = DockStyle.Left;
            panelNormaln1.ForeColor = Color.Black;
            panelNormaln1.Location = new Point(0, 40);
            panelNormaln1.Name = "panelNormaln1";
            panelNormaln1.Size = new Size(40, 366);
            panelNormaln1.TabIndex = 4;
            // 
            // panelRoundn1
            // 
            panelRoundn1.BackColor = Color.FromArgb(40, 40, 40);
            panelRoundn1.Controls.Add(panelRoundedWithBorder1);
            panelRoundn1.Controls.Add(buttonDeleteNetwork);
            panelRoundn1.Controls.Add(labelNormaln2);
            panelRoundn1.Controls.Add(txt_NetworkName);
            panelRoundn1.Controls.Add(buttonUpdateNetwork);
            panelRoundn1.Controls.Add(buttonAddNetwork);
            panelRoundn1.Controls.Add(labelFile);
            panelRoundn1.Dock = DockStyle.Bottom;
            panelRoundn1.ForeColor = Color.White;
            panelRoundn1.GD_Radius = 15F;
            panelRoundn1.Location = new Point(0, 406);
            panelRoundn1.Name = "panelRoundn1";
            panelRoundn1.Size = new Size(431, 168);
            panelRoundn1.TabIndex = 3;
            // 
            // panelRoundedWithBorder1
            // 
            panelRoundedWithBorder1.Controls.Add(labelNormaln3);
            panelRoundedWithBorder1.Controls.Add(labelNetworkSeleted);
            panelRoundedWithBorder1.DG_BorderRadius = 15;
            panelRoundedWithBorder1.Location = new Point(30, 76);
            panelRoundedWithBorder1.Name = "panelRoundedWithBorder1";
            panelRoundedWithBorder1.Size = new Size(375, 33);
            panelRoundedWithBorder1.TabIndex = 97;
            // 
            // labelNormaln3
            // 
            labelNormaln3.AutoSize = true;
            labelNormaln3.BackColor = Color.Transparent;
            labelNormaln3.DG_IsBrightBack = true;
            labelNormaln3.ForeColor = Color.White;
            labelNormaln3.Location = new Point(60, 9);
            labelNormaln3.Name = "labelNormaln3";
            labelNormaln3.Size = new Size(64, 15);
            labelNormaln3.TabIndex = 96;
            labelNormaln3.Text = "Selecting : ";
            // 
            // labelNetworkSeleted
            // 
            labelNetworkSeleted.AutoSize = true;
            labelNetworkSeleted.BackColor = Color.Transparent;
            labelNetworkSeleted.DG_IsBrightBack = true;
            labelNetworkSeleted.ForeColor = Color.White;
            labelNetworkSeleted.Location = new Point(126, 9);
            labelNetworkSeleted.Name = "labelNetworkSeleted";
            labelNetworkSeleted.Size = new Size(36, 15);
            labelNetworkSeleted.TabIndex = 95;
            labelNetworkSeleted.Text = "None";
            // 
            // buttonDeleteNetwork
            // 
            buttonDeleteNetwork.BackColor = Color.FromArgb(225, 225, 225);
            buttonDeleteNetwork.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonDeleteNetwork.DG_ForceColor = Color.Empty;
            buttonDeleteNetwork.FlatAppearance.BorderSize = 0;
            buttonDeleteNetwork.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonDeleteNetwork.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonDeleteNetwork.FlatStyle = FlatStyle.Flat;
            buttonDeleteNetwork.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonDeleteNetwork.ForeColor = Color.FromArgb(255, 70, 70);
            buttonDeleteNetwork.GD_Radius = 5F;
            buttonDeleteNetwork.ImageAlign = ContentAlignment.MiddleLeft;
            buttonDeleteNetwork.Location = new Point(284, 120);
            buttonDeleteNetwork.Name = "buttonDeleteNetwork";
            buttonDeleteNetwork.Padding = new Padding(25, 0, 20, 0);
            buttonDeleteNetwork.Size = new Size(121, 30);
            buttonDeleteNetwork.TabIndex = 94;
            buttonDeleteNetwork.TabStop = false;
            buttonDeleteNetwork.Text = "Delete";
            buttonDeleteNetwork.UseVisualStyleBackColor = false;
            buttonDeleteNetwork.Click += buttonDeleteNetwork_Click;
            // 
            // labelNormaln2
            // 
            labelNormaln2.AutoSize = true;
            labelNormaln2.BackColor = Color.Transparent;
            labelNormaln2.DG_IsBrightBack = true;
            labelNormaln2.ForeColor = Color.White;
            labelNormaln2.Location = new Point(31, 21);
            labelNormaln2.Name = "labelNormaln2";
            labelNormaln2.Size = new Size(87, 15);
            labelNormaln2.TabIndex = 93;
            labelNormaln2.Text = "Network Name";
            // 
            // txt_NetworkName
            // 
            txt_NetworkName.BackColor = Color.Gainsboro;
            txt_NetworkName.BorderStyle = BorderStyle.FixedSingle;
            txt_NetworkName.ForeColor = Color.Black;
            txt_NetworkName.Location = new Point(30, 41);
            txt_NetworkName.Multiline = true;
            txt_NetworkName.Name = "txt_NetworkName";
            txt_NetworkName.Size = new Size(375, 23);
            txt_NetworkName.TabIndex = 92;
            txt_NetworkName.Text = "Facebook";
            // 
            // buttonUpdateNetwork
            // 
            buttonUpdateNetwork.BackColor = Color.FromArgb(225, 225, 225);
            buttonUpdateNetwork.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonUpdateNetwork.DG_ForceColor = Color.Empty;
            buttonUpdateNetwork.FlatAppearance.BorderSize = 0;
            buttonUpdateNetwork.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonUpdateNetwork.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonUpdateNetwork.FlatStyle = FlatStyle.Flat;
            buttonUpdateNetwork.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonUpdateNetwork.ForeColor = Color.FromArgb(255, 70, 70);
            buttonUpdateNetwork.GD_Radius = 5F;
            buttonUpdateNetwork.ImageAlign = ContentAlignment.MiddleLeft;
            buttonUpdateNetwork.Location = new Point(157, 120);
            buttonUpdateNetwork.Name = "buttonUpdateNetwork";
            buttonUpdateNetwork.Padding = new Padding(25, 0, 20, 0);
            buttonUpdateNetwork.Size = new Size(121, 30);
            buttonUpdateNetwork.TabIndex = 19;
            buttonUpdateNetwork.TabStop = false;
            buttonUpdateNetwork.Text = "Update";
            buttonUpdateNetwork.UseVisualStyleBackColor = false;
            buttonUpdateNetwork.Click += buttonUpdateNetwork_Click;
            // 
            // buttonAddNetwork
            // 
            buttonAddNetwork.BackColor = Color.FromArgb(225, 225, 225);
            buttonAddNetwork.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonAddNetwork.DG_ForceColor = Color.Empty;
            buttonAddNetwork.FlatAppearance.BorderSize = 0;
            buttonAddNetwork.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonAddNetwork.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonAddNetwork.FlatStyle = FlatStyle.Flat;
            buttonAddNetwork.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonAddNetwork.ForeColor = Color.FromArgb(255, 70, 70);
            buttonAddNetwork.GD_Radius = 5F;
            buttonAddNetwork.ImageAlign = ContentAlignment.MiddleLeft;
            buttonAddNetwork.Location = new Point(30, 120);
            buttonAddNetwork.Name = "buttonAddNetwork";
            buttonAddNetwork.Padding = new Padding(25, 0, 20, 0);
            buttonAddNetwork.Size = new Size(121, 30);
            buttonAddNetwork.TabIndex = 18;
            buttonAddNetwork.TabStop = false;
            buttonAddNetwork.Text = "Add New";
            buttonAddNetwork.UseVisualStyleBackColor = false;
            buttonAddNetwork.Click += buttonAddNetwork_Click;
            // 
            // labelFile
            // 
            labelFile.AutoSize = true;
            labelFile.BackColor = Color.Transparent;
            labelFile.DG_IsBrightBack = true;
            labelFile.Font = new Font("Segoe UI", 9.75F);
            labelFile.ForeColor = Color.Black;
            labelFile.Location = new Point(22, 14);
            labelFile.Name = "labelFile";
            labelFile.Size = new Size(0, 17);
            labelFile.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(40, 40, 40);
            panel1.Controls.Add(textBoxSearch);
            panel1.Controls.Add(pictureBoxBrightn1);
            panel1.Controls.Add(PictureBoxCloseForm);
            panel1.Controls.Add(labelNormaln1);
            panel1.Dock = DockStyle.Top;
            panel1.ForeColor = Color.White;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(431, 40);
            panel1.TabIndex = 0;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxSearch.BackColor = Color.Gray;
            textBoxSearch.BorderStyle = BorderStyle.FixedSingle;
            textBoxSearch.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxSearch.ForeColor = Color.Black;
            textBoxSearch.Location = new Point(246, 10);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(124, 23);
            textBoxSearch.TabIndex = 12;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            // 
            // pictureBoxBrightn1
            // 
            pictureBoxBrightn1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBoxBrightn1.BackColor = Color.FromArgb(40, 40, 40);
            pictureBoxBrightn1.DG_ImageSize = new Size(18, 18);
            pictureBoxBrightn1.DG_IsBrightBack = false;
            pictureBoxBrightn1.DG_LightenPercent = 50;
            pictureBoxBrightn1.DG_SVGString = resources.GetString("pictureBoxBrightn1.DG_SVGString");
            pictureBoxBrightn1.ForeColor = Color.White;
            pictureBoxBrightn1.Image = (Image)resources.GetObject("pictureBoxBrightn1.Image");
            pictureBoxBrightn1.Location = new Point(222, 12);
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
            PictureBoxCloseForm.BackColor = Color.FromArgb(40, 40, 40);
            PictureBoxCloseForm.DG_ImageSize = new Size(18, 18);
            PictureBoxCloseForm.DG_LightenPercent = 50;
            PictureBoxCloseForm.DG_SVGString = resources.GetString("PictureBoxCloseForm.DG_SVGString");
            PictureBoxCloseForm.ForeColor = Color.Black;
            PictureBoxCloseForm.Image = (Image)resources.GetObject("PictureBoxCloseForm.Image");
            PictureBoxCloseForm.Location = new Point(393, 10);
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
            labelNormaln1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNormaln1.ForeColor = Color.White;
            labelNormaln1.Location = new Point(22, 10);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(114, 20);
            labelNormaln1.TabIndex = 0;
            labelNormaln1.Text = "Social Network";
            // 
            // FormSocicalNetwork
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(168, 168, 168);
            ClientSize = new Size(433, 576);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormSocicalNetwork";
            Opacity = 0.97D;
            Padding = new Padding(1);
            Text = "FormInstallAPK";
            Load += FormAndroidExplorer_Load;
            panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)aotSafeDataGridView1).EndInit();
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
        private PictureBoxBrightN PictureBoxCloseForm;
        private LabelNormalN labelFile;
        private PanelRoundN panelRoundn1;
        private ButtomRound buttonAddNetwork;
        private LabelNormalN labelNormaln1;
        private ButtomRound buttonUpdateNetwork;
        private TextBoxNoborberN textBoxSearch;
        private PictureBoxBrightN pictureBoxBrightn1;
        private TextBoxNoborberN txt_NetworkName;
        private LabelNormalN labelNormaln2;
        private ButtomRound buttonDeleteNetwork;
        private LabelNormalN labelNetworkSeleted;
        private LabelNormalN labelNormaln3;
        private PanelRoundedWithBorder panelRoundedWithBorder1;
        private AotSafeDataGridView aotSafeDataGridView1;
        private PanelNormalN panelNormaln2;
        private PanelNormalN panelNormaln1;
        private PanelNormalN panelMain;
        private PanelNormalN panel1;
    }
}