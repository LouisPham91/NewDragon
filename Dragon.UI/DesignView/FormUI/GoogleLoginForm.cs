using DirectN;
using Dragon.ControlHelper;
using Dragon.ControlHelper.WebSite;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Controller.Server.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using WebView2;
using WebView2.Utilities;

namespace Dragon.DesignView.FormUI
{
    public partial class GoogleLoginWebWindow : WebViewWindow
    {
        public string? IdToken { get; private set; }

        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string redirectUri = "http://localhost";

        public event EventHandler? LoginCompleted;

        private ICoreWebView2? _core;
        private bool _isHandled = false;

        public GoogleLoginWebWindow() : base("Google Login")
        {
            // Load credentials từ biến môi trường hoặc file local (không commit)
            clientId = GetSetting("ClientId", "GOOGLE_CLIENT_ID");
            clientSecret = GetSetting("ClientSecret", "GOOGLE_CLIENT_SECRET");

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new InvalidOperationException(
                    "Thiếu Google OAuth credentials. Tạo file google_oauth.json cạnh exe hoặc set biến môi trường GOOGLE_CLIENT_ID / GOOGLE_CLIENT_SECRET.");
            }

            _ = InitAsync();
        }

        private string GetSetting(string jsonKey, string envName)
        {
            // 1. Ưu tiên biến môi trường
            var env = Environment.GetEnvironmentVariable(envName);
            if (!string.IsNullOrWhiteSpace(env)) return env;

            // 2. Đọc file google_oauth.json (không đưa lên git)
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "google_oauth.json");
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty(jsonKey, out var val))
                        return val.GetString() ?? string.Empty;
                }
            }
            catch { /* bỏ qua lỗi đọc file */ }

            return string.Empty;
        }

        private async Task InitAsync()
        {
            var sw = Stopwatch.StartNew();

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

            string authUrl = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={clientId}&response_type=code&scope=openid%20email%20profile&redirect_uri={redirectUri}";
            Navigate(authUrl);
            NativeWindowHelper.ResizeAndCenter(this.Handle, 500, 680);
            sw.Stop();
            Debug.WriteLine($"Stop {sw.ElapsedMilliseconds} ms");
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
                IdToken = await ExchangeCodeForIdTokenAsync(code);
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

        private async Task<string?> ExchangeCodeForIdTokenAsync(string authCode)
        {
            var payload = new Dictionary<string, string>
            {
                { "code", authCode },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" }
            };

            using var client = new HttpClient();
            var response = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(payload));
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize(json, JsonServer.Default.GoogleTokenResponse);
            return tokenResponse?.id_token;
        }

        private async Task<string?> WaitForElementAsync(string id)
        {
            for (int i = 0; i < 20; i++)
            {
                string result = await ExecuteScriptAsync($"document.getElementById('{id}')?.innerText");
                if (result == "null")
                {
                    await Task.Delay(200);
                    continue;
                }
                string? value = JsonSerializer.Deserialize(result, JsonServer.Default.String);
                if (!string.IsNullOrEmpty(value))
                    return value;
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
