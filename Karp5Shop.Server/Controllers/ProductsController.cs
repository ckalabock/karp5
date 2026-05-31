using Karp5Shop.Server.Data;
using Karp5Shop.Server.Models;
using Karp5Shop.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karp5Shop.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts()
    {
        var products = await context.Products
            .OrderBy(product => product.Id)
            .Select(product => ToDto(product))
            .ToListAsync();

        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var product = new Product();
        ApplyDto(product, productDto);

        context.Products.Add(product);
        await context.SaveChangesAsync();

        var createdProduct = ToDto(product);
        return CreatedAtAction(nameof(GetProducts), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, ProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var product = await context.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        ApplyDto(product, productDto);
        await context.SaveChangesAsync();

        return Ok(ToDto(product));
    }

    [HttpPut]
    public Task<ActionResult<ProductDto>> UpdateProduct(ProductDto productDto) =>
        UpdateProduct(productDto.Id, productDto);

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private static ProductDto ToDto(Product product) =>
        new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };

    private static void ApplyDto(Product product, ProductDto productDto)
    {
        product.Name = productDto.Name.Trim();
        product.Description = productDto.Description.Trim();
        product.Price = productDto.Price;
        product.Stock = productDto.Stock;
    }
}
