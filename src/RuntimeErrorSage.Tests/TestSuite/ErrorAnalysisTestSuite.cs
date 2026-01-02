using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Application.LLM.Options;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Application.LLM;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Analysis; // For GraphAnalysisResult, RemediationAnalysis, etc.
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;
using RuntimeErrorSage.Tests.TestSuite.Models;
using RuntimeErrorSage.Tests.TestSuite.Enums;
using RuntimeErrorSage.Domain.Enums;
using PerformanceMetric = RuntimeErrorSage.Tests.TestSuite.Models.PerformanceMetric;

namespace RuntimeErrorSage.Tests.TestSuite
{
    /// <summary>
    /// Test suite for error analysis functionality as described in the research paper.
    /// </summary>
    public class ErrorAnalysisTestSuite
    {
        private readonly ILMStudioClient _llmClient;
        private readonly IErrorAnalyzer _errorAnalyzer;
        private readonly Dictionary<string, ErrorAnalysisScenario> _standardScenarios;
        private readonly Dictionary<string, ErrorAnalysisScenario> _realWorldScenarios;
        private readonly List<ErrorAnalysisScenario> _scenarios;
        private readonly List<PerformanceMetric> _metrics;

        public ErrorAnalysisTestSuite()
        {
            var httpClient = new System.Net.Http.HttpClient();
            var options = Microsoft.Extensions.Options.Options.Create(new LMStudioOptions
            {
                BaseUrl = "http://127.0.0.1:1234",
                ModelId = "qwen2.5-7b-instruct-1m",
                Temperature = 0.7f,
                MaxTokens = 4096
            });
            var logger = new Moq.Mock<Microsoft.Extensions.Logging.ILogger<LMStudioClient>>().Object;
            _llmClient = new LMStudioClient(httpClient, options, logger);

            var llmService = new Moq.Mock<RuntimeErrorSage.Application.LLM.ILLMService>().Object;
            var mcpClient = new Moq.Mock<RuntimeErrorSage.Application.MCP.Interfaces.IMCPClient>().Object;
            var errorAnalyzerLogger = new Moq.Mock<Microsoft.Extensions.Logging.ILogger<RuntimeErrorSage.Application.Analysis.ErrorAnalyzer>>().Object;
            _errorAnalyzer = new RuntimeErrorSage.Application.Analysis.ErrorAnalyzer(errorAnalyzerLogger, _llmClient, mcpClient, llmService);
            _standardScenarios = InitializeStandardScenarios();
            _realWorldScenarios = InitializeRealWorldScenarios();
            _scenarios = new List<ErrorAnalysisScenario>();
            _metrics = new List<PerformanceMetric>();
        }

        [Fact]
        public async Task TestDatabaseErrorScenarios()
        {
            foreach (var scenario in _standardScenarios.Values.Where(s => s.Type == ErrorType.Database))
            {
                var exception = new Exception(scenario.Context?.Message ?? "Test error");
                var result = await _errorAnalyzer.AnalyzeErrorAsync(exception, scenario.Context);
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
                var exception = new Exception(scenario.Context?.Message ?? "Test error");
                var result = await _errorAnalyzer.AnalyzeErrorAsync(exception, scenario.Context);
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
                var exception = new Exception(scenario.Context?.Message ?? "Test error");
                var result = await _errorAnalyzer.AnalyzeErrorAsync(exception, scenario.Context);
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
                var exception = new Exception(scenario.Context?.Message ?? "Test error");
                var result = await _errorAnalyzer.AnalyzeErrorAsync(exception, scenario.Context);
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
                var exception = new Exception(scenario.Context?.Message ?? "Test error");
                var result = await _errorAnalyzer.AnalyzeErrorAsync(exception, scenario.Context);
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
                var exception = new Exception(scenario.Context?.Message ?? "Test error");
                var result = await _errorAnalyzer.AnalyzeErrorAsync(exception, scenario.Context);
                var endTime = DateTime.UtcNow;

                var duration = (endTime - startTime).TotalMilliseconds;
                metrics.Add(new PerformanceMetric(
                    name: scenario.Name,
                    durationMs: duration,
                    memoryUsage: (long)result.MemoryUsage,
                    cpuUsage: result.CpuUsage,
                    passed: true
                ));
            }

            // Analyze metrics
            var avgLatency = metrics.Average(m => m.DurationMs);
            var maxLatency = metrics.Max(m => m.DurationMs);
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
                Assert.NotNull(result.SuggestedActions);
                Assert.True(result.SuggestedActions.Count > 0);
                foreach (var action in result.SuggestedActions)
                {
                    Assert.NotNull(action.Description);
                    Assert.NotNull(action.Name);
                    Assert.NotNull(action.Id);
                }
            }
        }

        private Dictionary<string, ErrorAnalysisScenario> InitializeStandardScenarios()
        {
            var scenarios = new Dictionary<string, ErrorAnalysisScenario>();

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

        private Dictionary<string, ErrorAnalysisScenario> InitializeRealWorldScenarios()
        {
            var scenarios = new Dictionary<string, ErrorAnalysisScenario>();

            // Database Scenarios
            scenarios.Add("db_connection_pool", new ErrorAnalysisScenario
            {
                Name = "Connection Pool Exhaustion",
                Type = ErrorType.Database,
                Context = new ErrorContext(
                    error: new RuntimeError(
                        message: "Connection pool exhausted",
                        errorType: "DatabaseConnectionError",
                        source: "DatabaseService",
                        stackTrace: "..."
                    ),
                    context: "DatabaseService",
                    timestamp: DateTime.UtcNow
                )
                {
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

        private void AddDatabaseScenarios(Dictionary<string, ErrorAnalysisScenario> scenarios)
        {
            scenarios.Add("DB_CONN_001", new ErrorAnalysisScenario
            {
                Name = "Database Connection Error",
                Type = ErrorType.Database,
                Context = new ErrorContext(
                    error: new RuntimeError(
                        message: "Failed to connect to database",
                        errorType: "DatabaseConnectionError",
                        source: "DatabaseService",
                        stackTrace: string.Empty
                    ),
                    context: "DatabaseService",
                    timestamp: DateTime.UtcNow
                ),
                ExpectedResult = new ExpectedResult
                {
                    Accuracy = 0.8,
                    Latency = 500
                }
            });

            scenarios.Add("DB_QUERY_001", new ErrorAnalysisScenario
            {
                Name = "Database Query Error",
                Type = ErrorType.Database,
                Context = new ErrorContext(
                    error: new RuntimeError(
                        message: "Failed to execute query",
                        errorType: "DatabaseQueryError",
                        source: "DatabaseService",
                        stackTrace: string.Empty
                    ),
                    context: "DatabaseService",
                    timestamp: DateTime.UtcNow
                ),
                ExpectedResult = new ExpectedResult
                {
                    Accuracy = 0.8,
                    Latency = 500
                }
            });
        }

        private void AddFileSystemScenarios(Dictionary<string, ErrorAnalysisScenario> scenarios)
        {
            scenarios.Add("FS_READ_001", new ErrorAnalysisScenario
            {
                Name = "File Read Error",
                Type = ErrorType.FileSystem,
                Context = new ErrorContext(
                    error: new RuntimeError(
                        message: "Failed to read file",
                        errorType: "FileReadError",
                        source: "FileSystemService",
                        stackTrace: string.Empty
                    ),
                    context: "FileSystemService",
                    timestamp: DateTime.UtcNow
                ),
                ExpectedResult = new ExpectedResult
                {
                    Accuracy = 0.8,
                    Latency = 500
                }
            });

            scenarios.Add("FS_WRITE_001", new ErrorAnalysisScenario
            {
                Name = "File Write Error",
                Type = ErrorType.FileSystem,
                Context = new ErrorContext(
                    error: new RuntimeError(
                        message: "Failed to write file",
                        errorType: "FileWriteError",
                        source: "FileSystemService",
                        stackTrace: string.Empty
                    ),
                    context: "FileSystemService",
                    timestamp: DateTime.UtcNow
                ),
                ExpectedResult = new ExpectedResult
                {
                    Accuracy = 0.8,
                    Latency = 500
                }
            });
        }

        private void AddHttpClientScenarios(Dictionary<string, ErrorAnalysisScenario> scenarios)
        {
            scenarios.Add("HTTP_CONN_001", new ErrorAnalysisScenario
            {
                Name = "HTTP Connection Error",
                Type = ErrorType.HttpClient,
                Context = new ErrorContext(
                    error: new RuntimeError(
                        message: "Failed to connect to HTTP service",
                        errorType: "HttpConnectionError",
                        source: "HttpClientService",
                        stackTrace: string.Empty
                    ),
                    context: "HttpClientService",
                    timestamp: DateTime.UtcNow
                ),
                ExpectedResult = new ExpectedResult
                {
                    Accuracy = 0.8,
                    Latency = 500
                }
            });

            scenarios.Add("HTTP_REQ_001", new ErrorAnalysisScenario
            {
                Name = "HTTP Request Error",
                Type = ErrorType.HttpClient,
                Context = new ErrorContext(
                    error: new RuntimeError(
                        message: "Failed to send HTTP request",
                        errorType: "HttpRequestError",
                        source: "HttpClientService",
                        stackTrace: string.Empty
                    ),
                    context: "HttpClientService",
                    timestamp: DateTime.UtcNow
                ),
                ExpectedResult = new ExpectedResult
                {
                    Accuracy = 0.8,
                    Latency = 500
                }
            });
        }

        private void AddResourceScenarios(Dictionary<string, ErrorAnalysisScenario> scenarios)
        {
            scenarios.Add("RES_MEM_001", new ErrorAnalysisScenario
            {
                Name = "Memory Error",
                Type = ErrorType.Resource,
                Context = new ErrorContext(
                    error: new RuntimeError(
                        message: "Out of memory",
                        errorType: "MemoryError",
                        source: "ResourceService",
                        stackTrace: string.Empty
                    ),
                    context: "ResourceService",
                    timestamp: DateTime.UtcNow
                ),
                ExpectedResult = new ExpectedResult
                {
                    Accuracy = 0.8,
                    Latency = 500
                }
            });

            scenarios.Add("RES_CPU_001", new ErrorAnalysisScenario
            {
                Name = "CPU Error",
                Type = ErrorType.Resource,
                Context = new ErrorContext(
                    error: new RuntimeError(
                        message: "CPU limit exceeded",
                        errorType: "CpuError",
                        source: "ResourceService",
                        stackTrace: string.Empty
                    ),
                    context: "ResourceService",
                    timestamp: DateTime.UtcNow
                ),
                ExpectedResult = new ExpectedResult
                {
                    Accuracy = 0.8,
                    Latency = 500
                }
            });
        }

        public async Task RunTestSuiteAsync()
        {
            foreach (var scenario in _scenarios)
            {
                var startTime = DateTime.UtcNow;
                var exception = new Exception(scenario.Context?.Message ?? "Test error");
                var result = await _errorAnalyzer.AnalyzeErrorAsync(exception, scenario.Context);
                var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;

                _metrics.Add(new PerformanceMetric(
                    name: $"AnalysisDuration_{scenario.Id}",
                    durationMs: duration,
                    memoryUsage: 0,
                    cpuUsage: 0,
                    passed: true
                ));

                Assert.Equal(scenario.ExpectedResult.Severity, result.Severity);
                Assert.Equal(scenario.ExpectedResult.CanAutoRemediate, result.CanAutoRemediate);
                Assert.Equal(scenario.ExpectedResult.RootCause, result.RootCause);
            }
        }

        public void AddScenario(ErrorAnalysisScenario scenario)
        {
            _scenarios.Add(scenario);
        }

        public IEnumerable<PerformanceMetric> GetMetrics()
        {
            return _metrics;
        }
    }

    public class ErrorAnalysisScenario
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
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

} 

