using Application.Common;
using Application.DTOs;

namespace Application.Services;

public interface IProductService
{
    Task<ServiceResult<IReadOnlyList<ProductDto>>> GetListAsync(int pageIndex, int pageSize, string? categoryIds, CancellationToken cancellationToken = default);
    Task<ServiceResult<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceResult<ProductDto>> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult<ProductDto>> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult<Unit>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
