//namespace Dragon.UI.DesignView.FormUI
//{
//    partial class FormTestDGMode
//    {
//        private System.ComponentModel.IContainer components = null;

//        private Button btnRefresh;
//        private Button btnSelectFirst;
//        private Button btnSelectBySerial;
//        private Button btnTestMouse;
//        private Button btnTestKeyboard;
//        private Button btnTestFull;
//        private Button btnDispose;
//        private TextBox txtSerial;
//        private ListBox lstDevices;
//        private RichTextBox rtbLog;
//        private Label lblStatus;
//        private GroupBox grpDevices;
//        private GroupBox grpTest;
//        private GroupBox grpLog;

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        private void InitializeComponent()
//        {
//            grpDevices = new GroupBox();
//            buttonSericeADB = new Button();
//            buttonStopADB = new Button();
//            buttonRunADB = new Button();
//            btnRefresh = new Button();
//            btnSelectFirst = new Button();
//            lstDevices = new ListBox();
//            txtSerial = new TextBox();
//            btnSelectBySerial = new Button();
//            lblStatus = new Label();
//            grpTest = new GroupBox();
//            btnTestMouse = new Button();
//            btnTestKeyboard = new Button();
//            btnTestFull = new Button();
//            btnDispose = new Button();
//            grpLog = new GroupBox();
//            rtbLog = new RichTextBox();
//            grpDevices.SuspendLayout();
//            grpTest.SuspendLayout();
//            grpLog.SuspendLayout();
//            SuspendLayout();
//            // 
//            // grpDevices
//            // 
//            grpDevices.Controls.Add(buttonSericeADB);
//            grpDevices.Controls.Add(buttonStopADB);
//            grpDevices.Controls.Add(buttonRunADB);
//            grpDevices.Controls.Add(btnRefresh);
//            grpDevices.Controls.Add(btnSelectFirst);
//            grpDevices.Controls.Add(lstDevices);
//            grpDevices.Controls.Add(txtSerial);
//            grpDevices.Controls.Add(btnSelectBySerial);
//            grpDevices.Controls.Add(lblStatus);
//            grpDevices.Location = new Point(12, 12);
//            grpDevices.Name = "grpDevices";
//            grpDevices.Size = new Size(662, 310);
//            grpDevices.TabIndex = 0;
//            grpDevices.TabStop = false;
//            grpDevices.Text = "Thiết bị USB";
//            // 
//            // buttonSericeADB
//            // 
//            buttonSericeADB.Location = new Point(458, 22);
//            buttonSericeADB.Name = "buttonSericeADB";
//            buttonSericeADB.Size = new Size(98, 30);
//            buttonSericeADB.TabIndex = 8;
//            buttonSericeADB.Text = "⇆ Restore ADB";
//            buttonSericeADB.Click += buttonSericeADB_Click;
//            // 
//            // buttonStopADB
//            // 
//            buttonStopADB.Location = new Point(352, 22);
//            buttonStopADB.Name = "buttonStopADB";
//            buttonStopADB.Size = new Size(100, 30);
//            buttonStopADB.TabIndex = 7;
//            buttonStopADB.Text = "⏹ Stop AdB";
//            buttonStopADB.Click += buttonStopADB_Click;
//            // 
//            // buttonRunADB
//            // 
//            buttonRunADB.Location = new Point(246, 22);
//            buttonRunADB.Name = "buttonRunADB";
//            buttonRunADB.Size = new Size(100, 30);
//            buttonRunADB.TabIndex = 6;
//            buttonRunADB.Text = "▶ Run ADB";
//            buttonRunADB.Click += buttonRunADB_Click;
//            // 
//            // btnRefresh
//            // 
//            btnRefresh.Location = new Point(10, 22);
//            btnRefresh.Name = "btnRefresh";
//            btnRefresh.Size = new Size(100, 30);
//            btnRefresh.TabIndex = 0;
//            btnRefresh.Text = "🔄 Refresh";
//            btnRefresh.Click += BtnRefresh_Click;
//            // 
//            // btnSelectFirst
//            // 
//            btnSelectFirst.Location = new Point(120, 22);
//            btnSelectFirst.Name = "btnSelectFirst";
//            btnSelectFirst.Size = new Size(120, 30);
//            btnSelectFirst.TabIndex = 1;
//            btnSelectFirst.Text = "▶ Chọn máy bật";
//            btnSelectFirst.Click += BtnSelectFirstDevice_Click;
//            // 
//            // lstDevices
//            // 
//            lstDevices.IntegralHeight = false;
//            lstDevices.Location = new Point(10, 60);
//            lstDevices.Name = "lstDevices";
//            lstDevices.Size = new Size(641, 180);
//            lstDevices.TabIndex = 2;
//            lstDevices.SelectedIndexChanged += LstDevices_SelectedIndexChanged;
//            // 
//            // txtSerial
//            // 
//            txtSerial.Location = new Point(10, 248);
//            txtSerial.Name = "txtSerial";
//            txtSerial.PlaceholderText = "Nhập serial...";
//            txtSerial.Size = new Size(535, 23);
//            txtSerial.TabIndex = 3;
//            // 
//            // btnSelectBySerial
//            // 
//            btnSelectBySerial.Location = new Point(551, 248);
//            btnSelectBySerial.Name = "btnSelectBySerial";
//            btnSelectBySerial.Size = new Size(100, 25);
//            btnSelectBySerial.TabIndex = 4;
//            btnSelectBySerial.Text = "🔍 Tìm";
//            btnSelectBySerial.Click += BtnSelectBySerial_Click;
//            // 
//            // lblStatus
//            // 
//            lblStatus.ForeColor = Color.Gray;
//            lblStatus.Location = new Point(10, 280);
//            lblStatus.Name = "lblStatus";
//            lblStatus.Size = new Size(350, 20);
//            lblStatus.TabIndex = 5;
//            lblStatus.Text = "Trạng thái: Chưa chọn thiết bị";
//            // 
//            // grpTest
//            // 
//            grpTest.Controls.Add(btnTestMouse);
//            grpTest.Controls.Add(btnTestKeyboard);
//            grpTest.Controls.Add(btnTestFull);
//            grpTest.Controls.Add(btnDispose);
//            grpTest.Location = new Point(675, 12);
//            grpTest.Name = "grpTest";
//            grpTest.Size = new Size(180, 310);
//            grpTest.TabIndex = 1;
//            grpTest.TabStop = false;
//            grpTest.Text = "Điều khiển AOA";
//            // 
//            // btnTestMouse
//            // 
//            btnTestMouse.Location = new Point(10, 25);
//            btnTestMouse.Name = "btnTestMouse";
//            btnTestMouse.Size = new Size(160, 50);
//            btnTestMouse.TabIndex = 0;
//            btnTestMouse.Text = "🖱 Test Mouse";
//            btnTestMouse.Click += BtnTestMouse_Click;
//            // 
//            // btnTestKeyboard
//            // 
//            btnTestKeyboard.Location = new Point(10, 85);
//            btnTestKeyboard.Name = "btnTestKeyboard";
//            btnTestKeyboard.Size = new Size(160, 50);
//            btnTestKeyboard.TabIndex = 1;
//            btnTestKeyboard.Text = "⌨ Test Keyboard";
//            btnTestKeyboard.Click += BtnTestKeyboard_Click;
//            // 
//            // btnTestFull
//            // 
//            btnTestFull.BackColor = Color.LightGreen;
//            btnTestFull.Location = new Point(10, 145);
//            btnTestFull.Name = "btnTestFull";
//            btnTestFull.Size = new Size(160, 60);
//            btnTestFull.TabIndex = 2;
//            btnTestFull.Text = "🚀 Test FULL";
//            btnTestFull.UseVisualStyleBackColor = false;
//            btnTestFull.Click += BtnTestFull_Click;
//            // 
//            // btnDispose
//            // 
//            btnDispose.BackColor = Color.LightCoral;
//            btnDispose.Location = new Point(10, 215);
//            btnDispose.Name = "btnDispose";
//            btnDispose.Size = new Size(160, 50);
//            btnDispose.TabIndex = 3;
//            btnDispose.Text = "⏹ Đóng kết nối";
//            btnDispose.UseVisualStyleBackColor = false;
//            btnDispose.Click += BtnDispose_Click;
//            // 
//            // grpLog
//            // 
//            grpLog.Controls.Add(rtbLog);
//            grpLog.Location = new Point(12, 330);
//            grpLog.Name = "grpLog";
//            grpLog.Size = new Size(843, 312);
//            grpLog.TabIndex = 2;
//            grpLog.TabStop = false;
//            grpLog.Text = "Log";
//            // 
//            // rtbLog
//            // 
//            rtbLog.BackColor = Color.Black;
//            rtbLog.Font = new Font("Consolas", 9F);
//            rtbLog.ForeColor = Color.LimeGreen;
//            rtbLog.Location = new Point(10, 22);
//            rtbLog.Name = "rtbLog";
//            rtbLog.ReadOnly = true;
//            rtbLog.Size = new Size(823, 284);
//            rtbLog.TabIndex = 0;
//            rtbLog.Text = "";
//            // 
//            // FormTestDGMode
//            // 
//            AutoScaleDimensions = new SizeF(7F, 15F);
//            AutoScaleMode = AutoScaleMode.Font;
//            ClientSize = new Size(867, 654);
//            Controls.Add(grpDevices);
//            Controls.Add(grpTest);
//            Controls.Add(grpLog);
//            FormBorderStyle = FormBorderStyle.FixedSingle;
//            MaximizeBox = false;
//            Name = "FormTestDGMode";
//            Text = "Dragon AOA Test";
//            FormClosing += FormTestDGMode_FormClosing;
//            Load += FormTestDGMode_Load;
//            grpDevices.ResumeLayout(false);
//            grpDevices.PerformLayout();
//            grpTest.ResumeLayout(false);
//            grpLog.ResumeLayout(false);
//            ResumeLayout(false);
//        }

//        #endregion

//        private Button buttonRunADB;
//        private Button buttonStopADB;
//        private Button buttonSericeADB;
//    }
//}