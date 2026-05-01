namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Cross-platform image picker. Shows the OS file picker, copies the user-selected
/// file into <c>ApplicationData.Current.LocalFolder</c> under a GUID-based name,
/// and returns the resulting <c>ms-appdata://</c> <see cref="Uri"/>.
/// </summary>
public interface IImagePickerService
{
    /// <summary>
    /// Shows the file picker and returns an <c>ms-appdata://</c> URI for the copy
    /// of the user-selected image, or <see langword="null"/> if the user cancelled.
    /// </summary>
    Task<Uri?> PickAsync();
}
