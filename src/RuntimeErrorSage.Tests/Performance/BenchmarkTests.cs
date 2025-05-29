using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Logging;
using Moq;
using RuntimeErrorSage.Application.LLM;
using RuntimeErrorSage.Application.MCP;
using RuntimeErrorSage.Application.Analysis;

namespace RuntimeErrorSage.Tests.Performance;

[MemoryDiagnoser]
public class BenchmarkTests
{
    private readonly Mock<ILogger<LMStudioClient>> _llmLoggerMock;
    private readonly Mock<ILogger<MCPClient>> _mcpLoggerMock;
    private readonly Mock<ILogger<ErrorAnalyzer>> _analyzerLoggerMock;
    private readonly Mock<HttpClient> _httpClientMock;
    private readonly LMStudioClient _llmClient;
    private readonly MCPClient _mcpClient;
    private readonly ErrorAnalyzer _errorAnalyzer;
    private readonly ErrorContext _testContext;
    private readonly Exception _testException;

    public BenchmarkTests()
    {
        _llmLoggerMock = new Mock<ILogger<LMStudioClient>>();
        _mcpLoggerMock = new Mock<ILogger<MCPClient>>();
        _analyzerLoggerMock = new Mock<ILogger<ErrorAnalyzer>>();
        _httpClientMock = new Mock<HttpClient>();

        _llmClient = new LMStudioClient(_httpClientMock.Object, _llmLoggerMock.Object);
        _mcpClient = new MCPClient(_mcpLoggerMock.Object);
        _errorAnalyzer = new ErrorAnalyzer(_analyzerLoggerMock.Object, _llmClient, _mcpClient);

        _testContext = new ErrorContext
        {
            ServiceName = "TestService",
            OperationName = "TestOperation",
            Timestamp = DateTime.UtcNow,
            CorrelationId = Guid.NewGuid().ToString(),
            AdditionalContext = new Dictionary<string, object>
            {
                { "DatabaseName", "TestDB" },
                { "FilePath", "/test/path" },
                { "ServiceEndpoint", "http://test.service" }
            }
        };

        _testException = new InvalidOperationException("Test error message");
    }

    [Benchmark]
    public async Task ErrorAnalysisBenchmark()
    {
        await _errorAnalyzer.AnalyzeErrorAsync(_testException, _testContext);
    }

    [Benchmark]
    public async Task RemediationGenerationBenchmark()
    {
        var analysis = await _errorAnalyzer.AnalyzeErrorAsync(_testException, _testContext);
        await _errorAnalyzer.GenerateRemediationAsync(analysis);
    }

    [Benchmark]
    public async Task ContextPublishingBenchmark()
    {
        await _mcpClient.PublishContextAsync(_testContext);
    }

    [Benchmark]
    public async Task PatternRetrievalBenchmark()
    {
        await _mcpClient.GetErrorPatternsAsync(_testContext.ServiceName);
    }

    [Benchmark]
    public async Task EndToEndErrorHandlingBenchmark()
    {
        // Simulate end-to-end error handling
        var analysis = await _errorAnalyzer.AnalyzeErrorAsync(_testException, _testContext);
        await _mcpClient.PublishContextAsync(_testContext);
        var remediation = await _errorAnalyzer.GenerateRemediationAsync(analysis);
        await _mcpClient.GetErrorPatternsAsync(_testContext.ServiceName);
    }
}

[MemoryDiagnoser]
public class StressTests
{
    private readonly Mock<ILogger<LMStudioClient>> _llmLoggerMock;
    private readonly Mock<ILogger<MCPClient>> _mcpLoggerMock;
    private readonly Mock<ILogger<ErrorAnalyzer>> _analyzerLoggerMock;
    private readonly Mock<HttpClient> _httpClientMock;
    private readonly LMStudioClient _llmClient;
    private readonly MCPClient _mcpClient;
    private readonly ErrorAnalyzer _errorAnalyzer;
    private readonly List<ErrorContext> _testContexts;
    private readonly List<Exception> _testExceptions;

    public StressTests()
    {
        _llmLoggerMock = new Mock<ILogger<LMStudioClient>>();
        _mcpLoggerMock = new Mock<ILogger<MCPClient>>();
        _analyzerLoggerMock = new Mock<ILogger<ErrorAnalyzer>>();
        _httpClientMock = new Mock<HttpClient>();

        _llmClient = new LMStudioClient(_httpClientMock.Object, _llmLoggerMock.Object);
        _mcpClient = new MCPClient(_mcpLoggerMock.Object);
        _errorAnalyzer = new ErrorAnalyzer(_analyzerLoggerMock.Object, _llmClient, _mcpClient);

        // Generate test data
        _testContexts = new List<ErrorContext>();
        _testExceptions = new List<Exception>();

        for (int i = 0; i < 1000; i++)
        {
            _testContexts.Add(new ErrorContext
            {
                ServiceName = $"TestService_{i % 10}",
                OperationName = $"TestOperation_{i % 5}",
                Timestamp = DateTime.UtcNow.AddSeconds(-i),
                CorrelationId = Guid.NewGuid().ToString(),
                AdditionalContext = new Dictionary<string, object>
                {
                    { "DatabaseName", $"TestDB_{i % 3}" },
                    { "FilePath", $"/test/path_{i}" },
                    { "ServiceEndpoint", $"http://test.service_{i % 2}" }
                }
            });

            _testExceptions.Add(new InvalidOperationException($"Test error message {i}"));
        }
    }

    [Benchmark]
    public async Task ConcurrentErrorAnalysisBenchmark()
    {
        var tasks = new List<Task>();
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(_errorAnalyzer.AnalyzeErrorAsync(_testExceptions[i], _testContexts[i]));
        }
        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task ConcurrentContextPublishingBenchmark()
    {
        var tasks = new List<Task>();
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(_mcpClient.PublishContextAsync(_testContexts[i]));
        }
        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task PatternRecognitionBenchmark()
    {
        // Publish contexts first
        foreach (var context in _testContexts.Take(100))
        {
            await _mcpClient.PublishContextAsync(context);
        }

        // Then retrieve patterns for all services
        var tasks = new List<Task>();
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(_mcpClient.GetErrorPatternsAsync($"TestService_{i}"));
        }
        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task EndToEndStressTestBenchmark()
    {
        var tasks = new List<Task>();
        for (int i = 0; i < 50; i++)
        {
            tasks.Add(Task.Run(async () =>
            {
                var analysis = await _errorAnalyzer.AnalyzeErrorAsync(_testExceptions[i], _testContexts[i]);
                await _mcpClient.PublishContextAsync(_testContexts[i]);
                var remediation = await _errorAnalyzer.GenerateRemediationAsync(analysis);
                await _mcpClient.GetErrorPatternsAsync(_testContexts[i].ServiceName);
            }));
        }
        await Task.WhenAll(tasks);
    }
}

public static class BenchmarkRunner
{
    public static void RunBenchmarks()
    {
        var summary = BenchmarkRunner.Run<BenchmarkTests>();
        Console.WriteLine("Performance Benchmarks:");
        Console.WriteLine(summary);

        var stressSummary = BenchmarkRunner.Run<StressTests>();
        Console.WriteLine("\nStress Test Results:");
        Console.WriteLine(stressSummary);
    }
} 

