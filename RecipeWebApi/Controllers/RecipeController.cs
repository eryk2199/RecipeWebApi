using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebApi.Data;
using RecipeWebApi.Dtos;
using RecipeWebApi.Models;

namespace RecipeWebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/recipes")]
public class RecipeController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public RecipeController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecipeDto>>> GetAll()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }
        
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .Where(r => r.UserId == userId)
            .Select(r => new RecipeDto()
            {
                Id = r.Id,
                Title = r.Title,
                Instructions = r.Instructions,
                Ingredients = r.Ingredients.Select(i => new IngredientDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Amount = i.Amount
                }).ToList(),
            })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeDto>> GetById(int id)
    {
        var recipe = await _context.Recipes
            .Where(r => r.Id == id)
            .Include(r => r.Ingredients)
            .Select(r => new Recipe()
            {
                Id = r.Id,
                Title = r.Title,
                Instructions = r.Instructions,
                Ingredients = r.Ingredients.Select(i => new Ingredient(){ Id = i.Id, Name = i.Name , Amount = i.Amount }).ToList(),
                UserId = r.UserId,
            })
            .FirstAsync();
        
        if (recipe == null)
        {
            return NotFound();
        }
        
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        if (userId != recipe.UserId)
        {
            return BadRequest();
        }

        var recipeDto = new RecipeDto()
        {
            Id = recipe.Id,
            Title = recipe.Title,
            Instructions = recipe.Instructions,
            Ingredients = recipe.Ingredients
                .Select(i => new IngredientDto() { Id = i.Id, Name = i.Name, Amount = i.Amount })
                .ToList(),
        };
        return recipeDto;
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateRecipeDto recipeDto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var recipe = new Recipe()
        {
            Title = recipeDto.Title,
            Instructions = recipeDto.Instructions,
            Ingredients = recipeDto.Ingredients.Select(i => new Ingredient() { Id = i.Id, Name = i.Name, Amount = i.Amount}).ToList(),
            UserId = userId,
        };
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
        
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        if (userId != recipeToUpdate.UserId)
        {
            return BadRequest();
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

        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }
        if (userId != recipe.UserId)
        {
            return BadRequest();
        }
        
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
        return Ok();
    }
}