namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// App-wide loading indicator with <c>using var _ = indicator.BeginLoading()</c>
/// ergonomics. Reference-counts concurrent loading scopes so nested operations
/// don't toggle <see cref="IsLoading"/> off prematurely.
/// </summary>
public interface ILoadingIndicator
{
    /// <summary>
    /// <see langword="true"/> when at least one loading scope is active.
    /// </summary>
    bool IsLoading { get; }

    /// <summary>
    /// Status message shown alongside the loading indicator. Setting this updates
    /// the underlying view-model immediately.
    /// </summary>
    string? StatusMessage { get; set; }

    /// <summary>
    /// Begins a loading scope. Dispose the returned <see cref="IDisposable"/> to end it.
    /// </summary>
    /// <param name="statusMessage">Optional status text shown for this scope.</param>
    IDisposable BeginLoading(string? statusMessage = null);
}
