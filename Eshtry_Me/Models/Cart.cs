using Eshtry_Me.DTOs;
using Eshtry_Me.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EshtryWepApi.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ApplicationUser User { get; set; } = null!;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
