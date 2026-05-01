using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Dialogs;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class ColorPickerDialogStringsTests
{
    // ColorPickerDialog itself derives from ContentDialog and can't be instantiated
    // headlessly. The strings POCO is the testable portion of the surface.

    [TestMethod]
    public void Defaults_AreEnglish()
    {
        var s = new ColorPickerDialogStrings();

        Assert.AreEqual("Choose a color", s.Title);
        Assert.AreEqual("OK", s.OkButtonText);
        Assert.AreEqual("Cancel", s.CancelButtonText);
    }

    [TestMethod]
    public void InitOnly_AppliesCustomValues()
    {
        var s = new ColorPickerDialogStrings
        {
            Title = "Vyberte barvu",
            OkButtonText = "Potvrdit",
            CancelButtonText = "Zrušit",
        };

        Assert.AreEqual("Vyberte barvu", s.Title);
        Assert.AreEqual("Potvrdit", s.OkButtonText);
        Assert.AreEqual("Zrušit", s.CancelButtonText);
    }
}
