using System.ComponentModel.DataAnnotations;

namespace Eshtry_Me.DTOs
{
    public class ProductModelView
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string ImageUrl { get; set; }

        public int? CategoryId { get; set; }
        public int UserId { get; set; }
        // المرجع للمستخدم

    }
}
