namespace Minimal_WebApi.Dtos;

public class PageResult<T>
{
    public IEnumerable<T> Items {get;set;}

    public int TotalCount {get;set;}
    public int CurrentPage {get;set;}
    public int PageSize {get;set;}
    public int TotalPages => (int) Math.Ceiling(TotalCount / (double) PageSize);
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => TotalPages > CurrentPage;

}