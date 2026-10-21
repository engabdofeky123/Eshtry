using System.ComponentModel.DataAnnotations;

namespace Eshtry_Me.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string RecipientName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Governorate { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required, MaxLength(300)]
        public string StreetAddress { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? AdditionalDetails { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
