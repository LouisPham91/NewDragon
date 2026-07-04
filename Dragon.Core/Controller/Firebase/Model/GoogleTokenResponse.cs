

namespace Dragon.Controller.Firebase.Model
{
    public class GoogleTokenResponse
    {
        public string? access_token { get; set; }
        public int expires_in { get; set; }   // sửa từ string? sang int
        public string? refresh_token { get; set; }
        public string? scope { get; set; }
        public string? token_type { get; set; }
        public string? id_token { get; set; }
    }


}
