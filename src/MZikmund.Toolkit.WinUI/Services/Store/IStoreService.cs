namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Cross-platform abstraction over an in-app-purchase store. Apps that want
/// "Pro / Premium" entitlements depend on this interface and let the host
/// pick a concrete implementation per build flavour:
/// <list type="bullet">
///   <item><description><see cref="FakeStoreService"/> for development</description></item>
///   <item><description><see cref="ProStoreService"/> for free / sideloaded builds where everyone is Pro</description></item>
///   <item><description><see cref="StoreService"/> for the real Microsoft Store on Windows</description></item>
/// </list>
/// </summary>
public interface IStoreService
{
    /// <summary>
    /// Returns <see langword="true"/> if the current user has an active Pro entitlement.
    /// </summary>
    Task<bool> HasProAsync();

    /// <summary>
    /// Initiates a Pro purchase flow.
    /// </summary>
    /// <returns><see langword="true"/> on a successful or already-owned purchase; <see langword="false"/> on user cancel or platform failure.</returns>
    Task<bool> TryPurchaseProAsync();

    /// <summary>
    /// Returns the localized display price for the Pro SKU, or <see langword="null"/> if the price is unknown.
    /// </summary>
    Task<string?> GetPriceAsync();

    /// <summary>
    /// Restores previously purchased entitlements (e.g. on a new install or after sign-in).
    /// </summary>
    /// <returns><see langword="true"/> if any entitlement was restored or already present; otherwise <see langword="false"/>.</returns>
    Task<bool> TryRestorePurchasesAsync();
}
