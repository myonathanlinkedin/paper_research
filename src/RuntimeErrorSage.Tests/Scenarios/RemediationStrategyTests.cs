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
/// Tests for remediation strategies.
/// </summary>
public class RemediationStrategyTests
{
    private readonly Mock<ILogger<RemediationStrategyModel>> _loggerMock;
    private readonly Mock<IErrorContextAnalyzer> _errorContextAnalyzerMock;
    private readonly Mock<IRemediationRegistry> _registryMock;
    private readonly Mock<IRemediationValidator> _validatorMock;
    private readonly Mock<IRemediationMetricsCollector> _metricsCollectorMock;
    private readonly Mock<IQwenLLMClient> _llmClientMock;
    private readonly RemediationStrategyModel _strategy;

    public RemediationStrategyTests()
    {
        _loggerMock = new Mock<ILogger<RemediationStrategyModel>>();
        _errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        _registryMock = new Mock<IRemediationRegistry>();
        _validatorMock = new Mock<IRemediationValidator>();
        _metricsCollectorMock = new Mock<IRemediationMetricsCollector>();
        _llmClientMock = new Mock<IQwenLLMClient>();

        _strategy = new RemediationStrategy(
            _loggerMock.Object,
            _errorContextAnalyzerMock.Object,
            _registryMock.Object,
            _llmClientMock.Object);
    }

    // ... existing code ...
} 
