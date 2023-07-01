namespace Bluedit.ResourceParameters;

public class TopicResourceParameters
{
    public string? TopicName { get; set; }
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;

    const int maxPageSize = 10;

    private int _pageSize = 5;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
    }
}
