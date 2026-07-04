
using Dragon.ControlHelper;
using Dragon.Controller.Firebase.Model;
using Dragon.Controller.Firebase.Services;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Controller.GlobalControl.Security;
using Dragon.Controller.Server.Base;
using Dragon.Controller.Server.Model;
using Dragon.Controller.Server.Services;
using Dragon.DesignView.FormUI;
using Dragon.DesignView.Public;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Dragon.DesignView.ControlUI.Private
{
    public partial class UserVersion : UserControl
    {
        FormMain formMain;
        UserPCRecognized userPCRecognized = new UserPCRecognized();
        UserNewOrChangePC? userNewOrChangePC;
        UserThisPC? userThisPC;

        public UserVersion(FormMain formMain)
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
            this.formMain = formMain;
            this.SizeChanged += UserVersion_SizeChanged;
        }

        private void UserVersion_SizeChanged(object? sender, EventArgs e)
        {
            Debug.WriteLine($"UserVersion SizeChanged: {this.Size.Width},{this.Size.Height}");
        }

        private async void UserVersion_Load(object sender, EventArgs e)
        {
            if (IsDisposed) return;
            UI_Helper.SapXepCenter(buttonLoginGmail);
            UI_Helper.SapXepCenter(labelStatus);
            UI_Helper.SapXepCenter(picboxLogo);
            UI_Helper.SapXepCenter(panel1);
            labelVersion.Text = "v" + Application.ProductVersion;
            picboxLogo.Image = DesignHelper.GetRandomLogo();
            ApplyTheme();
          //  await RegconizePC();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;
            
            labelDragon.ForeColor = ThemeHelper.ForceColofulFist;
            labelVersion.ForeColor = ThemeHelper.ForceColofulFist;

            flowOtherComputer.BackColor = ThemeHelper.BackNormalFirst;
            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                buttonLoginGmail.DefaultBack = Color.Gainsboro;
                buttonLoginGmail.ForeColor = Color.Black;
                labelStatus.ForeColor = Color.FromArgb(60, 139, 60);
            }
            else
            {
                buttonLoginGmail.DefaultBack = Color.FromArgb(70, 70, 70);
                buttonLoginGmail.ForeColor = Color.White;
                labelStatus.ForeColor = Color.FromArgb(128, 255, 128);
            }
           
        }

        public async Task RegconizePC()
        {
            if (IsDisposed) return;
            userPCRecognized.Dock = DockStyle.Fill;
            userPCRecognized.Margin = new Padding(0);
            panelThisPC.Controls.Add(userPCRecognized);
            userPCRecognized.Visible = true;
            UserPCRecognized();

            var service = new FirebaseService();
            var fireBaseToken = GetSettings.GetFireBaseToken();
            var fireBaseRefeshToken = GetSettings.GetFireBaseRefreshToken();

            var kq = await service.RefreshFirebaseTokenAsync(fireBaseRefeshToken);
            if (kq != null && kq.HasValue && !string.IsNullOrEmpty(kq.Value.idToken))
            {
                fireBaseToken = kq.Value.idToken;
                fireBaseRefeshToken = kq.Value.refreshToken;
                GetSettings.SetFireBaseToken(fireBaseToken);
                GetSettings.SetFireBaseRefreshToken(fireBaseRefeshToken);
            }
            else
            {
                GetSettings.SetFireBaseToken(string.Empty);
                GetSettings.SetFireBaseRefreshToken(string.Empty);
                return;
            }

            var firebaseUser = await service.GetFirebaseUserInfoAsync(fireBaseToken);
            if (firebaseUser == null) return;

            // BỎ gọi /api/User/Exit - vào thẳng login
            var value = await RsaKeyPair.MaHoa(GetSettings.GetWebFingerPrint() + "," + StaticValue.CreateCanvasFingerprint());

            var gettoken = await HttpServices.PostJsonAsync(
                new RefreshRequest { RefreshToken = GetSettings.GetRefreshAccessToken() },
                JsonServer.Default.RefreshRequest, "/api/User/refresh");

            if (gettoken.StatusCode == HttpStatusCode.OK)
            {
                var tokenString = await gettoken.Content.ReadAsStringAsync();
                var Token = JsonSerializer.Deserialize(tokenString, JsonServer.Default.Token);
                if (Token == null) return;

                GetSettings.SetAccessToken(Token.accessToken);
                GetSettings.SetRefreshAccessToken(Token.refreshToken);
                await StaticValue.LoadComputerInstance();
                await Login(firebaseUser.email, value);
            }
        }

        public void UserPCRecognized()
        {
            if (IsDisposed) return;
            formMain.UpdateExitUserLogin($"Login VIP to use all features", ThemeHelper.ForeNormalFirst);
            UpdateUI(labelStatus, "This PC Is Not Recognized! \nLogin To Google Account To Create New User", Color.FromArgb(251, 159, 0));
        }

        public void UserSuccsessLogin(string email = "")
        {
            if (IsDisposed) return;
            if (string.IsNullOrEmpty(email)) return;
            GetSettings.SetUserEmail(email);
            var user = StaticValue.getUsernstance();
            if (user == null) return;
            formMain.UpdateExitUserLogin($"Login Successed! Hello Mr. {user.Name}", ThemeHelper.ForeNormalFirst);
            var comp = StaticValue.getComputerInstance();
            if (comp == null) return;
            UpdateUI(labelStatus, $"Login Successed! Your Computer Expire On {comp.Expires_At:dd/MM/yyyy}", Color.FromArgb(79, 191, 107));
        }

        public void UpdateUI(Control control, string Text, Color? TextColor)
        {
            if (IsDisposed) return;
            control.Text = Text;
            if (TextColor.HasValue) control.ForeColor = TextColor.Value;
            if (control is Label lbl) lbl.TextAlign = ContentAlignment.MiddleCenter;
            UI_Helper.SapXepCenter(labelStatus);
        }

        public void AddThisPC(Computer computer, Color color, bool Isflow = false)
        {
            if (IsDisposed) return;
            if (userThisPC == null && !Isflow)
            {
                userThisPC = new UserThisPC(computer, color);
                userThisPC.Dock = DockStyle.Fill;
                userThisPC.Margin = new Padding(0);
                panelThisPC.Controls.Add(userThisPC);
            }
            else if (userThisPC == null && Isflow)
            {
                userThisPC = new UserThisPC(computer, color);
                userThisPC.Margin = new Padding(1);
                flowOtherComputer.Padding = new Padding(4);
                flowOtherComputer.Controls.Add(userThisPC);
            }
            else if (userThisPC != null) userThisPC.Changecomputer(computer, color);

            if (userThisPC != null) userThisPC.Visible = true;
            if (userPCRecognized != null) userPCRecognized.Visible = false;
            if (userNewOrChangePC != null) userNewOrChangePC.Visible = false;
        }

        public void AddComputer(ComputerOnline computer)
        {
            if (IsDisposed) return;
            var item = new UserPCOnline(computer, ThemeHelper.ForceColofulFist);
            flowOtherComputer.Controls.Add(item);
        }

        public async Task LoadComputerOnline(Computer computer)
        {
            if (IsDisposed) return;
            var response = await HttpServices.PostJsonAuthAsync(
                new EncryptedPayload { Data = await RsaKeyPair.MaHoa(JsonSerializer.Serialize(computer, JsonServer.Default.Computer)) },
                JsonServer.Default.EncryptedPayload,
                GetSettings.GetAccessToken(), "/api/Computer/online/");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json)) return;
                // server trả plain
                var compHashList = JsonSerializer.Deserialize(json, JsonServer.Default.ListComputerOnline);
                if (compHashList == null) return;
                foreach (var comp in compHashList) AddComputer(comp);
            }
        }

        private async Task Login(string email, string value, GoogleAccount? googleAccount = null)
        {
            if (IsDisposed) return;
            var responseLogin = await HttpServices.PostJsonAsync(
                new EncryptedPayload2value { Data = await RsaKeyPair.MaHoa(email), Data2 = value },
                JsonServer.Default.EncryptedPayload2value, "/api/User/login");

            if (responseLogin == null) { MessageBox.Show("Server maintenance"); return; }

            if (responseLogin.StatusCode == HttpStatusCode.OK)
            {
                var tokenString = await responseLogin.Content.ReadAsStringAsync();
                var Token = JsonSerializer.Deserialize(tokenString, JsonServer.Default.Token);
                if (Token == null) { MessageBox.Show("Error Get AccessToken"); return; }

                GetSettings.SetAccessToken(Token.accessToken);
                GetSettings.SetRefreshAccessToken(Token.refreshToken);
                await StaticValue.LoadComputerInstance();

                var responseGetUser = await HttpServices.PostAuthAsync(Token.accessToken);
                if (responseGetUser.StatusCode == HttpStatusCode.OK)
                {
                    var userJson = await responseGetUser.Content.ReadAsStringAsync();
                    StaticValue.userHash = userJson; // plain
                    await StaticValue.LoadComputerInstance();
                }

                var repFindComputerByHash = await HttpServices.PostJsonAuthAsync(
                    new EncryptedPayload { Data = value },
                    JsonServer.Default.EncryptedPayload, Token.accessToken, "/api/Computer/byhash");

                if (repFindComputerByHash.StatusCode == HttpStatusCode.OK)
                {
                    var comJson = await repFindComputerByHash.Content.ReadAsStringAsync();
                    var computer = JsonSerializer.Deserialize(comJson, JsonServer.Default.Computer);
                    StaticValue.computerHash = comJson;
                    if (computer != null)
                    {
                        AddThisPC(computer, ThemeHelper.ForceColofulFist, false);
                        UserSuccsessLogin(email);
                        HeartbeatClient.Instance.Start(computer.Id, Token.accessToken, TimeSpan.FromMinutes(1));
                        await LoadComputerOnline(computer);
                    }
                }
                else if (repFindComputerByHash.StatusCode == HttpStatusCode.NotFound)
                {
                    var responseAny = await HttpServices.GetAuthAsync(Token.accessToken, "/api/Computer/Any");
                    if (responseAny.StatusCode == HttpStatusCode.NotFound)
                    {
                        var responseCreate = await HttpServices.PostJsonAuthAsync(
                            new EncryptedPayload { Data = await RsaKeyPair.MaHoa(StaticValue.computerHash) },
                            JsonServer.Default.EncryptedPayload, Token.accessToken, "/api/Computer/create");

                        if (responseCreate.StatusCode == HttpStatusCode.OK)
                        {
                            var comJson = await responseCreate.Content.ReadAsStringAsync();
                            var computer = JsonSerializer.Deserialize(comJson, JsonServer.Default.Computer);
                            StaticValue.computerHash = comJson;
                            if (computer != null)
                            {
                                AddThisPC(computer, ThemeHelper.ForceColofulFist, false);
                                UserSuccsessLogin(email);
                                HeartbeatClient.Instance.Start(computer.Id, Token.accessToken, TimeSpan.FromMinutes(1));
                                await LoadComputerOnline(computer);
                            }
                        }
                    }
                    else if (responseAny.StatusCode == HttpStatusCode.OK)
                    {
                        userNewOrChangePC = new UserNewOrChangePC(this);
                        userNewOrChangePC.Dock = DockStyle.Fill;
                        panelThisPC.Controls.Add(userNewOrChangePC);
                        if (userThisPC != null) userThisPC.Visible = false;
                        if (userPCRecognized != null) userPCRecognized.Visible = false;
                        userNewOrChangePC.Visible = true;
                    }
                }
            }
            else
            {
                if (googleAccount == null) return;
                //  nếu có rồi thì báo lỗi, nếu chưa có thì hỏi có muốn đăng ký không

                var chk = MessageBox.Show("Register Request", "Bạn có muốn Đăng Ký 1 Account Mới không?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (chk == DialogResult.Yes)
                {

                    FormRegisterUser formRegisterUser = new FormRegisterUser(email, googleAccount);
                    formRegisterUser.ShowDialog();
                    if (string.IsNullOrEmpty(StaticValue.userHash)) return; // nếu userHash vẫn null tức là đăng ký thất bại hoặc bị hủy, không cần login
                    await Login(googleAccount.email, value, googleAccount);
                }
            }
        }

        private async void buttonLoginGmail_Click(object sender, EventArgs e)
        {
            if (IsDisposed) return;

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
                            if (!RegexHelper.IsValidEmail(googleAccount.email)) return;

                            var value = await RsaKeyPair.MaHoa(GetSettings.GetWebFingerPrint() + "," + GetSettings.GetFingerPrint());
                            // BỎ check Exit, thử login luôn
                            await Login(googleAccount.email, value, googleAccount);

                            //FormRegisterUser formRegisterUser = new FormRegisterUser(email, value);
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


    }

}