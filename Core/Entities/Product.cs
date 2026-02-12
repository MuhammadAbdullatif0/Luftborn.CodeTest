namespace Core.Entities;

public class Product : BaseEntity
{
    public  string Name { get; set; }
    public  string Description { get; set; }
    public decimal Price { get; set; }
    public  string PictureUrl { get; set; }
    public int ProductCategoryId { get; set; }
    public Category ProductCategory { get; set; }
}
