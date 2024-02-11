namespace Lafatkotob.ViewModels
{
    public class LoginResultModel
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }

        public DateTime Expiration { get; set; } 
        public string RefreshToken { get; set; } 
    }
}

