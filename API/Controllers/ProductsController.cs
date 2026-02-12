using API.Extensions;
using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? categoryIds = null,
        CancellationToken cancellationToken = default)
        => (await _productService.GetListAsync(pageIndex, pageSize, categoryIds, cancellationToken)).ToActionResult(this);

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id, CancellationToken cancellationToken = default)
        => (await _productService.GetByIdAsync(id, cancellationToken)).ToActionResult(this, nameof(GetProduct));

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto, CancellationToken cancellationToken = default)
        => (await _productService.CreateAsync(dto, cancellationToken)).ToActionResult(this, nameof(GetProduct));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto, CancellationToken cancellationToken = default)
        => (await _productService.UpdateAsync(id, dto, cancellationToken)).ToActionResult(this);

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken = default)
        => (await _productService.DeleteAsync(id, cancellationToken)).ToActionResult(this);
}
