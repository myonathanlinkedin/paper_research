using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Analysis; // For GraphAnalysisResult, RemediationAnalysis, etc.
using RuntimeErrorSage.Domain.Models.Graph;
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Remediation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RemediationValidatorOptions = RuntimeErrorSage.Application.Remediation.RemediationValidatorOptions;
using Xunit;
using RemediationSuggestion = RuntimeErrorSage.Domain.Models.Remediation.RemediationSuggestion;
using RemediationPlan = RuntimeErrorSage.Domain.Models.Remediation.RemediationPlan;

namespace RuntimeErrorSage.Tests.Scenarios;

/// <summary>
/// Tests for remediation validator.
/// </summary>
public class RemediationValidatorTests
{
    private readonly Mock<ILogger<RemediationValidator>> _loggerMock;
    private readonly Mock<IOptions<RemediationValidatorOptions>> _optionsMock;
    private readonly Mock<IErrorContextAnalyzer> _errorContextAnalyzerMock;
    private readonly Mock<IRemediationMetricsCollector> _metricsCollectorMock;
    private readonly Mock<IRemediationRegistry> _registryMock;
    private readonly Mock<ILLMClient> _llmClientMock;
    private readonly RemediationValidator _validator;

    public RemediationValidatorTests()
    {
        _loggerMock = new Mock<ILogger<RemediationValidator>>();
        _optionsMock = new Mock<IOptions<RemediationValidatorOptions>>();
        _optionsMock.Setup(x => x.Value).Returns(new RemediationValidatorOptions { IsEnabled = true });
        _errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        _metricsCollectorMock = new Mock<IRemediationMetricsCollector>();
        _registryMock = new Mock<IRemediationRegistry>();
        _llmClientMock = new Mock<ILLMClient>();
        _validator = new RemediationValidator(
            _loggerMock.Object,
            _optionsMock.Object,
            new List<IRemediationValidationRule>(),
            new List<IRemediationValidationWarning>(),
            _errorContextAnalyzerMock.Object,
            _metricsCollectorMock.Object,
            _registryMock.Object,
            _llmClientMock.Object,
            new List<string>());
    }

    // ... existing code ...
} 
