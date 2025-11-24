using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MZikmund.Toolkit.Sample;

public sealed partial class Shell : Page
{
    public Shell()
    {
        this.InitializeComponent();
        NavView.SelectedItem = NavView.MenuItems[0];
        ContentFrame.Navigate(typeof(HomePage));
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            // Navigate to settings if needed
            return;
        }

        var selectedItem = args.InvokedItemContainer as NavigationViewItem;
        if (selectedItem?.Tag is string tag)
        {
            var pageType = tag switch
            {
                "Home" => typeof(HomePage),
                "Preferences" => typeof(PreferencesSamplePage),
                "DialogCoordinator" => typeof(DialogCoordinatorSamplePage),
                "PackageVersion" => typeof(PackageVersionSamplePage),
                "XamlRootProvider" => typeof(XamlRootProviderSamplePage),
                _ => null
            };

            if (pageType != null && ContentFrame.CurrentSourcePageType != pageType)
            {
                ContentFrame.Navigate(pageType);
            }
        }
    }

    private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        if (ContentFrame.CanGoBack)
        {
            ContentFrame.GoBack();
        }
    }
}
