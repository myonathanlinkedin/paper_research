using Xunit;
using FluentAssertions;
using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Core.MCP;
using Moq;
using System.Threading.Tasks;
using RuntimeErrorSage.Tests.Helpers;

namespace RuntimeErrorSage.Tests.Scenarios;

public class DatabaseErrorScenarios
{
    private readonly Mock<IMCPClient> _mcpClientMock;
    private readonly Mock<IRemediationExecutor> _remediationExecutorMock;
    private readonly RuntimeErrorSageService _service;

    public DatabaseErrorScenarios()
    {
        _mcpClientMock = TestHelper.CreateMCPClientMock();
        _remediationExecutorMock = TestHelper.CreateRemediationExecutorMock();
        _service = new RuntimeErrorSageService(_mcpClientMock.Object, _remediationExecutorMock.Object);
    }

    [Theory]
    [InlineData("ConnectionTimeout", "Database connection timed out after 30 seconds")]
    [InlineData("ConnectionRefused", "Unable to connect to database server")]
    [InlineData("InvalidCredentials", "Authentication failed for user 'dbuser'")]
    [InlineData("DatabaseNotFound", "Database 'testdb' does not exist")]
    [InlineData("TableNotFound", "Table 'users' does not exist")]
    [InlineData("ColumnNotFound", "Column 'email' does not exist in table 'users'")]
    [InlineData("DuplicateKey", "Violation of PRIMARY KEY constraint")]
    [InlineData("ForeignKeyViolation", "Violation of FOREIGN KEY constraint")]
    [InlineData("CheckConstraintViolation", "Violation of CHECK constraint")]
    [InlineData("Deadlock", "Transaction deadlock detected")]
    [InlineData("LockTimeout", "Lock request time out period exceeded")]
    [InlineData("TransactionAborted", "Transaction was aborted")]
    [InlineData("QueryTimeout", "Query execution timed out")]
    [InlineData("InvalidQuery", "Syntax error in SQL statement")]
    [InlineData("PermissionDenied", "Permission denied for table 'users'")]
    [InlineData("DatabaseFull", "Database disk space is full")]
    [InlineData("ConnectionPoolExhausted", "Connection pool exhausted")]
    [InlineData("InvalidDataType", "Invalid data type for column 'age'")]
    [InlineData("UniqueConstraintViolation", "Violation of UNIQUE constraint")]
    [InlineData("IndexCorruption", "Index 'idx_users_email' is corrupted")]
    [InlineData("DatabaseCorruption", "Database file is corrupted")]
    [InlineData("ReplicationLag", "Replication lag exceeds threshold")]
    [InlineData("BackupFailure", "Database backup failed")]
    [InlineData("RestoreFailure", "Database restore failed")]
    [InlineData("SchemaVersionMismatch", "Database schema version mismatch")]
    public async Task ShouldHandleDatabaseError(string errorType, string errorMessage)
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(errorType, errorMessage, "Database");

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
    public async Task ShouldHandleRealWorldDatabaseScenario()
    {
        // Arrange
        var additionalContext = new Dictionary<string, object>
        {
            { "ConnectionString", "Server=localhost;Database=testdb;User=dbuser;Password=****" },
            { "Query", "SELECT * FROM users WHERE email = @email" },
            { "Parameters", new { email = "test@example.com" } },
            { "RetryCount", 3 },
            { "ConnectionPoolSize", 100 },
            { "ActiveConnections", 95 }
        };

        var errorContext = TestHelper.CreateErrorContext(
            "ConnectionTimeout",
            "Database connection timed out after 30 seconds",
            "Database",
            additionalContext);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.RemediationPlan.Should().NotBeNull();
        result.RemediationPlan.Strategies.Should().NotBeEmpty();
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "ConnectionPoolOptimization" || 
            s.Name == "QueryOptimization" ||
            s.Name == "ConnectionRetry");

        _mcpClientMock.Verify(x => x.PublishContextAsync(It.IsAny<ErrorContext>()), Times.Once);
        _remediationExecutorMock.Verify(x => x.ExecuteRemediationAsync(It.IsAny<RemediationPlan>()), Times.Once);
    }
} 
