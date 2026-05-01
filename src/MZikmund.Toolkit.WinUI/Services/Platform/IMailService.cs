namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Cross-platform <c>mailto:</c> launcher. Opens the user's default mail client
/// pre-populated with the supplied recipient, subject, and body.
/// </summary>
public interface IMailService
{
    /// <summary>
    /// Opens a new mail-compose window addressed to <paramref name="addressTo"/>.
    /// </summary>
    /// <param name="addressTo">Primary recipient.</param>
    /// <param name="subject">Optional subject line.</param>
    /// <param name="body">Optional body text.</param>
    /// <returns><see langword="true"/> when the mail client launched successfully.</returns>
    Task<bool> ComposeMailAsync(string addressTo, string? subject = null, string? body = null);
}
