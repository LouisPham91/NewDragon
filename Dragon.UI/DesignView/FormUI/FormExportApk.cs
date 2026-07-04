using AntdUI;
using Dragon.ControlHelper;

using Dragon.Controller.GlobalControl.Helper;
using Dragon.Database.Models;
using Dragon.DesignView.Public;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Dragon.DesignView.FormUI
{
    public partial class FormExportApk : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private float _roundRadius = 15f;
        private Color _borderColor = Color.FromArgb(168, 168, 168);
        public Size originalSize;
        public Point originalLocation;
        public bool isMaximized = false;
        string svgFile = "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 384 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M192 32L64 32C46.3 32 32 46.3 32 64l0 384c0 17.7 14.3 32 32 32l256 0c17.7 0 32-14.3 32-32l0-256-96 0c-35.3 0-64-28.7-64-64l0-96zM338.7 160L224 45.3 224 128c0 17.7 14.3 32 32 32l82.7 0zM0 64C0 28.7 28.7 0 64 0L197.5 0c17 0 33.3 6.7 45.3 18.7L365.3 141.3c12 12 18.7 28.3 18.7 45.3L384 448c0 35.3-28.7 64-64 64L64 512c-35.3 0-64-28.7-64-64L0 64z\"/></svg>";
        public FormExportApk()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            EnableFormDrag(panel1);
            EnableFormDrag(panelRoundn1);

            tree1.NodeMouseClick += Tree1_NodeMouseClick1;

            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            

            BackColor = ThemeHelper.BorderNormalFist;
            ForeColor = ThemeHelper.ForeNormalFirst;

            panelMain.BackColor = ThemeHelper.BackNormalFirst;
            panelMain.ForeColor = ThemeHelper.ForeNormalFirst;

            panel1.BackColor = ThemeHelper.BackNormalFirst;
            panelRoundn1.BackColor = ThemeHelper.BackNormalFirst;

            PictureBoxCloseForm.ApplyTheme();
            buttonExportFile.ApplyTheme();
            labelFileApk.ApplyTheme();
            buttomApkFolder.ApplyTheme();

            tree1.BackColor = ThemeHelper.BackNormalFirst;
            tree1.ForeColor = ThemeHelper.ForeNormalFirst;
            foreach (var node in GetAllItems(tree1))
            {
                node.Back = ThemeHelper.BackNormalFirst;
                node.Fore = ThemeHelper.ForeNormalFirst;
                var height = Font.Height - 2;
                node.Icon = SvgRenderer.RenderSvgFromString(svgFile, height, height, ThemeHelper.ForeNormalFirst);
            }

            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                textBoxSearch.BackColor = Color.White;
                textBoxSearch.ForeColor = Color.Black;
            }
            else
            {
                textBoxSearch.BackColor = Color.FromArgb(31, 31, 31);
                textBoxSearch.ForeColor = Color.White;
            }
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

        public class AndroidPackage
        {
            public string PackageName { get; set; } = string.Empty;
            public string PathApk { get; set; } = string.Empty;
        }
        Phone? Phone = null;
        public void GetApkPackage(Phone? phone = null)
        {
            if (phone == null) return;
            Phone = phone;

            var stringPackages = CMD.ExecuteAdb($"adb -s {Phone.Serial} shell pm list package") ?? string.Empty;
            if (!string.IsNullOrEmpty(stringPackages))
            {
                var ListPackages = new List<AndroidPackage>();
                var lines = stringPackages.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                int count = 0;
                foreach (var line in lines)
                {
                    var packeageName = line.Replace("package:", "").Trim();

                    AndroidPackage androidPackage = new AndroidPackage();
                    androidPackage.PackageName = packeageName;
                    ListPackages.Add(androidPackage);

                    var root = new TreeItem(packeageName);

                    root.Tag = androidPackage;
                    var height = Font.Height - 2;
                    root.Icon = SvgRenderer.RenderSvgFromString(svgFile, height, height, ThemeHelper.ForeNormalFirst);


                    tree1.Items.Add(root);
                    count++;
                }
            }
        }

        string Pathapk = string.Empty;
        private void Tree1_NodeMouseClick1(object sender, AntdUI.TreeSelectEventArgs e)
        {
            Pathapk = string.Empty;
            if (e.Item.Tag is AndroidPackage file)
            {
                if (string.IsNullOrEmpty(file.PathApk))
                {
                    if (Phone == null || string.IsNullOrEmpty(Phone.Serial) || file == null) return;

                    var pathApk = CMD.ExecuteAdb($"adb -s {Phone.Serial} shell pm path {file.PackageName}") ?? string.Empty;
                    string path = string.Empty;
                    if (!string.IsNullOrEmpty(pathApk))
                    {
                        var paths = pathApk.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (paths != null && paths.Count() > 0)
                        {
                            file.PathApk = paths.First().Replace("package:", "").Trim();
                        }
                        else
                        {
                            file.PathApk = pathApk;
                        }
                    }
                }

                Pathapk = file.PathApk;
                labelFileApk.Text = file.PathApk;
            }
        }

        public void FindNodeByPackageName(Tree tree, string packageName, bool ShowAll = false)
        {
            if (ShowAll)
            {
                foreach (var item in tree.Items)
                {
                    item.Visible = true;
                }
                return;
            }
            else
            {
                foreach (var item in tree.Items)
                {
                    var found = FindNodeRecursive(item, packageName);
                    if (found != null)
                    {
                        found.Visible = true;

                    }
                    else
                    {
                        item.Visible = false;
                    }
                }
            }

        }

        private TreeItem? FindNodeRecursive(TreeItem node, string packageName)
        {
            if (node.Tag is AndroidPackage pkg && pkg.PackageName.Contains(packageName, StringComparison.OrdinalIgnoreCase))
            {
                return node;
            }

            if (node.Sub != null && node.Sub.Count > 0)
            {
                foreach (var sub in node.Sub)
                {
                    var found = FindNodeRecursive(sub, packageName);
                    if (found != null) return found;
                }
            }

            return null;
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

        private void buttonExportFile_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Pathapk) && Phone != null)
            {
                bool isSu = false;
                var checkSu = CMD.ExecuteAdb($"adb -s {Phone.Serial} shell which su") ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(checkSu) && checkSu.Contains("su"))
                    isSu = true;
                var dir = Directory.GetCurrentDirectory();
                var pathSave = Path.Combine(dir, "Extension", "Apk");
                if (!Directory.Exists(pathSave))
                    Directory.CreateDirectory(pathSave);
                CMD.ExecuteAdb($"adb -s {Phone.Serial} shell mkdir /sdcard/TemplateFolder");
                bool isPermissionDenine = false;
                if (isSu)
                {
                    CMD.ExecuteAdb($"adb -s {Phone.Serial} shell su -c \"cp {Pathapk} /sdcard/TemplateFolder/{Path.GetFileName(Pathapk)}\"");
                    CMD.ExecuteAdb($"adb -s {Phone.Serial} pull /sdcard/TemplateFolder/{Path.GetFileName(Pathapk)} \"{pathSave}\"");
                    CMD.ExecuteAdb($"adb -s {Phone.Serial} shell rm /sdcard/TemplateFolder/{Path.GetFileName(Pathapk)}");
                }
                else
                {
                    var checkString = CMD.ExecuteAdb($"adb -s {Phone.Serial} shell \"cp {Pathapk} /sdcard/TemplateFolder/{Path.GetFileName(Pathapk)}\"");
                    if (!string.IsNullOrWhiteSpace(checkString) && checkString.Contains("denied"))
                    {
                        isPermissionDenine = true;
                    }
                    else
                    {
                        CMD.ExecuteAdb($"adb -s {Phone.Serial} pull /sdcard/TemplateFolder/{Path.GetFileName(Pathapk)} \"{pathSave}\"");
                    }

                    CMD.ExecuteAdb($"adb -s {Phone.Serial} shell rm /sdcard/TemplateFolder/{Path.GetFileName(Pathapk)}");
                }


                if (isPermissionDenine)
                {
                    MessageBox.Show(
                        "Unable to export file. Root access is required.",
                        "File Export",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                else
                {
                    MessageBox.Show(
                     $"File export completed successfully.\nFile name: {Path.GetFileName(Pathapk)}",
                     "File Export",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Information
                    );

                }
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBoxSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                FindNodeByPackageName(tree1, textBoxSearch.Text);
            }
            else
            {
                FindNodeByPackageName(tree1, textBoxSearch.Text, true);
            }

        }

        private void buttomApkFolder_Click(object sender, EventArgs e)
        {
            if (Phone == null) return;
            var dir = Directory.GetCurrentDirectory();
            var folder = Path.Combine(dir, "Extension", "Apk", Phone.Model);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            System.Diagnostics.Process.Start("explorer.exe", folder);
        }
    }
}
