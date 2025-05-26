using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Services.Interfaces;

namespace RuntimeErrorSage.Core.Services;

/// <summary>
/// Core service that orchestrates error analysis and remediation.
/// </summary>
public class RuntimeErrorSageService : IRuntimeErrorSageService
{
    private readonly ILogger<RuntimeErrorSageService> _logger;
    private readonly IErrorAnalysisService _errorAnalysisService;
    private readonly IRemediationService _remediationService;
    private readonly IContextEnrichmentService _contextEnrichmentService;

    public RuntimeErrorSageService(
        ILogger<RuntimeErrorSageService> logger,
        IErrorAnalysisService errorAnalysisService,
        IRemediationService remediationService,
        IContextEnrichmentService contextEnrichmentService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _errorAnalysisService = errorAnalysisService ?? throw new ArgumentNullException(nameof(errorAnalysisService));
        _remediationService = remediationService ?? throw new ArgumentNullException(nameof(remediationService));
        _contextEnrichmentService = contextEnrichmentService ?? throw new ArgumentNullException(nameof(contextEnrichmentService));
    }

    /// <inheritdoc />
    public async Task<ErrorAnalysisResult> ProcessExceptionAsync(Exception exception, ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(exception);
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Processing exception of type {ExceptionType}", exception.GetType().Name);
            
            // Enrich context with runtime information
            var enrichedContext = await _contextEnrichmentService.EnrichContextAsync(context);
            
            // Analyze the exception
            return await _errorAnalysisService.AnalyzeExceptionAsync(exception, enrichedContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing exception");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<RemediationResult> ApplyRemediationAsync(ErrorAnalysisResult analysisResult)
    {
        ArgumentNullException.ThrowIfNull(analysisResult);

        try
        {
            _logger.LogInformation("Applying remediation for analysis {AnalysisId}", analysisResult.AnalysisId);
            return await _remediationService.ApplyRemediationAsync(analysisResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying remediation");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ErrorContext> EnrichContextAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Enriching context for error {ErrorId}", context.ErrorId);
            return await _contextEnrichmentService.EnrichContextAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enriching context");
            throw;
        }
    }
} 