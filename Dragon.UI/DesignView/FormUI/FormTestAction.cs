using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using Dragon.ControlHelper.ScrcpyNet.DesignDevice;
using Dragon.Controller.DeviceControl.HATX;
using Dragon.Controller.GlobalControl.TaskDeviceManager.Runners;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Infrastructure.Services;
using Dragon.Controller.TaskDeviceManager.Model.App;
using Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn;
using Dragon.Controller.TaskDeviceManager.Model.Emoji;
using Dragon.Controller.TaskDeviceManager.Model.File;
using Dragon.Controller.TaskDeviceManager.Model.HttpResponse;
using Dragon.Controller.TaskDeviceManager.Model.Input;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Controller.TaskDeviceManager.Model.Vision;
using Dragon.Controller.TaskDeviceManager.Runners;
using Dragon.Database.Models;
using Dragon.Database.Services;
using Dragon.DesignView.Public;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Dragon.DesignView.FormUI
{
    public partial class FormTestAction : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        Phone? phone = null;
        private AtxDevice? Atx;
        PanelDevice? panelDevice;
        public string SaveTextValue = string.Empty;
        public FormKeybroadSetting? formKeybroadSetting = null;
        SaveNode? saveText;
        DeviceData? deviceData;
        AdbClient adbClient = new AdbClient();
        private float _roundRadius = 15f;
        private Color _borderColor = Color.FromArgb(168, 168, 168);
        
        public FormTestAction(Phone phone, DeviceData? deviceData = null, AtxDevice? Atx = null, PanelDevice? panelDevice = null)
        {
            this.phone = phone;
            this.deviceData = deviceData;
            this.Atx = Atx;
            this.panelDevice = panelDevice;

            label_Reaction_DeviceModel.Text = "Model : " + phone.Model;

            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            EnableFormDrag(panel1);

            
            
            ApplyTheme();

            LoadingMouseSwipeDirection();

            // Lấy tất cả giá trị enum và add vào ComboBox
            // AOT-safe: dùng generic overload
            combox_KeyBroad_HTTP_CompareType_Result.DataSource = EnumCache.CompareTypes;
            combox_KeyBroad_HTTP_CompareType_RegexNextRequest.DataSource = EnumCache.CompareTypes;
            combox_HTTP_UseRegexMode.DataSource = EnumCache.UseRegexModes;
            combox_InputText_TypeInput.DataSource = EnumCache.TypeOptions;
            combox_HTTP_TypingOption.DataSource = EnumCache.TypeOptions;
            combox_GetDatabase_TypeInput.DataSource = EnumCache.TypeOptions;
            combobox_Database_ReadText_Language.DataSource = EnumCache.Languages;

            combox_KeyEvent_Common_Command.DataSource = KeyCodeService.KeyIntentDict.Keys.ToList();
            combox_GetDatabase_Column.DataSource = AppDataServices.GetColumnAppData();
            combox_Database_Column_SaveData.DataSource = AppDataServices.GetColumnAppData();


            LoadSocialNetwork();
        }


        public void ApplyTheme()
        {
            

            BackColor = ThemeHelper.BorderNormalFist;
            ForeColor = ThemeHelper.ForeNormalFirst;

            panelMain.BackColor = ThemeHelper.BackNormalFirst;
            panelMain.ForeColor = ThemeHelper.ForeNormalFirst;

            panel1.BackColor = ThemeHelper.BackNormalFirst;
            PictureBoxCloseForm.ApplyTheme();

            groupBox1.BackColor = ThemeHelper.BackNormalFirst;
            groupBox1.ForeColor = ThemeHelper.ForeNormalFirst;
            groupBox2.BackColor = ThemeHelper.BackNormalFirst;
            groupBox2.ForeColor = ThemeHelper.ForeNormalFirst;
            groupBox3.BackColor = ThemeHelper.BackNormalFirst;
            groupBox3.ForeColor = ThemeHelper.ForeNormalFirst;
            groupBox4.BackColor = ThemeHelper.BackNormalFirst;
            groupBox4.ForeColor = ThemeHelper.ForeNormalFirst;
            groupBox5.BackColor = ThemeHelper.BackNormalFirst;
            groupBox5.ForeColor = ThemeHelper.ForeNormalFirst;
            groupBox6.BackColor = ThemeHelper.BackNormalFirst;
            groupBox6.ForeColor = ThemeHelper.ForeNormalFirst;

            txt_Mouse_Click_Point.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Click_Point.ForeColor = ThemeHelper.ForeNormalFirst;


            txt_Mouse_Drag_Duration.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Drag_Duration.ForeColor = ThemeHelper.ForeNormalFirst;
            txt_Mouse_Drag_PointA.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Drag_PointA.ForeColor = ThemeHelper.ForeNormalFirst;
            txt_Mouse_Drag_PointB.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Drag_PointB.ForeColor = ThemeHelper.ForeNormalFirst;
            txt_Mouse_Drag_PointC.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Drag_PointC.ForeColor = ThemeHelper.ForeNormalFirst;


            txt_LongPress_Duration.BackColor = ThemeHelper.BackNormalFirst;
            txt_LongPress_Duration.ForeColor = ThemeHelper.ForeNormalFirst;
            txt_Mouse_LongPress_Point.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_LongPress_Point.ForeColor = ThemeHelper.ForeNormalFirst;

            txt_Mouse_Swipe_Left.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Swipe_Left.ForeColor = ThemeHelper.ForeNormalFirst;
            txt_Mouse_Swipe_Right.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Swipe_Right.ForeColor = ThemeHelper.ForeNormalFirst;
            txt_Mouse_Swipe_Top.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Swipe_Top.ForeColor = ThemeHelper.ForeNormalFirst;
            txt_Mouse_Swipe_Bottom.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Swipe_Bottom.ForeColor = ThemeHelper.ForeNormalFirst;
            txt_Mouse_Swipe_Duration.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Swipe_Duration.ForeColor = ThemeHelper.ForeNormalFirst;
            txt_Mouse_Swipe_LoopTime.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Swipe_LoopTime.ForeColor = ThemeHelper.ForeNormalFirst;
            txt_Mouse_Swipe_Min.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Swipe_Min.ForeColor = ThemeHelper.ForeNormalFirst;
            txt_Mouse_Swipe_Max.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Swipe_Max.ForeColor = ThemeHelper.ForeNormalFirst;
            txt_Mouse_Swipe_PixelPerStep.BackColor = ThemeHelper.BackNormalFirst;
            txt_Mouse_Swipe_PixelPerStep.ForeColor = ThemeHelper.ForeNormalFirst;

            richTextBox1.BackColor = ThemeHelper.BackNormalFirst;
            richTextBox1.ForeColor = ThemeHelper.ForeNormalFirst;


            combox_MouseSwipe_Direction.ApplyTheme();

            combox_InputText_TypeInput.ApplyTheme();


            combox_KeyBroad_HTTP_CompareType_Result.ApplyTheme();
            combox_KeyBroad_HTTP_CompareType_RegexNextRequest.ApplyTheme();
            combox_HTTP_UseRegexMode.ApplyTheme();
            combox_HTTP_TypingOption.ApplyTheme();


            combox_KeyEvent_Common_Command.ApplyTheme();

            combox_Database_ListNode.ApplyTheme();
            combox_Database_Column_SaveData.ApplyTheme();
            combobox_Database_ReadText_Language.ApplyTheme();
            combox_SetColumnData_SocialNetwork.ApplyTheme();


            ComboxAppPackage_Other.ApplyTheme();
            ComboxStartApp_ChoosePackage.ApplyTheme();


            Combox_ReactionType.ApplyTheme();
            Combox_ReactionTemplate.ApplyTheme();
            Combox_Reaction_Social_Network.ApplyTheme();
            Combox_Reaction_NoteImage.ApplyTheme();
            Combox_ReactionType.ApplyTheme();

            combox_GetDatabase_TypeInput.ApplyTheme();
            combox_GetDatabase_Column.ApplyTheme();
            combox_GetDatabase_SocialNetwork.ApplyTheme();


        }
        private void FormAndroidExplorer_Load(object sender, EventArgs e)
        {

        }

        #region  FormKeybroadSetting

        private void buttomKeybroadSetting_Click(object sender, EventArgs e)
        {
            if (phone == null)
            {
                MessageBox.Show("Phone is null");
                return;
            }
            formKeybroadSetting = new FormKeybroadSetting(phone);
            formKeybroadSetting.StartPosition = FormStartPosition.CenterScreen;
            formKeybroadSetting.Show();
        }

        #endregion

        #region Support Begin No Edit
        private void EnableFormDrag(Control ctrl)
        {
            if (ctrl == null) return;

            ctrl.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, 0xA1, 2, 0); // WM_NCLBUTTONDOWN
                    Console.WriteLine($"Dragging from {ctrl.Name}");
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
            using (var path = GetRoundedRectanglePath(ClientRectangle, _roundRadius))
            {
                var region = new Region(path);
                var old = Region;

                Region = region;
                old?.Dispose();
            }
        }
        private void SetPanelMainRoundedRegion()
        {
            using (var path = GetRoundedRectanglePath(panelMain.ClientRectangle, _roundRadius))
            {
                var region = new Region(path);
                var old = panelMain.Region;

                panelMain.Region = region;
                old?.Dispose();
            }
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
        public Size originalSize;
        public Point originalLocation;
        public bool isMaximized = false;
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
        private void PictureBoxCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion



        #region SUPPORT 

        private static readonly Regex PercentRegex = new Regex(@"^(100(\.0+)?|[0-9]{1,2}(\.\d+)?)$", RegexOptions.CultureInvariant);

        private bool CheckPercentInput(string input, Control control)
        {
            bool isValid = PercentRegex.IsMatch(input.Trim());

            control.BackColor = isValid ? Color.DarkGreen : Color.DarkRed;
            return isValid; // <- trước bạn return true cả 2 nhánh
        }

        private static readonly Regex TwoPercentRegex = new Regex(@"^(?<x>100(\.0+)?|[0-9]{1,2}(\.\d+)?),(?<y>100(\.0+)?|[0-9]{1,2}(\.\d+)?)$", RegexOptions.CultureInvariant);

        private (bool isValid, float x, float y) CheckTwoPercentValues(string input, Control control)
        {
            var match = TwoPercentRegex.Match(input.Trim());

            if (match.Success &&
                float.TryParse(match.Groups["x"].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float x) &&
                float.TryParse(match.Groups["y"].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float y))
            {
                control.BackColor = Color.DarkGreen;
                return (true, x, y);
            }

            control.BackColor = Color.DarkRed;
            return (false, 0f, 0f);
        }

        private static readonly Regex NumberOnlyRegex = new Regex(@"^\d+$", RegexOptions.CultureInvariant);

        private bool CheckNumberOnly(string input, Control control)
        {
            bool isValid = NumberOnlyRegex.IsMatch(input);

            control.BackColor = isValid ? Color.DarkGreen : Color.DarkRed;
            return isValid; // đúng như bạn note: false khi không hợp lệ
        }


        #endregion

        #region MOUSE TEST

        private void txt_X_top_TextChanged(object sender, EventArgs e)
        {
            string text = txt_Mouse_Swipe_Left.Text.Trim();
            CheckPercentInput(text, txt_Mouse_Swipe_Left);
        }

        private void txt_Y_top_TextChanged(object sender, EventArgs e)
        {
            string text = txt_Mouse_Swipe_Right.Text.Trim();
            CheckPercentInput(text, txt_Mouse_Swipe_Right);
        }

        private void txt_X_bottom_TextChanged(object sender, EventArgs e)
        {
            string text = txt_Mouse_Swipe_Top.Text.Trim();
            CheckPercentInput(text, txt_Mouse_Swipe_Top);
        }

        private void txt_Y_bottom_TextChanged(object sender, EventArgs e)
        {
            string text = txt_Mouse_Swipe_Bottom.Text.Trim();
            CheckPercentInput(text, txt_Mouse_Swipe_Bottom);
        }

        private void txt_Duration_TextChanged(object sender, EventArgs e)
        {
            string text = txt_Mouse_Swipe_Duration.Text.Trim();
            CheckNumberOnly(text, txt_Mouse_Swipe_Duration);
        }

        private void txt_Mouse_Swipe_ATX_Text_TextChanged(object sender, EventArgs e)
        {

        }

        public void LoadingMouseSwipeDirection()
        {
            var directions = Enum.GetValues<Direction>().ToList();
            combox_MouseSwipe_Direction.DataSource = directions;
            combox_MouseSwipe_Direction.SelectedIndex = 0;
        }

        private async void buttomMouse_TEST_Click(object sender, EventArgs e)
        {
            ControlMode mouseType = ControlMode.ADB;
            NodeType NodeType = NodeType.Title;

            if (radioMouse_ADBEvent.Checked)
            {
                mouseType = ControlMode.ADBEvent;
            }
            else if (radioMouse_ATX.Checked)
            {
                mouseType = ControlMode.ATX;
            }
            else if (radioMouse_Scrcpy.Checked)
            {
                mouseType = ControlMode.Scrcpy;
            }
            else if (radioMouse_ADB.Checked)
            {
                mouseType = ControlMode.ADB;
            }
            else if (radioMouse_UHDI.Checked)
            {
                mouseType = ControlMode.HDI;
            }
            else if (radioMouse_ACC.Checked)
            {
                mouseType = ControlMode.ACC;
            }

            if (radioMouse_Click.Checked)
            {
                NodeType = NodeType.Click;
            }
            else if (radioMouse_LongPress.Checked)
            {
                NodeType = NodeType.LongPress;
            }
            else if (radioMouse_Swipe.Checked)
            {
                NodeType = NodeType.Swipe;
            }
            else if (radioMouse_DragDrop.Checked)
            {
                NodeType = NodeType.DragDrop;
            }


            if (NodeType == NodeType.Title)
            {
                MessageBox.Show("Please choose Action Type");
                return;
            }
            if (mouseType == ControlMode.HDI && phone != null && !phone.IsUHDI)
            {
                MessageBox.Show("This Device Doesnt Support UHDI Or OTP Mode");
                return;
            }
            await RunMouseTest(mouseType, NodeType);
        }
        public ClickArg? GetClickValue(ControlMode mouseType)
        {
            var pointXText = txt_Mouse_Click_Point.Text.Trim();
            var isDouble = chkIsMouseDoubleClick.Checked;
            var pointResult = CheckTwoPercentValues(pointXText, txt_Mouse_Click_Point);

            // Nếu không hợp lệ hoặc duration không hợp lệ thì báo lỗi
            if (!pointResult.isValid)
            {
                MessageBox.Show("Please enter the correct format for all fields");
                return null;
            }

            float pointX = pointResult.x;
            float pointY = pointResult.y;

            ClickArg click = new ClickArg
            {
                ControlMode = mouseType,
                ClickMode = isDouble ? ClickMode.DoubleClick : ClickMode.SingleClick,
                x = pointX,
                y = pointY
            };
            return click;
        }
        public LongPressArg? GetLongPress(ControlMode mouseType)
        {
            var pointText = txt_Mouse_LongPress_Point.Text.Trim();
            var durationText = txt_LongPress_Duration.Text.Trim();

            // Gọi hàm check 2 giá trị
            var pointResult = CheckTwoPercentValues(pointText, txt_Mouse_LongPress_Point);

            // Nếu không hợp lệ hoặc duration không hợp lệ thì báo lỗi
            if (!pointResult.isValid || !CheckNumberOnly(durationText, txt_LongPress_Duration))
            {
                MessageBox.Show("Please enter the correct format for all fields");
                return null;
            }

            float pointX = pointResult.x;
            float pointY = pointResult.y;
            int duration = int.Parse(durationText);

            LongPressArg longPress = new LongPressArg
            {
                ControlMode = mouseType,
                x = pointX,
                y = pointY,
                Duration = duration
            };
            return longPress;
        }
        public SwipeArg? GetMouseSwipe(ControlMode mouseType)
        {
            SwipeMode swipeMode = SwipeMode.Fixed;
            if (radioFixelLoop.Checked)
                swipeMode = SwipeMode.Fixed;
            if (radioRandomLoop.Checked)
                swipeMode = SwipeMode.Random;
            if (combox_MouseSwipe_Direction.SelectedItem == null)
            {
                MessageBox.Show("Please choose Swipe Direction");
                return null;
            }
            var direction = (Direction)combox_MouseSwipe_Direction.SelectedItem;

            string loopTimeText = txt_Mouse_Swipe_LoopTime.Text.Trim();
            string fromMin = txt_Mouse_Swipe_Min.Text.Trim();
            string toMax = txt_Mouse_Swipe_Max.Text.Trim();

            string leftText = txt_Mouse_Swipe_Left.Text.Trim();
            string rightText = txt_Mouse_Swipe_Right.Text.Trim();
            string topText = txt_Mouse_Swipe_Top.Text.Trim();
            string bottomText = txt_Mouse_Swipe_Bottom.Text.Trim();
            string durationText = txt_Mouse_Swipe_Duration.Text.Trim();
            string pixelPerStep = txt_Mouse_Swipe_PixelPerStep.Text.Trim();
            string delayPerStep = txt_Mouse_Swipe_DelayPerStep.Text.Trim();
            string uhdiMoveDelay = txt_Mouse_UHDI_Move_Delay.Text.Trim();
            if (!CheckPercentInput(leftText, txt_Mouse_Swipe_Left) ||
                !CheckPercentInput(rightText, txt_Mouse_Swipe_Right) ||
                !CheckPercentInput(topText, txt_Mouse_Swipe_Top) ||
                !CheckPercentInput(bottomText, txt_Mouse_Swipe_Bottom) ||
                !CheckNumberOnly(durationText, txt_Mouse_Swipe_Duration) ||
                !CheckNumberOnly(loopTimeText, txt_Mouse_Swipe_LoopTime) ||
                !CheckNumberOnly(fromMin, txt_Mouse_Swipe_Min) ||
                !CheckNumberOnly(toMax, txt_Mouse_Swipe_Max) ||
                !CheckNumberOnly(pixelPerStep, txt_Mouse_Swipe_PixelPerStep) ||
                !CheckNumberOnly(pixelPerStep, txt_Mouse_Swipe_DelayPerStep) ||
                 !CheckNumberOnly(uhdiMoveDelay, txt_Mouse_UHDI_Move_Delay)
            )
            {
                MessageBox.Show("Please enter the correct format for all fields");
                return null;
            }
            // Chuyển đổi giá trị từ TextBox
            float left = float.Parse(leftText);
            float right = float.Parse(rightText);
            float top = float.Parse(topText);
            float bottom = float.Parse(bottomText);
            int duration = int.Parse(durationText);

            int loopTime = int.Parse(loopTimeText);
            int randMin = int.Parse(fromMin);
            int randMax = int.Parse(toMax);
            int pixelperstep = int.Parse(pixelPerStep);
            int delayperStep = int.Parse(delayPerStep);
            int uhdimoveDelay = int.Parse(uhdiMoveDelay);
            SwipeArg swipe = new SwipeArg
            {
                ControlMode = mouseType,
                SwipeMode = swipeMode,
                Direction = direction,
                duration = duration,
                Left = left,
                Right = right,
                Top = top,
                Bottom = bottom,
                randMin = randMin,
                randMax = randMax,
                loopTime = loopTime,
                PixelPerStep = pixelperstep,
                DelayStep = delayperStep,
                UHDIMoveDelay = uhdimoveDelay
            };
            return swipe;
        }
        public DragArg? GetMouseDragDrop(ControlMode mouseType)
        {
            var pointAText = txt_Mouse_Drag_PointA.Text.Trim();
            var pointBText = txt_Mouse_Drag_PointB.Text.Trim();
            var pointCText = txt_Mouse_Drag_PointC.Text.Trim();
            var durationText = txt_Mouse_Drag_Duration.Text.Trim();
            var delayPerStep = txt_Mouse_DRAG_DelayPerStep.Text.Trim();
            var pixelPerStep = txt_Mouse_DRAG_PixelPerStep.Text.Trim();

            // Kiểm tra từng point
            var pointAResult = CheckTwoPercentValues(pointAText, txt_Mouse_Drag_PointA);
            var pointBResult = CheckTwoPercentValues(pointBText, txt_Mouse_Drag_PointB);
            var pointCResult = CheckTwoPercentValues(pointCText, txt_Mouse_Drag_PointC);

            // Kiểm tra duration
            bool durationValid = CheckNumberOnly(durationText, txt_Mouse_Drag_Duration);
            bool delayStepVaild = CheckNumberOnly(delayPerStep, txt_Mouse_DRAG_DelayPerStep);
            bool pixelSetpValid = CheckNumberOnly(pixelPerStep, txt_Mouse_DRAG_PixelPerStep);

            if (!pointAResult.isValid || !pointBResult.isValid || !pointCResult.isValid || !durationValid || !delayStepVaild || !pixelSetpValid)
            {
                MessageBox.Show("Please enter the correct format for all fields");
                return null;
            }

            // Parse ra các Point
            Point pointA = new Point((int)pointAResult.x, (int)pointAResult.y);
            Point pointB = new Point((int)pointBResult.x, (int)pointBResult.y);
            Point pointC = new Point((int)pointCResult.x, (int)pointCResult.y);

            int duration = int.Parse(durationText);
            int delay = int.Parse(delayPerStep);
            int pixel = int.Parse(pixelPerStep);

            DragArg drag = new DragArg
            {
                ControlMode = mouseType,
                Points = new List<Point> { pointA, pointB, pointC },
                Duration = duration,
                DelayPerStep = delay,
                PixelsPerStep = pixel,
            };

            return drag;
        }
        private object? ResolveMouseAction(ControlMode mouseType, NodeType NodeType)
        {
            switch (NodeType)
            {
                case NodeType.Click: return GetClickValue(mouseType);
                case NodeType.LongPress: return GetLongPress(mouseType);
                case NodeType.Swipe: return GetMouseSwipe(mouseType);
                case NodeType.DragDrop: return GetMouseDragDrop(mouseType);
                default: return null;
            }
        }

        public async Task RunMouseTest(ControlMode mouseType, NodeType NodeType)
        {
            var action = ResolveMouseAction(mouseType, NodeType);
            if (action == null)
            {
                MessageBox.Show("Invalid or missing action data");
                return;
            }
            if (phone == null)
            {
                MessageBox.Show("phone is null");
                return;
            }
            if (deviceData == null)
            {
                MessageBox.Show("DeviceData not found");
                return;
            }
            if (Atx == null)
            {
                MessageBox.Show("ATX not found");
                return;
            }
            if (phone == null)
            {
                MessageBox.Show("phone not found");
                return;
            }
            var session = new PhoneSession(phone, adbClient, deviceData, Atx);
            var ctx = new DLoopContext
            {
                Session = session,
                Token = session.Cts.Token
            };

            if (mouseType == ControlMode.ADBEvent && phone != null && deviceData != null)
            {
                if (NodeType == NodeType.Click)
                {
                    await ADBEventServices.Click(ctx, (ClickArg)action);
                }
                else if (NodeType == NodeType.LongPress)
                {
                    await ADBEventServices.LongPress(ctx, (LongPressArg)action);
                }
                else if (NodeType == NodeType.Swipe)
                {
                    await ADBEventServices.Swipe(ctx, (SwipeArg)action);
                }
                else if (NodeType == NodeType.DragDrop)
                {
                    await ADBEventServices.DragAndDrop(ctx, (DragArg)action);
                }
            }
            else if (mouseType == ControlMode.ADB && phone != null && deviceData != null)
            {
                if (NodeType == NodeType.Click)
                {
                    await ADBMouseServices.Click(ctx, (ClickArg)action);
                }
                else if (NodeType == NodeType.LongPress)
                {
                    await ADBMouseServices.LongPress(ctx, (LongPressArg)action);
                }
                else if (NodeType == NodeType.Swipe)
                {
                    await ADBMouseServices.Swipe(ctx, (SwipeArg)action);
                }
                else if (NodeType == NodeType.DragDrop)
                {
                    // bỏ MessageBox (sẽ treo farm), dùng Logger
                    Logger.Notify(phone.DeviceID, "ADB không hỗ trợ DragDrop, hãy dùng ADBEvent", Logger.Icon.Warning);
                }
            }
            else if (mouseType == ControlMode.ATX && Atx != null && phone != null)
            {
                if (NodeType == NodeType.Click)
                {
                    await Atx.ClickAsync(ctx, (ClickArg)action);
                }
                else if (NodeType == NodeType.LongPress)
                {
                    await Atx.LongPressAsync(ctx, (LongPressArg)action);
                }
                else if (NodeType == NodeType.Swipe)
                {
                    await Atx.SwipeAsync(ctx, (SwipeArg)action);
                }
                else if (NodeType == NodeType.DragDrop)
                {
                    await Atx.DragAndDropAsync(ctx, (DragArg)action);
                }
            }
            //else if (mouseType == ControlMode.Scrcpy && panelDevice?._mouseEventHandler != null && phone != null)
            //{
            //    if (NodeType == NodeType.Click)
            //    {
            //        await panelDevice._mouseEventHandler.Click(ctx, (ClickArg)action);
            //    }
            //    else if (NodeType == NodeType.LongPress)
            //    {
            //        await panelDevice._mouseEventHandler.LongPress(ctx, (LongPressArg)action);
            //    }
            //    else if (NodeType == NodeType.Swipe)
            //    {
            //        await panelDevice._mouseEventHandler.Swipe(ctx, (SwipeArg)action);
            //    }
            //    else if (NodeType == NodeType.DragDrop)
            //    {
            //        await panelDevice._mouseEventHandler.DragAndDrop(ctx, (DragArg)action);
            //    }
            //}
            //else if (mouseType == ControlMode.UHDI && panelDevice?._uhidMouseFakeEventHandle != null && phone != null)
            //{
            //    if (NodeType == NodeType.Click)
            //    {
            //        await panelDevice._uhidMouseFakeEventHandle.Click(ctx, (ClickArg)action);
            //    }
            //    else if (NodeType == NodeType.LongPress)
            //    {
            //        await panelDevice._uhidMouseFakeEventHandle.LongPress(ctx, (LongPressArg)action);
            //    }
            //    else if (NodeType == NodeType.Swipe)
            //    {
            //        await panelDevice._uhidMouseFakeEventHandle.Swipe(ctx, (SwipeArg)action);
            //    }
            //    else if (NodeType == NodeType.DragDrop)
            //    {
            //        await panelDevice._uhidMouseFakeEventHandle.DragAndDrop(ctx, (DragArg)action);
            //    }
            //}
            else if (mouseType == ControlMode.ACC)
            {
                MessageBox.Show("ACC mouse type not implemented yet");
            }
            else
            {
                MessageBox.Show("MouseType not supported or handler missing");
            }
        }

        private void buttomMouse_AutoALL_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region  TEST Input Text

        public string Get_Input_Text()
        {
            var text = txt_InputText_Text.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("Please enter text to send");
                return string.Empty;
            }
            return text;
        }
        public string Get_InputText_ListRandom()
        {
            var text = txt_InputText_RandomText.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("Please enter text to send");
                return string.Empty;
            }
            var list = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (list != null && list.Count() > 0)
            {
                var rand = new Random().Next(0, list.Count() - 1);
                return list[rand];
            }
            else
            {
                return string.Empty;
            }

        }
        private string? ResolveText_InputText(InputTextType textType)
        {
            if (textType == InputTextType.InputText)
                return Get_Input_Text();

            if (textType == InputTextType.Random)
                return Get_InputText_ListRandom();

            if (textType == InputTextType.SaveText)
            {
                var saveText = Save_Database_Text();
                if (saveText == null) return null;
                this.saveText = saveText;
                var SaveTextValue = saveText.Text ?? saveText.ContentDescription;

                return SaveTextValue;
            }


            return null;
        }
        private async void buttom_InputText_Test_Click(object sender, EventArgs e)
        {

            ControlMode ControlMode = ControlMode.ADB;
            InputTextType textType = InputTextType.None;

            if (radio_InputText_ADB.Checked)
            {
                ControlMode = ControlMode.ADB;
            }
            else if (radio_InputText_Scrcpy.Checked)
            {
                ControlMode = ControlMode.Scrcpy;
            }
            else if (radio_InputText_ATX.Checked)
            {
                ControlMode = ControlMode.ATX;
            }
            else if (radio_InputText_UHDI.Checked)
            {
                ControlMode = ControlMode.HDI;
            }
            else if (radio_InputText_ACC.Checked)
            {
                ControlMode = ControlMode.ACC;
            }

            if (radio_InputText.Checked)
            {
                textType = InputTextType.InputText;
            }
            else if (radio_InputText_RandomText.Checked)
            {
                textType = InputTextType.Random;
            }

            if (ControlMode == ControlMode.ADB || textType == InputTextType.None)
            {
                MessageBox.Show("Please choose Keybroad Type and Text Type");
                return;
            }

            if (string.IsNullOrEmpty(combox_InputText_TypeInput.Text))
            {
                MessageBox.Show("Please choose Type Input");
                return;
            }

            TypeOption typeOption;
            if (!Enum.TryParse<TypeOption>(combox_InputText_TypeInput.SelectedItem?.ToString(), out typeOption))
            {
                MessageBox.Show("Invalid TypeInput selection");
            }

            var text = ResolveText_InputText(textType);
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("Random text or Text input empty");
                return;
            }

            SendTextArgs inputTextInfo = new SendTextArgs
            {
                Text = text,
                TypeOption = typeOption,
                InputTextType = textType,
                ControlMode = ControlMode
            };

            if (deviceData == null)
            {
                MessageBox.Show("DeviceData not found");
                return;
            }
            if (Atx == null)
            {
                MessageBox.Show("ATX not found");
                return;
            }
            if (phone == null)
            {
                MessageBox.Show("phone not found");
                return;
            }
            var session = new PhoneSession(phone, adbClient, deviceData, Atx);
            var ctx = new DLoopContext
            {
                Session = session,
                Token = session.Cts.Token,
            };

            await InputRunner.Run_InputText_Test(ctx, inputTextInfo, 2000);



        }
        #endregion


        #region KeyPressArgs

        public KeyPressArgs? GetIntentCommand()
        {
            ControlMode ControlMode;
            if (radio_KeyEvent_ADB.Checked)
            {
                ControlMode = ControlMode.ADB;
            }
            else if (radio_KeyEvent_ATX.Checked)
            {
                ControlMode = ControlMode.ATX;
            }
            else if (radio_KeyEvent_ACC.Checked)
            {
                ControlMode = ControlMode.ACC;
            }
            else
            {
                MessageBox.Show("Please choose Intent Command Type");
                return null;
            }

            var commandText = txt_KeyEvent_Command.Text.Trim();
            if (string.IsNullOrEmpty(commandText))
            {
                MessageBox.Show("Please enter Intent Command");
                return null;
            }
            string nameText = string.Empty;

            bool IsCustom = false;
            if (radio_KeyEvent_Custom.Checked)
            {
                nameText = txt__KeyEvent__Name.Text.Trim();

                IsCustom = true;
            }
            else if (radio_KeyEvent_Common.Checked)
            {
                nameText = combox_KeyEvent_Common_Command.Text.Trim();
                IsCustom = false;
            }

            if (string.IsNullOrEmpty(nameText))
            {
                MessageBox.Show("Please enter Intent Name");
                return null;
            }

            KeyPressArgs intentCommand = new KeyPressArgs
            {
                Command = commandText,
                Name = nameText,
                ControlMode = ControlMode,
                IsCustom = IsCustom
            };

            return intentCommand;

        }
        private async void buttomTestEven_Click(object sender, EventArgs e)
        {
            var keyPressArgs = GetIntentCommand();
            if (keyPressArgs == null) return;
            if (deviceData == null)
            {
                MessageBox.Show("DeviceData not found");
                return;
            }
            if (Atx == null)
            {
                MessageBox.Show("ATX not found");
                return;
            }
            if (phone == null)
            {
                MessageBox.Show("phone not found");
                return;
            }
            var adbClient = new AdbClient();
            var session = new PhoneSession(phone, adbClient, deviceData, Atx);
            var ctx = new DLoopContext
            {
                Session = session,
                Token = session.Cts.Token,
            };

            await IntentRunner.ExecutePressKeyAsync(ctx, keyPressArgs);
        }

        #endregion

        #region CopyFile

        public FileArgs? GetCopyFileInfo()
        {
            if (phone == null)
            {
                MessageBox.Show("Phone not found");
                return null;
            }
            bool hasSu = chkSuperSu.Checked;

            ControlMode TypeRun;
            if (radioCopy_ADB.Checked)
                TypeRun = ControlMode.ADB;
            else
            {
                MessageBox.Show("Please choose Copy File Operation");
                return null;
            }

            CopyFileOperation copyFileOperation;
            if (radioCopy_Pull.Checked)
                copyFileOperation = CopyFileOperation.Pull;
            else if (radioCopy_Push.Checked)
                copyFileOperation = CopyFileOperation.Push;
            else if (radioCopy_FileInAndroid.Checked)
                copyFileOperation = CopyFileOperation.FileInAndroid;
            else
            {
                MessageBox.Show("Please choose Copy File Operation");
                return null;
            }

            var source = txtSourcePath.Text.Trim();
            var destination = txtDestinationPath.Text.Trim();
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(destination))
            {
                MessageBox.Show("Please enter both Source and Destination paths");
                return null;
            }

            string armBroadCommand = txt_AmBroadCommand.Text.Trim();
            bool IsArmBroadCast = chkArmBroadCast.Checked;
            if (IsArmBroadCast)
            {
                if (string.IsNullOrEmpty(armBroadCommand))
                {
                    MessageBox.Show("Please enter command arm broadcast");
                    return null;
                }
            }

            FileArgs copyFileInfo = new FileArgs
            {
                SourcePath = source,
                DestinationPath = destination,
                hasSu = hasSu,
                isArmBroadCast = IsArmBroadCast,
                ArmBroadCommand = armBroadCommand,
                CopyFileOperation = copyFileOperation,
                ControlMode = TypeRun
            };
            return copyFileInfo;
        }
        private async void buttom_Test_CopyFile_Click(object sender, EventArgs e)
        {
            var copyFileInfo = GetCopyFileInfo();
            if (copyFileInfo == null) return;
            if (deviceData == null)
            {
                MessageBox.Show("DeviceData not found");
                return;
            }
            if (Atx == null)
            {
                MessageBox.Show("ATX not found");
                return;
            }
            if (phone == null)
            {
                MessageBox.Show("phone not found");
                return;
            }
            var adbClient = new AdbClient();
            var session = new PhoneSession(phone, adbClient, deviceData, Atx);
            var ctx = new DLoopContext
            {
                Session = session,
                Token = session.Cts.Token,
            };
            var returnType = await FileRunner.RunCopyFiles(ctx, copyFileInfo);
        }

        #endregion

        #region AppPackage
        public AppArgs? GetAppPackageInfo()
        {
            ControlMode ControlMode = ControlMode.ADB;

            if (radioAppPackage_ADB.Checked)
            {
                ControlMode = ControlMode.ADB;
            }
            else if (radioAppPackage_ATX.Checked)
            {
                ControlMode = ControlMode.ATX;
            }
            else if (radioAppPackage_ACC.Checked)
            {
                ControlMode = ControlMode.ACC;
            }

            AppAction appAction;

            if (radioAppPackage_AppStart.Checked)
            {
                appAction = AppAction.Start;
            }
            else if (radioAppPackage_AppUninstall.Checked)
            {
                appAction = AppAction.Uninstall;
            }
            else if (radioAppPackage_AppStop.Checked)
            {
                appAction = AppAction.Stop;
            }
            else if (radioAppPackage_AppClear.Checked)
            {
                appAction = AppAction.Clear;
            }
            else if (radioAppPackage_AppInstall.Checked)
            {
                appAction = AppAction.Install;
            }
            else
            {
                MessageBox.Show("Please choose App Package Operation");
                return null;
            }

            if (appAction == AppAction.Install)
            {
                if (string.IsNullOrEmpty(txtAppPackage_PathAPK.Text.Trim()))
                {
                    MessageBox.Show("Please enter APK path");
                    return null;
                }
            }

            string package = ComboxStartApp_ChoosePackage.Text.Trim();
            if (string.IsNullOrEmpty(package))
            {
                MessageBox.Show("Please choose a package");
                return null;
            }

            var IsWait = chkAppPackage_Wait.Checked;
            int watingTime = 0;
            if (IsWait)
            {
                if (string.IsNullOrEmpty(txtAppPackage_WaitingTime.Text))
                {
                    MessageBox.Show("Please enter waiting time");
                    return null;
                }
                // kiểm tra: toàn bộ chuỗi chỉ chứa số
                var txt = txtAppPackage_WaitingTime.Text.Trim();
                if (!int.TryParse(txt, out int waitingTime) || waitingTime < 0)
                {
                    MessageBox.Show("Please enter a valid number for waiting time");
                    return null;
                }

                watingTime = int.Parse(txtAppPackage_WaitingTime.Text.Trim());
            }

            var name = txt_AppPackage_Name.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a name");
                return null;
            }

            AppArgs appPackageInfo = new AppArgs
            {
                Name = name,
                ControlMode = ControlMode,
                Action = appAction,
                Activity = txtAppPackage_Activity.Text.Trim(),
                Package = package,
                Monkey = chkAppPackage_Money.Checked,
                Stop = chkAppPackage_Stop.Checked,
                Wait = IsWait,
                WaitTime = watingTime,
                Front = chkAppPackage_Font.Checked
            };
            return appPackageInfo;
        }
        private async void buttomAppPackage_Click(object sender, EventArgs e)
        {
            var appPackageInfo = GetAppPackageInfo();
            if (appPackageInfo == null) return;

            if (deviceData == null)
            {
                MessageBox.Show("DeviceData not found");
                return;
            }
            if (Atx == null)
            {
                MessageBox.Show("ATX not found");
                return;
            }
            if (phone == null)
            {
                MessageBox.Show("phone not found");
                return;
            }
            var adbClient = new AdbClient();
            var session = new PhoneSession(phone, adbClient, deviceData, Atx);
            var ctx = new DLoopContext
            {
                Session = session,
                Token = session.Cts.Token,
            };
            // map từ AppPackageInfo cũ sang AppArgs mới
            var args = new AppArgs
            {
                Action = appPackageInfo.Action,
                Package = appPackageInfo.Package,
                PathAPk = appPackageInfo.PathAPk,
                Monkey = appPackageInfo.Monkey,
                Stop = appPackageInfo.Stop,
                Wait = appPackageInfo.Wait,
                WaitTime = appPackageInfo.WaitTime,
                Front = appPackageInfo.Front,
                Activity = appPackageInfo.Activity
            };

            // chọn mode (ATX hay ADB thì AppRunner tự ưu tiên ATX nếu có)
            if (radioAppPackage_ATX.Checked && Atx == null)
            {
                MessageBox.Show("ATX not found");
                return;
            }

            await AppRunner.RunAsync(ctx, args);
        }
        #endregion

        #region  HTTP Reponse

        public HttpRequestConfig? Get_Keybroad_HTTPInfo()
        {
            var url = txt_Keybroad_HTTP_Url.Text.Trim();
            var titile = txt_Keybroad_HTTP_Title.Text.Trim();
            var regex = txt_Keybroad_HTTP_RegexResult.Text.Trim();
            var Api = txt_Keybroad_HTTP_APIKey.Text.Trim();
            var regexNextRequest = txt_Keybroad_HTTP_RegexValueNextRequest.Text.Trim();
            var compareResult = txt_Keybroad_Http_CompareResult.Text.Trim();
            var compareNextRequest = txt_Keybroad_Http_CompareNextRequest.Text.Trim();
            var chkSaveNextRequest = chkSaveNextValue.Checked;
            var loopTime = number_HTTP_Looptime.Value;
            var delayTime = number_HTTP_DelayLoop.Value;

            UseRegexMode useRegexMode;
            Enum.TryParse<UseRegexMode>(combox_HTTP_UseRegexMode.SelectedItem?.ToString(), out useRegexMode);
            if (string.IsNullOrEmpty(combox_HTTP_UseRegexMode.Text))
            {
                MessageBox.Show("Please Select Use Regex Mode");
                return null;
            }

            CompareType compareType_Result;
            Enum.TryParse<CompareType>(combox_KeyBroad_HTTP_CompareType_Result.SelectedItem?.ToString(), out compareType_Result);
            if (string.IsNullOrEmpty(combox_KeyBroad_HTTP_CompareType_Result.Text))
            {
                MessageBox.Show("Please Select Compare Type for Compare Result");
                return null;
            }
            CompareType compareType_NextRequest;
            Enum.TryParse<CompareType>(combox_KeyBroad_HTTP_CompareType_RegexNextRequest.SelectedItem?.ToString(), out compareType_NextRequest);
            if (string.IsNullOrEmpty(combox_KeyBroad_HTTP_CompareType_RegexNextRequest.Text))
            {
                MessageBox.Show("Please Select Compare Type for Next Request");
                return null;
            }

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(titile) || string.IsNullOrEmpty(regex) || string.IsNullOrEmpty(Api))
            {
                MessageBox.Show("Please enter all fields for API Model");
                return null;
            }

            if (!string.IsNullOrEmpty(compareResult))
                if (string.IsNullOrEmpty(combox_KeyBroad_HTTP_CompareType_Result.Text) || compareType_Result == CompareType.None)
                {
                    MessageBox.Show("Please choose Compare Type for Compare Result");
                    return null;
                }

            if (!string.IsNullOrEmpty(compareNextRequest))
                if (string.IsNullOrEmpty(txt_Keybroad_Http_CompareNextRequest.Text) || compareType_NextRequest == CompareType.None)
                {
                    MessageBox.Show("Please choose Compare Type for Compare Result");
                    return null;
                }

            ControlMode ControlMode = ControlMode.ADB;
            if (radio_HTTP_ADB.Checked)
            {
                ControlMode = ControlMode.ADB;
            }
            else if (radio_HTTP_Scrcpy.Checked)
            {
                ControlMode = ControlMode.Scrcpy;
            }
            else if (radio_HTTP_ATX.Checked)
            {
                ControlMode = ControlMode.ATX;
            }
            else if (radio_HTTP_UHDI.Checked)
            {
                ControlMode = ControlMode.HDI;
            }
            else if (radio_HTTP_ACC.Checked)
            {
                ControlMode = ControlMode.ACC;
            }



            TypeOption typeOption = TypeOption.Typing;
            Enum.TryParse<TypeOption>(combox_HTTP_TypingOption.SelectedItem?.ToString(), out typeOption);
            if (string.IsNullOrEmpty(combox_HTTP_TypingOption.Text))
            {
                MessageBox.Show("Please choose Type Option");
                return null;
            }

            HttpRequestConfig httpInfo = new HttpRequestConfig
            {
                ControlMode = ControlMode,
                HttpUrl = url,
                HttpTitle = titile,
                RegexResult = regex,
                ApiKey = Api,
                RegexValueNextRequest = regexNextRequest,
                CompareResult = compareResult,
                CompareNextRequest = compareNextRequest,
                CompareType_Result = compareType_Result,
                CompareType_NextRequest = compareType_NextRequest,
                UseRegexMode = useRegexMode,
                IsSaveValue = chkSaveNextRequest,
                TypeOption = typeOption,
                LoopTimes = loopTime,
                LoopDelay = delayTime
            };

            return httpInfo;
        }
        private async void buttonHttpReponse_Click(object sender, EventArgs e)
        {
            var httpinfo = Get_Keybroad_HTTPInfo();
            if (httpinfo == null)
            {
                MessageBox.Show("Please enter valid HTTP Info");
                return;
            }



            if (deviceData == null)
            {
                MessageBox.Show("DeviceData not found");
                return;
            }
            if (Atx == null)
            {
                MessageBox.Show("ATX not found");
                return;
            }
            if (phone == null)
            {
                MessageBox.Show("phone not found");
                return;
            }
            var session = new PhoneSession(phone, adbClient, deviceData, Atx);
            var ctx = new DLoopContext
            {
                Session = session,
                Token = session.Cts.Token,
            };
            await InputRunner.Run_HttpReponse_Test(ctx, httpinfo, 2000);

        }


        #endregion


        //public string Get_Database_TwoFactor()
        //{
        //    var text = combox_Database_TwoFactor.Text.Trim();
        //    if (string.IsNullOrEmpty(text))
        //    {
        //        MessageBox.Show("Please enter text to send");
        //        return string.Empty;
        //    }

        //    var appData = combox_Database_TwoFactor.SelectedItem as AppData;
        //    if (appData != null && string.IsNullOrEmpty(appData.PrivateKey))
        //    {
        //        return appData.PrivateKey;
        //    }
        //    return string.Empty;
        //}

        private void buttomRound1_Click(object sender, EventArgs e)
        {
            if (phone != null)
            {
                FormAppData formAppData = new FormAppData(phone, deviceData, false);
                formAppData.StartPosition = FormStartPosition.CenterScreen;
                formAppData.Show();
            }
        }

        #region Set Value To A Column Database

        public void LoadSocialNetwork()
        {
            combox_SetColumnData_SocialNetwork.Items.Clear();
            var datas = SocialNetworkRepository.GetAll();
            combox_SetColumnData_SocialNetwork.DataSource = datas;
            combox_GetDatabase_SocialNetwork.DataSource = datas;
            Combox_Reaction_Social_Network.DataSource = datas;
        }

        public SetColumnDataArgs? GetReadTextData(SaveNode saveNode)
        {
            ControlMode ControlMode = ControlMode.ADB;
            if (radio_Control_Database_ADB.Checked)
            {
                ControlMode = ControlMode.ADB;
            }
            else if (radio_Control_Database_ATX.Checked)
            {
                ControlMode = ControlMode.ATX;
            }
            else if (radio_Control_Database_Scrcpy.Checked)
            {
                ControlMode = ControlMode.Scrcpy;
            }
            else if (radio_Control_Database_UHDI.Checked)
            {
                ControlMode = ControlMode.HDI;
            }
            else if (radio_Control_Database_ACC.Checked)
            {
                ControlMode = ControlMode.ACC;
            }

            ReadType readType = ReadType.None;
            if (radio_Database_ReadText_ReadNodeText.Checked)
            {
                readType = ReadType.NodeText;
            }
            else if (radio_Database_ReadText_ReadImageText.Checked)
            {
                readType = ReadType.ImageToText;
            }
            if (readType == ReadType.None)
            {
                MessageBox.Show("Please choose ReadType Text - Node or Image");
                return null;
            }
            bool isclass = chk_Database_ClassName.Checked;
            bool isResource = chk_Database_ResourceName.Checked;
            bool ispackage = chk_Database_PackageName.Checked;
            bool isGetfullText = chk_Database_IsGetFullText.Checked;
            string columnName = combox_Database_Column_SaveData.SelectedItem?.ToString() ?? string.Empty;
            string Nodepattern = txt_Database_RegexValue.Text.Trim();
            string region = txt_Database_ReadText_RectangleRegion.Text.Trim();
            string RegexPattern = txt_Database_ReadText_RegexPattern.Text.Trim();

            //Debug.WriteLine($"Picbox Size : {scre}");

            RectangleF? rect = null;
            if (!string.IsNullOrEmpty(region))
            {
                var parts = region.Split(',');
                if (parts.Length == 4 &&
                    float.TryParse(parts[0], out float x) &&
                    float.TryParse(parts[1], out float y) &&
                    float.TryParse(parts[2], out float w) &&
                    float.TryParse(parts[3], out float h))
                {
                    rect = new RectangleF(x, y, w, h);
                }
                else
                {
                    MessageBox.Show("RectangleRegion format must be: x,y,width,height");
                    return null;
                }
            }
            if (string.IsNullOrEmpty(columnName))
            {
                MessageBox.Show("Please select a column to save the text");
                return null;
            }
            if (RegexPattern == null)
            {
                MessageBox.Show("Image RegexPattern is null");
                return null;
            }

            if (saveNode == null)
            {
                MessageBox.Show("SaveNode is null");
                return null;
            }

            if (readType == ReadType.ImageToText && string.IsNullOrEmpty(region))
            {
                MessageBox.Show("Please enter RectangleRegion for ImageToText");
                return null;
            }
            if (combobox_Database_ReadText_Language.SelectedItem == null)
            {
                MessageBox.Show("Please Select Language In Image");
                return null;
            }

            Language language = (Language)combobox_Database_ReadText_Language.SelectedItem;
            var socialNetwork = combox_SetColumnData_SocialNetwork.SelectedItem as SocialNetwork;
            if (socialNetwork == null)
            {
                MessageBox.Show("Please Select SocialNetwork");
                return null;
            }
            var setColumnData = new SetColumnDataArgs
            {
                NodeClassName = saveNode.ClassName,
                NodeResourceName = saveNode.ResourceName,
                NodePackageName = saveNode.PackageName,
                IsNodeClassName = isclass,
                IsNodeResourceName = isResource,
                IsNodePackageName = ispackage,
                ColumnName = columnName,
                NodeRegexParttern = Nodepattern,
                ImageRectangleRegion = region,
                ImageRegexPattern = RegexPattern,
                ReadType = readType,
                ControlMode = ControlMode,
                Language = language,
                SocialNetworkName = socialNetwork.NetworkName,
                IsNodeGetFullText = isGetfullText,
            };
            return setColumnData;
        }
        // 2. bắt "key: value" trên nhiều dòng
        private static readonly Regex KeyValueRegex = new Regex(
            @"^(?<key>\w+):\s*(?<value>.*)$",
            RegexOptions.CultureInvariant | RegexOptions.Multiline);

        private async void buttom_Database_SetColumn_Test_Click(object sender, EventArgs e)
        {
            if (saveText == null) return;
            var setcolumnData = GetReadTextData(saveText);
            if (setcolumnData == null) return;

            if (deviceData == null)
            {
                MessageBox.Show("DeviceData not found");
                return;
            }
            if (Atx == null)
            {
                MessageBox.Show("ATX not found");
                return;
            }
            if (phone == null)
            {
                MessageBox.Show("phone not found");
                return;
            }
            var session = new PhoneSession(phone, adbClient, deviceData, Atx);
            var ctx = new DLoopContext
            {
                Session = session,
                Token = session.Cts.Token,
            };

            await DatabaseRunner.RunSetDatabaseColumn(ctx, setcolumnData, bmp =>
            {
                // chạy trên UI thread
                if (pic_Database_ReadText_Image.InvokeRequired)
                {
                    pic_Database_ReadText_Image.Invoke(() =>
                    {
                        pic_Database_ReadText_Image.Image?.Dispose();
                        pic_Database_ReadText_Image.Image = (Bitmap)bmp.Clone();
                    });
                }
                else
                {
                    pic_Database_ReadText_Image.Image?.Dispose();
                    pic_Database_ReadText_Image.Image = (Bitmap)bmp.Clone();
                }
            });
        }
        public SaveNode? Save_Database_Text()
        {
            var nodeText = txt_Database_SaveNode.Text.Trim();
            if (string.IsNullOrEmpty(nodeText))
            {
                MessageBox.Show("Please enter all fields for Save Text");
                return null;
            }

            var matches = KeyValueRegex.Matches(nodeText);

            SaveNode saveText = new SaveNode();

            foreach (Match match in matches)
            {
                string key = match.Groups["key"].Value;
                string value = match.Groups["value"].Value;

                switch (key)
                {
                    case "Text": saveText.Text = value; break;
                    case "ClassName": saveText.ClassName = value; break;
                    case "ResourceName": saveText.ResourceName = value; break;
                    case "PackageName": saveText.PackageName = value; break;
                    case "ContentDescription": saveText.ContentDescription = value; break;
                    case "Index":
                        if (int.TryParse(value, out int index))
                            saveText.Index = index;
                        break;
                }
            }


            return saveText;
        }
        private async Task txt_Database_SearchNode_TextChanged(object sender, EventArgs e)
        {
            if (Atx == null)
            {
                MessageBox.Show("ATX Not Show");
                return;
            }

            string search = txt_Database_SearchNode.Text.Trim();

            combox_Database_ListNode.Items.Clear();
            txt_Database_SaveNode.Text = string.Empty;

            var Objs = await Atx.FindNodeByTextOrDescription(search);

            if (Objs != null)
            {
                var node = Objs.First();
                txt_Database_SaveNode.Text =
                    $"Text: {node.Text}\r\n" +
                    $"ClassName: {node.ClassName}\r\n" +
                    $"ResourceName: {node.ResourceName}\r\n" +
                    $"PackageName: {node.PackageName}\r\n" +
                    $"ContentDescription: {node.ContentDescription}\r\n" +
                    $"Index: {node.Index}";

                saveText = new SaveNode
                {
                    Text = node.Text,
                    ClassName = node.ClassName,
                    ResourceName = node.ResourceName,
                    PackageName = node.PackageName,
                    ContentDescription = node.ContentDescription,
                    Index = node.Index
                };

                foreach (var node1 in Objs)
                {
                    combox_Database_ListNode.Items.Add(node1);
                }
            }

        }
        private void combox_Database_ListNode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var saveText = combox_Database_ListNode.SelectedItem as SaveNode;
            if (saveText == null)
                return;
            this.saveText = saveText;
            txt_Database_SaveNode.Text =
                  $"Text: {saveText.Text}\r\n" +
                  $"ClassName: {saveText.ClassName}\r\n" +
                  $"ResourceName: {saveText.ResourceName}\r\n" +
                  $"PackageName: {saveText.PackageName}\r\n" +
                  $"ContentDescription: {saveText.ContentDescription}\r\n" +
                  $"Index: {saveText.Index}";

        }

        private void buttom_Database_Test_PackageList_Click(object sender, EventArgs e)
        {
            if (phone != null && deviceData != null)
            {
                FormAppData formAppData = new FormAppData(phone, deviceData, false);
                formAppData.StartPosition = FormStartPosition.CenterScreen;
                formAppData.Show();
            }
        }
        #endregion

        #region Get Value To A Column Database
        public GetColumnDataArgs? GetCoumnData()
        {
            ControlMode ControlMode = ControlMode.ADB;
            if (radio_GetDatabase_ADB.Checked)
            {
                ControlMode = ControlMode.ADB;
            }
            else if (radio_GetDatabase_ATX.Checked)
            {
                ControlMode = ControlMode.ATX;
            }
            else if (radio_GetDatabase_Scrcpy.Checked)
            {
                ControlMode = ControlMode.Scrcpy;
            }
            else if (radio_GetDatabase_UHDI.Checked)
            {
                ControlMode = ControlMode.HDI;
            }
            else if (radio_GetDatabase_ACC.Checked)
            {
                ControlMode = ControlMode.ACC;
            }

            WriteAction writeAction = WriteAction.None;
            if (radio_GetDatabase_ColumnWrite.Checked)
            {
                writeAction = WriteAction.Column;
            }
            else if (radio_GetDatabase_OTP.Checked)
            {
                writeAction = WriteAction.OTP;
            }
            if (writeAction == WriteAction.None)
            {
                MessageBox.Show("Please choose WriteAction Mode");
                return null;
            }


            if (combox_GetDatabase_TypeInput.SelectedItem == null)
            {
                MessageBox.Show("Please Select Typing Option");
                return null;
            }
            TypeOption typeOption = (TypeOption)combox_GetDatabase_TypeInput.SelectedItem;

            if (combox_GetDatabase_Column.SelectedItem == null)
            {
                MessageBox.Show("Please Select Comlumn Option");
                return null;
            }
            string columnName = (string)combox_GetDatabase_Column.SelectedItem;

            if (combox_GetDatabase_SocialNetwork.SelectedItem == null)
            {
                MessageBox.Show("Please Select SocialNetwork Option");
                return null;
            }
            var Network = (SocialNetwork)combox_GetDatabase_SocialNetwork.SelectedItem;

            var getColumnDatabase = new GetColumnDataArgs
            {
                ControlMode = ControlMode,
                ColumnName = columnName,
                SocialNetworkName = Network.NetworkName,
                TypeOption = typeOption,
                WriteAction = writeAction
            };


            return getColumnDatabase;
        }
        private async void buttom_GetDatabase_Test_Click(object sender, EventArgs e)
        {
            var getcolumndata = GetCoumnData();
            if (getcolumndata == null) return;
            if (deviceData == null)
            {
                MessageBox.Show("DeviceData not found");
                return;
            }
            if (Atx == null)
            {
                MessageBox.Show("ATX not found");
                return;
            }
            if (phone == null)
            {
                MessageBox.Show("phone not found");
                return;
            }
            var session = new PhoneSession(phone, adbClient, deviceData, Atx);
            var ctx = new DLoopContext
            {
                Session = session,
                Token = session.Cts.Token,
            };
            await DatabaseRunner.RunGetDatabaseColumn(ctx, getcolumndata);

        }

        #endregion


        private bool CheckReactionSetValue(string buttomLikeText, string likeText, string loveText, string careText, string hahaText, string wowText, string sadText, string angeryText, string maxDyText, out ReactionSet reactionSet, out string errorMessage)
        {
            reactionSet = null!;
            errorMessage = string.Empty;

            // 1. Kiểm tra null/empty
            if (string.IsNullOrWhiteSpace(buttomLikeText))
            {
                errorMessage = "ButtomLike không được để trống";
                return false;
            }
            if (string.IsNullOrWhiteSpace(likeText))
            {
                errorMessage = "Like không được để trống";
                return false;
            }
            if (string.IsNullOrWhiteSpace(loveText))
            {
                errorMessage = "Love không được để trống";
                return false;
            }
            if (string.IsNullOrWhiteSpace(careText))
            {
                errorMessage = "Care không được để trống";
                return false;
            }
            if (string.IsNullOrWhiteSpace(hahaText))
            {
                errorMessage = "Haha không được để trống";
                return false;
            }
            if (string.IsNullOrWhiteSpace(wowText))
            {
                errorMessage = "Wow không được để trống";
                return false;
            }
            if (string.IsNullOrWhiteSpace(sadText))
            {
                errorMessage = "Sad không được để trống";
                return false;
            }
            if (string.IsNullOrWhiteSpace(angeryText))
            {
                errorMessage = "Angry không được để trống";
                return false;
            }
            if (string.IsNullOrWhiteSpace(maxDyText))
            {
                errorMessage = "MaxDY không được để trống";
                return false;
            }

            // 2. Parse từng giá trị
            if (!TryParseXY(buttomLikeText, out var buttomLike, out errorMessage))
                return false;

            if (!TryParseXY(likeText, out var like, out errorMessage))
                return false;

            if (!TryParseXY(loveText, out var love, out errorMessage))
                return false;

            if (!TryParseXY(careText, out var care, out errorMessage))
                return false;

            if (!TryParseXY(hahaText, out var haha, out errorMessage))
                return false;

            if (!TryParseXY(wowText, out var wow, out errorMessage))
                return false;

            if (!TryParseXY(sadText, out var sad, out errorMessage))
                return false;

            if (!TryParseXY(angeryText, out var angery, out errorMessage))
                return false;

            // 3. Parse MaxDY
            if (!float.TryParse(maxDyText.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var maxDy))
            {
                errorMessage = $"MaxDY không đúng định dạng số: '{maxDyText}'";
                return false;
            }

            // 4. Validate giá trị hợp lệ
            if (!ValidatePoint(buttomLike, "ButtomLike", out errorMessage)) return false;
            if (!ValidatePoint(like, "Like", out errorMessage)) return false;
            if (!ValidatePoint(love, "Love", out errorMessage)) return false;
            if (!ValidatePoint(care, "Care", out errorMessage)) return false;
            if (!ValidatePoint(haha, "Haha", out errorMessage)) return false;
            if (!ValidatePoint(wow, "Wow", out errorMessage)) return false;
            if (!ValidatePoint(sad, "Sad", out errorMessage)) return false;
            if (!ValidatePoint(angery, "Angry", out errorMessage)) return false;

            if (maxDy < 0 || maxDy > 100)
            {
                errorMessage = $"MaxDY phải nằm trong khoảng 0-100: {maxDy}";
                return false;
            }

            // 5. Tạo ReactionSet
            reactionSet = new ReactionSet
            {
                ButtomLike = buttomLike,
                Like = like,
                Love = love,
                Care = care,
                Haha = haha,
                Wow = wow,
                Sad = sad,
                Angry = angery,
                MaxDY = maxDy
            };

            return true;
        }
        private bool TryParseXY(string text, out ReactionPoint point, out string errorMessage)
        {
            point = null!;
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(text))
            {
                errorMessage = "Chuỗi rỗng hoặc null";
                return false;
            }

            var parts = text.Split(',');
            if (parts.Length != 2)
            {
                errorMessage = $"Định dạng không đúng (cần dạng 'x,y'): '{text}'";
                return false;
            }

            if (!float.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var x))
            {
                errorMessage = $"Giá trị X không đúng định dạng số: '{parts[0]}'";
                return false;
            }

            if (!float.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
            {
                errorMessage = $"Giá trị Y không đúng định dạng số: '{parts[1]}'";
                return false;
            }

            point = new ReactionPoint(x, y);
            return true;
        }
        private bool ValidatePoint(ReactionPoint point, string fieldName, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (point.X < 0 || point.X > 100)
            {
                errorMessage = $"{fieldName}.X phải nằm trong khoảng 0-100: {point.X}";
                return false;
            }

            if (point.Y < 0 || point.Y > 100)
            {
                errorMessage = $"{fieldName}.Y phải nằm trong khoảng 0-100: {point.Y}";
                return false;
            }

            return true;
        }

        private async void buttom_Reaction_Test_Click(object sender, EventArgs e)
        {
            ControlMode controlMode = ControlMode.ADB;
            if (radio_Reaction_ADB.Checked)
            {
                controlMode = ControlMode.ADB;
            }
            else if (radio_Reaction_ATX.Checked)
            {
                controlMode = ControlMode.ATX;
            }
            else if (radio_Reaction_Scrcpy.Checked)
            {
                controlMode = ControlMode.Scrcpy;
            }
            else if (radio_Reaction_UHDI.Checked)
            {
                controlMode = ControlMode.HDI;
            }
            else if (radio_Reaction_ACC.Checked)
            {
                controlMode = ControlMode.ACC;
            }


            // Validate TopSet
            if (!CheckReactionSetValue(
                txt_Reaction_TOP_ButtomLike.Text,
                txt_TOP_ReactionLike.Text,
                txt_TOP_ReactionLove.Text,
                txt_TOP_ReactionCare.Text,
                txt_TOP_ReactionHaha.Text,
                txt_TOP_ReactionWow.Text,
                txt_TOP_ReactionSAD.Text,
                txt_TOP_ReactionAngry.Text,
                txt_TOP_ReactionMaxDy.Text,
                out var topSet,
                out string topError))
            {
                MessageBox.Show($"Lỗi TopSet: {topError}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate BottomSet
            if (!CheckReactionSetValue(
                txt_Reaction_BOTTOM_Buttomike.Text,
                txt_BOTTOM_ReactionLike.Text,
                txt_BOTTOM_ReactionLove.Text,
                txt_BOTTOM_ReactionCare.Text,
                txt_BOTTOM_ReactionHaHa.Text,
                txt_BOTTOM_ReactionWow.Text,
                txt_BOTTOM_ReactionSAD.Text,
                txt_BOTTOM_ReactionAngry.Text,
                txt_BOTTOM_ReactionMaxDy.Text,
                out var bottomSet,
                out string bottomError))
            {
                MessageBox.Show($"Lỗi BottomSet: {bottomError}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var name = txt_Name_Of_This_Reaction.Text;
            var socialNetwork = Combox_Reaction_Social_Network.SelectedItem as SocialNetwork;
            if (socialNetwork == null)
            {
                MessageBox.Show("Please Select SocialNetwork");
                return;
            }
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please Select SwipeLike Name");
                return;
            }

            if (Combox_ReactionType.SelectedItem == null)
            {
                MessageBox.Show("Please Select Reaction Type");
                return;
            }
            var reActionType = (ReactionType)Combox_ReactionType.SelectedItem;


            if (Combox_Reaction_NoteImage.SelectedItem == null)
            {
                MessageBox.Show("Please Select Check Node, Image");
                return;
            }

            var VisionScanArgs = (VisionScanArgs)Combox_Reaction_NoteImage.SelectedItem;
            if (phone == null)
            {
                MessageBox.Show("phone not found");
                return;
            }

            // Nếu validate thành công, tiếp tục xử lý
            var reactions = new Reaction
            {
                TopSet = topSet,
                BottomSet = bottomSet,
                NetWorkName = socialNetwork.NetworkName,
                DeviceModel = phone.Model,
                ReactionName = name,
                ReactionType = reActionType,
            };

            EmojiArgs emoijReaction = new EmojiArgs
            {
                Name = name,
                ControlMode = controlMode,
                Reactions = reactions,
                VisionScanArgs = VisionScanArgs
            };
            if (deviceData == null)
            {
                MessageBox.Show("DeviceData not found");
                return;
            }
            if (Atx == null)
            {
                MessageBox.Show("ATX not found");
                return;
            }

            var session = new PhoneSession(phone, adbClient, deviceData, Atx);
            var ctx = new DLoopContext
            {
                Session = session,
                Token = session.Cts.Token,
            };

            await EmojiRunner.RunAsync(ctx, emoijReaction);
        }

        private void Combox_ReactionTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Combox_ReactionTemplate.SelectedItem == null) return;

            var reactions = (Reaction)Combox_ReactionTemplate.SelectedItem;
            if (reactions == null) return;

            // --- thông tin chung (nếu bạn có) ---
            txt_Name_Of_This_Reaction.Text = reactions.ReactionName;

            // chọn lại Social Network theo tên
            foreach (var item in Combox_Reaction_Social_Network.Items)
            {
                if (item is SocialNetwork sn && sn.NetworkName == reactions.NetWorkName)
                {
                    Combox_Reaction_Social_Network.SelectedItem = sn;
                    break;
                }
            }

            Combox_ReactionType.SelectedItem = reactions.ReactionType;

            // --- TOP SET ---
            txt_Reaction_TOP_ButtomLike.Text = reactions.TopSet.ButtomLike.ToString(); // "x,y"
            txt_TOP_ReactionLike.Text = reactions.TopSet.Like.ToString();
            txt_TOP_ReactionLove.Text = reactions.TopSet.Love.ToString();
            txt_TOP_ReactionCare.Text = reactions.TopSet.Care.ToString();
            txt_TOP_ReactionHaha.Text = reactions.TopSet.Haha.ToString();
            txt_TOP_ReactionWow.Text = reactions.TopSet.Wow.ToString();
            txt_TOP_ReactionSAD.Text = reactions.TopSet.Sad.ToString();
            txt_TOP_ReactionAngry.Text = reactions.TopSet.Angry.ToString();
            txt_TOP_ReactionMaxDy.Text = reactions.TopSet.MaxDY.ToString(System.Globalization.CultureInfo.InvariantCulture);

            // --- BOTTOM SET ---
            txt_Reaction_BOTTOM_Buttomike.Text = reactions.BottomSet.ButtomLike.ToString();
            txt_BOTTOM_ReactionLike.Text = reactions.BottomSet.Like.ToString();
            txt_BOTTOM_ReactionLove.Text = reactions.BottomSet.Love.ToString();
            txt_BOTTOM_ReactionCare.Text = reactions.BottomSet.Care.ToString(); // <-- cái bạn cần
            txt_BOTTOM_ReactionHaHa.Text = reactions.BottomSet.Haha.ToString();
            txt_BOTTOM_ReactionWow.Text = reactions.BottomSet.Wow.ToString();
            txt_BOTTOM_ReactionSAD.Text = reactions.BottomSet.Sad.ToString();
            txt_BOTTOM_ReactionAngry.Text = reactions.BottomSet.Angry.ToString();
            txt_BOTTOM_ReactionMaxDy.Text = reactions.BottomSet.MaxDY.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
