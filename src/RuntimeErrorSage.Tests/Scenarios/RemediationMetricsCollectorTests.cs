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
/// Tests for remediation metrics collector.
/// </summary>
public class RemediationMetricsCollectorTests
{
    private readonly Mock<ILogger<RemediationMetricsCollector>> _loggerMock;
    private readonly Mock<IErrorContextAnalyzer> _errorContextAnalyzerMock;
    private readonly Mock<IRemediationRegistry> _registryMock;
    private readonly Mock<IQwenLLMClient> _llmClientMock;
    private readonly RemediationMetricsCollector _collector;

    public RemediationMetricsCollectorTests()
    {
        _loggerMock = new Mock<ILogger<RemediationMetricsCollector>>();
        _errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        _registryMock = new Mock<IRemediationRegistry>();
        _llmClientMock = new Mock<IQwenLLMClient>();

        _collector = new RemediationMetricsCollector(
            _loggerMock.Object,
            _errorContextAnalyzerMock.Object,
            _registryMock.Object,
            _llmClientMock.Object);
    }

    // ... existing code ...
} 
