using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.MCP;

/// <summary>
/// Defines the interface for Model Context Protocol (MCP) clients.
/// </summary>
public interface IMCPClient
{
    /// <summary>
    /// Analyzes an error using the MCP.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>The analysis.</returns>
    Task<ErrorAnalysis> AnalyzeAsync(Error error);

    /// <summary>
    /// Analyzes an error context using the MCP.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>The analysis.</returns>
    Task<ErrorAnalysis> AnalyzeContextAsync(ErrorContext context);
} 