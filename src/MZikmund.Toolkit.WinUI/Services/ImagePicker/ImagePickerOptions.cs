namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Configures <see cref="ImagePickerService"/>: where the picked file is copied
/// inside <c>LocalFolder</c> and which file extensions are allowed.
/// </summary>
public sealed class ImagePickerOptions
{
    /// <summary>
    /// Subfolder of <c>ApplicationData.Current.LocalFolder</c> the picked file is copied into.
    /// Created on demand. Defaults to <c>"Images"</c>.
    /// </summary>
    public string TargetSubfolder { get; init; } = "Images";

    /// <summary>
    /// Allowed file extensions, including the leading dot
    /// (e.g. <c>".jpg"</c>, <c>".png"</c>). Defaults to a small JPEG / PNG / GIF / WebP set.
    /// </summary>
    public IReadOnlyList<string> AllowedExtensions { get; init; } =
        [".jpg", ".jpeg", ".png", ".gif", ".webp"];
}
