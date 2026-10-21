namespace Eshtry_Me.HelperClasses
{
    public class AuthModel
    {
        public string EmailOrPhone { get; set; } 
        public string Password { get; set; }
        public string Username { get; set; }
        public bool IsAuthenticated { get; set; } = false;
        public string Token { get; set; }
        public string Message { get; set; }
        public DateTime ExpiresON { get; set; }
        public List<string> Roles { get; set; }
    }
}
