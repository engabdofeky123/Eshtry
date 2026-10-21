using System.ComponentModel.DataAnnotations;

namespace EshtryWepApi.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        // Navigation property
        public List<Product> Products { get; set; }
    }
}
