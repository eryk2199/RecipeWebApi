using Microsoft.AspNetCore.Identity;

namespace RecipeWebApi.Models;

public class User: IdentityUser
{
    public List<Recipe> Recipes { get; set; } = new List<Recipe>();
}