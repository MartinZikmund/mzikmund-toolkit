using System.Threading.Channels;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="IDeepLinkService"/>. Backed by an unbounded
/// <see cref="Channel{T}"/> so concurrent <see cref="Enqueue"/> /
/// <see cref="TryDequeueAsync"/> calls are safe without explicit locking.
/// </summary>
public sealed class DeepLinkService : IDeepLinkService
{
    private readonly Channel<DeepLinkRequest> _channel = Channel.CreateUnbounded<DeepLinkRequest>(
        new UnboundedChannelOptions
        {
            SingleReader = false,
            SingleWriter = false,
        });

    /// <inheritdoc />
    public event EventHandler<DeepLinkRequest>? RequestReceived;

    /// <inheritdoc />
    public void Enqueue(DeepLinkRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        _channel.Writer.TryWrite(request);
    }

    /// <inheritdoc />
    public async Task<DeepLinkRequest?> TryDequeueAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _channel.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public int Replay()
    {
        var count = 0;
        while (_channel.Reader.TryRead(out var request))
        {
            RequestReceived?.Invoke(this, request);
            count++;
        }
        return count;
    }
}
