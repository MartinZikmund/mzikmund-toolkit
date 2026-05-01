namespace MZikmund.Toolkit.WinUI.Data;

/// <summary>
/// Marks an entity that supports soft-delete semantics — <see cref="IsDeleted"/>
/// flips to <see langword="true"/> on a "delete" instead of removing the row,
/// and <see cref="DeletedAt"/> records when.
/// </summary>
/// <remarks>
/// Apps using EF Core typically apply a global query filter and intercept
/// <c>Remove</c> in their DbContext:
/// <code>
/// // In OnModelCreating:
/// foreach (var et in modelBuilder.Model.GetEntityTypes())
/// {
///     if (typeof(ISoftDeletable).IsAssignableFrom(et.ClrType))
///     {
///         var param = Expression.Parameter(et.ClrType, "e");
///         var body = Expression.Equal(
///             Expression.Property(param, nameof(ISoftDeletable.IsDeleted)),
///             Expression.Constant(false));
///         modelBuilder.Entity(et.ClrType).HasQueryFilter(Expression.Lambda(body, param));
///     }
/// }
/// // In SaveChangesAsync override, swap Deleted state to Modified after marking:
/// foreach (var entry in ChangeTracker.Entries&lt;ISoftDeletable&gt;().Where(e =&gt; e.State == EntityState.Deleted))
/// {
///     entry.Entity.MarkDeleted();
///     entry.State = EntityState.Modified;
/// }
/// </code>
/// </remarks>
public interface ISoftDeletable
{
    /// <summary><see langword="true"/> when the entity is logically deleted.</summary>
    bool IsDeleted { get; set; }

    /// <summary>UTC timestamp recorded when the entity was soft-deleted; <see langword="null"/> while live.</summary>
    DateTime? DeletedAt { get; set; }
}
