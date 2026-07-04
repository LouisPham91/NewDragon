using Dragon.Controller.Firebase.Model;
using Dragon.Controller.GlobalControl.Security;
using Dragon.Controller.Server.Services;
using System.Text;
using System.Text.Json;

namespace Dragon.Controller.Firebase.Services
{
    public class FirebaseService
    {

        private const string firebaseApiKey = "/u0084/u00D9/u0003/u0038/u00D3/u009E/u00D5/u00C7/u00F3/u0039/u00FF/u00B1/u0027/u00E9/u0096/u0071/u00AE/u0065/u001D/u00BE/u008A/u00C8/u0065/u00F2/u00F3/u00D4/u0046/u00C3/u00FE/u0031/u0060/u001F/u0017/u0003/u0016/u0028/u00D3/u0092/u00AF";
        private string GetDecodedFirebaseApiKey()
        {
            return DecodeStringV2.DecodeV2(firebaseApiKey);
        }
        public async Task<(string idToken, string refreshToken)?> RefreshFirebaseTokenAsync(string refreshToken)
        {
            try
            {
                var getKey = GetDecodedFirebaseApiKey();
                //AIzaSyDQL2L2MPpIs0JPFO4hMIdXpIg6TbJiQwQ
                string url = $"https://securetoken.googleapis.com/v1/token?key={getKey}";

                var payload = new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", refreshToken }
                };

                using var client = new HttpClient();
                var response = await client.PostAsync(url, new FormUrlEncodedContent(payload));
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                var tokenResponse = await JsonSerializer.DeserializeAsync(stream, JsonServer.Default.FirebaseRefreshResponse);

                if (tokenResponse == null || tokenResponse.id_token == null || tokenResponse.refresh_token == null)
                    throw new Exception("Không lấy được token mới");

                return (tokenResponse.id_token, tokenResponse.refresh_token);
            }
            catch
            {
                return null;
            }
        }

        public async Task<FirebaseUser?> GetFirebaseUserInfoAsync(string firebaseIdToken)
        {
            string url = $"https://identitytoolkit.googleapis.com/v1/accounts:lookup?key={GetDecodedFirebaseApiKey()}";

            var payload = new FirebaseRefreshPayload { idToken = firebaseIdToken };

            using var client = new HttpClient();
            var json = JsonSerializer.Serialize(payload, JsonServer.Default.FirebaseRefreshPayload);
            var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var lookupResponse = await JsonSerializer.DeserializeAsync(stream, JsonServer.Default.FirebaseUserLookupResponse);

            if (lookupResponse == null || lookupResponse.users == null || lookupResponse.users.Count == 0)
                return null;

            return lookupResponse.users.First();
        }





        public async Task<GoogleAccount?> ExchangeGoogleTokenWithFirebaseAsync(string googleIdToken)
        {
            string url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={GetDecodedFirebaseApiKey()}";

            var payload = new FirebaseGoogleSignInPayload
            {
                postBody = $"id_token={googleIdToken}&providerId=google.com",
                requestUri = "http://localhost",
                returnSecureToken = true
            };
            using var client = new HttpClient();
            var json = JsonSerializer.Serialize(payload, JsonServer.Default.FirebaseGoogleSignInPayload);
            var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var account = await JsonSerializer.DeserializeAsync(stream, JsonServer.Default.GoogleAccount);

            if (account == null) return null;

            return account;
        }


    }
}



