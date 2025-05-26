using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Core.LLM;

namespace RuntimeErrorSage.Tests.TestSuite
{
    public class ErrorAnalysisTestSuite
    {
        private readonly ILMStudioClient _llmClient;
        private readonly IErrorAnalyzer _errorAnalyzer;
        private readonly Dictionary<string, ErrorScenario> _standardScenarios;
        private readonly Dictionary<string, ErrorScenario> _realWorldScenarios;

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
            // Permission issues
            scenarios.Add("fs_permission_denied", new ErrorScenario
            {
                Name = "File Permission Denied",
                Type = ErrorType.FileSystem,
                Context = new ErrorContext
                {
                    Exception = new UnauthorizedAccessException("Access to the path is denied"),
                    StackTrace = "...",
                    AdditionalContext = new Dictionary<string, string>
                    {
                        { "FilePath", "C:\\Program Files\\App\\config.json" },
                        { "User", "NETWORK SERVICE" },
                        { "RequiredAccess", "Write" }
                    }
                }
            });

            // Add more file system scenarios...
        }

        private void AddHttpClientScenarios(Dictionary<string, ErrorScenario> scenarios)
        {
            // Connection timeouts
            scenarios.Add("http_connection_timeout", new ErrorScenario
            {
                Name = "HTTP Connection Timeout",
                Type = ErrorType.HttpClient,
                Context = new ErrorContext
                {
                    Exception = new HttpRequestException("Connection timed out"),
                    StackTrace = "...",
                    AdditionalContext = new Dictionary<string, string>
                    {
                        { "Url", "https://api.example.com" },
                        { "Timeout", "30" },
                        { "Method", "GET" }
                    }
                }
            });

            // Add more HTTP client scenarios...
        }

        private void AddResourceScenarios(Dictionary<string, ErrorScenario> scenarios)
        {
            // Memory allocation
            scenarios.Add("mem_allocation_failed", new ErrorScenario
            {
                Name = "Memory Allocation Failed",
                Type = ErrorType.Resource,
                Context = new ErrorContext
                {
                    Exception = new OutOfMemoryException("Insufficient memory"),
                    StackTrace = "...",
                    AdditionalContext = new Dictionary<string, string>
                    {
                        { "RequestedSize", "1GB" },
                        { "AvailableMemory", "512MB" },
                        { "ProcessMemory", "2GB" }
                    }
                }
            });

            // Add more resource scenarios...
        }
    }

    public class ErrorScenario
    {
        public string Name { get; set; }
        public ErrorType Type { get; set; }
        public ErrorContext Context { get; set; }
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
    }
} 
