namespace MZikmund.Toolkit.WinUI.Data;

/// <summary>
/// Marks an entity that records when it was created and last modified.
/// </summary>
/// <remarks>
/// Apps using EF Core typically stamp these in a <c>DbContext.SaveChangesAsync</c>
/// override:
/// <code>
/// public override async Task&lt;int&gt; SaveChangesAsync(CancellationToken ct = default)
/// {
///     var now = DateTime.UtcNow;
///     foreach (var entry in ChangeTracker.Entries&lt;IAuditableEntity&gt;())
///     {
///         if (entry.State == EntityState.Added) entry.Entity.StampForCreate(now);
///         else if (entry.State == EntityState.Modified) entry.Entity.StampForUpdate(now);
///     }
///     return await base.SaveChangesAsync(ct);
/// }
/// </code>
/// The toolkit ships only the marker interface and the
/// <see cref="AuditableEntityExtensions"/> stamping helpers — the toolkit itself
/// doesn't take an EF Core dependency.
/// </remarks>
public interface IAuditableEntity
{
    /// <summary>UTC timestamp recorded when the entity was first persisted.</summary>
    DateTime CreatedAt { get; set; }

    /// <summary>UTC timestamp recorded on the most recent update.</summary>
    DateTime LastModifiedAt { get; set; }
}
