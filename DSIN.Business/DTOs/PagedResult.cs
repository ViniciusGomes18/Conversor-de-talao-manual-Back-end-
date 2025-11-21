namespace DSIN.Business.DTOs;

public sealed class PagedResult<T>
{
    public int Total { get; set; }
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
}