namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// "Always Pro" implementation of <see cref="IStoreService"/>. Use for sideloaded /
/// free build flavours where every user gets Pro features without going through
/// a store. Distinct from <see cref="FakeStoreService"/>: this one is a permanent
/// production stub, not a development convenience — there is no "non-Pro" state to
/// transition out of.
/// </summary>
public sealed class ProStoreService : IStoreService
{
    /// <inheritdoc />
    public Task<bool> HasProAsync() => Task.FromResult(true);

    /// <inheritdoc />
    public Task<bool> TryPurchaseProAsync() => Task.FromResult(true);

    /// <inheritdoc />
    public Task<string?> GetPriceAsync() => Task.FromResult<string?>(null);

    /// <inheritdoc />
    public Task<bool> TryRestorePurchasesAsync() => Task.FromResult(true);
}
