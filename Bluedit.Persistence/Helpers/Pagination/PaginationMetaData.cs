namespace Bluedit.Persistence.Helpers.Pagination;

public class PaginationMetaData<T>
{
    public int CurrentPage;
    public string? NextPageLink;
    public int PageSize;
    public string? PreviousPageLink;
    public int TotalCount;
    public int TotalPages;

    public PaginationMetaData(PagedList<T> pagedList, string? previousPageLink, string? nextPageLink)
    {
        TotalCount = pagedList.TotalCount;
        PageSize = pagedList.PageSize;
        CurrentPage = pagedList.CurrentPage;
        TotalPages = pagedList.TotalPages;
        PreviousPageLink = previousPageLink;
        NextPageLink = nextPageLink;
    }
}