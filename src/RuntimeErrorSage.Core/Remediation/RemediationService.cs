using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Remediation.Models.Analysis;
using RuntimeErrorSage.Core.Remediation.Models.Common;
using RuntimeErrorSage.Core.Remediation.Models.Validation;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Orchestrates the remediation process for error contexts.
    /// </summary>
    public class RemediationService : IRemediationService
    {
        private readonly ILogger<RemediationService> _logger;
        private readonly IRemediationAnalyzer _analyzer;
        private readonly IRemediationExecutor _executor;
        private readonly IRemediationValidator _validator;
        private readonly IRemediationTracker _tracker;
        private readonly IRemediationMetricsCollector _metricsCollector;

        public RemediationService(
            ILogger<RemediationService> logger,
            IRemediationAnalyzer analyzer,
            IRemediationExecutor executor,
            IRemediationValidator validator,
            IRemediationTracker tracker,
            IRemediationMetricsCollector metricsCollector)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
        }

        public async Task<RemediationResult> RemediateErrorAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var remediationId = Guid.NewGuid().ToString();

            try
            {
                // Validate context
                var validationResult = await _validator.ValidateRemediationAsync(context);
                if (!validationResult.IsValid)
                {
                    await _tracker.UpdateStatusAsync(
                        remediationId,
                        RemediationStatus.Failed,
                        $"Context validation failed: {string.Join(", ", validationResult.Errors)}");

                    return new RemediationResult
                    {
                        Success = false,
                        Message = $"Context validation failed: {string.Join(", ", validationResult.Errors)}",
                        Timestamp = DateTime.UtcNow
                    };
                }

                // Update status
                await _tracker.UpdateStatusAsync(remediationId, RemediationStatus.Analyzing);

                // Analyze error
                var analysis = await _analyzer.AnalyzeErrorAsync(context);
                if (!analysis.IsValid)
                {
                    await _tracker.UpdateStatusAsync(
                        remediationId,
                        RemediationStatus.Failed,
                        $"Analysis failed: {analysis.ErrorMessage}");

                    return new RemediationResult
                    {
                        Success = false,
                        Message = $"Analysis failed: {analysis.ErrorMessage}",
                        Timestamp = DateTime.UtcNow
                    };
                }

                // Record analysis metrics
                var analysisMetrics = new RemediationMetrics
                {
                    RemediationId = remediationId,
                    ErrorType = context.ErrorType,
                    StartTime = DateTime.UtcNow,
                    Analysis = analysis
                };

                await _metricsCollector.RecordRemediationMetricsAsync(analysisMetrics);

                // Update status
                await _tracker.UpdateStatusAsync(remediationId, RemediationStatus.Executing);

                // Execute remediation
                var result = await ExecuteRemediationAsync(context, analysis, remediationId);

                // Record execution metrics
                var executionMetrics = new RemediationMetrics
                {
                    RemediationId = remediationId,
                    ErrorType = context.ErrorType,
                    EndTime = DateTime.UtcNow,
                    Success = result.Success,
                    ErrorMessage = result.Error
                };

                await _metricsCollector.RecordRemediationMetricsAsync(executionMetrics);

                // Update status
                await _tracker.UpdateStatusAsync(
                    remediationId,
                    result.Success ? RemediationStatus.Completed : RemediationStatus.Failed,
                    result.Message);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error remediating error context");
                await _tracker.UpdateStatusAsync(
                    remediationId,
                    RemediationStatus.Failed,
                    $"Remediation failed: {ex.Message}");

                return new RemediationResult
                {
                    Success = false,
                    Message = $"Remediation failed: {ex.Message}",
                    Error = ex.ToString(),
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        private async Task<RemediationResult> ExecuteRemediationAsync(
            ErrorContext context,
            RemediationAnalysis analysis,
            string remediationId)
        {
            // Get recommended strategies
            var strategies = analysis.ApplicableStrategies
                .OrderByDescending(s => s.Confidence)
                .ThenByDescending(s => s.Priority)
                .ToList();

            if (!strategies.Any())
            {
                return new RemediationResult
                {
                    Success = false,
                    Message = "No applicable strategies found",
                    Timestamp = DateTime.UtcNow
                };
            }

            // Try each strategy in order
            foreach (var strategy in strategies)
            {
                try
                {
                    // Update status
                    await _tracker.UpdateStatusAsync(
                        remediationId,
                        RemediationStatus.Executing,
                        $"Executing strategy '{strategy.StrategyName}'");

                    // Execute strategy
                    var result = await _executor.ExecuteStrategyAsync(
                        strategy.StrategyName,
                        context,
                        new Dictionary<string, string>
                        {
                            ["confidence"] = strategy.Confidence.ToString(),
                            ["reasoning"] = strategy.Reasoning
                        });

                    // Record strategy metrics
                    var strategyMetrics = new RemediationMetrics
                    {
                        RemediationId = remediationId,
                        StrategyName = strategy.StrategyName,
                        ErrorType = context.ErrorType,
                        StartTime = DateTime.UtcNow,
                        Success = result.Success,
                        ErrorMessage = result.Error,
                        Confidence = strategy.Confidence
                    };

                    await _metricsCollector.RecordRemediationMetricsAsync(strategyMetrics);

                    // If strategy succeeded, return result
                    if (result.Success)
                    {
                        return result;
                    }

                    _logger.LogWarning(
                        "Strategy {StrategyName} failed: {Message}",
                        strategy.StrategyName,
                        result.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error executing strategy {StrategyName}",
                        strategy.StrategyName);

                    // Record failure metrics
                    var failureMetrics = new RemediationMetrics
                    {
                        RemediationId = remediationId,
                        StrategyName = strategy.StrategyName,
                        ErrorType = context.ErrorType,
                        StartTime = DateTime.UtcNow,
                        Success = false,
                        ErrorMessage = ex.Message,
                        Confidence = strategy.Confidence
                    };

                    await _metricsCollector.RecordRemediationMetricsAsync(failureMetrics);
                }
            }

            // All strategies failed
            return new RemediationResult
            {
                Success = false,
                Message = "All remediation strategies failed",
                Timestamp = DateTime.UtcNow
            };
        }
    }
} 