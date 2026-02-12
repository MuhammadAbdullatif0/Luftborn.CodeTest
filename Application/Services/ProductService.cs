using Application.Common;
using Application.DTOs;
using Application.Mappings;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<IReadOnlyList<ProductDto>>> GetListAsync(int pageIndex, int pageSize, string? categoryIds, CancellationToken cancellationToken = default)
    {
        var param = new ProductParams
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            CategoryIds = string.IsNullOrEmpty(categoryIds) ? [] : [categoryIds]
        };
        var spec = new ProductsSpecification(param);
        var products = await _unitOfWork.Repository<Product>().ListAsync(spec);
        return ServiceResult<IReadOnlyList<ProductDto>>.Ok(products.Select(p => p.ToDto()).ToList());
    }

    public async Task<ServiceResult<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var spec = new ProductsSpecification(id);
        var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);
        return product is null ? ServiceResult<ProductDto>.NotFound() : ServiceResult<ProductDto>.Ok(product.ToDto());
    }

    public async Task<ServiceResult<ProductDto>> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        if (!_unitOfWork.Repository<Category>().Exists(dto.ProductCategoryId))
            return ServiceResult<ProductDto>.BadRequest("Category does not exist.");

        var product = dto.ToEntity();
        _unitOfWork.Repository<Product>().Add(product);
        if (!await _unitOfWork.Complete())
            return ServiceResult<ProductDto>.BadRequest("Failed to create product.");

        var spec = new ProductsSpecification(product.Id);
        var created = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);
        return ServiceResult<ProductDto>.Created(created!.ToDto(), product.Id);
    }

    public async Task<ServiceResult<ProductDto>> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product is null)
            return ServiceResult<ProductDto>.NotFound();
        if (!_unitOfWork.Repository<Category>().Exists(dto.ProductCategoryId))
            return ServiceResult<ProductDto>.BadRequest("Category does not exist.");

        dto.Apply(product);
        _unitOfWork.Repository<Product>().Update(product);
        if (!await _unitOfWork.Complete())
            return ServiceResult<ProductDto>.BadRequest("Failed to update product.");

        var spec = new ProductsSpecification(id);
        var updated = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);
        return ServiceResult<ProductDto>.Ok(updated!.ToDto());
    }

    public async Task<ServiceResult<Unit>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product is null)
            return ServiceResult<Unit>.NotFound();
        _unitOfWork.Repository<Product>().Remove(product);
        if (!await _unitOfWork.Complete())
            return ServiceResult<Unit>.BadRequest("Failed to delete product.");
        return ServiceResult<Unit>.Ok(default);
    }
}
