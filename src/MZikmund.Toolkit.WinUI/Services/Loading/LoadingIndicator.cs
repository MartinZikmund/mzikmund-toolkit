using MZikmund.Toolkit.WinUI.ViewModels;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="ILoadingIndicator"/> implementation. Wraps an
/// <see cref="IWindowShellViewModel"/> and reference-counts <see cref="BeginLoading"/>
/// scopes. <see cref="IWindowShellViewModel.IsLoading"/> stays <see langword="true"/>
/// until the last outstanding scope is disposed; only then is the status message reset.
/// </summary>
/// <remarks>
/// Disposing a scope twice is a no-op. The implementation is thread-safe for the
/// concurrent-scope counting case but does not synchronize updates to the underlying
/// view-model with UI thread requirements — callers should set <see cref="StatusMessage"/>
/// from the UI thread.
/// </remarks>
public sealed class LoadingIndicator : ILoadingIndicator
{
    private readonly IWindowShellViewModel _shellViewModel;
    private readonly object _lock = new();
    private int _activeCount;

    /// <summary>Initializes a new instance backed by the supplied shell view-model.</summary>
    public LoadingIndicator(IWindowShellViewModel shellViewModel)
    {
        ArgumentNullException.ThrowIfNull(shellViewModel);
        _shellViewModel = shellViewModel;
    }

    /// <inheritdoc />
    public bool IsLoading => _shellViewModel.IsLoading;

    /// <inheritdoc />
    public string? StatusMessage
    {
        get => _shellViewModel.LoadingStatusMessage;
        set => _shellViewModel.LoadingStatusMessage = value;
    }

    /// <inheritdoc />
    public IDisposable BeginLoading(string? statusMessage = null)
    {
        lock (_lock)
        {
            _activeCount++;
            _shellViewModel.IsLoading = true;
            if (statusMessage is not null)
            {
                _shellViewModel.LoadingStatusMessage = statusMessage;
            }
        }

        return new Scope(this);
    }

    private void EndScope()
    {
        lock (_lock)
        {
            if (_activeCount == 0)
            {
                return;
            }

            _activeCount--;
            if (_activeCount == 0)
            {
                _shellViewModel.IsLoading = false;
                _shellViewModel.LoadingStatusMessage = null;
            }
        }
    }

    private sealed class Scope : IDisposable
    {
        private LoadingIndicator? _owner;

        public Scope(LoadingIndicator owner) => _owner = owner;

        public void Dispose()
        {
            var owner = Interlocked.Exchange(ref _owner, null);
            owner?.EndScope();
        }
    }
}
