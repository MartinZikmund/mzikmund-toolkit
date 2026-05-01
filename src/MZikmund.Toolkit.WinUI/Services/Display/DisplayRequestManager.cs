using Windows.System.Display;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="IDisplayRequestManager"/> implementation. Holds a single
/// <see cref="DisplayRequest"/> while at least one acquisition is outstanding.
/// Tracks a generation token so a stale <see cref="Clear"/> from an older instance
/// can't accidentally undo a fresh acquisition.
/// </summary>
public sealed class DisplayRequestManager : IDisplayRequestManager
{
    private readonly object _lock = new();
    private DisplayRequest? _request;
    private int _activeCount;

    /// <inheritdoc />
    public int ActiveCount
    {
        get
        {
            lock (_lock)
            {
                return _activeCount;
            }
        }
    }

    /// <inheritdoc />
    public void Acquire()
    {
        lock (_lock)
        {
            if (_activeCount == 0)
            {
                _request = new DisplayRequest();
                _request.RequestActive();
            }

            _activeCount++;
        }
    }

    /// <inheritdoc />
    public void Release()
    {
        lock (_lock)
        {
            if (_activeCount == 0)
            {
                return;
            }

            _activeCount--;
            if (_activeCount == 0 && _request is not null)
            {
                _request.RequestRelease();
                _request = null;
            }
        }
    }

    /// <inheritdoc />
    public void Clear()
    {
        lock (_lock)
        {
            if (_activeCount > 0 && _request is not null)
            {
                _request.RequestRelease();
            }

            _request = null;
            _activeCount = 0;
        }
    }
}
