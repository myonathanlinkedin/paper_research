using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Application.Remediation;
using RuntimeErrorSage.Application.MCP;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Execution;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Validation;
using RuntimeErrorSage.Domain.Enums;

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
        mock.Setup(x => x.ExecuteRemediationAsync(It.IsAny<ErrorAnalysisResult>(), It.IsAny<ErrorContext>()))
            .ReturnsAsync(new RemediationExecution { Success = true });
        return mock;
    }

    public static ErrorContext CreateErrorContext(
        string errorType,
        string message,
        string source,
        Dictionary<string, string>? additionalContext = null)
    {
        var error = new Error(
            type: errorType,
            message: message,
            source: source,
            stackTrace: "Test stack trace",
            metadata: additionalContext);

        return new ErrorContext(error);
    }

    public static RemediationPlan CreateRemediationPlan(
        string errorType,
        IEnumerable<IRemediationStrategy> strategies)
    {
        var context = CreateErrorContext(errorType, "Test error", "Test source");
        return new RemediationPlan(
            name: "Test plan",
            description: "Test description",
            actions: new Collection<RemediationAction>(),
            parameters: new Dictionary<string, object>(),
            estimatedDuration: TimeSpan.FromMinutes(5))
        {
            Context = context,
            Strategies = strategies.ToList(),
            Status = RemediationStatusEnum.Pending
        };
    }

    public static Mock<IRemediationStrategy> CreateRemediationStrategy(
        string name,
        string description,
        int priority = 0)
    {
        var mock = new Mock<IRemediationStrategy>();
        mock.Setup(x => x.Name).Returns(name);
        mock.Setup(x => x.Description).Returns(description);
        mock.Setup(x => x.GetPriorityAsync(It.IsAny<ErrorContext>()))
            .ReturnsAsync(priority);
        mock.Setup(x => x.CreatedAt).Returns(DateTime.UtcNow);
        mock.Setup(x => x.Status).Returns(RemediationStatusEnum.NotStarted);
        return mock;
    }

    public static RemediationExecution CreateRemediationExecution(
        bool isSuccessful = true,
        string? errorMessage = null)
    {
        var now = DateTime.UtcNow;
        return new RemediationExecution
        {
            Success = isSuccessful,
            ErrorMessage = errorMessage,
            StartTime = now,
            EndTime = now.AddSeconds(1),
            Status = isSuccessful ? RemediationStatusEnum.Success : RemediationStatusEnum.Failed
        };
    }

    public static ValidationResult CreateValidationResult(
        bool isSuccessful = true,
        string? message = null,
        Dictionary<string, object>? details = null)
    {
        var result = new ValidationResult
        {
            IsValid = isSuccessful,
            Metadata = details ?? new Dictionary<string, object>(),
            Timestamp = DateTime.UtcNow
        };
        
        if (!string.IsNullOrEmpty(message))
        {
            result.Messages.Add(message);
        }
        
        return result;
    }
} 






