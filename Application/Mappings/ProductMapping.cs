using Application.DTOs;
using Core.Entities;

namespace Application.Mappings;

public static class ProductMapping
{
    public static ProductDto ToDto(this Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        PictureUrl = p.PictureUrl,
        ProductCategoryId = p.ProductCategoryId,
        CategoryName = p.ProductCategory?.Name
    };

    public static Product ToEntity(this CreateProductDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Price = dto.Price,
        PictureUrl = dto.PictureUrl,
        ProductCategoryId = dto.ProductCategoryId
    };

    public static void Apply(this UpdateProductDto dto, Product product)
    {
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.PictureUrl = dto.PictureUrl;
        product.ProductCategoryId = dto.ProductCategoryId;
    }
}
