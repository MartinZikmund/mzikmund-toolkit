namespace MZikmund.Toolkit.WinUI.Localization;

/// <summary>
/// Convenience helpers for resolving <see cref="ILocalizedDefinition"/> keys through an
/// <see cref="ILocalizer"/> (or the static <see cref="Localizer.Current"/>).
/// </summary>
public static class LocalizedDefinitionExtensions
{
    /// <summary>
    /// Returns the localized display name for <paramref name="definition"/> using the supplied <paramref name="localizer"/>.
    /// </summary>
    public static string GetName(this ILocalizedDefinition definition, ILocalizer localizer)
    {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(localizer);
        return localizer.GetString(definition.NameKey);
    }

    /// <summary>
    /// Returns the localized description for <paramref name="definition"/> using the supplied <paramref name="localizer"/>.
    /// Empty <see cref="ILocalizedDefinition.DescriptionKey"/> short-circuits to <see cref="string.Empty"/>.
    /// </summary>
    public static string GetDescription(this ILocalizedDefinition definition, ILocalizer localizer)
    {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(localizer);
        return string.IsNullOrEmpty(definition.DescriptionKey)
            ? string.Empty
            : localizer.GetString(definition.DescriptionKey);
    }

    /// <summary>
    /// Returns the localized display name through <see cref="Localizer.Current"/>.
    /// Use the <see cref="GetName(ILocalizedDefinition, ILocalizer)"/> overload when DI is preferred.
    /// </summary>
    public static string GetName(this ILocalizedDefinition definition) =>
        definition.GetName(Localizer.Current);

    /// <summary>
    /// Returns the localized description through <see cref="Localizer.Current"/>.
    /// Use the <see cref="GetDescription(ILocalizedDefinition, ILocalizer)"/> overload when DI is preferred.
    /// </summary>
    public static string GetDescription(this ILocalizedDefinition definition) =>
        definition.GetDescription(Localizer.Current);
}
