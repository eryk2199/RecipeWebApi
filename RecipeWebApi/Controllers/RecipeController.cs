using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebApi.Data;
using RecipeWebApi.Models;

namespace RecipeWebApi.Controllers;

[ApiController]
[Route("api/recipes")]
public class RecipeController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public RecipeController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IEnumerable<Recipe>> GetAll()
    {
        return await _context.Recipes.Include(r => r.Ingredients).ToListAsync();
    }

    [HttpGet("/{id}")]
    public async Task<ActionResult<Recipe>> GetById(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null)
        {
            return NotFound();
        }
        return recipe;
    }

    [HttpPost]
    public async Task<ActionResult> Create(Recipe recipe)
    {
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = recipe.Id }, recipe);
    }

    [HttpPut("/{id}")]
    public async Task<ActionResult> Update(int id, Recipe recipe)
    {
        var recipeToUpdate = await _context.Recipes.FindAsync(id);
        if (recipeToUpdate == null)
        {
            return NotFound();
        }
        recipeToUpdate.Title = recipe.Title;
        recipeToUpdate.Instructions = recipe.Instructions;
        recipeToUpdate.Ingredients = recipe.Ingredients;

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null)
        {
            return NotFound();
        }
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
        return Ok();
    }
}