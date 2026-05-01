using MZikmund.Toolkit.WinUI.Views;

namespace MZikmund.Toolkit.Sample;

// XAML doesn't accept a generic class as a root element, so DemoView's XAML inherits
// from this non-generic intermediate, which is the standard ViewBase<T> pattern.
public partial class DemoViewBase : ViewBase<DemoViewModel>
{
}
