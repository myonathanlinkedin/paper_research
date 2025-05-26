using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Interfaces.Storage;

/// <summary>
/// Interface for pattern storage operations.
/// </summary>
public interface IPatternStorage
{
    /// <summary>
    /// Gets error patterns from storage.
    /// </summary>
    /// <returns>List of error patterns.</returns>
    Task<List<ErrorPattern>> GetPatternsAsync();

    /// <summary>
    /// Saves error patterns to storage.
    /// </summary>
    /// <param name="patterns">The patterns to save.</param>
    Task SavePatternsAsync(List<ErrorPattern> patterns);

    /// <summary>
    /// Gets the connection status of the storage.
    /// </summary>
    /// <returns>The connection status.</returns>
    Task<ConnectionStatus> GetConnectionStatusAsync();
} 