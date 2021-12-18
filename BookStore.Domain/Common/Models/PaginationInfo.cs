namespace BookStore.Domain.Common.Models;

public class PaginationInfo<T>
{
    public IEnumerable<T> PaginatedList { get; }
    public int Total { get; }

    public PaginationInfo(int total, IEnumerable<T> paginatedList)
    {
        Total = total;
        PaginatedList = paginatedList;
    }
}