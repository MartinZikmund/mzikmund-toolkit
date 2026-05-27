namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Cross-platform "share text / URL" wrapper over <c>DataTransferManager</c>.
/// </summary>
public interface IShareService
{
    /// <summary>
    /// Shows the system share picker pre-populated with <paramref name="title"/> and
    /// <paramref name="text"/>. Optional <paramref name="webLink"/> is attached as a URL
    /// when supplied.
    /// </summary>
    Task ShareTextAsync(string title, string text, Uri? webLink = null);
}
