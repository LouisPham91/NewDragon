using AdvancedSharpAdbClient;

using Dragon.DesignView.Public.NormalMode;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Database.Models;
using Dragon.ControlHelper;
using Dragon.ControlHelper.UIController;
using System.ComponentModel;
using System.Data;
using Dragon.DesignView.Public;
using Dragon.Controller.Database.Services;
using Dragon.Database.Services;

namespace Dragon.DesignView.FormUI
{
    public partial class FormActionIntent : FormOriginal
    {
        AdbCommandIntent? comandSelected = null;
        List<IntentInfo> intentInfos = new List<IntentInfo>();
        List<string> packageList = new List<string>();
        private List<string> _allCommandPackages = new List<string>();

        public FormActionIntent(Phone phonez)
        {
            InitializeComponent();

            // 1. Xóa hết phần tử cũ (nếu có)
            comboxPhoneList.Items.Clear();

            var phonelist = PhoneBoxRepository.LoadAll();
            foreach (var phones in phonelist)
            {
                comboxPhoneList.Items.Add(phones);
            }
            // 3. Thiết lập chọn phần tử đầu (tuỳ ý)
            if (comboxPhoneList.Items.Count > 0)
                comboxPhoneList.SelectedIndex = phonelist.FindIndex(c => c.Id == phonez.Id);

            ThemeHelper.ThemeChanged += OnGlobalThemeChanged;
        }

        private void OnGlobalThemeChanged(object? s, EventArgs e)
        {
            if (IsDisposed || !IsHandleCreated) return;
            BeginInvoke(new Action(ApplyTheme)); // tránh cross-thread
        }
        public void ApplyTheme()
        {
            SuspendLayout();

            foreach (Control child in Controls)
            {
                if (child is IThemeable themeable)
                    themeable.ApplyTheme();
            }


            ResumeLayout();
        }

        private void FormActionIntent_Load(object sender, EventArgs e)
        {
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.KeyUp += (s, e) =>
            {
                if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) && dataGridView1.CurrentRow != null)
                {
                    var intentInfo = dataGridView1.CurrentRow.DataBoundItem as IntentInfo;

                    var cmd = intentInfo?.BuildCommand();
                    if (cmd != null && cmd.Any())
                    {
                        GridViewBinders.BindAdbCommand(dataGridView2, cmd, new List<string> { "VersionName", "VersionCode", "Id" });
                        dataGridView2.AutoResizeColumns();
                    }
                }
            };

            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.CellClick += dataGridView2_CellClick;
            dataGridView2.CellEndEdit += (s, ev) =>
            {
                if (ev.RowIndex < 0 || ev.ColumnIndex < 0) return;
                var comand = dataGridView2.Rows[ev.RowIndex].DataBoundItem as AdbCommandIntent;
                comandSelected = comand;
            };


        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {

            if (keyData == Keys.F1 && dataGridView2.CurrentRow != null)
            {
                SaveCommand();
                return true;
            }
            if (keyData == Keys.F2 && dataGridView2.CurrentRow != null)
            {
                Invoke(async () =>
                {
                    var selectedPhone = comboxPhoneList.SelectedItem as Phone;
                    if (comandSelected != null && selectedPhone != null)
                    {
                        await CMD.ExecuteAdbAsync($"adb -s {selectedPhone.Serial} shell {comandSelected.Command}");
                    }

                });

                return true;
            }
            if (keyData == Keys.F3 && dataGridView2.CurrentRow != null)
            {
                GetAllSaveCommandPackage();
                return true;
            }

            if (keyData == Keys.F4 && dataGridView2.CurrentRow != null)
            {
                DeleteCommand();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void LoadDataGridView()
        {
            GridViewBinders.BindIntentInfo(dataGridView1, intentInfos, new List<string> { "Model", "PackageName", "VersionName", "VersionCode", "API", "Id" });
        }
        private void dataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var intentInfo = dataGridView1.Rows[e.RowIndex].DataBoundItem as IntentInfo;
            var cmd = intentInfo?.BuildCommand();
            if (cmd != null && cmd.Count() > 0)
            {
                GridViewBinders.BindAdbCommand(dataGridView2, cmd, new List<string> { "VersionName", "VersionCode", "Id" });
                dataGridView2.AutoResizeColumns();
            }
        }
        private void dataGridView2_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var comand = dataGridView2.Rows[e.RowIndex].DataBoundItem as AdbCommandIntent;
            comandSelected = comand;
        }


        private async void comboxPhoneList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedPhone = comboxPhoneList.SelectedItem as Phone;
            if (selectedPhone == null) return;
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            txtDumpy.Clear();
            intentInfos.Clear();
            comboxPackageList.Items.Clear();
            packageList.Clear();


            var values = await SettingAppList.getAllPackage(selectedPhone);
            if (values == null || values.Count() == 0)
            {
                MessageBox.Show("No packages found for the selected phone.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 1-a. Nếu bạn dùng LINQ
            var sorted = values
                .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                .ToList();

            packageList = sorted;

            foreach (var item in sorted)
            {
                comboxPackageList.Items.Add(item);
            }

            GetAllSaveCommandPackageByModel();

        }
        private async void comboxPackageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedPackage = comboxPackageList.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedPackage)) return;
            var phone = comboxPhoneList.SelectedItem as Phone;
            if (phone == null)
            {
                MessageBox.Show("Please select a phone.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await GetSinglePackage(phone, selectedPackage);

            GetAllSaveCommandPackage();
        }
        public async Task GetSinglePackage(Phone phone, string packageName)
        {
            var devicedatas = AdbClient.Instance.GetDevices();
            if (devicedatas.Count() == 0)
            {
                MessageBox.Show("No devices connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedDevice = devicedatas.FirstOrDefault(d => d.Serial == phone.Serial);
            if (selectedDevice == null)
            {
                MessageBox.Show("Selected phone is not connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (phone == null) return;
            var valueget = await SettingAppList.getIntentSinglePackage(phone, packageName);
            if (valueget.DumpyString != null && valueget.IntentList != null)
            {
                this.Invoke((Delegate)(() =>
                {
                    txtDumpy.Text = valueget.DumpyString;
                    intentInfos = valueget.IntentList;
                    LoadDataGridView();

                }));
            }
            else
            {
                MessageBox.Show("No intents found for the selected package.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }



        private async void buttonTestAction_Click(object sender, EventArgs e)
        {
            var selectedPhone = comboxPhoneList.SelectedItem as Phone;
            if (comandSelected != null && selectedPhone != null)
            {
                await CMD.ExecuteAdbAsync($"adb -s {selectedPhone.Serial} shell {comandSelected.Command}");
            }
        }


        private void buttonShowScreen_Click(object sender, EventArgs e)
        {
            var phone = comboxPhoneList.SelectedItem as Phone;
            if (phone == null) return;



        }
        private async void buttonLoadAllUserApp_Click(object sender, EventArgs e)
        {
            comboxPackageList.Items.Clear();
            var selectedPhone = comboxPhoneList.SelectedItem as Phone;
            if (selectedPhone == null) return;
            var values = await SettingAppList.getAllUserPackage(selectedPhone);
            if (values == null || values.Count() == 0)
            {
                MessageBox.Show("No packages found for the selected phone.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 1-a. Nếu bạn dùng LINQ
            var sorted = values
                .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var item in sorted)
            {
                comboxPackageList.Items.Add(item);
            }
        }
        private async void buttonLoadAllApp_Click(object sender, EventArgs e)
        {
            comboxPackageList.Items.Clear();
            var selectedPhone = comboxPhoneList.SelectedItem as Phone;
            if (selectedPhone == null) return;

            var values = await SettingAppList.getAllPackage(selectedPhone);
            if (values == null || values.Count() == 0)
            {
                MessageBox.Show("No packages found for the selected phone.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 1-a. Nếu bạn dùng LINQ
            var sorted = values
                .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var item in sorted)
            {
                comboxPackageList.Items.Add(item);
            }
        }

        public void SaveCommand()
        {
            if (comandSelected == null) return;
            if (string.IsNullOrEmpty(comandSelected.Name) || string.IsNullOrEmpty(comandSelected.PackageName) || string.IsNullOrEmpty(comandSelected.VersionName))
            {

                MessageBox.Show(Text = "Command Name is Empty", "Error", MessageBoxButtons.OK);

                return;
            }

            var check = AdbCommandIntentRepository.IsAny(comandSelected.Name, comandSelected.PackageName, comandSelected.VersionName, comandSelected.VersionCode, comandSelected.Id);
            if (check)
            {
                MessageBox.Show(
                    $"Model: {comandSelected.Model}\n" +
                    $"Package: {comandSelected.PackageName}\n" +
                    $"App Version: {comandSelected.VersionName}\n",
                    $"Existing Command Name: {comandSelected.Name}",
                    MessageBoxButtons.OK
                );
            }
            else
            {
                var command = AdbCommandIntentRepository.FindOneById(comandSelected.Id);
                if (command != null)
                {
                    AdbCommandIntentRepository.Update(comandSelected);
                }
                else
                {
                    AdbCommandIntentRepository.Add(comandSelected);
                }

                MessageBox.Show("Command saved successfully!");
            }
        }

        private void buttonSaveCommand_Click(object sender, EventArgs e)
        {
            SaveCommand();
        }
        private void buttonLoadCommand_Click(object sender, EventArgs e)
        {
            GetAllSaveCommandPackage();
        }


        public void GetAllSaveCommandPackageByModel(Phone? phone = null)
        {
            comboxListPackageCommands.Items.Clear();

            // Chọn phone đầu vào hoặc lấy từ ComboBox khác
            var selectedPhone = phone ?? comboxPhoneList.SelectedItem as Phone;
            if (selectedPhone == null)
                return;

            // Lấy tất cả command từ SQLite
            var commands = AdbCommandIntentRepository.FindManyByModel(selectedPhone.Model);


            if (commands == null || commands.Count == 0)
                return;

            // Tạo danh sách package unique, sort, cache vào biến toàn cục
            _allCommandPackages = commands
                .Select(c => c.PackageName)
                .Distinct()
                .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Đổ vào ComboBox lần đầu
            comboxListPackageCommands.Items.AddRange(
                _allCommandPackages.ToArray());
        }
        private void comboxListPackageCommands_KeyUp(object sender, KeyEventArgs e)
        {
            string filter = comboxListPackageCommands.Text;

            // Lọc case-insensitive
            var filtered = string.IsNullOrEmpty(filter)
                ? _allCommandPackages
                : _allCommandPackages
                    .Where(p => p.IndexOf(
                        filter,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

            comboxListPackageCommands.BeginUpdate();
            comboxListPackageCommands.Items.Clear();
            comboxListPackageCommands.Items.AddRange(
                filtered.ToArray());

            // Mở dropdown và giữ con trỏ ở cuối chuỗi
            comboxListPackageCommands.DroppedDown = true;
            comboxListPackageCommands.SelectionStart = filter.Length;
            comboxListPackageCommands.SelectionLength = 0;
            comboxListPackageCommands.EndUpdate();
        }

        public void GetAllSaveCommandPackage()
        {
            var package = comboxPackageList.SelectedItem as string;
            if (string.IsNullOrEmpty(package))
            {
                return;
            }

            var phone = comboxPhoneList.SelectedItem as Phone;
            if (phone == null)
            {
                return;
            }

            var commands = AdbCommandIntentRepository.FindManyByPackageName(package);
            if (commands == null || commands.Count == 0)
            {
                return;
            }

            GridViewBinders.BindAdbCommand(dataGridView2, commands, new List<string> { "VersionName", "VersionCode", "Id" });
            dataGridView2.AutoResizeColumns();
        }
        public void GetAllSaveCommandPackageV2()
        {
            var package = comboxListPackageCommands.SelectedItem as string;
            if (string.IsNullOrEmpty(package))
            {
                return;
            }

            var phone = comboxPhoneList.SelectedItem as Phone;
            if (phone == null)
            {
                return;
            }

            var commands = AdbCommandIntentRepository.FindManyByPackageName(package);
            if (commands != null && commands.Count > 0)
            {
                GridViewBinders.BindAdbCommand(dataGridView2, commands, new List<string> { "VersionName", "VersionCode", "Id" });
            }

        }
        private void comboxListPackageCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetAllSaveCommandPackageV2();
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var text = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                SearchAndBind(text);
            }
        }
        private void SearchAndBind(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                GridViewBinders.BindIntentInfo(dataGridView1, intentInfos, new List<string> { "Model", "PackageName", "VersionName", "VersionCode", "API", "Id" });

                return;
            }

            string kw = keyword.ToLowerInvariant();
            var result = intentInfos.Where(x =>
                x.MimeActionType.ToLowerInvariant().Contains(kw) ||
                x.Component.ToLowerInvariant().Contains(kw) ||
                x.Action.ToLowerInvariant().Contains(kw) ||
                x.Types.ToLowerInvariant().Contains(kw) ||
                x.Scheme.ToLowerInvariant().Contains(kw) ||
                x.Path.ToLowerInvariant().Contains(kw) ||
                x.Authority.ToLowerInvariant().Contains(kw) ||
                x.Categories.ToLowerInvariant().Contains(kw) ||
                x.AutoVerify.ToString().ToLowerInvariant().Contains(kw)
            ).ToList();
            if (result != null && result.Count() > 0)
            {
                GridViewBinders.BindIntentInfo(dataGridView1, result, new List<string> { "Model", "PackageName", "VersionName", "VersionCode", "API", "Id" });
                dataGridView1.AutoResizeColumns();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeleteCommand();
        }

        public void DeleteCommand()
        {
            if (comandSelected == null || comandSelected.Id == 0) return;

            var check = AdbCommandIntentRepository.IsAny(comandSelected.Id);
            if (check)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the command:\n" +
                    $"Model: {comandSelected.Model}\n" +
                    $"Package: {comandSelected.PackageName}\n" +
                    $"App Version: {comandSelected.VersionName}\n" +
                    $"Command Name: {comandSelected.Name}",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes)
                {
                    return; // User cancelled the deletion
                }

                AdbCommandIntentRepository.Delete(comandSelected.Id);
                var bindingList = dataGridView2.DataSource as BindingList<AdbCommandIntent>;
                if (bindingList != null)
                {
                    bindingList.Remove(comandSelected);
                }

                MessageBox.Show("Command deleted successfully!");
            }
        }

        private void comboxPackageList_KeyUp(object sender, KeyEventArgs e)
        {
            string filter = comboxPackageList.Text;
            var filtered = packageList
                .Where(x => x.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            comboxPackageList.BeginUpdate();
            comboxPackageList.Items.Clear();
            foreach (var item in filtered)
                comboxPackageList.Items.Add(item);

            // Mở dropdown để người dùng thấy kết quả
            comboxPackageList.DroppedDown = true;

            // Giữ con trỏ ở cuối text
            comboxPackageList.SelectionStart = filter.Length;
            comboxPackageList.SelectionLength = 0;
            comboxPackageList.EndUpdate();
        }

    }
}
