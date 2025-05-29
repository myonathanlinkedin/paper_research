using Xunit;
using FluentAssertions;
using RuntimeErrorSage.Model.Analysis;
using RuntimeErrorSage.Model.Remediation;
using RuntimeErrorSage.Model.MCP;
using Moq;
using System.Threading.Tasks;
using RuntimeErrorSage.Tests.Helpers;

namespace RuntimeErrorSage.Tests.Scenarios;

public class FileSystemErrorScenarios
{
    private readonly Mock<IMCPClient> _mcpClientMock;
    private readonly Mock<IRemediationExecutor> _remediationExecutorMock;
    private readonly RuntimeErrorSageService _service;

    public FileSystemErrorScenarios()
    {
        _mcpClientMock = TestHelper.CreateMCPClientMock();
        _remediationExecutorMock = TestHelper.CreateRemediationExecutorMock();
        _service = new RuntimeErrorSageService(_mcpClientMock.Object, _remediationExecutorMock.Object);
    }

    [Theory]
    [InlineData("FileNotFound", "The file 'config.json' was not found")]
    [InlineData("DirectoryNotFound", "The directory 'logs' was not found")]
    [InlineData("AccessDenied", "Access to the file 'settings.xml' is denied")]
    [InlineData("PathTooLong", "The specified path exceeds the maximum allowed length")]
    [InlineData("InvalidPath", "The path contains invalid characters")]
    [InlineData("FileInUse", "The file 'database.db' is being used by another process")]
    [InlineData("DiskFull", "There is not enough space on the disk")]
    [InlineData("FileCorrupted", "The file 'data.bin' is corrupted")]
    [InlineData("FileLocked", "The file 'document.docx' is locked for editing")]
    [InlineData("FileExists", "The file 'output.txt' already exists")]
    [InlineData("DirectoryNotEmpty", "The directory 'temp' is not empty")]
    [InlineData("InvalidFileFormat", "The file 'image.jpg' has an invalid format")]
    [InlineData("FileTooLarge", "The file 'video.mp4' exceeds the maximum allowed size")]
    [InlineData("FileSystemReadOnly", "The file system is read-only")]
    [InlineData("FileSystemCorrupted", "The file system is corrupted")]
    [InlineData("FileSystemFull", "The file system is full")]
    [InlineData("FileSystemUnavailable", "The file system is unavailable")]
    [InlineData("FileSystemQuotaExceeded", "The file system quota has been exceeded")]
    [InlineData("FileSystemPermissionDenied", "Permission denied to access the file system")]
    [InlineData("FileSystemIOError", "An I/O error occurred while accessing the file system")]
    [InlineData("FileSystemNetworkError", "A network error occurred while accessing the file system")]
    [InlineData("FileSystemTimeout", "The file system operation timed out")]
    [InlineData("FileSystemInvalidOperation", "The file system operation is invalid")]
    [InlineData("FileSystemNotSupported", "The file system operation is not supported")]
    [InlineData("FileSystemUnknownError", "An unknown error occurred in the file system")]
    public async Task ShouldHandleFileSystemError(string errorType, string errorMessage)
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(errorType, errorMessage, "FileSystem");

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
    public async Task ShouldHandleRealWorldFileSystemScenario()
    {
        // Arrange
        var additionalContext = new Dictionary<string, object>
        {
            { "FilePath", "C:\\Data\\database.db" },
            { "FileSize", 1024 * 1024 * 100 }, // 100 MB
            { "ProcessId", 1234 },
            { "ProcessName", "sqlserver.exe" },
            { "FileHandleCount", 5 },
            { "LastAccessTime", DateTime.UtcNow.AddMinutes(-5) },
            { "FileAttributes", "Archive, Normal" }
        };

        var errorContext = TestHelper.CreateErrorContext(
            "FileInUse",
            "The file 'database.db' is being used by another process",
            "FileSystem",
            additionalContext);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.RemediationPlan.Should().NotBeNull();
        result.RemediationPlan.Strategies.Should().NotBeEmpty();
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "FileHandleRelease" || 
            s.Name == "ProcessTermination" ||
            s.Name == "FileBackup");

        _mcpClientMock.Verify(x => x.PublishContextAsync(It.IsAny<ErrorContext>()), Times.Once);
        _remediationExecutorMock.Verify(x => x.ExecuteRemediationAsync(It.IsAny<RemediationPlan>()), Times.Once);
    }
} 
