using Dragon.Database.Models;
using Dragon.DesignView.Public.NormalMode;
using System.Diagnostics;

namespace Dragon.DesignView.Public
{
    public static class GridViewBinders
    {
        private static void ApplyThemeToColumns(DataGridView dgv)
        {
            foreach (DataGridViewColumn c in dgv.Columns)
            {
                c.DefaultCellStyle.BackColor = dgv.DefaultCellStyle.BackColor;
                c.DefaultCellStyle.ForeColor = dgv.DefaultCellStyle.ForeColor;
                c.DefaultCellStyle.SelectionBackColor = dgv.DefaultCellStyle.SelectionBackColor;
                c.DefaultCellStyle.SelectionForeColor = dgv.DefaultCellStyle.SelectionForeColor;
            }
        }

        // ===== 1. AppData =====
        public static void BindAppData(AotSafeDataGridView dgv, List<AppData> data, List<string>? hide = null)
        {
            hide ??= new();
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Clear();

            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AppData.guId), HeaderText = "GUID", Width = 80, Visible = false });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AppData.DeviceID), HeaderText = "DeviceID", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AppData.NetworkName), HeaderText = "Network", Width = 90 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AppData.PackageName), HeaderText = "Package", Width = 120 });
            dgv.Columns.Add(new DataGridViewComboBoxColumn { DataPropertyName = nameof(AppData.AccStatus), HeaderText = "Status", DataSource = EnumCache.AccStatuses, Width = 90, FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AppData.Email), HeaderText = "Email", Width = 150 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AppData.Username), HeaderText = "User", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AppData.HoTen), HeaderText = "Họ tên", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AppData.BirtDay), HeaderText = "NS", Width = 80 });
            dgv.Columns.Add(new DataGridViewComboBoxColumn { DataPropertyName = nameof(AppData.Gender), HeaderText = "GT", DataSource = EnumCache.Genders, Width = 70, FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing });
            dgv.Columns.Add(new DataGridViewComboBoxColumn { DataPropertyName = nameof(AppData.Marital), HeaderText = "HN", DataSource = EnumCache.Maritals, Width = 80, FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AppData.Phone), HeaderText = "Phone", Width = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AppData.FriendCount), HeaderText = "Bạn", Width = 50 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AppData.FollowCount), HeaderText = "Follow", Width = 60 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AppData.AppVersion), HeaderText = "Ver", Width = 70 });

            foreach (var c in dgv.Columns.Cast<DataGridViewColumn>().Where(c => hide.Contains(c.DataPropertyName)))
                c.Visible = false;

            ApplyThemeToColumns(dgv);
            dgv.DataSource = data;
        }

        // ===== 2. Phone =====
        public static void BindPhone(AotSafeDataGridView dgv, List<Phone> data,
    List<string>? hide = null,
    List<(string old, string name, int w)>? rename = null,
    Action<DataGridView>? customize = null) // <-- THÊM
        {
            hide ??= new();
            rename ??= new();
            data ??= new List<Phone>();

            // CHỈ TẠO CỘT LẦN ĐẦU, các lần sau giữ nguyên để custom không bị xóa
            if (dgv.Columns.Count == 0)
            {
                dgv.AutoGenerateColumns = false;

                dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Phone.Id), HeaderText = "ID", Name = "Id", Width = 50 });
                dgv.Columns.Add(new DataGridViewImageColumn { DataPropertyName = nameof(Phone.Im), HeaderText = "", Name = "Im", Width = 30, ImageLayout = DataGridViewImageCellLayout.Normal });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Phone.PhoneTagNumber), HeaderText = "Tag", Name = "PhoneTagNumber", Width = 50 });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Phone.Serial), HeaderText = "Serial", Name = "Serial", Width = 120 });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Phone.DeviceID), HeaderText = "DeviceId", Name = "DeviceID", Width = 120 });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Phone.Ipv4), HeaderText = "IP", Name = "Ipv4", Width = 100 });
                dgv.Columns.Add(new DataGridViewComboBoxColumn { DataPropertyName = nameof(Phone.DeviceState), HeaderText = "DeviceState", Name = "DeviceState", DataSource = EnumCache.States, Width = 90, FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing });
                dgv.Columns.Add(new DataGridViewComboBoxColumn { DataPropertyName = nameof(Phone.PhoneMode), HeaderText = "Mode", Name = "PhoneMode", DataSource = EnumCache.PhoneModes, Width = 70, FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Phone.Model), HeaderText = "Model", Name = "Model", Width = 100 });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Phone.AndroidVersion), HeaderText = "Android", Name = "AndroidVersion", Width = 70 });
                dgv.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = nameof(Phone.Online), HeaderText = "Online", Name = "IsOnline", Width = 60 });
                dgv.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = nameof(Phone.IsRooted), HeaderText = "Root", Name = "IsRooted", Width = 50 });
                dgv.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = nameof(Phone.IsRunning), HeaderText = "Run", Name = "IsRunning", Width = 50 });
            }

            // ẩn cột theo logic cũ của bạn
            foreach (DataGridViewColumn c in dgv.Columns)
            {
                if (hide.Contains(c.Name))
                {
                    Debug.WriteLine("hide col:" + c.DataPropertyName);
                    c.Visible = false;
                }
                else
                {
                    Debug.WriteLine("Show col:" + c.DataPropertyName);
                    c.Visible = true;
                }
            }


            // rename + set width + sắp xếp thứ tự
            int i = 0;
            foreach (var (oldName, newName, w) in rename)
            {
                if (dgv.Columns.Contains(oldName))
                {
                    var c = dgv.Columns[oldName];
                    if (!string.IsNullOrEmpty(newName)) c?.HeaderText = newName;
                    if (w > 0) c?.Width = w;
                    c?.DisplayIndex = i++;
                }
            }

            ApplyThemeToColumns(dgv);

            // CHẠY CODE CUSTOM CỦA BẠN Ở ĐÂY
            customize?.Invoke(dgv);

            // bind lại, kể cả list rỗng
            dgv.DataSource = null;
            dgv.DataSource = data;
        }

        // ===== 3. SocialNetwork =====
        public static void BindSocialNetwork(AotSafeDataGridView dgv, List<SocialNetwork> data, List<string>? hide = null)
        {
            hide ??= new();
            dgv.AutoGenerateColumns = false; dgv.Columns.Clear();
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(SocialNetwork.guId), HeaderText = "ID", Visible = false });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(SocialNetwork.NetworkName), HeaderText = "Network", Width = 150 });
            foreach (var c in dgv.Columns.Cast<DataGridViewColumn>().Where(c => hide.Contains(c.DataPropertyName)))
                c.Visible = false;

            ApplyThemeToColumns(dgv);
            dgv.DataSource = data;
        }

        // ===== 4. KeybroadSetting =====
        public static void BindKeybroad(AotSafeDataGridView dgv, List<KeybroadSetting> data, List<string>? hide = null)
        {
            hide ??= new();
            dgv.AutoGenerateColumns = false; dgv.Columns.Clear();
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(KeybroadSetting.Id), HeaderText = "ID", Width = 50 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(KeybroadSetting.DeviceId), HeaderText = "Device", Width = 120 });
            dgv.Columns.Add(new DataGridViewComboBoxColumn { DataPropertyName = nameof(KeybroadSetting.Langeuage), HeaderText = "Lang", DataSource = EnumCache.Langeuages, Width = 100, FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(KeybroadSetting.IMEId), HeaderText = "IME", Width = 150 });
            foreach (var c in dgv.Columns.Cast<DataGridViewColumn>().Where(c => hide.Contains(c.DataPropertyName)))
                c.Visible = false;

            ApplyThemeToColumns(dgv);
            dgv.DataSource = data;
        }

        // ===== 5. IntentInfo =====
        public static void BindIntentInfo(AotSafeDataGridView dgv, List<IntentInfo> data, List<string>? hide = null)
        {
            hide ??= new();
            dgv.AutoGenerateColumns = false; dgv.Columns.Clear();
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(IntentInfo.Id), HeaderText = "ID", Width = 50 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(IntentInfo.Model), HeaderText = "Model", Width = 90 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(IntentInfo.PackageName), HeaderText = "Package", Width = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(IntentInfo.VersionName), HeaderText = "Ver", Width = 70 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(IntentInfo.VersionCode), HeaderText = "Code", Width = 60 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(IntentInfo.API), HeaderText = "API", Width = 50 });
            dgv.Columns.Add(new DataGridViewComboBoxColumn { DataPropertyName = nameof(IntentInfo.ResolverType), HeaderText = "Resolver", DataSource = EnumCache.ResolverTypes, Width = 90, FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing });
            dgv.Columns.Add(new DataGridViewComboBoxColumn { DataPropertyName = nameof(IntentInfo.TypeAction), HeaderText = "ActionType", DataSource = EnumCache.TypeActions, Width = 90, FlatStyle = FlatStyle.Flat, DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(IntentInfo.Action), HeaderText = "Action", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(IntentInfo.Scheme), HeaderText = "Scheme", Width = 80 });
            dgv.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = nameof(IntentInfo.AutoVerify), HeaderText = "Auto", Width = 50 });
            dgv.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = nameof(IntentInfo.RequiresRoot), HeaderText = "Root", Width = 50 });

            foreach (var c in dgv.Columns.Cast<DataGridViewColumn>().Where(c => hide.Contains(c.DataPropertyName)))
                c.Visible = false;

            ApplyThemeToColumns(dgv);
            dgv.DataSource = data;
        }

        // ===== 6. AdbCommandIntent =====
        public static void BindAdbCommand(AotSafeDataGridView dgv, List<AdbCommandIntent> data, List<string>? hide = null)
        {
            hide ??= new();
            dgv.AutoGenerateColumns = false; dgv.Columns.Clear();
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AdbCommandIntent.Id), HeaderText = "ID", Width = 50 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AdbCommandIntent.Model), HeaderText = "Model", Width = 90 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AdbCommandIntent.PackageName), HeaderText = "Package", Width = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AdbCommandIntent.Name), HeaderText = "Name", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(AdbCommandIntent.Command), HeaderText = "Command", Width = 300 });
            foreach (var c in dgv.Columns.Cast<DataGridViewColumn>().Where(c => hide.Contains(c.DataPropertyName)))
                c.Visible = false;

            ApplyThemeToColumns(dgv);
            dgv.DataSource = data;
        }
    }


}