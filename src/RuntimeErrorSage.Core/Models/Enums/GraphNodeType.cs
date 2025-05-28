namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the type of node in a dependency graph.
/// </summary>
public enum GraphNodeType
{
    /// <summary>
    /// Unknown node type.
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Service node type.
    /// </summary>
    Service = 1,
    
    /// <summary>
    /// Component node type.
    /// </summary>
    Component = 2,
    
    /// <summary>
    /// Resource node type.
    /// </summary>
    Resource = 3,
    
    /// <summary>
    /// Database node type.
    /// </summary>
    Database = 4,
    
    /// <summary>
    /// API node type.
    /// </summary>
    API = 5,
    
    /// <summary>
    /// Infrastructure node type.
    /// </summary>
    Infrastructure = 6
} 