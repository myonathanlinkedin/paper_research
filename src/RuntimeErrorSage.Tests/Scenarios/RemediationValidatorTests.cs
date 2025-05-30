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
/// Tests for remediation validator.
/// </summary>
public class RemediationValidatorTests
{
    private readonly Mock<ILogger<RemediationValidator>> _loggerMock;
    private readonly Mock<IErrorContextAnalyzer> _errorContextAnalyzerMock;
    private readonly Mock<IRemediationRegistry> _registryMock;
    private readonly RemediationValidator _validator;

    public RemediationValidatorTests()
    {
        _loggerMock = new Mock<ILogger<RemediationValidator>>();
        _errorContextAnalyzerMock = new Mock<IErrorContextAnalyzer>();
        _registryMock = new Mock<IRemediationRegistry>();
        _validator = new RemediationValidator(
            _loggerMock.Object,
            _errorContextAnalyzerMock.Object,
            _registryMock.Object);
    }

    // ... existing code ...
} 
