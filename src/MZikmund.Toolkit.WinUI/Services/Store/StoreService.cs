#if WINDOWS
using Windows.Services.Store;
#endif

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Microsoft Store implementation of <see cref="IStoreService"/> backed by
/// <c>Windows.Services.Store.StoreContext</c>. On non-Windows targets every method throws
/// <see cref="PlatformNotSupportedException"/> — pick a different <see cref="IStoreService"/>
/// implementation per platform via DI.
/// </summary>
/// <remarks>
/// The purchase flow on WinAppSDK requires the <c>StoreContext</c> to be initialized with the
/// owning window handle (<c>WinRT.Interop.InitializeWithWindow.Initialize</c>). Apps that need
/// purchase support should subclass and override <see cref="InitializeContext"/> to perform
/// that handle binding before the first call.
/// </remarks>
public class StoreService : IStoreService
{
    private readonly StoreServiceOptions _options;

#if WINDOWS
    private StoreContext? _context;
#endif

    /// <summary>Initializes a new instance.</summary>
    public StoreService(StoreServiceOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options;
    }

#if WINDOWS
    /// <summary>
    /// Hook for initializing the Microsoft Store <c>StoreContext</c>, e.g. binding it to
    /// a window via <c>WinRT.Interop.InitializeWithWindow.Initialize</c>. The default
    /// implementation does nothing.
    /// </summary>
    /// <param name="context">The newly created context, before its first use.</param>
    protected virtual void InitializeContext(StoreContext context)
    {
    }

    private StoreContext Context
    {
        get
        {
            if (_context is null)
            {
                _context = StoreContext.GetDefault();
                InitializeContext(_context);
            }

            return _context;
        }
    }
#endif

    /// <inheritdoc />
    public async Task<bool> HasProAsync()
    {
#if WINDOWS
        var license = await Context.GetAppLicenseAsync();
        if (license is null)
        {
            return false;
        }

        if (string.IsNullOrEmpty(_options.ProSkuId))
        {
            return false;
        }

        return license.AddOnLicenses.TryGetValue(_options.ProSkuId, out var addOn) && addOn.IsActive;
#else
        await Task.CompletedTask;
        throw new PlatformNotSupportedException("StoreService is only supported on Windows. Use FakeStoreService or ProStoreService elsewhere.");
#endif
    }

    /// <inheritdoc />
    public async Task<bool> TryPurchaseProAsync()
    {
#if WINDOWS
        if (string.IsNullOrEmpty(_options.ProSkuId))
        {
            return false;
        }

        var result = await Context.RequestPurchaseAsync(_options.ProSkuId);
        return result.Status is StorePurchaseStatus.Succeeded or StorePurchaseStatus.AlreadyPurchased;
#else
        await Task.CompletedTask;
        throw new PlatformNotSupportedException("StoreService is only supported on Windows. Use FakeStoreService or ProStoreService elsewhere.");
#endif
    }

    /// <inheritdoc />
    public async Task<string?> GetPriceAsync()
    {
#if WINDOWS
        if (string.IsNullOrEmpty(_options.ProSkuId))
        {
            return null;
        }

        var query = await Context.GetStoreProductsAsync(["Durable"], [_options.ProSkuId]);
        if (query.Products.TryGetValue(_options.ProSkuId, out var product))
        {
            return product.Price?.FormattedPrice;
        }

        return null;
#else
        await Task.CompletedTask;
        throw new PlatformNotSupportedException("StoreService is only supported on Windows. Use FakeStoreService or ProStoreService elsewhere.");
#endif
    }

    /// <inheritdoc />
    public Task<bool> TryRestorePurchasesAsync()
    {
#if WINDOWS
        // Microsoft Store entitlements are surfaced through GetAppLicenseAsync directly; there
        // is no separate "restore" call. Querying HasPro forces a license refresh.
        return HasProAsync();
#else
        throw new PlatformNotSupportedException("StoreService is only supported on Windows. Use FakeStoreService or ProStoreService elsewhere.");
#endif
    }
}
