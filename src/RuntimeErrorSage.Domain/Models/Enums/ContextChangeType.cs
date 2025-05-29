namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Represents the type of change in a context.
/// </summary>
public enum ContextChangeType
{
    /// <summary>
    /// No change.
    /// </summary>
    None,

    /// <summary>
    /// Context was created.
    /// </summary>
    Created,

    /// <summary>
    /// Context was updated.
    /// </summary>
    Updated,

    /// <summary>
    /// Context was deleted.
    /// </summary>
    Deleted,

    /// <summary>
    /// Context was modified.
    /// </summary>
    Modified,

    /// <summary>
    /// Context was restored.
    /// </summary>
    Restored,

    /// <summary>
    /// Context was archived.
    /// </summary>
    Archived,

    /// <summary>
    /// Context was merged.
    /// </summary>
    Merged,

    /// <summary>
    /// Context was split.
    /// </summary>
    Split
} 
