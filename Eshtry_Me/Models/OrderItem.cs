using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EshtryWepApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0", "9999999999999999.99")]
        public decimal UnitPrice { get; set; }

        // Product snapshot at purchase time
        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(2048)]
        public string? ProductImageUrl { get; set; }

        // Navigation Properties
        public Order Order { get; set; } = null!;

        public Product Product { get; set; } = null!;

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;


    }
}
