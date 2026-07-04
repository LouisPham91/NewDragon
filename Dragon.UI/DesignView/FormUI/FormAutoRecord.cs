using AdvancedSharpAdbClient;
using AntdUI;
using Dragon.ControlHelper.ScrcpyNet.DesignDevice;
using Dragon.ControlHelper.UIController;
using Dragon.Controller.DeviceControl.HATX;
using Dragon.Controller.DeviceControl.Orc;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Infrastructure.Services;
using Dragon.DesignView.Public;
using System.Diagnostics;

namespace Dragon.DesignView.FormUI
{
    public partial class FormAutoRecord : FormOriginal
    {
        AtxDevice? atx = null;
        DLoop? dLoop = null;
        AdbClient adbClient = new AdbClient();
        public FormAutoRecord()
        {
            InitializeComponent();
            this.KeyPreview = true; // Form bắt phím trước
            this.KeyUp += FormAutoRecord_KeyUp; // đăng ký sự kiện KeyUp
            dragTimer = new System.Windows.Forms.Timer();
            dragTimer.Interval = 150;
            dragTimer.Tick += DragTimer_Tick;
            StartPosition = FormStartPosition.CenterScreen;
            ApplyTheme();
        }
        private void FormAutoRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.KeyUp -= FormAutoRecord_KeyUp;

            picboxScreenShot.MouseClick -= PicboxScreenShot_MouseClick;
            picboxScreenShot.MouseDown -= picboxScreenShot_MouseDown;
            picboxScreenShot.MouseMove -= picboxScreenShot_MouseMove;
            picboxScreenShot.MouseUp -= picboxScreenShot_MouseUp;
            picboxScreenShot.Paint -= PicboxScreenShot_Paint;

            treeMain.NodeMouseClick -= TreeMain_NodeMouseClick;
            treeMain.MouseMove -= TreeMain_MouseMove;
            treeMain.MouseDown -= treeMain_MouseDown;
            treeMain.MouseUp -= treeMain_MouseUp;
            treeMain.Paint -= treeMain_Paint;

            FormManager.Instance.Switch_AutoRecord_To_Flow_OPEN(true);
        }
        public void ApplyTheme()
        {
            foreach (var ctrl in DesignHelper.GetAllControls(this))
            {
                if (ctrl is IThemeable themeable)
                    themeable.ApplyTheme();
            }

            BackColor = ThemeHelper.BorderNormalFist;
            ForeColor = ThemeHelper.ForeNormalFirst;

            panelMain.BackColor = ThemeHelper.BackNormalFirst;
            panelMain.ForeColor = ThemeHelper.ForeNormalFirst;
            treeMain.BackColor = ThemeHelper.BackNormalFirst;
            treeMain.ForeColor = ThemeHelper.ForeNormalFirst;

            //labelNormaln2.ApplyTheme();
            //panelNormaln11.ApplyTheme();
            //panelNormaln6.ApplyTheme();
            //labelNameAutoRun.ApplyTheme();
            //panelNormaln7.ApplyTheme();
            //txtDloopTitle.ApplyTheme();
            //labelNormaln3.ApplyTheme();
        }

        #region Load 
        private void FormAutoRecord_Load(object sender, EventArgs e)
        {
            SetUpUI();

            picboxScreenShot.MouseClick += PicboxScreenShot_MouseClick;
            picboxScreenShot.MouseDown += picboxScreenShot_MouseDown;
            picboxScreenShot.MouseMove += picboxScreenShot_MouseMove;
            picboxScreenShot.MouseUp += picboxScreenShot_MouseUp;
            picboxScreenShot.Paint += PicboxScreenShot_Paint;


        }
        public void SetUpUI()
        {
            var panelDevices = GetPanelDevice();
            if (panelDevices == null) return;

            Height = panelDevices.Height + panelNormaln3.Height + 42;
            panelUnderBack.Width = (panelDevices.Width) * 2 + 6;

            panelPhone.Width = panelDevices.Width + 6;

            panelPhone.BackColor = Color.FromArgb(50, 50, 50);
            panelScreenShot.BackColor = Color.FromArgb(50, 50, 50);
            // Đăng ký event cho Tree
            treeMain.NodeMouseClick += TreeMain_NodeMouseClick;
            treeMain.MouseMove += TreeMain_MouseMove;

            // Đăng ký event chuột chung
            treeMain.MouseDown += treeMain_MouseDown;
            treeMain.MouseUp += treeMain_MouseUp;
            treeMain.Paint += treeMain_Paint;
            var serial = panelDevices._PDcontroller?.DeviceData?.Serial;
            if (!string.IsNullOrEmpty(serial))
            {
                Task.Run(async () =>
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    atx = await AtxDevice.CreateAsync(serial, false);
                    stopwatch.Stop();
                    Debug.WriteLine($"Create ATX Take: {stopwatch.ElapsedMilliseconds}ms");
                });

            }

        }
        #endregion

        #region PictureBox ScreenShot - CheckClickImage,CheckNote

        Bitmap? SreeShotTemplate = null;
        // Khai báo biến toàn cục
        private bool isDrawing = false;
        private Point startPoint;
        private Rectangle currentRect;
        Random rand = new Random();
        FormADDNoteImage? formADDNoteImage = null;

        public void DrawingRectOnPicBox(Rectangle rectangle, Color? color = null)
        {
            Color c = color ?? Color.Green; // nếu null thì dùng Green
            if (picboxScreenShot.Image == null) return;
            using (Graphics g = Graphics.FromImage(picboxScreenShot.Image))
            {
                using (Pen pen = new Pen(c, 2))
                {
                    g.DrawRectangle(pen, rectangle);
                }
                using (Brush brush = new SolidBrush(Color.FromArgb(50, c)))
                {
                    g.FillRectangle(brush, rectangle);
                }
            }
        }
        public void ScreenShot()
        {
            var stopwatch = Stopwatch.StartNew();
            var paneldevice = GetPanelDevice();
            if (paneldevice == null)
                return;
            Bitmap? bmp = paneldevice._PDcontroller?.ScreenShot();
            if (bmp == null) return;

            picboxScreenShot.Image?.Dispose();
            picboxScreenShot.Image = bmp;
            //picboxScreenShot.Size = bmp.Size;
            SreeShotTemplate = new Bitmap(bmp);

            stopwatch.Stop();
            Debug.WriteLine($"ScreenShot Scrcpy Take: {stopwatch.ElapsedMilliseconds}ms");

        }
        private async void PicboxScreenShot_MouseClick(object? sender, MouseEventArgs e)
        {
            var paneldevice = GetPanelDevice();
            if (paneldevice == null)
                return;
            var point = TranslateZoomMousePosition(picboxScreenShot, e.Location);

            if (e.Button == MouseButtons.Left)
            {
                //if (picboxScreenShot.Image == null)
                //    ScreenShot();
                if (picboxScreenShot.Image == null) return;

                var bm = new Bitmap(picboxScreenShot.Image);
                var pointPercent = ConvertPercent.PointToPercentString(point, bm);

                txtMouseClick.Text = pointPercent;
            }
            else if (e.Button == MouseButtons.Right)
            {
                ScreenShot();
                if (picboxScreenShot.Image == null) return;

                var bm = new Bitmap(picboxScreenShot.Image);
                var pointPercent = ConvertPercent.PointToPercentString(point, bm);
                var PointPhySical = ConvertPercent.PercentStringToPoint(pointPercent, paneldevice.phone.PhysicalWidth, paneldevice.phone.PhysicalHeight);
                if (atx != null)
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    var smallestNode = await atx.FindNodesByPointSmallest(PointPhySical.X, PointPhySical.Y);
                    if (smallestNode != null)
                    {
                        var rectPhysical = new Rectangle(
                                smallestNode.Bound.Left,
                                smallestNode.Bound.Top,
                                smallestNode.Bound.Width,
                                smallestNode.Bound.Height);

                        var rectBitmap = ConvertPercent.PhysicalRectToBitmapRect(
                             rectPhysical,
                             paneldevice.phone.PhysicalWidth,
                             paneldevice.phone.PhysicalHeight,
                             bm);


                        DrawingRectOnPicBox(rectBitmap, Color.Green);


                        picboxScreenShot.Refresh();
                        if (paneldevice._PDcontroller == null) return;

                        var atxNode = NodeConverterServices.ToATXNode(smallestNode, paneldevice.phone.PhysicalWidth, paneldevice.phone.PhysicalHeight);

                        var percentSmaller = ConvertPercent.RectToPercentString(rectBitmap, bm);
                        var bitmap = BitmapCropper.CropByRectangle(bm, rectBitmap);
                        if (bitmap == null) return;

                        stopwatch.Stop();
                        Debug.WriteLine("ATX Find Node Take : " + stopwatch.ElapsedMilliseconds + "ms");
                        if (formADDNoteImage != null) formADDNoteImage.Close();
                        stopwatch.Start();

                        var deviceData = paneldevice._PDcontroller.DeviceData;

                        stopwatch.Stop();
                        Debug.WriteLine("ScreenShot adb : " + stopwatch.ElapsedMilliseconds + "ms");
                        //var percentBigger = ConvertPercent.RectToPercentString(rectPhysical, bmp2);

                        var pointClick = panelNormaln1.PointToScreen(new Point(0, 0));

                        formADDNoteImage = new FormADDNoteImage((percentSmaller, smallestNode, bitmap), this, paneldevice.phone, deviceData, atx);
                        formADDNoteImage.Location = pointClick;
                        formADDNoteImage.Show();


                        picboxScreenShot.Refresh();
                    }
                }
            }
        }

        private Rectangle savedControlRectForDebug = Rectangle.Empty; // control coords (x,y,w,h)
        private Rectangle savedImageRectForDebug = Rectangle.Empty;   // image coords

        private void picboxScreenShot_Click(object sender, EventArgs e)
        {
            ScreenShot();
        }


        // -----------------------------
        // Helper: rect ảnh hiển thị trong PictureBox (control coords)
        // -----------------------------
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

        // -----------------------------
        // Translate control mouse -> image coords (dùng GetImageDisplayRect để đồng bộ)
        // -----------------------------
        private Point TranslateZoomMousePosition(PictureBox picBox, Point mousePoint)
        {
            if (picBox.Image == null) return mousePoint;

            var disp = GetImageDisplayRect(picBox);
            if (disp == Rectangle.Empty) return mousePoint;

            float ratio = (float)disp.Width / picBox.Image.Width; // ratioX == ratioY khi SizeMode = Zoom
            int x = (int)Math.Round((mousePoint.X - disp.X) / ratio);
            int y = (int)Math.Round((mousePoint.Y - disp.Y) / ratio);

            x = Math.Max(0, Math.Min(picBox.Image.Width - 1, x));
            y = Math.Max(0, Math.Min(picBox.Image.Height - 1, y));

            return new Point(x, y);
        }

        // -----------------------------
        // Chuyển Rectangle từ image coords -> control coords (dùng để vẽ)
        // -----------------------------
        private Rectangle ImageRectToControlRect(PictureBox pic, Rectangle imgRect)
        {
            if (pic.Image == null) return Rectangle.Empty;
            var disp = GetImageDisplayRect(pic);
            if (disp == Rectangle.Empty) return Rectangle.Empty;

            float ratio = (float)disp.Width / pic.Image.Width;

            int x = disp.X + (int)Math.Round(imgRect.X * ratio);
            int y = disp.Y + (int)Math.Round(imgRect.Y * ratio);
            int w = Math.Max(1, (int)Math.Round(imgRect.Width * ratio));
            int h = Math.Max(1, (int)Math.Round(imgRect.Height * ratio));

            return new Rectangle(x, y, w, h);
        }

        // -----------------------------
        // Paint handler: vẽ overlay (dùng control coords)
        // -----------------------------
        private void PicboxScreenShot_Paint(object? sender, PaintEventArgs e)
        {
            if (picboxScreenShot.Image == null) return;

            // Vẽ vùng chọn tạm thời (image coords -> control coords)
            if (currentRect.Width > 0 && currentRect.Height > 0)
            {
                var controlRect = ImageRectToControlRect(picboxScreenShot, currentRect);
                if (controlRect != Rectangle.Empty)
                {
                    using (Pen pen = new Pen(Color.Red, 2))
                    {
                        e.Graphics.DrawRectangle(pen, controlRect);
                    }
                    using (Brush brush = new SolidBrush(Color.FromArgb(50, Color.Red)))
                    {
                        e.Graphics.FillRectangle(brush, controlRect);
                    }
                }
            }

            // Vẽ khung debug màu xanh (percent -> image -> control)
            if (savedControlRectForDebug != Rectangle.Empty)
            {
                using (Pen pen = new Pen(Color.Blue, 2))
                {
                    e.Graphics.DrawRectangle(pen, savedControlRectForDebug);
                }
            }
            // Nếu bạn có danh sách các rect đã lưu (ví dụ List<Rectangle> savedRects in image coords),
            // vẽ chúng ở đây bằng cách chuyển từng rect sang control coords.
        }
        public void DrawBluePenTocheckImagePercentRect()
        {
            // Lấy percent từ textbox
            var percentRect = txtRectangeRegion?.Text?.Trim();
            if (string.IsNullOrEmpty(percentRect))
            {
                Debug.WriteLine("percentRect empty");
                return;
            }

            if (picboxScreenShot.Image == null)
            {
                Debug.WriteLine("No image in picboxScreenShot");
                return;
            }

            var bm = SreeShotTemplate;
            if (bm == null)
                return;

            // Chuyển percent -> image rect
            var imgRect = PercentStringToImageRect(percentRect, bm);
            if (imgRect == Rectangle.Empty)
            {
                Debug.WriteLine("Percent -> ImageRect failed");
                return;
            }

            // Lưu image rect để debug/crop
            savedImageRectForDebug = imgRect;

            // Chuyển sang control coords để vẽ overlay xanh
            savedControlRectForDebug = ImageRectToControlRectSafe(picboxScreenShot, imgRect);

            // In debug info
            Debug.WriteLine($"Percent: {percentRect}");
            Debug.WriteLine($"Image size: {bm.Width}x{bm.Height}");
            Debug.WriteLine($"ImageRect (px): {imgRect}");
            Debug.WriteLine($"Display rect: {GetImageDisplayRect(picboxScreenShot)}");
            Debug.WriteLine($"ControlRect (px): {savedControlRectForDebug}");

            // Yêu cầu PictureBox vẽ lại (Paint sẽ vẽ savedControlRectForDebug)
            picboxScreenShot.Invalidate();
        }



        // -----------------------------
        // MouseDown: bắt đầu vẽ (lưu startPoint ở image coords)
        // -----------------------------
        private void picboxScreenShot_MouseDown(object? sender, MouseEventArgs e)
        {
            if (picboxScreenShot.Image == null) return;

            if (e.Button != MouseButtons.Left) return;

            isDrawing = true;
            startPoint = TranslateZoomMousePosition(picboxScreenShot, e.Location); // image coords
            currentRect = new Rectangle(startPoint, new Size(0, 0));
            picboxScreenShot.Capture = true;
            picboxScreenShot.Invalidate();
        }

        // -----------------------------
        // MouseMove: cập nhật currentRect (image coords) và Invalidate để Paint vẽ
        // -----------------------------
        private void picboxScreenShot_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!isDrawing) return;
            if (picboxScreenShot.Image == null) return;

            Point currentPoint = TranslateZoomMousePosition(picboxScreenShot, e.Location); // image coords

            int x = Math.Min(startPoint.X, currentPoint.X);
            int y = Math.Min(startPoint.Y, currentPoint.Y);
            int w = Math.Abs(startPoint.X - currentPoint.X);
            int h = Math.Abs(startPoint.Y - currentPoint.Y);

            currentRect = new Rectangle(x, y, w, h);
            picboxScreenShot.Invalidate();
        }

        // -----------------------------
        // MouseUp: hoàn tất vùng chọn, crop, gọi form note, v.v.
        // -----------------------------
        private void picboxScreenShot_MouseUp(object? sender, MouseEventArgs e)
        {
            if (!isDrawing) return;
            isDrawing = false;
            picboxScreenShot.Capture = false;

            if (picboxScreenShot.Image == null) return;
            if (e.Button == MouseButtons.Right) return;

            Point endPoint = TranslateZoomMousePosition(picboxScreenShot, e.Location); // image coords

            int x = Math.Min(startPoint.X, endPoint.X);
            int y = Math.Min(startPoint.Y, endPoint.Y);
            int w = Math.Abs(startPoint.X - endPoint.X);
            int h = Math.Abs(startPoint.Y - endPoint.Y);

            currentRect = new Rectangle(x, y, w, h);

            const int minWidth = 5;
            const int minHeight = 5;
            if (currentRect.Width < minWidth || currentRect.Height < minHeight)
            {
                currentRect = Rectangle.Empty;
                picboxScreenShot.Invalidate();
                return;
            }

            // Tính percent dựa trên kích thước ảnh gốc (image coords)
            var bm = picboxScreenShot.Image as Bitmap;
            if (bm == null) return;

            double leftPercent = (double)currentRect.Left / bm.Width * 100.0;
            double topPercent = (double)currentRect.Top / bm.Height * 100.0;
            double rightPercent = (double)currentRect.Right / bm.Width * 100.0;
            double bottomPercent = (double)currentRect.Bottom / bm.Height * 100.0;
            string percentRect = $"{leftPercent:F2},{topPercent:F2},{rightPercent:F2},{bottomPercent:F2}";
            txtRectangeRegion.Text = percentRect;
            // Vẽ final lên SreeShotTemplate (không sửa trực tiếp picboxScreenShot.Image nếu không muốn)
            if (SreeShotTemplate == null)
                SreeShotTemplate = new Bitmap(bm); // clone ảnh gốc

            using (Graphics g = Graphics.FromImage(SreeShotTemplate))
            {
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    g.DrawRectangle(pen, currentRect);
                }
                using (Brush brush = new SolidBrush(Color.FromArgb(50, Color.Red)))
                {
                    g.FillRectangle(brush, currentRect);
                }
            }

            Debug.WriteLine($"Picbox Size: {picboxScreenShot.Size.Width},{picboxScreenShot.Size.Height}");
            Debug.WriteLine($"Image Size: {bm.Size.Width},{bm.Size.Height}");


            // Crop từ SreeShotTemplate (image coords)
            var bit = BitmapCropper.CropByRectangle(bm, currentRect);
            if (bit == null) return;

            var paneldevice = GetPanelDevice();
            if (paneldevice == null)
                return;
            if (paneldevice._PDcontroller == null) return;
            var deviceData = paneldevice._PDcontroller.DeviceData;

            var framebuffer = adbClient.GetFrameBuffer(deviceData);
            var bmp2 = FrameBufferDrawingExtensions.ToImage(framebuffer);
            if (bmp2 == null) return;

            var RectBigger = BitmapConverter.ConvertRect(currentRect, bm.Size, bmp2.Size);
            var percentRectBigger = ConvertPercent.RectToPercentString(RectBigger, bmp2);


            if (formADDNoteImage != null)
                formADDNoteImage.addImage((percentRect, bit));

            if (formTestAction != null)
            {
                formTestAction.txt_Database_ReadText_RectangleRegion.Text = percentRect;
                formTestAction.pic_Database_ReadText_Image.Image = bit;
            }

            // Giữ hoặc reset currentRect tuỳ ý
            picboxScreenShot.Invalidate();
        }

        private Rectangle PercentStringToImageRect(string percentRect, Bitmap image)
        {
            if (string.IsNullOrWhiteSpace(percentRect) || image == null) return Rectangle.Empty;
            var parts = percentRect.Split(',');
            if (parts.Length != 4) return Rectangle.Empty;

            if (!double.TryParse(parts[0], out var leftP)) return Rectangle.Empty;
            if (!double.TryParse(parts[1], out var topP)) return Rectangle.Empty;
            if (!double.TryParse(parts[2], out var rightP)) return Rectangle.Empty;
            if (!double.TryParse(parts[3], out var bottomP)) return Rectangle.Empty;

            int left = (int)Math.Round(leftP / 100.0 * image.Width);
            int top = (int)Math.Round(topP / 100.0 * image.Height);
            int right = (int)Math.Round(rightP / 100.0 * image.Width);
            int bottom = (int)Math.Round(bottomP / 100.0 * image.Height);

            int w = Math.Max(1, right - left);
            int h = Math.Max(1, bottom - top);

            return new Rectangle(left, top, w, h);
        }
        // Convert image rect -> control rect (dùng GetImageDisplayRect)
        private Rectangle ImageRectToControlRectSafe(PictureBox pic, Rectangle imgRect)
        {
            if (pic.Image == null || imgRect == Rectangle.Empty) return Rectangle.Empty;
            var disp = GetImageDisplayRect(pic);
            if (disp == Rectangle.Empty) return Rectangle.Empty;

            float ratio = (float)disp.Width / pic.Image.Width;
            int x = disp.X + (int)Math.Round(imgRect.X * ratio);
            int y = disp.Y + (int)Math.Round(imgRect.Y * ratio);
            int w = Math.Max(1, (int)Math.Round(imgRect.Width * ratio));
            int h = Math.Max(1, (int)Math.Round(imgRect.Height * ratio));
            return new Rectangle(x, y, w, h);
        }
        #endregion

        #region Action Test Direct 
        private void buttonLeft_Click(object sender, EventArgs e)
        {

        }

        private void buttonDown_Click(object sender, EventArgs e)
        {

        }

        private void buttonRight_Click(object sender, EventArgs e)
        {

        }

        private void buttonUp_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Support


        public PanelDevice? GetPanelDevice()
        {
            return this.panelPhone.Controls.OfType<PanelDevice>().FirstOrDefault();
        }

        #endregion


        // create new auto record
        private void buttomFlatRound1_Click(object sender, EventArgs e)
        {
            var tilte = txtDloopTitle.Text;
            if (string.IsNullOrEmpty(tilte))
            {
                MessageBox.Show("Please enter a title for the auto record.");
                return;
            }

            if (dLoop != null && dLoop.Name == tilte)
            {
                MessageBox.Show("An auto record with this title already exists.");
                return;
            }

            dLoop = new DLoop(tilte);
            AddRoot(dLoop);
        }
        // setting
        private void buttomFlatRound2_Click(object sender, EventArgs e)
        {
            var newNums = new Random().Next(1000, 9999);
            DLoop dLoop = new DLoop($"Auto Record {newNums}");
            AddTreeItem(dLoop);
        }

        public void AddRoot(DLoop dLoop)
        {
            var root = new TreeItem(dLoop.Name);
            root.Tag = dLoop;
            treeMain.Items.Add(root);
        }
        public void AddTreeItem(DLoop dLoop)
        {
            if (dLoop == null) return;
            if (treeItemSeleted == null)
            {
                MessageBox.Show("Please select a node to add the auto record.");
                return;
            }

            var treeItem = new TreeItem(dLoop.Name);
            treeItemSeleted.Sub.Add(treeItem);


            redoHistory.Clear();

            history.Push(new TreeAction
            {
                Type = TreeAction.ActionType.Add,
                Item = treeItem,
                ParentOld = treeItemSeleted,
                Index = treeItem.Index
            });
        }



        // Biến trạng thái
        TreeItem? draggedItem;
        TreeItem? dropTarget;
        TreeItem? treeItemSeleted;
        TreeItem? originalParent;
        int originalIndex;
        bool isDragging = false;
        System.Windows.Forms.Timer dragTimer;
        Point currentMousePos;

        // MouseDown: reset trạng thái và khởi động timer
        private void treeMain_MouseDown(object? sender, MouseEventArgs e)
        {
            treeItemSeleted = null;
            treeItemSeleted = FindNodeAt(treeMain, e.Location);
            if (treeItemSeleted == null) return;
            if (treeItemSeleted != null && treeItemSeleted.Name == dLoop?.Name) return;
            isDragging = false;
            draggedItem = null;
            dropTarget = null;
            originalParent = null;
            originalIndex = -1;
            dragTimer.Start();
        }

        // Sau 1 giây giữ chuột thì bắt đầu drag
        private void DragTimer_Tick(object? sender, EventArgs e)
        {
            dragTimer.Stop();
            if (treeItemSeleted != null && !isDragging)
            {
                draggedItem = treeItemSeleted;
                isDragging = true;

                // Lưu vị trí cũ
                originalParent = draggedItem.ParentItem;
                originalIndex = draggedItem.Index;

                // Remove khỏi parent
                draggedItem.Remove();
            }
        }

        // Click bình thường
        private void TreeMain_NodeMouseClick(object sender, AntdUI.TreeSelectEventArgs e)
        {
            //if (!isDragging && e.Button == MouseButtons.Left)
            //{
            //    treeItemSeleted = e.Item;
            //}

            if (e.Button == MouseButtons.Right)
            {
                MessageBox.Show("Right click menu to be implemented.");
            }
        }

        // MouseMove: cập nhật vị trí chuột và highlight node target duy nhất
        private void TreeMain_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isDragging && draggedItem != null)
            {
                currentMousePos = e.Location;

                var newTarget = FindNodeAt(treeMain, e.Location);

                if (dropTarget != null && dropTarget != newTarget)
                {
                    dropTarget.Back = null; // clear màu node cũ
                }

                dropTarget = newTarget;

                if (dropTarget != null)
                {
                    dropTarget.Back = Color.LightSkyBlue; // highlight node mới
                }

                treeMain.Invalidate();
            }
        }

        // Paint: vẽ ghost node theo chuột
        private void treeMain_Paint(object? sender, PaintEventArgs e)
        {
            if (isDragging && draggedItem != null)
            {
                using (Brush b = new SolidBrush(Color.FromArgb(120, Color.Gray)))
                {
                    var size = e.Graphics.MeasureString(draggedItem.Text, treeMain.Font);
                    e.Graphics.FillRectangle(b, currentMousePos.X, currentMousePos.Y, size.Width, size.Height);
                    e.Graphics.DrawString(draggedItem.Text, treeMain.Font, Brushes.Black,
                                          currentMousePos.X, currentMousePos.Y);
                }
            }
        }

        private void treeMain_MouseUp(object? sender, MouseEventArgs e)
        {
            dragTimer.Stop();

            if (isDragging && draggedItem != null)
            {
                if (dropTarget != null)
                {
                    // Remove khỏi parent cũ trước
                    if (originalParent != null)
                    {
                        originalParent.Sub.Remove(draggedItem);

                    }

                    // Add vào parent mới
                    dropTarget.Sub.Add(draggedItem);


                    // Clear redoHistory khi có hành động mới
                    redoHistory.Clear();

                    // Lưu action vào history
                    history.Push(new TreeAction
                    {
                        Type = TreeAction.ActionType.Move,
                        Item = draggedItem,
                        ParentOld = originalParent,
                        ParentNew = dropTarget,
                        Index = originalIndex
                    });

                    dropTarget.Back = null;
                    dropTarget = null;
                }
                else
                {
                    // Khôi phục node về vị trí cũ
                    if (originalParent != null)
                    {
                        originalParent.Sub.Insert(originalIndex, draggedItem);

                    }
                }
            }

            dropTarget = null;
            originalParent = null;
            originalIndex = -1;
            draggedItem = null;
            isDragging = false;
            treeMain.Refresh();
            treeMain.Invalidate();
        }


        // Hàm phụ: tìm node tại vị trí chuột
        private TreeItem? FindNodeAt(Tree tree, Point location)
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


        private void FormAutoRecord_KeyUp(object? sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F1)
            {
                SreeShotTemplate = null;
                var paneldevice = GetPanelDevice();
                if (paneldevice == null)
                    return;

                ScreenShot();
            }

            if (e.KeyCode == Keys.F5)
            {
                treeMain.Refresh();
            }

            if (e.Control && e.KeyCode == Keys.Z)
            {
                Undo();
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                Redo();
            }
        }


        Stack<TreeAction> history = new Stack<TreeAction>();
        Stack<TreeAction> redoHistory = new Stack<TreeAction>();

        public class TreeAction
        {
            public enum ActionType { Add, Remove, Move }

            public ActionType Type { get; set; }
            public TreeItem? Item { get; set; }

            // Parent cũ (trước thao tác)
            public TreeItem? ParentOld { get; set; }

            // Parent mới (sau thao tác, dùng cho Move)
            public TreeItem? ParentNew { get; set; }

            public int Index { get; set; }
        }

        public void Undo()
        {
            if (history.Count == 0) return;

            var action = history.Pop();
            if (action == null || action.Item == null) return;

            switch (action.Type)
            {
                case TreeAction.ActionType.Add:
                    // Undo Add → remove node vừa thêm
                    if (action.ParentOld != null)
                    {
                        action.ParentOld.Sub.Remove(action.Item);

                    }
                    break;

                case TreeAction.ActionType.Remove:
                    // Undo Remove → thêm lại node vào parent cũ
                    if (action.ParentOld != null)
                    {
                        // đảm bảo node không còn ở đâu khác
                        action.ParentOld.Sub.Remove(action.Item);
                        action.ParentOld.Sub.Insert(action.Index, action.Item);
                    }
                    break;

                case TreeAction.ActionType.Move:
                    // Undo Move → remove khỏi ParentNew (nơi node đang nằm sau Move)
                    if (action.ParentNew != null)
                    {
                        action.ParentNew.Sub.Remove(action.Item);

                    }

                    // Thêm lại vào ParentOld (vị trí cũ)
                    if (action.ParentOld != null)
                    {
                        action.ParentOld.Sub.Remove(action.Item);

                    }
                    break;
            }

            redoHistory.Push(action);
            treeMain.Invalidate();
        }
        public void Redo()
        {
            if (redoHistory.Count == 0) return;

            var action = redoHistory.Pop();
            if (action == null || action.Item == null) return;
            switch (action.Type)
            {
                case TreeAction.ActionType.Add:
                    // Redo Add → thêm lại node vào parent
                    if (action.ParentOld != null)
                    {
                        // đảm bảo node không còn ở đâu khác
                        action.ParentOld.Sub.Remove(action.Item);
                        action.ParentOld.Sub.Insert(action.Index, action.Item);

                    }
                    break;

                case TreeAction.ActionType.Remove:
                    // Redo Remove → xoá lại node khỏi parent
                    if (action.ParentOld != null)
                    {
                        action.ParentOld.Sub.Remove(action.Item);

                    }
                    break;

                case TreeAction.ActionType.Move:
                    // Redo Move → remove khỏi ParentOld, add lại vào ParentNew
                    if (action.ParentOld != null)
                    {
                        action.ParentOld.Sub.Remove(action.Item);

                    }

                    if (action.ParentNew != null)
                    {
                        action.ParentNew.Sub.Add(action.Item);

                    }
                    break;
            }

            history.Push(action);
            treeMain.Invalidate();
        }

        public IEnumerable<TreeItem> GetAllItems(Tree tree)
        {
            foreach (var item in tree.Items)
            {
                yield return item;

                foreach (var sub in GetAllSubItems(item))
                    yield return sub;
            }
        }

        private IEnumerable<TreeItem> GetAllSubItems(TreeItem parent)
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

        FormTestAction? formTestAction = null;
        // Test
        private void buttomFlatRound5_Click(object sender, EventArgs e)
        {
            var paneldevice = GetPanelDevice();
            if (paneldevice == null)
                return;
            if (formTestAction != null)
                formTestAction.Close();
            // Giả sử bạn có panel panelNormaln11
            Point locationOnScreen = labelNormaln2.PointToScreen(new Point(10, 0));
            if (paneldevice._PDcontroller == null) return;
            var deviceData = paneldevice._PDcontroller.DeviceData;
            formTestAction = new FormTestAction(paneldevice.phone, deviceData, atx, paneldevice);
            formTestAction.StartPosition = FormStartPosition.Manual;
            formTestAction.Location = locationOnScreen;
            formTestAction.Show();
        }

        private void buttomDrawRegion_Click(object sender, EventArgs e)
        {
            DrawBluePenTocheckImagePercentRect();
        }


    }
}
