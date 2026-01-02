namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the status of a language model.
/// </summary>
public enum ModelStatus
{
    /// <summary>
    /// Model status is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Model is available and ready to use.
    /// </summary>
    Available = 1,

    /// <summary>
    /// Model is currently unavailable.
    /// </summary>
    Unavailable = 2,

    /// <summary>
    /// Model is loading.
    /// </summary>
    Loading = 3,

    /// <summary>
    /// Model has encountered an error.
    /// </summary>
    Error = 4,

    /// <summary>
    /// Model is being updated.
    /// </summary>
    Updating = 5,

    /// <summary>
    /// Model is deprecated.
    /// </summary>
    Deprecated = 6
}

