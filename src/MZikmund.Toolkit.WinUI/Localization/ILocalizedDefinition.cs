namespace MZikmund.Toolkit.WinUI.Localization;

/// <summary>
/// Marks a model — typically an enum-like definition (achievement, settings option,
/// game level, …) — that carries localization keys instead of hard-coded labels.
/// </summary>
/// <remarks>
/// Pair with <see cref="LocalizedDefinitionExtensions.GetName(ILocalizedDefinition)"/> /
/// <see cref="LocalizedDefinitionExtensions.GetDescription(ILocalizedDefinition)"/> to
/// resolve through <see cref="Localizer.Current"/>, or with the overloads that take an
/// <see cref="ILocalizer"/> directly when DI is preferred.
/// </remarks>
public interface ILocalizedDefinition
{
    /// <summary>
    /// Resource key for the human-readable display name.
    /// </summary>
    string NameKey { get; }

    /// <summary>
    /// Resource key for the descriptive subtitle / longer text. Implementations may return
    /// an empty string when no description applies.
    /// </summary>
    string DescriptionKey { get; }
}
