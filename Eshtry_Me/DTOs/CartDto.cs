using EshtryWepApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Eshtry_Me.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public decimal TotalPrice
        {
            get
            {
                return CartItems.Sum(ci => ci.UnitPrice * ci.Quantity);
            }
        }
    }
}