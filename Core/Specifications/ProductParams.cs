namespace Core.Specifications;

public class ProductParams
{
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;

    private int _pageSize = 6;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    private List<string> _categoryIds = [];
    public List<string> CategoryIds
    {
        get => _categoryIds;
        set => _categoryIds = value.SelectMany(x => x.Split(',',
                StringSplitOptions.RemoveEmptyEntries)).ToList();
    }
}