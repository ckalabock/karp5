using System.ComponentModel.DataAnnotations;

namespace Karp5Shop.Shared;

public sealed class ProductDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название товара")]
    [StringLength(120, ErrorMessage = "Название не должно быть длиннее 120 символов")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Описание не должно быть длиннее 500 символов")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 1_000_000, ErrorMessage = "Цена должна быть больше 0")]
    public decimal Price { get; set; }

    [Range(0, 100_000, ErrorMessage = "Количество не может быть отрицательным")]
    public int Stock { get; set; }
}
