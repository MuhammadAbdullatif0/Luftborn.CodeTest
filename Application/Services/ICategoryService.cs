using Application.Common;
using Application.DTOs;

namespace Application.Services;

public interface ICategoryService
{
    Task<ServiceResult<IReadOnlyList<CategoryDto>>> GetListAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<CategoryDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceResult<CategoryDto>> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult<Unit>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
