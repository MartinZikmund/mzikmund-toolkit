using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Data;
using SQLite;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class SqliteBatchTests
{
    [TestMethod]
    public void Chunk_NullItems_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => SqliteBatch.Chunk<int>(null!).ToList());
    }

    [TestMethod]
    public void Chunk_NonPositivePerItem_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => SqliteBatch.Chunk(new[] { 1 }, parametersPerItem: 0).ToList());
    }

    [TestMethod]
    public void Chunk_NonPositiveLimit_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => SqliteBatch.Chunk(new[] { 1 }, parameterLimit: 0).ToList());
    }

    [TestMethod]
    public void Chunk_DefaultLimit_RespectsCap()
    {
        var input = Enumerable.Range(0, 70_000).ToArray();

        var chunks = SqliteBatch.Chunk(input).ToList();

        // 70000 / 32766 = 2 full + remainder ⇒ 3 chunks total
        Assert.AreEqual(3, chunks.Count);
        Assert.AreEqual(SqliteBatch.DefaultParameterLimit, chunks[0].Count);
        Assert.AreEqual(SqliteBatch.DefaultParameterLimit, chunks[1].Count);
        Assert.AreEqual(70_000 - 2 * SqliteBatch.DefaultParameterLimit, chunks[2].Count);
    }

    [TestMethod]
    public void Chunk_WithMultipleParametersPerItem_HalvesChunkSize()
    {
        var input = Enumerable.Range(0, 5_000).ToArray();

        var chunks = SqliteBatch.Chunk(input, parametersPerItem: 2, parameterLimit: 100).ToList();

        // 100 / 2 = 50 per chunk ⇒ 5000 / 50 = 100 chunks
        Assert.AreEqual(100, chunks.Count);
        Assert.IsTrue(chunks.All(c => c.Count == 50));
    }

    [TestMethod]
    public void Chunk_EmptyInput_YieldsNothing()
    {
        Assert.AreEqual(0, SqliteBatch.Chunk(Array.Empty<int>()).Count());
    }

    [TestMethod]
    public void Chunk_PreservesOrderAndContent()
    {
        var input = Enumerable.Range(0, 250).ToArray();

        var chunks = SqliteBatch.Chunk(input, parameterLimit: 100).ToList();
        var flattened = chunks.SelectMany(c => c).ToArray();

        CollectionAssert.AreEqual(input, flattened);
    }
}

[TestClass]
public class DateRangeTests
{
    [TestMethod]
    public void Between_OrdersBoundaries()
    {
        var start = new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2024, 1, 12, 0, 0, 0, DateTimeKind.Utc);

        var direct = DateRange.Between(start, end);
        var reversed = DateRange.Between(end, start);

        Assert.AreEqual(direct, reversed);
    }

    [TestMethod]
    public void From_To_RoundTripTicks()
    {
        var range = DateRange.Between(
            new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc));

        Assert.AreEqual(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), range.From);
        Assert.AreEqual(new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc), range.To);
    }

    [TestMethod]
    public void Duration_ReturnsSpanBetweenBounds()
    {
        var range = DateRange.Between(
            new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 1, 4, 0, 0, 0, DateTimeKind.Utc));

        Assert.AreEqual(TimeSpan.FromDays(3), range.Duration);
    }

    [TestMethod]
    public void Contains_InclusiveBounds()
    {
        var range = DateRange.Between(
            new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2024, 1, 3, 0, 0, 0, DateTimeKind.Utc));

        Assert.IsTrue(range.Contains(range.FromTicks));
        Assert.IsTrue(range.Contains(range.ToTicks));
        Assert.IsTrue(range.Contains(new DateTime(2024, 1, 2).Ticks));
        Assert.IsFalse(range.Contains(new DateTime(2023, 12, 31).Ticks));
        Assert.IsFalse(range.Contains(new DateTime(2024, 1, 4).Ticks));
    }
}

[TestClass]
public class UserVersionMigrationRunnerTests
{
    [TestMethod]
    public void Ctor_NullMigrations_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new UserVersionMigrationRunner(null!));
    }

    [TestMethod]
    public async Task RunAsync_FreshDatabase_RunsAllMigrationsAndReportsTargetVersion()
    {
        var connection = new SQLiteAsyncConnection(":memory:");
        try
        {
            var runner = new UserVersionMigrationRunner(new Func<SQLiteAsyncConnection, Task>[]
            {
                static async c => await c.ExecuteAsync(
                    "CREATE TABLE Foo (Id INTEGER PRIMARY KEY)"),
                static async c => await c.ExecuteAsync(
                    "ALTER TABLE Foo ADD COLUMN Name TEXT"),
            });

            var version = await runner.RunAsync(connection);

            Assert.AreEqual(2, version);
            Assert.AreEqual(2, await connection.ExecuteScalarAsync<int>("PRAGMA user_version"));
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    [TestMethod]
    public async Task RunAsync_AlreadyAtTarget_DoesNotReapply()
    {
        var connection = new SQLiteAsyncConnection(":memory:");
        try
        {
            var ran = 0;
            var runner = new UserVersionMigrationRunner(new Func<SQLiteAsyncConnection, Task>[]
            {
                async c =>
                {
                    ran++;
                    await c.ExecuteAsync("CREATE TABLE Foo (Id INTEGER PRIMARY KEY)");
                },
            });

            await runner.RunAsync(connection);
            Assert.AreEqual(1, ran);

            await runner.RunAsync(connection);
            Assert.AreEqual(1, ran, "Migration should not re-run when user_version matches the target.");
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    [TestMethod]
    public async Task RunAsync_AdvancesUserVersionIncrementallyOnFailure()
    {
        var connection = new SQLiteAsyncConnection(":memory:");
        try
        {
            var runner = new UserVersionMigrationRunner(new Func<SQLiteAsyncConnection, Task>[]
            {
                static async c => await c.ExecuteAsync("CREATE TABLE Foo (Id INTEGER PRIMARY KEY)"),
                static c => throw new InvalidOperationException("boom"),
            });

            await Assert.ThrowsAsync<InvalidOperationException>(() => runner.RunAsync(connection));

            // First migration's PRAGMA write committed; second never ran.
            Assert.AreEqual(1, await connection.ExecuteScalarAsync<int>("PRAGMA user_version"));
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}

[TestClass]
public class SqliteDataServiceBaseTests
{
    [TestMethod]
    public void Ctor_NullOrEmptyPath_Throws()
    {
        Assert.Throws<ArgumentException>(() => new TestDb(string.Empty));
        Assert.Throws<ArgumentException>(() => new TestDb(null!));
    }

    [TestMethod]
    public async Task EnsureInitializedAsync_RunsMigrationsOnFirstCall()
    {
        await using var db = new TestDb(":memory:");

        var connection = await db.EnsureInitializedAsync();

        Assert.AreEqual(1, await connection.ExecuteScalarAsync<int>("PRAGMA user_version"));
    }

    [TestMethod]
    public async Task EnsureInitializedAsync_SubsequentCalls_ReturnSameInstance()
    {
        await using var db = new TestDb(":memory:");

        var first = await db.EnsureInitializedAsync();
        var second = await db.EnsureInitializedAsync();

        Assert.AreSame(first, second);
    }

    [TestMethod]
    public async Task DatabasePath_ReflectsConstructorArgument()
    {
        await using var db = new TestDb(":memory:");

        Assert.AreEqual(":memory:", db.DatabasePath);
    }

    private sealed class TestDb : SqliteDataServiceBase
    {
        public TestDb(string path) : base(path)
        {
        }

        protected override IMigrationRunner? CreateMigrationRunner() =>
            new UserVersionMigrationRunner(new Func<SQLiteAsyncConnection, Task>[]
            {
                static async c => await c.ExecuteAsync(
                    "CREATE TABLE Item (Id INTEGER PRIMARY KEY)"),
            });
    }
}
