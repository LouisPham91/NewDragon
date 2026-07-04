namespace Dragon.Controller.Firebase.Model
{
    public class FirebaseGoogleSignInPayload
    {
        public string postBody { get; set; } = "";
        public string requestUri { get; set; } = "";
        public bool returnSecureToken { get; set; } = true;
    }

}
