using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;
using MZikmund.Toolkit.WinUI.ViewModels;

namespace MZikmund.Toolkit.Sample;

public sealed partial class LoadingIndicatorSamplePage : Page
{
    private readonly Stack<IDisposable> _activeScopes = new();
    private readonly ILoadingIndicator _indicator;

    public LoadingIndicatorSamplePage()
    {
        ShellViewModel = new WindowShellViewModel();
        _indicator = new LoadingIndicator(ShellViewModel);
        this.InitializeComponent();
        UpdateState();
    }

    public WindowShellViewModel ShellViewModel { get; }

    private void Begin_Click(object sender, RoutedEventArgs e)
    {
        _activeScopes.Push(_indicator.BeginLoading($"Scope {_activeScopes.Count + 1}…"));
        UpdateState();
    }

    private void End_Click(object sender, RoutedEventArgs e)
    {
        if (_activeScopes.Count > 0)
        {
            _activeScopes.Pop().Dispose();
        }
        UpdateState();
    }

    private async void Nested_Click(object sender, RoutedEventArgs e)
    {
        using var outer = _indicator.BeginLoading("Outer job…");
        UpdateState();
        await Task.Delay(2_000);

        using (_indicator.BeginLoading("Inner job…"))
        {
            UpdateState();
            await Task.Delay(2_000);
        }

        UpdateState();
        await Task.Delay(1_000);
        UpdateState();
    }

    private void UpdateState()
    {
        StateText.Text = $"IsLoading: {_indicator.IsLoading}, active scopes: {_activeScopes.Count}";
    }
}
