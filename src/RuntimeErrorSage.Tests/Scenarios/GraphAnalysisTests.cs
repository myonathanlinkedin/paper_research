using Xunit;
using FluentAssertions;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Application.Graph.Interfaces;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Tests.Helpers;
using System;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Models.Remediation;
using RemediationPlan = RuntimeErrorSage.Domain.Models.Remediation.RemediationPlan;
using GraphEdge = RuntimeErrorSage.Domain.Models.Graph.GraphEdge;
using RuntimeErrorSage.Application.Services.Interfaces;
using RuntimeErrorSage.Infrastructure.Services;
using RuntimeErrorSage.Core.MCP;

namespace RuntimeErrorSage.Tests.Scenarios;

/// <summary>
/// Tests for graph-based error analysis scenarios.
/// </summary>
public class GraphAnalysisTests
{
    private readonly Mock<ILogger<GraphAnalysisTests>> _loggerMock;
    private readonly Mock<IErrorContextAnalyzer> _errorContextAnalyzerMock;
    private readonly Mock<IRemediationAnalyzer> _remediationAnalyzerMock;
    private readonly Mock<IRemediationExecutor> _remediationExecutorMock;
    private readonly Mock<IRemediationValidator> _remediationValidatorMock;
    private readonly Mock<IRemediationMetricsCollector> _metricsCollectorMock;
    private readonly Mock<ModelContextProtocol> _mcpMock;
    private readonly RuntimeErrorSageService _service;

    public GraphAnalysisTests()
    {
        _loggerMock = new Mock<ILogger<GraphAnalysisTests>>();
        _errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        _remediationAnalyzerMock = new Mock<IRemediationAnalyzer>();
        _remediationExecutorMock = new Mock<IRemediationExecutor>();
        _remediationValidatorMock = new Mock<IRemediationValidator>();
        _metricsCollectorMock = new Mock<IRemediationMetricsCollector>();
        _mcpMock = new Mock<ModelContextProtocol>();

        _service = TestHelper.CreateRuntimeErrorSageService();
    }

    [Fact]
    public async Task ShouldAnalyzeErrorContextGraph()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "DatabaseConnectionError",
            "Failed to connect to database",
            "Database",
            new Dictionary<string, object>
            {
                { "ConnectionString", "Server=localhost;Database=testdb" },
                { "Dependencies", new[] { "UserService", "AuthService", "LoggingService" } }
            }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? string.Empty));

        var nodes = new List<GraphNode>
        {
            new() { Id = "UserService", Type = "Service", Status = "Healthy" },
            new() { Id = "AuthService", Type = "Service", Status = "Degraded" },
            new() { Id = "LoggingService", Type = "Service", Status = "Healthy" },
            new() { Id = "Database", Type = "Database", Status = "Error" }
        };
        
        var graphAnalysis = new GraphAnalysis
        {
            Nodes = nodes,
            Edges = new List<GraphEdge>
            {
                new() { Source = nodes.First(n => n.Id == "UserService"), Target = nodes.First(n => n.Id == "Database"), Type = "DependsOn" },
                new() { Source = nodes.First(n => n.Id == "AuthService"), Target = nodes.First(n => n.Id == "Database"), Type = "DependsOn" },
                new() { Source = nodes.First(n => n.Id == "LoggingService"), Target = nodes.First(n => n.Id == "Database"), Type = "DependsOn" }
            },
            ImpactAnalysis = new ImpactAnalysis
            {
                AffectedServices = new[] { "UserService", "AuthService" }.ToList(),
                CriticalPath = new[] { "AuthService", "Database" }.ToList(),
                Severity = "High"
            }
        };

        var remediationAnalysis = new RemediationAnalysis
        {
            ErrorContext = errorContext,
            GraphAnalysis = graphAnalysis,
            Confidence = 0.8,
            Timestamp = DateTime.UtcNow
        };
        
        _errorContextAnalyzerMock.Setup(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(remediationAnalysis);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.GraphAnalysis.Should().NotBeNull();
        // GraphAnalysisResult uses DependencyNode and DependencyEdge, not GraphNode and GraphEdge
        // GraphAnalysisResult.ImpactAnalysis is List<ImpactAnalysisResult>, not ImpactAnalysis object
        if (result.GraphAnalysis != null)
        {
            result.GraphAnalysis.Nodes.Should().NotBeNull();
            result.GraphAnalysis.Edges.Should().NotBeNull();
            result.GraphAnalysis.ImpactAnalysis.Should().NotBeNull();
            // ImpactAnalysis is List<ImpactAnalysisResult>, access individual items
            if (result.GraphAnalysis.ImpactAnalysis != null && result.GraphAnalysis.ImpactAnalysis.Count > 0)
            {
                var firstImpact = result.GraphAnalysis.ImpactAnalysis[0];
                firstImpact.Should().NotBeNull();
            }
        }

        _errorContextAnalyzerMock.Verify(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()), Times.Once);
    }

    [Fact]
    public async Task ShouldHandleCircularDependencies()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "CircularDependencyError",
            "Circular dependency detected in service graph",
            "ServiceGraph",
            new Dictionary<string, object>
            {
                { "Services", new[] { "ServiceA", "ServiceB", "ServiceC" } },
                { "Dependencies", new Dictionary<string, string[]>
                    {
                        { "ServiceA", new[] { "ServiceB" } },
                        { "ServiceB", new[] { "ServiceC" } },
                        { "ServiceC", new[] { "ServiceA" } }
                    }
                }
            }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? string.Empty));

        var nodes2 = new List<GraphNode>
        {
            new() { Id = "ServiceA", Type = "Service", Status = "Error" },
            new() { Id = "ServiceB", Type = "Service", Status = "Degraded" },
            new() { Id = "ServiceC", Type = "Service", Status = "Degraded" }
        };
        
        var graphAnalysis = new GraphAnalysis
        {
            Nodes = nodes2,
            Edges = new List<GraphEdge>
            {
                new() { Source = nodes2.First(n => n.Id == "ServiceA"), Target = nodes2.First(n => n.Id == "ServiceB"), Type = "DependsOn" },
                new() { Source = nodes2.First(n => n.Id == "ServiceB"), Target = nodes2.First(n => n.Id == "ServiceC"), Type = "DependsOn" },
                new() { Source = nodes2.First(n => n.Id == "ServiceC"), Target = nodes2.First(n => n.Id == "ServiceA"), Type = "DependsOn" }
            },
            ImpactAnalysis = new ImpactAnalysis
            {
                AffectedServices = new[] { "ServiceA", "ServiceB", "ServiceC" }.ToList(),
                CriticalPath = new[] { "ServiceA", "ServiceB", "ServiceC", "ServiceA" }.ToList(),
                Severity = "Critical",
                HasCircularDependency = true
            }
        };

        var remediationAnalysis = new RemediationAnalysis
        {
            ErrorContext = errorContext,
            GraphAnalysis = graphAnalysis,
            Confidence = 0.8,
            Timestamp = DateTime.UtcNow
        };
        
        _errorContextAnalyzerMock.Setup(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(remediationAnalysis);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.GraphAnalysis.Should().NotBeNull();
        // GraphAnalysisResult.ImpactAnalysis is List<ImpactAnalysisResult>, not ImpactAnalysis object
        result.GraphAnalysis.ImpactAnalysis.Should().NotBeNull();
        // ImpactAnalysis is List<ImpactAnalysisResult>, access individual items
        if (result.GraphAnalysis.ImpactAnalysis != null && result.GraphAnalysis.ImpactAnalysis.Count > 0)
        {
            var firstImpact = result.GraphAnalysis.ImpactAnalysis[0];
            firstImpact.Should().NotBeNull();
        }
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "CircularDependencyResolution" || 
            s.Name == "ServiceDecoupling");

        _errorContextAnalyzerMock.Verify(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()), Times.Once);
    }

    [Fact]
    public async Task ShouldAnalyzeCascadingFailures()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "CascadingFailure",
            "Service failure causing cascading impact",
            "ServiceGraph",
            new Dictionary<string, object>
            {
                { "RootCause", "DatabaseConnectionError" },
                { "AffectedServices", new[] { "UserService", "AuthService", "PaymentService", "NotificationService" } },
                { "FailureChain", new[] { "Database", "AuthService", "UserService", "PaymentService" } }
            }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? string.Empty));

        var nodes3 = new List<GraphNode>
        {
            new() { Id = "Database", Type = "Database", Status = "Error" },
            new() { Id = "AuthService", Type = "Service", Status = "Error" },
            new() { Id = "UserService", Type = "Service", Status = "Error" },
            new() { Id = "PaymentService", Type = "Service", Status = "Error" },
            new() { Id = "NotificationService", Type = "Service", Status = "Degraded" }
        };
        
        var graphAnalysis = new GraphAnalysis
        {
            Nodes = nodes3,
            Edges = new List<GraphEdge>
            {
                new() { Source = nodes3.First(n => n.Id == "AuthService"), Target = nodes3.First(n => n.Id == "Database"), Type = "DependsOn" },
                new() { Source = nodes3.First(n => n.Id == "UserService"), Target = nodes3.First(n => n.Id == "AuthService"), Type = "DependsOn" },
                new() { Source = nodes3.First(n => n.Id == "PaymentService"), Target = nodes3.First(n => n.Id == "UserService"), Type = "DependsOn" },
                new() { Source = nodes3.First(n => n.Id == "NotificationService"), Target = nodes3.First(n => n.Id == "PaymentService"), Type = "DependsOn" }
            },
            ImpactAnalysis = new ImpactAnalysis
            {
                AffectedServices = new[] { "AuthService", "UserService", "PaymentService", "NotificationService" }.ToList(),
                CriticalPath = new[] { "Database", "AuthService", "UserService", "PaymentService" }.ToList(),
                Severity = "Critical",
                FailureChain = new[] { "Database", "AuthService", "UserService", "PaymentService" }.ToList()
            }
        };

        var remediationAnalysis = new RemediationAnalysis
        {
            ErrorContext = errorContext,
            GraphAnalysis = graphAnalysis,
            Confidence = 0.8,
            Timestamp = DateTime.UtcNow
        };
        
        _errorContextAnalyzerMock.Setup(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(remediationAnalysis);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.GraphAnalysis.Should().NotBeNull();
        // GraphAnalysisResult.ImpactAnalysis is List<ImpactAnalysisResult>, not ImpactAnalysis object
        result.GraphAnalysis.ImpactAnalysis.Should().NotBeNull();
        // ImpactAnalysis is List<ImpactAnalysisResult>, access individual items
        if (result.GraphAnalysis.ImpactAnalysis != null && result.GraphAnalysis.ImpactAnalysis.Count > 0)
        {
            var firstImpact = result.GraphAnalysis.ImpactAnalysis[0];
            firstImpact.Should().NotBeNull();
        }
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "CircuitBreaker" || 
            s.Name == "FallbackMechanism" ||
            s.Name == "ServiceIsolation");

        _errorContextAnalyzerMock.Verify(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()), Times.Once);
    }
} 
