namespace Bluedit.Application.Contracts;

public interface IPagedList
{
    int CurrentPage { get; }
    bool HasNext { get; }
    bool HasPrevious { get; }
    int PageSize { get; }
    int TotalCount { get; }
    int TotalPages { get; }
}