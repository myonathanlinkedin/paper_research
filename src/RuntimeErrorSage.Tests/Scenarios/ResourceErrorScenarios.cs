using Xunit;
using FluentAssertions;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Application.Remediation;
using RuntimeErrorSage.Application.MCP;
using Moq;
using System.Threading.Tasks;
using RuntimeErrorSage.Tests.Helpers;

namespace RuntimeErrorSage.Tests.Scenarios;

public class ResourceErrorScenarios
{
    private readonly Mock<IMCPClient> _mcpClientMock;
    private readonly Mock<IRemediationExecutor> _remediationExecutorMock;
    private readonly RuntimeErrorSageService _service;

    public ResourceErrorScenarios()
    {
        _mcpClientMock = TestHelper.CreateMCPClientMock();
        _remediationExecutorMock = TestHelper.CreateRemediationExecutorMock();
        _service = new RuntimeErrorSageService(_mcpClientMock.Object, _remediationExecutorMock.Object);
    }

    [Theory]
    [InlineData("MemoryExhausted", "The process has exhausted available memory")]
    [InlineData("CPUExhausted", "The process has exhausted available CPU resources")]
    [InlineData("DiskSpaceExhausted", "The process has exhausted available disk space")]
    [InlineData("ThreadPoolExhausted", "The thread pool has been exhausted")]
    [InlineData("ConnectionPoolExhausted", "The connection pool has been exhausted")]
    [InlineData("SocketExhausted", "The socket pool has been exhausted")]
    [InlineData("HandleExhausted", "The handle pool has been exhausted")]
    [InlineData("PortExhausted", "The port pool has been exhausted")]
    [InlineData("ProcessQuotaExceeded", "The process quota has been exceeded")]
    [InlineData("ThreadQuotaExceeded", "The thread quota has been exceeded")]
    [InlineData("MemoryQuotaExceeded", "The memory quota has been exceeded")]
    [InlineData("CPUQuotaExceeded", "The CPU quota has been exceeded")]
    [InlineData("DiskQuotaExceeded", "The disk quota has been exceeded")]
    [InlineData("NetworkQuotaExceeded", "The network quota has been exceeded")]
    [InlineData("IOQuotaExceeded", "The I/O quota has been exceeded")]
    [InlineData("ResourceLeak", "A resource leak has been detected")]
    [InlineData("ResourceDeadlock", "A resource deadlock has been detected")]
    [InlineData("ResourceTimeout", "A resource operation has timed out")]
    [InlineData("ResourceUnavailable", "A required resource is unavailable")]
    [InlineData("ResourceCorrupted", "A resource has been corrupted")]
    [InlineData("ResourceInconsistent", "A resource is in an inconsistent state")]
    [InlineData("ResourcePermissionDenied", "Permission to access a resource has been denied")]
    [InlineData("ResourceLocked", "A resource is locked by another process")]
    [InlineData("ResourceBusy", "A resource is busy")]
    [InlineData("ResourceUnknown", "An unknown resource error has occurred")]
    public async Task ShouldHandleResourceError(string errorType, string errorMessage)
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(errorType, errorMessage, "Resource");

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.RemediationPlan.Should().NotBeNull();
        result.RemediationPlan.Strategies.Should().NotBeEmpty();

        _mcpClientMock.Verify(x => x.PublishContextAsync(It.IsAny<ErrorContext>()), Times.Once);
        _remediationExecutorMock.Verify(x => x.ExecuteRemediationAsync(It.IsAny<RemediationPlan>()), Times.Once);
    }

    [Fact]
    public async Task ShouldHandleRealWorldResourceScenario()
    {
        // Arrange
        var additionalContext = new Dictionary<string, object>
        {
            { "ProcessId", 1234 },
            { "ProcessName", "app.exe" },
            { "MemoryUsage", 1024 * 1024 * 1024 * 2 }, // 2 GB
            { "MemoryLimit", 1024 * 1024 * 1024 }, // 1 GB
            { "CPUUsage", 95.5 },
            { "ThreadCount", 100 },
            { "HandleCount", 1000 },
            { "GCPressure", "High" },
            { "MemoryPressure", "Critical" },
            { "SystemMemoryAvailable", 1024 * 1024 * 512 }, // 512 MB
            { "SystemMemoryTotal", 1024 * 1024 * 1024 * 8 }, // 8 GB
            { "SystemMemoryUsed", 1024 * 1024 * 1024 * 7.5 } // 7.5 GB
        };

        var errorContext = TestHelper.CreateErrorContext(
            "MemoryExhausted",
            "The process has exhausted available memory",
            "Resource",
            additionalContext);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.RemediationPlan.Should().NotBeNull();
        result.RemediationPlan.Strategies.Should().NotBeEmpty();
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "MemoryOptimization" || 
            s.Name == "GarbageCollection" ||
            s.Name == "ResourceCleanup");

        _mcpClientMock.Verify(x => x.PublishContextAsync(It.IsAny<ErrorContext>()), Times.Once);
        _remediationExecutorMock.Verify(x => x.ExecuteRemediationAsync(It.IsAny<RemediationPlan>()), Times.Once);
    }
} 
