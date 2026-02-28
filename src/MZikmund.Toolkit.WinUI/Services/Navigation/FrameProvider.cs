namespace MZikmund.Toolkit.WinUI.Services.Navigation;

/// <summary>
/// Default implementation of <see cref="IFrameProvider"/> that retrieves
/// the frame from the <see cref="IWindowShellProvider"/>.
/// </summary>
public class FrameProvider : IFrameProvider
{
	private readonly IWindowShellProvider _windowShellProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="FrameProvider"/> class.
	/// </summary>
	/// <param name="windowShellProvider">The window shell provider.</param>
	public FrameProvider(IWindowShellProvider windowShellProvider)
	{
		_windowShellProvider = windowShellProvider;
	}

	/// <inheritdoc/>
	public Frame GetForCurrentScope() => _windowShellProvider.RootFrame;
}
