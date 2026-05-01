using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Navigation;

namespace MZikmund.Toolkit.Sample;

[NavigationInfo("settings", NavigationTransition.FromBottom)]
public sealed partial class NavigationDemoSettingsPage : Page
{
    public NavigationDemoSettingsPage()
    {
        this.InitializeComponent();
    }
}
