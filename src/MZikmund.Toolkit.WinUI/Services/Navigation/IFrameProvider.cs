namespace MZikmund.Toolkit.WinUI.Services.Navigation;

/// <summary>
/// Provides access to the navigation frame for the current scope.
/// </summary>
public interface IFrameProvider
{
	/// <summary>
	/// Gets the navigation frame for the current scope.
	/// </summary>
	/// <returns>The current navigation frame.</returns>
	Frame GetForCurrentScope();
}
