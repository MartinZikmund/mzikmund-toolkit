using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class ScheduledNotificationsSamplePage : Page
{
    private readonly IScheduledNotificationService _service = new InMemoryScheduledNotificationService();

    public ScheduledNotificationsSamplePage()
    {
        this.InitializeComponent();
        _ = RefreshAsync();
    }

    private async void Schedule_Click(object sender, RoutedEventArgs e)
    {
        var notification = new ScheduledNotification(
            Id: Guid.NewGuid().ToString(),
            Title: TitleInput.Text,
            Body: BodyInput.Text,
            DeliveryTime: DateTimeOffset.Now.AddMinutes(1));

        await _service.ScheduleAsync(notification);
        await RefreshAsync();
    }

    private async void CancelLast_Click(object sender, RoutedEventArgs e)
    {
        var pending = await _service.GetPendingAsync();
        if (pending.Count > 0)
        {
            await _service.CancelAsync(pending[^1].Id);
        }
        await RefreshAsync();
    }

    private async void CancelAll_Click(object sender, RoutedEventArgs e)
    {
        await _service.CancelAllAsync();
        await RefreshAsync();
    }

    private async void Refresh_Click(object sender, RoutedEventArgs e) => await RefreshAsync();

    private async Task RefreshAsync()
    {
        var pending = await _service.GetPendingAsync();
        PendingText.Text = pending.Count == 0
            ? "(none)"
            : string.Join("\n", pending.Select(n => $"{n.Id[..8]}…  fires {n.DeliveryTime:HH:mm:ss}  {n.Title}: {n.Body}"));
    }

    // The toolkit base class throws PlatformNotSupportedException by default.
    // For the sample we override every method with an in-memory store so the
    // API surface can be exercised without wiring an actual OS toast / alarm.
    private sealed class InMemoryScheduledNotificationService : ScheduledNotificationService
    {
        private readonly Dictionary<string, ScheduledNotification> _pending = new();

        public override Task ScheduleAsync(ScheduledNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification);
            _pending[notification.Id] = notification;
            return Task.CompletedTask;
        }

        public override Task CancelAsync(string id)
        {
            _pending.Remove(id);
            return Task.CompletedTask;
        }

        public override Task CancelAllAsync()
        {
            _pending.Clear();
            return Task.CompletedTask;
        }

        public override Task<IReadOnlyList<ScheduledNotification>> GetPendingAsync() =>
            Task.FromResult<IReadOnlyList<ScheduledNotification>>(_pending.Values.ToList());
    }
}
