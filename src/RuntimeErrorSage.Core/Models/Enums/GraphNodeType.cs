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
    /// Error node type.
    /// </summary>
    Error = 3,
    
    /// <summary>
    /// Dependency node type.
    /// </summary>
    Dependency = 4,
    
    /// <summary>
    /// Resource node type.
    /// </summary>
    Resource = 5,
    
    /// <summary>
    /// Database node type.
    /// </summary>
    Database = 6,
    
    /// <summary>
    /// API node type.
    /// </summary>
    API = 7,
    
    /// <summary>
    /// Infrastructure node type.
    /// </summary>
    Infrastructure = 8
} 
