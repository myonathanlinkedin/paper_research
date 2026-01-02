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
using RemediationRegistry = RuntimeErrorSage.Core.Remediation.RemediationRegistry;
using RemediationSuggestion = RuntimeErrorSage.Domain.Models.Remediation.RemediationSuggestion;
using RemediationPlan = RuntimeErrorSage.Domain.Models.Remediation.RemediationPlan;

namespace RuntimeErrorSage.Tests.Scenarios;

/// <summary>
/// Tests for remediation registry scenarios.
/// </summary>
public class RemediationRegistryTests
{
    private readonly Mock<ILogger<RemediationRegistry>> _loggerMock;
    private readonly Mock<IErrorContextAnalyzer> _errorContextAnalyzerMock;
    private readonly Mock<IQwenLLMClient> _llmClientMock;
    private readonly RemediationRegistry _registry;

    public RemediationRegistryTests()
    {
        _loggerMock = new Mock<ILogger<RemediationRegistry>>();
        _errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        _llmClientMock = new Mock<IQwenLLMClient>();

        var llmClientAdapter = new Mock<RuntimeErrorSage.Application.LLM.Interfaces.ILLMClient>();
        _registry = new RemediationRegistry(
            _loggerMock.Object,
            _errorContextAnalyzerMock.Object,
            llmClientAdapter.Object);
    }

    // ... existing code ...
} 
