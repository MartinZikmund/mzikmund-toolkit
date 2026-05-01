namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Development / DEBUG implementation of <see cref="IStoreService"/>. Tracks Pro state
/// in memory and lets a "purchase" toggle it on. <see cref="GetPriceAsync"/> returns a
/// fake price string so UI tests aren't blocked on a real store fetch.
/// </summary>
/// <remarks>
/// The default starts as Pro because most dev workflows want to see the unlocked UI.
/// Construct with <c>startsAsPro: false</c> when exercising the upgrade flow.
/// </remarks>
public sealed class FakeStoreService : IStoreService
{
    private bool _hasPro;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="startsAsPro">Initial Pro state. Defaults to <see langword="true"/> for development convenience.</param>
    /// <param name="fakePrice">Display price returned by <see cref="GetPriceAsync"/>.</param>
    public FakeStoreService(bool startsAsPro = true, string fakePrice = "$0.00 (dev)")
    {
        _hasPro = startsAsPro;
        FakePrice = fakePrice;
    }

    /// <summary>The display price returned by <see cref="GetPriceAsync"/>.</summary>
    public string FakePrice { get; }

    /// <inheritdoc />
    public Task<bool> HasProAsync() => Task.FromResult(_hasPro);

    /// <inheritdoc />
    public Task<bool> TryPurchaseProAsync()
    {
        _hasPro = true;
        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<string?> GetPriceAsync() => Task.FromResult<string?>(FakePrice);

    /// <inheritdoc />
    public Task<bool> TryRestorePurchasesAsync() => Task.FromResult(_hasPro);
}
