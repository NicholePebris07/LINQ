using LinqRetrievalLab.Data;
using LinqRetrievalLab.DTOs;
using LinqRetrievalLab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinqRetrievalLab.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(ProductDto dto)
    {
        var category = await _db.Categories.FindAsync(dto.CategoryId);

        if (category is null)
            return BadRequest("Category does not exist.");

        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return Ok(product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> Update(
        int id,
        ProductDto dto)
    {
        var product = await _db.Products.FindAsync(id);

        if (product is null)
            return NotFound();

        var category = await _db.Categories.FindAsync(dto.CategoryId);

        if (category is null)
            return BadRequest("Category does not exist.");

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.CategoryId = dto.CategoryId;

        await _db.SaveChangesAsync();

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product is null)
            return NotFound();

        _db.Products.Remove(product);

        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductWithCategory(int id)
    {
        var product = await _db.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(
        int categoryId)
    {
        var category = await _db.Categories.FindAsync(categoryId);

        if (category is null)
            return NotFound("Category does not exist.");

        var products = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Product>>> SearchByName(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Search string is required.");

        var products = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.Name.Contains(name))
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("price-range")]
    public async Task<ActionResult<IEnumerable<Product>>> GetByPriceRange(
        decimal lowerLimit,
        decimal upperLimit)
    {
        if (lowerLimit > upperLimit)
            return BadRequest("Lower limit cannot be greater than upper limit.");

        var products = await _db.Products
            .Include(p => p.Category)
            .Where(p => p.Price >= lowerLimit &&
                       p.Price <= upperLimit)
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("total-stock")]
    public async Task<ActionResult<int>> GetTotalStock()
    {
        var totalStock = await _db.Products
            .SumAsync(p => p.Stock);

        return Ok(totalStock);
    }

    [HttpGet("average-price")]
    public async Task<ActionResult<decimal>> GetAveragePrice()
    {
        var count = await _db.Products.CountAsync();

        if (count == 0)
            return Ok(0);

        var averagePrice = await _db.Products
            .AverageAsync(p => p.Price);

        return Ok(averagePrice);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetProductCount()
    {
        var count = await _db.Products.CountAsync();

        return Ok(count);
    }
}
