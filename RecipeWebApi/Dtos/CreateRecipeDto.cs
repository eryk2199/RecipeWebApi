using System.ComponentModel.DataAnnotations;

namespace RecipeWebApi.Dtos;

public class CreateRecipeDto
{
    [StringLength(100, MinimumLength = 3)]
    public required string Title { get; set; }
    [StringLength(1000)]
    public required string Instructions { get; set; }
    public List<IngredientDto> Ingredients { get; set; } = new List<IngredientDto>();
}