namespace Bluedit.Persistence.Helpers.Pagination;

public class PaginationMetaData<T>
{
    public int TotalCount;
    public int PageSize;
    public int CurrentPage;
    public int TotalPages;
    public string? PreviousPageLink;
    public string? NextPageLink;

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
