namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// A captured deep-link / pending-navigation request: the activating <see cref="Uri"/>
/// plus optional structured parameters extracted from it.
/// </summary>
/// <param name="Uri">The activating URI (e.g. <c>myapp://reminder/42</c>).</param>
/// <param name="Parameters">
/// Optional already-parsed parameters. The toolkit doesn't prescribe how callers parse
/// the URI — apps that already split the URI into a key/value bag can pass it here.
/// </param>
public sealed record DeepLinkRequest(Uri Uri, IReadOnlyDictionary<string, string>? Parameters = null);
