using Application.Common;
using Application.DTOs;
using Application.Mappings;
using Core.Entities;
using Core.Interfaces;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<IReadOnlyList<CategoryDto>>> GetListAsync(CancellationToken cancellationToken = default)
    {
        var list = await _unitOfWork.Repository<Category>().ListAllAsync();
        return ServiceResult<IReadOnlyList<CategoryDto>>.Ok(list.Select(c => c.ToDto()).ToList());
    }

    public async Task<ServiceResult<CategoryDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
        return category is null ? ServiceResult<CategoryDto>.NotFound() : ServiceResult<CategoryDto>.Ok(category.ToDto());
    }

    public async Task<ServiceResult<CategoryDto>> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = dto.ToEntity();
        _unitOfWork.Repository<Category>().Add(category);
        if (!await _unitOfWork.Complete())
            return ServiceResult<CategoryDto>.BadRequest("Failed to create category.");
        return ServiceResult<CategoryDto>.Created(category.ToDto(), category.Id);
    }

    public async Task<ServiceResult<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
        if (category is null)
            return ServiceResult<CategoryDto>.NotFound();
        category.Name = dto.Name;
        _unitOfWork.Repository<Category>().Update(category);
        if (!await _unitOfWork.Complete())
            return ServiceResult<CategoryDto>.BadRequest("Failed to update category.");
        return ServiceResult<CategoryDto>.Ok(category.ToDto());
    }

    public async Task<ServiceResult<Unit>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
        if (category is null)
            return ServiceResult<Unit>.NotFound();
        _unitOfWork.Repository<Category>().Remove(category);
        if (!await _unitOfWork.Complete())
            return ServiceResult<Unit>.BadRequest("Failed to delete category.");
        return ServiceResult<Unit>.Ok(default);
    }
}
