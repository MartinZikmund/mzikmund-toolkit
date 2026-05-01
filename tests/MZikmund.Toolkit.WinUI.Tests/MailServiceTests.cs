using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class MailServiceTests
{
    [TestMethod]
    public void Ctor_NullLauncher_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new MailService(null!));
    }

    [TestMethod]
    public void BuildMailtoUri_OnlyAddress_OmitsQuery()
    {
        var uri = MailService.BuildMailtoUri("a@example.com", subject: null, body: null);

        Assert.AreEqual("mailto:a@example.com", uri.OriginalString);
    }

    [TestMethod]
    public void BuildMailtoUri_WithSubject_AddsQuery()
    {
        var uri = MailService.BuildMailtoUri("a@example.com", subject: "Hello world", body: null);

        Assert.AreEqual("mailto:a@example.com?subject=Hello%20world", uri.OriginalString);
    }

    [TestMethod]
    public void BuildMailtoUri_WithBody_AddsBody()
    {
        var uri = MailService.BuildMailtoUri("a@example.com", subject: null, body: "Line1\nLine2");

        Assert.AreEqual("mailto:a@example.com?body=Line1%0ALine2", uri.OriginalString);
    }

    [TestMethod]
    public void BuildMailtoUri_WithSubjectAndBody_JoinsByAmpersand()
    {
        var uri = MailService.BuildMailtoUri("a@example.com", subject: "Re: question", body: "Hi.");

        Assert.AreEqual("mailto:a@example.com?subject=Re%3A%20question&body=Hi.", uri.OriginalString);
    }

    [TestMethod]
    public void BuildMailtoUri_EmptyStrings_TreatedAsAbsent()
    {
        var uri = MailService.BuildMailtoUri("a@example.com", subject: string.Empty, body: string.Empty);

        Assert.AreEqual("mailto:a@example.com", uri.OriginalString);
    }

    [TestMethod]
    public async Task ComposeMailAsync_NullOrWhitespaceAddress_Throws()
    {
        var service = new MailService(new CapturingLauncher());

        await Assert.ThrowsAsync<ArgumentException>(() => service.ComposeMailAsync(null!));
        await Assert.ThrowsAsync<ArgumentException>(() => service.ComposeMailAsync(string.Empty));
        await Assert.ThrowsAsync<ArgumentException>(() => service.ComposeMailAsync("   "));
    }

    [TestMethod]
    public async Task ComposeMailAsync_DispatchesBuiltUri_ToLauncher()
    {
        var launcher = new CapturingLauncher();
        var service = new MailService(launcher);

        await service.ComposeMailAsync("a@example.com", subject: "Hi", body: "Body");

        Assert.AreEqual(
            "mailto:a@example.com?subject=Hi&body=Body",
            launcher.LastUri?.OriginalString);
    }

    private sealed class CapturingLauncher : ILauncherService
    {
        public Uri? LastUri { get; private set; }

        public Task<bool> LaunchUriAsync(Uri uri)
        {
            LastUri = uri;
            return Task.FromResult(true);
        }
    }
}
