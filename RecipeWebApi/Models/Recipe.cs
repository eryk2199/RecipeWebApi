using System.ComponentModel.DataAnnotations;

namespace RecipeWebApi.Models;

public class Recipe
{
    public int Id { get; set; }
    [StringLength(100, MinimumLength = 3)]
    public required string Title { get; set; }
    [StringLength(1000)]
    public required string Instructions { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}