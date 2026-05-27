namespace MZikmund.Toolkit.WinUI.Models;

/// <summary>
/// Generic DTO for a single page of a paginated API response.
/// </summary>
/// <typeparam name="T">Element type.</typeparam>
/// <param name="Items">Items in this page.</param>
/// <param name="Page">1-based page number this response represents.</param>
/// <param name="PageSize">Maximum items per page.</param>
/// <param name="TotalCount">Total number of items across all pages.</param>
public sealed record PaginatedResponse<T>(
    T[] Items,
    int Page,
    int PageSize,
    int TotalCount)
{
    /// <summary>
    /// Total number of pages. Zero when <see cref="TotalCount"/> is zero or <see cref="PageSize"/> is non-positive.
    /// </summary>
    public int TotalPages => PageSize > 0
        ? (int)Math.Ceiling(TotalCount / (double)PageSize)
        : 0;

    /// <summary>
    /// <see langword="true"/> when a page exists before this one.
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// <see langword="true"/> when a page exists after this one.
    /// </summary>
    public bool HasNextPage => Page < TotalPages;

    /// <summary>
    /// Convenience: an empty response (page 1 of 0 items).
    /// </summary>
    /// <param name="pageSize">Page size to record on the empty response.</param>
    public static PaginatedResponse<T> Empty(int pageSize = 0) =>
        new(Array.Empty<T>(), Page: 1, PageSize: pageSize, TotalCount: 0);
}
