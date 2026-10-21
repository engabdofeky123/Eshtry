namespace Eshtry_Me.DTOs
{

    public class LoginDto
    {
        public string EmailOrPhone { get; set; }
        public string Password { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }


}