namespace MZikmund.Toolkit.WinUI.Data;

/// <summary>
/// Helpers for staying under SQLite's parameter cap when running an
/// <c>IN (?, ?, ?, …)</c> or batched-insert query against a large input set.
/// SQLite limits <c>SQLITE_MAX_VARIABLE_NUMBER</c> to <strong>32766</strong> on
/// 3.32+ (and <strong>999</strong> on older versions).
/// </summary>
public static class SqliteBatch
{
    /// <summary>SQLite's default parameter cap on 3.32+ (32766).</summary>
    public const int DefaultParameterLimit = 32766;

    /// <summary>SQLite's parameter cap on versions older than 3.32 (999).</summary>
    public const int LegacyParameterLimit = 999;

    /// <summary>
    /// Splits <paramref name="items"/> into chunks no larger than
    /// <c>parameterLimit / parametersPerItem</c> (with a floor of 1). Pass
    /// <paramref name="parametersPerItem"/> &gt; 1 when each item contributes more
    /// than one bound parameter (e.g. an upsert with two columns per row).
    /// </summary>
    /// <param name="items">Source sequence to chunk.</param>
    /// <param name="parametersPerItem">Bound parameters each item contributes (default 1).</param>
    /// <param name="parameterLimit">Hard parameter cap (default <see cref="DefaultParameterLimit"/>).</param>
    /// <returns>An enumerable of chunks suitable for executing in separate queries / transactions.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">A non-positive value was supplied for either limit.</exception>
    public static IEnumerable<IReadOnlyList<T>> Chunk<T>(
        IEnumerable<T> items,
        int parametersPerItem = 1,
        int parameterLimit = DefaultParameterLimit)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentOutOfRangeException.ThrowIfLessThan(parametersPerItem, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(parameterLimit, 1);

        var chunkSize = Math.Max(1, parameterLimit / parametersPerItem);
        var current = new List<T>(chunkSize);
        foreach (var item in items)
        {
            current.Add(item);
            if (current.Count == chunkSize)
            {
                yield return current;
                current = new List<T>(chunkSize);
            }
        }

        if (current.Count > 0)
        {
            yield return current;
        }
    }
}
