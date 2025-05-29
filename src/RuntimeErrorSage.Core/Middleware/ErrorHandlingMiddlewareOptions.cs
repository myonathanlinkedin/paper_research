using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Model.Middleware;

/// <summary>
/// Configuration options for the error handling middleware.
/// </summary>
public class ErrorHandlingMiddlewareOptions
{
    /// <summary>
    /// Gets or sets whether error analysis is enabled.
    /// </summary>
    public bool EnableErrorAnalysis { get; set; } = true;

    /// <summary>
    /// Gets or sets whether automated remediation is enabled.
    /// </summary>
    public bool EnableAutomatedRemediation { get; set; } = true;

    /// <summary>
    /// Gets or sets whether request correlation is enabled.
    /// </summary>
    public bool EnableRequestCorrelation { get; set; } = true;

    /// <summary>
    /// Gets or sets whether performance monitoring is enabled.
    /// </summary>
    public bool EnablePerformanceMonitoring { get; set; } = true;

    /// <summary>
    /// Gets or sets the analysis timeout.
    /// </summary>
    public TimeSpan AnalysisTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets the remediation timeout.
    /// </summary>
    public TimeSpan RemediationTimeout { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Gets or sets the excluded paths.
    /// </summary>
    public Dictionary<string, string[]> ExcludedPaths { get; set; } = new()
    {
        ["/health"] = new[] { "GET" },
        ["/metrics"] = new[] { "GET" },
        ["/swagger"] = new[] { "GET" }
    };
} 
