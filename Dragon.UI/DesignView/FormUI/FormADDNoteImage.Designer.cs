
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormADDNoteImage
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormADDNoteImage));
            panelMain = new Panel();
            panelNormaln2 = new PanelNormalN();
            flowPanel1 = new AntdUI.FlowPanel();
            panelNormaln4 = new PanelNormalN();
            labelNormaln3 = new LabelNormalN();
            panelNormaln1 = new PanelNormalN();
            txtCheckNote = new TextBoxNoborberN();
            tableLayoutPanel2 = new TableLayoutPanel();
            labelThongBao = new LabelNormalN();
            panelNormaln3 = new PanelNormalN();
            labelNormaln2 = new LabelNormalN();
            panelRoundn1 = new PanelRoundN();
            labelNormaln5 = new LabelNormalN();
            ComboxControlMouseType = new ComboBoxCustomFlatN();
            groupBox2 = new GroupBox();
            radioDetectAndClickAtPoint = new RadioButton();
            radioDetectOnly = new RadioButton();
            radioDetectAndClick = new RadioButton();
            groupBox1 = new GroupBox();
            radioATXNode = new RadioButton();
            radioImageExiting = new RadioButton();
            radioTextExitingInImage = new RadioButton();
            lblAtxPoint = new LabelNormalN();
            txtAtxClickPoint = new TextBoxNoborberN();
            labelNormaln4 = new LabelNormalN();
            txtTenNote = new TextBoxNoborberN();
            buttonAddCheckExitNote = new ButtomRound();
            buttomTestCheckExiting = new ButtomRound();
            labelFileApk = new LabelNormalN();
            panel1 = new Panel();
            PictureBoxCloseForm = new PictureBoxBrightN();
            labelNormaln1 = new LabelNormalN();
            panelMain.SuspendLayout();
            panelNormaln2.SuspendLayout();
            panelNormaln4.SuspendLayout();
            panelNormaln1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panelNormaln3.SuspendLayout();
            panelRoundn1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(40, 40, 40);
            panelMain.Controls.Add(panelNormaln2);
            panelMain.Controls.Add(panelNormaln1);
            panelMain.Controls.Add(panelRoundn1);
            panelMain.Controls.Add(panel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(876, 748);
            panelMain.TabIndex = 0;
            // 
            // panelNormaln2
            // 
            panelNormaln2.BackColor = Color.FromArgb(40, 40, 40);
            panelNormaln2.BorderStyle = BorderStyle.FixedSingle;
            panelNormaln2.Controls.Add(flowPanel1);
            panelNormaln2.Controls.Add(panelNormaln4);
            panelNormaln2.Dock = DockStyle.Fill;
            panelNormaln2.ForeColor = Color.Black;
            panelNormaln2.Location = new Point(369, 40);
            panelNormaln2.Name = "panelNormaln2";
            panelNormaln2.Size = new Size(507, 512);
            panelNormaln2.TabIndex = 5;
            // 
            // flowPanel1
            // 
            flowPanel1.AutoScroll = true;
            flowPanel1.BackColor = Color.FromArgb(40, 40, 40);
            flowPanel1.Dock = DockStyle.Fill;
            flowPanel1.Location = new Point(0, 37);
            flowPanel1.Name = "flowPanel1";
            flowPanel1.Size = new Size(505, 473);
            flowPanel1.TabIndex = 2;
            // 
            // panelNormaln4
            // 
            panelNormaln4.BackColor = Color.FromArgb(40, 40, 40);
            panelNormaln4.BorderStyle = BorderStyle.FixedSingle;
            panelNormaln4.Controls.Add(labelNormaln3);
            panelNormaln4.Dock = DockStyle.Top;
            panelNormaln4.ForeColor = Color.Black;
            panelNormaln4.Location = new Point(0, 0);
            panelNormaln4.Name = "panelNormaln4";
            panelNormaln4.Size = new Size(505, 37);
            panelNormaln4.TabIndex = 1;
            // 
            // labelNormaln3
            // 
            labelNormaln3.AutoSize = true;
            labelNormaln3.BackColor = Color.Transparent;
            labelNormaln3.DG_BackColor = Color.Transparent;
            labelNormaln3.DG_IsBrightBack = false;
            labelNormaln3.DG_IsColorMode = false;
            labelNormaln3.DG_IsGrayImage = false;
            labelNormaln3.DG_LightenPercent = 80;
            labelNormaln3.DG_SVGImageColor = Color.Transparent;
            labelNormaln3.DG_SVGString = "";
            labelNormaln3.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            labelNormaln3.ForeColor = Color.White;
            labelNormaln3.Location = new Point(166, 7);
            labelNormaln3.Name = "labelNormaln3";
            labelNormaln3.Size = new Size(171, 20);
            labelNormaln3.TabIndex = 2;
            labelNormaln3.Text = "Create Image Template";
            // 
            // panelNormaln1
            // 
            panelNormaln1.BackColor = Color.FromArgb(40, 40, 40);
            panelNormaln1.BorderStyle = BorderStyle.FixedSingle;
            panelNormaln1.Controls.Add(txtCheckNote);
            panelNormaln1.Controls.Add(tableLayoutPanel2);
            panelNormaln1.Controls.Add(panelNormaln3);
            panelNormaln1.Dock = DockStyle.Left;
            panelNormaln1.ForeColor = Color.Black;
            panelNormaln1.Location = new Point(0, 40);
            panelNormaln1.Name = "panelNormaln1";
            panelNormaln1.Size = new Size(369, 512);
            panelNormaln1.TabIndex = 4;
            // 
            // txtCheckNote
            // 
            txtCheckNote.BackColor = Color.FromArgb(40, 40, 40);
            txtCheckNote.BorderStyle = BorderStyle.FixedSingle;
            txtCheckNote.DG_BorderThickness = 1;
            txtCheckNote.Dock = DockStyle.Fill;
            txtCheckNote.ForeColor = Color.White;
            txtCheckNote.Location = new Point(0, 37);
            txtCheckNote.Multiline = true;
            txtCheckNote.Name = "txtCheckNote";
            txtCheckNote.ScrollBars = ScrollBars.Horizontal;
            txtCheckNote.Size = new Size(367, 429);
            txtCheckNote.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(labelThongBao);
            tableLayoutPanel2.Dock = DockStyle.Bottom;
            tableLayoutPanel2.Location = new Point(0, 466);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(367, 44);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // labelThongBao
            // 
            labelThongBao.BackColor = Color.Transparent;
            labelThongBao.DG_BackColor = Color.Transparent;
            labelThongBao.DG_IsBrightBack = false;
            labelThongBao.DG_IsColorMode = false;
            labelThongBao.DG_IsGrayImage = false;
            labelThongBao.DG_LightenPercent = 80;
            labelThongBao.DG_SVGImageColor = Color.Transparent;
            labelThongBao.DG_SVGString = "";
            labelThongBao.Dock = DockStyle.Fill;
            labelThongBao.ForeColor = Color.White;
            labelThongBao.Location = new Point(3, 0);
            labelThongBao.Name = "labelThongBao";
            labelThongBao.Size = new Size(361, 44);
            labelThongBao.TabIndex = 0;
            labelThongBao.Text = "Text and Description cannot be null or empty";
            labelThongBao.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelNormaln3
            // 
            panelNormaln3.BackColor = Color.FromArgb(40, 40, 40);
            panelNormaln3.BorderStyle = BorderStyle.FixedSingle;
            panelNormaln3.Controls.Add(labelNormaln2);
            panelNormaln3.Dock = DockStyle.Top;
            panelNormaln3.ForeColor = Color.Black;
            panelNormaln3.Location = new Point(0, 0);
            panelNormaln3.Name = "panelNormaln3";
            panelNormaln3.Size = new Size(367, 37);
            panelNormaln3.TabIndex = 2;
            // 
            // labelNormaln2
            // 
            labelNormaln2.AutoSize = true;
            labelNormaln2.BackColor = Color.Transparent;
            labelNormaln2.DG_BackColor = Color.Transparent;
            labelNormaln2.DG_IsBrightBack = false;
            labelNormaln2.DG_IsColorMode = false;
            labelNormaln2.DG_IsGrayImage = false;
            labelNormaln2.DG_LightenPercent = 80;
            labelNormaln2.DG_SVGImageColor = Color.Transparent;
            labelNormaln2.DG_SVGString = "";
            labelNormaln2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            labelNormaln2.ForeColor = Color.White;
            labelNormaln2.Location = new Point(119, 7);
            labelNormaln2.Name = "labelNormaln2";
            labelNormaln2.Size = new Size(126, 20);
            labelNormaln2.TabIndex = 2;
            labelNormaln2.Text = "Create ATX Note";
            // 
            // panelRoundn1
            // 
            panelRoundn1.BackColor = Color.FromArgb(40, 40, 40);
            panelRoundn1.Controls.Add(labelNormaln5);
            panelRoundn1.Controls.Add(ComboxControlMouseType);
            panelRoundn1.Controls.Add(groupBox2);
            panelRoundn1.Controls.Add(groupBox1);
            panelRoundn1.Controls.Add(lblAtxPoint);
            panelRoundn1.Controls.Add(txtAtxClickPoint);
            panelRoundn1.Controls.Add(labelNormaln4);
            panelRoundn1.Controls.Add(txtTenNote);
            panelRoundn1.Controls.Add(buttonAddCheckExitNote);
            panelRoundn1.Controls.Add(buttomTestCheckExiting);
            panelRoundn1.Controls.Add(labelFileApk);
            panelRoundn1.Dock = DockStyle.Bottom;
            panelRoundn1.GD_Radius = 15F;
            panelRoundn1.Location = new Point(0, 552);
            panelRoundn1.Name = "panelRoundn1";
            panelRoundn1.Size = new Size(876, 196);
            panelRoundn1.TabIndex = 6;
            // 
            // labelNormaln5
            // 
            labelNormaln5.AutoSize = true;
            labelNormaln5.BackColor = Color.Transparent;
            labelNormaln5.DG_BackColor = Color.Transparent;
            labelNormaln5.DG_IsBrightBack = false;
            labelNormaln5.DG_IsColorMode = false;
            labelNormaln5.DG_IsGrayImage = false;
            labelNormaln5.DG_LightenPercent = 80;
            labelNormaln5.DG_SVGImageColor = Color.Transparent;
            labelNormaln5.DG_SVGString = "";
            labelNormaln5.ForeColor = Color.White;
            labelNormaln5.Location = new Point(202, 141);
            labelNormaln5.Name = "labelNormaln5";
            labelNormaln5.Size = new Size(99, 15);
            labelNormaln5.TabIndex = 10;
            labelNormaln5.Text = "Mouse Click Type";
            // 
            // ComboxControlMouseType
            // 
            ComboxControlMouseType.BackColor = Color.White;
            ComboxControlMouseType.DG_ArrowColor = Color.Black;
            ComboxControlMouseType.DG_ArrowHeight = 1.5F;
            ComboxControlMouseType.DG_ArrowWidth = 3F;
            ComboxControlMouseType.DG_BorderColor = Color.Black;
            ComboxControlMouseType.DG_BorderSize = 1;
            ComboxControlMouseType.DrawMode = DrawMode.OwnerDrawFixed;
            ComboxControlMouseType.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboxControlMouseType.FlatStyle = FlatStyle.Flat;
            ComboxControlMouseType.ForeColor = Color.Black;
            ComboxControlMouseType.FormattingEnabled = true;
            ComboxControlMouseType.Location = new Point(202, 159);
            ComboxControlMouseType.Name = "ComboxControlMouseType";
            ComboxControlMouseType.Size = new Size(224, 24);
            ComboxControlMouseType.TabIndex = 9;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(radioDetectAndClickAtPoint);
            groupBox2.Controls.Add(radioDetectOnly);
            groupBox2.Controls.Add(radioDetectAndClick);
            groupBox2.ForeColor = Color.White;
            groupBox2.Location = new Point(16, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(410, 56);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Action";
            // 
            // radioDetectAndClickAtPoint
            // 
            radioDetectAndClickAtPoint.AutoSize = true;
            radioDetectAndClickAtPoint.ForeColor = Color.White;
            radioDetectAndClickAtPoint.Location = new Point(231, 22);
            radioDetectAndClickAtPoint.Name = "radioDetectAndClickAtPoint";
            radioDetectAndClickAtPoint.Size = new Size(159, 19);
            radioDetectAndClickAtPoint.TabIndex = 2;
            radioDetectAndClickAtPoint.Text = "Detect And Click At Point";
            // 
            // radioDetectOnly
            // 
            radioDetectOnly.AutoSize = true;
            radioDetectOnly.ForeColor = Color.White;
            radioDetectOnly.Location = new Point(13, 22);
            radioDetectOnly.Name = "radioDetectOnly";
            radioDetectOnly.Size = new Size(87, 19);
            radioDetectOnly.TabIndex = 0;
            radioDetectOnly.Text = "Detect Only";
            // 
            // radioDetectAndClick
            // 
            radioDetectAndClick.AutoSize = true;
            radioDetectAndClick.ForeColor = Color.White;
            radioDetectAndClick.Location = new Point(112, 22);
            radioDetectAndClick.Name = "radioDetectAndClick";
            radioDetectAndClick.Size = new Size(113, 19);
            radioDetectAndClick.TabIndex = 1;
            radioDetectAndClick.Text = "Detect And Click";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioATXNode);
            groupBox1.Controls.Add(radioImageExiting);
            groupBox1.Controls.Add(radioTextExitingInImage);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(456, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(400, 56);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Mode";
            // 
            // radioATXNode
            // 
            radioATXNode.AutoSize = true;
            radioATXNode.ForeColor = Color.White;
            radioATXNode.Location = new Point(13, 22);
            radioATXNode.Name = "radioATXNode";
            radioATXNode.Size = new Size(75, 19);
            radioATXNode.TabIndex = 0;
            radioATXNode.Text = "Atx Node";
            // 
            // radioImageExiting
            // 
            radioImageExiting.AutoSize = true;
            radioImageExiting.ForeColor = Color.White;
            radioImageExiting.Location = new Point(101, 22);
            radioImageExiting.Name = "radioImageExiting";
            radioImageExiting.Size = new Size(131, 19);
            radioImageExiting.TabIndex = 1;
            radioImageExiting.Text = "Image Template Exit";
            // 
            // radioTextExitingInImage
            // 
            radioTextExitingInImage.AutoSize = true;
            radioTextExitingInImage.ForeColor = Color.White;
            radioTextExitingInImage.Location = new Point(236, 22);
            radioTextExitingInImage.Name = "radioTextExitingInImage";
            radioTextExitingInImage.Size = new Size(149, 19);
            radioTextExitingInImage.TabIndex = 2;
            radioTextExitingInImage.Text = "Image To Text Exit (Orc)";
            // 
            // lblAtxPoint
            // 
            lblAtxPoint.AutoSize = true;
            lblAtxPoint.BackColor = Color.Transparent;
            lblAtxPoint.DG_BackColor = Color.Transparent;
            lblAtxPoint.DG_IsBrightBack = false;
            lblAtxPoint.DG_IsColorMode = false;
            lblAtxPoint.DG_IsGrayImage = false;
            lblAtxPoint.DG_LightenPercent = 80;
            lblAtxPoint.DG_SVGImageColor = Color.Transparent;
            lblAtxPoint.DG_SVGString = "";
            lblAtxPoint.ForeColor = Color.White;
            lblAtxPoint.Location = new Point(13, 143);
            lblAtxPoint.Name = "lblAtxPoint";
            lblAtxPoint.Size = new Size(128, 15);
            lblAtxPoint.TabIndex = 2;
            lblAtxPoint.Text = "ATX Click Point % (X,Y)";
            // 
            // txtAtxClickPoint
            // 
            txtAtxClickPoint.BackColor = Color.FromArgb(50, 50, 50);
            txtAtxClickPoint.BorderStyle = BorderStyle.FixedSingle;
            txtAtxClickPoint.DG_BorderThickness = 1;
            txtAtxClickPoint.ForeColor = Color.White;
            txtAtxClickPoint.Location = new Point(14, 160);
            txtAtxClickPoint.Multiline = true;
            txtAtxClickPoint.Name = "txtAtxClickPoint";
            txtAtxClickPoint.Size = new Size(182, 23);
            txtAtxClickPoint.TabIndex = 3;
            // 
            // labelNormaln4
            // 
            labelNormaln4.AutoSize = true;
            labelNormaln4.BackColor = Color.Transparent;
            labelNormaln4.DG_BackColor = Color.Transparent;
            labelNormaln4.DG_IsBrightBack = false;
            labelNormaln4.DG_IsColorMode = false;
            labelNormaln4.DG_IsGrayImage = false;
            labelNormaln4.DG_LightenPercent = 80;
            labelNormaln4.DG_SVGImageColor = Color.Transparent;
            labelNormaln4.DG_SVGString = "";
            labelNormaln4.ForeColor = Color.White;
            labelNormaln4.Location = new Point(14, 88);
            labelNormaln4.Name = "labelNormaln4";
            labelNormaln4.Size = new Size(99, 15);
            labelNormaln4.TabIndex = 4;
            labelNormaln4.Text = "Name This Check";
            // 
            // txtTenNote
            // 
            txtTenNote.BackColor = Color.FromArgb(50, 50, 50);
            txtTenNote.BorderStyle = BorderStyle.FixedSingle;
            txtTenNote.DG_BorderThickness = 1;
            txtTenNote.ForeColor = Color.White;
            txtTenNote.Location = new Point(13, 106);
            txtTenNote.Multiline = true;
            txtTenNote.Name = "txtTenNote";
            txtTenNote.Size = new Size(413, 23);
            txtTenNote.TabIndex = 5;
            // 
            // buttonAddCheckExitNote
            // 
            buttonAddCheckExitNote.BackColor = Color.FromArgb(225, 225, 225);
            buttonAddCheckExitNote.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttonAddCheckExitNote.DG_ForceColor = Color.FromArgb(255, 70, 70);
            buttonAddCheckExitNote.DG_LightenPercent = 0.2F;
            buttonAddCheckExitNote.DG_SVGString = "";
            buttonAddCheckExitNote.FlatStyle = FlatStyle.Flat;
            buttonAddCheckExitNote.ForeColor = Color.Black;
            buttonAddCheckExitNote.GD_Radius = 15F;
            buttonAddCheckExitNote.Location = new Point(456, 154);
            buttonAddCheckExitNote.Name = "buttonAddCheckExitNote";
            buttonAddCheckExitNote.Size = new Size(182, 30);
            buttonAddCheckExitNote.TabIndex = 6;
            buttonAddCheckExitNote.TabStop = false;
            buttonAddCheckExitNote.Text = "Add New";
            buttonAddCheckExitNote.UseVisualStyleBackColor = false;
            buttonAddCheckExitNote.Click += buttonAddCheckExitNote_Click;
            // 
            // buttomTestCheckExiting
            // 
            buttomTestCheckExiting.BackColor = Color.FromArgb(225, 225, 225);
            buttomTestCheckExiting.DG_BackColor = Color.FromArgb(225, 225, 225);
            buttomTestCheckExiting.DG_ForceColor = Color.FromArgb(255, 70, 70);
            buttomTestCheckExiting.DG_LightenPercent = 0.2F;
            buttomTestCheckExiting.DG_SVGString = "";
            buttomTestCheckExiting.FlatStyle = FlatStyle.Flat;
            buttomTestCheckExiting.ForeColor = Color.Black;
            buttomTestCheckExiting.GD_Radius = 15F;
            buttomTestCheckExiting.Location = new Point(674, 154);
            buttomTestCheckExiting.Name = "buttomTestCheckExiting";
            buttomTestCheckExiting.Size = new Size(182, 30);
            buttomTestCheckExiting.TabIndex = 7;
            buttomTestCheckExiting.TabStop = false;
            buttomTestCheckExiting.Text = "Test Detect Now";
            buttomTestCheckExiting.UseVisualStyleBackColor = false;
            buttomTestCheckExiting.Click += buttomTestCheckExiting_Click;
            // 
            // labelFileApk
            // 
            labelFileApk.AutoSize = true;
            labelFileApk.BackColor = Color.Transparent;
            labelFileApk.DG_BackColor = Color.Transparent;
            labelFileApk.DG_IsBrightBack = false;
            labelFileApk.DG_IsColorMode = false;
            labelFileApk.DG_IsGrayImage = false;
            labelFileApk.DG_LightenPercent = 80;
            labelFileApk.DG_SVGImageColor = Color.Transparent;
            labelFileApk.DG_SVGString = "";
            labelFileApk.ForeColor = Color.Black;
            labelFileApk.Location = new Point(22, 14);
            labelFileApk.Name = "labelFileApk";
            labelFileApk.Size = new Size(0, 15);
            labelFileApk.TabIndex = 8;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(40, 40, 40);
            panel1.Controls.Add(PictureBoxCloseForm);
            panel1.Controls.Add(labelNormaln1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(876, 40);
            panel1.TabIndex = 7;
            // 
            // PictureBoxCloseForm
            // 
            PictureBoxCloseForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PictureBoxCloseForm.BackColor = Color.FromArgb(245, 245, 245);
            PictureBoxCloseForm.DG_ImageColor = Color.Black;
            PictureBoxCloseForm.DG_ImageSize = new Size(20, 20);
            PictureBoxCloseForm.DG_IsBackTransparent = false;
            PictureBoxCloseForm.DG_IsBrightBack = true;
            PictureBoxCloseForm.DG_IsGrayImage = false;
            PictureBoxCloseForm.DG_IsWhiteImage = false;
            PictureBoxCloseForm.DG_LightenPercent = 20;
            PictureBoxCloseForm.DG_SVGString = resources.GetString("PictureBoxCloseForm.DG_SVGString");
            PictureBoxCloseForm.DG_UseMode = UseMode.ThemeMode;
            PictureBoxCloseForm.ForeColor = Color.Black;
            PictureBoxCloseForm.Image = (Image)resources.GetObject("PictureBoxCloseForm.Image");
            PictureBoxCloseForm.Location = new Point(838, 10);
            PictureBoxCloseForm.Name = "PictureBoxCloseForm";
            PictureBoxCloseForm.Size = new Size(18, 18);
            PictureBoxCloseForm.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBoxCloseForm.TabIndex = 0;
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
            labelNormaln1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            labelNormaln1.ForeColor = Color.White;
            labelNormaln1.Location = new Point(3, 10);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(190, 20);
            labelNormaln1.TabIndex = 1;
            labelNormaln1.Text = "Create Vision Scan Tagert ";
            // 
            // FormADDNoteImage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            ClientSize = new Size(876, 748);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormADDNoteImage";
            StartPosition = FormStartPosition.Manual;
            panelMain.ResumeLayout(false);
            panelNormaln2.ResumeLayout(false);
            panelNormaln4.ResumeLayout(false);
            panelNormaln4.PerformLayout();
            panelNormaln1.ResumeLayout(false);
            panelNormaln1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            panelNormaln3.ResumeLayout(false);
            panelNormaln3.PerformLayout();
            panelRoundn1.ResumeLayout(false);
            panelRoundn1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private Panel panel1;
        private PictureBoxBrightN PictureBoxCloseForm;
        private LabelNormalN labelFileApk;
        private PanelRoundN panelRoundn1;
        private LabelNormalN labelNormaln1;
        private PanelNormalN panelNormaln1;
        private PanelNormalN panelNormaln2;
        private PanelNormalN panelNormaln4;
        private PanelNormalN panelNormaln3;
        private TextBoxNoborberN txtCheckNote;
        private AntdUI.FlowPanel flowPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private LabelNormalN labelThongBao;
        private ButtomRound buttonAddCheckExitNote;
        private RadioButton radioDetectOnly;
        private RadioButton radioDetectAndClick;
        private TextBoxNoborberN txtTenNote;
        private ButtomRound buttomTestCheckExiting;
        private LabelNormalN labelNormaln4;
        private RadioButton radioTextExitingInImage;
        private RadioButton radioATXNode;
        private RadioButton radioImageExiting;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private LabelNormalN lblAtxPoint;
        private TextBoxNoborberN txtAtxClickPoint;
        private RadioButton radioDetectAndClickAtPoint;
        private LabelNormalN labelNormaln3;
        private LabelNormalN labelNormaln2;
        private ComboBoxCustomFlatN ComboxControlMouseType;
        private LabelNormalN labelNormaln5;
    }
}
