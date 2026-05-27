using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Data;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class AuditableEntityExtensionsTests
{
    [TestMethod]
    public void StampForCreate_NullEntity_Throws()
    {
        IAuditableEntity? entity = null;

        Assert.Throws<ArgumentNullException>(() => entity!.StampForCreate());
    }

    [TestMethod]
    public void StampForCreate_AssignsBothTimestamps()
    {
        var entity = new TestEntity();
        var at = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);

        entity.StampForCreate(at);

        Assert.AreEqual(at, entity.CreatedAt);
        Assert.AreEqual(at, entity.LastModifiedAt);
    }

    [TestMethod]
    public void StampForCreate_NoTimestamp_DefaultsToUtcNow()
    {
        var entity = new TestEntity();
        var before = DateTime.UtcNow;

        entity.StampForCreate();

        var after = DateTime.UtcNow;
        Assert.IsTrue(entity.CreatedAt >= before && entity.CreatedAt <= after);
        Assert.AreEqual(entity.CreatedAt, entity.LastModifiedAt);
    }

    [TestMethod]
    public void StampForUpdate_OnlyUpdatesLastModifiedAt()
    {
        var entity = new TestEntity
        {
            CreatedAt = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            LastModifiedAt = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        };
        var at = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc);

        entity.StampForUpdate(at);

        Assert.AreEqual(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), entity.CreatedAt);
        Assert.AreEqual(at, entity.LastModifiedAt);
    }

    private sealed class TestEntity : IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}

[TestClass]
public class SoftDeletableExtensionsTests
{
    [TestMethod]
    public void MarkDeleted_NullEntity_Throws()
    {
        ISoftDeletable? entity = null;

        Assert.Throws<ArgumentNullException>(() => entity!.MarkDeleted());
    }

    [TestMethod]
    public void Restore_NullEntity_Throws()
    {
        ISoftDeletable? entity = null;

        Assert.Throws<ArgumentNullException>(() => entity!.Restore());
    }

    [TestMethod]
    public void MarkDeleted_FlipsFlagAndStampsTimestamp()
    {
        var entity = new TestEntity();
        var at = new DateTime(2024, 5, 6, 7, 8, 9, DateTimeKind.Utc);

        entity.MarkDeleted(at);

        Assert.IsTrue(entity.IsDeleted);
        Assert.AreEqual(at, entity.DeletedAt);
    }

    [TestMethod]
    public void MarkDeleted_NoTimestamp_DefaultsToUtcNow()
    {
        var entity = new TestEntity();
        var before = DateTime.UtcNow;

        entity.MarkDeleted();

        var after = DateTime.UtcNow;
        Assert.IsTrue(entity.IsDeleted);
        Assert.IsNotNull(entity.DeletedAt);
        Assert.IsTrue(entity.DeletedAt >= before && entity.DeletedAt <= after);
    }

    [TestMethod]
    public void Restore_ClearsFlagAndTimestamp()
    {
        var entity = new TestEntity { IsDeleted = true, DeletedAt = DateTime.UtcNow };

        entity.Restore();

        Assert.IsFalse(entity.IsDeleted);
        Assert.IsNull(entity.DeletedAt);
    }

    private sealed class TestEntity : ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
