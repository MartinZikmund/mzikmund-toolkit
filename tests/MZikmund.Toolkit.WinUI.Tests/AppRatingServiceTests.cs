using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class AppRatingServiceTests
{
    [TestMethod]
    public void Ctor_NullPreferences_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new AppRatingService(null!, new AppRatingOptions()));
    }

    [TestMethod]
    public void Ctor_NullOptions_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new AppRatingService(new InMemoryPreferences(), null!));
    }

    [TestMethod]
    public void LaunchCount_StartsAtZero_AndIncrements()
    {
        var service = new AppRatingService(new InMemoryPreferences(), new AppRatingOptions());

        Assert.AreEqual(0, service.LaunchCount);
        service.IncrementLaunchCount();
        Assert.AreEqual(1, service.LaunchCount);
        service.IncrementLaunchCount();
        Assert.AreEqual(2, service.LaunchCount);
    }

    [TestMethod]
    public void ShouldRequestRating_RespectsThreshold()
    {
        var service = new AppRatingService(
            new InMemoryPreferences(),
            new AppRatingOptions { MinLaunchCountForRating = 3 });

        service.IncrementLaunchCount();
        Assert.IsFalse(service.ShouldRequestRating);
        service.IncrementLaunchCount();
        Assert.IsFalse(service.ShouldRequestRating);
        service.IncrementLaunchCount();
        Assert.IsTrue(service.ShouldRequestRating);
    }

    [TestMethod]
    public async Task RequestRatingAsync_FlipsHasBeenAsked_AndSuppressesShouldRequest()
    {
        var service = new TestableAppRatingService(
            new InMemoryPreferences(),
            new AppRatingOptions
            {
                WindowsProductId = "9P0",
                MinLaunchCountForRating = 1,
            });
        service.IncrementLaunchCount();
        Assert.IsTrue(service.ShouldRequestRating);

        await service.RequestRatingAsync();

        Assert.IsTrue(service.HasBeenAsked);
        Assert.IsFalse(service.ShouldRequestRating);
    }

    [TestMethod]
    public async Task RequestRatingAsync_NoUriForPlatform_DoesNotMarkAsAsked()
    {
        // No platform identifiers configured ⇒ GetReviewUri returns null ⇒ HasBeenAsked stays false.
        var service = new TestableAppRatingService(
            new InMemoryPreferences(),
            new AppRatingOptions(),
            forcedReviewUri: null);

        var launched = await service.RequestRatingAsync();

        Assert.IsFalse(launched);
        Assert.IsFalse(service.HasBeenAsked);
    }

    [TestMethod]
    public async Task RequestRatingAsync_PassesUriToLaunchHook()
    {
        var service = new TestableAppRatingService(
            new InMemoryPreferences(),
            new AppRatingOptions { WindowsProductId = "9P0" },
            forcedReviewUri: new Uri("ms-windows-store://review/?ProductId=9P0"));

        await service.RequestRatingAsync();

        Assert.AreEqual("ms-windows-store://review/?ProductId=9P0", service.LastUri?.OriginalString);
    }

    [TestMethod]
    public void Reset_ClearsLaunchCountAndAskedFlag()
    {
        var prefs = new InMemoryPreferences();
        var service = new AppRatingService(prefs, new AppRatingOptions { MinLaunchCountForRating = 1 });
        service.IncrementLaunchCount();
        prefs.Set("AppRating.HasBeenAsked", true);

        service.Reset();

        Assert.AreEqual(0, service.LaunchCount);
        Assert.IsFalse(service.HasBeenAsked);
    }

    private sealed class TestableAppRatingService : AppRatingService
    {
        private readonly Uri? _forcedUri;
        private readonly bool _useForcedUri;

        public Uri? LastUri { get; private set; }

        public TestableAppRatingService(IPreferences preferences, AppRatingOptions options)
            : base(preferences, options)
        {
        }

        public TestableAppRatingService(IPreferences preferences, AppRatingOptions options, Uri? forcedReviewUri)
            : base(preferences, options)
        {
            _forcedUri = forcedReviewUri;
            _useForcedUri = true;
        }

        protected override Uri? GetReviewUri() => _useForcedUri ? _forcedUri : base.GetReviewUri();

        protected override Task<bool> LaunchAsync(Uri uri)
        {
            LastUri = uri;
            return Task.FromResult(true);
        }
    }

    private sealed class InMemoryPreferences : IPreferences
    {
        private readonly Dictionary<string, object?> _values = new();

        public T Get<T>(string key, T defaultValue) =>
            _values.TryGetValue(key, out var v) && v is T typed ? typed : defaultValue;

        public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T value)
        {
            if (_values.TryGetValue(key, out var v) && v is T typed)
            {
                value = typed;
                return true;
            }
            value = default;
            return false;
        }

        public void Set<T>(string key, T? value)
        {
            if (value is null) _values.Remove(key);
            else _values[key] = value;
        }

        public T GetComplex<T>(string key, T defaultValue) => Get(key, defaultValue);

        public bool TryGetComplex<T>(string key, [MaybeNullWhen(false)] out T value) => TryGet(key, out value);

        public void SetComplex<T>(string key, T? value) => Set(key, value);

        public bool ContainsKey(string key) => _values.ContainsKey(key);

        public void Remove(string key) => _values.Remove(key);

        public void Clear() => _values.Clear();
    }
}
