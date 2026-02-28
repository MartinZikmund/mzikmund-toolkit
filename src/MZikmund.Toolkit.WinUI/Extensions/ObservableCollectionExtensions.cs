using System.Collections.ObjectModel;

namespace MZikmund.Toolkit.WinUI.Extensions;

/// <summary>
/// Extension methods for <see cref="ObservableCollection{T}"/>.
/// </summary>
public static class ObservableCollectionExtensions
{
	/// <summary>
	/// Merges the target collection with a source list, minimizing UI notifications
	/// by removing, inserting, and moving items rather than clearing and re-adding.
	/// </summary>
	/// <typeparam name="T">The element type.</typeparam>
	/// <param name="target">The target observable collection to update.</param>
	/// <param name="source">The source list representing the desired state.</param>
	/// <param name="equalityComparer">A function that determines whether two items are equal.</param>
	public static void MergeWith<T>(this ObservableCollection<T> target, IList<T> source, Func<T, T, bool> equalityComparer)
	{
		// 1. Remove any items from target that aren't in source.
		for (int i = target.Count - 1; i >= 0; i--)
		{
			if (!source.Any(s => equalityComparer(s, target[i])))
			{
				target.RemoveAt(i);
			}
		}

		// 2. Ensure items are present and in the same order as source.
		for (int i = 0; i < source.Count; i++)
		{
			var sourceItem = source[i];

			if (i >= target.Count)
			{
				// If target is shorter, simply add the item.
				target.Add(sourceItem);
			}
			else if (!equalityComparer(sourceItem, target[i]))
			{
				// Try to find the item elsewhere in target.
				int existingIndex = -1;
				for (int j = i + 1; j < target.Count; j++)
				{
					if (equalityComparer(sourceItem, target[j]))
					{
						existingIndex = j;
						break;
					}
				}

				if (existingIndex >= 0)
				{
					// Move the item to the correct position.
					target.Move(existingIndex, i);
				}
				else
				{
					// Item doesn't exist in target, so insert it.
					target.Insert(i, sourceItem);
				}
			}
		}
	}
}
