namespace Dragon.Controller.Firebase.Model
{
    public class FirebaseRefreshResponse
    {
        public string? access_token { get; set; }
        public string? expires_in { get; set; }
        public string? token_type { get; set; }
        public string? refresh_token { get; set; }
        public string? id_token { get; set; }
        public string? user_id { get; set; }
        public string? project_id { get; set; }
    }

}
