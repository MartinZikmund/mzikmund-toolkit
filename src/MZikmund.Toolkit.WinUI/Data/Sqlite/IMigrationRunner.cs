using SQLite;

namespace MZikmund.Toolkit.WinUI.Data;

/// <summary>
/// Runs idempotent SQLite schema migrations against an open
/// <see cref="SQLiteAsyncConnection"/>. Implementations decide how to track which
/// migrations have already been applied — typical strategies are
/// <c>PRAGMA user_version</c> (see <see cref="UserVersionMigrationRunner"/>) or a
/// dedicated <c>__migrations</c> table.
/// </summary>
public interface IMigrationRunner
{
    /// <summary>
    /// Applies all pending migrations and returns the resulting schema version.
    /// </summary>
    Task<int> RunAsync(SQLiteAsyncConnection connection);
}
