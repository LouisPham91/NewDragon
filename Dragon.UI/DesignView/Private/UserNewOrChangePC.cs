
using Dragon.ControlHelper;
using Dragon.Controller.Firebase.Services;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Controller.GlobalControl.Security;
using Dragon.Controller.Server.Base;
using Dragon.Controller.Server.Model;
using Dragon.Controller.Server.Services;
using Dragon.DesignView.FormUI;
using Dragon.DesignView.Public;
using System;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;

namespace Dragon.DesignView.ControlUI.Private
{
    public partial class UserNewOrChangePC : UserControl
    {
        UserVersion? userVersion;

        

        public UserNewOrChangePC(UserVersion? userVersion)
        {
            this.userVersion = userVersion;
            InitializeComponent();
            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;
            
            ChangeComputer();
        }

        public void ChangeComputer()
        {
            if (IsDisposed) return;
            iconComputer.BadgeBack = ThemeHelper.ForceColofulFist;
            iconExchange.BadgeBack = ThemeHelper.ForceColofulFist;
            iconPlus.BadgeBack = ThemeHelper.ForceColofulFist;
            iconUser.BadgeBack = ThemeHelper.ForceColofulFist;
            panel1.Size = new Size(this.Width / 2, this.Height);
            UI_Helper.SapXepCenter(panelTransfer);
            UI_Helper.SapXepCenter(panelCreate);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (!IsDisposed) ChangeComputer();
        }

        private async void buttonCreateNew_Click(object sender, EventArgs e)
        {
            await StaticValue.LoadComputerInstance();

            var Accesstoken = GetSettings.GetAccessToken();
            if (string.IsNullOrEmpty(Accesstoken))
            {
                MessageBox.Show("AccessToken is null or empty");
                return;
            }

            var fingerPrint = GetSettings.GetFingerPrint();
            var chkfinger = StaticValue.CreateCanvasFingerprint();
            var webFingerPrint = GetSettings.GetWebFingerPrint();

            if (string.IsNullOrEmpty(webFingerPrint) || string.IsNullOrEmpty(fingerPrint) || chkfinger != fingerPrint)
            {

                var login = new GoogleLoginWebWindow();

                login.LoginCompleted += (s, _) =>
                {
                    // chạy trên UI thread
                    this.BeginInvoke(async () =>
                    {
                        if (!string.IsNullOrEmpty(login.IdToken))
                        {
                            var service = new FirebaseService();
                            var googleAccount = await service.ExchangeGoogleTokenWithFirebaseAsync(login.IdToken);
                            if (googleAccount != null)
                            {
                                GetSettings.SetFireBaseToken(googleAccount.idToken);
                                GetSettings.SetFireBaseRefreshToken(googleAccount.refreshToken);
                                GetSettings.SetFingerPrint(StaticValue.CreateCanvasFingerprint());
                                await StaticValue.LoadComputerInstance();
                            }
                            else
                            {
                                MessageBox.Show("Invalid data! Please login your google account");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Bạn đã đóng cửa sổ");
                        }
                    });
                };

                login.Show(); // không phải ShowDialog
            }

            // KHÔNG còn GiaiMa - computerHash giờ là JSON plain
            var responseCreate = await HttpServices.PostJsonAuthAsync(
                new EncryptedPayload { Data = await RsaKeyPair.MaHoa(StaticValue.computerHash) },
                JsonServer.Default.EncryptedPayload,
                Accesstoken,
                "/api/Computer/create");

            if (responseCreate.StatusCode == HttpStatusCode.OK)
            {
                var comJson = await responseCreate.Content.ReadAsStringAsync();
                // server trả plain JSON, deserialize trực tiếp
                var computer = JsonSerializer.Deserialize<Computer>(comJson, JsonServer.Default.Computer);
                StaticValue.computerHash = comJson; // lưu plain

                if (computer != null && userVersion != null)
                {
                    userVersion.AddThisPC(computer, ThemeHelper.ForceColofulFist, false);
                    userVersion.UserSuccsessLogin();
                    HeartbeatClient.Instance.Start(computer.Id, GetSettings.GetAccessToken(), TimeSpan.FromMinutes(1));
                    await userVersion.LoadComputerOnline(computer);
                }
            }
            else
            {
                var reason = await responseCreate.Content.ReadAsStringAsync();
                MessageBox.Show("BadRequest: " + reason);
            }
        }

        private async void buttonTransfer_Click(object sender, EventArgs e)
        {
            FormTransferComputer transferForm = new FormTransferComputer();
            if (transferForm.ShowDialog() == DialogResult.OK)
            {
                var computer = StaticValue.getComputerInstance();
                if (computer != null && userVersion != null)
                {
                    userVersion.AddThisPC(computer, ThemeHelper.ForceColofulFist);
                    userVersion.UserSuccsessLogin();
                    HeartbeatClient.Instance.Start(computer.Id, GetSettings.GetAccessToken(), TimeSpan.FromMinutes(1));
                    await userVersion.LoadComputerOnline(computer);
                }
            }
        }
    }
}