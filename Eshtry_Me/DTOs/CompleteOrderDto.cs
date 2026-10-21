using System.ComponentModel.DataAnnotations;

namespace Eshtry_Me.DTOs
{
    public class CompleteOrderDto
    {
        public int AddressId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;

    }
}
