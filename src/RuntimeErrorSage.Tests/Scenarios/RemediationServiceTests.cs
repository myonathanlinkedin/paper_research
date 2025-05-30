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
using Xunit;

namespace RuntimeErrorSage.Tests.Scenarios;

/// <summary>
/// Tests for remediation service.
/// </summary>
public class RemediationServiceTests
{
    private readonly Mock<ILogger<RemediationService>> _loggerMock;
    private readonly Mock<IErrorContextAnalyzer> _errorContextAnalyzerMock;
    private readonly Mock<IRemediationRegistry> _registryMock;
    private readonly Mock<IRemediationExecutor> _executorMock;
    private readonly Mock<IRemediationValidator> _validatorMock;
    private readonly Mock<IRemediationMetricsCollector> _metricsCollectorMock;
    private readonly RemediationService _service;

    public RemediationServiceTests()
    {
        _loggerMock = new Mock<ILogger<RemediationService>>();
        _errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        _registryMock = new Mock<IRemediationRegistry>();
        _executorMock = new Mock<IRemediationExecutor>();
        _validatorMock = new Mock<IRemediationValidator>();
        _metricsCollectorMock = new Mock<IRemediationMetricsCollector>();
        _service = new RemediationService(
            _loggerMock.Object,
            _errorContextAnalyzerMock.Object,
            _registryMock.Object,
            _executorMock.Object,
            _validatorMock.Object,
            _metricsCollectorMock.Object);
    }

    // ... existing code ...
} 
