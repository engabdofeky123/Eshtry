using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EshtryWepApi.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Navigation Properties
        public Cart Cart { get; set; } = null!;

        public Product Product { get; set; } = null!;

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
