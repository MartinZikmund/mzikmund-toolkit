namespace MZikmund.Toolkit.WinUI.Data;

/// <summary>
/// Helpers for stamping audit fields on <see cref="IAuditableEntity"/> instances.
/// Apps wire these into their DbContext save pipeline (or any other persistence layer).
/// </summary>
public static class AuditableEntityExtensions
{
    /// <summary>
    /// Sets <see cref="IAuditableEntity.CreatedAt"/> and <see cref="IAuditableEntity.LastModifiedAt"/>
    /// to the same timestamp. Use on freshly-added entities.
    /// </summary>
    /// <param name="entity">Entity to stamp.</param>
    /// <param name="at">Optional timestamp; defaults to <see cref="DateTime.UtcNow"/>.</param>
    public static void StampForCreate(this IAuditableEntity entity, DateTime? at = null)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var now = at ?? DateTime.UtcNow;
        entity.CreatedAt = now;
        entity.LastModifiedAt = now;
    }

    /// <summary>
    /// Sets <see cref="IAuditableEntity.LastModifiedAt"/>. Use on entities being updated.
    /// </summary>
    /// <param name="entity">Entity to stamp.</param>
    /// <param name="at">Optional timestamp; defaults to <see cref="DateTime.UtcNow"/>.</param>
    public static void StampForUpdate(this IAuditableEntity entity, DateTime? at = null)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entity.LastModifiedAt = at ?? DateTime.UtcNow;
    }
}
