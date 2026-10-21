using EshtryWepApi.Models;

namespace Eshtry_Me.DTOs
{
    public class SellerDashboardViewModel
    {
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public List<Product> Products { get; set; }

        // الفلاتر
        public string SearchQuery { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // معلومات التصفح
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
