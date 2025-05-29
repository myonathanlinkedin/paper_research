using Xunit;
using FluentAssertions;
using RuntimeErrorSage.Model.Analysis;
using RuntimeErrorSage.Model.Remediation;
using RuntimeErrorSage.Model.MCP;
using RuntimeErrorSage.Model.LLM;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using RuntimeErrorSage.Tests.Helpers;

namespace RuntimeErrorSage.Tests.Scenarios;

public class LLMIntegrationTests
{
    private readonly Mock<IMCPClient> _mcpClientMock;
    private readonly Mock<IRemediationExecutor> _remediationExecutorMock;
    private readonly Mock<IQwenLLMClient> _llmClientMock;
    private readonly RuntimeErrorSageService _service;

    public LLMIntegrationTests()
    {
        _mcpClientMock = TestHelper.CreateMCPClientMock();
        _remediationExecutorMock = TestHelper.CreateRemediationExecutorMock();
        _llmClientMock = new Mock<IQwenLLMClient>();
        _service = new RuntimeErrorSageService(
            _mcpClientMock.Object, 
            _remediationExecutorMock.Object,
            llmClient: _llmClientMock.Object);
    }

    [Fact]
    public async Task ShouldAnalyzeErrorWithLLM()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "DatabaseConnectionError",
            "Failed to connect to database: Connection timeout after 30 seconds",
            "Database",
            new Dictionary<string, object>
            {
                { "ConnectionString", "Server=localhost;Database=testdb" },
                { "RetryCount", 3 },
                { "Timeout", 30000 }
            });

        var llmResponse = new LLMResponse
        {
            Analysis = new ErrorAnalysis
            {
                RootCause = "Network connectivity issues between application and database server",
                Confidence = 0.85,
                SuggestedActions = new[]
                {
                    "Check network connectivity",
                    "Verify database server is running",
                    "Validate connection string",
                    "Implement connection retry with exponential backoff"
                },
                ContextualInsights = new Dictionary<string, string>
                {
                    { "NetworkLatency", "High latency detected in network path" },
                    { "ServerLoad", "Database server showing high CPU usage" },
                    { "ConnectionPool", "Connection pool near capacity" }
                }
            },
            RemediationSuggestions = new[]
            {
                new RemediationSuggestion
                {
                    Action = "ImplementCircuitBreaker",
                    Priority = 1,
                    Description = "Add circuit breaker pattern to prevent cascading failures",
                    ImplementationSteps = new[]
                    {
                        "Configure circuit breaker thresholds",
                        "Add fallback mechanism",
                        "Implement health checks"
                    }
                },
                new RemediationSuggestion
                {
                    Action = "OptimizeConnectionPool",
                    Priority = 2,
                    Description = "Optimize database connection pool settings",
                    ImplementationSteps = new[]
                    {
                        "Adjust pool size based on load",
                        "Implement connection timeout",
                        "Add connection validation"
                    }
                }
            }
        };

        _llmClientMock.Setup(x => x.AnalyzeErrorAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(llmResponse);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.LLMAnalysis.Should().NotBeNull();
        result.LLMAnalysis.RootCause.Should().Be("Network connectivity issues between application and database server");
        result.LLMAnalysis.Confidence.Should().Be(0.85);
        result.LLMAnalysis.SuggestedActions.Should().HaveCount(4);
        result.LLMAnalysis.ContextualInsights.Should().HaveCount(3);
        result.RemediationPlan.Strategies.Should().HaveCount(2);
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "ImplementCircuitBreaker" || 
            s.Name == "OptimizeConnectionPool");

        _llmClientMock.Verify(x => x.AnalyzeErrorAsync(It.IsAny<ErrorContext>()), Times.Once);
    }

    [Fact]
    public async Task ShouldHandleLLMTimeout()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "LLMTimeout",
            "LLM analysis timed out",
            "LLMService",
            new Dictionary<string, object>
            {
                { "Timeout", 30000 },
                { "Model", "Qwen-2.5-7B-Instruct-1M" },
                { "RequestId", "req-123456" }
            });

        _llmClientMock.Setup(x => x.AnalyzeErrorAsync(It.IsAny<ErrorContext>()))
            .ThrowsAsync(new LLMTimeoutException("LLM analysis timed out after 30 seconds"));

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.LLMAnalysis.Should().BeNull();
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "FallbackToRuleBasedAnalysis" || 
            s.Name == "RetryLLMAnalysis");

        _llmClientMock.Verify(x => x.AnalyzeErrorAsync(It.IsAny<ErrorContext>()), Times.Once);
    }

    [Fact]
    public async Task ShouldHandleLLMError()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "LLMError",
            "LLM analysis failed",
            "LLMService",
            new Dictionary<string, object>
            {
                { "Model", "Qwen-2.5-7B-Instruct-1M" },
                { "RequestId", "req-123456" },
                { "ErrorDetails", "Model inference failed" }
            });

        _llmClientMock.Setup(x => x.AnalyzeErrorAsync(It.IsAny<ErrorContext>()))
            .ThrowsAsync(new LLMException("Model inference failed"));

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.LLMAnalysis.Should().BeNull();
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "FallbackToRuleBasedAnalysis" || 
            s.Name == "SwitchToBackupModel");

        _llmClientMock.Verify(x => x.AnalyzeErrorAsync(It.IsAny<ErrorContext>()), Times.Once);
    }

    [Fact]
    public async Task ShouldHandleComplexErrorScenario()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "ComplexSystemError",
            "Multiple services affected by cascading failures",
            "System",
            new Dictionary<string, object>
            {
                { "AffectedServices", new[] { "AuthService", "UserService", "PaymentService" } },
                { "ErrorChain", new[] { "DatabaseError", "AuthServiceFailure", "UserServiceFailure" } },
                { "SystemMetrics", new Dictionary<string, object>
                    {
                        { "CPUUsage", 95.5 },
                        { "MemoryUsage", 85.2 },
                        { "NetworkLatency", 250 },
                        { "ErrorRate", 0.15 }
                    }
                }
            });

        var llmResponse = new LLMResponse
        {
            Analysis = new ErrorAnalysis
            {
                RootCause = "Database connection pool exhaustion leading to cascading service failures",
                Confidence = 0.92,
                SuggestedActions = new[]
                {
                    "Implement circuit breaker pattern",
                    "Optimize database connection pool",
                    "Add service isolation",
                    "Implement graceful degradation",
                    "Add monitoring and alerting"
                },
                ContextualInsights = new Dictionary<string, string>
                {
                    { "SystemLoad", "High system load detected" },
                    { "ResourceContention", "Database connection pool exhausted" },
                    { "ServiceDependencies", "Tight coupling between services" },
                    { "ErrorPropagation", "Errors cascading through service chain" }
                }
            },
            RemediationSuggestions = new[]
            {
                new RemediationSuggestion
                {
                    Action = "ImplementCircuitBreaker",
                    Priority = 1,
                    Description = "Add circuit breaker to prevent cascading failures",
                    ImplementationSteps = new[]
                    {
                        "Configure circuit breaker thresholds",
                        "Add fallback mechanisms",
                        "Implement health checks"
                    }
                },
                new RemediationSuggestion
                {
                    Action = "OptimizeConnectionPool",
                    Priority = 2,
                    Description = "Optimize database connection pool settings",
                    ImplementationSteps = new[]
                    {
                        "Adjust pool size",
                        "Add connection timeout",
                        "Implement connection validation"
                    }
                },
                new RemediationSuggestion
                {
                    Action = "ServiceIsolation",
                    Priority = 3,
                    Description = "Implement service isolation patterns",
                    ImplementationSteps = new[]
                    {
                        "Add bulkhead pattern",
                        "Implement timeouts",
                        "Add retry policies"
                    }
                }
            }
        };

        _llmClientMock.Setup(x => x.AnalyzeErrorAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(llmResponse);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.LLMAnalysis.Should().NotBeNull();
        result.LLMAnalysis.RootCause.Should().Be("Database connection pool exhaustion leading to cascading service failures");
        result.LLMAnalysis.Confidence.Should().Be(0.92);
        result.LLMAnalysis.SuggestedActions.Should().HaveCount(5);
        result.LLMAnalysis.ContextualInsights.Should().HaveCount(4);
        result.RemediationPlan.Strategies.Should().HaveCount(3);
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "ImplementCircuitBreaker" || 
            s.Name == "OptimizeConnectionPool" ||
            s.Name == "ServiceIsolation");

        _llmClientMock.Verify(x => x.AnalyzeErrorAsync(It.IsAny<ErrorContext>()), Times.Once);
    }
} 
