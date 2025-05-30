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

        _registry = new RemediationRegistry(
            _loggerMock.Object,
            _errorContextAnalyzerMock.Object,
            _llmClientMock.Object);
    }

    // ... existing code ...
} 
