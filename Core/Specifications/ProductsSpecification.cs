using Core.Entities;

namespace Core.Specifications;

public class ProductsSpecification : BaseSpecification<Product>
{
    public ProductsSpecification(int id) : base(x => x.Id == id)
    {

    }
    public ProductsSpecification(ProductParams productParams)
        : base(x =>
         (productParams.CategoryIds.Count == 0 || productParams.CategoryIds.Contains(x.ProductCategoryId.ToString()))
        )
    {
        AddInclude(x => x.ProductCategory);
        AddOrderBy(x => x.Name);
        ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);
    }
}