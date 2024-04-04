namespace Bluedit.Persistence.Helpers.Pagination;

public class PaginationMetaData<T>
{
    public int totalCount;
    public int pageSize;
    public int currentPage;
    public int totalPages;
    public string? previousPageLink;
    public string? nextPageLink;

    public PaginationMetaData(PagedList<T> pagedList, string? previousPageLink, string? nextPageLink)
    {
        totalCount = pagedList.TotalCount;
        pageSize = pagedList.PageSize;
        currentPage = pagedList.CurrentPage;
        totalPages = pagedList.TotalPages;
        this.previousPageLink = previousPageLink;
        this.nextPageLink = nextPageLink;
    }

}
