using System.Text;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="IMailService"/> implementation. Builds an <c>mailto:</c> URI
/// from the supplied parameters and dispatches it through <see cref="ILauncherService"/>.
/// </summary>
public sealed class MailService : IMailService
{
    private readonly ILauncherService _launcher;

    /// <summary>Initializes a new instance using the supplied <paramref name="launcher"/>.</summary>
    public MailService(ILauncherService launcher)
    {
        ArgumentNullException.ThrowIfNull(launcher);
        _launcher = launcher;
    }

    /// <summary>Initializes a new instance using a default <see cref="LauncherService"/>.</summary>
    public MailService() : this(new LauncherService())
    {
    }

    /// <inheritdoc />
    public Task<bool> ComposeMailAsync(string addressTo, string? subject = null, string? body = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(addressTo);

        var uri = BuildMailtoUri(addressTo, subject, body);
        return _launcher.LaunchUriAsync(uri);
    }

    /// <summary>
    /// Builds the <c>mailto:</c> URI for the supplied parameters. Exposed for
    /// testing; production code should call <see cref="ComposeMailAsync"/>.
    /// </summary>
    public static Uri BuildMailtoUri(string addressTo, string? subject, string? body)
    {
        // Mailto URIs use the raw email address — escaping the '@' breaks the parser.
        var builder = new StringBuilder("mailto:");
        builder.Append(addressTo);

        var hasSubject = !string.IsNullOrEmpty(subject);
        var hasBody = !string.IsNullOrEmpty(body);
        if (hasSubject || hasBody)
        {
            builder.Append('?');
            var first = true;
            if (hasSubject)
            {
                builder.Append("subject=").Append(Uri.EscapeDataString(subject!));
                first = false;
            }
            if (hasBody)
            {
                if (!first) builder.Append('&');
                builder.Append("body=").Append(Uri.EscapeDataString(body!));
            }
        }

        return new Uri(builder.ToString());
    }
}
