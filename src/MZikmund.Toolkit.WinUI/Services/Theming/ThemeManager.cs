using Microsoft.UI;
using MZikmund.Toolkit.WinUI.Services.Navigation;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace MZikmund.Toolkit.WinUI.Services.Theming;

/// <summary>
/// Manages the application theme and updates the Windows title bar colors
/// to match the current theme.
/// </summary>
public sealed class ThemeManager : IThemeManager
{
	private readonly IWindowShellProvider _windowShellProvider;
	private readonly UISettings _uiSettings = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="ThemeManager"/> class.
	/// </summary>
	/// <param name="windowShellProvider">The window shell provider.</param>
	public ThemeManager(IWindowShellProvider windowShellProvider)
	{
		_windowShellProvider = windowShellProvider;
		_uiSettings.ColorValuesChanged += OnColorValuesChanged;
	}

	/// <inheritdoc/>
	public void SetTheme(ElementTheme theme)
	{
		GetRootElement().RequestedTheme = theme;
		UpdateTitleBarTheming();
	}

	/// <inheritdoc/>
	public ElementTheme CurrentTheme => GetRootElement().RequestedTheme;

	/// <inheritdoc/>
	public ApplicationTheme ActualTheme => CurrentTheme switch
	{
		ElementTheme.Default => Application.Current.RequestedTheme,
		ElementTheme.Light => ApplicationTheme.Light,
		ElementTheme.Dark => ApplicationTheme.Dark,
		_ => throw new ArgumentOutOfRangeException(),
	};

	private FrameworkElement GetRootElement()
	{
		return _windowShellProvider.Shell as FrameworkElement
			?? throw new InvalidOperationException("Root element of the window is not a FrameworkElement.");
	}

	private void UpdateTitleBarTheming()
	{
#if !HAS_UNO
		try
		{
			var titleBar = _windowShellProvider.Window.AppWindow.TitleBar;
			titleBar.BackgroundColor = Colors.Transparent;
			titleBar.ButtonBackgroundColor = Colors.Transparent;
			titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

			if (ActualTheme == ApplicationTheme.Dark)
			{
				titleBar.ButtonForegroundColor = Colors.White;
				titleBar.ButtonInactiveForegroundColor = Colors.Gray;
				titleBar.ButtonHoverBackgroundColor = Color.FromArgb(100, 100, 100, 100);
				titleBar.ButtonHoverForegroundColor = Colors.White;
				titleBar.ButtonPressedBackgroundColor = Color.FromArgb(200, 100, 100, 100);
				titleBar.ButtonPressedForegroundColor = Colors.White;
			}
			else
			{
				titleBar.ButtonForegroundColor = Colors.Black;
				titleBar.ButtonInactiveForegroundColor = Colors.Gray;
				titleBar.ButtonHoverBackgroundColor = Color.FromArgb(100, 200, 200, 200);
				titleBar.ButtonHoverForegroundColor = Colors.Black;
				titleBar.ButtonPressedBackgroundColor = Color.FromArgb(200, 200, 200, 200);
				titleBar.ButtonPressedForegroundColor = Colors.Black;
			}
		}
		catch
		{
			// Title bar customization not supported on this platform
		}
#endif
	}

	private void OnColorValuesChanged(UISettings sender, object args)
	{
		_windowShellProvider.DispatcherQueue.TryEnqueue(UpdateTitleBarTheming);
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		_uiSettings.ColorValuesChanged -= OnColorValuesChanged;
	}
}
