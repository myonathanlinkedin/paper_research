using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Remediation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Core.Remediation.Mapping;
using Xunit;
using RemediationSuggestion = RuntimeErrorSage.Domain.Models.Remediation.RemediationSuggestion;
using RemediationPlan = RuntimeErrorSage.Domain.Models.Remediation.RemediationPlan;

namespace RuntimeErrorSage.Tests.Scenarios;

/// <summary>
/// Tests for remediation service.
/// </summary>
public class RemediationServiceTests
{
    private readonly Mock<ILogger<RemediationService>> _loggerMock;
    private readonly Mock<IRemediationPlanManager> _planManagerMock;
    private readonly Mock<IRemediationExecutor> _executorMock;
    private readonly Mock<IRemediationMetricsCollector> _metricsCollectorMock;
    private readonly Mock<IRemediationSuggestionManager> _suggestionManagerMock;
    private readonly Mock<IRemediationActionManager> _actionManagerMock;
    private readonly Mock<IRemediationStrategySelector> _strategySelectorMock;
    private readonly Mock<IRemediationValidator> _validatorMock;
    private readonly Mock<IValidationRuleProvider> _validationRuleProviderMock;
    private readonly Mock<IRemediationStrategyMapper> _strategyMapperMock;
    private readonly Mock<IRemediationModelMapper> _modelMapperMock;
    private readonly RemediationService _service;

    public RemediationServiceTests()
    {
        _loggerMock = new Mock<ILogger<RemediationService>>();
        _planManagerMock = new Mock<IRemediationPlanManager>();
        _executorMock = new Mock<IRemediationExecutor>();
        _metricsCollectorMock = new Mock<IRemediationMetricsCollector>();
        _suggestionManagerMock = new Mock<IRemediationSuggestionManager>();
        _actionManagerMock = new Mock<IRemediationActionManager>();
        _strategySelectorMock = new Mock<IRemediationStrategySelector>();
        _validatorMock = new Mock<IRemediationValidator>();
        _validationRuleProviderMock = new Mock<IValidationRuleProvider>();
        _strategyMapperMock = new Mock<IRemediationStrategyMapper>();
        _modelMapperMock = new Mock<IRemediationModelMapper>();
        _service = new RemediationService(
            _loggerMock.Object,
            _planManagerMock.Object,
            _executorMock.Object,
            _metricsCollectorMock.Object,
            _suggestionManagerMock.Object,
            _actionManagerMock.Object,
            _strategySelectorMock.Object,
            _validatorMock.Object,
            _validationRuleProviderMock.Object,
            _strategyMapperMock.Object,
            _modelMapperMock.Object);
    }

    // ... existing code ...
} 
