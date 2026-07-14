using AntdUI;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    partial class FormOTGLoopEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOTGLoopEditor));
            panelMain = new System.Windows.Forms.Panel();
            panelWorking = new PanelNormalN();
            treeActions = new Tree();
            panelCaptureConnected = new PanelNormalN();
            selectCaptureConnected = new Select();
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
            panelScreenshot = new System.Windows.Forms.Panel();
            labelScreenshot = new LabelNormalN();
            btnCaptureScreenshot = new ButtomFlatRound();
            picScreenshot = new PictureBox();
            labelCoordinates = new LabelNormalN();
            panelRoundn1 = new PanelRoundN();
            btnSave = new ButtomFlatRound();
            btnTest = new ButtomFlatRound();
            labelFile = new LabelNormalN();
            panelRight = new System.Windows.Forms.Panel();
            panelParams = new AntdUI.Panel();
            labelActionType = new LabelNormalN();
            selectActionType = new Select();
            panel1 = new System.Windows.Forms.Panel();
            textBoxSearch = new TextBoxNoborberN();
            pictureBoxBrightn1 = new PictureBoxBrightN();
            PictureBoxCloseForm = new PictureBoxBrightN();
            labelNormaln1 = new LabelNormalN();
            panelMain.SuspendLayout();
            panelWorking.SuspendLayout();
            panelCaptureConnected.SuspendLayout();
            panelLeft.SuspendLayout();
            panelScreenshot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picScreenshot).BeginInit();
            panelRoundn1.SuspendLayout();
            panelRight.SuspendLayout();
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
            panelMain.Controls.Add(panelRight);
            panelMain.Controls.Add(panel1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(1, 1);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1283, 821);
            panelMain.TabIndex = 0;
            // 
            // panelWorking
            // 
            panelWorking.BackColor = Color.FromArgb(40, 40, 40);
            panelWorking.Controls.Add(treeActions);
            panelWorking.Controls.Add(panelCaptureConnected);
            panelWorking.Controls.Add(panelLeft);
            panelWorking.Dock = DockStyle.Fill;
            panelWorking.ForeColor = Color.Black;
            panelWorking.Location = new Point(0, 40);
            panelWorking.Name = "panelWorking";
            panelWorking.Size = new Size(994, 657);
            panelWorking.TabIndex = 5;
            // 
            // treeActions
            // 
            treeActions.Dock = DockStyle.Fill;
            treeActions.Font = new Font("Segoe UI", 9.5F);
            treeActions.Location = new Point(260, 37);
            treeActions.Name = "treeActions";
            treeActions.Size = new Size(734, 620);
            treeActions.TabIndex = 4;
            // 
            // panelCaptureConnected
            // 
            panelCaptureConnected.BackColor = Color.FromArgb(40, 40, 40);
            panelCaptureConnected.Controls.Add(selectCaptureConnected);
            panelCaptureConnected.Dock = DockStyle.Top;
            panelCaptureConnected.ForeColor = Color.Black;
            panelCaptureConnected.Location = new Point(260, 0);
            panelCaptureConnected.Name = "panelCaptureConnected";
            panelCaptureConnected.Size = new Size(734, 37);
            panelCaptureConnected.TabIndex = 3;
            // 
            // selectCaptureConnected
            // 
            selectCaptureConnected.Font = new Font("Segoe UI", 9F);
            selectCaptureConnected.List = true;
            selectCaptureConnected.ListAutoWidth = true;
            selectCaptureConnected.Location = new Point(5, 1);
            selectCaptureConnected.Name = "selectCaptureConnected";
            selectCaptureConnected.PlaceholderText = "Select or type model...";
            selectCaptureConnected.Size = new Size(364, 34);
            selectCaptureConnected.TabIndex = 3;
            selectCaptureConnected.SelectedValueChanged += SelectCaptureConnected_Changed;
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
            panelLeft.Controls.Add(panelScreenshot);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(260, 657);
            panelLeft.TabIndex = 0;
            // 
            // labelPhoneInfo
            // 
            labelPhoneInfo.BackColor = Color.Transparent;
            labelPhoneInfo.Font = new Font("Segoe UI", 9F);
            labelPhoneInfo.ForeColor = Color.White;
            labelPhoneInfo.Location = new Point(10, 10);
            labelPhoneInfo.Name = "labelPhoneInfo";
            labelPhoneInfo.Size = new Size(240, 54);
            labelPhoneInfo.TabIndex = 0;
            labelPhoneInfo.Text = "No phone selected";
            // 
            // labelPhoneModel
            // 
            labelPhoneModel.BackColor = Color.Transparent;
            labelPhoneModel.Font = new Font("Segoe UI", 8.5F);
            labelPhoneModel.ForeColor = Color.White;
            labelPhoneModel.Location = new Point(10, 66);
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
            selectPhoneModel.Location = new Point(10, 86);
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
            btnLoad.Location = new Point(10, 131);
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
            btnNewTemplate.Location = new Point(135, 131);
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
            labelSeparator1.Location = new Point(10, 171);
            labelSeparator1.Name = "labelSeparator1";
            labelSeparator1.Size = new Size(240, 2);
            labelSeparator1.TabIndex = 5;
            // 
            // labelSelectedNode
            // 
            labelSelectedNode.BackColor = Color.Transparent;
            labelSelectedNode.Font = new Font("Segoe UI", 8.5F);
            labelSelectedNode.ForeColor = Color.White;
            labelSelectedNode.Location = new Point(10, 181);
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
            btnAddNode.Location = new Point(10, 211);
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
            btnDeleteNode.Location = new Point(100, 211);
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
            labelAddAction.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelAddAction.ForeColor = Color.Cyan;
            labelAddAction.Location = new Point(10, 246);
            labelAddAction.Name = "labelAddAction";
            labelAddAction.Size = new Size(240, 22);
            labelAddAction.TabIndex = 9;
            labelAddAction.Text = "Add: Click";
            // 
            // panelScreenshot
            // 
            panelScreenshot.BackColor = Color.Transparent;
            panelScreenshot.Controls.Add(labelScreenshot);
            panelScreenshot.Controls.Add(btnCaptureScreenshot);
            panelScreenshot.Controls.Add(picScreenshot);
            panelScreenshot.Controls.Add(labelCoordinates);
            panelScreenshot.Location = new Point(5, 275);
            panelScreenshot.Name = "panelScreenshot";
            panelScreenshot.Size = new Size(250, 376);
            panelScreenshot.TabIndex = 10;
            // 
            // labelScreenshot
            // 
            labelScreenshot.BackColor = Color.Transparent;
            labelScreenshot.DG_IsBrightBack = true;
            labelScreenshot.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelScreenshot.ForeColor = Color.Cyan;
            labelScreenshot.Location = new Point(0, 0);
            labelScreenshot.Name = "labelScreenshot";
            labelScreenshot.Size = new Size(150, 22);
            labelScreenshot.TabIndex = 0;
            labelScreenshot.Text = "📸 Screenshot";
            // 
            // btnCaptureScreenshot
            // 
            btnCaptureScreenshot.BackColor = Color.White;
            btnCaptureScreenshot.DG_BackColor = Color.White;
            btnCaptureScreenshot.DG_ForeColor = Color.Black;
            btnCaptureScreenshot.DG_Padding = 6;
            btnCaptureScreenshot.FlatAppearance.BorderSize = 0;
            btnCaptureScreenshot.FlatStyle = FlatStyle.Flat;
            btnCaptureScreenshot.Font = new Font("Segoe UI", 8.5F);
            btnCaptureScreenshot.ForeColor = Color.Black;
            btnCaptureScreenshot.Location = new Point(155, 0);
            btnCaptureScreenshot.Name = "btnCaptureScreenshot";
            btnCaptureScreenshot.Size = new Size(95, 22);
            btnCaptureScreenshot.TabIndex = 1;
            btnCaptureScreenshot.Text = "📷 Capture";
            btnCaptureScreenshot.UseVisualStyleBackColor = false;
            btnCaptureScreenshot.Click += BtnCaptureScreenshot_Click;
            // 
            // picScreenshot
            // 
            picScreenshot.BackColor = Color.Black;
            picScreenshot.BorderStyle = BorderStyle.FixedSingle;
            picScreenshot.Location = new Point(0, 25);
            picScreenshot.Name = "picScreenshot";
            picScreenshot.Size = new Size(250, 310);
            picScreenshot.SizeMode = PictureBoxSizeMode.Zoom;
            picScreenshot.TabIndex = 2;
            picScreenshot.TabStop = false;
            picScreenshot.MouseClick += PicScreenshot_MouseClick;
            // 
            // labelCoordinates
            // 
            labelCoordinates.BackColor = Color.Transparent;
            labelCoordinates.DG_IsBrightBack = true;
            labelCoordinates.Font = new Font("Segoe UI", 8.5F);
            labelCoordinates.ForeColor = Color.LightGreen;
            labelCoordinates.Location = new Point(0, 351);
            labelCoordinates.Name = "labelCoordinates";
            labelCoordinates.Size = new Size(250, 25);
            labelCoordinates.TabIndex = 3;
            labelCoordinates.Text = "📋 Click image to get % coordinates";
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
            panelRoundn1.Location = new Point(0, 697);
            panelRoundn1.Name = "panelRoundn1";
            panelRoundn1.Size = new Size(994, 124);
            panelRoundn1.TabIndex = 3;
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
            btnSave.Location = new Point(20, 55);
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
            btnTest.Location = new Point(160, 55);
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
            // panelRight
            // 
            panelRight.BackColor = Color.FromArgb(40, 40, 40);
            panelRight.Controls.Add(panelParams);
            panelRight.Controls.Add(labelActionType);
            panelRight.Controls.Add(selectActionType);
            panelRight.Dock = DockStyle.Right;
            panelRight.Location = new Point(994, 40);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(289, 781);
            panelRight.TabIndex = 2;
            // 
            // panelParams
            // 
            panelParams.AutoScroll = true;
            panelParams.Back = Color.FromArgb(40, 40, 40);
            panelParams.Location = new Point(10, 75);
            panelParams.Name = "panelParams";
            panelParams.Size = new Size(270, 670);
            panelParams.TabIndex = 2;
            panelParams.Text = "panel2";
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
            selectActionType.Location = new Point(13, 30);
            selectActionType.Name = "selectActionType";
            selectActionType.PlaceholderText = "Select action type...";
            selectActionType.Size = new Size(262, 34);
            selectActionType.TabIndex = 1;
            selectActionType.SelectedValueChanged += SelectActionType_Changed;
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
            // FormOTGLoopEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(168, 168, 168);
            ClientSize = new Size(1285, 823);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormOTGLoopEditor";
            Opacity = 0.97D;
            Padding = new Padding(1);
            Text = "AOA Loop Editor";
            Load += FormAoaLoopEditor_Load;
            panelMain.ResumeLayout(false);
            panelWorking.ResumeLayout(false);
            panelCaptureConnected.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            panelScreenshot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picScreenshot).EndInit();
            panelRoundn1.ResumeLayout(false);
            panelRight.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBrightn1).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCloseForm).EndInit();
            ResumeLayout(false);
        }

        // Form base controls
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panel1;
        private PictureBoxBrightN PictureBoxCloseForm;
        private LabelNormalN labelNormaln1;
        private TextBoxNoborberN textBoxSearch;
        private PictureBoxBrightN pictureBoxBrightn1;

        // Right panel
        private System.Windows.Forms.Panel panelRight;
        private LabelNormalN labelActionType;
        private AntdUI.Select selectActionType;
        private PanelRoundN panelRoundn1;
        private ButtomFlatRound btnSave;
        private ButtomFlatRound btnTest;
        private LabelNormalN labelFile;
        private PanelNormalN panelWorking;
        private Tree treeActions;
        private PanelNormalN panelCaptureConnected;
        private Select selectCaptureConnected;
        private System.Windows.Forms.Panel panelLeft;
        private LabelNormalN labelPhoneInfo;
        private LabelNormalN labelPhoneModel;
        private Select selectPhoneModel;
        private ButtomFlatRound btnLoad;
        private ButtomFlatRound btnNewTemplate;
        private System.Windows.Forms.Label labelSeparator1;
        private LabelNormalN labelSelectedNode;
        private ButtomFlatRound btnAddNode;
        private ButtomFlatRound btnDeleteNode;
        private LabelNormalN labelAddAction;
        private AntdUI.Panel panelParams;

        // Thêm vào cuối file Designer.cs (trong class FormOTGLoopEditor):
        private System.Windows.Forms.Panel panelScreenshot;
        private LabelNormalN labelScreenshot;
        private ButtomFlatRound btnCaptureScreenshot;
        private System.Windows.Forms.PictureBox picScreenshot;
        private LabelNormalN labelCoordinates;
    }
}