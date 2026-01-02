using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Tests.TestSuite.Models;
using System.Net.Sockets;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Factory for creating test suites with predefined scenarios
/// </summary>
public class TestSuiteFactory
{
    private readonly IErrorAnalyzer _errorAnalyzer;

    public TestSuiteFactory(IErrorAnalyzer errorAnalyzer)
    {
        _errorAnalyzer = errorAnalyzer;
    }

    /// <summary>
    /// Creates a test suite with all standard scenarios
    /// </summary>
    public TestSuite CreateStandardSuite()
    {
        var suite = new TestSuite(_errorAnalyzer);

        // Database error scenarios
        suite.AddScenario(new TestScenario
        {
            Name = "Database Connection Timeout",
            Description = "Tests handling of database connection timeout errors",
            Category = "Database",
            ExpectedErrorType = "DatabaseConnectionError",
            ExpectedErrorMessage = "Connection timeout",
            ExpectedRemediation = "Retry with exponential backoff",
            SetupAction = async () => await Task.Delay(100),
            TriggerAction = async () => throw new TimeoutException("Database connection timed out"),
            Metadata = new Dictionary<string, object>
            {
                { "PerformanceTest", true },
                { "BaselineComparison", true }
            }
        });

        // HTTP error scenarios
        suite.AddScenario(new TestScenario
        {
            Name = "HTTP 500 Error",
            Description = "Tests handling of HTTP 500 server errors",
            Category = "HTTP",
            ExpectedErrorType = "HttpServerError",
            ExpectedErrorMessage = "Internal server error",
            ExpectedRemediation = "Retry with circuit breaker",
            SetupAction = async () => await Task.Delay(100),
            TriggerAction = async () => throw new HttpRequestException("Internal server error", null, System.Net.HttpStatusCode.InternalServerError),
            Metadata = new Dictionary<string, object>
            {
                { "PerformanceTest", true }
            }
        });

        // Memory error scenarios
        suite.AddScenario(new TestScenario
        {
            Name = "Out of Memory",
            Description = "Tests handling of out of memory errors",
            Category = "Memory",
            ExpectedErrorType = "OutOfMemoryError",
            ExpectedErrorMessage = "Insufficient memory",
            ExpectedRemediation = "Release unused resources",
            SetupAction = async () => await Task.Delay(100),
            TriggerAction = async () => throw new OutOfMemoryException("Insufficient memory"),
            Metadata = new Dictionary<string, object>
            {
                { "PerformanceTest", true },
                { "BaselineComparison", true }
            }
        });

        // File system error scenarios
        suite.AddScenario(new TestScenario
        {
            Name = "File Not Found",
            Description = "Tests handling of file not found errors",
            Category = "FileSystem",
            ExpectedErrorType = "FileNotFoundError",
            ExpectedErrorMessage = "File not found",
            ExpectedRemediation = "Create file if not exists",
            SetupAction = async () => await Task.Delay(100),
            TriggerAction = async () => throw new FileNotFoundException("File not found"),
            Metadata = new Dictionary<string, object>
            {
                { "PerformanceTest", true }
            }
        });

        // Network error scenarios
        suite.AddScenario(new TestScenario
        {
            Name = "Network Unavailable",
            Description = "Tests handling of network unavailability",
            Category = "Network",
            ExpectedErrorType = "NetworkError",
            ExpectedErrorMessage = "Network is unreachable",
            ExpectedRemediation = "Wait for network recovery",
            SetupAction = async () => await Task.Delay(100),
            TriggerAction = async () => throw new SocketException((int)SocketError.NetworkUnreachable),
            Metadata = new Dictionary<string, object>
            {
                { "PerformanceTest", true },
                { "BaselineComparison", true }
            }
        });

        return suite;
    }

    /// <summary>
    /// Creates a test suite with performance-focused scenarios
    /// </summary>
    public TestSuite CreatePerformanceSuite()
    {
        var suite = new TestSuite(_errorAnalyzer);

        // Add performance-intensive scenarios
        suite.AddScenario(new TestScenario
        {
            Name = "High Concurrency Database Errors",
            Description = "Tests handling of multiple concurrent database errors",
            Category = "Performance",
            ExpectedErrorType = "ConcurrentDatabaseError",
            ExpectedErrorMessage = "Too many concurrent connections",
            ExpectedRemediation = "Implement connection pooling",
            SetupAction = async () => await Task.Delay(100),
            TriggerAction = async () => throw new InvalidOperationException("Too many concurrent connections"),
            Metadata = new Dictionary<string, object>
            {
                { "PerformanceTest", true },
                { "ConcurrentUsers", 100 }
            }
        });

        return suite;
    }

    /// <summary>
    /// Creates a test suite with baseline comparison scenarios
    /// </summary>
    public TestSuite CreateBaselineComparisonSuite()
    {
        var suite = new TestSuite(_errorAnalyzer);

        // Add scenarios for baseline comparison
        suite.AddScenario(new TestScenario
        {
            Name = "Baseline Error Analysis",
            Description = "Tests error analysis against baseline implementation",
            Category = "Baseline",
            ExpectedErrorType = "GenericError",
            ExpectedErrorMessage = "Test error",
            ExpectedRemediation = "No remediation needed",
            SetupAction = async () => await Task.Delay(100),
            TriggerAction = async () => throw new Exception("Test error"),
            Metadata = new Dictionary<string, object>
            {
                { "BaselineComparison", true }
            }
        });

        return suite;
    }
} 
