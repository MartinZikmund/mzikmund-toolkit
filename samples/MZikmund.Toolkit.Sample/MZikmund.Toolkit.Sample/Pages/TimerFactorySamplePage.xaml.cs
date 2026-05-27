using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class TimerFactorySamplePage : Page
{
    private readonly MZikmund.Toolkit.WinUI.Services.ITimerFactory _factory;
    private readonly MZikmund.Toolkit.WinUI.Services.ITimer _timer;

    public TimerFactorySamplePage()
    {
        this.InitializeComponent();
        _factory = new TimerFactory();
        _timer = _factory.CreateTimer(TimeSpan.FromSeconds(1));
        _timer.Tick += OnTick;
        UpdateClock();
    }

    private void StartStop_Click(object sender, RoutedEventArgs e)
    {
        if (_timer.IsRunning)
        {
            _timer.Stop();
            StartStopButton.Content = "Start";
        }
        else
        {
            _timer.Start();
            StartStopButton.Content = "Stop";
        }
    }

    private void OnTick(object? sender, EventArgs e) => UpdateClock();

    private void UpdateClock() => ClockText.Text = DateTimeOffset.Now.ToString("HH:mm:ss");
}
