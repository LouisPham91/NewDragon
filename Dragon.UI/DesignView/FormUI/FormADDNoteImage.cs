using AdvancedSharpAdbClient.Models;
using Dragon.ControlHelper.ScrcpyNet.DesignDevice;
using Dragon.Controller.Controller.DeviceControl.HATX.Core.Model;
using Dragon.Controller.DeviceControl.HATX;
using Dragon.Controller.GlobalControl.TaskDeviceManager.Runners;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Controller.TaskDeviceManager.Model.Vision;
using Dragon.Database.Models;
using Dragon.DesignView.ControlUI.Private;
using Dragon.DesignView.Public;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;


namespace Dragon.DesignView.FormUI
{
    public partial class FormADDNoteImage : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public VisionScanArgs VisionScanArgs = new VisionScanArgs();

        List<(string cropPercentSmaller, Bitmap bmSmaller)> ListImageCheck = new List<(string cropPercentSmaller, Bitmap bmSmaller)>();
        internal (string PercentSmaller, NodeObj nodeObj, Bitmap bmSmaller) NoteCrop;
        private float _roundRadius = 15f;
        private Color _borderColor = Color.FromArgb(168, 168, 168);
        public Size originalSize;
        public Point originalLocation;
        public bool isMaximized = false;
        FormAutoRecord? formAutoRecord = null;
        Phone phone;
        AtxDevice atx;
        DeviceData deviceData;

        public FormADDNoteImage((string PercentSmaller, NodeObj nodeObj, Bitmap bmSmaller) noteCrop, FormAutoRecord formAutoRecord, Phone phone, DeviceData deviceData, AtxDevice hAtx)
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |

                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            InitializeComponent();
            EnableFormDrag(panel1);
            EnableFormDrag(panelRoundn1);
            NoteCrop = noteCrop;
            this.phone = phone;
            this.atx = hAtx;
            this.deviceData = deviceData;

            this.formAutoRecord = formAutoRecord;

            
            LoadNodeObj();

            // mặc định
            radioATXNode.Checked = true;
            radioDetectOnly.Checked = true;
            //txtAtxClickPoint.Text = "50,50";
            this.FormClosing += FormADDNoteImage_FormClosing;
            radioDetectOnly.CheckedChanged += radioDetectOnly_CheckedChanged;
            radioDetectAndClick.CheckedChanged += radioDetectAndClick_CheckedChanged;
            radioDetectAndClickAtPoint.CheckedChanged += radioDetectAndClickAtPoint_CheckedChanged; // thêm
            ComboxControlMouseType.Items.AddRange(Enum.GetNames(typeof(ControlMode)));
            ComboxControlMouseType.SelectedItem = "ADBEvent";

            UpdateCheckType(VisionAction.DetectOnly);
            ApplyTheme();
        }

        public void ApplyTheme()
        {

            SuspendLayout();

            foreach (var ctrl in DesignHelper.GetAllControls(this))
            {

                //Debug.WriteLine("Control Name: " + ctrl.Name);
                if (ctrl is IThemeable themeable)
                    themeable.ApplyTheme();
            }

            
            BackColor = ThemeHelper.BackNormalFirst;
            ForeColor = ThemeHelper.ForeNormalFirst;

            panelMain.BackColor = ThemeHelper.BackNormalFirst;
            panelMain.ForeColor = ThemeHelper.ForeNormalFirst;
            txtCheckNote.ForeColor = ThemeHelper.ForeNormalFirst;
            flowPanel1.BackColor = ThemeHelper.ForeNormalFirst;

            panel1.BackColor = ThemeHelper.BackNormalFirst;
            panelRoundn1.BackColor = ThemeHelper.BackNormalFirst;
            panelNormaln3.BackColor = ThemeHelper.BackNormalFirst;
            txtCheckNote.BackColor = ThemeHelper.BackNormalFirst;
            panelNormaln4.BackColor = ThemeHelper.BackNormalFirst;

            if (ThemeHelper.CurrentMode == ThemeMode.Dark)
            {
                txtTenNote.BackColor = Color.FromArgb(50, 50, 50);
                txtAtxClickPoint.BackColor = Color.FromArgb(50, 50, 50);
            }
            else
            {
                txtTenNote.BackColor = Color.FromArgb(180, 180, 180);
                txtAtxClickPoint.BackColor = Color.FromArgb(220, 220, 220);
            }

            txtAtxClickPoint.ForeColor = Color.White;
            PictureBoxCloseForm.ApplyTheme();
            labelFileApk.ApplyTheme();

            

            ResumeLayout();
        }

        private void FormADDNoteImage_FormClosing(object? sender, FormClosingEventArgs e)
        {

        }

        #region Setting Begin
        private void EnableFormDrag(Control ctrl)
        {
            if (ctrl == null) return;
            ctrl.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, 0xA1, 2, 0);
                }
            };
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetRoundedRegion();
            SetPanelMainRoundedRegion();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            panelMain.Size = new Size(ClientSize.Width - 2, ClientSize.Height - 2);
            SetRoundedRegion();
            SetPanelMainRoundedRegion();
        }
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                this.BackColor = value;
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float DG_RoundRadius
        {
            get => _roundRadius;
            set
            {
                _roundRadius = value;
                SetRoundedRegion();
                SetPanelMainRoundedRegion();
            }
        }
        private void SetRoundedRegion()
        {
            using var path = GetRoundedRectanglePath(ClientRectangle, _roundRadius);
            var region = new Region(path);
            var old = Region;
            Region = region;
            old?.Dispose();
        }
        private void SetPanelMainRoundedRegion()
        {
            using var path = GetRoundedRectanglePath(panelMain.ClientRectangle, _roundRadius);
            var region = new Region(path);
            var old = panelMain.Region;
            panelMain.Region = region;
            old?.Dispose();
        }
        public static GraphicsPath GetRoundedRectanglePath(RectangleF rect, float radius)
        {
            float d = radius * 2f;
            var path = new GraphicsPath();
            if (radius <= 0) { path.AddRectangle(rect); return path; }
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
            path.CloseFigure();
            return path;
        }
        public void UpdateOriginalState()
        {
            originalSize = this.Size;
            originalLocation = this.Location;
        }
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            if (!isMaximized && this.WindowState == FormWindowState.Normal)
            {
                UpdateOriginalState();
            }
        }
        private void PictureBoxCloseForm_Click(object sender, EventArgs e) => this.Close();
        #endregion

        private void buttonAddCheckExitNote_Click(object sender, EventArgs e)
        {
            var tenNote = txtTenNote.Text.Trim();
            if (string.IsNullOrEmpty(tenNote))
            {
                MessageBox.Show("Please enter a name for the note.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cập nhật CheckMode
            if (radioATXNode.Checked) VisionScanArgs.VisionMode = VisionMode.ByAtxNode;
            else if (radioImageExiting.Checked) VisionScanArgs.VisionMode = VisionMode.ByImageTemplate;
            else if (radioTextExitingInImage.Checked) VisionScanArgs.VisionMode = VisionMode.ByOcrText;

            // Cập nhật ATX click point
            if (VisionScanArgs.ATXNode != null)
            {
                VisionScanArgs.ATXNode.SecialClickPoint = txtAtxClickPoint.Text.Trim();
            }

            VisionScanArgs.ControlMode = (ControlMode)Enum.Parse(typeof(ControlMode), ComboxControlMouseType.SelectedItem?.ToString() ?? "ADBEvent");



            // Tạo DLoop
            DLoop dLoop = new DLoop
            {
                Id = Guid.NewGuid(),
                Name = $"{VisionScanArgs.ATXNode} - {tenNote}",
                Type = NodeType.VisionScanArgs,

                Children = new List<DLoop>()
            };
            dLoop.SetArgs(VisionScanArgs);
            formAutoRecord?.AddTreeItem(dLoop);
            this.Close();
        }

        public void addImage((string cropPercentSmaller, Bitmap bmSmaller) StringAndImage)
        {
            if (ListImageCheck.Any(c => c.cropPercentSmaller == StringAndImage.cropPercentSmaller)) return;
            ListImageCheck.Add(StringAndImage);
            var userCheckNoteImage = new UserCheckNoteImage(this, StringAndImage);
            userCheckNoteImage.SetVisionActionFromParent(VisionScanArgs.VisionAction);
            flowPanel1.Controls.Add(userCheckNoteImage);
            VisionScanArgs.ImageOrcTexts.Add(userCheckNoteImage.imageOrcText);
            buttonAddCheckExitNote.Focus();
        }

        private void radioDetectOnly_CheckedChanged(object? sender, EventArgs e)
        {
            if (radioDetectOnly.Checked) UpdateCheckType(VisionAction.DetectOnly);
        }

        private void radioDetectAndClick_CheckedChanged(object? sender, EventArgs e)
        {
            if (radioDetectAndClick.Checked) UpdateCheckType(VisionAction.DetectAndClick);
        }
        private void radioDetectAndClickAtPoint_CheckedChanged(object? sender, EventArgs e)
        {
            if (radioDetectAndClickAtPoint.Checked) UpdateCheckType(VisionAction.ClickAtPoint);
        }

        public void UpdateCheckType(VisionAction visionAction)
        {
            VisionScanArgs.VisionAction = visionAction;
            VisionScanArgs.ATXNode.VisionAction = visionAction;

            foreach (var img in VisionScanArgs.ImageOrcTexts)
                img.VisionAction = visionAction;

            foreach (UserCheckNoteImage ui in flowPanel1.Controls.OfType<UserCheckNoteImage>())
            {
                ui.SetVisionActionFromParent(visionAction);
            }
        }
        private void SyncTextToFindFromATX()
        {
            if (string.IsNullOrWhiteSpace(VisionScanArgs.ATXNode?.Text)) return;

            string atxText = VisionScanArgs.ATXNode.Text;
            foreach (var img in VisionScanArgs.ImageOrcTexts)
            {
                if (string.IsNullOrWhiteSpace(img.TextToFind))
                    img.TextToFind = atxText;
            }
            foreach (UserCheckNoteImage ui in flowPanel1.Controls.OfType<UserCheckNoteImage>())
            {
                if (string.IsNullOrWhiteSpace(ui.txtTextToFind.Text))
                    ui.txtTextToFind.Text = atxText;
            }
        }
        public void UpdateCheckNoteImage(ImageOrcText checkExitImage)
        {
            var existing = VisionScanArgs.ImageOrcTexts.FirstOrDefault(c => c.Id == checkExitImage.Id);
            if (existing != null)
            {
                existing.IsActive = checkExitImage.IsActive;
                existing.VisionAction = checkExitImage.VisionAction;
                existing.Accuracy = checkExitImage.Accuracy;
                existing.SecialClickPoint = checkExitImage.SecialClickPoint;
                existing.CropRegion = checkExitImage.CropRegion;
                existing.TextToFind = checkExitImage.TextToFind;
                existing.ImageDataSrcpy = checkExitImage.ImageDataSrcpy;
            }
        }

        public void LoadNodeObj()
        {
            txtCheckNote.Text = FormatNodeInfo(NoteCrop.nodeObj);
            addImage((NoteCrop.PercentSmaller, NoteCrop.bmSmaller));

            bool hasText = !string.IsNullOrEmpty(NoteCrop.nodeObj.Text);
            bool hasDesc = !string.IsNullOrEmpty(NoteCrop.nodeObj.ContentDescription);

            if (!hasText && !hasDesc)
            {
                labelThongBao.Text = "Text and Description cannot be null or empty";
                labelThongBao.ForeColor = Color.DarkRed;
            }
            else
            {
                labelThongBao.Text = "This is good Note bro";
                labelThongBao.ForeColor = Color.Green;
            }

            VisionScanArgs.ATXNode = ConvertToHatxNote(NoteCrop.nodeObj, NoteCrop.bmSmaller.Width, NoteCrop.bmSmaller.Height);
            txtAtxClickPoint.Text = VisionScanArgs.ATXNode.SecialClickPoint ?? "";
            SyncTextToFindFromATX();
        }

        private ATXNode ConvertToHatxNote(NodeObj nodeObj, int screenWidth = 1920, int screenHeight = 1080)
        {
            var hatxNode = new ATXNode
            {
                ClassName = nodeObj.ClassName,
                Text = nodeObj.Text,
                ResourceName = nodeObj.ResourceName,
                PackageName = nodeObj.PackageName,
                ContentDescription = nodeObj.ContentDescription,
                VisionAction = VisionAction.DetectOnly,
            };

            var bounds = nodeObj?.Bound ?? NodeObj.Bounds.Empty;
            var center = bounds.CenterPos;

            static float ToPercent(int value, int total)
            {
                if (total <= 0) return 0f;
                return (float)Math.Round((double)value * 100.0 / total, 2);
            }

            hatxNode.PotisonClick = new PointCenter
            {
                X = ToPercent(center.X, screenWidth),
                Y = ToPercent(center.Y, screenHeight)
            };

            return hatxNode;
        }

        public void RemoveUserCheckNoteImage(UserCheckNoteImage userCheckNoteImage)
        {
            flowPanel1.Controls.Remove(userCheckNoteImage);
            VisionScanArgs.ImageOrcTexts.Remove(userCheckNoteImage.imageOrcText);
        }

        private string FormatNodeInfo(NodeObj node)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ClassName: {node.ClassName}");
            sb.AppendLine($"Text: {node.Text}");
            sb.AppendLine($"ResourceName: {node.ResourceName}");
            sb.AppendLine($"PackageName: {node.PackageName}");
            sb.AppendLine($"ContentDescription: {node.ContentDescription}");
            sb.AppendLine($"Bounds: Left={node.Bound.Left}, Top={node.Bound.Top}, Right={node.Bound.Right}, Bottom={node.Bound.Bottom}");
            sb.AppendLine($"VisibleBounds: Left={node.VisibleBounds.Left}, Top={node.VisibleBounds.Top}, Right={node.VisibleBounds.Right}, Bottom={node.VisibleBounds.Bottom}");
            sb.AppendLine($"Index: {node.Index}");
            sb.AppendLine($"ChildCount: {node.ChildCount}");
            sb.AppendLine($"Clickable: {node.Clickable}");
            sb.AppendLine($"Enabled: {node.Enabled}");
            sb.AppendLine($"Focusable: {node.Focusable}");
            sb.AppendLine($"Focused: {node.Focused}");
            sb.AppendLine($"Checkable: {node.Checkable}");
            sb.AppendLine($"Checked: {node.Checked}");
            sb.AppendLine($"LongClickable: {node.LongClickable}");
            sb.AppendLine($"Scrollable: {node.Scrollable}");
            sb.AppendLine($"Selected: {node.Selected}");
            sb.AppendLine($"Password: {node.Password}");
            sb.AppendLine($"VisibleToUser: {node.VisibleToUser}");
            sb.AppendLine($"DrawingOrder: {node.DrawingOrder}");
            sb.AppendLine($"Hint: {node.Hint}");
            sb.AppendLine($"Naf: {node.Naf}");
            return sb.ToString();
        }

        private async void buttomTestCheckExiting_Click(object sender, EventArgs e)
        {
            if (VisionScanArgs == null) return;

            // cập nhật mode trước khi test
            if (radioATXNode.Checked) VisionScanArgs.VisionMode = VisionMode.ByAtxNode;
            else if (radioImageExiting.Checked) VisionScanArgs.VisionMode = VisionMode.ByImageTemplate;
            else if (radioTextExitingInImage.Checked) VisionScanArgs.VisionMode = VisionMode.ByOcrText;

            VisionScanArgs.ATXNode.SecialClickPoint = txtAtxClickPoint.Text.Trim();

            var aDBClient = new AdvancedSharpAdbClient.AdbClient();
            PhoneSession session = new PhoneSession(phone, aDBClient, deviceData, atx);

            DLoopContext ctx = new DLoopContext
            {
                Session = session,
                Token = CancellationToken.None,
            };
            var result = await VisionScanRunner.GetPositionAndClick(ctx, VisionScanArgs);
            if (result == NodeResult.Ok())
            {
                MessageBox.Show($"Success Get Position", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Failed to Get Position", "Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }


    }
}
