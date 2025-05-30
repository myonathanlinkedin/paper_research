namespace RuntimeErrorSage.Domain.Models.Remediation;

/// <summary>
/// Represents an edge in a dependency graph.
/// </summary>
public class GraphEdge
{
    /// <summary>
    /// Gets or sets the source node ID.
    /// </summary>
    public string Source { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the target node ID.
    /// </summary>
    public string Target { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the edge type.
    /// </summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the edge weight.
    /// </summary>
    public double Weight { get; set; }
    
    /// <summary>
    /// Gets or sets the edge metadata.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the edge label.
    /// </summary>
    public string Label { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the edge color.
    /// </summary>
    public string Color { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the edge style.
    /// </summary>
    public string Style { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the edge thickness.
    /// </summary>
    public double Thickness { get; set; }
    
    /// <summary>
    /// Gets or sets whether the edge is directed.
    /// </summary>
    public bool IsDirected { get; set; }
    
    /// <summary>
    /// Gets or sets whether the edge is bidirectional.
    /// </summary>
    public bool IsBidirectional { get; set; }
} 
