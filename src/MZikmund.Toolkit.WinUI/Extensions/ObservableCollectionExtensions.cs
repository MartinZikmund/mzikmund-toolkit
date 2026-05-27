using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MZikmund.Toolkit.WinUI.Extensions;

/// <summary>
/// Extensions for <see cref="ObservableCollection{T}"/>.
/// </summary>
public static class ObservableCollectionExtensions
{
    /// <summary>
    /// Synchronizes the contents of <paramref name="target"/> with <paramref name="source"/>
    /// using minimal insert, remove and <see cref="ObservableCollection{T}.Move(int, int)"/>
    /// operations, preserving the order of <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="target">Observable collection to mutate in place.</param>
    /// <param name="source">Desired final sequence of items.</param>
    /// <param name="comparer">Equality comparer used to match items between collections. Defaults to <see cref="EqualityComparer{T}.Default"/>.</param>
    /// <remarks>
    /// Avoids the visual flicker of <c>Clear</c> + <c>Add</c> patterns by emitting per-item
    /// change notifications, which lets bound list controls animate item-level changes.
    /// </remarks>
    public static void MergeWith<T>(this ObservableCollection<T> target, IEnumerable<T> source, IEqualityComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(source);

        comparer ??= EqualityComparer<T>.Default;
        var sourceList = source as IList<T> ?? source.ToList();

        for (var i = 0; i < sourceList.Count; i++)
        {
            var sourceItem = sourceList[i];

            if (i >= target.Count)
            {
                target.Add(sourceItem);
                continue;
            }

            if (comparer.Equals(target[i], sourceItem))
            {
                continue;
            }

            var foundAt = -1;
            for (var j = i + 1; j < target.Count; j++)
            {
                if (comparer.Equals(target[j], sourceItem))
                {
                    foundAt = j;
                    break;
                }
            }

            if (foundAt >= 0)
            {
                target.Move(foundAt, i);
            }
            else
            {
                target.Insert(i, sourceItem);
            }
        }

        while (target.Count > sourceList.Count)
        {
            target.RemoveAt(target.Count - 1);
        }
    }
}
