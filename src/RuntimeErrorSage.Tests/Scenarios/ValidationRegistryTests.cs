using Xunit;
using FluentAssertions;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Application.Remediation;
using RuntimeErrorSage.Application.MCP;
using RuntimeErrorSage.Application.Validation;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using RuntimeErrorSage.Tests.Helpers;

namespace RuntimeErrorSage.Tests.Scenarios;

public class ValidationRegistryTests
{
    private readonly Mock<IMCPClient> _mcpClientMock;
    private readonly Mock<IRemediationExecutor> _remediationExecutorMock;
    private readonly Mock<IValidationRegistry> _validationRegistryMock;
    private readonly RuntimeErrorSageService _service;

    public ValidationRegistryTests()
    {
        _mcpClientMock = TestHelper.CreateMCPClientMock();
        _remediationExecutorMock = TestHelper.CreateRemediationExecutorMock();
        _validationRegistryMock = new Mock<IValidationRegistry>();
        _service = new RuntimeErrorSageService(
            _mcpClientMock.Object, 
            _remediationExecutorMock.Object,
            validationRegistry: _validationRegistryMock.Object);
    }

    [Fact]
    public async Task ShouldValidateErrorContext()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "DatabaseConnectionError",
            "Failed to connect to database",
            "Database",
            new Dictionary<string, object>
            {
                { "ConnectionString", "Server=localhost;Database=testdb" },
                { "RetryCount", 3 },
                { "Timeout", 30000 }
            });

        var validationResult = new ValidationResult
        {
            IsValid = true,
            Messages = new List<string> { "Context validation successful" },
            Metadata = new Dictionary<string, object>
            {
                { "ValidationTime", 100 },
                { "ContextSize", 1024 },
                { "RequiredFields", new[] { "ErrorType", "ErrorMessage", "Source", "Timestamp" } }
            }
        };

        _validationRegistryMock.Setup(x => x.ValidateContextAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.ValidationResult.Should().NotBeNull();
        result.ValidationResult.IsValid.Should().BeTrue();
        result.ValidationResult.Messages.Should().Contain("Context validation successful");
        result.ValidationResult.Metadata.Should().HaveCount(3);

        _validationRegistryMock.Verify(x => x.ValidateContextAsync(It.IsAny<ErrorContext>()), Times.Once);
    }

    [Fact]
    public async Task ShouldHandleValidationFailure()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "InvalidErrorContext",
            "Missing required fields",
            "Validation",
            new Dictionary<string, object>
            {
                { "MissingFields", new[] { "Timestamp", "Source" } }
            });

        var validationResult = new ValidationResult
        {
            IsValid = false,
            Messages = new List<string> { "Context validation failed: Missing required fields" },
            Metadata = new Dictionary<string, object>
            {
                { "MissingFields", new[] { "Timestamp", "Source" } },
                { "ValidationTime", 100 },
                { "ContextSize", 512 }
            }
        };

        _validationRegistryMock.Setup(x => x.ValidateContextAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeFalse();
        result.ValidationResult.Should().NotBeNull();
        result.ValidationResult.IsValid.Should().BeFalse();
        result.ValidationResult.Messages.Should().Contain("Context validation failed: Missing required fields");
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "ContextValidationFix" || 
            s.Name == "RetryWithValidContext");

        _validationRegistryMock.Verify(x => x.ValidateContextAsync(It.IsAny<ErrorContext>()), Times.Once);
    }

    [Fact]
    public async Task ShouldHandleValidationTimeout()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "ValidationTimeout",
            "Context validation timed out",
            "Validation",
            new Dictionary<string, object>
            {
                { "Timeout", 30000 },
                { "RetryCount", 3 }
            });

        _validationRegistryMock.Setup(x => x.ValidateContextAsync(It.IsAny<ErrorContext>()))
            .ThrowsAsync(new ValidationTimeoutException("Context validation timed out after 30 seconds"));

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.ValidationResult.Should().BeNull();
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "RetryValidation" || 
            s.Name == "SkipValidation");

        _validationRegistryMock.Verify(x => x.ValidateContextAsync(It.IsAny<ErrorContext>()), Times.Once);
    }

    [Fact]
    public async Task ShouldHandleValidationError()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "ValidationError",
            "Validation service unavailable",
            "Validation",
            new Dictionary<string, object>
            {
                { "ErrorCode", "VAL-503" },
                { "ErrorMessage", "Service unavailable" }
            });

        _validationRegistryMock.Setup(x => x.ValidateContextAsync(It.IsAny<ErrorContext>()))
            .ThrowsAsync(new ValidationException("Validation service unavailable"));

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.ValidationResult.Should().BeNull();
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "RetryValidation" || 
            s.Name == "SkipValidation" ||
            s.Name == "SwitchToBackupValidator");

        _validationRegistryMock.Verify(x => x.ValidateContextAsync(It.IsAny<ErrorContext>()), Times.Once);
    }

    [Fact]
    public async Task ShouldHandleCustomValidationRules()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "CustomValidationError",
            "Custom validation rule violation",
            "Validation",
            new Dictionary<string, object>
            {
                { "CustomRule", "PasswordPolicy" },
                { "Violations", new[] { "Password too short", "Missing special character" } }
            });

        var validationResult = new ValidationResult
        {
            IsValid = false,
            Messages = new List<string> { "Custom validation rule violation: Password policy not met" },
            Metadata = new Dictionary<string, object>
            {
                { "Rule", "PasswordPolicy" },
                { "Violations", new[] { "Password too short", "Missing special character" } },
                { "ValidationTime", 150 },
                { "ContextSize", 768 }
            }
        };

        _validationRegistryMock.Setup(x => x.ValidateContextAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeFalse();
        result.ValidationResult.Should().NotBeNull();
        result.ValidationResult.IsValid.Should().BeFalse();
        result.ValidationResult.Messages.Should().Contain("Custom validation rule violation: Password policy not met");
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "FixPasswordPolicy" || 
            s.Name == "RetryWithValidPassword");

        _validationRegistryMock.Verify(x => x.ValidateContextAsync(It.IsAny<ErrorContext>()), Times.Once);
    }

    [Fact]
    public async Task ShouldHandleValidationCache()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "CachedValidation",
            "Using cached validation result",
            "Validation",
            new Dictionary<string, object>
            {
                { "CacheKey", "ctx-123456" },
                { "CacheTimestamp", DateTime.UtcNow.AddMinutes(-5) }
            });

        var validationResult = new ValidationResult
        {
            IsValid = true,
            Messages = new List<string> { "Using cached validation result" },
            Metadata = new Dictionary<string, object>
            {
                { "CacheKey", "ctx-123456" },
                { "CacheTimestamp", DateTime.UtcNow.AddMinutes(-5) },
                { "ValidationTime", 50 },
                { "IsFromCache", true }
            }
        };

        _validationRegistryMock.Setup(x => x.ValidateContextAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.ValidationResult.Should().NotBeNull();
        result.ValidationResult.IsValid.Should().BeTrue();
        result.ValidationResult.Messages.Should().Contain("Using cached validation result");
        result.ValidationResult.Metadata["IsFromCache"].Should().Be(true);

        _validationRegistryMock.Verify(x => x.ValidateContextAsync(It.IsAny<ErrorContext>()), Times.Once);
    }
} 
