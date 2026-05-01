namespace MZikmund.Toolkit.WinUI.Data;

/// <summary>
/// Inclusive date range expressed as sortable <see cref="DateTime.Ticks"/> values.
/// Designed for SQLite tables that store timestamps as <c>INTEGER</c> ticks: those
/// columns are cheap to <c>BETWEEN ? AND ?</c> against and use the standard B-tree
/// index without expression-based lookups.
/// </summary>
/// <param name="FromTicks">Inclusive lower bound, in <see cref="DateTime.Ticks"/> form.</param>
/// <param name="ToTicks">Inclusive upper bound, in <see cref="DateTime.Ticks"/> form.</param>
/// <example>
/// <code>
/// var range = DateRange.Between(weekStartLocal, weekEndLocal);
/// var rows = await connection.Table&lt;Reminder&gt;()
///     .Where(r =&gt; r.DueAtTicks &gt;= range.FromTicks &amp;&amp; r.DueAtTicks &lt;= range.ToTicks)
///     .ToListAsync();
/// </code>
/// </example>
public readonly record struct DateRange(long FromTicks, long ToTicks)
{
    /// <summary>
    /// Builds a <see cref="DateRange"/> from two <see cref="DateTime"/> bounds. Order is
    /// auto-corrected — passing <c>(end, start)</c> produces the same range as
    /// <c>(start, end)</c>.
    /// </summary>
    public static DateRange Between(DateTime from, DateTime to)
    {
        var (lo, hi) = from <= to ? (from, to) : (to, from);
        return new DateRange(lo.Ticks, hi.Ticks);
    }

    /// <summary>The lower bound as a <see cref="DateTime"/>.</summary>
    public DateTime From => new(FromTicks);

    /// <summary>The upper bound as a <see cref="DateTime"/>.</summary>
    public DateTime To => new(ToTicks);

    /// <summary>The duration of the range.</summary>
    public TimeSpan Duration => To - From;

    /// <summary>Returns <see langword="true"/> when <paramref name="ticks"/> falls within the range (inclusive).</summary>
    public bool Contains(long ticks) => ticks >= FromTicks && ticks <= ToTicks;
}
