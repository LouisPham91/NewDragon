
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormExportFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportFile));
            panelMain = new Panel();
            tree1 = new AntdUI.Tree();
            panelRoundn1 = new PanelRoundN();
            buttomRound1 = new ButtomRound();
            buttonExportFile = new ButtomRound();
            labelFile = new LabelNormalN();
            panel1 = new Panel();
            pictureBoxBrightn1 = new PictureBoxBrightN();
            PictureBoxCloseForm = new PictureBoxBrightN();
            labelNormaln1 = new LabelNormalN();
            textBoxSearch = new TextBoxNoborberN();
            panelMain.SuspendLayout();
            panelRoundn1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.WhiteSmoke;
            panelMain.Controls.Add(tree1);
            panelMain.Controls.Add(panelRoundn1);
            panelMain.Controls.Add(panel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(1, 1);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(839, 768);
            panelMain.TabIndex = 0;
            // 
            // tree1
            // 
            tree1.Dock = DockStyle.Fill;
            tree1.Gap = 4;
            tree1.Location = new Point(0, 40);
            tree1.Multiple = true;
            tree1.Name = "tree1";
            tree1.Size = new Size(839, 611);
            tree1.TabIndex = 4;
            tree1.Text = "tree1";
            // 
            // panelRoundn1
            // 
            panelRoundn1.BackColor = Color.FromArgb(245, 245, 245);
            panelRoundn1.Controls.Add(buttomRound1);
            panelRoundn1.Controls.Add(buttonExportFile);
            panelRoundn1.Controls.Add(labelFile);
            panelRoundn1.Dock = DockStyle.Bottom;
            panelRoundn1.ForeColor = Color.Black;
            panelRoundn1.GD_Radius = 15F;
            panelRoundn1.Location = new Point(0, 651);
            panelRoundn1.Name = "panelRoundn1";
            panelRoundn1.Size = new Size(839, 117);
            panelRoundn1.TabIndex = 3;
            // 
            // buttomRound1
            // 
            buttomRound1.BackColor = Color.FromArgb(225, 225, 225);
            buttomRound1.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttomRound1.DG_ForceColor = Color.Empty;
            buttomRound1.FlatAppearance.BorderSize = 0;
            buttomRound1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttomRound1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttomRound1.FlatStyle = FlatStyle.Flat;
            buttomRound1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttomRound1.ForeColor = Color.FromArgb(255, 70, 70);
            buttomRound1.GD_Radius = 5F;
            buttomRound1.ImageAlign = ContentAlignment.MiddleLeft;
            buttomRound1.Location = new Point(646, 60);
            buttomRound1.Name = "buttomRound1";
            buttomRound1.Padding = new Padding(25, 0, 20, 0);
            buttomRound1.Size = new Size(173, 30);
            buttomRound1.TabIndex = 19;
            buttomRound1.TabStop = false;
            buttomRound1.Text = "Export Folder";
            buttomRound1.UseVisualStyleBackColor = false;
            buttomRound1.Click += buttomRound1_Click;
            // 
            // buttonExportFile
            // 
            buttonExportFile.BackColor = Color.FromArgb(225, 225, 225);
            buttonExportFile.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonExportFile.DG_ForceColor = Color.Empty;
            buttonExportFile.FlatAppearance.BorderSize = 0;
            buttonExportFile.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonExportFile.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonExportFile.FlatStyle = FlatStyle.Flat;
            buttonExportFile.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonExportFile.ForeColor = Color.FromArgb(255, 70, 70);
            buttonExportFile.GD_Radius = 5F;
            buttonExportFile.ImageAlign = ContentAlignment.MiddleLeft;
            buttonExportFile.Location = new Point(452, 60);
            buttonExportFile.Name = "buttonExportFile";
            buttonExportFile.Padding = new Padding(25, 0, 20, 0);
            buttonExportFile.Size = new Size(173, 30);
            buttonExportFile.TabIndex = 18;
            buttonExportFile.TabStop = false;
            buttonExportFile.Text = "Export File";
            buttonExportFile.UseVisualStyleBackColor = false;
            buttonExportFile.Click += buttonExportFile_Click;
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
            panel1.Controls.Add(textBoxSearch);
            panel1.Controls.Add(pictureBoxBrightn1);
            panel1.Controls.Add(PictureBoxCloseForm);
            panel1.Controls.Add(labelNormaln1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(839, 40);
            panel1.TabIndex = 0;
            // 
            // pictureBoxBrightn1
            // 
            pictureBoxBrightn1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBoxBrightn1.BackColor = Color.FromArgb(245, 245, 245);
            pictureBoxBrightn1.DG_ImageColor = SystemColors.ControlText;
            pictureBoxBrightn1.DG_ImageSize = new Size(18, 18);
            pictureBoxBrightn1.DG_LightenPercent = 50;
            pictureBoxBrightn1.ForeColor = Color.Black;
            pictureBoxBrightn1.Location = new Point(530, 12);
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
            PictureBoxCloseForm.DG_ImageColor = SystemColors.ControlText;
            PictureBoxCloseForm.DG_ImageSize = new Size(18, 18);
            PictureBoxCloseForm.DG_LightenPercent = 50;
            PictureBoxCloseForm.DG_SVGString = resources.GetString("PictureBoxCloseForm.DG_SVGString");
            PictureBoxCloseForm.ForeColor = Color.Black;
            PictureBoxCloseForm.Image = (Image)resources.GetObject("PictureBoxCloseForm.Image");
            PictureBoxCloseForm.Location = new Point(801, 10);
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
            labelNormaln1.ForeColor = Color.Black;
            labelNormaln1.Location = new Point(22, 10);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(156, 20);
            labelNormaln1.TabIndex = 0;
            labelNormaln1.Text = "Android Explorer File";
            // 
            // textBoxSearch
            // 
            textBoxSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxSearch.BackColor = Color.FromArgb(245, 245, 245);
            textBoxSearch.BorderStyle = BorderStyle.FixedSingle;
            textBoxSearch.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxSearch.ForeColor = Color.Black;
            textBoxSearch.Location = new Point(554, 10);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(224, 23);
            textBoxSearch.TabIndex = 12;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            // 
            // FormExportFile
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(168, 168, 168);
            ClientSize = new Size(841, 770);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormExportFile";
            Opacity = 0.97D;
            Padding = new Padding(1);
            Text = "FormInstallAPK";
            Load += FormAndroidExplorer_Load;
            panelMain.ResumeLayout(false);
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
        private ButtomRound buttonExportFile;
        private LabelNormalN labelNormaln1;
        private AntdUI.Tree tree1;
        private ButtomRound buttomRound1;
        private PictureBoxBrightN pictureBoxBrightn1;
        private TextBoxNoborberN textBoxSearch;
    }
}