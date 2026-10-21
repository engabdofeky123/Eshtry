
using System.ComponentModel.DataAnnotations;

public class AddToCartModel
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, 100)]
    public int Quantity { get; set; } = 1;

    [MaxLength(50)]
    public string? Color { get; set; }
}