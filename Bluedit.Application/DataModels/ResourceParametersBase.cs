namespace Bluedit.Application.DataModels;

public class ResourceParametersBase
{
    public string? TopicName { get; set; }
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;

    private const int MaxPageSize = 10;

    private int _pageSize = 5;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public string? Fields { get; set; }
    public virtual string OrderBy { get; set; } 
}