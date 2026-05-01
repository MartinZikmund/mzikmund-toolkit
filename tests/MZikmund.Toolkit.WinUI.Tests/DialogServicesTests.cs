using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class DialogServicesTests
{
    // The DialogService and ConfirmationDialogService construct ContentDialog instances,
    // which is not possible in the headless test host (FrameworkElement requires a running
    // XAML runtime). Service-level behavior is exercised manually through the sample page.
    // These tests cover the support types only.

    [TestMethod]
    public void DialogStrings_Defaults_AreEnglish()
    {
        var strings = new DialogStrings();

        Assert.AreEqual("OK", strings.OkButtonText);
        Assert.AreEqual("Yes", strings.YesButtonText);
        Assert.AreEqual("No", strings.NoButtonText);
    }

    [TestMethod]
    public void DialogStrings_AreInitOnly_AndPreserveCustomValues()
    {
        var strings = new DialogStrings
        {
            OkButtonText = "Continuer",
            YesButtonText = "Oui",
            NoButtonText = "Non",
        };

        Assert.AreEqual("Continuer", strings.OkButtonText);
        Assert.AreEqual("Oui", strings.YesButtonText);
        Assert.AreEqual("Non", strings.NoButtonText);
    }

    [TestMethod]
    public void ConfirmationResult_HasYesAndNoMembers()
    {
        // Sanity check that the public enum surface stays stable.
        Assert.AreEqual(0, (int)ConfirmationResult.Yes);
        Assert.AreEqual(1, (int)ConfirmationResult.No);
    }
}
