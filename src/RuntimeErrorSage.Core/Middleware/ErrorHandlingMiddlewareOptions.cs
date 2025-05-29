using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Middleware;

/// <summary>
/// Configuration options for the error handling middleware.
/// </summary>
public class ErrorHandlingMiddlewareOptions
{
    /// <summary>
    /// Gets or sets whether error analysis is enabled.
    /// </summary>
    public bool EnableErrorAnalysis { get; } = true;

    /// <summary>
    /// Gets or sets whether automated remediation is enabled.
    /// </summary>
    public bool EnableAutomatedRemediation { get; } = true;

    /// <summary>
    /// Gets or sets whether request correlation is enabled.
    /// </summary>
    public bool EnableRequestCorrelation { get; } = true;

    /// <summary>
    /// Gets or sets whether performance monitoring is enabled.
    /// </summary>
    public bool EnablePerformanceMonitoring { get; } = true;

    /// <summary>
    /// Gets or sets the analysis timeout.
    /// </summary>
    public TimeSpan AnalysisTimeout { get; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets the remediation timeout.
    /// </summary>
    public TimeSpan RemediationTimeout { get; } = TimeSpan.FromMinutes(5);

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





