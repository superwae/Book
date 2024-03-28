namespace Lafatkotob.ViewModels
{
    public class LoginResultModel
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string ProfilePicture { get; set; }
        public string ErrorMessage { get; set; }

        public DateTime Expiration { get; set; } 
        public string RefreshToken { get; set; } 
        public string Role { get; set; }
    }
}

