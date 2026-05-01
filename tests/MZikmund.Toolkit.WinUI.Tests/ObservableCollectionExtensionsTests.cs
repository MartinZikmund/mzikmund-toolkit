using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Extensions;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class ObservableCollectionExtensionsTests
{
    [TestMethod]
    public void MergeWith_IdenticalSequence_DoesNotRaiseEvents()
    {
        var target = new ObservableCollection<string> { "A", "B", "C" };
        var events = Track(target);

        target.MergeWith(new[] { "A", "B", "C" });

        CollectionAssert.AreEqual(new[] { "A", "B", "C" }, target);
        Assert.AreEqual(0, events.Count, "no notifications expected when sequences already match");
    }

    [TestMethod]
    public void MergeWith_EmptyTarget_AddsAllItemsInOrder()
    {
        var target = new ObservableCollection<string>();

        target.MergeWith(new[] { "A", "B", "C" });

        CollectionAssert.AreEqual(new[] { "A", "B", "C" }, target);
    }

    [TestMethod]
    public void MergeWith_EmptySource_RemovesAllItems()
    {
        var target = new ObservableCollection<string> { "A", "B", "C" };
        var events = Track(target);

        target.MergeWith(Array.Empty<string>());

        Assert.AreEqual(0, target.Count);
        Assert.IsFalse(
            events.Any(e => e.Action == NotifyCollectionChangedAction.Reset),
            "Reset notifications must not be emitted; per-item Remove is required for animation friendliness.");
    }

    [TestMethod]
    public void MergeWith_AppendsNewItemsToTail()
    {
        var target = new ObservableCollection<string> { "A", "B" };
        var events = Track(target);

        target.MergeWith(new[] { "A", "B", "C", "D" });

        CollectionAssert.AreEqual(new[] { "A", "B", "C", "D" }, target);
        Assert.AreEqual(2, events.Count(e => e.Action == NotifyCollectionChangedAction.Add));
    }

    [TestMethod]
    public void MergeWith_RemovesMissingItemsFromMiddle()
    {
        var target = new ObservableCollection<string> { "A", "B", "C", "D" };
        var events = Track(target);

        target.MergeWith(new[] { "A", "C" });

        CollectionAssert.AreEqual(new[] { "A", "C" }, target);
        Assert.IsFalse(events.Any(e => e.Action == NotifyCollectionChangedAction.Reset));
    }

    [TestMethod]
    public void MergeWith_ReorderingUsesMoveNotification()
    {
        var target = new ObservableCollection<string> { "A", "B", "C" };
        var events = Track(target);

        target.MergeWith(new[] { "C", "A", "B" });

        CollectionAssert.AreEqual(new[] { "C", "A", "B" }, target);
        Assert.IsTrue(
            events.Any(e => e.Action == NotifyCollectionChangedAction.Move),
            "Reordering an existing item must emit a Move, not Remove+Add.");
    }

    [TestMethod]
    public void MergeWith_InsertsNewItemInMiddle()
    {
        var target = new ObservableCollection<string> { "A", "C" };

        target.MergeWith(new[] { "A", "B", "C" });

        CollectionAssert.AreEqual(new[] { "A", "B", "C" }, target);
    }

    [TestMethod]
    public void MergeWith_MixedInsertRemoveReorder_ProducesCorrectFinalSequence()
    {
        var target = new ObservableCollection<string> { "A", "B", "C", "D", "E" };

        target.MergeWith(new[] { "E", "B", "F", "A" });

        CollectionAssert.AreEqual(new[] { "E", "B", "F", "A" }, target);
    }

    [TestMethod]
    public void MergeWith_HandlesDuplicates()
    {
        var target = new ObservableCollection<string> { "A", "A", "B" };

        target.MergeWith(new[] { "A", "B" });

        CollectionAssert.AreEqual(new[] { "A", "B" }, target);
    }

    [TestMethod]
    public void MergeWith_UsesProvidedComparer()
    {
        var target = new ObservableCollection<string> { "alpha", "beta", "gamma" };

        target.MergeWith(new[] { "BETA", "ALPHA", "GAMMA" }, StringComparer.OrdinalIgnoreCase);

        CollectionAssert.AreEqual(new[] { "beta", "alpha", "gamma" }, target);
    }

    [TestMethod]
    public void MergeWith_PreservesExistingInstanceWhenComparerMatches()
    {
        var alice = new Person(1, "Alice");
        var bob = new Person(2, "Bob");
        var target = new ObservableCollection<Person> { alice, bob };

        var updatedAlice = new Person(1, "Alice (updated)");
        target.MergeWith(new[] { bob, updatedAlice }, new PersonByIdComparer());

        Assert.AreSame(bob, target[0]);
        Assert.AreSame(alice, target[1], "Move should keep the existing instance, not swap to the source instance.");
    }

    [TestMethod]
    public void MergeWith_AcceptsNonListEnumerable()
    {
        var target = new ObservableCollection<int> { 1, 2, 3 };

        target.MergeWith(EnumerateLazy());

        CollectionAssert.AreEqual(new[] { 4, 5, 6 }, target);

        static IEnumerable<int> EnumerateLazy()
        {
            yield return 4;
            yield return 5;
            yield return 6;
        }
    }

    [TestMethod]
    public void MergeWith_NullTarget_Throws()
    {
        ObservableCollection<int>? target = null;

        Assert.Throws<ArgumentNullException>(() => target!.MergeWith(new[] { 1 }));
    }

    [TestMethod]
    public void MergeWith_NullSource_Throws()
    {
        var target = new ObservableCollection<int> { 1 };

        Assert.Throws<ArgumentNullException>(() => target.MergeWith(null!));
    }

    private static List<NotifyCollectionChangedEventArgs> Track<T>(ObservableCollection<T> collection)
    {
        var events = new List<NotifyCollectionChangedEventArgs>();
        collection.CollectionChanged += (_, e) => events.Add(e);
        return events;
    }

    private sealed record Person(int Id, string Name);

    private sealed class PersonByIdComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person? x, Person? y) => x?.Id == y?.Id;

        public int GetHashCode(Person obj) => obj.Id.GetHashCode();
    }
}
