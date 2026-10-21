using EshtryWepApi.Models;

namespace Eshtry_Me.DTOs
{
    public class HomeDataDto
    {
        public List<ProductDto> OfferProducts { get; set; }
        public List<CategoryDto> Categories { get; set; }
    }

    public class ProductDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

        public int? CategoryId { get; set; }
        public bool? HasDiscount { get; set; }
        public decimal? Discount { get; set; }
        public DateTime? OfferCreatedDate { get; set; }
        public DateTime? OfferEndDate { get; set; }
    }
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       
    }
}
