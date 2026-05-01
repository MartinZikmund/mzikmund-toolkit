namespace MZikmund.Toolkit.WinUI.Data;

/// <summary>
/// Helpers for flipping <see cref="ISoftDeletable.IsDeleted"/> /
/// <see cref="ISoftDeletable.DeletedAt"/>.
/// </summary>
public static class SoftDeletableExtensions
{
    /// <summary>
    /// Sets <see cref="ISoftDeletable.IsDeleted"/> to <see langword="true"/> and stamps
    /// <see cref="ISoftDeletable.DeletedAt"/>.
    /// </summary>
    /// <param name="entity">Entity to mark as deleted.</param>
    /// <param name="at">Optional timestamp; defaults to <see cref="DateTime.UtcNow"/>.</param>
    public static void MarkDeleted(this ISoftDeletable entity, DateTime? at = null)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entity.IsDeleted = true;
        entity.DeletedAt = at ?? DateTime.UtcNow;
    }

    /// <summary>
    /// Restores a previously soft-deleted entity by clearing <see cref="ISoftDeletable.IsDeleted"/>
    /// and <see cref="ISoftDeletable.DeletedAt"/>.
    /// </summary>
    public static void Restore(this ISoftDeletable entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entity.IsDeleted = false;
        entity.DeletedAt = null;
    }
}
