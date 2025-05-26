using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Remediation;

/// <summary>
/// Provides remediation strategies based on error context analysis.
/// </summary>
public class RemediationStrategyProvider : IRemediationStrategyProvider
{
    private readonly ILogger<RemediationStrategyProvider> _logger;
    private readonly IErrorContextAnalyzer _errorContextAnalyzer;
    private readonly IRemediationRegistry _registry;
    private readonly IRemediationValidator _validator;
    private readonly IRemediationMetricsCollector _metricsCollector;
    private readonly IQwenLLMClient _llmClient;

    public RemediationStrategyProvider(
        ILogger<RemediationStrategyProvider> logger,
        IErrorContextAnalyzer errorContextAnalyzer,
        IRemediationRegistry registry,
        IRemediationValidator validator,
        IRemediationMetricsCollector metricsCollector,
        IQwenLLMClient llmClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _errorContextAnalyzer = errorContextAnalyzer ?? throw new ArgumentNullException(nameof(errorContextAnalyzer));
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
        _llmClient = llmClient ?? throw new ArgumentNullException(nameof(llmClient));
    }

    // ... existing code ...
} 