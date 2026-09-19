using LinqRetrievalLab.Data;
using LinqRetrievalLab.DTOs;
using LinqRetrievalLab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinqRetrievalLab.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;

    public CategoriesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Create(CategoryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Category name is required.");

        var category = new Category
        {
            Name = dto.Name
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        return Ok(category);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Category>> Update(int id, CategoryDto dto)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category is null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Category name is required.");

        category.Name = dto.Name;

        await _db.SaveChangesAsync();

        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category is null)
            return NotFound();

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
