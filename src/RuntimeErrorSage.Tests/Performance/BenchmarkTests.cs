using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Application.LLM.Options;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.LLM;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Core.MCP;
using RuntimeErrorSage.Domain.Models.Error;

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

        var optionsMock = new Mock<IOptions<LMStudioOptions>>();
        optionsMock.Setup(x => x.Value).Returns(new LMStudioOptions { BaseUrl = "http://localhost:1234" });
        _llmClient = new LMStudioClient(_httpClientMock.Object, optionsMock.Object, _llmLoggerMock.Object);
        var coreOptions = new RuntimeErrorSage.Core.MCP.MCPClientOptions();
        var coreOptionsMock = new Mock<IOptions<RuntimeErrorSage.Core.MCP.MCPClientOptions>>();
        coreOptionsMock.Setup(x => x.Value).Returns(coreOptions);
        var storageMock = new Mock<RuntimeErrorSage.Application.Storage.Interfaces.IPatternStorage>();
        var errorAnalyzerMock = new Mock<RuntimeErrorSage.Application.Analysis.Interfaces.IErrorAnalyzer>();
        _mcpClient = new MCPClient(_mcpLoggerMock.Object, coreOptionsMock.Object, storageMock.Object, errorAnalyzerMock.Object);
        var llmServiceMock = new Mock<RuntimeErrorSage.Application.LLM.ILLMService>();
        _errorAnalyzer = new ErrorAnalyzer(_analyzerLoggerMock.Object, _llmClient, _mcpClient, llmServiceMock.Object);

        _testContext = new ErrorContext(
            error: new RuntimeError(
                message: "Test error message",
                errorType: "TestError",
                source: "TestService",
                stackTrace: string.Empty
            ),
            context: "TestService",
            timestamp: DateTime.UtcNow
        );

        _testContext.AddMetadata("DatabaseName", "TestDB");
        _testContext.AddMetadata("FilePath", "/test/path");
        _testContext.AddMetadata("ServiceEndpoint", "http://test.service");

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
        await _errorAnalyzer.AnalyzeRemediationAsync(_testContext);
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
        var remediation = await _errorAnalyzer.AnalyzeRemediationAsync(_testContext);
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

        var optionsMock = new Mock<IOptions<LMStudioOptions>>();
        optionsMock.Setup(x => x.Value).Returns(new LMStudioOptions { BaseUrl = "http://localhost:1234" });
        _llmClient = new LMStudioClient(_httpClientMock.Object, optionsMock.Object, _llmLoggerMock.Object);
        var coreOptions = new RuntimeErrorSage.Core.MCP.MCPClientOptions();
        var coreOptionsMock = new Mock<IOptions<RuntimeErrorSage.Core.MCP.MCPClientOptions>>();
        coreOptionsMock.Setup(x => x.Value).Returns(coreOptions);
        var storageMock = new Mock<RuntimeErrorSage.Application.Storage.Interfaces.IPatternStorage>();
        var errorAnalyzerMock = new Mock<RuntimeErrorSage.Application.Analysis.Interfaces.IErrorAnalyzer>();
        _mcpClient = new MCPClient(_mcpLoggerMock.Object, coreOptionsMock.Object, storageMock.Object, errorAnalyzerMock.Object);
        var llmServiceMock = new Mock<RuntimeErrorSage.Application.LLM.ILLMService>();
        _errorAnalyzer = new ErrorAnalyzer(_analyzerLoggerMock.Object, _llmClient, _mcpClient, llmServiceMock.Object);

        // Generate test data
        _testContexts = new List<ErrorContext>();
        _testExceptions = new List<Exception>();

        for (int i = 0; i < 1000; i++)
        {
            _testContexts.Add(new ErrorContext(
                error: new RuntimeError(
                    message: $"Test error message {i}",
                    errorType: "TestError",
                    source: $"TestService_{i % 10}",
                    stackTrace: string.Empty
                ),
                context: $"TestService_{i % 10}",
                timestamp: DateTime.UtcNow.AddSeconds(-i)
            ));

            _testContexts[i].AddMetadata("DatabaseName", $"TestDB_{i % 3}");
            _testContexts[i].AddMetadata("FilePath", $"/test/path_{i}");
            _testContexts[i].AddMetadata("ServiceEndpoint", $"http://test.service_{i % 2}");

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
                var remediation = await _errorAnalyzer.AnalyzeRemediationAsync(_testContexts[i]);
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
        var summary = BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchmarkTests>();
        Console.WriteLine("Performance Benchmarks:");
        Console.WriteLine(summary);

        var stressSummary = BenchmarkDotNet.Running.BenchmarkRunner.Run<StressTests>();
        Console.WriteLine("\nStress Test Results:");
        Console.WriteLine(stressSummary);
    }
} 

