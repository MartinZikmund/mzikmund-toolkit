using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using MZikmund.Toolkit.WinUI.Services;
using Windows.Storage.Pickers;

namespace MZikmund.Toolkit.Sample;

public sealed partial class ImagePickerSamplePage : Page
{
    public ImagePickerSamplePage()
    {
        this.InitializeComponent();
    }

    private async void Pick_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var service = new SampleImagePickerService(this);
            var uri = await service.PickAsync();
            if (uri is null)
            {
                UriText.Text = "Picker cancelled.";
                PreviewImage.Source = null;
                return;
            }

            UriText.Text = uri.OriginalString;
            PreviewImage.Source = new BitmapImage(uri);
        }
        catch (Exception ex)
        {
            UriText.Text = $"Failed: {ex.GetType().Name}: {ex.Message}";
            PreviewImage.Source = null;
        }
    }

    // The default ImagePickerService doesn't bind a window handle, so on WinAppSDK
    // desktop the picker fails to open. This subclass provides the binding for the
    // sample app's window.
    private sealed class SampleImagePickerService : ImagePickerService
    {
        private readonly Page _hostPage;

        public SampleImagePickerService(Page hostPage)
            : base(new ImagePickerOptions { TargetSubfolder = "SamplePickedImages" })
        {
            _hostPage = hostPage;
        }

        protected override void ConfigurePicker(FileOpenPicker picker)
        {
#if WINDOWS
            var window = (App.Current as App)?.MainWindow;
            if (window is not null)
            {
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            }
#endif
        }
    }
}
