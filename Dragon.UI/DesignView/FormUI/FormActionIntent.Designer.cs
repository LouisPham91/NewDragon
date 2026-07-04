using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormActionIntent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormActionIntent));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            label1 = new LabelN();
            tableLayoutPanel1 = new TableLayoutPanel();
            buttonShowScreen = new Button();
            comboxPhoneList = new ComboBoxCustomFlatN();
            label2 = new LabelN();
            buttonLoadAllApp = new Button();
            buttonLoadAllUserApp = new Button();
            txtSearch = new TextBoxNoborberN();
            comboxPackageList = new ComboBoxCustomFlatN();
            labelSearch = new LabelNormalN();
            panel1 = new PanelNormalN();
            dataGridView2 = new AotSafeDataGridView();
            panelTitleAdbCommand = new PanelNormalN();
            tableLayoutPanel3 = new TableLayoutPanel();
            button1 = new Button();
            comboxListPackageCommands = new ComboBoxCustomFlatN();
            buttonSaveCommand = new Button();
            label5 = new LabelN();
            buttonLoadCommand = new Button();
            buttonTestAction = new Button();
            txtDumpy = new TextBox();
            dataGridView1 = new AotSafeDataGridView();
            tableLayoutPanel2 = new TableLayoutPanel();
            label4 = new LabelN();
            label3 = new LabelN();
            panel3 = new PanelNormalN();
            panel5 = new PanelNormalN();
            panelChangeSize = new PanelNormalN();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).BeginInit();
            panelBottomBordern1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            panelTitleAdbCommand.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            panel3.SuspendLayout();
            panel5.SuspendLayout();
            panelChangeSize.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(40, 40, 40);
            panelMain.Controls.Add(panel3);
            panelMain.Controls.Add(tableLayoutPanel2);
            panelMain.Controls.Add(tableLayoutPanel1);
            panelMain.Location = new Point(1, 1);
            panelMain.Size = new Size(1390, 998);
            panelMain.Controls.SetChildIndex(panelBottomBordern1, 0);
            panelMain.Controls.SetChildIndex(tableLayoutPanel1, 0);
            panelMain.Controls.SetChildIndex(tableLayoutPanel2, 0);
            panelMain.Controls.SetChildIndex(panel3, 0);
            // 
            // labelTitle
            // 
            labelTitle.ForeColor = SystemColors.ControlText;
            labelTitle.Size = new Size(172, 17);
            labelTitle.Text = "Adb Inten Action Deeplink";
            // 
            // panelBottomBordern1
            // 
            panelBottomBordern1.Size = new Size(1390, 34);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(40, 40, 40);
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(368, 0);
            label1.Name = "label1";
            label1.Size = new Size(111, 30);
            label1.TabIndex = 1;
            label1.Text = "Package List :";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(40, 40, 40);
            tableLayoutPanel1.ColumnCount = 10;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 117F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 242F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 148F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 147F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 51F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 301F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 8F));
            tableLayoutPanel1.Controls.Add(buttonShowScreen, 2, 0);
            tableLayoutPanel1.Controls.Add(comboxPhoneList, 1, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(buttonLoadAllApp, 6, 0);
            tableLayoutPanel1.Controls.Add(buttonLoadAllUserApp, 5, 0);
            tableLayoutPanel1.Controls.Add(label1, 3, 0);
            tableLayoutPanel1.Controls.Add(txtSearch, 8, 0);
            tableLayoutPanel1.Controls.Add(comboxPackageList, 4, 0);
            tableLayoutPanel1.Controls.Add(labelSearch, 7, 0);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 34);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1390, 30);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // buttonShowScreen
            // 
            buttonShowScreen.BackColor = Color.FromArgb(40, 40, 40);
            buttonShowScreen.FlatStyle = FlatStyle.Popup;
            buttonShowScreen.ForeColor = Color.White;
            buttonShowScreen.Location = new Point(223, 3);
            buttonShowScreen.Name = "buttonShowScreen";
            buttonShowScreen.Size = new Size(139, 24);
            buttonShowScreen.TabIndex = 2;
            buttonShowScreen.Text = "Show Phone Screen";
            buttonShowScreen.UseVisualStyleBackColor = false;
            buttonShowScreen.Click += buttonShowScreen_Click;
            // 
            // comboxPhoneList
            // 
            comboxPhoneList.BackColor = Color.FromArgb(40, 40, 40);
            comboxPhoneList.DG_ArrowColor = Color.Black;
            comboxPhoneList.DG_BorderColor = Color.FromArgb(61, 61, 61);
            comboxPhoneList.DrawMode = DrawMode.OwnerDrawFixed;
            comboxPhoneList.DropDownStyle = ComboBoxStyle.DropDownList;
            comboxPhoneList.FlatStyle = FlatStyle.Popup;
            comboxPhoneList.ForeColor = Color.White;
            comboxPhoneList.FormattingEnabled = true;
            comboxPhoneList.Location = new Point(103, 3);
            comboxPhoneList.Name = "comboxPhoneList";
            comboxPhoneList.Size = new Size(114, 24);
            comboxPhoneList.TabIndex = 4;
            comboxPhoneList.SelectedIndexChanged += comboxPhoneList_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(40, 40, 40);
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(3, 0);
            label2.Name = "label2";
            label2.Size = new Size(94, 30);
            label2.TabIndex = 4;
            label2.Text = "Phone List :";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // buttonLoadAllApp
            // 
            buttonLoadAllApp.BackColor = Color.FromArgb(40, 40, 40);
            buttonLoadAllApp.Dock = DockStyle.Fill;
            buttonLoadAllApp.FlatStyle = FlatStyle.Popup;
            buttonLoadAllApp.ForeColor = Color.White;
            buttonLoadAllApp.Location = new Point(875, 3);
            buttonLoadAllApp.Name = "buttonLoadAllApp";
            buttonLoadAllApp.Size = new Size(141, 24);
            buttonLoadAllApp.TabIndex = 9;
            buttonLoadAllApp.Text = "Load All App";
            buttonLoadAllApp.UseVisualStyleBackColor = false;
            buttonLoadAllApp.Click += buttonLoadAllApp_Click;
            // 
            // buttonLoadAllUserApp
            // 
            buttonLoadAllUserApp.BackColor = Color.FromArgb(40, 40, 40);
            buttonLoadAllUserApp.Dock = DockStyle.Fill;
            buttonLoadAllUserApp.FlatStyle = FlatStyle.Popup;
            buttonLoadAllUserApp.ForeColor = Color.White;
            buttonLoadAllUserApp.Location = new Point(727, 3);
            buttonLoadAllUserApp.Name = "buttonLoadAllUserApp";
            buttonLoadAllUserApp.Size = new Size(142, 24);
            buttonLoadAllUserApp.TabIndex = 8;
            buttonLoadAllUserApp.Text = "Load All User App";
            buttonLoadAllUserApp.UseVisualStyleBackColor = false;
            buttonLoadAllUserApp.Click += buttonLoadAllUserApp_Click;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.FromArgb(40, 40, 40);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Dock = DockStyle.Fill;
            txtSearch.Font = new Font("Segoe UI", 12F);
            txtSearch.ForeColor = Color.White;
            txtSearch.Location = new Point(1073, 3);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(295, 29);
            txtSearch.TabIndex = 12;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // comboxPackageList
            // 
            comboxPackageList.BackColor = Color.FromArgb(40, 40, 40);
            comboxPackageList.DG_ArrowColor = Color.Black;
            comboxPackageList.DG_BorderColor = Color.FromArgb(61, 61, 61);
            comboxPackageList.Dock = DockStyle.Fill;
            comboxPackageList.DrawMode = DrawMode.OwnerDrawFixed;
            comboxPackageList.DropDownStyle = ComboBoxStyle.DropDownList;
            comboxPackageList.FlatStyle = FlatStyle.Flat;
            comboxPackageList.ForeColor = Color.White;
            comboxPackageList.FormattingEnabled = true;
            comboxPackageList.Location = new Point(485, 3);
            comboxPackageList.Name = "comboxPackageList";
            comboxPackageList.Size = new Size(236, 24);
            comboxPackageList.TabIndex = 13;
            comboxPackageList.SelectedIndexChanged += comboxPackageList_SelectedIndexChanged;
            comboxPackageList.KeyUp += comboxPackageList_KeyUp;
            // 
            // labelSearch
            // 
            labelSearch.AutoSize = true;
            labelSearch.BackColor = Color.Transparent;
            labelSearch.DG_SVGImageColor = Color.White;
            labelSearch.DG_SVGString = resources.GetString("labelSearch.DG_SVGString");
            labelSearch.Dock = DockStyle.Fill;
            labelSearch.ForeColor = Color.White;
            labelSearch.Image = (Image)resources.GetObject("labelSearch.Image");
            labelSearch.Location = new Point(1022, 0);
            labelSearch.Name = "labelSearch";
            labelSearch.Size = new Size(45, 30);
            labelSearch.TabIndex = 14;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(40, 40, 40);
            panel1.Controls.Add(dataGridView2);
            panel1.Controls.Add(panelTitleAdbCommand);
            panel1.Dock = DockStyle.Fill;
            panel1.ForeColor = Color.White;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1238, 467);
            panel1.TabIndex = 4;
            // 
            // dataGridView2
            // 
            dataGridView2.BackgroundColor = Color.FromArgb(40, 40, 40);
            dataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView2.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.GridColor = Color.Gray;
            dataGridView2.Location = new Point(0, 32);
            dataGridView2.Name = "dataGridView2";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.Size = new Size(1238, 435);
            dataGridView2.TabIndex = 10;
            // 
            // panelTitleAdbCommand
            // 
            panelTitleAdbCommand.BackColor = Color.FromArgb(40, 40, 40);
            panelTitleAdbCommand.BorderStyle = BorderStyle.FixedSingle;
            panelTitleAdbCommand.Controls.Add(tableLayoutPanel3);
            panelTitleAdbCommand.Dock = DockStyle.Top;
            panelTitleAdbCommand.ForeColor = Color.White;
            panelTitleAdbCommand.Location = new Point(0, 0);
            panelTitleAdbCommand.Name = "panelTitleAdbCommand";
            panelTitleAdbCommand.Size = new Size(1238, 32);
            panelTitleAdbCommand.TabIndex = 9;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.FromArgb(40, 40, 40);
            tableLayoutPanel3.ColumnCount = 9;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 169F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 159F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 246F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 53F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 13F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 8F));
            tableLayoutPanel3.Controls.Add(button1, 3, 0);
            tableLayoutPanel3.Controls.Add(comboxListPackageCommands, 1, 0);
            tableLayoutPanel3.Controls.Add(buttonSaveCommand, 2, 0);
            tableLayoutPanel3.Controls.Add(label5, 0, 0);
            tableLayoutPanel3.Controls.Add(buttonLoadCommand, 5, 0);
            tableLayoutPanel3.Controls.Add(buttonTestAction, 4, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(1236, 30);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.FlatStyle = FlatStyle.Popup;
            button1.ForeColor = Color.White;
            button1.Location = new Point(592, 3);
            button1.Name = "button1";
            button1.Size = new Size(159, 24);
            button1.TabIndex = 8;
            button1.Text = "Delete Command (F4)";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // comboxListPackageCommands
            // 
            comboxListPackageCommands.BackColor = Color.FromArgb(40, 40, 40);
            comboxListPackageCommands.DG_ArrowColor = Color.Black;
            comboxListPackageCommands.DG_BorderColor = Color.FromArgb(61, 61, 61);
            comboxListPackageCommands.Dock = DockStyle.Fill;
            comboxListPackageCommands.DrawMode = DrawMode.OwnerDrawFixed;
            comboxListPackageCommands.DropDownStyle = ComboBoxStyle.DropDownList;
            comboxListPackageCommands.FlatStyle = FlatStyle.Popup;
            comboxListPackageCommands.ForeColor = Color.White;
            comboxListPackageCommands.FormattingEnabled = true;
            comboxListPackageCommands.Location = new Point(223, 3);
            comboxListPackageCommands.Name = "comboxListPackageCommands";
            comboxListPackageCommands.Size = new Size(194, 24);
            comboxListPackageCommands.TabIndex = 6;
            comboxListPackageCommands.SelectedIndexChanged += comboxListPackageCommands_SelectedIndexChanged;
            comboxListPackageCommands.KeyUp += comboxListPackageCommands_KeyUp;
            // 
            // buttonSaveCommand
            // 
            buttonSaveCommand.Dock = DockStyle.Fill;
            buttonSaveCommand.FlatStyle = FlatStyle.Popup;
            buttonSaveCommand.ForeColor = Color.White;
            buttonSaveCommand.Location = new Point(423, 3);
            buttonSaveCommand.Name = "buttonSaveCommand";
            buttonSaveCommand.Size = new Size(163, 24);
            buttonSaveCommand.TabIndex = 0;
            buttonSaveCommand.Text = "Save Command (F1)";
            buttonSaveCommand.UseVisualStyleBackColor = true;
            buttonSaveCommand.Click += buttonSaveCommand_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(40, 40, 40);
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(3, 0);
            label5.Name = "label5";
            label5.Size = new Size(214, 30);
            label5.TabIndex = 5;
            label5.Text = "List Pakage Save Commands:";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // buttonLoadCommand
            // 
            buttonLoadCommand.Dock = DockStyle.Fill;
            buttonLoadCommand.FlatStyle = FlatStyle.Popup;
            buttonLoadCommand.ForeColor = Color.White;
            buttonLoadCommand.Location = new Point(916, 3);
            buttonLoadCommand.Name = "buttonLoadCommand";
            buttonLoadCommand.Size = new Size(240, 24);
            buttonLoadCommand.TabIndex = 7;
            buttonLoadCommand.Text = "Load All Comand Package Selected (F3)";
            buttonLoadCommand.UseVisualStyleBackColor = true;
            buttonLoadCommand.Click += buttonLoadCommand_Click;
            // 
            // buttonTestAction
            // 
            buttonTestAction.FlatStyle = FlatStyle.Popup;
            buttonTestAction.ForeColor = Color.White;
            buttonTestAction.Location = new Point(757, 3);
            buttonTestAction.Name = "buttonTestAction";
            buttonTestAction.Size = new Size(153, 24);
            buttonTestAction.TabIndex = 1;
            buttonTestAction.Text = "Test Action (F2)";
            buttonTestAction.UseVisualStyleBackColor = true;
            buttonTestAction.Click += buttonTestAction_Click;
            // 
            // txtDumpy
            // 
            txtDumpy.BackColor = Color.FromArgb(40, 40, 40);
            txtDumpy.BorderStyle = BorderStyle.FixedSingle;
            txtDumpy.Dock = DockStyle.Left;
            txtDumpy.Location = new Point(0, 0);
            txtDumpy.Multiline = true;
            txtDumpy.Name = "txtDumpy";
            txtDumpy.ScrollBars = ScrollBars.Vertical;
            txtDumpy.Size = new Size(341, 465);
            txtDumpy.TabIndex = 7;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.FromArgb(40, 40, 40);
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle5.SelectionForeColor = Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.GridColor = Color.Gray;
            dataGridView1.Location = new Point(341, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle6.SelectionForeColor = Color.White;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Size = new Size(897, 465);
            dataGridView1.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(label4, 0, 1);
            tableLayoutPanel2.Controls.Add(label3, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Left;
            tableLayoutPanel2.Location = new Point(0, 64);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(150, 934);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(40, 40, 40);
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 23F);
            label4.ForeColor = Color.White;
            label4.Location = new Point(4, 467);
            label4.Name = "label4";
            label4.Size = new Size(142, 466);
            label4.TabIndex = 1;
            label4.Text = "Adb Comand List For Action View";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(40, 40, 40);
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 23F);
            label3.ForeColor = Color.White;
            label3.Location = new Point(4, 1);
            label3.Name = "label3";
            label3.Size = new Size(142, 465);
            label3.TabIndex = 0;
            label3.Text = "Auto General Intent Action";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(40, 40, 40);
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(panel5);
            panel3.Controls.Add(panelChangeSize);
            panel3.Dock = DockStyle.Fill;
            panel3.ForeColor = Color.White;
            panel3.Location = new Point(150, 64);
            panel3.Name = "panel3";
            panel3.Size = new Size(1240, 934);
            panel3.TabIndex = 9;
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(40, 40, 40);
            panel5.Controls.Add(panel1);
            panel5.Dock = DockStyle.Fill;
            panel5.ForeColor = Color.White;
            panel5.Location = new Point(0, 465);
            panel5.Name = "panel5";
            panel5.Size = new Size(1238, 467);
            panel5.TabIndex = 1;
            // 
            // panelChangeSize
            // 
            panelChangeSize.BackColor = Color.FromArgb(40, 40, 40);
            panelChangeSize.Controls.Add(dataGridView1);
            panelChangeSize.Controls.Add(txtDumpy);
            panelChangeSize.Dock = DockStyle.Top;
            panelChangeSize.ForeColor = Color.White;
            panelChangeSize.Location = new Point(0, 0);
            panelChangeSize.Name = "panelChangeSize";
            panelChangeSize.Size = new Size(1238, 465);
            panelChangeSize.TabIndex = 0;
            // 
            // FormActionIntent
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1392, 1000);
            Name = "FormActionIntent";
            Padding = new Padding(1);
            Text = "FormSettingIntent";
            Load += FormActionIntent_Load;
            panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxIcon).EndInit();
            panelBottomBordern1.ResumeLayout(false);
            panelBottomBordern1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            panelTitleAdbCommand.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panelChangeSize.ResumeLayout(false);
            panelChangeSize.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private ComboBoxCustomFlatN comboxPhoneList;
        private Button buttonLoadAllUserApp;
        private Button buttonLoadAllApp;
        private AotSafeDataGridView dataGridView2;
        private Button buttonSaveCommand;
        private AotSafeDataGridView dataGridView1;
        private TextBox txtDumpy;
        private Button buttonTestAction;
        private Button buttonShowScreen;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private ComboBoxCustomFlatN comboxListPackageCommands;
        private Button buttonLoadCommand;
        private Button button1;
        private ComboBoxCustomFlatN comboxPackageList;
        private TextBoxNoborberN txtSearch;
        private LabelNormalN labelSearch;
        private LabelN label1;
        private LabelN label2;
        private LabelN label4;
        private LabelN label3;
        private LabelN label5;
        private PanelNormalN panel1;
        private PanelNormalN panelTitleAdbCommand;
        private PanelNormalN panel3;
        private PanelNormalN panel5;
        private PanelNormalN panelChangeSize;
    }
}