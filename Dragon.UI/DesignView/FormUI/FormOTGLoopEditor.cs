using AntdUI;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl;
using Dragon.Controller.DeviceControl.Orc;
using Dragon.Controller.DeviceControl.OTG;
using Dragon.Controller.DeviceControl.OTG.Loop;
using Dragon.Controller.DeviceControl.OTG.Model;
using Dragon.Database.Models;
using Dragon.DesignView.Public;
using System.Runtime.InteropServices;

namespace Dragon.DesignView.FormUI
{
    public partial class FormOTGLoopEditor : Form
    {
        private Phone? _phone;
        private AoaLoop? _currentLoop;
        private List<AoaLoop> _allLoops = new();
        private AntdUI.TreeItem? _selectedNode;
        private bool _isCaptureConnected = false; // Mặc định KHÔNG connect

        public FormOTGLoopEditor(Phone? phone = null)
        {
            InitializeComponent();
            _phone = phone;

            treeActions.NodeMouseClick += TreeActions_NodeMouseClick;
            treeActions.MouseDown += TreeActions_MouseDown;
            treeActions.MouseMove += TreeActions_MouseMove;
            treeActions.MouseUp += TreeActions_MouseUp;
            treeActions.Paint += TreeActions_Paint;

            // Khởi tạo drag timer
            dragTimer = new System.Windows.Forms.Timer();
            dragTimer.Interval = 400; // Giữ chuột 400ms mới bắt đầu drag
            dragTimer.Tick += DragTimer_Tick;

            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.S)
                {
                    _ = SaveCurrentLoop();
                    e.Handled = true;
                }

                // ===== THÊM DÒNG NÀY =====
                if (e.KeyCode == Keys.Delete)
                {
                    DeleteSelectedNode();
                    e.Handled = true;
                }
            };

            InitCaptureConnectedSelect();

            PopulateActionTypes();

            if (_phone != null)
            {
                UpdatePhoneInfo();
                LoadLoopsForPhone();
            }

            LoadAllPhoneModels();

            EnableFormDrag(panel1);
            EnableFormDrag(panelRoundn1);


            StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += FormAoaLoopEditor_FormClosing;
        }

        private void FormAoaLoopEditor_Load(object? sender, EventArgs e)
        {
            ApplyTheme();
        }
        private void FormAoaLoopEditor_FormClosing(object? sender, FormClosingEventArgs e)
        {
            treeActions.NodeMouseClick -= TreeActions_NodeMouseClick;
            treeActions.MouseDown -= TreeActions_MouseDown;
            treeActions.MouseMove -= TreeActions_MouseMove;
            treeActions.MouseUp -= TreeActions_MouseUp;
            treeActions.Paint -= TreeActions_Paint;

            dragTimer?.Stop();
            dragTimer?.Dispose();
        }
        private void InitCaptureConnectedSelect()
        {
            selectCaptureConnected.Items.Clear();
            selectCaptureConnected.Items.Add(new AntdUI.SelectItem("📱 Capture Connected (WiFi + AppCapture)", "true"));
            selectCaptureConnected.Items.Add(new AntdUI.SelectItem("🔌 No Capture (USB HID Only)", "false"));

            // Mặc định chọn "No Capture"
            selectCaptureConnected.SelectedIndex = 1; // Index 1 = "false"
            _isCaptureConnected = false;

            selectCaptureConnected.SelectedValueChanged += SelectCaptureConnected_Changed;
        }

        private void SelectCaptureConnected_Changed(object? sender, AntdUI.ObjectNEventArgs e)
        {
            var value = e.Value?.ToString();
            _isCaptureConnected = value == "true";

            // Cập nhật label trạng thái
            labelFile.Text = _isCaptureConnected
                ? "📱 Mode: Capture Connected (WiFi + AppCapture)"
                : "🔌 Mode: No Capture (USB HID Only)";

            // ===== THÊM: Cập nhật lại danh sách action types =====
            PopulateActionTypes();

            // Reset selected node về None
            _selectedNode = null;
            labelSelectedNode.Text = "Selected: None";
            labelAddAction.Text = "Add: Click";

            // Clear panel params
            panelParams.Controls.Clear();

            // Reload loops cho mode capture tương ứng
            if (_phone != null)
            {
                LoadLoopsForPhone(_isCaptureConnected);
            }
        }
        #region Theme

        public void ApplyTheme()
        {
            foreach (var ctrl in GetAllControls(this))
            {
                if (ctrl is IThemeable themeable)
                    themeable.ApplyTheme();
            }

            BackColor = ThemeHelper.BorderNormalFist;
            ForeColor = ThemeHelper.ForeNormalFirst;

            panelMain.BackColor = ThemeHelper.BackNormalFirst;
            panelMain.ForeColor = ThemeHelper.ForeNormalFirst;
            panelWorking.BackColor = ThemeHelper.BackNormalFirst;
            panelWorking.ForeColor = ThemeHelper.ForeNormalFirst;
            panelRoundn1.BackColor = ThemeHelper.BackNormalFirst;
            panel1.BackColor = ThemeHelper.BackNormalFirst;
            panelRight.BackColor = ThemeHelper.BackNormalFirst;
            PictureBoxCloseForm.ApplyTheme();
            panelParams.Back = ThemeHelper.BackNormalFirst;
            treeActions.BackColor = ThemeHelper.BackNormalFirst;
            treeActions.ForeColor = ThemeHelper.ForeNormalFirst;

            // Theme cho panelParams labels
            foreach (System.Windows.Forms.Label lbl in panelParams.Controls.OfType<System.Windows.Forms.Label>())
            {
                lbl.ForeColor = ThemeHelper.ForeNormalFirst;
            }
        }

        private static IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                yield return c;
                foreach (var child in GetAllControls(c))
                    yield return child;
            }
        }

        #endregion

        #region Actions

        private void PictureBoxCloseForm_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
        private Dictionary<string, int> _actionTypeIndexMap = new();
        private void PopulateActionTypes()
        {
            selectActionType.Items.Clear();
            _actionTypeIndexMap.Clear();

            // ===== SỬA: Tạo danh sách dựa vào _isCaptureConnected =====
            var items = new List<(string Value, string Display)>();

            // Actions LUÔN có (cho cả 2 mode)
            items.Add(("CloseAllApps", "🧹 Close All Apps"));
            items.Add(("Click", "👆 Click"));
            items.Add(("Swipe", "👈 Swipe"));
            items.Add(("KeyPress", "⌨️ Key Press"));
            items.Add(("SendText", "📝 Send Text"));
            items.Add(("Delay", "⏱️ Delay (ms)"));

            // Actions CHỈ có khi capture connected
            if (_isCaptureConnected)
            {
                items.Add(("Deeplink", "🔗 Deeplink"));
                items.Add(("Ocr", "🔍 OCR (Find & Click)"));
            }

            int idx = 0;
            foreach (var (value, display) in items)
            {
                var item = new AntdUI.SelectItem(display, value);
                selectActionType.Items.Add(item);
                _actionTypeIndexMap[value] = idx;
                idx++;
            }

            if (selectActionType.Items.Count > 0)
                selectActionType.SelectedIndex = 1; // Chọn Click làm default
        }

        private void UpdatePhoneInfo()
        {
            if (_phone == null)
            {
                labelPhoneInfo.Text = "No phone selected";
                return;
            }

            string captureMode = _isCaptureConnected ? "📱 WiFi+Capture" : "🔌 USB HID";

            labelPhoneInfo.Text = $"📱 {_phone.Model}\n" +
                                  $"API: {_phone.API} | Mode: {captureMode}\n" +
                                  $"CPU: {(_phone.ProcCpuInfo?.Length > 25 ? _phone.ProcCpuInfo[..25] + "..." : _phone.ProcCpuInfo)}";
        }

        #endregion

        #region Select Events

        private void SelectPhoneModel_Changed(object? sender, AntdUI.ObjectNEventArgs e)
        {
            var model = e.Value?.ToString();
            if (string.IsNullOrEmpty(model)) return;

            var phones = PhoneRepository.LoadAll()
                .Where(p => p.Model.Equals(model, StringComparison.OrdinalIgnoreCase))
                .ToList();
            var phone = phones.FirstOrDefault();
            if (phone != null)
            {
                _phone = phone;
                UpdatePhoneInfo();
                LoadLoopsForPhone(_isCaptureConnected); // ===== SỬA =====
            }
        }

        private void SelectActionType_Changed(object? sender, AntdUI.ObjectNEventArgs e)
        {
            var type = e.Value?.ToString();
            if (!string.IsNullOrEmpty(type))
            {
                labelAddAction.Text = $"Add: {type}";

                // ===== THÊM: Nếu đổi type khi đang có node selected, tự động apply =====
                // Chỉ build panel mới, KHÔNG apply vì user có thể đang xem
                BuildParamsPanel(type);
            }
        }

        #endregion

        #region Button Events

        private void BtnLoad_Click(object? sender, EventArgs e)
        {
            LoadLoopsForPhone(_isCaptureConnected);
        }

        private void BtnNewTemplate_Click(object? sender, EventArgs e)
        {
            if (_phone == null)
            {
                AntdUI.Message.info(this, "Please select a phone model first!");
                return;
            }

            // ===== SỬA: Truyền isCaptureConnected =====
            _currentLoop = AoaLoopMatcher.CreateDefaultForPhone(_phone, _isCaptureConnected);
            if (_currentLoop != null)
                PopulateTreeFromLoop(_currentLoop);

            AntdUI.Message.info(this, $"Default template created! (Capture={_isCaptureConnected})");
        }

        private void BtnAddNode_Click(object? sender, EventArgs e)
        {
            // Lấy typeStr từ SelectedValue (trả về Tag của SelectItem)
            var typeStr = selectActionType.SelectedValue?.ToString();

            // Debug
            System.Diagnostics.Debug.WriteLine($"BtnAdd: SelectedValue='{typeStr}'");

            if (string.IsNullOrEmpty(typeStr))
            {
                AntdUI.Message.info(this, "Please select an action type first!");
                return;
            }

            var newLoop = CreateLoopFromParams(typeStr);
            if (newLoop == null)
            {
                AntdUI.Message.info(this, $"Failed to create loop for type: {typeStr}");
                return;
            }


            // Đảm bảo _currentLoop tồn tại
            if (_currentLoop == null)
            {
                _currentLoop = new AoaLoop
                {
                    PhoneModel = _phone?.Model ?? "",
                    ProcVersion = _phone?.ProcVersion ?? "",
                    ProcCpuInfo = _phone?.ProcCpuInfo ?? "",
                    API = _phone?.API ?? 0,
                   
                };
            }
            _currentLoop.Children ??= new List<AoaLoop>();

            if (_selectedNode != null)
            {
                // Thêm vào node đang chọn (làm con)
                var parentLoop = _selectedNode.Tag as AoaLoop;
                if (parentLoop != null)
                {
                    parentLoop.Children ??= new List<AoaLoop>();
                    parentLoop.Children.Add(newLoop);
                    labelFile.Text = $"✅ Added '{typeStr}' as child of selected node";
                }
                else
                {
                    _currentLoop.Children.Add(newLoop);
                    labelFile.Text = $"✅ Added '{typeStr}' to root";
                }
            }
            else
            {
                // Thêm vào root
                _currentLoop.Children.Add(newLoop);
                labelFile.Text = $"✅ Added '{typeStr}' to root";
            }

            RebuildTree();
        }

        private void BtnDeleteNode_Click(object? sender, EventArgs e)
        {
            if (_selectedNode == null)
            {
                AntdUI.Message.info(this, "Please select a node to delete!");
                return;
            }

            var loopToDelete = _selectedNode.Tag as AoaLoop;
            if (loopToDelete == null) return;

            var parentLoop = FindParentLoop(_selectedNode);
            if (parentLoop?.Children != null)
            {
                parentLoop.Children.Remove(loopToDelete);
            }

            var name = _selectedNode.Text;
            _selectedNode = null;
            labelSelectedNode.Text = "Selected: None";
            labelFile.Text = $"🗑️ Deleted: {name}";
            RebuildTree();
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            if (_currentLoop == null || _phone == null)
            {
                AntdUI.Message.info(this, "No data to save!");
                return;
            }

            _currentLoop.PhoneModel = _phone.Model;
            _currentLoop.ProcVersion = _phone.ProcVersion;
            _currentLoop.ProcCpuInfo = _phone.ProcCpuInfo;
            _currentLoop.API = _phone.API;
            _currentLoop.PhysicalHeight = _phone.PhysicalHeight;
            _currentLoop.PhysicalWidth = _phone.PhysicalWidth;
            _currentLoop.IsAppCaptureConnected = _isCaptureConnected;

            // ===== THÊM: Nếu đang sửa PointCloseApp, update cả 2 loop =====
            string currentPointClose = _currentLoop.PointCloseApp;

            bool saved = AoaLoopRepository.Save(_currentLoop);

            if (saved)
            {
                // Update PointCloseApp cho loop còn lại (mode capture ngược lại)
                var otherLoop = AoaLoopRepository.FindOneByUnique(
                    _phone.Model, _phone.ProcVersion, _phone.ProcCpuInfo, _phone.API, !_isCaptureConnected);

                if (otherLoop != null)
                {
                    otherLoop.PointCloseApp = currentPointClose;
                    AoaLoopRepository.Update(otherLoop);
                }

                labelFile.Text = $"✅ Saved successfully! (Capture={_isCaptureConnected})";
                AntdUI.Message.info(this, "Saved successfully!");
            }
            else
            {
                labelFile.Text = "❌ Save failed!";
                AntdUI.Message.info(this, "Save failed!");
            }
        }

        private async void BtnTest_Click(object? sender, EventArgs e)
        {
            if (_currentLoop == null || _phone == null)
            {
                AntdUI.Message.info(this, "No loop or phone selected!");
                return;
            }

            _currentLoop.HydrateTree();

            try
            {
                labelFile.Text = "⏳ Opening AOA session...";
                btnTest.Enabled = false;

                var session = await AoaDeviceManager.Instance.StartByDeviceIdAsync(_phone.DeviceID);
                if (session == null)
                {
                    AntdUI.Message.info(this, "Cannot open AOA session!");
                    labelFile.Text = "❌ Cannot open session";
                    btnTest.Enabled = true;
                    return;
                }

                // ===== SỬA: Chỉ lấy capture nếu _isCaptureConnected = true =====
                AppCapture? capture = null;
                if (_isCaptureConnected)
                {
                    capture = AppCaptureManager.Instance.GetByDeviceId(_phone.DeviceID);
                    if (capture == null)
                    {
                        labelFile.Text = "⚠️ Capture mode ON but no AppCapture found! Running without OCR...";
                    }
                }

                using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));

                btnTest.Text = "⏳ Running...";
                labelFile.Text = $"▶ Running AOA Loop (Capture={_isCaptureConnected})...";

                await Task.Run(async () =>
                {
                    await AoaLoopRunner.RunAsync(_currentLoop, session.ctrl, capture, cts.Token);
                });

                labelFile.Text = "✅ Test completed!";
                AntdUI.Message.info(this, "Test completed!");
            }
            catch (Exception ex)
            {
                labelFile.Text = $"❌ Error: {ex.Message}";
                AntdUI.Message.info(this, $"Error: {ex.Message}");
            }
            finally
            {
                btnTest.Text = "▶ Test Run";
                btnTest.Enabled = true;
            }
        }

        private Bitmap? _currentScreenshot;

        private async void BtnCaptureScreenshot_Click(object? sender, EventArgs e)
        {
            if (_phone == null)
            {
                AntdUI.Message.info(this, "No phone selected!");
                return;
            }

            try
            {
                btnCaptureScreenshot.Text = "⏳...";
                btnCaptureScreenshot.Enabled = false;

                // Lấy screenshot từ AppCapture nếu có, hoặc ADB
                if (_isCaptureConnected)
                {
                    var capture = AppCaptureManager.Instance.GetByDeviceId(_phone.DeviceID);
                    if (capture != null)
                    {
                        _currentScreenshot = await capture.ScreenshotBitmapAsync();
                    }
                }

                if (_currentScreenshot == null)
                {
                    var adbClient = new AdvancedSharpAdbClient.AdbClient();
                    var devices = adbClient.GetDevices();
                    var deviceData = devices.FirstOrDefault(d =>
                        d.Serial == _phone.DeviceID || d.Serial == _phone.Ipv4 + ":5555");

                    if (deviceData != null)
                    {
                        await Task.Run(() =>
                        {
                            var fb = adbClient.GetFrameBuffer(deviceData);
                            _currentScreenshot = fb.ToImage();
                        });
                    }
                }

                if (_currentScreenshot != null)
                {
                    picScreenshot.Image = _currentScreenshot;
                    labelFile.Text = "📸 Screenshot captured!";
                }
            }
            catch (Exception ex)
            {
                AntdUI.Message.info(this, $"Screenshot error: {ex.Message}");
            }
            finally
            {
                btnCaptureScreenshot.Text = "📷 Capture";
                btnCaptureScreenshot.Enabled = true;
            }
        }

        private void PicScreenshot_MouseClick(object? sender, MouseEventArgs e)
        {
            if (_currentScreenshot == null || picScreenshot.Image == null) return;

            // Tính % từ vị trí click
            var displayRect = GetImageDisplayRect(picScreenshot);
            if (displayRect == Rectangle.Empty) return;

            float ratioX = (float)picScreenshot.Image.Width / displayRect.Width;
            float ratioY = (float)picScreenshot.Image.Height / displayRect.Height;

            int imgX = (int)((e.X - displayRect.X) * ratioX);
            int imgY = (int)((e.Y - displayRect.Y) * ratioY);

            imgX = Math.Max(0, Math.Min(picScreenshot.Image.Width - 1, imgX));
            imgY = Math.Max(0, Math.Min(picScreenshot.Image.Height - 1, imgY));

            float pctX = (float)imgX / picScreenshot.Image.Width * 100f;
            float pctY = (float)imgY / picScreenshot.Image.Height * 100f;

            string coordText = $"{pctX:F2},{pctY:F2}";
            labelCoordinates.Text = $"📋 {coordText} (copied)";

            Clipboard.SetText(coordText);

            // Auto fill vào Click params nếu đang chọn Click
            if (_inputClickX != null && _inputClickY != null)
            {
                _inputClickX.Text = pctX.ToString("F2");
                _inputClickY.Text = pctY.ToString("F2");
            }

            // Auto fill vào Close Point nếu đang chọn CloseAllApps
            if (_inputClosePoint != null)
            {
                _inputClosePoint.Text = coordText;
            }
        }

        private Rectangle GetImageDisplayRect(PictureBox pic)
        {
            if (pic.Image == null) return Rectangle.Empty;

            int imgW = pic.Image.Width;
            int imgH = pic.Image.Height;
            int boxW = pic.ClientSize.Width;
            int boxH = pic.ClientSize.Height;

            float ratio = Math.Min((float)boxW / imgW, (float)boxH / imgH);
            int scaledW = (int)Math.Round(imgW * ratio);
            int scaledH = (int)Math.Round(imgH * ratio);
            int offsetX = (boxW - scaledW) / 2;
            int offsetY = (boxH - scaledH) / 2;

            return new Rectangle(offsetX, offsetY, scaledW, scaledH);
        }

        #endregion

        #region Drag & Drop Tree (2 chế độ: Insert Child / Insert After)

        // Biến trạng thái
        private AntdUI.TreeItem? draggedItem;
        private AntdUI.TreeItem? dropTarget;
        private AntdUI.TreeItem? treeItemSelected;
        private AntdUI.TreeItem? originalParent;
        private int originalIndex;
        private bool isDragging = false;
        private bool _isDragStarted = false; // ===== THÊM: phân biệt click vs drag =====
        private System.Windows.Forms.Timer dragTimer;
        private Point currentMousePos;
        private Point _mouseDownPos; // ===== THÊM: vị trí chuột lúc nhấn =====

        private enum DropPosition
        {
            None,
            Child,
            After
        }
        private DropPosition _dropPosition = DropPosition.None;

        // MouseDown: reset trạng thái và khởi động timer
        private void TreeActions_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            treeItemSelected = null;
            treeItemSelected = FindNodeAt(treeActions, e.Location);
            if (treeItemSelected == null) return;
            // Không cho drag root node
            if (treeItemSelected.ParentItem == null) return;

            _mouseDownPos = e.Location;
            isDragging = false;
            _isDragStarted = false;
            draggedItem = null;
            dropTarget = null;
            originalParent = null;
            originalIndex = -1;
            _dropPosition = DropPosition.None;
            dragTimer.Start();
        }

        // Timer tick - bắt đầu drag sau khi giữ chuột
        private void DragTimer_Tick(object? sender, EventArgs e)
        {
            dragTimer.Stop();
            if (treeItemSelected != null && !isDragging)
            {
                draggedItem = treeItemSelected;
                isDragging = true;
                _isDragStarted = true; // ===== THÊM =====

                // Lưu vị trí cũ
                originalParent = draggedItem.ParentItem;
                originalIndex = draggedItem.Index;

                treeActions.Cursor = Cursors.Hand;
            }
        }

        // MouseMove: cập nhật vị trí chuột và highlight node target
        private void TreeActions_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!isDragging || draggedItem == null) return;

            currentMousePos = e.Location;

            var newTarget = FindNodeAt(treeActions, e.Location);

            // Không cho drop vào chính nó hoặc con của chính nó
            if (newTarget == draggedItem || IsChildOf(draggedItem, newTarget))
            {
                newTarget = null;
            }

            // Clear highlight cũ
            if (dropTarget != null && dropTarget != newTarget)
            {
                dropTarget.Back = null;
            }

            dropTarget = newTarget;

            if (dropTarget != null)
            {
                var nodeRect = GetNodeRect(dropTarget);
                int mouseYInNode = e.Location.Y - nodeRect.Y;
                int nodeHeight = nodeRect.Height;

                if (mouseYInNode < nodeHeight / 3)
                {
                    _dropPosition = DropPosition.Child;
                    dropTarget.Back = Color.FromArgb(100, 180, 255);
                }
                else
                {
                    _dropPosition = DropPosition.After;
                    dropTarget.Back = Color.FromArgb(255, 200, 100);
                }
            }
            else
            {
                _dropPosition = DropPosition.None;
            }

            treeActions.Invalidate();
        }

        // Paint: vẽ ghost node + indicator line
        private void TreeActions_Paint(object? sender, PaintEventArgs e)
        {
            if (isDragging && draggedItem != null)
            {
                // Vẽ ghost node theo chuột
                using (Brush b = new SolidBrush(Color.FromArgb(180, Color.LightGray)))
                {
                    var size = e.Graphics.MeasureString(draggedItem.Text, treeActions.Font);
                    var ghostRect = new RectangleF(currentMousePos.X + 10, currentMousePos.Y + 5, size.Width + 10, size.Height + 4);
                    e.Graphics.FillRectangle(b, ghostRect);
                    e.Graphics.DrawString(draggedItem.Text, treeActions.Font, Brushes.Black, ghostRect.X + 5, ghostRect.Y + 2);
                }

                // Vẽ indicator line nếu là chế độ After
                if (dropTarget != null && _dropPosition == DropPosition.After)
                {
                    var nodeRect = GetNodeRect(dropTarget);
                    int lineY = nodeRect.Bottom - 2;
                    using (Pen pen = new Pen(Color.OrangeRed, 2))
                    {
                        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        e.Graphics.DrawLine(pen, nodeRect.X + 20, lineY, nodeRect.Right - 10, lineY);
                    }
                    e.Graphics.FillPolygon(Brushes.OrangeRed, new Point[]
                    {
                new Point(nodeRect.X + 20, lineY - 5),
                new Point(nodeRect.X + 20, lineY + 5),
                new Point(nodeRect.X + 30, lineY)
                    });
                }

                // Vẽ indicator nếu là chế độ Child
                if (dropTarget != null && _dropPosition == DropPosition.Child)
                {
                    var nodeRect = GetNodeRect(dropTarget);
                    using (Pen pen = new Pen(Color.DodgerBlue, 2))
                    {
                        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        e.Graphics.DrawRectangle(pen, nodeRect.X + 2, nodeRect.Y + 2,
                            nodeRect.Width - 4, nodeRect.Height - 4);
                    }
                }
            }
        }

        // MouseUp: hoàn tất drag & drop hoặc select node
        private void TreeActions_MouseUp(object? sender, MouseEventArgs e)
        {
            dragTimer.Stop();
            treeActions.Cursor = Cursors.Default;

            if (isDragging && draggedItem != null && dropTarget != null)
            {
                var draggedLoop = draggedItem.Tag as AoaLoop;
                if (draggedLoop == null) goto Cleanup;

                // ===== 1. CẬP NHẬT DATA MODEL =====
                // Remove khỏi parent cũ
                var oldParentLoop = originalParent?.Tag as AoaLoop;
                if (oldParentLoop?.Children != null)
                {
                    oldParentLoop.Children.Remove(draggedLoop);
                }

                if (_dropPosition == DropPosition.Child)
                {
                    var targetLoop = dropTarget.Tag as AoaLoop;
                    if (targetLoop != null)
                    {
                        targetLoop.Children ??= new List<AoaLoop>();
                        targetLoop.Children.Add(draggedLoop);
                        labelFile.Text = $"↘️ Moved → child of '{dropTarget.Text}'";
                    }
                }
                else if (_dropPosition == DropPosition.After)
                {
                    var targetParentItem = dropTarget.ParentItem;
                    var targetLoop = dropTarget.Tag as AoaLoop;
                    var targetParentLoop = targetParentItem?.Tag as AoaLoop ?? _currentLoop;

                    if (targetParentLoop?.Children != null && targetLoop != null)
                    {
                        int targetIndex = targetParentLoop.Children.IndexOf(targetLoop);
                        if (targetIndex >= 0)
                            targetParentLoop.Children.Insert(targetIndex + 1, draggedLoop);
                        else
                            targetParentLoop.Children.Add(draggedLoop);
                    }
                    labelFile.Text = $"↔️ Moved → after '{dropTarget.Text}'";
                }

                // ===== 2. LƯU SCROLL =====
                int scrollY = treeActions.ScrollBar.ValueY;

                // ===== 3. REBUILD TREE =====
                RebuildTree();

                // ===== 4. KHÔI PHỤC SCROLL =====
                treeActions.ScrollBar.ValueY = Math.Min(scrollY, treeActions.ScrollBar.MaxY);
            }

        Cleanup:
            if (dropTarget != null)
            {
                dropTarget.Back = null;
                dropTarget = null;
            }

            originalParent = null;
            originalIndex = -1;
            draggedItem = null;
            isDragging = false;
            _dropPosition = DropPosition.None;
            treeActions.Refresh();
            treeActions.Invalidate();
        }
        private void UpdateNodeTexts(AntdUI.TreeItem parent)
        {
            for (int i = 0; i < parent.Sub.Count; i++)
            {
                var child = parent.Sub[i];
                var loop = child.Tag as AoaLoop;
                if (loop == null) continue;

                string icon = loop.Type switch
                {
                    AoaType.CloseAllApps => "🧹",
                    AoaType.Click => "👆",
                    AoaType.Swipe => "👈",
                    AoaType.KeyPress => "⌨",
                    AoaType.Deeplink => "🔗",
                    AoaType.SendText => "📝",
                    AoaType.Ocr => "🔍",
                    AoaType.Delay => "⏱",
                    _ => "📌"
                };

                string summary = GetLoopSummary(loop);
                child.Text = $"{i + 1}. {icon} [{loop.Type}] {summary}";
                child.Text = child.Text.Replace("\uFE0F", "").Replace("\u200D", "");

                // Đệ quy cho children
                UpdateNodeTexts(child);
            }
        }
        // ===== THÊM: Kiểm tra node có phải con của node khác không =====
        private bool IsChildOf(AntdUI.TreeItem? parent, AntdUI.TreeItem? child)
        {
            if (parent == null || child == null) return false;
            var current = child.ParentItem;
            while (current != null)
            {
                if (current == parent) return true;
                current = current.ParentItem;
            }
            return false;
        }

        private Rectangle GetNodeRect(AntdUI.TreeItem node)
        {
            int sx = treeActions.ScrollBar.ValueX;
            int sy = treeActions.ScrollBar.ValueY;
            return node.Rect("", sx, sy);
        }

        private AntdUI.TreeItem? FindNodeAt(AntdUI.Tree tree, Point location)
        {
            int sx = tree.ScrollBar.ValueX;
            int sy = tree.ScrollBar.ValueY;

            foreach (var item in tree.Items)
            {
                if (item.Rect("", sx, sy).Contains(location))
                    return item;

                foreach (var sub in GetAllSubItems(item))
                {
                    if (sub.Rect("", sx, sy).Contains(location))
                        return sub;
                }
            }
            return null;
        }

        private IEnumerable<AntdUI.TreeItem> GetAllSubItems(AntdUI.TreeItem parent)
        {
            if (parent.Sub == null || parent.Sub.Count == 0)
                yield break;

            foreach (var item in parent.Sub)
            {
                yield return item;

                foreach (var sub in GetAllSubItems(item))
                    yield return sub;
            }
        }

        #endregion

        #region Tree Events

        // ===== SỬA: Chỉ select khi KHÔNG drag =====
        private void TreeActions_NodeMouseClick(object? sender, AntdUI.TreeSelectEventArgs e)
        {
            if (isDragging || _isDragStarted) return;

            // ===== THIẾU DÒNG NÀY =====
            // Apply changes cho node cũ trước khi chọn node mới
            if (_selectedNode != null && _selectedNode != e.Item)
            {
                ApplyCurrentParamsToSelectedNode();
            }

            _selectedNode = e.Item;
            var loop = e.Item?.Tag as AoaLoop;
            if (loop != null)
            {
                labelSelectedNode.Text = $"Selected: {loop.Type}";

                var typeStr = loop.Type.ToString();
                if (_actionTypeIndexMap.TryGetValue(typeStr, out int idx))
                {
                    selectActionType.SelectedIndex = idx;
                }

                BuildParamsPanel(typeStr);
                LoadParamsFromLoop(loop);
            }
        }

        #endregion

        #region Tree Helper Methods

        private void RebuildTree()
        {
            if (_currentLoop == null) return;

            // ===== LƯU TRẠNG THÁI EXPAND =====
            var expandedPaths = GetExpandedPaths();

            // ===== REBUILD =====
            PopulateTreeFromLoop(_currentLoop);

            // ===== KHÔI PHỤC EXPAND (không ExpandAll) =====
            RestoreExpandedPaths(expandedPaths);
        }

        private HashSet<string> GetExpandedPaths()
        {
            var paths = new HashSet<string>();
            if (treeActions.Items.Count > 0)
            {
                CollectExpandedPaths(treeActions.Items[0], "", paths);
            }
            return paths;
        }
        private void CollectExpandedPaths(AntdUI.TreeItem? node, string path, HashSet<string> paths)
        {
            if (node == null || paths == null) return;

            string nodeText = node.Text ?? "";
            string currentPath = string.IsNullOrEmpty(path) ? nodeText : $"{path}/{nodeText}";

            if (node.Expand)
                paths.Add(currentPath);

            if (node.Sub != null)
            {
                foreach (AntdUI.TreeItem child in node.Sub)
                {
                    CollectExpandedPaths(child, currentPath, paths);
                }
            }
        }
        private void RestoreExpandedPaths(HashSet<string> paths)
        {
            if (paths.Count == 0)
            {
                treeActions.ExpandAll();
                return;
            }
            // Nếu có paths đã lưu thì không làm gì, giữ nguyên
        }

        private AoaLoop? FindParentLoop(AntdUI.TreeItem node)
        {
            var parent = node.ParentItem;
            while (parent != null)
            {
                if (parent.Tag is AoaLoop loop)
                    return loop;
                parent = parent.ParentItem;
            }
            return _currentLoop;
        }

        #endregion

        #region Data Methods

        // ===== SỬA: Thêm tham số isCaptureConnected =====
        private void LoadLoopsForPhone(bool? isCaptureConnected = null)
        {
            if (_phone == null) return;

            bool filterCapture = isCaptureConnected ?? _isCaptureConnected;

            // Lọc theo IsAppCaptureConnected
            _allLoops = AoaLoopRepository.FindByPhoneModel(_phone.Model, filterCapture);

            treeActions.Items.Clear();

            if (_allLoops.Count > 0)
            {
                // Tìm exact match với 5 trường unique
                _currentLoop = _allLoops.FirstOrDefault(
                    l => l.PhoneModel == _phone.Model
                      && l.ProcVersion == _phone.ProcVersion
                      && l.ProcCpuInfo == _phone.ProcCpuInfo
                      && l.API == _phone.API
                      && l.IsAppCaptureConnected == filterCapture)
                    ?? _allLoops.First();

                _currentLoop.HydrateTree();
                PopulateTreeFromLoop(_currentLoop);
                labelFile.Text = $"Loaded {_allLoops.Count} loop(s) for {_phone.Model} (Capture={filterCapture})";
            }
            else
            {
                // ===== SỬA: Truyền isCaptureConnected =====
                _currentLoop = AoaLoopMatcher.CreateDefaultForPhone(_phone, filterCapture);
                if (_currentLoop != null)
                {
                    PopulateTreeFromLoop(_currentLoop);
                    labelFile.Text = $"Created default template for {_phone.Model} (Capture={filterCapture})";
                }
                else
                {
                    labelFile.Text = "No loops found. Click 'New Template' to create.";
                }
            }
        }

        private void LoadAllPhoneModels()
        {
            var phones = PhoneRepository.LoadAll();
            var models = phones.Select(p => p.Model).Distinct().OrderBy(m => m).ToList();

            selectPhoneModel.Items.Clear();
            foreach (var model in models)
                selectPhoneModel.Items.Add(new AntdUI.SelectItem(model, model));
        }

        private void PopulateTreeFromLoop(AoaLoop loop)
        {
            treeActions.Items.Clear();

            string captureIcon = loop.IsAppCaptureConnected ? "📱" : "🔌";
            string captureText = loop.IsAppCaptureConnected ? "WiFi+Capture" : "USB HID";

            var rootNode = new AntdUI.TreeItem($"{captureIcon} {loop.PhoneModel} (API {loop.API}) [{captureText}]")
            {
                Tag = loop
            };

            if (loop.Children != null)
            {
                for (int i = 0; i < loop.Children.Count; i++)
                {
                    AddNodeToTree(rootNode, loop.Children[i], i + 1);
                }
            }

            treeActions.Items.Add(rootNode);
            treeActions.ExpandAll();
        }

        private void AddNodeToTree(AntdUI.TreeItem parent, AoaLoop loop, int index)
        {
            string icon = loop.Type switch
            {
                AoaType.CloseAllApps => "🧹",
                AoaType.Click => "👆",
                AoaType.Swipe => "👈",
                AoaType.KeyPress => "⌨",
                AoaType.Deeplink => "🔗",
                AoaType.SendText => "📝",
                AoaType.Ocr => "🔍",
                AoaType.Delay => "⏱",
                _ => "📌"
            };

            string summary = GetLoopSummary(loop);  // <-- Đã gọi hàm mới
            var text = $"{index}. {icon} [{loop.Type}] {summary}";

            // ===== THÊM: Strip variation selectors =====
            text = text.Replace("\uFE0F", "").Replace("\u200D", "");

            var node = new AntdUI.TreeItem(text)
            {
                Tag = loop
            };

            if (loop.Children != null)
            {
                for (int i = 0; i < loop.Children.Count; i++)
                {
                    AddNodeToTree(node, loop.Children[i], i + 1);
                }
            }

            parent.Sub.Add(node);
        }

        private string GetLoopSummary(AoaLoop loop)
        {
            loop.HydratePayload();

            string summary = loop.Payload switch
            {
                AoaClick c => $"({c.X},{c.Y}) x{c.NumClicks}",
                AoaSwipe s => $"({s.X1},{s.Y1})→({s.X2},{s.Y2}) {s.DurationMs}ms x{s.NumSwipe}",  // <-- SỬA
                AoaKeyPress k => $"'{k.Key}' x{k.Repeat}",
                AoaDeeplink d => d.Url.Length > 25 ? d.Url[..25] + "..." : d.Url,
                AoaSendText t => t.Text.Length > 20 ? t.Text[..20] + "..." : t.Text,
                AoaOcr o => $"'{string.Join(",", o.Keywords)}' ({o.TimeoutMs}ms) x{o.MaxSwipes} swipes",  // <-- SỬA
                int delay => $"{delay}ms",
                string point when loop.Type == AoaType.CloseAllApps => $"Close at: {point}",
                _ => "(no params)"
            };

            // Thêm Title vào sau summary nếu có
            string title = loop.Payload switch
            {
                AoaClick c => c.Tilte,
                AoaSwipe s => s.Tilte,
                AoaOcr o => o.Title,
                _ => ""
            };

            if (!string.IsNullOrEmpty(title))
                return $"{summary} - {title}";

            return summary;
        }

        #endregion

        #region Params Panel

        private AntdUI.Input? _inputClickX, _inputClickY, _inputClickCount, _inputClickDelay;
        private AntdUI.Input? _inputSwipeX1, _inputSwipeY1, _inputSwipeX2, _inputSwipeY2, _inputSwipeDuration, _inputSwipeCount;
        private AntdUI.Input? _inputDelayMs;
        private AntdUI.Input? _inputKey, _inputKeyRepeat, _inputKeyDelay;
        private AntdUI.Input? _inputDeeplinkUrl, _inputDeeplinkWait;
        private AntdUI.Input? _inputSendText, _inputSendDelay;
        private AntdUI.Input?  _inputOcrKeywords, _inputOcrTimeout, _inputOcrInterval, _inputOcrMaxSwipes, _inputOcrOffsetX, _inputOcrOffsetY;
        private AntdUI.Input? _inputClosePoint;
        private System.Windows.Forms.TextBox? _rtbOcrKeywords;
        private AntdUI.Input? _inputClickTitle;
        private AntdUI.Input? _inputSwipeTitle;
        private AntdUI.Input? _inputOcrTitle;
        private AntdUI.Select? _selectKeyPress;
        private AntdUI.Switch? _switchClickIsPercent;
        private AntdUI.Switch? _switchSwipeIsPercent;
        // Thêm vào phần khai báo biến:
        private AntdUI.Input? _inputOcrSwipeX1, _inputOcrSwipeY1, _inputOcrSwipeX2, _inputOcrSwipeY2, _inputOcrSwipeDuration;

        private void BuildParamsPanel(string actionType)
        {
            panelParams.Controls.Clear();
            int y = 5;

            // Hiển thị title của panel params
            var titleLabel = new System.Windows.Forms.Label
            {
                Location = new Point(5, y),
                Size = new Size(230, 22),
                Text = $"⚙️ {actionType} Settings",
                ForeColor = Color.Cyan,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                BackColor = Color.Transparent
            };
            panelParams.Controls.Add(titleLabel);
            y += 30;

            switch (actionType)
            {
                case "CloseAllApps":
                    AddInput(panelParams, "Close Point (%,%):", "Tọa độ nút 'Close All' theo % màn hình (vd: 50,90)", ref _inputClosePoint, "50,90", ref y);
                    break;

                case "Click":
                    AddInput(panelParams, "Title:", "Tên mô tả cho action này", ref _inputClickTitle, "Click Action", ref y);
                    AddInput(panelParams, "X (%):", "Tọa độ X theo % màn hình (0-100)", ref _inputClickX, "50", ref y);
                    AddInput(panelParams, "Y (%):", "Tọa độ Y theo % màn hình (0-100)", ref _inputClickY, "50", ref y);
                    AddInput(panelParams, "Count:", "Số lần click liên tiếp", ref _inputClickCount, "1", ref y);
                    AddInput(panelParams, "Delay (ms):", "Thời gian nghỉ giữa các lần click", ref _inputClickDelay, "300", ref y);

                    // ===== THÊM: Toggle IsPercent =====
                    AddLabel(panelParams, "Use Percent (%):", ref y);
                    _switchClickIsPercent = new AntdUI.Switch
                    {
                        Location = new Point(5, y),
                        Size = new Size(50, 24),
                        Checked = true
                    };
                    panelParams.Controls.Add(_switchClickIsPercent);
                    y += 30;
                    break;

                case "Swipe":
                    AddInput(panelParams, "Title:", "Tên mô tả cho action này", ref _inputSwipeTitle, "Swipe Action", ref y);
                    AddInput(panelParams, "Start X (%):", "Tọa độ X bắt đầu theo %", ref _inputSwipeX1, "50", ref y);
                    AddInput(panelParams, "Start Y (%):", "Tọa độ Y bắt đầu theo %", ref _inputSwipeY1, "80", ref y);
                    AddInput(panelParams, "End X (%):", "Tọa độ X kết thúc theo %", ref _inputSwipeX2, "50", ref y);
                    AddInput(panelParams, "End Y (%):", "Tọa độ Y kết thúc theo %", ref _inputSwipeY2, "40", ref y);
                    AddInput(panelParams, "Duration (ms):", "Thời gian vuốt (ms)", ref _inputSwipeDuration, "300", ref y);
                    AddInput(panelParams, "Count:", "Số lần vuốt", ref _inputSwipeCount, "1", ref y);

                    // ===== THÊM: Toggle IsPercent =====
                    AddLabel(panelParams, "Use Percent (%):", ref y);
                    _switchSwipeIsPercent = new AntdUI.Switch
                    {
                        Location = new Point(5, y),
                        Size = new Size(50, 24),
                        Checked = true
                    };
                    panelParams.Controls.Add(_switchSwipeIsPercent);
                    y += 30;
                    break;

                case "KeyPress":
                    // Key - Dùng Select (combobox) thay vì Input
                    AddLabel(panelParams, "Key:", ref y);
                    _selectKeyPress = new AntdUI.Select
                    {
                        Location = new Point(5, y),
                        Size = new Size(230, 30),
                        Font = new Font("Segoe UI", 9f),
                        List = true,
                        ListAutoWidth = true,
                        PlaceholderText = "Chọn phím..."
                    };
                    foreach (var key in AoaLoopRunner.AoaKeypressList)
                    {
                        _selectKeyPress.Items.Add(new AntdUI.SelectItem(key, key));
                    }
                    panelParams.Controls.Add(_selectKeyPress);
                    y += 38;

                    AddInput(panelParams, "Repeat:", "Số lần nhấn", ref _inputKeyRepeat, "1", ref y);
                    AddInput(panelParams, "Delay (ms):", "Thời gian nghỉ giữa các lần nhấn", ref _inputKeyDelay, "200", ref y);
                    break;

                case "Deeplink":
                    AddInput(panelParams, "URL:", "settings, wifi, bluetooth, hoặc URL đầy đủ https://...", ref _inputDeeplinkUrl, "settings", ref y);
                    AddInput(panelParams, "Wait (ms):", "Thời gian chờ sau khi mở deeplink", ref _inputDeeplinkWait, "2000", ref y);
                    break;

                case "SendText":
                    AddInput(panelParams, "Text:", "Nội dung text cần gõ qua bàn phím AOA", ref _inputSendText, "", ref y);
                    AddInput(panelParams, "Delay/Char (ms):", "Độ trễ giữa mỗi ký tự khi gõ", ref _inputSendDelay, "50", ref y);
                    break;

                case "Ocr":
                    AddInput(panelParams, "Title:", "Tên mô tả cho action này", ref _inputOcrTitle, "OCR Action", ref y);
                    AddLabel(panelParams, "Keywords (mỗi dòng 1 từ khóa):", ref y);
                    _rtbOcrKeywords = new System.Windows.Forms.TextBox
                    {
                        Location = new Point(5, y),
                        Size = new Size(230, 60),
                        Text = "Settings\nAbout\nGiới thiệu\nCài đặt",
                        Font = new Font("Segoe UI", 9f),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = ThemeHelper.BackNormalFirst,
                        ForeColor = ThemeHelper.ForeNormalFirst,
                        Multiline = true,
                    };
                    panelParams.Controls.Add(_rtbOcrKeywords);
                    y += 68;

                    AddInput(panelParams, "Timeout (ms):", "Thời gian tối đa tìm kiếm", ref _inputOcrTimeout, "5000", ref y);
                    AddInput(panelParams, "Interval (ms):", "Thời gian giữa các lần thử", ref _inputOcrInterval, "500", ref y);
                    AddInput(panelParams, "Max Swipes:", "Số lần vuốt tối đa", ref _inputOcrMaxSwipes, "5", ref y);

                    // ===== THÊM: Tọa độ swipe =====
                    AddLabel(panelParams, "Swipe Config:", ref y);
                    AddInput(panelParams, "  Start X (%):", "Tọa độ X bắt đầu vuốt", ref _inputOcrSwipeX1, "50", ref y);
                    AddInput(panelParams, "  Start Y (%):", "Tọa độ Y bắt đầu vuốt (thường ở dưới)", ref _inputOcrSwipeY1, "80", ref y);
                    AddInput(panelParams, "  End X (%):", "Tọa độ X kết thúc vuốt", ref _inputOcrSwipeX2, "50", ref y);
                    AddInput(panelParams, "  End Y (%):", "Tọa độ Y kết thúc vuốt (thường ở trên)", ref _inputOcrSwipeY2, "40", ref y);
                    AddInput(panelParams, "  Swipe Duration (ms):", "Thời gian vuốt", ref _inputOcrSwipeDuration, "300", ref y);

                    // Offset X, Y
                    string physicalInfo = _phone != null ? $" (Physical: {_phone.PhysicalWidth}x{_phone.PhysicalHeight})" : "";
                    AddInput(panelParams, $"Offset X{physicalInfo}:", "Click lệch X (pixel)", ref _inputOcrOffsetX, "0", ref y);
                    AddInput(panelParams, $"Offset Y{physicalInfo}:", "Click lệch Y (pixel)", ref _inputOcrOffsetY, "0", ref y);
                    break;


                case "Delay":
                    AddInput(panelParams, "Delay (ms):", "Thời gian chờ (ms)", ref _inputDelayMs, "1000", ref y);
                    break;
            }
        }

        // Helper mới: Chỉ thêm Label không có Input
        private void AddLabel(Control parent, string labelText, ref int y)
        {
            var lbl = new System.Windows.Forms.Label
            {
                Location = new Point(5, y),
                Size = new Size(230, 18),
                Text = labelText,
                ForeColor = ThemeHelper.ForeNormalFirst,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                BackColor = Color.Transparent
            };
            parent.Controls.Add(lbl);
            y += 20;
        }


        private void AddInput(Control parent, string labelText, string hint, ref AntdUI.Input? input, string defaultValue, ref int y)
        {
            var lbl = new System.Windows.Forms.Label
            {
                Location = new Point(5, y),
                Size = new Size(230, 18),
                Text = labelText,
                ForeColor = ThemeHelper.ForeNormalFirst,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                BackColor = Color.Transparent
            };
            parent.Controls.Add(lbl);
            y += 20;

            input = new AntdUI.Input
            {
                Location = new Point(5, y),
                Size = new Size(230, 30),
                Text = defaultValue,
                Font = new Font("Segoe UI", 9f),
                PlaceholderText = hint
            };
            parent.Controls.Add(input);
            y += 38;
        }
        private AoaLoop? CreateLoopFromParams(string typeStr)
        {
            return typeStr switch
            {
                "CloseAllApps" => new AoaLoop
                {
                    Type = AoaType.CloseAllApps,
                    PointCloseApp = _inputClosePoint?.Text ?? "50,90"  // Lưu thẳng string percent
                },

                "Click" => new AoaLoop
                {
                    Type = AoaType.Click,
                    Payload = new AoaClick
                    {
                        Tilte = _inputClickTitle?.Text ?? "Click",  // <-- THÊM
                        X = float.TryParse(_inputClickX?.Text, out var cx) ? cx : 50,
                        Y = float.TryParse(_inputClickY?.Text, out var cy) ? cy : 50,
                        NumClicks = int.TryParse(_inputClickCount?.Text, out var cc) ? cc : 1,
                        DelayBetweenMs = int.TryParse(_inputClickDelay?.Text, out var cd) ? cd : 300,
                        IsPerCent = _switchClickIsPercent?.Checked ?? true
                    }
                },

                "Swipe" => new AoaLoop
                {
                    Type = AoaType.Swipe,
                    Payload = new AoaSwipe
                    {
                        Tilte = _inputSwipeTitle?.Text ?? "Swipe",  // <-- THÊM
                        X1 = float.TryParse(_inputSwipeX1?.Text, out var s1) ? s1 : 50,
                        Y1 = float.TryParse(_inputSwipeY1?.Text, out var s2) ? s2 : 80,
                        X2 = float.TryParse(_inputSwipeX2?.Text, out var s3) ? s3 : 50,
                        Y2 = float.TryParse(_inputSwipeY2?.Text, out var s4) ? s4 : 40,
                        DurationMs = int.TryParse(_inputSwipeDuration?.Text, out var s5) ? s5 : 300,
                        NumSwipe = int.TryParse(_inputSwipeCount?.Text, out var s6) ? s6 : 1,
                        IsPerCent = _switchSwipeIsPercent?.Checked ?? true  // ===== SỬA =====
                    }
                },

                "KeyPress" => new AoaLoop
                {
                    Type = AoaType.KeyPress,
                    Payload = new AoaKeyPress
                    {
                        Key = _selectKeyPress?.SelectedValue?.ToString() ?? "home",
                        Repeat = int.TryParse(_inputKeyRepeat?.Text, out var k1) ? k1 : 1,
                        DelayBetweenMs = int.TryParse(_inputKeyDelay?.Text, out var k2) ? k2 : 200
                    }
                },

                "Deeplink" => new AoaLoop
                {
                    Type = AoaType.Deeplink,
                    Payload = new AoaDeeplink
                    {
                        Url = _inputDeeplinkUrl?.Text ?? "settings",
                        WaitAfterMs = int.TryParse(_inputDeeplinkWait?.Text, out var d1) ? d1 : 2000
                    }
                },

                "SendText" => new AoaLoop
                {
                    Type = AoaType.SendText,
                    Payload = new AoaSendText
                    {
                        Text = _inputSendText?.Text ?? "",
                        DelayPerCharMs = int.TryParse(_inputSendDelay?.Text, out var t1) ? t1 : 50
                    }
                },

                "Ocr" => new AoaLoop
                {
                    Type = AoaType.Ocr,
                    Payload = new AoaOcr
                    {
                        Title = _inputOcrTitle?.Text ?? "OCR",  // <-- THÊM
                        Keywords = _rtbOcrKeywords?.Lines
                        .Where(line => !string.IsNullOrWhiteSpace(line))
                        .Select(line => line.Trim())
                        .ToArray() ?? new[] { "Settings" },
                        TimeoutMs = int.TryParse(_inputOcrTimeout?.Text, out var o1) ? o1 : 5000,
                        IntervalMs = int.TryParse(_inputOcrInterval?.Text, out var o2) ? o2 : 500,
                        MaxSwipes = int.TryParse(_inputOcrMaxSwipes?.Text, out var o3) ? o3 : 5,
                        SwipeStartX = float.TryParse(_inputOcrSwipeX1?.Text, out var sx1) ? sx1 : 50,
                        SwipeStartY = float.TryParse(_inputOcrSwipeY1?.Text, out var sy1) ? sy1 : 80,
                        SwipeEndX = float.TryParse(_inputOcrSwipeX2?.Text, out var sx2) ? sx2 : 50,
                        SwipeEndY = float.TryParse(_inputOcrSwipeY2?.Text, out var sy2) ? sy2 : 40,
                        SwipeDurationMs = int.TryParse(_inputOcrSwipeDuration?.Text, out var sd) ? sd : 300,
                        SwipeIsPercent = true,
                        OffsetX = int.TryParse(_inputOcrOffsetX?.Text, out var o4) ? o4 : 0,
                        OffsetY = int.TryParse(_inputOcrOffsetY?.Text, out var o5) ? o5 : 0
                    }
                },

                "Delay" => new AoaLoop
                {
                    Type = AoaType.Delay,
                    Payload = int.TryParse(_inputDelayMs?.Text, out var dm) ? dm : 1000
                },

                _ => null
            };
        }

        private void LoadParamsFromLoop(AoaLoop loop)
        {
            loop.HydratePayload();

            if (loop.Type == AoaType.CloseAllApps)
            {
                if (_inputClosePoint != null) _inputClosePoint.Text = loop.PointCloseApp;
                return;
            }


            switch (loop.Payload)
            {
                case AoaClick c:
                    if (_inputClickTitle != null) _inputClickTitle.Text = c.Tilte;  // <-- THÊM
                    if (_inputClickX != null) _inputClickX.Text = c.X.ToString();
                    if (_inputClickY != null) _inputClickY.Text = c.Y.ToString();
                    if (_inputClickCount != null) _inputClickCount.Text = c.NumClicks.ToString();
                    if (_inputClickDelay != null) _inputClickDelay.Text = c.DelayBetweenMs.ToString();
                    break;

                case AoaSwipe s:
                    if (_inputSwipeTitle != null) _inputSwipeTitle.Text = s.Tilte;  // <-- THÊM
                    if (_inputSwipeX1 != null) _inputSwipeX1.Text = s.X1.ToString();
                    if (_inputSwipeY1 != null) _inputSwipeY1.Text = s.Y1.ToString();
                    if (_inputSwipeX2 != null) _inputSwipeX2.Text = s.X2.ToString();
                    if (_inputSwipeY2 != null) _inputSwipeY2.Text = s.Y2.ToString();
                    if (_inputSwipeDuration != null) _inputSwipeDuration.Text = s.DurationMs.ToString();
                    if (_inputSwipeCount != null) _inputSwipeCount.Text = s.NumSwipe.ToString();
                    break;

                case AoaKeyPress k:
                    // Set Select value
                    if (_selectKeyPress != null)
                        _selectKeyPress.SelectedValue = k.Key;
                    if (_inputKeyRepeat != null) _inputKeyRepeat.Text = k.Repeat.ToString();
                    if (_inputKeyDelay != null) _inputKeyDelay.Text = k.DelayBetweenMs.ToString();
                    break;

                case AoaDeeplink d:
                    if (_inputDeeplinkUrl != null) _inputDeeplinkUrl.Text = d.Url;
                    if (_inputDeeplinkWait != null) _inputDeeplinkWait.Text = d.WaitAfterMs.ToString();
                    break;

                case AoaSendText t:
                    if (_inputSendText != null) _inputSendText.Text = t.Text;
                    if (_inputSendDelay != null) _inputSendDelay.Text = t.DelayPerCharMs.ToString();
                    break;

                case AoaOcr o:
                    if (_inputOcrTitle != null) _inputOcrTitle.Text = o.Title;  // <-- THÊM
                    if (_rtbOcrKeywords != null)
                        _rtbOcrKeywords.Text = string.Join(Environment.NewLine, o.Keywords);
                    if (_inputOcrTimeout != null) _inputOcrTimeout.Text = o.TimeoutMs.ToString();
                    if (_inputOcrInterval != null) _inputOcrInterval.Text = o.IntervalMs.ToString();
                    if (_inputOcrMaxSwipes != null) _inputOcrMaxSwipes.Text = o.MaxSwipes.ToString();
                    // ===== THÊM =====
                    if (_inputOcrSwipeX1 != null) _inputOcrSwipeX1.Text = o.SwipeStartX.ToString();
                    if (_inputOcrSwipeY1 != null) _inputOcrSwipeY1.Text = o.SwipeStartY.ToString();
                    if (_inputOcrSwipeX2 != null) _inputOcrSwipeX2.Text = o.SwipeEndX.ToString();
                    if (_inputOcrSwipeY2 != null) _inputOcrSwipeY2.Text = o.SwipeEndY.ToString();
                    if (_inputOcrSwipeDuration != null) _inputOcrSwipeDuration.Text = o.SwipeDurationMs.ToString();
                    if (_inputOcrOffsetX != null) _inputOcrOffsetX.Text = o.OffsetX.ToString();
                    if (_inputOcrOffsetY != null) _inputOcrOffsetY.Text = o.OffsetY.ToString();
                    break;

                case int delay:
                    if (_inputDelayMs != null) _inputDelayMs.Text = delay.ToString();
                    break;
            }
        }

        private async Task SaveCurrentLoop()
        {
            if (_currentLoop == null || _phone == null) return;

            _currentLoop.PhoneModel = _phone.Model;
            _currentLoop.ProcVersion = _phone.ProcVersion;
            _currentLoop.ProcCpuInfo = _phone.ProcCpuInfo;
            _currentLoop.API = _phone.API;
            _currentLoop.PhysicalWidth = _phone.PhysicalWidth;
            _currentLoop.PhysicalHeight = _phone.PhysicalHeight;
            _currentLoop.IsAppCaptureConnected = _isCaptureConnected;

            string currentPointClose = _currentLoop.PointCloseApp;

            bool saved = AoaLoopRepository.Save(_currentLoop);

            if (saved)
            {
                // Update PointCloseApp cho loop còn lại
                var otherLoop = AoaLoopRepository.FindOneByUnique(
                    _phone.Model, _phone.ProcVersion, _phone.ProcCpuInfo, _phone.API, !_isCaptureConnected);

                if (otherLoop != null)
                {
                    otherLoop.PointCloseApp = currentPointClose;
                    AoaLoopRepository.Update(otherLoop);
                }

                labelFile.Text = $"✅ Saved successfully! (Capture={_isCaptureConnected})";
                AntdUI.Message.info(this, "Saved successfully!");
            }
            else
            {
                labelFile.Text = "❌ Save failed!";
                AntdUI.Message.info(this, "Save failed!");
            }
        }
        private void DeleteSelectedNode()
        {
            if (_selectedNode == null)
            {
                AntdUI.Modal.open(this, "Delete Node", "Please select a node to delete!", TType.Warn);
                return;
            }

            var nodeText = _selectedNode.Text;

            var result = AntdUI.Modal.open(
                this,
                "Confirm Delete",
                $"Delete:\n\n'{nodeText}'?\n\nThis action cannot be undone.",
                TType.Warn);

            if (result == DialogResult.OK)
            {
                var loopToDelete = _selectedNode.Tag as AoaLoop;
                if (loopToDelete == null) return;

                // ===== 1. XÓA KHỎI DATA MODEL =====
                var parentLoop = FindParentLoop(_selectedNode);
                if (parentLoop?.Children != null)
                {
                    parentLoop.Children.Remove(loopToDelete);
                }

                // ===== 2. LƯU VỊ TRÍ SCROLL =====
                int scrollY = treeActions.ScrollBar.ValueY;

                // ===== 3. REBUILD TREE TỪ DATA MODEL =====
                RebuildTree();

                // ===== 4. KHÔI PHỤC SCROLL =====
                treeActions.ScrollBar.ValueY = Math.Min(scrollY, treeActions.ScrollBar.MaxY);

                _selectedNode = null;
                labelSelectedNode.Text = "Selected: None";
                labelFile.Text = $"🗑️ Deleted: {nodeText}";
            }
        }
        private void ApplyCurrentParamsToSelectedNode()
        {
            if (_selectedNode == null) return;

            var loop = _selectedNode.Tag as AoaLoop;
            if (loop == null) return;

            var typeStr = selectActionType.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(typeStr)) return;

            var updatedLoop = CreateLoopFromParams(typeStr);
            if (updatedLoop == null) return;

            // Copy giá trị mới vào loop cũ
            loop.Type = updatedLoop.Type;
            loop.Payload = updatedLoop.Payload;
            loop.PointCloseApp = updatedLoop.PointCloseApp;

            // Cập nhật text trên tree
            int idx = GetNodeIndex(_selectedNode);
            string icon = loop.Type switch
            {
                AoaType.CloseAllApps => "🧹",
                AoaType.Click => "👆",
                AoaType.Swipe => "👈",
                AoaType.KeyPress => "⌨",
                AoaType.Deeplink => "🔗",
                AoaType.SendText => "📝",
                AoaType.Ocr => "🔍",
                AoaType.Delay => "⏱",
                _ => "📌"
            };

            string summary = GetLoopSummary(loop); // <-- Gọi hàm đã sửa ở trên

            _selectedNode.Text = $"{idx}. {icon} [{loop.Type}] {summary}";
            _selectedNode.Text = _selectedNode.Text.Replace("\uFE0F", "").Replace("\u200D", "");

            labelFile.Text = $"✅ Updated: {summary}";
        }
        private int GetNodeIndex(AntdUI.TreeItem node)
        {
            if (node == null) return 0;

            var parent = node.ParentItem;

            // Nếu là root node (không có parent)
            if (parent == null)
            {
                if (treeActions.Items.Contains(node))
                    return treeActions.Items.IndexOf(node) + 1;
                return 0;
            }

            // Tìm trong sub của parent
            if (parent.Sub != null && parent.Sub.Contains(node))
                return parent.Sub.IndexOf(node) + 1;

            return 0;
        }
        #endregion

        #region PointCloseApp
        // Trong FormOTGLoopEditor, khi save PointCloseApp:
        private void SavePointCloseAppForAllModes(string pointCloseApp)
        {
            if (_phone == null) return;

            // Tìm cả 2 loop (capture=true và capture=false)
            var loopWithCapture = AoaLoopRepository.FindOneByUnique(
                _phone.Model, _phone.ProcVersion, _phone.ProcCpuInfo, _phone.API, true);
            var loopWithoutCapture = AoaLoopRepository.FindOneByUnique(
                _phone.Model, _phone.ProcVersion, _phone.ProcCpuInfo, _phone.API, false);

            // Update cả 2
            if (loopWithCapture != null)
            {
                loopWithCapture.PointCloseApp = pointCloseApp;
                AoaLoopRepository.Update(loopWithCapture);
            }

            if (loopWithoutCapture != null)
            {
                loopWithoutCapture.PointCloseApp = pointCloseApp;
                AoaLoopRepository.Update(loopWithoutCapture);
            }
        }
        // Lấy từ loop capture=true (ưu tiên) hoặc loop capture=false
        public static string GetPointCloseApp(Phone phone)
        {
            // Ưu tiên loop có capture=true trước
            var loop = AoaLoopRepository.FindOneByUnique(
                phone.Model, phone.ProcVersion, phone.ProcCpuInfo, phone.API, true);

            if (loop != null && !string.IsNullOrEmpty(loop.PointCloseApp))
                return loop.PointCloseApp;

            // Fallback: loop capture=false
            loop = AoaLoopRepository.FindOneByUnique(
                phone.Model, phone.ProcVersion, phone.ProcCpuInfo, phone.API, false);

            if (loop != null && !string.IsNullOrEmpty(loop.PointCloseApp))
                return loop.PointCloseApp;

            // Default
            return "50,90";
        }

        #endregion
        #region Form Drag

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

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

        #endregion
    }
}