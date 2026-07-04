

namespace Dragon.Controller.Firebase.Model
{
    public class GoogleAccount
    {
        public string localId { get; set; } = string.Empty;
        public string idToken { get; set; } = string.Empty;
        public string refreshToken { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public string photoUrl { get; set; } = string.Empty;
    }

}
