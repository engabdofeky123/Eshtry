using EshtryWepApi.Models;

namespace Eshtry_Me.DTOs
{
    public class CheckoutViewModel
    {
        public int UserId { get; set; } // Assuming this is available from the logged-in user
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public Cart Cart { get; internal set; }
        // You can add more fields here (e.g., payment method, contact information)
    }
}
