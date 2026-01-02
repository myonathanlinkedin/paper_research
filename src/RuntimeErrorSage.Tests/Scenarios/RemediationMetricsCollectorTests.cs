using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Remediation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Application.Options;
using Xunit;
using RemediationSuggestion = RuntimeErrorSage.Domain.Models.Remediation.RemediationSuggestion;
using RemediationPlan = RuntimeErrorSage.Domain.Models.Remediation.RemediationPlan;

namespace RuntimeErrorSage.Tests.Scenarios;

/// <summary>
/// Tests for remediation metrics collector.
/// </summary>
public class RemediationMetricsCollectorTests
{
    private readonly Mock<ILogger<RemediationMetricsCollector>> _loggerMock;
    private readonly Mock<IOptions<RemediationMetricsCollectorOptions>> _optionsMock;
    private readonly Mock<IErrorContextAnalyzer> _errorContextAnalyzerMock;
    private readonly Mock<IRemediationRegistry> _registryMock;
    private readonly Mock<ILLMClient> _llmClientMock;
    private readonly RemediationMetricsCollector _collector;

    public RemediationMetricsCollectorTests()
    {
        _loggerMock = new Mock<ILogger<RemediationMetricsCollector>>();
        _optionsMock = new Mock<IOptions<RemediationMetricsCollectorOptions>>();
        _optionsMock.Setup(x => x.Value).Returns(new RemediationMetricsCollectorOptions
        {
            MetricThresholds = new Dictionary<string, double>(),
            EnableDetailedMetrics = true,
            CollectionTimeout = TimeSpan.FromSeconds(30),
            CollectionInterval = TimeSpan.FromSeconds(5)
        });
        _errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        _registryMock = new Mock<IRemediationRegistry>();
        _llmClientMock = new Mock<ILLMClient>();

        _collector = new RemediationMetricsCollector(
            _loggerMock.Object,
            _optionsMock.Object,
            _errorContextAnalyzerMock.Object,
            _registryMock.Object,
            _llmClientMock.Object);
    }

    // ... existing code ...
} 
