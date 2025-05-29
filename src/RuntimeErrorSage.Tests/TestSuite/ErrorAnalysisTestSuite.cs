using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Application.LLM;
using Moq;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Tests.TestSuite.Models;
using RuntimeErrorSage.Tests.TestSuite.Enums;
using RuntimeErrorSage.Application.Analysis.Interfaces;

namespace RuntimeErrorSage.Tests.TestSuite
{
    /// <summary>
    /// Test suite for error analysis functionality as described in the research paper.
    /// </summary>
    public class ErrorAnalysisTestSuite
    {
        private readonly ILMStudioClient _llmClient;
        private readonly IErrorAnalyzer _errorAnalyzer;
        private readonly Dictionary<string, ErrorScenario> _standardScenarios;
        private readonly Dictionary<string, ErrorScenario> _realWorldScenarios;
        private readonly List<ErrorScenario> _scenarios;
        private readonly List<PerformanceMetric> _metrics;

        public ErrorAnalysisTestSuite()
        {
            _llmClient = new LMStudioClient(new LMStudioOptions
            {
                ApiEndpoint = "http://127.0.0.1:1234/v1",
                ModelName = "qwen2.5-7b-instruct-1m",
                Temperature = 0.7f,
                MaxTokens = 4096
            });

            _errorAnalyzer = new ErrorAnalyzer(_llmClient);
            _standardScenarios = InitializeStandardScenarios();
            _realWorldScenarios = InitializeRealWorldScenarios();
            _scenarios = new List<ErrorScenario>();
            _metrics = new List<PerformanceMetric>();
        }

        [Fact]
        public async Task TestDatabaseErrorScenarios()
        {
            foreach (var scenario in _standardScenarios.Values.Where(s => s.Type == ErrorType.Database))
            {
                var result = await _errorAnalyzer.AnalyzeErrorAsync(scenario.Context);
                Assert.NotNull(result);
                Assert.True(result.Accuracy >= 0.8, $"Accuracy below threshold for {scenario.Name}");
                Assert.True(result.Latency <= 500, $"Latency too high for {scenario.Name}");
            }
        }

        [Fact]
        public async Task TestFileSystemErrorScenarios()
        {
            foreach (var scenario in _standardScenarios.Values.Where(s => s.Type == ErrorType.FileSystem))
            {
                var result = await _errorAnalyzer.AnalyzeErrorAsync(scenario.Context);
                Assert.NotNull(result);
                Assert.True(result.Accuracy >= 0.8, $"Accuracy below threshold for {scenario.Name}");
                Assert.True(result.Latency <= 500, $"Latency too high for {scenario.Name}");
            }
        }

        [Fact]
        public async Task TestHttpClientErrorScenarios()
        {
            foreach (var scenario in _standardScenarios.Values.Where(s => s.Type == ErrorType.HttpClient))
            {
                var result = await _errorAnalyzer.AnalyzeErrorAsync(scenario.Context);
                Assert.NotNull(result);
                Assert.True(result.Accuracy >= 0.8, $"Accuracy below threshold for {scenario.Name}");
                Assert.True(result.Latency <= 500, $"Latency too high for {scenario.Name}");
            }
        }

        [Fact]
        public async Task TestResourceErrorScenarios()
        {
            foreach (var scenario in _standardScenarios.Values.Where(s => s.Type == ErrorType.Resource))
            {
                var result = await _errorAnalyzer.AnalyzeErrorAsync(scenario.Context);
                Assert.NotNull(result);
                Assert.True(result.Accuracy >= 0.8, $"Accuracy below threshold for {scenario.Name}");
                Assert.True(result.Latency <= 500, $"Latency too high for {scenario.Name}");
            }
        }

        [Fact]
        public async Task TestRealWorldScenarios()
        {
            foreach (var scenario in _realWorldScenarios.Values)
            {
                var result = await _errorAnalyzer.AnalyzeErrorAsync(scenario.Context);
                Assert.NotNull(result);
                Assert.True(result.Accuracy >= 0.7, $"Accuracy below threshold for {scenario.Name}");
                Assert.True(result.Latency <= 500, $"Latency too high for {scenario.Name}");
            }
        }

        [Fact]
        public async Task TestPerformanceMetrics()
        {
            var metrics = new List<PerformanceMetric>();
            
            foreach (var scenario in _standardScenarios.Values)
            {
                var startTime = DateTime.UtcNow;
                var result = await _errorAnalyzer.AnalyzeErrorAsync(scenario.Context);
                var endTime = DateTime.UtcNow;

                metrics.Add(new PerformanceMetric
                {
                    ScenarioName = scenario.Name,
                    Latency = (endTime - startTime).TotalMilliseconds,
                    MemoryUsage = result.MemoryUsage,
                    CpuUsage = result.CpuUsage
                });
            }

            // Analyze metrics
            var avgLatency = metrics.Average(m => m.Latency);
            var maxLatency = metrics.Max(m => m.Latency);
            var avgMemory = metrics.Average(m => m.MemoryUsage);
            var maxMemory = metrics.Max(m => m.MemoryUsage);
            var avgCpu = metrics.Average(m => m.CpuUsage);
            var maxCpu = metrics.Max(m => m.CpuUsage);

            Assert.True(avgLatency <= 500, $"Average latency too high: {avgLatency}ms");
            Assert.True(maxLatency <= 1000, $"Max latency too high: {maxLatency}ms");
            Assert.True(avgMemory <= 100 * 1024 * 1024, $"Average memory usage too high: {avgMemory}MB");
            Assert.True(maxMemory <= 200 * 1024 * 1024, $"Max memory usage too high: {maxMemory}MB");
            Assert.True(avgCpu <= 10, $"Average CPU usage too high: {avgCpu}%");
            Assert.True(maxCpu <= 20, $"Max CPU usage too high: {maxCpu}%");
        }

        [Fact]
        public async Task TestGraphAnalysis()
        {
            foreach (var scenario in _standardScenarios.Values)
            {
                var result = await _errorAnalyzer.AnalyzeGraphAsync(scenario.Context);
                Assert.NotNull(result);
                Assert.True(result.IsValid);
                Assert.NotNull(result.DependencyGraph);
                Assert.True(result.DependencyGraph.Nodes.Count > 0);
                Assert.True(result.DependencyGraph.Edges.Count > 0);
            }
        }

        [Fact]
        public async Task TestImpactAnalysis()
        {
            foreach (var scenario in _standardScenarios.Values)
            {
                var result = await _errorAnalyzer.AnalyzeImpactAsync(scenario.Context);
                Assert.NotNull(result);
                Assert.True(result.IsValid);
                Assert.NotNull(result.AffectedNodes);
                Assert.NotNull(result.ImpactMetrics);
                Assert.True(result.ImpactMetrics.ContainsKey("severity"));
                Assert.True(result.ImpactMetrics.ContainsKey("spread"));
                Assert.True(result.ImpactMetrics.ContainsKey("recency"));
                Assert.True(result.ImpactMetrics.ContainsKey("importance"));
                Assert.True(result.ImpactMetrics.ContainsKey("connectivity"));
                Assert.True(result.ImpactMetrics.ContainsKey("error_proximity"));
            }
        }

        [Fact]
        public async Task TestRemediationAnalysis()
        {
            foreach (var scenario in _standardScenarios.Values)
            {
                var result = await _errorAnalyzer.AnalyzeRemediationAsync(scenario.Context);
                Assert.NotNull(result);
                Assert.True(result.IsValid);
                Assert.NotNull(result.Suggestions);
                Assert.True(result.Suggestions.Count > 0);
                foreach (var suggestion in result.Suggestions)
                {
                    Assert.NotNull(suggestion.Description);
                    Assert.NotNull(suggestion.Implementation);
                    Assert.True(suggestion.Confidence >= 0.0 && suggestion.Confidence <= 1.0);
                }
            }
        }

        private Dictionary<string, ErrorScenario> InitializeStandardScenarios()
        {
            var scenarios = new Dictionary<string, ErrorScenario>();

            // Database Scenarios
            AddDatabaseScenarios(scenarios);
            
            // File System Scenarios
            AddFileSystemScenarios(scenarios);
            
            // HTTP Client Scenarios
            AddHttpClientScenarios(scenarios);
            
            // Resource Scenarios
            AddResourceScenarios(scenarios);

            return scenarios;
        }

        private Dictionary<string, ErrorScenario> InitializeRealWorldScenarios()
        {
            var scenarios = new Dictionary<string, ErrorScenario>();

            // Database Scenarios
            scenarios.Add("db_connection_pool", new ErrorScenario
            {
                Name = "Connection Pool Exhaustion",
                Type = ErrorType.Database,
                Context = new ErrorContext
                {
                    Exception = new SqlException("Connection pool exhausted"),
                    StackTrace = "...",
                    AdditionalContext = new Dictionary<string, string>
                    {
                        { "ConnectionCount", "100" },
                        { "MaxPoolSize", "50" },
                        { "ActiveConnections", "49" }
                    }
                }
            });

            // Add more real-world scenarios...

            return scenarios;
        }

        private void AddDatabaseScenarios(Dictionary<string, ErrorScenario> scenarios)
        {
            // Connection failures
            scenarios.Add("db_connection_timeout", new ErrorScenario
            {
                Name = "Database Connection Timeout",
                Type = ErrorType.Database,
                Context = new ErrorContext
                {
                    Exception = new SqlException("Connection timeout"),
                    StackTrace = "...",
                    AdditionalContext = new Dictionary<string, string>
                    {
                        { "Server", "localhost" },
                        { "Port", "1433" },
                        { "Timeout", "30" }
                    }
                }
            });

            // Add more database scenarios...
        }

        private void AddFileSystemScenarios(Dictionary<string, ErrorScenario> scenarios)
        {
            // File access errors
            scenarios.Add("file_access_denied", new ErrorScenario
            {
                Name = "File Access Denied",
                Type = ErrorType.FileSystem,
                Context = new ErrorContext
                {
                    Exception = new UnauthorizedAccessException("Access to the path is denied"),
                    StackTrace = "...",
                    AdditionalContext = new Dictionary<string, string>
                    {
                        { "Path", "C:\\Program Files\\App\\config.json" },
                        { "User", "SYSTEM" },
                        { "Permissions", "Read" }
                    }
                }
            });

            // Add more file system scenarios...
        }

        private void AddHttpClientScenarios(Dictionary<string, ErrorScenario> scenarios)
        {
            // HTTP client errors
            scenarios.Add("http_timeout", new ErrorScenario
            {
                Name = "HTTP Request Timeout",
                Type = ErrorType.HttpClient,
                Context = new ErrorContext
                {
                    Exception = new HttpRequestException("The request timed out"),
                    StackTrace = "...",
                    AdditionalContext = new Dictionary<string, string>
                    {
                        { "Url", "https://api.example.com/data" },
                        { "Method", "GET" },
                        { "Timeout", "30" }
                    }
                }
            });

            // Add more HTTP client scenarios...
        }

        private void AddResourceScenarios(Dictionary<string, ErrorScenario> scenarios)
        {
            // Resource exhaustion errors
            scenarios.Add("memory_exhaustion", new ErrorScenario
            {
                Name = "Memory Exhaustion",
                Type = ErrorType.Resource,
                Context = new ErrorContext
                {
                    Exception = new OutOfMemoryException("Insufficient memory to continue the execution of the program"),
                    StackTrace = "...",
                    AdditionalContext = new Dictionary<string, string>
                    {
                        { "AvailableMemory", "512MB" },
                        { "RequiredMemory", "1GB" },
                        { "ProcessId", "1234" }
                    }
                }
            });

            // Add more resource scenarios...
        }

        public async Task RunTestSuiteAsync()
        {
            foreach (var scenario in _scenarios)
            {
                var startTime = DateTime.UtcNow;
                var result = await _errorAnalyzer.AnalyzeErrorAsync(scenario.Context);
                var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;

                _metrics.Add(new PerformanceMetric
                {
                    Name = $"AnalysisDuration_{scenario.Id}",
                    Value = duration,
                    Unit = "ms",
                    Metadata = new Dictionary<string, object>
                    {
                        { "ScenarioId", scenario.Id },
                        { "ScenarioName", scenario.Name }
                    }
                });

                Assert.Equal(scenario.ExpectedResult.Severity, result.Severity);
                Assert.Equal(scenario.ExpectedResult.CanAutoRemediate, result.CanAutoRemediate);
                Assert.Equal(scenario.ExpectedResult.RootCause, result.RootCause);
            }
        }

        public void AddScenario(ErrorScenario scenario)
        {
            _scenarios.Add(scenario);
        }

        public IEnumerable<PerformanceMetric> GetMetrics()
        {
            return _metrics;
        }
    }

    public class ErrorScenario
    {
        public string Name { get; set; }
        public ErrorType Type { get; set; }
        public ErrorContext Context { get; set; }
        public ExpectedResult ExpectedResult { get; set; }
    }

    public enum ErrorType
    {
        Database,
        FileSystem,
        HttpClient,
        Resource
    }

    public class PerformanceMetric
    {
        public string ScenarioName { get; set; }
        public double Latency { get; set; }
        public long MemoryUsage { get; set; }
        public double CpuUsage { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }
} 

