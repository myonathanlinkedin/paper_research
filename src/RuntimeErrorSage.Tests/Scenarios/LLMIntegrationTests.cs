using Xunit;
using FluentAssertions;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Application.Exceptions;
using RuntimeErrorSage.Infrastructure.Services;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Enums;
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;
using RuntimeErrorSage.Domain.Models.LLM;
using RuntimeErrorSage.Core.Extensions;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
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
        _service = TestHelper.CreateRuntimeErrorSageService(
            mcpClientMock: _mcpClientMock,
            remediationExecutorMock: _remediationExecutorMock,
            llmClientMock: _llmClientMock);
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
            }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? string.Empty));

        var llmAnalysisResult = new LLMAnalysisResult
        {
            RootCause = "Network connectivity issues between application and database server",
            Confidence = 0.85,
            SuggestedActions = new List<RemediationAction>
            {
                new RemediationAction
                {
                    Name = "Check network connectivity",
                    Description = "Check network connectivity"
                },
                new RemediationAction
                {
                    Name = "Verify database server is running",
                    Description = "Verify database server is running"
                },
                new RemediationAction
                {
                    Name = "Validate connection string",
                    Description = "Validate connection string"
                },
                new RemediationAction
                {
                    Name = "Implement connection retry with exponential backoff",
                    Description = "Implement connection retry with exponential backoff"
                }
            },
            ContextualInsights = new Dictionary<string, string>
            {
                { "NetworkLatency", "High latency detected in network path" },
                { "ServerLoad", "Database server showing high CPU usage" },
                { "ConnectionPool", "Connection pool near capacity" }
            }
        };

        _llmClientMock.Setup(x => x.AnalyzeErrorAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(llmAnalysisResult);

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
            }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? string.Empty));

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
            }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? string.Empty));

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
            }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? string.Empty));

        var llmAnalysisResult = new LLMAnalysisResult
        {
            RootCause = "Database connection pool exhaustion leading to cascading service failures",
            Confidence = 0.92,
            SuggestedActions = new List<RemediationAction>
            {
                new RemediationAction
                {
                    Name = "Implement circuit breaker pattern",
                    Description = "Implement circuit breaker pattern"
                },
                new RemediationAction
                {
                    Name = "Optimize database connection pool",
                    Description = "Optimize database connection pool"
                },
                new RemediationAction
                {
                    Name = "Add service isolation",
                    Description = "Add service isolation"
                },
                new RemediationAction
                {
                    Name = "Implement graceful degradation",
                    Description = "Implement graceful degradation"
                },
                new RemediationAction
                {
                    Name = "Add monitoring and alerting",
                    Description = "Add monitoring and alerting"
                }
            },
            ContextualInsights = new Dictionary<string, string>
            {
                { "SystemLoad", "High system load detected" },
                { "ResourceContention", "Database connection pool exhausted" },
                { "ServiceDependencies", "Tight coupling between services" },
                { "ErrorPropagation", "Errors cascading through service chain" }
            }
        };

        _llmClientMock.Setup(x => x.AnalyzeErrorAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(llmAnalysisResult);

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
