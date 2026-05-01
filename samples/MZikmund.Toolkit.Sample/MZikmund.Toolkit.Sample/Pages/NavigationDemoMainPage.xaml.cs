using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Navigation;

namespace MZikmund.Toolkit.Sample;

[NavigationInfo("main", NavigationTransition.FromRight)]
public sealed partial class NavigationDemoMainPage : Page
{
    public NavigationDemoMainPage()
    {
        this.InitializeComponent();
    }
}
