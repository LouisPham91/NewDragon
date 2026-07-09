using AntdUI;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormAoaLoopEditor
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAoaLoopEditor));
            panelMain = new System.Windows.Forms.Panel();
            panelWorking = new PanelNormalN();
            panelLeft = new System.Windows.Forms.Panel();
            labelPhoneInfo = new LabelNormalN();
            labelPhoneModel = new LabelNormalN();
            selectPhoneModel = new Select();
            btnLoad = new ButtomFlatRound();
            btnNewTemplate = new ButtomFlatRound();
            labelSeparator1 = new System.Windows.Forms.Label();
            labelSelectedNode = new LabelNormalN();
            btnAddNode = new ButtomFlatRound();
            btnDeleteNode = new ButtomFlatRound();
            labelAddAction = new LabelNormalN();
            treeActions = new Tree();
            panelRight = new System.Windows.Forms.Panel();
            labelActionType = new LabelNormalN();
            selectActionType = new Select();
            panelParams = new System.Windows.Forms.Panel();
            panelRoundn1 = new PanelRoundN();
            btnSave = new ButtomFlatRound();
            btnTest = new ButtomFlatRound();
            labelFile = new LabelNormalN();
            panel1 = new System.Windows.Forms.Panel();
            textBoxSearch = new TextBoxNoborberN();
            pictureBoxBrightn1 = new PictureBoxBrightN();
            PictureBoxCloseForm = new PictureBoxBrightN();
            labelNormaln1 = new LabelNormalN();
            panelMain.SuspendLayout();
            panelWorking.SuspendLayout();
            panelLeft.SuspendLayout();
            panelRight.SuspendLayout();
            panelRoundn1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.WhiteSmoke;
            panelMain.Controls.Add(panelWorking);
            panelMain.Controls.Add(panelRoundn1);
            panelMain.Controls.Add(panel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(1, 1);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1283, 700);
            panelMain.TabIndex = 0;
            // 
            // panelWorking
            // 
            panelWorking.BackColor = Color.FromArgb(40, 40, 40);
            panelWorking.Controls.Add(panelLeft);
            panelWorking.Controls.Add(treeActions);
            panelWorking.Controls.Add(panelRight);
            panelWorking.Dock = DockStyle.Fill;
            panelWorking.ForeColor = Color.Black;
            panelWorking.Location = new Point(0, 40);
            panelWorking.Name = "panelWorking";
            panelWorking.Size = new Size(1283, 543);
            panelWorking.TabIndex = 1;
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.Transparent;
            panelLeft.Controls.Add(labelPhoneInfo);
            panelLeft.Controls.Add(labelPhoneModel);
            panelLeft.Controls.Add(selectPhoneModel);
            panelLeft.Controls.Add(btnLoad);
            panelLeft.Controls.Add(btnNewTemplate);
            panelLeft.Controls.Add(labelSeparator1);
            panelLeft.Controls.Add(labelSelectedNode);
            panelLeft.Controls.Add(btnAddNode);
            panelLeft.Controls.Add(btnDeleteNode);
            panelLeft.Controls.Add(labelAddAction);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(260, 543);
            panelLeft.TabIndex = 0;
            // 
            // labelPhoneInfo
            // 
            labelPhoneInfo.BackColor = Color.Transparent;
            labelPhoneInfo.DG_IsBrightBack = true;
            labelPhoneInfo.Font = new Font("Segoe UI", 9F);
            labelPhoneInfo.ForeColor = Color.White;
            labelPhoneInfo.Location = new Point(10, 10);
            labelPhoneInfo.Name = "labelPhoneInfo";
            labelPhoneInfo.Size = new Size(240, 40);
            labelPhoneInfo.TabIndex = 0;
            labelPhoneInfo.Text = "No phone selected";
            // 
            // labelPhoneModel
            // 
            labelPhoneModel.BackColor = Color.Transparent;
            labelPhoneModel.DG_IsBrightBack = true;
            labelPhoneModel.Font = new Font("Segoe UI", 8.5F);
            labelPhoneModel.ForeColor = Color.White;
            labelPhoneModel.Location = new Point(10, 55);
            labelPhoneModel.Name = "labelPhoneModel";
            labelPhoneModel.Size = new Size(100, 20);
            labelPhoneModel.TabIndex = 1;
            labelPhoneModel.Text = "Phone Model:";
            // 
            // selectPhoneModel
            // 
            selectPhoneModel.Font = new Font("Segoe UI", 9F);
            selectPhoneModel.List = true;
            selectPhoneModel.ListAutoWidth = true;
            selectPhoneModel.Location = new Point(10, 75);
            selectPhoneModel.Name = "selectPhoneModel";
            selectPhoneModel.PlaceholderText = "Select or type model...";
            selectPhoneModel.Size = new Size(240, 34);
            selectPhoneModel.TabIndex = 2;
            selectPhoneModel.SelectedValueChanged += SelectPhoneModel_Changed;
            // 
            // btnLoad
            // 
            btnLoad.BackColor = Color.White;
            btnLoad.DG_BackColor = Color.White;
            btnLoad.DG_ForeColor = Color.Black;
            btnLoad.DG_Padding = 6;
            btnLoad.FlatAppearance.BorderSize = 0;
            btnLoad.FlatStyle = FlatStyle.Flat;
            btnLoad.Font = new Font("Segoe UI", 9F);
            btnLoad.ForeColor = Color.Black;
            btnLoad.Location = new Point(10, 120);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(115, 30);
            btnLoad.TabIndex = 3;
            btnLoad.Text = "Load Loops";
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += BtnLoad_Click;
            // 
            // btnNewTemplate
            // 
            btnNewTemplate.BackColor = Color.White;
            btnNewTemplate.DG_BackColor = Color.White;
            btnNewTemplate.DG_ForeColor = Color.Black;
            btnNewTemplate.DG_Padding = 6;
            btnNewTemplate.FlatAppearance.BorderSize = 0;
            btnNewTemplate.FlatStyle = FlatStyle.Flat;
            btnNewTemplate.Font = new Font("Segoe UI", 9F);
            btnNewTemplate.ForeColor = Color.Black;
            btnNewTemplate.Location = new Point(135, 120);
            btnNewTemplate.Name = "btnNewTemplate";
            btnNewTemplate.Size = new Size(115, 30);
            btnNewTemplate.TabIndex = 4;
            btnNewTemplate.Text = "New Template";
            btnNewTemplate.UseVisualStyleBackColor = false;
            btnNewTemplate.Click += BtnNewTemplate_Click;
            // 
            // labelSeparator1
            // 
            labelSeparator1.BackColor = Color.Gray;
            labelSeparator1.Location = new Point(10, 160);
            labelSeparator1.Name = "labelSeparator1";
            labelSeparator1.Size = new Size(240, 2);
            labelSeparator1.TabIndex = 5;
            // 
            // labelSelectedNode
            // 
            labelSelectedNode.BackColor = Color.Transparent;
            labelSelectedNode.DG_IsBrightBack = true;
            labelSelectedNode.Font = new Font("Segoe UI", 8.5F);
            labelSelectedNode.ForeColor = Color.White;
            labelSelectedNode.Location = new Point(10, 170);
            labelSelectedNode.Name = "labelSelectedNode";
            labelSelectedNode.Size = new Size(240, 20);
            labelSelectedNode.TabIndex = 6;
            labelSelectedNode.Text = "Selected: None";
            // 
            // btnAddNode
            // 
            btnAddNode.BackColor = Color.White;
            btnAddNode.DG_BackColor = Color.White;
            btnAddNode.DG_ForeColor = Color.Black;
            btnAddNode.DG_Padding = 6;
            btnAddNode.FlatAppearance.BorderSize = 0;
            btnAddNode.FlatStyle = FlatStyle.Flat;
            btnAddNode.Font = new Font("Segoe UI", 9F);
            btnAddNode.ForeColor = Color.Black;
            btnAddNode.Location = new Point(10, 200);
            btnAddNode.Name = "btnAddNode";
            btnAddNode.Size = new Size(80, 30);
            btnAddNode.TabIndex = 7;
            btnAddNode.Text = "+ Add";
            btnAddNode.UseVisualStyleBackColor = false;
            btnAddNode.Click += BtnAddNode_Click;
            // 
            // btnDeleteNode
            // 
            btnDeleteNode.BackColor = Color.White;
            btnDeleteNode.DG_BackColor = Color.White;
            btnDeleteNode.DG_ForeColor = Color.Black;
            btnDeleteNode.DG_Padding = 6;
            btnDeleteNode.FlatAppearance.BorderSize = 0;
            btnDeleteNode.FlatStyle = FlatStyle.Flat;
            btnDeleteNode.Font = new Font("Segoe UI", 9F);
            btnDeleteNode.ForeColor = Color.Black;
            btnDeleteNode.Location = new Point(100, 200);
            btnDeleteNode.Name = "btnDeleteNode";
            btnDeleteNode.Size = new Size(80, 30);
            btnDeleteNode.TabIndex = 8;
            btnDeleteNode.Text = "- Delete";
            btnDeleteNode.UseVisualStyleBackColor = false;
            btnDeleteNode.Click += BtnDeleteNode_Click;
            // 
            // labelAddAction
            // 
            labelAddAction.BackColor = Color.Transparent;
            labelAddAction.DG_IsBrightBack = true;
            labelAddAction.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelAddAction.ForeColor = Color.Cyan;
            labelAddAction.Location = new Point(10, 235);
            labelAddAction.Name = "labelAddAction";
            labelAddAction.Size = new Size(240, 22);
            labelAddAction.TabIndex = 9;
            labelAddAction.Text = "Add: Click";
            // 
            // treeActions
            // 
            treeActions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            treeActions.Font = new Font("Segoe UI", 9.5F);
            treeActions.Location = new Point(270, 10);
            treeActions.Name = "treeActions";
            treeActions.Size = new Size(541, 523);
            treeActions.TabIndex = 1;
            treeActions.NodeMouseClick += TreeActions_NodeMouseClick;
            treeActions.Paint += TreeActions_Paint;
            treeActions.MouseDown += TreeActions_MouseDown;
            treeActions.MouseMove += TreeActions_MouseMove;
            treeActions.MouseUp += TreeActions_MouseUp;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.Transparent;
            panelRight.Controls.Add(labelActionType);
            panelRight.Controls.Add(selectActionType);
            panelRight.Controls.Add(panelParams);
            panelRight.Dock = DockStyle.Right;
            panelRight.Location = new Point(1013, 0);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(270, 543);
            panelRight.TabIndex = 2;
            // 
            // labelActionType
            // 
            labelActionType.BackColor = Color.Transparent;
            labelActionType.DG_IsBrightBack = true;
            labelActionType.Font = new Font("Segoe UI", 8.5F);
            labelActionType.ForeColor = Color.White;
            labelActionType.Location = new Point(10, 10);
            labelActionType.Name = "labelActionType";
            labelActionType.Size = new Size(100, 20);
            labelActionType.TabIndex = 0;
            labelActionType.Text = "Action Type:";
            // 
            // selectActionType
            // 
            selectActionType.Font = new Font("Segoe UI", 9F);
            selectActionType.List = true;
            selectActionType.ListAutoWidth = true;
            selectActionType.Location = new Point(10, 30);
            selectActionType.Name = "selectActionType";
            selectActionType.PlaceholderText = "Select action type...";
            selectActionType.Size = new Size(250, 34);
            selectActionType.TabIndex = 1;
            selectActionType.SelectedValueChanged += SelectActionType_Changed;
            // 
            // panelParams
            // 
            panelParams.AutoScroll = true;
            panelParams.BackColor = Color.Transparent;
            panelParams.Location = new Point(10, 75);
            panelParams.Name = "panelParams";
            panelParams.Size = new Size(250, 458);
            panelParams.TabIndex = 2;
            // 
            // panelRoundn1
            // 
            panelRoundn1.BackColor = Color.FromArgb(40, 40, 40);
            panelRoundn1.Controls.Add(btnSave);
            panelRoundn1.Controls.Add(btnTest);
            panelRoundn1.Controls.Add(labelFile);
            panelRoundn1.Dock = DockStyle.Bottom;
            panelRoundn1.ForeColor = Color.White;
            panelRoundn1.GD_Radius = 15F;
            panelRoundn1.Location = new Point(0, 583);
            panelRoundn1.Name = "panelRoundn1";
            panelRoundn1.Size = new Size(1283, 117);
            panelRoundn1.TabIndex = 2;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(76, 175, 80);
            btnSave.DG_BackColor = Color.FromArgb(76, 175, 80);
            btnSave.DG_ForeColor = Color.White;
            btnSave.DG_Padding = 6;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(20, 60);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 35);
            btnSave.TabIndex = 0;
            btnSave.Text = "💾 Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;
            // 
            // btnTest
            // 
            btnTest.BackColor = Color.FromArgb(33, 150, 243);
            btnTest.DG_BackColor = Color.FromArgb(33, 150, 243);
            btnTest.DG_ForeColor = Color.White;
            btnTest.DG_Padding = 6;
            btnTest.FlatAppearance.BorderSize = 0;
            btnTest.FlatStyle = FlatStyle.Flat;
            btnTest.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnTest.ForeColor = Color.White;
            btnTest.Location = new Point(160, 60);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(150, 35);
            btnTest.TabIndex = 1;
            btnTest.Text = "▶ Test Run";
            btnTest.UseVisualStyleBackColor = false;
            btnTest.Click += BtnTest_Click;
            // 
            // labelFile
            // 
            labelFile.BackColor = Color.Transparent;
            labelFile.Font = new Font("Segoe UI", 9F);
            labelFile.ForeColor = Color.White;
            labelFile.Location = new Point(20, 15);
            labelFile.Name = "labelFile";
            labelFile.Size = new Size(300, 25);
            labelFile.TabIndex = 2;
            labelFile.Text = "Ready";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(40, 40, 40);
            panel1.Controls.Add(textBoxSearch);
            panel1.Controls.Add(pictureBoxBrightn1);
            panel1.Controls.Add(PictureBoxCloseForm);
            panel1.Controls.Add(labelNormaln1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1283, 40);
            panel1.TabIndex = 0;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxSearch.BackColor = Color.FromArgb(40, 40, 40);
            textBoxSearch.BorderStyle = BorderStyle.FixedSingle;
            textBoxSearch.Font = new Font("Segoe UI", 9F);
            textBoxSearch.ForeColor = Color.White;
            textBoxSearch.Location = new Point(983, 10);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(224, 23);
            textBoxSearch.TabIndex = 12;
            // 
            // pictureBoxBrightn1
            // 
            pictureBoxBrightn1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBoxBrightn1.BackColor = Color.FromArgb(40, 40, 40);
            pictureBoxBrightn1.DG_ImageSize = new Size(18, 18);
            pictureBoxBrightn1.DG_IsBrightBack = false;
            pictureBoxBrightn1.DG_SVGString = resources.GetString("pictureBoxBrightn1.DG_SVGString");
            pictureBoxBrightn1.ForeColor = Color.White;
            pictureBoxBrightn1.Location = new Point(959, 12);
            pictureBoxBrightn1.Name = "pictureBoxBrightn1";
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
            PictureBoxCloseForm.DG_SVGString = resources.GetString("PictureBoxCloseForm.DG_SVGString");
            PictureBoxCloseForm.ForeColor = Color.White;
            PictureBoxCloseForm.Location = new Point(1251, 10);
            PictureBoxCloseForm.Name = "PictureBoxCloseForm";
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
            labelNormaln1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            labelNormaln1.ForeColor = Color.White;
            labelNormaln1.Location = new Point(22, 10);
            labelNormaln1.Name = "labelNormaln1";
            labelNormaln1.Size = new Size(123, 20);
            labelNormaln1.TabIndex = 0;
            labelNormaln1.Text = "OTG Loop Editor";
            // 
            // FormAoaLoopEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(168, 168, 168);
            ClientSize = new Size(1285, 702);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormAoaLoopEditor";
            Opacity = 0.97D;
            Padding = new Padding(1);
            Text = "AOA Loop Editor";
            Load += FormAoaLoopEditor_Load;
            panelMain.ResumeLayout(false);
            panelWorking.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            panelRight.ResumeLayout(false);
            panelRoundn1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).EndInit();
            ResumeLayout(false);
        }

        // Form base controls
        private System.Windows.Forms.Panel panelMain;
        private PanelNormalN panelWorking;
        private PanelRoundN panelRoundn1;
        private System.Windows.Forms.Panel panel1;
        private PictureBoxBrightN PictureBoxCloseForm;
        private LabelNormalN labelNormaln1;
        private TextBoxNoborberN textBoxSearch;
        private PictureBoxBrightN pictureBoxBrightn1;

        // Left panel
        private System.Windows.Forms.Panel panelLeft;
        private LabelNormalN labelPhoneInfo;
        private LabelNormalN labelPhoneModel;
        private AntdUI.Select selectPhoneModel;
        private ButtomFlatRound btnLoad;
        private ButtomFlatRound btnNewTemplate;
        private System.Windows.Forms.Label labelSeparator1;
        private LabelNormalN labelSelectedNode;
        private ButtomFlatRound btnAddNode;
        private ButtomFlatRound btnDeleteNode;

        // Center
        private AntdUI.Tree treeActions;

        // Right panel
        private System.Windows.Forms.Panel panelRight;
        private LabelNormalN labelActionType;
        private AntdUI.Select selectActionType;
        private System.Windows.Forms.Panel panelParams;

        // Bottom
        private ButtomFlatRound btnSave;
        private ButtomFlatRound btnTest;
        private LabelNormalN labelFile;

        private LabelNormalN labelAddAction;
    }
}