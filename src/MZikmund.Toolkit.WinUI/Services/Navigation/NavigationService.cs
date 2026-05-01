using System.Reflection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.Navigation;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="INavigationService"/> implementation. Resolves the active
/// <see cref="Frame"/> through <see cref="IWindowShellProvider"/>, attaches to
/// <see cref="Frame.Navigated"/> the first time it sees the frame, reads
/// <see cref="NavigationInfoAttribute"/> on each destination type, and dispatches
/// the corresponding <see cref="NavigationTransitionInfo"/>.
/// </summary>
public sealed class NavigationService : INavigationService
{
    private readonly IWindowShellProvider _shellProvider;
    private Frame? _subscribedFrame;

    /// <summary>Initializes a new instance.</summary>
    public NavigationService(IWindowShellProvider shellProvider)
    {
        ArgumentNullException.ThrowIfNull(shellProvider);
        _shellProvider = shellProvider;
    }

    /// <inheritdoc />
    public string? CurrentSection { get; private set; }

    /// <inheritdoc />
    public bool CanGoBack => GetFrame()?.CanGoBack ?? false;

    /// <inheritdoc />
    public event EventHandler? Navigated;

    /// <inheritdoc />
    public bool Navigate(Type pageType, object? parameter = null)
    {
        ArgumentNullException.ThrowIfNull(pageType);

        var frame = GetFrame()
            ?? throw new InvalidOperationException("No IWindowShell.RootFrame available — register an IWindowShell before navigating.");

        var info = pageType.GetCustomAttribute<NavigationInfoAttribute>();
        var transition = info?.Transition ?? NavigationTransition.Default;
        return frame.Navigate(pageType, parameter, BuildTransitionInfo(transition));
    }

    /// <inheritdoc />
    public bool GoBack()
    {
        var frame = GetFrame();
        if (frame is null || !frame.CanGoBack)
        {
            return false;
        }

        frame.GoBack();
        return true;
    }

    /// <summary>
    /// Maps a <see cref="NavigationTransition"/> to its corresponding <see cref="NavigationTransitionInfo"/>
    /// (or <see langword="null"/> for the frame's default).
    /// </summary>
    public static NavigationTransitionInfo? BuildTransitionInfo(NavigationTransition transition) => transition switch
    {
        NavigationTransition.Suppress => new SuppressNavigationTransitionInfo(),
        NavigationTransition.FromRight => new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight },
        NavigationTransition.FromLeft => new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft },
        NavigationTransition.FromBottom => new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromBottom },
        _ => null,
    };

    private Frame? GetFrame()
    {
        Frame? frame;
        try
        {
            frame = _shellProvider.Shell.RootFrame;
        }
        catch (InvalidOperationException)
        {
            return null;
        }

        if (frame is not null && !ReferenceEquals(_subscribedFrame, frame))
        {
            if (_subscribedFrame is not null)
            {
                _subscribedFrame.Navigated -= OnFrameNavigated;
            }

            frame.Navigated += OnFrameNavigated;
            _subscribedFrame = frame;
        }

        return frame;
    }

    private void OnFrameNavigated(object sender, NavigationEventArgs e)
    {
        var info = e.SourcePageType?.GetCustomAttribute<NavigationInfoAttribute>();
        CurrentSection = info?.SectionTag;
        Navigated?.Invoke(this, EventArgs.Empty);
    }
}
