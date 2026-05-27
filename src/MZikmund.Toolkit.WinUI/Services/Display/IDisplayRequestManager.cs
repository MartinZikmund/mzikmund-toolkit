namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Reference-counted "keep the display awake" wrapper over
/// <see cref="Windows.System.Display.DisplayRequest"/>. Callers acquire and release in
/// pairs; the underlying request is held while at least one acquisition is outstanding.
/// </summary>
public interface IDisplayRequestManager
{
    /// <summary>Number of active acquisitions.</summary>
    int ActiveCount { get; }

    /// <summary>Acquires the keep-awake lock. Idempotent in pairs with <see cref="Release"/>.</summary>
    void Acquire();

    /// <summary>Releases one acquisition. No-op if no acquisitions are active.</summary>
    void Release();

    /// <summary>Drops every outstanding acquisition. Useful from app suspension hooks.</summary>
    void Clear();
}
