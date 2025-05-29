using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RuntimeErrorSage.Model.Interfaces;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Graph;
using RuntimeErrorSage.Model.Models.Remediation;
using RuntimeErrorSage.Model.Remediation;
using RuntimeErrorSage.Model.Remediation.Interfaces;
using Xunit;

namespace RuntimeErrorSage.Tests.Scenarios;

/// <summary>
/// Tests for remediation execution scenarios.
/// </summary>
public class RemediationExecutorTests
{
    private readonly Mock<ILogger<RemediationExecutor>> _loggerMock;
    private readonly Mock<IErrorContextAnalyzer> _errorContextAnalyzerMock;
    private readonly Mock<IRemediationValidator> _validatorMock;
    private readonly Mock<IRemediationMetricsCollector> _metricsCollectorMock;
    private readonly Mock<IRemediationRegistry> _registryMock;
    private readonly Mock<IQwenLLMClient> _llmClientMock;
    private readonly RemediationExecutor _executor;

    public RemediationExecutorTests()
    {
        _loggerMock = new Mock<ILogger<RemediationExecutor>>();
        _errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        _validatorMock = new Mock<IRemediationValidator>();
        _metricsCollectorMock = new Mock<IRemediationMetricsCollector>();
        _registryMock = new Mock<IRemediationRegistry>();
        _llmClientMock = new Mock<IQwenLLMClient>();

        _executor = new RemediationExecutor(
            _loggerMock.Object,
            _errorContextAnalyzerMock.Object,
            _validatorMock.Object,
            _metricsCollectorMock.Object,
            _registryMock.Object,
            _llmClientMock.Object);
    }

    // ... existing code ...
} 
