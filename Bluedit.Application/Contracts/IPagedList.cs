namespace Bluedit.Services.Repositories.TopicRepo;

public interface IPagedList<T>
{
    int CurrentPage { get; }
    bool HasNext { get; }
    bool HasPrevious { get; }
    int PageSize { get; }
    int TotalCount { get; }
    int TotalPages { get; }
}