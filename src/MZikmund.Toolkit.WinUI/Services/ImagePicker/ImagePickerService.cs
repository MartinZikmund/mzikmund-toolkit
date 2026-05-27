using Windows.Storage;
using Windows.Storage.Pickers;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="IImagePickerService"/> backed by <see cref="FileOpenPicker"/>.
/// Configurable via <see cref="ImagePickerOptions"/>: target subfolder name and
/// allowed file extensions.
/// </summary>
/// <remarks>
/// On WinAppSDK desktop, the picker requires its <c>FileOpenPicker</c> to be initialized
/// with the active window handle (<c>WinRT.Interop.InitializeWithWindow.Initialize</c>).
/// Apps that ship on Windows subclass and override <see cref="ConfigurePicker"/> to do
/// that bind before the picker is shown. The default implementation only sets the
/// allowed extensions.
/// </remarks>
public class ImagePickerService : IImagePickerService
{
    private readonly ImagePickerOptions _options;

    /// <summary>Initializes a new instance.</summary>
    public ImagePickerService(ImagePickerOptions? options = null)
    {
        _options = options ?? new ImagePickerOptions();
    }

    /// <inheritdoc />
    public async Task<Uri?> PickAsync()
    {
        var picker = new FileOpenPicker
        {
            ViewMode = PickerViewMode.Thumbnail,
            SuggestedStartLocation = PickerLocationId.PicturesLibrary,
        };

        foreach (var ext in _options.AllowedExtensions)
        {
            picker.FileTypeFilter.Add(ext);
        }

        ConfigurePicker(picker);

        var file = await picker.PickSingleFileAsync();
        if (file is null)
        {
            return null;
        }

        return await CopyToLocalFolderAsync(file);
    }

    /// <summary>
    /// Hook for app-specific picker configuration — typically the WinAppSDK
    /// <c>InitializeWithWindow.Initialize(picker, hWnd)</c> call. Default is a no-op.
    /// </summary>
    protected virtual void ConfigurePicker(FileOpenPicker picker)
    {
    }

    /// <summary>
    /// Copies <paramref name="source"/> into the configured <see cref="ImagePickerOptions.TargetSubfolder"/>
    /// under a fresh GUID-based name and returns the <c>ms-appdata://</c> URI for it.
    /// </summary>
    /// <remarks>Exposed for testing; production code should use <see cref="PickAsync"/>.</remarks>
    public async Task<Uri> CopyToLocalFolderAsync(StorageFile source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var localFolder = ApplicationData.Current.LocalFolder;
        var subfolder = await localFolder.CreateFolderAsync(_options.TargetSubfolder, CreationCollisionOption.OpenIfExists);
        var copiedName = BuildCopiedFileName(source.Path);
        var copy = await source.CopyAsync(subfolder, copiedName, NameCollisionOption.ReplaceExisting);
        return new Uri($"ms-appdata:///local/{_options.TargetSubfolder}/{copy.Name}");
    }

    /// <summary>
    /// Builds the GUID-based file name for a copy of <paramref name="originalPath"/>,
    /// preserving the original extension. Exposed for tests.
    /// </summary>
    public static string BuildCopiedFileName(string originalPath)
    {
        var extension = Path.GetExtension(originalPath);
        return string.IsNullOrEmpty(extension)
            ? Guid.NewGuid().ToString("N")
            : $"{Guid.NewGuid():N}{extension}";
    }
}
