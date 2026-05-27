using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Data;
using SQLite;

namespace MZikmund.Toolkit.Sample;

public sealed partial class SqliteSamplePage : Page
{
    private DemoDb? _db;

    public SqliteSamplePage()
    {
        this.InitializeComponent();
    }

    private async void Init_Click(object sender, RoutedEventArgs e)
    {
        _db ??= new DemoDb(":memory:");
        var connection = await _db.EnsureInitializedAsync();
        var version = await connection.ExecuteScalarAsync<int>("PRAGMA user_version");
        SchemaVersionText.Text = $"Initialized. PRAGMA user_version = {version}";
    }

    private async void Insert_Click(object sender, RoutedEventArgs e)
    {
        if (_db is null)
        {
            ResultsText.Text = "Click Initialize first.";
            return;
        }

        var connection = await _db.EnsureInitializedAsync();
        var reminder = new Reminder
        {
            Title = TitleInput.Text,
            DueAtTicks = DateTime.UtcNow.AddHours(Random.Shared.Next(-12, 12)).Ticks,
        };
        await connection.InsertAsync(reminder);
        ResultsText.Text = $"Inserted: {reminder.Title} due {new DateTime(reminder.DueAtTicks):O}";
    }

    private async void QueryToday_Click(object sender, RoutedEventArgs e)
    {
        if (_db is null)
        {
            ResultsText.Text = "Click Initialize first.";
            return;
        }

        var connection = await _db.EnsureInitializedAsync();
        var today = DateTime.UtcNow.Date;
        var range = DateRange.Between(today, today.AddDays(1).AddTicks(-1));
        var rows = await connection.Table<Reminder>()
            .Where(r => r.DueAtTicks >= range.FromTicks && r.DueAtTicks <= range.ToTicks)
            .OrderBy(r => r.DueAtTicks)
            .ToListAsync();

        ResultsText.Text = rows.Count == 0
            ? "No reminders today."
            : string.Join("\n", rows.Select(r => $"#{r.Id} {new DateTime(r.DueAtTicks):HH:mm:ss}  {r.Title}"));
    }

    private sealed class DemoDb : SqliteDataServiceBase
    {
        public DemoDb(string path) : base(path)
        {
        }

        protected override IMigrationRunner CreateMigrationRunner() =>
            new UserVersionMigrationRunner(new Func<SQLiteAsyncConnection, Task>[]
            {
                static async c => await c.ExecuteAsync(
                    "CREATE TABLE Reminder (Id INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT NOT NULL)"),
                static async c => await c.ExecuteAsync(
                    "ALTER TABLE Reminder ADD COLUMN DueAtTicks INTEGER NOT NULL DEFAULT 0"),
            });
    }

    public sealed class Reminder
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        [Indexed]
        public long DueAtTicks { get; set; }
    }
}
