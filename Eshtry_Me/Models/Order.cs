using Eshtry_Me.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EshtryWepApi.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0", "9999999999999999.99")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(500)]
        public string ShippingAddress { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? BillingAddress { get; set; }

        public OrderStatus OrderStatus { get; set; }
            = OrderStatus.Pending;

        public PaymentStatus PaymentStatus { get; set; }
            = PaymentStatus.Pending;

        // Navigation Properties
        public ApplicationUser User { get; set; } = null!;


        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}