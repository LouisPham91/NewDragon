using AntdUI;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl;
using Dragon.Controller.DeviceControl.OTG;
using Dragon.Controller.DeviceControl.OTG.Loop;
using Dragon.Controller.DeviceControl.OTG.Model;
using Dragon.Database.Models;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.ColorMode;
using Dragon.DesignView.Public.NormalMode;

namespace Dragon.DesignView.FormUI
{
    public partial class FormAoaLoopEditor : FormCopy
    {
        private Phone? _phone;
        private AoaLoop? _currentLoop;
        private List<AoaLoop> _allLoops = new();

        // Tree
        private AntdUI.Tree? _treeActions;
        private AntdUI.TreeItem? _selectedNode;
        private AntdUI.TreeItem? _draggedNode;
        private bool _isDragging;
        private Point _dragStartPoint;
        private System.Windows.Forms.Timer? _dragTimer;

        // Select
        private AntdUI.Select? _selectActionType;
        private AntdUI.Select? _selectPhoneModel;

        // Panel params (System.Windows.Forms.Panel)
        private System.Windows.Forms.Panel? _panelParams;

        // Inputs
        private AntdUI.Input? _inputClickX, _inputClickY, _inputClickCount, _inputClickDelay;
        private AntdUI.Input? _inputSwipeX1, _inputSwipeY1, _inputSwipeX2, _inputSwipeY2, _inputSwipeDuration, _inputSwipeCount;
        private AntdUI.Input? _inputDelayMs;
        private AntdUI.Input? _inputKeyPress, _inputKeyRepeat, _inputKeyDelay;
        private AntdUI.Input? _inputDeeplinkUrl, _inputDeeplinkWait;
        private AntdUI.Input? _inputSendText, _inputSendDelay;
        private AntdUI.Input? _inputOcrKeywords, _inputOcrTimeout, _inputOcrInterval, _inputOcrMaxSwipes, _inputOcrOffsetX, _inputOcrOffsetY;
        private AntdUI.Input? _inputClosePoint;

        // Labels
        private LabelNormalN? _labelPhoneInfo;
        private LabelNormalN? _labelSelectedNode;

        // Buttons
        private ButtomFlatRound? _btnAddNode;
        private ButtomFlatRound? _btnDeleteNode;
        private ButtomFlatRound? _btnSave;
        private ButtomFlatRound? _btnLoad;
        private ButtomFlatRound? _btnTest;
        private ButtomFlatRound? _btnNewTemplate;

        public FormAoaLoopEditor(Phone? phone = null)
        {
            _phone = phone;

            // Khởi tạo tất cả field nullable
            _treeActions = null!;
            _dragTimer = null!;
            _selectActionType = null!;
            _selectPhoneModel = null!;
            _panelParams = null!;
            _inputClickX = _inputClickY = _inputClickCount = _inputClickDelay = null!;
            _inputSwipeX1 = _inputSwipeY1 = _inputSwipeX2 = _inputSwipeY2 = _inputSwipeDuration = _inputSwipeCount = null!;
            _inputDelayMs = null!;
            _inputKeyPress = _inputKeyRepeat = _inputKeyDelay = null!;
            _inputDeeplinkUrl = _inputDeeplinkWait = null!;
            _inputSendText = _inputSendDelay = null!;
            _inputOcrKeywords = _inputOcrTimeout = _inputOcrInterval = _inputOcrMaxSwipes = _inputOcrOffsetX = _inputOcrOffsetY = null!;
            _inputClosePoint = null!;
            _labelPhoneInfo = null!;
            _labelSelectedNode = null!;
            _btnAddNode = _btnDeleteNode = _btnSave = _btnLoad = _btnTest = _btnNewTemplate = null!;

            this.KeyPreview = true;
            this.KeyDown += FormAoaLoopEditor_KeyDown;

            // Xóa controls cũ của FormCopy nếu cần (chỉ giữ lại panelMain, panel1, panelRoundn1)
            ClearFormCopyContent();

            BuildUI();

            if (_phone != null)
            {
                LoadLoopsForPhone();
            }

            LoadAllPhoneModels();
        }

        private void ClearFormCopyContent()
        {
            // FormCopy có: panelMain chứa panelRoundn1 + panel1
            // panel1 chứa textBoxSearch, pictureBoxBrightn1, PictureBoxCloseForm, labelNormaln1
            // panelRoundn1 chứa buttomRound1, buttonExportFile, labelFile
            // Ta xóa nội dung cũ và build lại
            // labelNormaln1.Text = "AOA Loop Editor"; // sửa title

            // Xóa event cũ của PictureBoxCloseForm (giữ nguyên control)
            if (PictureBoxCloseForm != null)
            {
                PictureBoxCloseForm.Click -= PictureBoxCloseForm_Click;
                PictureBoxCloseForm.Click += PictureBoxCloseForm_Click;
            }

            // Xóa buttonExportFile và buttomRound1 khỏi panelRoundn1
            if (buttonExportFile != null && panelRoundn1 != null)
            {
                panelRoundn1.Controls.Remove(buttonExportFile);
                buttonExportFile.Click -= buttonExportFile_Click;
            }
            if (buttomRound1 != null && panelRoundn1 != null)
            {
                panelRoundn1.Controls.Remove(buttomRound1);
                buttomRound1.Click -= buttomRound1_Click;
            }
            if (labelFile != null && panelRoundn1 != null)
            {
                panelRoundn1.Controls.Remove(labelFile);
            }
        }

        private void FormAoaLoopEditor_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                _ = SaveCurrentLoop();
                e.Handled = true;
            }
        }

        #region Build UI

        private void BuildUI()
        {
            // Title
            if (labelNormaln1 != null)
            {
                labelNormaln1.Text = "AOA Loop Editor";
                labelNormaln1.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            }

            // ===== LEFT PANEL =====
            var leftPanel = new System.Windows.Forms.Panel
            {
                Dock = DockStyle.Left,
                Width = 280,
                BackColor = Color.Transparent,
                Location = new Point(0, 45)
            };

            // Phone Info Label
            _labelPhoneInfo = new LabelNormalN
            {
                Location = new Point(10, 10),
                Size = new Size(260, 40),
                Text = _phone != null
                    ? $"Model: {_phone.Model}\nAPI: {_phone.API}\nCPU: {_phone.ProcCpuInfo}"
                    : "No phone selected",
                DG_IsBrightBack = true,
                Font = new Font("Segoe UI", 9f)
            };
            leftPanel.Controls.Add(_labelPhoneInfo);

            // Select Phone Model
            var lblModel = new LabelNormalN
            {
                Location = new Point(10, 55),
                Size = new Size(100, 20),
                Text = "Phone Model:",
                DG_IsBrightBack = true,
                Font = new Font("Segoe UI", 8.5f)
            };
            leftPanel.Controls.Add(lblModel);

            _selectPhoneModel = new AntdUI.Select
            {
                Location = new Point(10, 75),
                Size = new Size(260, 34),
                PlaceholderText = "Select or type model...",
                List = true,
                ListAutoWidth = true
            };
            // AntdUI.Select dùng Items là AntdUI.BaseCollection
            _selectPhoneModel.SelectedValueChanged += (s, e) => SelectPhoneModel_Changed(s, e);
            leftPanel.Controls.Add(_selectPhoneModel);

            // Load button
            _btnLoad = new ButtomFlatRound
            {
                Location = new Point(10, 120),
                Size = new Size(120, 30),
                Text = "Load Loops",
                DG_BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnLoad.Click += BtnLoad_Click;
            leftPanel.Controls.Add(_btnLoad);

            // New Template button
            _btnNewTemplate = new ButtomFlatRound
            {
                Location = new Point(140, 120),
                Size = new Size(130, 30),
                Text = "New Template",
                DG_BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnNewTemplate.Click += BtnNewTemplate_Click;
            leftPanel.Controls.Add(_btnNewTemplate);

            // Separator
            var sep1 = new System.Windows.Forms.Label
            {
                Location = new Point(10, 160),
                Size = new Size(260, 2),
                BackColor = Color.Gray,
                Text = ""
            };
            leftPanel.Controls.Add(sep1);

            // Selected node info
            _labelSelectedNode = new LabelNormalN
            {
                Location = new Point(10, 170),
                Size = new Size(260, 20),
                Text = "Selected: None",
                DG_IsBrightBack = true,
                Font = new Font("Segoe UI", 8.5f)
            };
            leftPanel.Controls.Add(_labelSelectedNode);

            // Add / Delete buttons
            _btnAddNode = new ButtomFlatRound
            {
                Location = new Point(10, 200),
                Size = new Size(80, 30),
                Text = "+ Add",
                DG_BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnAddNode.Click += BtnAddNode_Click;
            leftPanel.Controls.Add(_btnAddNode);

            _btnDeleteNode = new ButtomFlatRound
            {
                Location = new Point(100, 200),
                Size = new Size(80, 30),
                Text = "- Delete",
                DG_BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnDeleteNode.Click += BtnDeleteNode_Click;
            leftPanel.Controls.Add(_btnDeleteNode);

            // ===== CENTER: Tree =====
            _treeActions = new AntdUI.Tree
            {
                Location = new Point(290, 45),
                Size = new Size(350, this.ClientSize.Height - 160),
                Font = new Font("Segoe UI", 9.5f),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };
            _treeActions.NodeMouseClick += TreeActions_NodeMouseClick;
            _treeActions.MouseDown += TreeActions_MouseDown;
            _treeActions.MouseMove += TreeActions_MouseMove;
            _treeActions.MouseUp += TreeActions_MouseUp;

            // ===== RIGHT: Action Type + Params =====
            var rightPanel = new System.Windows.Forms.Panel
            {
                Location = new Point(650, 45),
                Size = new Size(280, this.ClientSize.Height - 160),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.Transparent
            };

            var lblActionType = new LabelNormalN
            {
                Location = new Point(10, 10),
                Size = new Size(100, 20),
                Text = "Action Type:",
                DG_IsBrightBack = true,
                Font = new Font("Segoe UI", 8.5f)
            };
            rightPanel.Controls.Add(lblActionType);

            _selectActionType = new AntdUI.Select
            {
                Location = new Point(10, 30),
                Size = new Size(260, 34),
                PlaceholderText = "Select action type...",
                List = true,
                ListAutoWidth = true
            };
            // Thêm items vào AntdUI.Select
            var actionItems = new AntdUI.SelectItem[]
            {
                new("CloseAllApps", "🧹 Close All Apps"),
                new("Click", "👆 Click"),
                new("Swipe", "👈 Swipe"),
                new("KeyPress", "⌨️ Key Press"),
                new("Deeplink", "🔗 Deeplink"),
                new("SendText", "📝 Send Text"),
                new("OCR", "🔍 OCR (Find & Click)"),
                new("Delay", "⏱️ Delay (ms)")
            };
            foreach (var item in actionItems)
                _selectActionType.Items.Add(item);

            _selectActionType.SelectedValueChanged += (s, e) => SelectActionType_Changed(s, e);
            rightPanel.Controls.Add(_selectActionType);

            // Panel params
            _panelParams = new System.Windows.Forms.Panel
            {
                Location = new Point(10, 75),
                Size = new Size(260, 350),
                BackColor = Color.Transparent,
                AutoScroll = true
            };
            rightPanel.Controls.Add(_panelParams);

            // Add controls vào panelMain
            if (panelMain != null)
            {
                panelMain.Controls.Add(leftPanel);
                panelMain.Controls.Add(_treeActions);
                panelMain.Controls.Add(rightPanel);
            }

            // ===== BOTTOM BUTTONS trong panelRoundn1 =====
            _btnSave = new ButtomFlatRound
            {
                Location = new Point(10, 60),
                Size = new Size(120, 30),
                Text = "💾 Save",
                DG_BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnSave.Click += BtnSave_Click;
            if (panelRoundn1 != null)
                panelRoundn1.Controls.Add(_btnSave);

            _btnTest = new ButtomFlatRound
            {
                Location = new Point(140, 60),
                Size = new Size(120, 30),
                Text = "▶ Test Run",
                DG_BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnTest.Click += BtnTest_Click;
            if (panelRoundn1 != null)
                panelRoundn1.Controls.Add(_btnTest);

            // Resize
            this.Resize += (s, e) =>
            {
                if (_treeActions != null)
                    _treeActions.Height = this.ClientSize.Height - 160;
                if (rightPanel != null)
                    rightPanel.Height = this.ClientSize.Height - 160;
            };
        }

        private void BuildParamsPanel(string actionType)
        {
            if (_panelParams == null) return;
            _panelParams.Controls.Clear();
            int y = 10;

            switch (actionType)
            {
                case "CloseAllApps":
                    AddLabelInput(_panelParams, "Close Point (x,y):", ref _inputClosePoint, "945,1437", ref y);
                    break;
                case "Click":
                    AddLabelInput(_panelParams, "X:", ref _inputClickX, "540", ref y);
                    AddLabelInput(_panelParams, "Y:", ref _inputClickY, "960", ref y);
                    AddLabelInput(_panelParams, "Count:", ref _inputClickCount, "1", ref y);
                    AddLabelInput(_panelParams, "Delay (ms):", ref _inputClickDelay, "300", ref y);
                    break;
                case "Swipe":
                    AddLabelInput(_panelParams, "Start X:", ref _inputSwipeX1, "540", ref y);
                    AddLabelInput(_panelParams, "Start Y:", ref _inputSwipeY1, "1500", ref y);
                    AddLabelInput(_panelParams, "End X:", ref _inputSwipeX2, "540", ref y);
                    AddLabelInput(_panelParams, "End Y:", ref _inputSwipeY2, "400", ref y);
                    AddLabelInput(_panelParams, "Duration (ms):", ref _inputSwipeDuration, "300", ref y);
                    AddLabelInput(_panelParams, "Count:", ref _inputSwipeCount, "1", ref y);
                    break;
                case "KeyPress":
                    AddLabelInput(_panelParams, "Key:", ref _inputKeyPress, "home", ref y);
                    AddLabelInput(_panelParams, "Repeat:", ref _inputKeyRepeat, "1", ref y);
                    AddLabelInput(_panelParams, "Delay (ms):", ref _inputKeyDelay, "200", ref y);
                    break;
                case "Deeplink":
                    AddLabelInput(_panelParams, "URL:", ref _inputDeeplinkUrl, "settings", ref y);
                    AddLabelInput(_panelParams, "Wait (ms):", ref _inputDeeplinkWait, "2000", ref y);
                    break;
                case "SendText":
                    AddLabelInput(_panelParams, "Text:", ref _inputSendText, "", ref y);
                    AddLabelInput(_panelParams, "Delay/Char:", ref _inputSendDelay, "50", ref y);
                    break;
                case "OCR":
                    AddLabelInput(_panelParams, "Keywords (comma):", ref _inputOcrKeywords, "Settings,About", ref y);
                    AddLabelInput(_panelParams, "Timeout (ms):", ref _inputOcrTimeout, "5000", ref y);
                    AddLabelInput(_panelParams, "Interval (ms):", ref _inputOcrInterval, "500", ref y);
                    AddLabelInput(_panelParams, "Max Swipes:", ref _inputOcrMaxSwipes, "3", ref y);
                    AddLabelInput(_panelParams, "Offset X:", ref _inputOcrOffsetX, "0", ref y);
                    AddLabelInput(_panelParams, "Offset Y:", ref _inputOcrOffsetY, "0", ref y);
                    break;
                case "Delay":
                    AddLabelInput(_panelParams, "Delay (ms):", ref _inputDelayMs, "1000", ref y);
                    break;
            }
        }

        private void AddLabelInput(System.Windows.Forms.Panel parent, string labelText, ref AntdUI.Input? input, string defaultValue, ref int y)
        {
            var lbl = new System.Windows.Forms.Label
            {
                Location = new Point(0, y),
                Size = new Size(260, 18),
                Text = labelText,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8f)
            };
            parent.Controls.Add(lbl);
            y += 20;

            input = new AntdUI.Input
            {
                Location = new Point(0, y),
                Size = new Size(250, 28),
                Text = defaultValue,
                Font = new Font("Segoe UI", 9f)
            };
            parent.Controls.Add(input);
            y += 35;
        }

        #endregion

        #region Event Handlers

        private void SelectPhoneModel_Changed(object? sender, AntdUI.ObjectNEventArgs? e)
        {
            var selected = _selectPhoneModel?.SelectedValue;
            var model = selected?.ToString();
            if (!string.IsNullOrEmpty(model))
            {
                // Dùng LoadAll thay vì FindManyByPhoneModel
                var phones = PhoneRepository.LoadAll()
                    .Where(p => p.Model.Equals(model, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                var phone = phones.FirstOrDefault();
                if (phone != null)
                {
                    _phone = phone;
                    if (_labelPhoneInfo != null)
                        _labelPhoneInfo.Text = $"Model: {phone.Model}\nAPI: {phone.API}\nCPU: {phone.ProcCpuInfo}";
                    LoadLoopsForPhone();
                }
            }
        }

        private void SelectActionType_Changed(object? sender, AntdUI.ObjectNEventArgs? e)
        {
            var selected = _selectActionType?.SelectedValue;
            var type = selected?.ToString();
            if (!string.IsNullOrEmpty(type))
            {
                BuildParamsPanel(type);
            }
        }

        private void TreeActions_NodeMouseClick(object? sender, AntdUI.TreeSelectEventArgs e)
        {
            _selectedNode = e.Item;
            var loop = e.Item?.Tag as AoaLoop;
            if (loop != null && _labelSelectedNode != null)
            {
                _labelSelectedNode.Text = $"Selected: {loop.Type}";
                if (_selectActionType != null)
                    _selectActionType.SelectedValue = loop.Type.ToString();
                BuildParamsPanel(loop.Type.ToString());
                LoadParamsFromLoop(loop);
            }
        }

        private void TreeActions_MouseDown(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _dragStartPoint = e.Location;
                _dragTimer?.Stop();
                _dragTimer = new System.Windows.Forms.Timer { Interval = 400 };
                _dragTimer.Tick += (s, ev) =>
                {
                    _dragTimer?.Stop();
                    _isDragging = true;
                };
                _dragTimer.Start();
            }
        }

        private void TreeActions_MouseMove(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Drag visual feedback - TODO
        }

        private void TreeActions_MouseUp(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            _dragTimer?.Stop();
            _isDragging = false;
        }

        private void BtnAddNode_Click(object? sender, EventArgs e)
        {
            var typeStr = _selectActionType?.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(typeStr)) return;

            var newLoop = CreateLoopFromParams(typeStr);
            if (newLoop == null) return;

            var newNode = new AntdUI.TreeItem($"{typeStr} {GetLoopSummary(newLoop)}")
            {
                Tag = newLoop
            };

            if (_selectedNode != null)
            {
                _selectedNode.Sub.Add(newNode);
            }
            else if (_treeActions != null)
            {
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
                _currentLoop.Children.Add(newLoop);

                if (_treeActions.Items.Count == 0)
                {
                    var rootNode = new AntdUI.TreeItem($"Root: {_currentLoop.PhoneModel}")
                    {
                        Tag = _currentLoop
                    };
                    _treeActions.Items.Add(rootNode);
                }

                newNode = new AntdUI.TreeItem($"{typeStr} {GetLoopSummary(newLoop)}")
                {
                    Tag = newLoop
                };
                _treeActions.Items[0].Sub.Add(newNode);
            }

            _treeActions?.Invalidate();
        }

        private void BtnDeleteNode_Click(object? sender, EventArgs e)
        {
            if (_selectedNode == null) return;
            _selectedNode.Remove();
            _selectedNode = null;
            if (_labelSelectedNode != null)
                _labelSelectedNode.Text = "Selected: None";
            _treeActions?.Invalidate();
        }

        private void BtnLoad_Click(object? sender, EventArgs e)
        {
            LoadLoopsForPhone();
        }

        private void BtnNewTemplate_Click(object? sender, EventArgs e)
        {
            if (_phone == null)
            {
                AntdUI.Message.info(this, "Please select a phone model first!");
                return;
            }

            // Đổi CreateDefaultForPhone thành public trong AoaLoopMatcher
            _currentLoop = AoaLoopMatcher.CreateDefaultForPhone(_phone);
            if (_currentLoop != null)
            {
                PopulateTreeFromLoop(_currentLoop);
            }
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            await SaveCurrentLoop();
        }

        private async void BtnTest_Click(object? sender, EventArgs e)
        {
            if (_currentLoop == null || _phone == null) return;

            UpdateLoopFromTree();
            _currentLoop.HydrateTree();

            try
            {
                var session = await AoaDeviceManager.Instance.StartByDeviceIdAsync(_phone.DeviceID);
                if (session == null)
                {
                    AntdUI.Message.info(this, "Cannot open AOA session!");
                    return;
                }

                var capture = AppCaptureManager.Instance.GetByDeviceId(_phone.DeviceID);
                using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));

                if (_btnTest != null)
                {
                    _btnTest.Text = "⏳ Running...";
                    _btnTest.Enabled = false;
                }

                await Task.Run(async () =>
                {
                    await AoaLoopRunner.RunAsync(_currentLoop, session.ctrl, capture, cts.Token);
                });

                AntdUI.Message.info(this, "Test completed!");
            }
            catch (Exception ex)
            {
                AntdUI.Message.info(this, $"Error: {ex.Message}");
            }
            finally
            {
                if (_btnTest != null)
                {
                    _btnTest.Text = "▶ Test Run";
                    _btnTest.Enabled = true;
                }
            }
        }

        #endregion

        #region Data Methods

        private void LoadLoopsForPhone()
        {
            if (_phone == null) return;
            if (_treeActions == null) return;

            _allLoops = AoaLoopRepository.FindByPhoneModel(_phone.Model);

            _treeActions.Items.Clear();

            if (_allLoops.Count > 0)
            {
                _currentLoop = _allLoops.FirstOrDefault(
                    l => l.API == _phone.API && l.ProcCpuInfo == _phone.ProcCpuInfo)
                    ?? _allLoops.First();

                _currentLoop.HydrateTree();
                PopulateTreeFromLoop(_currentLoop);
            }
            else
            {
                _currentLoop = AoaLoopMatcher.CreateDefaultForPhone(_phone);
                if (_currentLoop != null)
                {
                    PopulateTreeFromLoop(_currentLoop);
                }
            }
        }

        private void LoadAllPhoneModels()
        {
            if (_selectPhoneModel == null) return;

            var phones = PhoneRepository.LoadAll();
            var models = phones.Select(p => p.Model).Distinct().OrderBy(m => m).ToList();

            _selectPhoneModel.Items.Clear();
            foreach (var model in models)
            {
                _selectPhoneModel.Items.Add(new AntdUI.SelectItem(model, model));
            }
        }

        private void PopulateTreeFromLoop(AoaLoop loop)
        {
            if (_treeActions == null) return;
            _treeActions.Items.Clear();

            var rootNode = new AntdUI.TreeItem($"📱 {loop.PhoneModel} (API {loop.API})")
            {
                Tag = loop
            };

            if (loop.Children != null)
            {
                foreach (var child in loop.Children)
                {
                    AddNodeToTree(rootNode, child);
                }
            }

            _treeActions.Items.Add(rootNode);
            _treeActions.ExpandAll();
        }

        private void AddNodeToTree(AntdUI.TreeItem parent, AoaLoop loop)
        {
            string icon = loop.Type switch
            {
                AoaType.CloseAllApps => "🧹",
                AoaType.Click => "👆",
                AoaType.Swipe => "👈",
                AoaType.KeyPress => "⌨️",
                AoaType.Deeplink => "🔗",
                AoaType.SendText => "📝",
                AoaType.Ocr => "🔍",
                AoaType.Delay => "⏱️",
                _ => "📌"
            };

            var node = new AntdUI.TreeItem($"{icon} {loop.Type}: {GetLoopSummary(loop)}")
            {
                Tag = loop
            };

            if (loop.Children != null)
            {
                foreach (var child in loop.Children)
                {
                    AddNodeToTree(node, child);
                }
            }

            parent.Sub.Add(node);
        }

        private string GetLoopSummary(AoaLoop loop)
        {
            loop.HydratePayload();

            return loop.Payload switch
            {
                AoaClick c => $"({c.X},{c.Y}) x{c.NumClicks}",
                AoaSwipe s => $"({s.X1},{s.Y1})→({s.X2},{s.Y2})",
                AoaKeyPress k => $"{k.Key} x{k.Repeat}",
                AoaDeeplink d => d.Url,
                AoaSendText t => t.Text.Length > 20 ? t.Text[..20] + "..." : t.Text,
                AoaOcr o => string.Join(",", o.Keywords),
                int delay => $"{delay}ms",
                string point when loop.Type == AoaType.CloseAllApps => $"Point: {point}",
                _ => ""
            };
        }

        private AoaLoop? CreateLoopFromParams(string typeStr)
        {
            return typeStr switch
            {
                "CloseAllApps" => new AoaLoop
                {
                    Type = AoaType.CloseAllApps,
                    Payload = null,
                },
                "Click" => new AoaLoop
                {
                    Type = AoaType.Click,
                    Payload = new AoaClick
                    {
                        X = int.TryParse(_inputClickX?.Text, out var cx) ? cx : 540,
                        Y = int.TryParse(_inputClickY?.Text, out var cy) ? cy : 960,
                        NumClicks = int.TryParse(_inputClickCount?.Text, out var cc) ? cc : 1,
                        DelayBetweenMs = int.TryParse(_inputClickDelay?.Text, out var cd) ? cd : 300
                    }
                },
                "Swipe" => new AoaLoop
                {
                    Type = AoaType.Swipe,
                    Payload = new AoaSwipe
                    {
                        X1 = int.TryParse(_inputSwipeX1?.Text, out var sx1) ? sx1 : 540,
                        Y1 = int.TryParse(_inputSwipeY1?.Text, out var sy1) ? sy1 : 1500,
                        X2 = int.TryParse(_inputSwipeX2?.Text, out var sx2) ? sx2 : 540,
                        Y2 = int.TryParse(_inputSwipeY2?.Text, out var sy2) ? sy2 : 400,
                        DurationMs = int.TryParse(_inputSwipeDuration?.Text, out var sd) ? sd : 300,
                        NumSwipe = int.TryParse(_inputSwipeCount?.Text, out var sc) ? sc : 1
                    }
                },
                "KeyPress" => new AoaLoop
                {
                    Type = AoaType.KeyPress,
                    Payload = new AoaKeyPress
                    {
                        Key = _inputKeyPress?.Text ?? "home",
                        Repeat = int.TryParse(_inputKeyRepeat?.Text, out var kr) ? kr : 1,
                        DelayBetweenMs = int.TryParse(_inputKeyDelay?.Text, out var kd) ? kd : 200
                    }
                },
                "Deeplink" => new AoaLoop
                {
                    Type = AoaType.Deeplink,
                    Payload = new AoaDeeplink
                    {
                        Url = _inputDeeplinkUrl?.Text ?? "settings",
                        WaitAfterMs = int.TryParse(_inputDeeplinkWait?.Text, out var dw) ? dw : 2000
                    }
                },
                "SendText" => new AoaLoop
                {
                    Type = AoaType.SendText,
                    Payload = new AoaSendText
                    {
                        Text = _inputSendText?.Text ?? "",
                        DelayPerCharMs = int.TryParse(_inputSendDelay?.Text, out var sd2) ? sd2 : 50
                    }
                },
                "OCR" => new AoaLoop
                {
                    Type = AoaType.Ocr,
                    Payload = new AoaOcr
                    {
                        Keywords = _inputOcrKeywords?.Text?.Split(',') ?? new[] { "Settings" },
                        TimeoutMs = int.TryParse(_inputOcrTimeout?.Text, out var ot) ? ot : 5000,
                        IntervalMs = int.TryParse(_inputOcrInterval?.Text, out var oi) ? oi : 500,
                        MaxSwipes = int.TryParse(_inputOcrMaxSwipes?.Text, out var om) ? om : 3,
                        OffsetX = int.TryParse(_inputOcrOffsetX?.Text, out var ox) ? ox : 0,
                        OffsetY = int.TryParse(_inputOcrOffsetY?.Text, out var oy) ? oy : 0
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

            switch (loop.Payload)
            {
                case AoaClick c:
                    if (_inputClickX != null) _inputClickX.Text = c.X.ToString();
                    if (_inputClickY != null) _inputClickY.Text = c.Y.ToString();
                    if (_inputClickCount != null) _inputClickCount.Text = c.NumClicks.ToString();
                    if (_inputClickDelay != null) _inputClickDelay.Text = c.DelayBetweenMs.ToString();
                    break;
                case AoaSwipe s:
                    if (_inputSwipeX1 != null) _inputSwipeX1.Text = s.X1.ToString();
                    if (_inputSwipeY1 != null) _inputSwipeY1.Text = s.Y1.ToString();
                    if (_inputSwipeX2 != null) _inputSwipeX2.Text = s.X2.ToString();
                    if (_inputSwipeY2 != null) _inputSwipeY2.Text = s.Y2.ToString();
                    if (_inputSwipeDuration != null) _inputSwipeDuration.Text = s.DurationMs.ToString();
                    if (_inputSwipeCount != null) _inputSwipeCount.Text = s.NumSwipe.ToString();
                    break;
                case AoaKeyPress k:
                    if (_inputKeyPress != null) _inputKeyPress.Text = k.Key;
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
                    if (_inputOcrKeywords != null) _inputOcrKeywords.Text = string.Join(",", o.Keywords);
                    if (_inputOcrTimeout != null) _inputOcrTimeout.Text = o.TimeoutMs.ToString();
                    if (_inputOcrInterval != null) _inputOcrInterval.Text = o.IntervalMs.ToString();
                    if (_inputOcrMaxSwipes != null) _inputOcrMaxSwipes.Text = o.MaxSwipes.ToString();
                    if (_inputOcrOffsetX != null) _inputOcrOffsetX.Text = o.OffsetX.ToString();
                    if (_inputOcrOffsetY != null) _inputOcrOffsetY.Text = o.OffsetY.ToString();
                    break;
                case int delay:
                    if (_inputDelayMs != null) _inputDelayMs.Text = delay.ToString();
                    break;
            }
        }

        private void UpdateLoopFromTree()
        {
            if (_treeActions == null || _treeActions.Items.Count == 0) return;

            var rootNode = _treeActions.Items[0];
            var rootLoop = rootNode.Tag as AoaLoop;
            if (rootLoop == null) return;

            rootLoop.Children = new List<AoaLoop>();
            foreach (AntdUI.TreeItem child in rootNode.Sub)
            {
                rootLoop.Children.Add(ExtractLoopFromNode(child));
            }

            _currentLoop = rootLoop;
        }

        private AoaLoop ExtractLoopFromNode(AntdUI.TreeItem node)
        {
            var loop = node.Tag as AoaLoop ?? new AoaLoop();

            loop.Children = new List<AoaLoop>();
            foreach (AntdUI.TreeItem child in node.Sub)
            {
                loop.Children.Add(ExtractLoopFromNode(child));
            }

            return loop;
        }

        private async Task SaveCurrentLoop()
        {
            if (_currentLoop == null || _phone == null) return;

            UpdateLoopFromTree();

            _currentLoop.PhoneModel = _phone.Model;
            _currentLoop.ProcVersion = _phone.ProcVersion;
            _currentLoop.ProcCpuInfo = _phone.ProcCpuInfo;
            _currentLoop.API = _phone.API;
            _currentLoop.PointCloseApp = _inputClosePoint?.Text ?? "945,1437";

            bool saved = AoaLoopRepository.Save(_currentLoop);

            if (saved)
            {
                AntdUI.Message.info(this, "Saved successfully!");
            }
            else
            {
                AntdUI.Message.info(this, "Save failed!");
            }
        }

        #endregion

        #region Theme

        public new void ApplyTheme()
        {
            base.ApplyTheme();

            if (_treeActions != null)
            {
                _treeActions.BackColor = ThemeHelper.BackNormalFirst;
                _treeActions.ForeColor = ThemeHelper.ForeNormalFirst;
            }

            if (_panelParams != null)
            {
                _panelParams.BackColor = Color.Transparent;
            }
        }

        #endregion
    }
}