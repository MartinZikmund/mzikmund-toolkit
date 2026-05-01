using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Models;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class PaginatedResponseTests
{
    [TestMethod]
    public void TotalPages_ExactDivisor()
    {
        var response = new PaginatedResponse<int>([1, 2], Page: 1, PageSize: 10, TotalCount: 100);

        Assert.AreEqual(10, response.TotalPages);
    }

    [TestMethod]
    public void TotalPages_RoundsUpForRemainder()
    {
        var response = new PaginatedResponse<int>([], Page: 1, PageSize: 20, TotalCount: 137);

        Assert.AreEqual(7, response.TotalPages);
    }

    [TestMethod]
    public void TotalPages_ZeroTotalCount_IsZero()
    {
        var response = new PaginatedResponse<int>([], Page: 1, PageSize: 10, TotalCount: 0);

        Assert.AreEqual(0, response.TotalPages);
    }

    [TestMethod]
    public void TotalPages_ZeroPageSize_IsZero()
    {
        var response = new PaginatedResponse<int>([], Page: 1, PageSize: 0, TotalCount: 10);

        Assert.AreEqual(0, response.TotalPages);
    }

    [TestMethod]
    public void HasPreviousPage_OnFirstPage_IsFalse()
    {
        var response = new PaginatedResponse<int>([], Page: 1, PageSize: 10, TotalCount: 100);

        Assert.IsFalse(response.HasPreviousPage);
    }

    [TestMethod]
    public void HasPreviousPage_OnLaterPage_IsTrue()
    {
        var response = new PaginatedResponse<int>([], Page: 4, PageSize: 10, TotalCount: 100);

        Assert.IsTrue(response.HasPreviousPage);
    }

    [TestMethod]
    public void HasNextPage_OnLastPage_IsFalse()
    {
        var response = new PaginatedResponse<int>([], Page: 10, PageSize: 10, TotalCount: 100);

        Assert.IsFalse(response.HasNextPage);
    }

    [TestMethod]
    public void HasNextPage_OnEarlierPage_IsTrue()
    {
        var response = new PaginatedResponse<int>([], Page: 5, PageSize: 10, TotalCount: 100);

        Assert.IsTrue(response.HasNextPage);
    }

    [TestMethod]
    public void Empty_HasNoItems()
    {
        var empty = PaginatedResponse<string>.Empty();

        Assert.AreEqual(0, empty.Items.Length);
        Assert.AreEqual(0, empty.TotalCount);
        Assert.AreEqual(0, empty.TotalPages);
        Assert.IsFalse(empty.HasPreviousPage);
        Assert.IsFalse(empty.HasNextPage);
    }

    [TestMethod]
    public void Records_SameValues_AreEqual()
    {
        var a = new PaginatedResponse<int>([1, 2], Page: 1, PageSize: 10, TotalCount: 2);
        var b = a with { };

        // Reference equality of arrays is preserved by `with`, so these records compare equal.
        Assert.AreEqual(a, b);
    }
}
