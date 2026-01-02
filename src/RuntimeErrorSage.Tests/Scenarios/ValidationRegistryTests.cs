using Xunit;
using FluentAssertions;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Application.Validation.Interfaces;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Application.Exceptions;
using RuntimeErrorSage.Infrastructure.Services;
using RuntimeErrorSage.Application.Runtime.Interfaces;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;
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
        _service = TestHelper.CreateRuntimeErrorSageService(
            mcpClientMock: _mcpClientMock,
            remediationExecutorMock: _remediationExecutorMock,
            validationRegistryMock: _validationRegistryMock);
    }

    [Fact]
    public async Task ShouldValidateErrorContext()
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(
            "DatabaseConnectionError",
            "Failed to connect to database",
            "Database",
            new Dictionary<string, string>
            {
                { "ConnectionString", "Server=localhost;Database=testdb" },
                { "RetryCount", "3" },
                { "Timeout", "30000" }
            });

        var validationResult = new ValidationResult
        {
            IsValid = true,
            Messages = new List<string> { "Context validation successful" }
        };
        validationResult.AddMetadata("ValidationTime", 100);
        validationResult.AddMetadata("ContextSize", 1024);
        validationResult.AddMetadata("RequiredFields", new[] { "ErrorType", "ErrorMessage", "Source", "Timestamp" });

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
            new Dictionary<string, string>
            {
                { "MissingFields", "Timestamp,Source" }
            });

        var validationResult = new ValidationResult
        {
            IsValid = false,
            Messages = new List<string> { "Context validation failed: Missing required fields" }
        };
        validationResult.AddMetadata("MissingFields", new[] { "Timestamp", "Source" });
        validationResult.AddMetadata("ValidationTime", 100);
        validationResult.AddMetadata("ContextSize", 512);

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
            new Dictionary<string, string>
            {
                { "Timeout", "30000" },
                { "RetryCount", "3" }
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
            new Dictionary<string, string>
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
            new Dictionary<string, string>
            {
                { "CustomRule", "PasswordPolicy" },
                { "Violations", "Password too short,Missing special character" }
            });

        var validationResult = new ValidationResult
        {
            IsValid = false,
            Messages = new List<string> { "Custom validation rule violation: Password policy not met" }
        };
        validationResult.AddMetadata("Rule", "PasswordPolicy");
        validationResult.AddMetadata("Violations", new[] { "Password too short", "Missing special character" });
        validationResult.AddMetadata("ValidationTime", 150);
        validationResult.AddMetadata("ContextSize", 768);

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
            new Dictionary<string, string>
            {
                { "CacheKey", "ctx-123456" },
                { "CacheTimestamp", DateTime.UtcNow.AddMinutes(-5).ToString("O") }
            });

        var validationResult = new ValidationResult
        {
            IsValid = true,
            Messages = new List<string> { "Using cached validation result" }
        };
        validationResult.AddMetadata("CacheKey", "ctx-123456");
        validationResult.AddMetadata("CacheTimestamp", DateTime.UtcNow.AddMinutes(-5));
        validationResult.AddMetadata("ValidationTime", 50);
        validationResult.AddMetadata("IsFromCache", true);

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
