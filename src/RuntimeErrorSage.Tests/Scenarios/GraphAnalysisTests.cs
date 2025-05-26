using Xunit;
using FluentAssertions;
using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Core.MCP;
using RuntimeErrorSage.Core.Graph;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using RuntimeErrorSage.Tests.Helpers;

namespace RuntimeErrorSage.Tests.Scenarios;

public class GraphAnalysisTests
{
    private readonly Mock<IMCPClient> _mcpClientMock;
    private readonly Mock<IRemediationExecutor> _remediationExecutorMock;
    private readonly Mock<IGraphAnalyzer> _graphAnalyzerMock;
    private readonly RuntimeErrorSageService _service;

    public GraphAnalysisTests()
    {
        _mcpClientMock = TestHelper.CreateMCPClientMock();
        _remediationExecutorMock = TestHelper.CreateRemediationExecutorMock();
        _graphAnalyzerMock = new Mock<IGraphAnalyzer>();
        _service = new RuntimeErrorSageService(
            _mcpClientMock.Object, 
            _remediationExecutorMock.Object,
            _graphAnalyzerMock.Object);
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
            });

        var graphAnalysis = new GraphAnalysis
        {
            Nodes = new List<GraphNode>
            {
                new() { Id = "UserService", Type = "Service", Status = "Healthy" },
                new() { Id = "AuthService", Type = "Service", Status = "Degraded" },
                new() { Id = "LoggingService", Type = "Service", Status = "Healthy" },
                new() { Id = "Database", Type = "Database", Status = "Error" }
            },
            Edges = new List<GraphEdge>
            {
                new() { Source = "UserService", Target = "Database", Type = "DependsOn" },
                new() { Source = "AuthService", Target = "Database", Type = "DependsOn" },
                new() { Source = "LoggingService", Target = "Database", Type = "DependsOn" }
            },
            ImpactAnalysis = new ImpactAnalysis
            {
                AffectedServices = new[] { "UserService", "AuthService" },
                CriticalPath = new[] { "AuthService", "Database" },
                Severity = "High"
            }
        };

        _graphAnalyzerMock.Setup(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(graphAnalysis);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.GraphAnalysis.Should().NotBeNull();
        result.GraphAnalysis.Nodes.Should().HaveCount(4);
        result.GraphAnalysis.Edges.Should().HaveCount(3);
        result.GraphAnalysis.ImpactAnalysis.Should().NotBeNull();
        result.GraphAnalysis.ImpactAnalysis.AffectedServices.Should().HaveCount(2);
        result.GraphAnalysis.ImpactAnalysis.CriticalPath.Should().HaveCount(2);
        result.GraphAnalysis.ImpactAnalysis.Severity.Should().Be("High");

        _graphAnalyzerMock.Verify(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()), Times.Once);
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
            });

        var graphAnalysis = new GraphAnalysis
        {
            Nodes = new List<GraphNode>
            {
                new() { Id = "ServiceA", Type = "Service", Status = "Error" },
                new() { Id = "ServiceB", Type = "Service", Status = "Degraded" },
                new() { Id = "ServiceC", Type = "Service", Status = "Degraded" }
            },
            Edges = new List<GraphEdge>
            {
                new() { Source = "ServiceA", Target = "ServiceB", Type = "DependsOn" },
                new() { Source = "ServiceB", Target = "ServiceC", Type = "DependsOn" },
                new() { Source = "ServiceC", Target = "ServiceA", Type = "DependsOn" }
            },
            ImpactAnalysis = new ImpactAnalysis
            {
                AffectedServices = new[] { "ServiceA", "ServiceB", "ServiceC" },
                CriticalPath = new[] { "ServiceA", "ServiceB", "ServiceC", "ServiceA" },
                Severity = "Critical",
                HasCircularDependency = true
            }
        };

        _graphAnalyzerMock.Setup(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(graphAnalysis);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.GraphAnalysis.Should().NotBeNull();
        result.GraphAnalysis.ImpactAnalysis.HasCircularDependency.Should().BeTrue();
        result.GraphAnalysis.ImpactAnalysis.Severity.Should().Be("Critical");
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "CircularDependencyResolution" || 
            s.Name == "ServiceDecoupling");

        _graphAnalyzerMock.Verify(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()), Times.Once);
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
            });

        var graphAnalysis = new GraphAnalysis
        {
            Nodes = new List<GraphNode>
            {
                new() { Id = "Database", Type = "Database", Status = "Error" },
                new() { Id = "AuthService", Type = "Service", Status = "Error" },
                new() { Id = "UserService", Type = "Service", Status = "Error" },
                new() { Id = "PaymentService", Type = "Service", Status = "Error" },
                new() { Id = "NotificationService", Type = "Service", Status = "Degraded" }
            },
            Edges = new List<GraphEdge>
            {
                new() { Source = "AuthService", Target = "Database", Type = "DependsOn" },
                new() { Source = "UserService", Target = "AuthService", Type = "DependsOn" },
                new() { Source = "PaymentService", Target = "UserService", Type = "DependsOn" },
                new() { Source = "NotificationService", Target = "PaymentService", Type = "DependsOn" }
            },
            ImpactAnalysis = new ImpactAnalysis
            {
                AffectedServices = new[] { "AuthService", "UserService", "PaymentService", "NotificationService" },
                CriticalPath = new[] { "Database", "AuthService", "UserService", "PaymentService" },
                Severity = "Critical",
                FailureChain = new[] { "Database", "AuthService", "UserService", "PaymentService" }
            }
        };

        _graphAnalyzerMock.Setup(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(graphAnalysis);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.GraphAnalysis.Should().NotBeNull();
        result.GraphAnalysis.ImpactAnalysis.FailureChain.Should().HaveCount(4);
        result.GraphAnalysis.ImpactAnalysis.Severity.Should().Be("Critical");
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "CircuitBreaker" || 
            s.Name == "FallbackMechanism" ||
            s.Name == "ServiceIsolation");

        _graphAnalyzerMock.Verify(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()), Times.Once);
    }
} 