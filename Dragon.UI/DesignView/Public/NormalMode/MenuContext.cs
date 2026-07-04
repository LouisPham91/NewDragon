
using AdvancedSharpAdbClient;
using Dragon.ControlHelper.UIController;
using Dragon.Database.Models;
using Dragon.DesignView.FormUI;
using System.ComponentModel;


namespace Dragon.DesignView.Public.NormalMode
{
    public static class ToolStripHelper
    {
        public static void AddInfoPanel(this ToolStripDropDown dropDown, TableLayoutPanel panel)
        {
            var host = new ToolStripControlHost(panel)
            {
                AutoSize = false,
                Size = panel.PreferredSize
            };
            dropDown.Items.Add(host);
        }
    }

    public class MenuContext
    {
        public AutoSizeContextMenu MenuADB { get; } = new();
        public AutoSizeContextMenu MenuSwitchMode { get; } = new();
        private readonly Phone? _phone;
        private readonly AdbClient _adbClient;
        public MenuContext(Phone phone, AdbClient adbClient)
        {
            _phone = phone;
            _adbClient = adbClient;
            BuildMenuAdb();
            BuildMenuSwitchMode();
        }

        public void MenuADBShow(Point? point = null) =>
            MenuADB.Show(point ?? Cursor.Position);

        public void MenuSwitchModeShow(Point? point = null) =>
            MenuSwitchMode.Show(point ?? Cursor.Position);

        public void ApplyTheme()
        {
            MenuADB.ApplyTheme();
            MenuSwitchMode.ApplyTheme();
        }

        private void BuildMenuAdb()
        {
            MenuADB.Items.Add(new ToolStripMenuItem("Type Commands")
            {
                Enabled = false,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            });

            MenuADB.Items.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("Build Command")
            });

            MenuADB.Items.Add(new ToolStripMenuItem("Manage Commands")
            {
                Enabled = false,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            });

            MenuADB.Items.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("1. WIFI"),
                new ToolStripMenuItem("2. Chrome"),
                new ToolStripMenuItem("3. Mozilla"),
                new ToolStripMenuItem("4. Edge"),
                new ToolStripMenuItem("5. Opera Mini"),
                new ToolStripMenuItem("6. DuckDuckGo"),
                new ToolStripMenuItem("7. Brave"),
                new ToolStripMenuItem("8. Kiwi"),
                new ToolStripMenuItem("9. Vivaldi"),
                new ToolStripMenuItem("10. Ecosia"),
                new ToolStripMenuItem("11. Tor")
            });

            MenuADB.Items.Add(new ToolStripMenuItem("Built-in Commands")
            {
                Enabled = false,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            });

            MenuADB.Items.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("1. Shutdown"),
                new ToolStripMenuItem("2. Volume Up"),
                new ToolStripMenuItem("3. Volume Down"),
                new ToolStripMenuItem("4. Turn on WIFI"),
                new ToolStripMenuItem("5. Turn off WIFI"),
                new ToolStripMenuItem("6. Reset DPI"),
                new ToolStripMenuItem("7. Reset resolution")
            });

            MenuADB.Items.Add(new ToolStripMenuItem("Social Apps")
            {
                Enabled = false,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            });

            MenuADB.Items.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("1. Facebook"),
                new ToolStripMenuItem("2. Facebook Lite"),
                new ToolStripMenuItem("3. Messenger"),
                new ToolStripMenuItem("4. Instagram"),
                new ToolStripMenuItem("5. Instagram Lite"),
                new ToolStripMenuItem("6. TikTok"),
                new ToolStripMenuItem("7. TikTok Lite"),
                new ToolStripMenuItem("8. Telegram"),
                new ToolStripMenuItem("9. Pinterest"),
                new ToolStripMenuItem("10. Snapchat"),
                new ToolStripMenuItem("11. LinkedIn"),
                new ToolStripMenuItem("12. Thread"),
                new ToolStripMenuItem("13. Twitter"),
                new ToolStripMenuItem("14. Lazada"),
                new ToolStripMenuItem("15. Tripadvisor"),
                new ToolStripMenuItem("16. BlueSky"),
                new ToolStripMenuItem("17. YouTube"),
                new ToolStripMenuItem("18. Discord"),
                new ToolStripMenuItem("19. WeChat"),
                new ToolStripMenuItem("20. Sina Weibo"),
                new ToolStripMenuItem("21. Tencent QQ"),
                new ToolStripMenuItem("22. Reddit"),
                new ToolStripMenuItem("23. Skype"),
                new ToolStripMenuItem("24. Skype Lite")
            });

            foreach (ToolStripItem item in MenuADB.Items)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    menuItem.Click += MenuADBItem_Click;
                }
            }
        }
        // Khai báo dictionary ánh xạ
        private readonly Dictionary<string, string> _adbCommands = new()
        {
            // Built-in Commands
            { "1. Shutdown", "reboot -p" },
            { "2. Volume Up", "input keyevent 24" },
            { "3. Volume Down", "input keyevent 25" },
            { "4. Turn on WIFI", "svc wifi enable" },
            { "5. Turn off WIFI", "svc wifi disable" },
            { "6. Reset DPI", "wm density reset" },
            { "7. Reset resolution", "wm size reset" },

            // Manage Commands (ví dụ mở app)
            { "1. WIFI", "am start -a android.settings.WIFI_SETTINGS" },
            { "2. Chrome", "monkey -p com.android.chrome 1" },
            { "3. Mozilla", "monkey -p org.mozilla.firefox 1" },
            { "4. Edge", "monkey -p com.microsoft.emmx 1" },
            { "5. Opera Mini", "monkey -p com.opera.mini.native 1" },
            { "6. DuckDuckGo", "monkey -p com.duckduckgo.mobile.android 1" },
            { "7. Brave", "monkey -p com.brave.browser 1" },
            { "8. Kiwi", "monkey -p com.kiwibrowser.browser 1" },
            { "9. Vivaldi", "monkey -p com.vivaldi.browser 1" },
            { "10. Ecosia", "monkey -p com.ecosia.android 1" },
            { "11. Tor", "monkey -p org.torproject.torbrowser 1" },

            // Social Apps
            { "1. Facebook", "monkey -p com.facebook.katana 1" },
            { "2. Facebook Lite", "monkey -p com.facebook.lite 1" },
            { "3. Messenger", "monkey -p com.facebook.orca 1" },
            { "4. Instagram", "monkey -p com.instagram.android 1" },
            { "5. Instagram Lite", "monkey -p com.instagram.lite 1" },
            { "6. TikTok", "monkey -p com.zhiliaoapp.musically 1" },
            { "7. TikTok Lite", "monkey -p com.zhiliaoapp.musically.go 1" },
            { "8. Telegram", "monkey -p org.telegram.messenger 1" },
            { "9. Pinterest", "monkey -p com.pinterest 1" },
            { "10. Snapchat", "monkey -p com.snapchat.android 1" },
            { "11. LinkedIn", "monkey -p com.linkedin.android 1" },
            { "12. Thread", "monkey -p com.instagram.barcelona 1" },
            { "13. Twitter", "monkey -p com.twitter.android 1" },
            { "14. Lazada", "monkey -p com.lazada.android 1" },
            { "15. Tripadvisor", "monkey -p com.tripadvisor.tripadvisor 1" },
            { "16. BlueSky", "monkey -p xyz.blueskyweb.app 1" },
            { "17. YouTube", "monkey -p com.google.android.youtube 1" },
            { "18. Discord", "monkey -p com.discord 1" },
            { "19. WeChat", "monkey -p com.tencent.mm 1" },
            { "20. Sina Weibo", "monkey -p com.sina.weibo 1" },
            { "21. Tencent QQ", "monkey -p com.tencent.mobileqq 1" },
            { "22. Reddit", "monkey -p com.reddit.frontpage 1" },
            { "23. Skype", "monkey -p com.skype.raider 1" },
            { "24. Skype Lite", "monkey -p com.skype.m2 1" }
        };
        private void MenuADBItem_Click(object? sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item == null || string.IsNullOrEmpty(item.Text)) return;
            if (_phone == null) return;
            var deviceDatas = _adbClient.GetDevices();
            if (deviceDatas == null || !deviceDatas.Any()) return;
            var device = deviceDatas.FirstOrDefault(c => c.Serial == _phone.Serial);
            if (device == null) return;

            if (item.Text == "Build Command")
            {
                var formActionIntent = FormManager.Instance.formActionIntent;
                if (formActionIntent != null) formActionIntent.Close();
                formActionIntent = new FormActionIntent(_phone);
                formActionIntent.StartPosition = FormStartPosition.CenterScreen;
                formActionIntent.Show();
                return;
            }

            if (_adbCommands.TryGetValue(item.Text, out var command))
            {
                _adbClient.ExecuteRemoteCommand(command, device);
            }
            else
            {
                MessageBox.Show($"Chưa gán lệnh cho: {item.Text}");
            }
        }

        private void BuildMenuSwitchMode()
        {
            MenuSwitchMode.Items.Add(new ToolStripMenuItem("Choose Mode")
            {
                Enabled = false,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            });

            MenuSwitchMode.Items.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("USB"),
                new ToolStripMenuItem("WIFI"),
                new ToolStripMenuItem("UATX"),
                new ToolStripMenuItem("WATX"),
                new ToolStripMenuItem("UHDI"),
                new ToolStripMenuItem("WHDI"),
                new ToolStripMenuItem("ACC")
            });
            foreach (ToolStripItem item in MenuSwitchMode.Items)
            {
                if (item is ToolStripMenuItem menuItem && menuItem.Enabled)
                {
                    menuItem.Click += MenuSwitchModeItem_Click;
                }
            }
        }
        private void MenuSwitchModeItem_Click(object? sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item == null || string.IsNullOrEmpty(item.Text)) return;
            if (_phone == null) return;
            var deviceDatas = _adbClient.GetDevices();
            if (deviceDatas == null || !deviceDatas.Any()) return;
            var device = deviceDatas.FirstOrDefault(c => c.Serial == _phone.Serial);
            if (device == null) return;

            switch (item.Text)
            {
                case "USB":
                   
                    break;

                case "WIFI":

                    break;

                case "UATX":
                    MessageBox.Show("WATX Mode work on runtime only");
                    break;

                case "WATX":
                    MessageBox.Show("WATX Mode work on runtime only");
                    break;

                case "UHDI":
                    
                    break;

                case "WHDI":
                   
                    break;

                case "ACC":
                    MessageBox.Show("ACC mode under development.");
                    break;

                default:
                    MessageBox.Show($"Chưa gán lệnh cho: {item.Text}");
                    break;
            }
        }
    }

    public class AutoSizeDropDownMenu : ToolStripDropDownMenu
    {
        public AutoSizeDropDownMenu()
        {
            ShowImageMargin = false;
            ShowCheckMargin = false;
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            int maxWidth = 0;
            foreach (ToolStripItem item in Items)
            {
                if (item is ToolStripSeparator || string.IsNullOrEmpty(item.Text)) continue;
                var sz = TextRenderer.MeasureText(item.Text, item.Font);
                if (sz.Width > maxWidth) maxWidth = sz.Width;
            }
            int width = maxWidth + (ShowImageMargin ? 40 : 10);
            var baseSize = base.GetPreferredSize(proposedSize);
            return new Size(width, baseSize.Height);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (OwnerItem?.Owner != null)
            {
                var parent = OwnerItem.Owner;
                var bounds = OwnerItem.Bounds;
                var screen = parent.PointToScreen(bounds.Location);
                x = screen.X + bounds.Width - width - 100;
                y = screen.Y + bounds.Height;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }
    }

    public class AutoSizeContextMenu : ContextMenuStrip, IThemeable
    {
        private bool _isNormalWidth;

        [Category("Dragon")]
        [DisplayName("DG_IsNormalWidth")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_IsNormalWidth
        {
            get => _isNormalWidth;
            set
            {
                if (_isNormalWidth == value) return;
                _isNormalWidth = value;
                if (IsHandleCreated) Invalidate();
            }
        }

        public AutoSizeContextMenu()
        {
            ShowImageMargin = false;
            ShowCheckMargin = false;
            RenderMode = ToolStripRenderMode.System;
            Font = new Font("Segoe UI", 9f);
            Renderer = new ContextMenuRenderer();
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            BackColor = ThemeHelper.BackNormalFirst;
            ForeColor = ThemeHelper.ForeNormalFirst;
            ((ContextMenuRenderer)Renderer).ApplyTheme();
            Invalidate();
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            int maxWidth = 0;
            foreach (ToolStripItem item in Items)
            {
                if (item is ToolStripSeparator || string.IsNullOrEmpty(item.Text)) continue;
                var sz = TextRenderer.MeasureText(item.Text, item.Font);
                if (sz.Width > maxWidth) maxWidth = sz.Width;
            }
            int width = maxWidth + (ShowImageMargin ? 40 : 10);
            if (_isNormalWidth) width += 27;
            var baseSize = base.GetPreferredSize(proposedSize);
            return new Size(width, baseSize.Height);
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            Invalidate();
        }

        private sealed class ContextMenuColorTable : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground => ThemeHelper.BackNormalFirst;
            public override Color MenuItemSelected => ThemeHelper.BackNormalFirst;
            public override Color MenuItemBorder => ThemeHelper.BackNormalFirst;
            public override Color ImageMarginGradientBegin => ThemeHelper.BackNormalFirst;
            public override Color ImageMarginGradientMiddle => ThemeHelper.BackNormalFirst;
            public override Color ImageMarginGradientEnd => ThemeHelper.BackNormalFirst;
        }

        private sealed class ContextMenuRenderer : ToolStripProfessionalRenderer, IThemeable
        {
            private Color _back;
            private Color _fore;

            public ContextMenuRenderer() : base(new ContextMenuColorTable())
            {
                ApplyTheme();
            }

            public void ApplyTheme()
            {
                _back = ThemeHelper.BackNormalFirst;
                _fore = ThemeHelper.ForeNormalFirst;
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e) =>
                e.Graphics.Clear(_back);

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                using var pen = new Pen(_back);
                var r = new Rectangle(Point.Empty, e.ToolStrip.Size);
                r.Width--; r.Height--;
                e.Graphics.DrawRectangle(pen, r);
            }

            protected override void OnRenderImageMargin(ToolStripRenderEventArgs e) =>
                e.Graphics.FillRectangle(new SolidBrush(_back), e.AffectedBounds);

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) =>
                e.Graphics.FillRectangle(new SolidBrush(_back), new Rectangle(Point.Empty, e.Item.Size));

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = _fore;
                base.OnRenderItemText(e);
            }
        }
    }
}