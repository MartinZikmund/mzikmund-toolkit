namespace MZikmund.Toolkit.WinUI.Localization;

/// <summary>
/// Static accessor for the currently registered <see cref="ILocalizer"/>.
/// Used by <c>LocalizeExtension</c> and other consumers that can't accept constructor injection.
/// </summary>
/// <remarks>
/// The default value never returns null — it falls back to <c>???key???</c>, making missing
/// translations visually obvious in the UI. Apps replace <see cref="Current"/> at startup.
/// </remarks>
public static class Localizer
{
    /// <summary>
    /// The active localizer. Defaults to a fallback that returns <c>???key???</c> for any key.
    /// </summary>
    public static ILocalizer Current { get; set; } = new MissingKeyLocalizer();

    private sealed class MissingKeyLocalizer : ILocalizer
    {
        public string GetString(string key) => $"???{key}???";
    }
}
