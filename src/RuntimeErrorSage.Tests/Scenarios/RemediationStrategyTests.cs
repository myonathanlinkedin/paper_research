using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Remediation;
using RuntimeErrorSage.Core.Remediation.Base;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using Xunit;

namespace RuntimeErrorSage.Tests.Scenarios;

/// <summary>
/// Tests for remediation strategies.
/// </summary>
public class RemediationStrategyTests
{
    private readonly Mock<ILogger<RemediationStrategy>> _loggerMock;
    private readonly Mock<IErrorContextAnalyzer> _errorContextAnalyzerMock;
    private readonly Mock<IRemediationRegistry> _registryMock;
    private readonly Mock<IRemediationValidator> _validatorMock;
    private readonly Mock<IRemediationMetricsCollector> _metricsCollectorMock;
    private readonly Mock<IQwenLLMClient> _llmClientMock;
    private readonly TestRemediationStrategy _strategy;

    public RemediationStrategyTests()
    {
        _loggerMock = new Mock<ILogger<RemediationStrategy>>();
        _errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        _registryMock = new Mock<IRemediationRegistry>();
        _validatorMock = new Mock<IRemediationValidator>();
        _metricsCollectorMock = new Mock<IRemediationMetricsCollector>();
        _llmClientMock = new Mock<IQwenLLMClient>();

        _strategy = new TestRemediationStrategy(_loggerMock.Object);
    }

    private class TestRemediationStrategy : RemediationStrategy
    {
        public override string Name { get; set; } = "Test Strategy";
        public override string Description { get; set; } = "Test remediation strategy for unit tests";

        public TestRemediationStrategy(Microsoft.Extensions.Logging.ILogger<RemediationStrategy> logger) 
            : base(logger)
        {
        }
    }

    // ... existing code ...
} 
