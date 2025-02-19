using System.ComponentModel.DataAnnotations;

namespace RecipeWebApi.Models;

public class Ingredient
{
    public int Id { get; set; }
    [StringLength(50, MinimumLength = 3)]
    public required string Name { get; set; }
    [StringLength(20)]
    public string? Amount { get; set; }
    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
}