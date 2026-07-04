using DirectN;
using Dragon.ControlHelper;
using Dragon.ControlHelper.WebSite;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Controller.Server.Services;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using WebView2;
using WebView2.Utilities;

namespace Dragon.DesignView.FormUI
{
    public partial class GoogleLoginWebWindow : WebViewWindow
    {
        public string? IdToken { get; private set; }
        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }

        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string redirectUri = "http://localhost";
        private readonly string codeVerifier;

        public event EventHandler? LoginCompleted;

        private ICoreWebView2? _core;
        private bool _isHandled = false;

        public GoogleLoginWebWindow() : base("Google Login")
        {
            // Đọc từ file nhúng, không hardcode
            clientId = GetSetting("ClientId", "GOOGLE_CLIENT_ID");
            clientSecret = GetSetting("ClientSecret", "GOOGLE_CLIENT_SECRET");

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                throw new InvalidOperationException("Thiếu ClientId/Secret. Tạo google_oauth.json (Embedded Resource).");

            codeVerifier = GenerateCodeVerifier();
            _ = InitAsync();
        }

        private string GetSetting(string jsonKey, string envName)
        {
            var env = Environment.GetEnvironmentVariable(envName);
            if (!string.IsNullOrWhiteSpace(env)) return env;

            try
            {
                var asm = typeof(GoogleLoginWebWindow).Assembly;
                using var stream = asm.GetManifestResourceStream("Dragon.UI.google_oauth.json");
                if (stream != null)
                {
                    using var doc = JsonDocument.Parse(stream);
                    if (doc.RootElement.TryGetProperty(jsonKey, out var val))
                        return val.GetString() ?? string.Empty;
                }
            }
            catch { }
            return string.Empty;
        }

        private async Task InitAsync()
        {
            while (_core == null)
            {
                var field = typeof(WebViewWindow).GetField("_controller",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var ctrl = field?.GetValue(this) as DirectN.Extensions.Com.ComObject<ICoreWebView2Controller>;
                if (ctrl != null)
                {
                    ctrl.Object.get_CoreWebView2(out var wv).ThrowOnError();
                    _core = wv;
                    var token = default(EventRegistrationToken);
                    _core.add_NavigationCompleted(new CoreWebView2NavigationCompletedEventHandler(OnNavigationCompleted), ref token);
                }
                await Task.Delay(50);
            }

            await CreateCanvasFingerprintAsync();

            string codeChallenge = GenerateCodeChallenge(codeVerifier);
            string authUrl = $"https://accounts.google.com/o/oauth2/v2/auth" +
                $"?client_id={Uri.EscapeDataString(clientId)}" +
                $"&response_type=code" +
                $"&scope={Uri.EscapeDataString("openid email profile")}" +
                $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                $"&access_type=offline" +      // xin refresh_token
                $"&prompt=consent" +            // ép trả refresh_token lần đầu
                $"&code_challenge={codeChallenge}" +
                $"&code_challenge_method=S256";

            Navigate(authUrl);
            NativeWindowHelper.ResizeAndCenter(this.Handle, 500, 680);
        }

        private void Navigate(string url) => _core?.Navigate(PWSTR.From(url));

        private Task<string> ExecuteScriptAsync(string js)
        {
            var tcs = new TaskCompletionSource<string>();
            _core?.ExecuteScript(PWSTR.From(js),
                new CoreWebView2ExecuteScriptCompletedHandler((hr, result) =>
                {
                    tcs.TrySetResult(result.ToString() ?? "null");
                }));
            return tcs.Task;
        }

        private async void OnNavigationCompleted(ICoreWebView2 sender, ICoreWebView2NavigationCompletedEventArgs args)
        {
            if (_isHandled) return;
            sender.get_Source(out var src);
            string url = src.ToString() ?? "";

            if (url.StartsWith(redirectUri) && url.Contains("code="))
            {
                var query = System.Web.HttpUtility.ParseQueryString(new Uri(url).Query);
                string? code = query["code"];
                if (code == null) return;

                _isHandled = true;
                await ExchangeCodeForTokensAsync(code);
                Finish();
            }
        }

        private void Finish()
        {
            LoginCompleted?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) LoginCompleted?.Invoke(this, EventArgs.Empty);
            base.Dispose(disposing);
        }

        private async Task ExchangeCodeForTokensAsync(string authCode)
        {
            var payload = new Dictionary<string, string>
            {
                { "code", authCode },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" },
                { "code_verifier", codeVerifier }
            };

            using var client = new HttpClient();
            var response = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(payload));
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize(json, JsonServer.Default.GoogleTokenResponse);

            IdToken = token?.id_token;
            AccessToken = token?.access_token;
            RefreshToken = token?.refresh_token;

            if (!string.IsNullOrEmpty(RefreshToken))
                SaveRefreshToken(RefreshToken);
        }

        public async Task<string?> RefreshAccessTokenAsync()
        {
            var refresh = LoadRefreshToken();
            if (string.IsNullOrEmpty(refresh)) return null;

            var payload = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", refresh }
            };

            using var client = new HttpClient();
            var resp = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(payload));
            if (!resp.IsSuccessStatusCode) return null;

            var json = await resp.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize(json, JsonServer.Default.GoogleTokenResponse);
            AccessToken = token?.access_token;
            IdToken = token?.id_token;
            return AccessToken;
        }

        private void SaveRefreshToken(string refreshToken)
        {
            try
            {
                var bytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(refreshToken), null, DataProtectionScope.CurrentUser);
                var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Dragon");
                Directory.CreateDirectory(dir);
                File.WriteAllBytes(Path.Combine(dir, "google_refresh.bin"), bytes);
            }
            catch { }
        }

        private string? LoadRefreshToken()
        {
            try
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Dragon", "google_refresh.bin");
                if (!File.Exists(path)) return null;
                var bytes = File.ReadAllBytes(path);
                var plain = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(plain);
            }
            catch { return null; }
        }

        private static string GenerateCodeVerifier()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Base64UrlEncode(bytes);
        }

        private static string GenerateCodeChallenge(string verifier)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(verifier));
            return Base64UrlEncode(hash);
        }

        private static string Base64UrlEncode(byte[] data)
        {
            return Convert.ToBase64String(data).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        private async Task<string?> WaitForElementAsync(string id)
        {
            for (int i = 0; i < 20; i++)
            {
                string result = await ExecuteScriptAsync($"document.getElementById('{id}')?.innerText");
                if (result == "null") { await Task.Delay(200); continue; }
                string? value = JsonSerializer.Deserialize(result, JsonServer.Default.String);
                if (!string.IsNullOrEmpty(value)) return value;
                await Task.Delay(200);
            }
            return null;
        }

        private async Task CreateCanvasFingerprintAsync()
        {
            Navigate("https://browserleaks.com/canvas");
            var value = await WaitForElementAsync("canvas-hash");
            if (!string.IsNullOrEmpty(value))
                GetSettings.SetWebFingerPrint(value);
        }
    }
}
