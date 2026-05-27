using SQLite;

namespace MZikmund.Toolkit.WinUI.Data;

/// <summary>
/// Base class for app-specific SQLite data services. Lazy-initializes a
/// <see cref="SQLiteAsyncConnection"/>, runs the <see cref="IMigrationRunner"/>,
/// and exposes the connection through <see cref="EnsureInitializedAsync"/>.
/// </summary>
/// <remarks>
/// Initialization is single-threaded via <see cref="SemaphoreSlim"/> — the first
/// caller drives initialization, concurrent callers wait, and subsequent callers
/// take a fast path that skips the lock.
/// </remarks>
public abstract class SqliteDataServiceBase : IAsyncDisposable
{
    private readonly string _databasePath;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private SQLiteAsyncConnection? _connection;

    /// <summary>Initializes a new instance.</summary>
    /// <param name="databasePath">Filesystem path of the SQLite database file (or <c>":memory:"</c>).</param>
    protected SqliteDataServiceBase(string databasePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(databasePath);
        _databasePath = databasePath;
    }

    /// <summary>The path passed to the constructor.</summary>
    public string DatabasePath => _databasePath;

    /// <summary>
    /// Returns the initialized connection, creating it (and running migrations) on first call.
    /// </summary>
    public async Task<SQLiteAsyncConnection> EnsureInitializedAsync()
    {
        if (_connection is not null)
        {
            return _connection;
        }

        await _initLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (_connection is null)
            {
                var connection = new SQLiteAsyncConnection(_databasePath);
                await OnConfiguringAsync(connection).ConfigureAwait(false);
                var runner = CreateMigrationRunner();
                if (runner is not null)
                {
                    await runner.RunAsync(connection).ConfigureAwait(false);
                }
                _connection = connection;
            }

            return _connection;
        }
        finally
        {
            _initLock.Release();
        }
    }

    /// <summary>
    /// Called immediately after the connection is created and before migrations run.
    /// Override to apply pragmas, register custom collations, etc. Default is a no-op.
    /// </summary>
    protected virtual Task OnConfiguringAsync(SQLiteAsyncConnection connection) =>
        Task.CompletedTask;

    /// <summary>
    /// Returns the migration runner that will populate / upgrade the schema, or
    /// <see langword="null"/> if no migrations are needed (typically only for tests).
    /// </summary>
    protected abstract IMigrationRunner? CreateMigrationRunner();

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_connection is { } connection)
        {
            await connection.CloseAsync().ConfigureAwait(false);
        }
        _initLock.Dispose();
        GC.SuppressFinalize(this);
    }
}
