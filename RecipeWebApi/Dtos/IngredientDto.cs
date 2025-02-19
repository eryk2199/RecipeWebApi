using System.ComponentModel.DataAnnotations;

namespace RecipeWebApi.Dtos;

public class IngredientDto
{
    public int Id { get; set; }
    [StringLength(50, MinimumLength = 3)]
    public required string Name { get; set; }
    [StringLength(20)]
    public string? Amount { get; set; }
}