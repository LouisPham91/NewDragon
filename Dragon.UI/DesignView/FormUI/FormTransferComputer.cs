
using Dragon.ControlHelper;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Controller.GlobalControl.Security;
using Dragon.Controller.Server.Base;
using Dragon.Controller.Server.Model;
using Dragon.Controller.Server.Services;
using Dragon.DesignView.ControlUI.Private;
using Dragon.DesignView.Public;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dragon.DesignView.FormUI
{
    public partial class FormTransferComputer : Form
    {
        public FormTransferComputer()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private async void FormTransferComputer_Load(object sender, EventArgs e)
        {
            var computer = StaticValue.getComputerInstance();
            if (computer == null) return;

            var UserPc = new UserThisPC(computer, ThemeHelper.ForceColofulFist);
            UserPc.Size = panelGetTransfer.Size;
            UserPc.SapXepControl();
            UserPc.Dock = DockStyle.Fill;
            panelGetTransfer.Controls.Add(UserPc);

            var response = await HttpServices.PostJsonAuthAsync(
                new EncryptedPayload { Data = await RsaKeyPair.MaHoa(JsonSerializer.Serialize(computer, JsonServer.Default.Computer)) },
                JsonServer.Default.EncryptedPayload,
                GetSettings.GetAccessToken(),
                "/api/Computer/online/"
            );

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json)) return;

                // server trả plain, không GiaiMa
                var compHashList = JsonSerializer.Deserialize(json, JsonServer.Default.ListComputerOnline);
                if (compHashList == null || !compHashList.Any()) return;

                foreach (var comp in compHashList) AddComputer(comp);

                var first = flowCompList.Controls.OfType<UserPCOnline>().LastOrDefault();
                if (first != null) UserPc_Selected(first, EventArgs.Empty);
            }
            UI_Helper.SapXepCenter(label2);
            ApplyTheme();
        }
        public void ApplyTheme()
        {
            
        }
        public void AddComputer(ComputerOnline computer)
        {
            var item = new UserPCOnline(computer, ThemeHelper.ForceColofulFist);
            item.Selected += UserPc_Selected;
            flowCompList.Controls.Add(item);
        }

        private void UserPc_Selected(object? sender, EventArgs e)
        {
            if (sender is UserPCOnline selectedPc)
            {
                foreach (var ctrl in flowCompList.Controls.OfType<UserPCOnline>())
                    if (ctrl != selectedPc) ctrl.SetSelected(false);

                selectedPc.SetSelected(true);
                panelTranfer.Controls.Clear();
                var clone = new UserPCOnline(selectedPc.computerOnline!, ThemeHelper.ForceColofulFist);
                panelTranfer.Controls.Add(clone);

                var userpc = panelGetTransfer.Controls.OfType<UserThisPC>().FirstOrDefault();
                if (userpc == null || selectedPc.computerOnline == null) return;

                var newCom = new Computer
                {
                    Status = selectedPc.computerOnline.Status,
                    Expires_At = selectedPc.computerOnline.Expires_At,
                };
                userpc.Changecomputer(newCom, ThemeHelper.ForceColofulFist);
            }
        }

        private async void buttomTransferNow_Click(object sender, EventArgs e)
        {
            var userpconline = panelTranfer.Controls.OfType<UserPCOnline>().FirstOrDefault();
            if (userpconline == null)
            {
                MessageBox.Show("Please select a computer to transfer data to.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (userpconline.computerOnline == null)
            {
                MessageBox.Show("No computer information found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var computerOnline = userpconline.computerOnline;
            var computer = StaticValue.getComputerInstance();
            if (computer == null) return;

            var reponse = await HttpServices.PostJsonAuth2ValueAsync(new EncryptedPayload2value
            {
                Data = await RsaKeyPair.MaHoa(JsonSerializer.Serialize(computer, JsonServer.Default.Computer)),
                Data2 = await RsaKeyPair.MaHoa(JsonSerializer.Serialize(computerOnline, JsonServer.Default.ComputerOnline))
            }, JsonServer.Default.EncryptedPayload2value, GetSettings.GetAccessToken());

            if (reponse.StatusCode == HttpStatusCode.OK)
            {
                // server trả plain computer json
                StaticValue.computerHash = await reponse.Content.ReadAsStringAsync();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                var reason = await reponse.Content.ReadAsStringAsync();
                MessageBox.Show($"{reponse.StatusCode}: {reason}");
            }
        }
    }
}