using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Services;

namespace RuntimeErrorSage.Tests.TestSuite
{
    /// <summary>
    /// Implements the standardized error scenarios required by the research paper.
    /// Contains 100 test scenarios across the four required error types.
    /// </summary>
    public class StandardizedErrorScenarios
    {
        private readonly IRuntimeErrorSageService _RuntimeErrorSageService;
        private readonly Dictionary<string, List<ErrorScenario>> _scenarios;

        public StandardizedErrorScenarios(IRuntimeErrorSageService RuntimeErrorSageService)
        {
            _RuntimeErrorSageService = RuntimeErrorSageService;
            _scenarios = InitializeScenarios();
        }

        /// <summary>
        /// Initializes all 100 standardized error scenarios as required by the research paper.
        /// </summary>
        private Dictionary<string, List<ErrorScenario>> InitializeScenarios()
        {
            return new Dictionary<string, List<ErrorScenario>>
            {
                ["Database"] = new List<ErrorScenario>
                {
                    // Connection failures (6 scenarios)
                    new ErrorScenario("DB_CONN_001", "Connection string invalid", 
                        () => throw new System.Data.SqlClient.SqlException("Invalid connection string")),
                    new ErrorScenario("DB_CONN_002", "Server unreachable", 
                        () => throw new System.Data.SqlClient.SqlException("Network-related error")),
                    new ErrorScenario("DB_CONN_003", "Authentication failed", 
                        () => throw new System.Data.SqlClient.SqlException("Login failed")),
                    new ErrorScenario("DB_CONN_004", "Database not found", 
                        () => throw new System.Data.SqlClient.SqlException("Database does not exist")),
                    new ErrorScenario("DB_CONN_005", "Connection pool exhausted", 
                        () => throw new System.Data.SqlClient.SqlException("Connection pool limit reached")),
                    new ErrorScenario("DB_CONN_006", "Connection timeout", 
                        () => throw new System.Data.SqlClient.SqlException("Connection timed out")),

                    // Query timeouts (6 scenarios)
                    new ErrorScenario("DB_QUERY_001", "Query execution timeout", 
                        () => throw new System.Data.SqlClient.SqlException("Query timeout expired")),
                    new ErrorScenario("DB_QUERY_002", "Command timeout", 
                        () => throw new System.Data.SqlClient.SqlException("Command timeout expired")),
                    new ErrorScenario("DB_QUERY_003", "Transaction timeout", 
                        () => throw new System.Data.SqlClient.SqlException("Transaction timeout")),
                    new ErrorScenario("DB_QUERY_004", "Lock timeout", 
                        () => throw new System.Data.SqlClient.SqlException("Lock request timeout")),
                    new ErrorScenario("DB_QUERY_005", "Resource timeout", 
                        () => throw new System.Data.SqlClient.SqlException("Resource timeout")),
                    new ErrorScenario("DB_QUERY_006", "Network timeout", 
                        () => throw new System.Data.SqlClient.SqlException("Network timeout")),

                    // Deadlocks (6 scenarios)
                    new ErrorScenario("DB_DEAD_001", "Transaction deadlock", 
                        () => throw new System.Data.SqlClient.SqlException("Transaction deadlock")),
                    new ErrorScenario("DB_DEAD_002", "Lock deadlock", 
                        () => throw new System.Data.SqlClient.SqlException("Lock deadlock")),
                    new ErrorScenario("DB_DEAD_003", "Resource deadlock", 
                        () => throw new System.Data.SqlClient.SqlException("Resource deadlock")),
                    new ErrorScenario("DB_DEAD_004", "Distributed deadlock", 
                        () => throw new System.Data.SqlClient.SqlException("Distributed deadlock")),
                    new ErrorScenario("DB_DEAD_005", "Application deadlock", 
                        () => throw new System.Data.SqlClient.SqlException("Application deadlock")),
                    new ErrorScenario("DB_DEAD_006", "Cross-database deadlock", 
                        () => throw new System.Data.SqlClient.SqlException("Cross-database deadlock")),

                    // Constraint violations (7 scenarios)
                    new ErrorScenario("DB_CONS_001", "Primary key violation", 
                        () => throw new System.Data.SqlClient.SqlException("Violation of PRIMARY KEY constraint")),
                    new ErrorScenario("DB_CONS_002", "Unique constraint violation", 
                        () => throw new System.Data.SqlClient.SqlException("Violation of UNIQUE constraint")),
                    new ErrorScenario("DB_CONS_003", "Foreign key violation", 
                        () => throw new System.Data.SqlClient.SqlException("Violation of FOREIGN KEY constraint")),
                    new ErrorScenario("DB_CONS_004", "Check constraint violation", 
                        () => throw new System.Data.SqlClient.SqlException("Violation of CHECK constraint")),
                    new ErrorScenario("DB_CONS_005", "Not null violation", 
                        () => throw new System.Data.SqlClient.SqlException("Cannot insert NULL")),
                    new ErrorScenario("DB_CONS_006", "Data type violation", 
                        () => throw new System.Data.SqlClient.SqlException("Data type mismatch")),
                    new ErrorScenario("DB_CONS_007", "Default constraint violation", 
                        () => throw new System.Data.SqlClient.SqlException("Violation of DEFAULT constraint"))
                },

                ["FileSystem"] = new List<ErrorScenario>
                {
                    // Permission issues (7 scenarios)
                    new ErrorScenario("FS_PERM_001", "Access denied", 
                        () => throw new System.UnauthorizedAccessException("Access to the path is denied")),
                    new ErrorScenario("FS_PERM_002", "Insufficient permissions", 
                        () => throw new System.Security.SecurityException("Insufficient permissions")),
                    new ErrorScenario("FS_PERM_003", "File in use", 
                        () => throw new System.IO.IOException("The process cannot access the file")),
                    new ErrorScenario("FS_PERM_004", "Directory access denied", 
                        () => throw new System.UnauthorizedAccessException("Access to the directory is denied")),
                    new ErrorScenario("FS_PERM_005", "Read-only file", 
                        () => throw new System.IO.IOException("Cannot write to read-only file")),
                    new ErrorScenario("FS_PERM_006", "Network share access denied", 
                        () => throw new System.UnauthorizedAccessException("Network share access denied")),
                    new ErrorScenario("FS_PERM_007", "File system permissions", 
                        () => throw new System.Security.SecurityException("File system permissions error")),

                    // Disk space errors (6 scenarios)
                    new ErrorScenario("FS_DISK_001", "Disk full", 
                        () => throw new System.IO.IOException("There is not enough space on the disk")),
                    new ErrorScenario("FS_DISK_002", "Quota exceeded", 
                        () => throw new System.IO.IOException("Disk quota exceeded")),
                    new ErrorScenario("FS_DISK_003", "Volume full", 
                        () => throw new System.IO.IOException("The volume is full")),
                    new ErrorScenario("FS_DISK_004", "Disk space low", 
                        () => throw new System.IO.IOException("Low disk space warning")),
                    new ErrorScenario("FS_DISK_005", "Disk write protected", 
                        () => throw new System.IO.IOException("The disk is write protected")),
                    new ErrorScenario("FS_DISK_006", "Disk not ready", 
                        () => throw new System.IO.IOException("The disk is not ready")),

                    // File locking (6 scenarios)
                    new ErrorScenario("FS_LOCK_001", "File locked", 
                        () => throw new System.IO.IOException("The file is locked")),
                    new ErrorScenario("FS_LOCK_002", "File in use by another process", 
                        () => throw new System.IO.IOException("File in use by another process")),
                    new ErrorScenario("FS_LOCK_003", "File sharing violation", 
                        () => throw new System.IO.IOException("File sharing violation")),
                    new ErrorScenario("FS_LOCK_004", "File locked for reading", 
                        () => throw new System.IO.IOException("File locked for reading")),
                    new ErrorScenario("FS_LOCK_005", "File locked for writing", 
                        () => throw new System.IO.IOException("File locked for writing")),
                    new ErrorScenario("FS_LOCK_006", "File locked for deletion", 
                        () => throw new System.IO.IOException("File locked for deletion")),

                    // Path resolution (6 scenarios)
                    new ErrorScenario("FS_PATH_001", "Path too long", 
                        () => throw new System.IO.PathTooLongException("The path is too long")),
                    new ErrorScenario("FS_PATH_002", "Invalid path", 
                        () => throw new System.IO.IOException("Invalid path")),
                    new ErrorScenario("FS_PATH_003", "Path not found", 
                        () => throw new System.IO.DirectoryNotFoundException("Path not found")),
                    new ErrorScenario("FS_PATH_004", "Invalid characters in path", 
                        () => throw new System.IO.IOException("Invalid characters in path")),
                    new ErrorScenario("FS_PATH_005", "Network path not found", 
                        () => throw new System.IO.DirectoryNotFoundException("Network path not found")),
                    new ErrorScenario("FS_PATH_006", "Path format not supported", 
                        () => throw new System.IO.IOException("Path format not supported"))
                },

                ["HttpClient"] = new List<ErrorScenario>
                {
                    // Connection timeouts (7 scenarios)
                    new ErrorScenario("HTTP_CONN_001", "Connection timeout", 
                        () => throw new System.Net.WebException("The operation has timed out")),
                    new ErrorScenario("HTTP_CONN_002", "Request timeout", 
                        () => throw new System.Net.WebException("Request timed out")),
                    new ErrorScenario("HTTP_CONN_003", "DNS resolution timeout", 
                        () => throw new System.Net.WebException("DNS resolution timeout")),
                    new ErrorScenario("HTTP_CONN_004", "Proxy timeout", 
                        () => throw new System.Net.WebException("Proxy timeout")),
                    new ErrorScenario("HTTP_CONN_005", "Keep-alive timeout", 
                        () => throw new System.Net.WebException("Keep-alive timeout")),
                    new ErrorScenario("HTTP_CONN_006", "Server timeout", 
                        () => throw new System.Net.WebException("Server timeout")),
                    new ErrorScenario("HTTP_CONN_007", "Gateway timeout", 
                        () => throw new System.Net.WebException("Gateway timeout")),

                    // SSL/TLS errors (6 scenarios)
                    new ErrorScenario("HTTP_SSL_001", "SSL certificate invalid", 
                        () => throw new System.Net.WebException("SSL certificate invalid")),
                    new ErrorScenario("HTTP_SSL_002", "SSL handshake failed", 
                        () => throw new System.Net.WebException("SSL handshake failed")),
                    new ErrorScenario("HTTP_SSL_003", "SSL protocol error", 
                        () => throw new System.Net.WebException("SSL protocol error")),
                    new ErrorScenario("HTTP_SSL_004", "SSL certificate expired", 
                        () => throw new System.Net.WebException("SSL certificate expired")),
                    new ErrorScenario("HTTP_SSL_005", "SSL certificate not trusted", 
                        () => throw new System.Net.WebException("SSL certificate not trusted")),
                    new ErrorScenario("HTTP_SSL_006", "SSL certificate revoked", 
                        () => throw new System.Net.WebException("SSL certificate revoked")),

                    // Rate limiting (6 scenarios)
                    new ErrorScenario("HTTP_RATE_001", "Too many requests", 
                        () => throw new System.Net.WebException("429 Too Many Requests")),
                    new ErrorScenario("HTTP_RATE_002", "Rate limit exceeded", 
                        () => throw new System.Net.WebException("Rate limit exceeded")),
                    new ErrorScenario("HTTP_RATE_003", "Quota exceeded", 
                        () => throw new System.Net.WebException("Quota exceeded")),
                    new ErrorScenario("HTTP_RATE_004", "Throttling", 
                        () => throw new System.Net.WebException("Throttling")),
                    new ErrorScenario("HTTP_RATE_005", "Request limit exceeded", 
                        () => throw new System.Net.WebException("Request limit exceeded")),
                    new ErrorScenario("HTTP_RATE_006", "Bandwidth limit exceeded", 
                        () => throw new System.Net.WebException("Bandwidth limit exceeded")),

                    // Service unavailability (6 scenarios)
                    new ErrorScenario("HTTP_SVC_001", "Service unavailable", 
                        () => throw new System.Net.WebException("503 Service Unavailable")),
                    new ErrorScenario("HTTP_SVC_002", "Bad gateway", 
                        () => throw new System.Net.WebException("502 Bad Gateway")),
                    new ErrorScenario("HTTP_SVC_003", "Gateway timeout", 
                        () => throw new System.Net.WebException("504 Gateway Timeout")),
                    new ErrorScenario("HTTP_SVC_004", "Internal server error", 
                        () => throw new System.Net.WebException("500 Internal Server Error")),
                    new ErrorScenario("HTTP_SVC_005", "Service overloaded", 
                        () => throw new System.Net.WebException("Service overloaded")),
                    new ErrorScenario("HTTP_SVC_006", "Service maintenance", 
                        () => throw new System.Net.WebException("Service maintenance"))
                },

                ["Resource"] = new List<ErrorScenario>
                {
                    // Memory allocation (7 scenarios)
                    new ErrorScenario("RES_MEM_001", "Out of memory", 
                        () => throw new System.OutOfMemoryException("Out of memory")),
                    new ErrorScenario("RES_MEM_002", "Memory allocation failed", 
                        () => throw new System.OutOfMemoryException("Memory allocation failed")),
                    new ErrorScenario("RES_MEM_003", "Memory pressure", 
                        () => throw new System.OutOfMemoryException("Memory pressure")),
                    new ErrorScenario("RES_MEM_004", "Large object heap", 
                        () => throw new System.OutOfMemoryException("Large object heap")),
                    new ErrorScenario("RES_MEM_005", "Memory fragmentation", 
                        () => throw new System.OutOfMemoryException("Memory fragmentation")),
                    new ErrorScenario("RES_MEM_006", "Memory leak", 
                        () => throw new System.OutOfMemoryException("Memory leak")),
                    new ErrorScenario("RES_MEM_007", "Memory limit exceeded", 
                        () => throw new System.OutOfMemoryException("Memory limit exceeded")),

                    // Thread pool exhaustion (6 scenarios)
                    new ErrorScenario("RES_THREAD_001", "Thread pool exhausted", 
                        () => throw new System.Threading.ThreadPoolException("Thread pool exhausted")),
                    new ErrorScenario("RES_THREAD_002", "Thread starvation", 
                        () => throw new System.Threading.ThreadPoolException("Thread starvation")),
                    new ErrorScenario("RES_THREAD_003", "Thread limit exceeded", 
                        () => throw new System.Threading.ThreadPoolException("Thread limit exceeded")),
                    new ErrorScenario("RES_THREAD_004", "Thread creation failed", 
                        () => throw new System.Threading.ThreadPoolException("Thread creation failed")),
                    new ErrorScenario("RES_THREAD_005", "Thread pool full", 
                        () => throw new System.Threading.ThreadPoolException("Thread pool full")),
                    new ErrorScenario("RES_THREAD_006", "Thread pool timeout", 
                        () => throw new System.Threading.ThreadPoolException("Thread pool timeout")),

                    // Socket limits (6 scenarios)
                    new ErrorScenario("RES_SOCK_001", "Socket limit exceeded", 
                        () => throw new System.Net.Sockets.SocketException("Socket limit exceeded")),
                    new ErrorScenario("RES_SOCK_002", "Socket exhaustion", 
                        () => throw new System.Net.Sockets.SocketException("Socket exhaustion")),
                    new ErrorScenario("RES_SOCK_003", "Socket creation failed", 
                        () => throw new System.Net.Sockets.SocketException("Socket creation failed")),
                    new ErrorScenario("RES_SOCK_004", "Socket pool exhausted", 
                        () => throw new System.Net.Sockets.SocketException("Socket pool exhausted")),
                    new ErrorScenario("RES_SOCK_005", "Socket timeout", 
                        () => throw new System.Net.Sockets.SocketException("Socket timeout")),
                    new ErrorScenario("RES_SOCK_006", "Socket buffer full", 
                        () => throw new System.Net.Sockets.SocketException("Socket buffer full")),

                    // Process limits (6 scenarios)
                    new ErrorScenario("RES_PROC_001", "Process limit exceeded", 
                        () => throw new System.ComponentModel.Win32Exception("Process limit exceeded")),
                    new ErrorScenario("RES_PROC_002", "Process creation failed", 
                        () => throw new System.ComponentModel.Win32Exception("Process creation failed")),
                    new ErrorScenario("RES_PROC_003", "Process handle limit", 
                        () => throw new System.ComponentModel.Win32Exception("Process handle limit")),
                    new ErrorScenario("RES_PROC_004", "Process memory limit", 
                        () => throw new System.ComponentModel.Win32Exception("Process memory limit")),
                    new ErrorScenario("RES_PROC_005", "Process CPU limit", 
                        () => throw new System.ComponentModel.Win32Exception("Process CPU limit")),
                    new ErrorScenario("RES_PROC_006", "Process I/O limit", 
                        () => throw new System.ComponentModel.Win32Exception("Process I/O limit"))
                }
            };
        }

        /// <summary>
        /// Executes all standardized error scenarios and validates the results against research requirements.
        /// </summary>
        [Fact]
        public async Task ExecuteAllScenarios()
        {
            var results = new List<ErrorAnalysisResult>();
            var performanceMetrics = new List<PerformanceMetrics>();

            foreach (var errorType in _scenarios.Keys)
            {
                foreach (var scenario in _scenarios[errorType])
                {
                    try
                    {
                        // Execute the scenario
                        var result = await ExecuteScenario(scenario);
                        results.Add(result);
                        performanceMetrics.Add(result.PerformanceMetrics);

                        // Validate against research requirements
                        Assert.True(result.MeetsAccuracyRequirements, 
                            $"Scenario {scenario.Id} failed accuracy requirements");
                        Assert.True(result.MeetsPerformanceRequirements, 
                            $"Scenario {scenario.Id} failed performance requirements");
                        Assert.Equal(errorType, result.ErrorType);
                    }
                    catch (Exception ex)
                    {
                        Assert.True(false, $"Scenario {scenario.Id} failed: {ex.Message}");
                    }
                }
            }

            // Validate overall results against research requirements
            ValidateOverallResults(results, performanceMetrics);
        }

        /// <summary>
        /// Executes a single error scenario and returns the analysis result.
        /// </summary>
        private async Task<ErrorAnalysisResult> ExecuteScenario(ErrorScenario scenario)
        {
            try
            {
                // Execute the scenario to generate the error
                scenario.Execute();

                // This line should not be reached as the scenario should throw an exception
                throw new InvalidOperationException("Scenario did not throw expected exception");
            }
            catch (Exception ex)
            {
                // Create error context
                var context = new ErrorContext
                {
                    ErrorType = scenario.ErrorType,
                    TestScenarioId = scenario.Id,
                    Timestamp = DateTime.UtcNow,
                    CorrelationId = Guid.NewGuid().ToString()
                };

                // Process the error through RuntimeErrorSage
                return await _RuntimeErrorSageService.ProcessExceptionAsync(ex, context);
            }
        }

        /// <summary>
        /// Validates overall results against research requirements.
        /// </summary>
        private void ValidateOverallResults(List<ErrorAnalysisResult> results, List<PerformanceMetrics> metrics)
        {
            // Validate accuracy requirements (80% root cause, 70% remediation)
            var rootCauseAccuracy = results.Average(r => r.RootCauseConfidence);
            var remediationAccuracy = results.Average(r => r.RemediationConfidence);

            Assert.True(rootCauseAccuracy >= 0.8, 
                $"Root cause accuracy {rootCauseAccuracy:P2} below required 80%");
            Assert.True(remediationAccuracy >= 0.7, 
                $"Remediation accuracy {remediationAccuracy:P2} below required 70%");

            // Validate performance requirements
            var latencyPercentile95 = metrics
                .Select(m => m.TotalProcessingTime.TotalMilliseconds)
                .OrderByDescending(ms => ms)
                .Skip((int)(metrics.Count * 0.05))
                .First();

            var maxMemoryUsage = metrics.Max(m => m.PhaseResourceUsage.Values.Max(r => r.MemoryUsageMB));
            var maxCpuUsage = metrics.Max(m => m.PhaseResourceUsage.Values.Max(r => r.CPUUsagePercent));

            Assert.True(latencyPercentile95 <= 500, 
                $"95th percentile latency {latencyPercentile95:F2}ms exceeds 500ms limit");
            Assert.True(maxMemoryUsage <= 100, 
                $"Maximum memory usage {maxMemoryUsage}MB exceeds 100MB limit");
            Assert.True(maxCpuUsage <= 10.0, 
                $"Maximum CPU usage {maxCpuUsage:F1}% exceeds 10% limit");
        }
    }

    /// <summary>
    /// Represents a standardized error scenario for testing.
    /// </summary>
    public class ErrorScenario
    {
        public string Id { get; }
        public string Description { get; }
        public string ErrorType { get; }
        private readonly Action _execute;

        public ErrorScenario(string id, string description, Action execute)
        {
            Id = id;
            Description = description;
            ErrorType = id.Split('_')[0];
            _execute = execute;
        }

        public void Execute() => _execute();
    }
} 

