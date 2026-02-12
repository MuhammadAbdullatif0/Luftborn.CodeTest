using API.Extensions;
using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken = default)
        => (await _categoryService.GetListAsync(cancellationToken)).ToActionResult(this);

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategory(int id, CancellationToken cancellationToken = default)
        => (await _categoryService.GetByIdAsync(id, cancellationToken)).ToActionResult(this, nameof(GetCategory));

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto, CancellationToken cancellationToken = default)
        => (await _categoryService.CreateAsync(dto, cancellationToken)).ToActionResult(this, nameof(GetCategory));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto, CancellationToken cancellationToken = default)
        => (await _categoryService.UpdateAsync(id, dto, cancellationToken)).ToActionResult(this);

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken = default)
        => (await _categoryService.DeleteAsync(id, cancellationToken)).ToActionResult(this);
}
