using Eshtry_Me.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EshtryWepApi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0.01", "9999999999999999.99")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [MaxLength(2048)]
        public string? ImageUrl { get; set; }

        public int? CategoryId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Column(TypeName = "decimal(5,2)")]
        [Range(typeof(decimal), "0", "100")]
        public decimal DiscountPercent { get; set; }

        public DateTime? OfferCreatedDate { get; set; }
        public DateTime? OfferEndDate { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(UserId))]
        public ApplicationUser Seller { get; set; } = null!;

        public Category? Category { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
            = new List<CartItem>();

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public decimal DiscountAmount =>
            Math.Round(Price * DiscountPercent / 100m, 2);

        [NotMapped]
        public decimal FinalPrice => Price - DiscountAmount;

        [NotMapped]
        public bool HasActiveOffer => DiscountPercent > 0 && OfferEndDate.HasValue && OfferEndDate.Value > DateTime.UtcNow;

    }
}
