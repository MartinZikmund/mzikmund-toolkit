namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Configuration for <see cref="StoreService"/> — the SKU identifiers used when talking
/// to the Microsoft Store.
/// </summary>
public sealed class StoreServiceOptions
{
    /// <summary>The Microsoft Store add-on (durable / consumable) SKU ID for the Pro entitlement.</summary>
    public string ProSkuId { get; init; } = string.Empty;
}
