namespace Dragon.Controller.Firebase.Model
{
    public class FirebaseUserLookupResponse
    {
        public List<FirebaseUser> users { get; set; } = new();
    }

    public class FirebaseUser
    {
        public string localId { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public string photoUrl { get; set; } = string.Empty;
    }

}
