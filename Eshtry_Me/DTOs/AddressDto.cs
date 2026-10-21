using System.ComponentModel.DataAnnotations;

namespace Eshtry_Me.DTOs
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Governorate { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string? AdditionalDetails { get; set; }
        public bool IsDefault { get; set; }
    }
}
