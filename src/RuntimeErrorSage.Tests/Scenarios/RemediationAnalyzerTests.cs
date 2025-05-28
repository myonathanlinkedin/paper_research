using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using Xunit;

namespace RuntimeErrorSage.Tests.Scenarios;

/// <summary>
/// Tests for remediation analysis scenarios.
/// </summary>
public class RemediationAnalyzerTests
{
    private readonly Mock<ILogger<RemediationAnalyzer>> _loggerMock;
    private readonly Mock<IErrorContextAnalyzer> _errorContextAnalyzerMock;
    private readonly Mock<IRemediationStrategyProvider> _strategyProviderMock;
    private readonly Mock<IRemediationValidator> _validatorMock;
    private readonly Mock<IRemediationMetricsCollector> _metricsCollectorMock;
    private readonly Mock<IRemediationRegistry> _registryMock;
    private readonly Mock<IQwenLLMClient> _llmClientMock;
    private readonly RemediationAnalyzer _analyzer;

    public RemediationAnalyzerTests()
    {
        _loggerMock = new Mock<ILogger<RemediationAnalyzer>>();
        _errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        _strategyProviderMock = new Mock<IRemediationStrategyProvider>();
        _validatorMock = new Mock<IRemediationValidator>();
        _metricsCollectorMock = new Mock<IRemediationMetricsCollector>();
        _registryMock = new Mock<IRemediationRegistry>();
        _llmClientMock = new Mock<IQwenLLMClient>();

        _analyzer = new RemediationAnalyzer(
            _loggerMock.Object,
            _errorContextAnalyzerMock.Object,
            _strategyProviderMock.Object,
            _validatorMock.Object,
            _metricsCollectorMock.Object,
            _registryMock.Object,
            _llmClientMock.Object);
    }

    // ... existing code ...
} 
