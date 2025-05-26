using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Core.MCP;
using Moq;
using System.Collections.Generic;

namespace RuntimeErrorSage.Tests.Helpers;

public static class TestHelper
{
    public static Mock<IMCPClient> CreateMCPClientMock()
    {
        var mock = new Mock<IMCPClient>();
        mock.Setup(x => x.PublishContextAsync(It.IsAny<ErrorContext>()))
            .Returns(Task.CompletedTask);
        return mock;
    }

    public static Mock<IRemediationExecutor> CreateRemediationExecutorMock()
    {
        var mock = new Mock<IRemediationExecutor>();
        mock.Setup(x => x.ExecuteRemediationAsync(It.IsAny<RemediationPlan>()))
            .ReturnsAsync(new RemediationExecution { IsSuccessful = true });
        return mock;
    }

    public static ErrorContext CreateErrorContext(
        string errorType,
        string errorMessage,
        string source,
        Dictionary<string, object>? additionalContext = null)
    {
        return new ErrorContext
        {
            ErrorType = errorType,
            ErrorMessage = errorMessage,
            Source = source,
            Timestamp = DateTime.UtcNow,
            AdditionalContext = additionalContext ?? new Dictionary<string, object>()
        };
    }

    public static RemediationPlan CreateRemediationPlan(
        string errorType,
        IEnumerable<RemediationStrategy> strategies)
    {
        return new RemediationPlan
        {
            ErrorType = errorType,
            Strategies = strategies.ToList(),
            CreatedAt = DateTime.UtcNow,
            Status = RemediationStatus.Pending
        };
    }

    public static RemediationStrategy CreateRemediationStrategy(
        string name,
        string description,
        int priority = 0)
    {
        return new RemediationStrategy
        {
            Name = name,
            Description = description,
            Priority = priority,
            CreatedAt = DateTime.UtcNow,
            Status = RemediationStrategyStatus.Created
        };
    }

    public static RemediationExecution CreateRemediationExecution(
        bool isSuccessful = true,
        string? errorMessage = null)
    {
        return new RemediationExecution
        {
            IsSuccessful = isSuccessful,
            ErrorMessage = errorMessage,
            StartedAt = DateTime.UtcNow,
            CompletedAt = DateTime.UtcNow.AddSeconds(1),
            Status = isSuccessful ? RemediationActionStatus.Completed : RemediationActionStatus.Failed
        };
    }

    public static ValidationResult CreateValidationResult(
        bool isSuccessful = true,
        string? message = null,
        Dictionary<string, object>? details = null)
    {
        return new ValidationResult
        {
            IsSuccessful = isSuccessful,
            Message = message,
            Details = details ?? new Dictionary<string, object>(),
            Timestamp = DateTime.UtcNow,
            DurationMs = 100,
            IsFromCache = false
        };
    }
} 