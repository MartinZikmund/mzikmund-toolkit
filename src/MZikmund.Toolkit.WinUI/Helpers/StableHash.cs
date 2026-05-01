using System.Text;

namespace MZikmund.Toolkit.WinUI.Helpers;

/// <summary>
/// Produces a stable <see cref="int"/> hash for a <see cref="Guid"/> or <see cref="string"/>
/// by XOR-ing 4-byte chunks of the value. Useful for platform APIs that demand integer IDs
/// (notification IDs, alarm IDs) when the natural domain key is a string or GUID.
/// </summary>
/// <remarks>
/// Stability is the only contract: the same input always yields the same output across runs
/// and processes. This is not a cryptographic hash and the int range is small enough that
/// collisions are expected — callers must tolerate them.
/// </remarks>
public static class StableHash
{
    /// <summary>
    /// Computes a stable 32-bit hash of <paramref name="value"/> by XOR-ing its 16 bytes
    /// in four 4-byte chunks.
    /// </summary>
    /// <param name="value">GUID to hash.</param>
    /// <returns>Stable integer hash.</returns>
    public static int FromGuid(Guid value)
    {
        Span<byte> bytes = stackalloc byte[16];
        value.TryWriteBytes(bytes);
        return BitConverter.ToInt32(bytes[..4])
             ^ BitConverter.ToInt32(bytes[4..8])
             ^ BitConverter.ToInt32(bytes[8..12])
             ^ BitConverter.ToInt32(bytes[12..16]);
    }

    /// <summary>
    /// Computes a stable 32-bit hash of <paramref name="value"/> by XOR-ing its UTF-8 bytes
    /// in 4-byte chunks. A trailing partial chunk is zero-padded before XOR.
    /// </summary>
    /// <param name="value">String to hash. <see langword="null"/> and empty strings both hash to 0.</param>
    /// <returns>Stable integer hash.</returns>
    public static int FromString(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return 0;
        }

        var bytes = Encoding.UTF8.GetBytes(value);
        var result = 0;
        var i = 0;
        while (i + 4 <= bytes.Length)
        {
            result ^= BitConverter.ToInt32(bytes, i);
            i += 4;
        }

        if (i < bytes.Length)
        {
            Span<byte> tail = stackalloc byte[4];
            bytes.AsSpan(i).CopyTo(tail);
            result ^= BitConverter.ToInt32(tail);
        }

        return result;
    }
}
