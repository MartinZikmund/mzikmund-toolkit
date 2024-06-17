namespace MZikmund.Toolkit.WinUI.Infrastructure;

/// <summary>
/// Provides access to the XamlRoot instance.
/// </summary>
public interface IXamlRootProvider
{
    /// <summary>
    /// Gets the XamlRoot instance.
    /// </summary>
    XamlRoot XamlRoot { get; }
}
