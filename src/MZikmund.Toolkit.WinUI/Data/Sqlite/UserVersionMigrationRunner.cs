using SQLite;

namespace MZikmund.Toolkit.WinUI.Data;

/// <summary>
/// <see cref="IMigrationRunner"/> that tracks the applied schema version through
/// SQLite's built-in <c>PRAGMA user_version</c>. Migrations are an ordered list —
/// each entry's index in the list is its target version. The runner advances
/// <c>user_version</c> step-by-step and skips migrations whose index is &lt;= the
/// stored version.
/// </summary>
/// <example>
/// <code>
/// var runner = new UserVersionMigrationRunner(new[]
/// {
///     async c => await c.ExecuteAsync("CREATE TABLE Reminder (Id INTEGER PRIMARY KEY, Title TEXT)"),
///     async c => await c.ExecuteAsync("ALTER TABLE Reminder ADD COLUMN DueAt INTEGER"),
/// });
/// var version = await runner.RunAsync(connection);
/// </code>
/// </example>
public sealed class UserVersionMigrationRunner : IMigrationRunner
{
    private readonly IReadOnlyList<Func<SQLiteAsyncConnection, Task>> _migrations;

    /// <summary>Initializes a new instance with the supplied ordered migration list.</summary>
    /// <param name="migrations">
    /// Ordered list of migration delegates. The number of entries determines the target
    /// schema version — fresh databases are upgraded all the way to <c>migrations.Count</c>.
    /// </param>
    public UserVersionMigrationRunner(IReadOnlyList<Func<SQLiteAsyncConnection, Task>> migrations)
    {
        ArgumentNullException.ThrowIfNull(migrations);
        _migrations = migrations;
    }

    /// <summary>The target schema version (count of migrations).</summary>
    public int TargetVersion => _migrations.Count;

    /// <inheritdoc />
    public async Task<int> RunAsync(SQLiteAsyncConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        var current = await connection.ExecuteScalarAsync<int>("PRAGMA user_version");
        for (var i = current; i < _migrations.Count; i++)
        {
            await _migrations[i](connection);
            await connection.ExecuteAsync($"PRAGMA user_version = {i + 1}");
        }

        return _migrations.Count;
    }
}
