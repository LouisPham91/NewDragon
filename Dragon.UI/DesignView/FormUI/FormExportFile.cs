using AntdUI;
using Dragon.ControlHelper;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Database.Models;
using Dragon.DesignView.Public;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Dragon.DesignView.FormUI
{
    public partial class FormExportFile : Form
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private float _roundRadius = 15f;
        private Color _borderColor = Color.FromArgb(168, 168, 168);
        string svgFile = "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 512 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M64 416l384 0c17.7 0 32-14.3 32-32l0-240c0-17.7-14.3-32-32-32l-149.3 0c-13.8 0-27.3-4.5-38.4-12.8L221.9 70.4c-5.5-4.2-12.3-6.4-19.2-6.4L64 64C46.3 64 32 78.3 32 96l0 288c0 17.7 14.3 32 32 32zm384 32L64 448c-35.3 0-64-28.7-64-64L0 96C0 60.7 28.7 32 64 32l138.7 0c13.8 0 27.3 4.5 38.4 12.8l38.4 28.8c5.5 4.2 12.3 6.4 19.2 6.4L448 80c35.3 0 64 28.7 64 64l0 240c0 35.3-28.7 64-64 64z\"/></svg>";
        string svgFolder = "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 384 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M192 32L64 32C46.3 32 32 46.3 32 64l0 384c0 17.7 14.3 32 32 32l256 0c17.7 0 32-14.3 32-32l0-256-96 0c-35.3 0-64-28.7-64-64l0-96zM338.7 160L224 45.3 224 128c0 17.7 14.3 32 32 32l82.7 0zM0 64C0 28.7 28.7 0 64 0L197.5 0c17 0 33.3 6.7 45.3 18.7L365.3 141.3c12 12 18.7 28.3 18.7 45.3L384 448c0 35.3-28.7 64-64 64L64 512c-35.3 0-64-28.7-64-64L0 64z\"/></svg>";
        string svgSysmlink = "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 576 512\"><!--! Font Awesome Pro 7.0.0 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2025 Fonticons, Inc. --><path fill=\"currentColor\" d=\"M341.1 92.5c8.2 6.9 15.7 14.5 22.6 22.7 19.8-17.4 45.3-27.1 71.8-27.1 59.9 0 108.5 48.6 108.5 108.5 0 28.8-11.4 56.3-31.8 76.7l-79.1 79.1c-20.3 20.3-47.9 31.8-76.7 31.8-59.9 0-108.5-48.6-108.5-108.5 0-15.6 3.4-30.9 9.7-44.9 3.7-8 .1-17.5-7.9-21.2s-17.5-.1-21.2 7.9c-8.2 18.1-12.6 37.9-12.6 58.2 0 77.6 62.9 140.5 140.5 140.5 37.2 0 73-14.8 99.3-41.1l79.1-79.1c26.3-26.3 41.1-62.1 41.1-99.3 0-77.6-62.9-140.5-140.5-140.5-35 0-68.6 13.1-94.4 36.5zM219.5 128c59.9 0 108.5 48.6 108.5 108.5 0 15.6-3.4 30.9-9.7 44.9-3.7 8-.1 17.5 7.9 21.2s17.5 .1 21.2-7.9c8.2-18.1 12.6-37.9 12.6-58.2 0-77.6-62.9-140.5-140.5-140.5-37.2 0-73 14.8-99.3 41.1L41.1 216.2c-26.3 26.3-41.1 62.1-41.1 99.3 0 77.6 62.9 140.5 140.5 140.5 35 0 68.6-13.1 94.4-36.5-8.2-6.9-15.7-14.5-22.6-22.7-19.8 17.4-45.3 27.1-71.8 27.1-59.9 0-108.5-48.6-108.5-108.5 0-28.8 11.4-56.3 31.8-76.7l79.1-79.1c20.3-20.3 47.9-31.8 76.7-31.8z\"/></svg>";

        public FormExportFile()
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
            //TreeviewTest();
            tree1.BeforeExpand += tree1_BeforeExpand;
            tree1.NodeMouseClick += Tree1_NodeMouseClick1;
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
            buttomRound1.ApplyTheme();
            labelFile.ApplyTheme();

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

        public class AndroidFile
        {
            public string Name { get; set; } = "";
            public TypeFile TypeFile { get; set; }
            public string FullPath { get; set; } = "";
            public string SymlinkTarget { get; set; } = ""; // <-- cột mới
            public long Size { get; set; }
            public string Owner { get; set; } = "";
            public string Group { get; set; } = "";
        }
        public enum TypeFile
        {
            File,
            Folder,
            Sysmlink
        }
        Phone? Phone = null;


        private static readonly Regex LsBoth = new Regex(
            @"^(?<perm>.{10})\s+" + // 1. quyền
            @"(?:(?<links>\d+)\s+)?" + // 2. số link - optional (Android 9 có, 6 không)
            @"(?<owner>\S+)\s+" + // 3. owner
            @"(?<group>\S+)\s+" + // 4. group
            @"(?:(?<size>\d+)\s+)?" + // 5. size - optional (folder Android 6 để trống)
            @"(?<date>\d{4}-\d{2}-\d{2})\s+" + // 6. ngày
            @"(?<time>\d{2}:\d{2})\s+" + // 7. giờ
            @"(?<name>.+)$", // 8. tên (+ -> target nếu symlink)
            RegexOptions.Compiled);

        public void GetFileFolder(Phone? phone = null)
        {
            if (phone == null) return;
            Phone = phone;

            bool isSu = (CMD.ExecuteAdb($"adb -s {Phone.Serial} shell which su") ?? "").Contains("su");
            string raw = CMD.ExecuteAdb($"adb -s {Phone.Serial} shell {(isSu ? "su -c \"ls -l -a /\"" : "ls -l -a /")}") ?? "";

            tree1.Items.Clear();

            foreach (var line in raw.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var l = line.Trim();
                if (l.StartsWith("total") || l.StartsWith("ls:") || l.Contains("Permission denied")) continue;

                var m = LsBoth.Match(l);
                if (!m.Success) continue;

                char t = m.Groups["perm"].Value[0];
                string nameFull = m.Groups["name"].Value;
                if (nameFull == "." || nameFull == "..") continue;

                string name = nameFull, target = "";
                if (t == 'l' && nameFull.Contains(" -> "))
                {
                    var p = nameFull.Split(new[] { " -> " }, 2, StringSplitOptions.None);
                    name = p[0]; target = p[1];
                }

                var af = new AndroidFile
                {
                    Name = name,
                    FullPath = "/" + name,
                    SymlinkTarget = target,
                    Owner = m.Groups["owner"].Value,
                    Group = m.Groups["group"].Value,
                    Size = long.TryParse(m.Groups["size"].Value, out var s) ? s : 0,
                    TypeFile = t == 'd' ? TypeFile.Folder : t == 'l' ? TypeFile.Sysmlink : TypeFile.File
                };

                var node = new TreeItem(name) { Tag = af };
                int h = Font.Height - 2;
                node.Icon = af.TypeFile switch
                {
                    TypeFile.Folder => SvgRenderer.RenderSvgFromString(svgFolder, h, h, ThemeHelper.ForeNormalFirst),
                    TypeFile.Sysmlink => SvgRenderer.RenderSvgFromString(svgSysmlink, h, h, ThemeHelper.ForeNormalFirst),
                    _ => SvgRenderer.RenderSvgFromString(svgFile, h, h, ThemeHelper.ForeNormalFirst)
                };

                if (af.TypeFile != TypeFile.File) AddTreeItem(af, node);
                tree1.Items.Add(node);
            }
        }

        public void AddTreeItem(AndroidFile androidFile, TreeItem treeItem)
        {
            if (Phone == null || string.IsNullOrEmpty(Phone.Serial) || androidFile == null || string.IsNullOrEmpty(androidFile.FullPath))
                return;

            bool isSu = (CMD.ExecuteAdb($"adb -s {Phone.Serial} shell which su") ?? "").Contains("su");
            string cmd = isSu
                ? $"su -c \"ls -l -a '{androidFile.FullPath}'\""
                : $"ls -l -a '{androidFile.FullPath}'";

            string notes = CMD.ExecuteAdb($"adb -s {Phone.Serial} shell {cmd}") ?? "";
            if (string.IsNullOrWhiteSpace(notes)) return;

            treeItem.Sub.Clear(); // xóa node "dummy" nếu có

            foreach (var raw in notes.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var line = raw.Trim();
                if (line.StartsWith("total") || line.StartsWith("ls:") || line.Contains("Permission denied"))
                    continue;

                var m = LsBoth.Match(line);
                if (!m.Success) continue;

                char t = m.Groups["perm"].Value[0];
                string nameFull = m.Groups["name"].Value;
                if (nameFull == "." || nameFull == "..") continue;

                string name = nameFull;
                string target = "";
                if (t == 'l' && nameFull.Contains(" -> "))
                {
                    var sp = nameFull.Split(new[] { " -> " }, 2, StringSplitOptions.None);
                    name = sp[0];
                    target = sp[1];
                }

                string full = androidFile.FullPath == "/" ? $"/{name}" : $"{androidFile.FullPath}/{name}";

                var child = new AndroidFile
                {
                    Name = name,
                    FullPath = full,
                    SymlinkTarget = target,
                    Owner = m.Groups["owner"].Value,
                    Group = m.Groups["group"].Value,
                    Size = long.TryParse(m.Groups["size"].Value, out var sz) ? sz : 0,
                    TypeFile = t == 'd' ? TypeFile.Folder : t == 'l' ? TypeFile.Sysmlink : TypeFile.File
                };

                // nếu là symlink trỏ tuyệt đối thì cho FullPath trỏ thẳng
                if (child.TypeFile == TypeFile.Sysmlink && target.StartsWith("/"))
                    child.FullPath = target;

                var node = new TreeItem(name) { Tag = child };
                int h = Font.Height - 2;
                node.Icon = child.TypeFile switch
                {
                    TypeFile.Folder => SvgRenderer.RenderSvgFromString(svgFolder, h, h, ThemeHelper.ForeNormalFirst),
                    TypeFile.Sysmlink => SvgRenderer.RenderSvgFromString(svgSysmlink, h, h, ThemeHelper.ForeNormalFirst),
                    _ => SvgRenderer.RenderSvgFromString(svgFile, h, h, ThemeHelper.ForeNormalFirst)
                };

                if (child.TypeFile != TypeFile.File)
                    node.Sub.Add(new TreeItem("...") { Tag = "dummy" }); // để lần sau expand tiếp

                treeItem.Sub.Add(node);
            }
        }


        private void tree1_BeforeExpand(object sender, AntdUI.TreeExpandEventArgs e)
        {
            if (Phone == null || !e.Value || e.Item.Tag is not AndroidFile androidFile) return;
            if (string.IsNullOrEmpty(Phone.Serial)) return;

            // nếu đã load rồi thì bỏ qua, nếu chỉ có node "dummy" thì load
            if (e.Item.Sub.Count > 0 && (e.Item.Sub[0].Tag?.ToString() != "dummy")) return;

            bool isSu = (CMD.ExecuteAdb($"adb -s {Phone.Serial} shell which su") ?? "").Contains("su");
            string cmd = isSu
               ? $"adb -s {Phone.Serial} shell su -c \"ls -l -a '{androidFile.FullPath}'\""
                : $"adb -s {Phone.Serial} shell ls -l -a \"{androidFile.FullPath}\"";

            string notes = CMD.ExecuteAdb(cmd) ?? "";
            if (string.IsNullOrWhiteSpace(notes)) return;

            e.Item.Sub.Clear();
            int height = Font.Height - 2;

            foreach (var raw in notes.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var line = raw.Trim();
                if (line.StartsWith("total") || line.StartsWith("ls:") || line.Contains("Permission denied"))
                    continue;

                var m = LsBoth.Match(line);
                if (!m.Success) continue;

                char t = m.Groups["perm"].Value[0];
                string nameFull = m.Groups["name"].Value;
                if (nameFull == "." || nameFull == "..") continue;

                string name = nameFull;
                string target = "";
                if (t == 'l' && nameFull.Contains(" -> "))
                {
                    var sp = nameFull.Split(new[] { " -> " }, 2, StringSplitOptions.None);
                    name = sp[0];
                    target = sp[1];
                }

                string full = androidFile.FullPath == "/" ? $"/{name}" : $"{androidFile.FullPath}/{name}";

                var child = new AndroidFile
                {
                    Name = name,
                    FullPath = full,
                    SymlinkTarget = target,
                    Owner = m.Groups["owner"].Value,
                    Group = m.Groups["group"].Value,
                    Size = long.TryParse(m.Groups["size"].Value, out var sz) ? sz : 0,
                    TypeFile = t == 'd' ? TypeFile.Folder : t == 'l' ? TypeFile.Sysmlink : TypeFile.File
                };

                // symlink trỏ tuyệt đối thì cho nhảy thẳng
                if (child.TypeFile == TypeFile.Sysmlink && target.StartsWith("/"))
                    child.FullPath = target;

                var node = new AntdUI.TreeItem(name) { Tag = child };
                node.Icon = child.TypeFile switch
                {
                    TypeFile.Folder => SvgRenderer.RenderSvgFromString(svgFolder, height, height, ThemeHelper.ForeNormalFirst),
                    TypeFile.Sysmlink => SvgRenderer.RenderSvgFromString(svgSysmlink, height, height, ThemeHelper.ForeNormalFirst),
                    _ => SvgRenderer.RenderSvgFromString(svgFile, height, height, ThemeHelper.ForeNormalFirst)
                };

                // để lần sau expand tiếp
                if (child.TypeFile != TypeFile.File)
                    node.Sub.Add(new AntdUI.TreeItem("...") { Tag = "dummy" });

                e.Item.Sub.Add(node);
            }
        }
        private void Tree1_NodeMouseClick1(object sender, AntdUI.TreeSelectEventArgs e)
        {
            // Lấy text hiển thị
            var text = e.Item.Text;

            // Nếu lúc tạo node bạn có gán AndroidFile vào Tag
            if (e.Item.Tag is AndroidFile file)
            {
                if (file.TypeFile == TypeFile.File)
                {
                    labelFile.Text = file.FullPath;
                }
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

        private void buttonExportFile_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(labelFile.Text) && Phone != null)
            {
                var dir = Directory.GetCurrentDirectory();
                var path = Path.Combine(dir, "Extension", "File", $"{Phone.Model}");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                bool isPermissionDenine = false;
                CMD.ExecuteAdb($"adb -s {Phone.Serial} shell mkdir /sdcard/TemplateFolder");
                var checkSu = CMD.ExecuteAdb($"adb -s {Phone.Serial} shell which su");
                if (!string.IsNullOrWhiteSpace(checkSu) && checkSu.Contains("su"))
                {
                    CMD.ExecuteAdb($"adb -s {Phone.Serial} shell su -c \"cp {labelFile.Text} /sdcard/TemplateFolder/{Path.GetFileName(labelFile.Text)}\"");
                    CMD.ExecuteAdb($"adb -s {Phone.Serial} pull /sdcard/TemplateFolder/{Path.GetFileName(labelFile.Text)} \"{path}\"");
                    CMD.ExecuteAdb($"adb -s {Phone.Serial} shell rm /sdcard/TemplateFolder/{Path.GetFileName(labelFile.Text)}");

                }
                else
                {
                    var checkString = CMD.ExecuteAdb($"adb -s {Phone.Serial} shell \"cp {labelFile.Text} /sdcard/TemplateFolder/{Path.GetFileName(labelFile.Text)}\"");
                    if (!string.IsNullOrWhiteSpace(checkString) && checkString.Contains("denied"))
                    {
                        isPermissionDenine = true;
                    }
                    else
                    {
                        CMD.ExecuteAdb($"adb -s {Phone.Serial} pull /sdcard/TemplateFolder/{Path.GetFileName(labelFile.Text)} \"{path}\"");
                    }

                    if (!isPermissionDenine)
                        CMD.ExecuteAdb($"adb -s {Phone.Serial} shell rm /sdcard/TemplateFolder/{Path.GetFileName(labelFile.Text)}");
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
                     $"File export completed successfully.\nFile name: {Path.GetFileName(labelFile.Text)}",
                     "File Export",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Information
                 );
                }




            }
        }
        private void buttomRound1_Click(object sender, EventArgs e)
        {
            if (Phone == null) return;
            var dir = Directory.GetCurrentDirectory();
            var folder = Path.Combine(dir, "Extension", "File", Phone.Model);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            System.Diagnostics.Process.Start("explorer.exe", folder);
        }
        private void FormAndroidExplorer_Load(object sender, EventArgs e)
        {

        }


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
            if (node.Tag is AndroidFile pkg && pkg.Name.Contains(packageName, StringComparison.OrdinalIgnoreCase))
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
    }
}
