using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Application.Runtime.Interfaces;
using RuntimeErrorSage.Application.Services.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Application.Graph.Interfaces;
using RuntimeErrorSage.Application.Protocols;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Execution;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;

namespace RuntimeErrorSage.Tests.Helpers;

public static class TestHelper
{
    public static RuntimeErrorSageService CreateRuntimeErrorSageService(
        Mock<IMCPClient> mcpClientMock = null,
        Mock<IRemediationExecutor> remediationExecutorMock = null,
        Mock<IValidationRegistry> validationRegistryMock = null,
        Mock<IQwenLLMClient> llmClientMock = null)
    {
        var loggerMock = new Mock<ILogger<RuntimeErrorSageService>>();
        var errorAnalysisServiceMock = new Mock<IErrorAnalysisService>();
        var remediationServiceMock = new Mock<IRemediationService>();
        var contextEnrichmentServiceMock = new Mock<IContextEnrichmentService>();
        var validationRegistry = validationRegistryMock?.Object ?? new Mock<IValidationRegistry>().Object;
        var errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        var remediationAnalyzerMock = new Mock<IRemediationAnalyzer>();
        var remediationValidatorMock = new Mock<IRemediationValidator>();
        var metricsCollectorMock = new Mock<IRemediationMetricsCollector>();
        var mcp = new Mock<IModelContextProtocol>().Object;
        var llmClient = llmClientMock?.Object ?? new Mock<IQwenLLMClient>().Object;
        var errorAnalyzerMock = new Mock<IErrorAnalyzer>();
        var graphAnalyzerMock = new Mock<IDependencyGraphAnalyzer>();
        var modelContextMock = new Mock<IModelContextProtocol>();

        return new RuntimeErrorSageService(
            loggerMock.Object,
            errorAnalysisServiceMock.Object,
            remediationServiceMock.Object,
            contextEnrichmentServiceMock.Object,
            validationRegistry,
            errorContextAnalyzerMock.Object,
            remediationAnalyzerMock.Object,
            remediationValidatorMock.Object,
            metricsCollectorMock.Object,
            mcp,
            llmClient,
            errorAnalyzerMock.Object,
            graphAnalyzerMock.Object,
            modelContextMock.Object);
    }
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
        mock.Setup(x => x.ExecuteRemediationAsync(It.IsAny<RemediationPlan>(), It.IsAny<ErrorContext>()))
            .ReturnsAsync(new RemediationResult { Success = true, Status = RemediationStatusEnum.Success });
        return mock;
    }

    public static ErrorContext CreateErrorContext(
        string errorType,
        string message,
        string source,
        Dictionary<string, string>? additionalContext = null)
    {
        var error = new RuntimeError(
            message: message,
            errorType: errorType,
            source: source,
            stackTrace: "Test stack trace"
        );

        var context = new ErrorContext(
            error: error,
            context: source,
            timestamp: DateTime.UtcNow
        );

        if (additionalContext != null)
        {
            foreach (var kvp in additionalContext)
            {
                context.AddMetadata(kvp.Key, kvp.Value);
            }
        }

        return context;
    }

    public static RemediationPlan CreateRemediationPlan(
        string errorType,
        IEnumerable<IRemediationStrategy> strategies)
    {
        var context = CreateErrorContext(errorType, "Test error", "Test source");
        
        // Convert Application.IRemediationStrategy to Domain.IRemediationStrategy using adapter
        var domainStrategies = strategies
            .Select(s => RuntimeErrorSage.Core.Remediation.RemediationStrategyAdapterExtensions.ToDomainStrategy(s))
            .ToList();
            
        return new RemediationPlan(
            name: "Test plan",
            description: "Test description",
            actions: new List<RemediationAction>(),
            parameters: new Dictionary<string, object>(),
            estimatedDuration: TimeSpan.FromMinutes(5))
        {
            Context = context,
            Strategies = domainStrategies,
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
        mock.Setup(x => x.Priority).Returns((RemediationPriority)priority);
        mock.Setup(x => x.CreatedAt).Returns(DateTime.UtcNow);
        // Note: Application.IRemediationStrategy does not have GetPriorityAsync or Status
        // These are only in Domain.IRemediationStrategy
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
            Timestamp = DateTime.UtcNow
        };
        
        if (details != null)
        {
            foreach (var kvp in details)
            {
                result.AddMetadata(kvp.Key, kvp.Value);
            }
        }
        
        if (!string.IsNullOrEmpty(message))
        {
            result.Messages.Add(message);
        }
        
        return result;
    }
} 
