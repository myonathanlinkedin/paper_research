using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Remediation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Application.LLM.Interfaces;
using Xunit;
using RemediationSuggestion = RuntimeErrorSage.Domain.Models.Remediation.RemediationSuggestion;
using RemediationPlan = RuntimeErrorSage.Domain.Models.Remediation.RemediationPlan;

namespace RuntimeErrorSage.Tests.Scenarios;

/// <summary>
/// Tests for remediation strategy provider scenarios.
/// </summary>
public class RemediationStrategyProviderTests
{
    private readonly Mock<ILogger<RemediationStrategyProvider>> _loggerMock;
    private readonly Mock<IErrorContextAnalyzer> _errorContextAnalyzerMock;
    private readonly Mock<IRemediationRegistry> _registryMock;
    private readonly Mock<IRemediationValidator> _validatorMock;
    private readonly Mock<IRemediationMetricsCollector> _metricsCollectorMock;
    private readonly Mock<IQwenLLMClient> _llmClientMock;
    private readonly RemediationStrategyProvider _provider;

    public RemediationStrategyProviderTests()
    {
        _loggerMock = new Mock<ILogger<RemediationStrategyProvider>>();
        _errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        _registryMock = new Mock<IRemediationRegistry>();
        _validatorMock = new Mock<IRemediationValidator>();
        _metricsCollectorMock = new Mock<IRemediationMetricsCollector>();
        _llmClientMock = new Mock<IQwenLLMClient>();

        var llmClientAdapter = new Mock<RuntimeErrorSage.Application.LLM.Interfaces.ILLMClient>();
        _provider = new RemediationStrategyProvider(
            _loggerMock.Object,
            _errorContextAnalyzerMock.Object,
            _registryMock.Object,
            _validatorMock.Object,
            _metricsCollectorMock.Object,
            llmClientAdapter.Object);
    }

    // ... existing code ...
} 
