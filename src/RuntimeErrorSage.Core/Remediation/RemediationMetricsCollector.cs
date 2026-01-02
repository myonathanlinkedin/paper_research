using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Common;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Options;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using RuntimeErrorSage.Domain.Models.Metrics;
using RuntimeErrorSage.Domain.Enums;
using ValidationError = RuntimeErrorSage.Domain.Models.Validation.ValidationError;
using ValidationResult = RuntimeErrorSage.Domain.Models.Validation.ValidationResult;
using ValidationWarning = RuntimeErrorSage.Domain.Models.Validation.ValidationWarning;
using ValidationSeverity = RuntimeErrorSage.Domain.Enums.ValidationSeverity;
using System;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Linq;

namespace RuntimeErrorSage.Core.Remediation;

/// <summary>
/// Collects and aggregates remediation metrics.
/// </summary>
public sealed class RemediationMetricsCollector : IRemediationMetricsCollector, IDisposable
{
    private readonly ILogger<RemediationMetricsCollector> _logger;
    private readonly RemediationMetricsCollectorOptions _options;
    private readonly Process _currentProcess;
    private readonly Timer _collectionTimer;
    private readonly ConcurrentDictionary<string, List<RemediationMetrics>> _metricsHistory;
    private readonly ConcurrentDictionary<string, List<StepMetrics>> _stepMetricsHistory;
    private readonly ConcurrentDictionary<string, RemediationResult> _remediationResults;
    private readonly ConcurrentDictionary<string, RemediationActionResult> _actionResults;
    private readonly object _lock = new();
    private readonly CancellationTokenSource _cts = new();
    private bool _disposed;
    private readonly IErrorContextAnalyzer _errorContextAnalyzer;
    private readonly IRemediationRegistry _registry;
    private readonly ILLMClient _llmClient;
    private readonly Dictionary<string, EventId> _eventIds;
    private readonly Func<List<RemediationMetrics>> _remediationMetricsListFactory;
    private readonly Func<List<StepMetrics>> _stepMetricsListFactory;
    private readonly Func<List<Domain.Models.Remediation.MetricValue>> _metricValueListFactory;
    private readonly Func<List<ValidationWarning>> _validationWarningListFactory;

    public RemediationMetricsCollector(
        ILogger<RemediationMetricsCollector> logger,
        IOptions<RemediationMetricsCollectorOptions> options,
        IErrorContextAnalyzer errorContextAnalyzer,
        IRemediationRegistry registry,
        ILLMClient llmClient,
        ConcurrentDictionary<string, List<RemediationMetrics>>? metricsHistory = null,
        ConcurrentDictionary<string, List<StepMetrics>>? stepMetricsHistory = null,
        ConcurrentDictionary<string, RemediationResult>? remediationResults = null,
        ConcurrentDictionary<string, RemediationActionResult>? actionResults = null,
        Dictionary<string, EventId>? eventIds = null,
        Func<List<RemediationMetrics>>? remediationMetricsListFactory = null,
        Func<List<StepMetrics>>? stepMetricsListFactory = null,
        Func<List<Domain.Models.Remediation.MetricValue>>? metricValueListFactory = null,
        Func<List<ValidationWarning>>? validationWarningListFactory = null)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(errorContextAnalyzer);
        ArgumentNullException.ThrowIfNull(registry);
        ArgumentNullException.ThrowIfNull(llmClient);

        _logger = logger;
        _options = options.Value;
        _currentProcess = Process.GetCurrentProcess();
        _errorContextAnalyzer = errorContextAnalyzer;
        _registry = registry;
        _llmClient = llmClient;
        _metricsHistory = metricsHistory ?? new ConcurrentDictionary<string, List<RemediationMetrics>>();
        _stepMetricsHistory = stepMetricsHistory ?? new ConcurrentDictionary<string, List<StepMetrics>>();
        _remediationResults = remediationResults ?? new ConcurrentDictionary<string, RemediationResult>();
        _actionResults = actionResults ?? new ConcurrentDictionary<string, RemediationActionResult>();
        _eventIds = eventIds ?? new Dictionary<string, EventId>
        {
            { nameof(CollectMetricsAsync), new EventId(1, nameof(CollectMetricsAsync)) },
            { nameof(RecordMetricAsync), new EventId(2, nameof(RecordMetricAsync)) },
            { nameof(GetMetricsHistoryAsync), new EventId(3, nameof(GetMetricsHistoryAsync)) },
            { nameof(RecordExecutionAsync), new EventId(4, nameof(RecordExecutionAsync)) },
            { nameof(RecordRollbackAsync), new EventId(5, nameof(RecordRollbackAsync)) },
            { nameof(GetRemediationResultAsync), new EventId(6, nameof(GetRemediationResultAsync)) },
            { nameof(RecordActionExecutionAsync), new EventId(7, nameof(RecordActionExecutionAsync)) },
            { nameof(RecordActionRollbackAsync), new EventId(8, nameof(RecordActionRollbackAsync)) },
            { nameof(RecordRemediationMetricsAsync), new EventId(9, nameof(RecordRemediationMetricsAsync)) },
            { nameof(RecordStepMetricsAsync), new EventId(10, nameof(RecordStepMetricsAsync)) },
            { nameof(GetMetricsAsync), new EventId(11, nameof(GetMetricsAsync)) },
            { nameof(GetAggregatedMetricsAsync), new EventId(12, nameof(GetAggregatedMetricsAsync)) },
            { nameof(ValidateMetricsAsync), new EventId(13, nameof(ValidateMetricsAsync)) },
            { nameof(CollectSystemMetricsAsync), new EventId(14, nameof(CollectSystemMetricsAsync)) },
            { nameof(GetWindowsMemoryMetrics), new EventId(15, nameof(GetWindowsMemoryMetrics)) },
            { nameof(GetLinuxMemoryMetrics), new EventId(16, nameof(GetLinuxMemoryMetrics)) }
        };

        // TODO: Implement factories for lists if needed
        _remediationMetricsListFactory = remediationMetricsListFactory ?? (() => new List<RemediationMetrics>());
        _stepMetricsListFactory = stepMetricsListFactory ?? (() => new List<StepMetrics>());
        _metricValueListFactory = metricValueListFactory ?? (() => new List<Domain.Models.Remediation.MetricValue>());
        _validationWarningListFactory = validationWarningListFactory ?? (() => new List<ValidationWarning>());

        //// Setup collection timer with dueTime and period in milliseconds
        //_collectionTimer = new Timer(
        //    async _ => await CollectMetricsAsync().ConfigureAwait(false),
        //    null,
        //    0, // Start immediately
        //    (int)_options.CollectionInterval.TotalMilliseconds);
    }

    /// <inheritdoc/>
    public bool IsEnabled => !_disposed;

    /// <inheritdoc/>
    public string Name => "RuntimeErrorSage Remediation Metrics Collector";

    /// <inheritdoc/>
    public string Version => "1.0.0";

    /// <inheritdoc/>
    public async Task<Dictionary<string, object>> CollectMetricsAsync(ErrorContext context = null)
    {
        try
        {
            _logger.LogInformation(_eventIds[nameof(CollectMetricsAsync)], 
                "Collecting metrics for context {ContextId}", context?.Id ?? "system");

            var metrics = new Dictionary<string, object>();

            // Add system metrics
            var systemMetrics = await CollectSystemMetricsAsync(_cts.Token);
            foreach (var (key, value) in systemMetrics)
            {
                metrics[key] = value;
            }

            if (context != null)
            {
                // Add network metrics
                var networkMetrics = await CollectNetworkMetricsAsync(context);
                foreach (var (key, value) in networkMetrics)
                {
                    metrics[key] = value;
                }

                // Add error metrics
                var errorMetrics = await CollectErrorMetricsAsync(context);
                foreach (var (key, value) in errorMetrics)
                {
                    metrics[key] = value;
                }

                // Add additional context
                if (context.AdditionalContext != null)
                {
                    foreach (var kvp in context.AdditionalContext)
                    {
                        metrics[kvp.Key] = kvp.Value;
                    }
                }
            }

            return metrics;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(CollectMetricsAsync)], ex, 
                "Error collecting metrics for context {ContextId}", context?.Id ?? "system");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task RecordMetricAsync(string remediationId, string metricName, object value)
    {
        ArgumentNullException.ThrowIfNull(remediationId);
        ArgumentNullException.ThrowIfNull(metricName);
        ThrowIfDisposed();

        try
        {
            _logger.LogInformation(_eventIds[nameof(RecordMetricAsync)], 
                "Recording metric {MetricName} for remediation {RemediationId}", metricName, remediationId);

            // Convert value to double for the Values dictionary
            double doubleValue = 0;
            if (value is double d)
                doubleValue = d;
            else if (value is int i)
                doubleValue = i;
            else if (value is long l)
                doubleValue = l;
            else if (value is float f)
                doubleValue = f;
            else if (value is decimal m)
                doubleValue = (double)m;
            else if (value is bool b)
                doubleValue = b ? 1 : 0;
            else if (double.TryParse(value?.ToString(), out double parsed))
                doubleValue = parsed;
            
            var metrics = new RemediationMetrics
            {
                ExecutionId = remediationId,
                Timestamp = DateTime.UtcNow,
                Values = new Dictionary<string, double> { [metricName] = doubleValue }
            };

            // Store the original value in metadata for reference
            metrics.Metadata[metricName] = value;

            _metricsHistory.AddOrUpdate(
                remediationId,
                _remediationMetricsListFactory().Append(metrics).ToList(),
                (_, list) => { list.Add(metrics); return list; });
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(RecordMetricAsync)], ex, 
                "Error recording metric {MetricName} for remediation {RemediationId}", metricName, remediationId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Dictionary<string, List<RemediationMetrics>>> GetMetricsHistoryAsync(string remediationId)
    {
        ArgumentNullException.ThrowIfNull(remediationId);
        ThrowIfDisposed();

        try
        {
            _logger.LogInformation(_eventIds[nameof(GetMetricsHistoryAsync)], 
                "Getting metrics history for remediation {RemediationId}", remediationId);

            var history = new Dictionary<string, List<RemediationMetrics>>();
            if (_metricsHistory.TryGetValue(remediationId, out var metrics))
            {
                history["remediation"] = metrics;
            }

            return history;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(GetMetricsHistoryAsync)], ex, 
                "Error getting metrics history for remediation {RemediationId}", remediationId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task RecordExecutionAsync(string remediationId, RemediationResult result)
    {
        ArgumentNullException.ThrowIfNull(remediationId);
        ArgumentNullException.ThrowIfNull(result);
        ThrowIfDisposed();

        try
        {
            _logger.LogInformation(_eventIds[nameof(RecordExecutionAsync)], 
                "Recording execution for remediation {RemediationId}", remediationId);
            _remediationResults[remediationId] = result;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(RecordExecutionAsync)], ex, 
                "Error recording execution for remediation {RemediationId}", remediationId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task RecordRollbackAsync(string remediationId, RemediationResult result)
    {
        ArgumentNullException.ThrowIfNull(remediationId);
        ArgumentNullException.ThrowIfNull(result);
        ThrowIfDisposed();

        try
        {
            _logger.LogInformation(_eventIds[nameof(RecordRollbackAsync)], 
                "Recording rollback for remediation {RemediationId}", remediationId);
            _remediationResults[remediationId] = result;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(RecordRollbackAsync)], ex, 
                "Error recording rollback for remediation {RemediationId}", remediationId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationResult> GetRemediationResultAsync(string remediationId)
    {
        ArgumentNullException.ThrowIfNull(remediationId);
        ThrowIfDisposed();

        try
        {
            _logger.LogInformation(_eventIds[nameof(GetRemediationResultAsync)], 
                "Getting remediation result for {RemediationId}", remediationId);

            if (!_remediationResults.TryGetValue(remediationId, out var result))
            {
                return null;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(GetRemediationResultAsync)], ex, 
                "Error getting remediation result for {RemediationId}", remediationId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task RecordActionExecutionAsync(string actionId, RemediationActionResult result)
    {
        ArgumentNullException.ThrowIfNull(actionId);
        ArgumentNullException.ThrowIfNull(result);
        ThrowIfDisposed();

        try
        {
            _logger.LogInformation(_eventIds[nameof(RecordActionExecutionAsync)], 
                "Recording action execution for {ActionId}", actionId);
            _actionResults[actionId] = result;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(RecordActionExecutionAsync)], ex, 
                "Error recording action execution for {ActionId}", actionId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task RecordActionRollbackAsync(string actionId, RemediationActionResult result)
    {
        ArgumentNullException.ThrowIfNull(actionId);
        ArgumentNullException.ThrowIfNull(result);
        ThrowIfDisposed();

        try
        {
            _logger.LogInformation(_eventIds[nameof(RecordActionRollbackAsync)], 
                "Recording action rollback for {ActionId}", actionId);
            _actionResults[actionId] = result;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(RecordActionRollbackAsync)], ex, 
                "Error recording action rollback for {ActionId}", actionId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task RecordRemediationMetricsAsync(RemediationMetrics metrics)
    {
        ArgumentNullException.ThrowIfNull(metrics);
        ThrowIfDisposed();

        try
        {
            _logger.LogInformation(_eventIds[nameof(RecordRemediationMetricsAsync)], 
                "Recording remediation metrics for {ExecutionId}", metrics.ExecutionId);

            _metricsHistory.AddOrUpdate(
                metrics.ExecutionId,
                _remediationMetricsListFactory().Append(metrics).ToList(),
                (_, list) => { list.Add(metrics); return list; });
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(RecordRemediationMetricsAsync)], ex, 
                "Error recording remediation metrics for {ExecutionId}", metrics.ExecutionId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task RecordStepMetricsAsync(StepMetrics metrics)
    {
        ArgumentNullException.ThrowIfNull(metrics);
        ThrowIfDisposed();

        try
        {
            _logger.LogInformation(_eventIds[nameof(RecordStepMetricsAsync)], 
                "Recording step metrics for {StepId}", metrics.StepId);

            var metricsStepMetrics = new StepMetrics
            {
                StepId = metrics.StepId,
                //ActionId = metrics.StepName, // TODO: implement if needed 
                DurationMs = metrics.DurationMs,
                Status = metrics.Status,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMilliseconds(metrics.DurationMs)
            };

            _stepMetricsHistory.AddOrUpdate(
                metricsStepMetrics.StepId,
                _stepMetricsListFactory().Append(metricsStepMetrics).ToList(),
                (_, list) => { list.Add(metricsStepMetrics); return list; });
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(RecordStepMetricsAsync)], ex, 
                "Error recording step metrics for {StepId}", metrics.StepId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationMetrics> GetMetricsAsync(string remediationId)
    {
        ArgumentNullException.ThrowIfNull(remediationId);
        ThrowIfDisposed();

        try
        {
            _logger.LogInformation(_eventIds[nameof(GetMetricsAsync)], 
                "Getting metrics for remediation {RemediationId}", remediationId);

            if (!_metricsHistory.TryGetValue(remediationId, out var metrics) || !metrics.Any())
            {
                return null;
            }

            return metrics.Last();
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(GetMetricsAsync)], ex, 
                "Error getting metrics for remediation {RemediationId}", remediationId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<RemediationMetrics> GetAggregatedMetricsAsync(TimeRange range)
    {
        ArgumentNullException.ThrowIfNull(range);
        ThrowIfDisposed();

        try
        {
            _logger.LogInformation(_eventIds[nameof(GetAggregatedMetricsAsync)], 
                "Getting aggregated metrics for range {StartTime} to {EndTime}", range.StartTime, range.EndTime);

            var result = new RemediationMetrics
            {
                ExecutionId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                Values = new Dictionary<string, double>(),
                Labels = new Dictionary<string, string>()
            };

            var metricsInRange = _metricsHistory.Values
                .SelectMany(list => list)
                .Where(m => m.Timestamp >= range.StartTime && m.Timestamp <= range.EndTime)
                .ToList();

            if (!metricsInRange.Any())
            {
                return result;
            }

            // Aggregate metrics
            foreach (var metric in metricsInRange)
            {
                foreach (var (key, value) in metric.Values)
                {
                    if (!result.Values.ContainsKey(key))
                    {
                        result.Values[key] = value;
                    }
                    else
                    {
                        // Aggregate values - both are already double
                        result.Values[key] = (result.Values[key] + value) / 2;
                    }
                }

                // Merge labels
                foreach (var (key, value) in metric.Labels)
                {
                    if (!result.Labels.ContainsKey(key))
                    {
                        result.Labels[key] = value;
                    }
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(GetAggregatedMetricsAsync)], ex, 
                "Error getting aggregated metrics for range {StartTime} to {EndTime}", range.StartTime, range.EndTime);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<ValidationResult> ValidateMetricsAsync(RemediationMetrics metrics)
    {
        try
        {
            _logger.LogInformation(_eventIds[nameof(ValidateMetricsAsync)], 
                "Validating metrics for {ExecutionId}", metrics.ExecutionId);

            var result = new ValidationResult();
            var warnings = _validationWarningListFactory();

            // Validate required fields
            if (string.IsNullOrEmpty(metrics.ExecutionId))
            {
                warnings.Add(new ValidationWarning
                {
                    Message = "Execution ID is required",
                    Severity = ValidationSeverity.Error
                });
            }

            if (metrics.Timestamp == default)
            {
                warnings.Add(new ValidationWarning
                {
                    Message = "Timestamp is required",
                    Severity = ValidationSeverity.Error
                });
            }

            // Validate metric values
            foreach (var (key, value) in metrics.Values)
            {
                if (value == null)
                {
                    warnings.Add(new ValidationWarning
                    {
                        Message = $"Metric value for {key} is null",
                        Severity = ValidationSeverity.Warning
                    });
                }
            }

            // Add warnings to result using AddWarning method
            foreach (var warning in warnings)
            {
                result.AddWarning(warning.Message, warning.Severity, warning.Code ?? "WARNING");
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(ValidateMetricsAsync)], ex, 
                "Error validating metrics for {ExecutionId}", metrics.ExecutionId);
            throw;
        }
    }

    private async Task<Dictionary<string, object>> CollectSystemMetricsAsync(CancellationToken cancellationToken)
    {
        var metrics = new Dictionary<string, object>();
        
        try
        {
            // CPU usage percentage
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            metrics["cpu.usage"] = cpuCounter.NextValue();
            await Task.Delay(100, cancellationToken); // Wait for a more accurate reading
            metrics["cpu.usage"] = cpuCounter.NextValue();
            
            // Memory usage
            double memoryUsage;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                memoryUsage = GetWindowsMemoryMetrics();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                memoryUsage = GetLinuxMemoryMetrics();
            }
            else
            {
                memoryUsage = 0;
            }
            metrics["memory.usage"] = memoryUsage;
            
            // Disk usage
            var drives = DriveInfo.GetDrives().Where(d => d.IsReady);
            foreach (var drive in drives)
            {
                var driveName = drive.Name.Replace(":\\", "").ToLowerInvariant();
                metrics[$"disk.{driveName}.total"] = drive.TotalSize;
                metrics[$"disk.{driveName}.free"] = drive.AvailableFreeSpace;
                metrics[$"disk.{driveName}.used"] = drive.TotalSize - drive.AvailableFreeSpace;
            }
            
            // Process info
            metrics["process.cpu"] = _currentProcess.TotalProcessorTime.TotalMilliseconds;
            metrics["process.memory"] = _currentProcess.WorkingSet64;
            metrics["process.threads"] = _currentProcess.Threads.Count;
            metrics["process.handles"] = _currentProcess.HandleCount;
            
            // System uptime
            metrics["system.uptime"] = Environment.TickCount64 / 1000.0;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(CollectMetricsAsync)], ex, "Error collecting system metrics");
        }
        
        return metrics;
    }
    
    private double GetWindowsMemoryMetrics()
    {
        try
        {
            // Simple implementation for Windows
            return new PerformanceCounter("Memory", "Available MBytes").NextValue();
        }
        catch
        {
            return 0;
        }
    }
    
    private double GetLinuxMemoryMetrics()
    {
        try
        {
            // Simple implementation for Linux
            return 0; // Would normally read from /proc/meminfo
        }
        catch
        {
            return 0;
        }
    }

    private async Task<Dictionary<string, object>> CollectNetworkMetricsAsync(ErrorContext context)
    {
        try
        {
            _logger.LogInformation(_eventIds[nameof(CollectMetricsAsync)], 
                "Collecting network metrics for context {ContextId}", context.ContextId);

            var metrics = new Dictionary<string, object>();

            if (context is HttpErrorContext httpContext)
            {
                metrics["request_url"] = httpContext.Url;
                metrics["request_method"] = httpContext.Method;
                metrics["status_code"] = httpContext.StatusCode;
                // metrics["response_time"] = httpContext.ResponseTime; // TODO: implement if needed
                metrics["request_size"] = httpContext.RequestBody?.Length ?? 0;
                metrics["response_size"] = httpContext.ResponseBody?.Length ?? 0;
            }

            return metrics;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(CollectMetricsAsync)], ex, 
                "Error collecting network metrics for context {ContextId}", context.ContextId);
            throw;
        }
    }

    private async Task<Dictionary<string, object>> CollectErrorMetricsAsync(ErrorContext context)
    {
        try
        {
            _logger.LogInformation(_eventIds[nameof(CollectMetricsAsync)], 
                "Collecting error metrics for context {ContextId}", context.ContextId);

            var metrics = new Dictionary<string, object>
            {
                ["error_type"] = context.ErrorType,
                ["error_severity"] = context.Severity.ToString(),
                ["error_message"] = context.Error?.Message ?? context.Message,
                ["error_source"] = context.Error?.Source ?? context.Source,
                ["error_stack_trace"] = context.Error?.StackTrace ?? context.StackTrace,
                ["error_inner_exception"] = null // ErrorContext doesn't have InnerException directly
            };

            if (context is DatabaseErrorContext dbContext)
            {
                metrics["database_name"] = dbContext.DatabaseName;
                metrics["database_operation"] = dbContext.OperationName;
                metrics["database_error_code"] = dbContext.ErrorCode;
                //metrics["database_error_state"] = dbContext.ErrorState; // TODO:  implement if needed
            }

            return metrics;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(CollectMetricsAsync)], ex, 
                "Error collecting error metrics for context {ContextId}", context.ContextId);
            throw;
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(RemediationMetricsCollector));
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _cts.Cancel();
        _collectionTimer.Dispose();
        _cts.Dispose();
        _disposed = true;
    }

    private Dictionary<string, object> CalculateMetricTrends()
    {
        var trends = new Dictionary<string, object>();
        
        try
        {
            var latestMetrics = _metricsHistory.Values
                .SelectMany(m => m)
                .OrderByDescending(m => m.Timestamp)
                .Take(10)
                .ToList();
                
            if (latestMetrics.Count < 2)
            {
                return trends;
            }
            
            var commonMetrics = latestMetrics
                .SelectMany(m => m.Values.Keys)
                .GroupBy(k => k)
                .Where(g => g.Count() >= 2)
                .Select(g => g.Key)
                .ToList();
                
            foreach (var metric in commonMetrics)
            {
                var values = latestMetrics
                    .Where(m => m.Values.ContainsKey(metric))
                    .OrderBy(m => m.Timestamp)
                    .Select(m => 
                    {
                        var value = m.Values[metric];
                        // Convert to double using Convert.ToDouble which handles all numeric types
                        try
                        {
                            return Convert.ToDouble(value);
                        }
                        catch
                        {
                            return 0.0;
                        }
                    })
                    .ToList();
                    
                if (values.Count >= 2)
                {
                    var firstValue = values.First();
                    var lastValue = values.Last();
                    
                    if (Math.Abs(firstValue) > 0.001) // Avoid division by zero
                    {
                        var change = (lastValue - firstValue) / firstValue;
                        trends[metric] = change;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(CalculateMetricTrends)], ex, 
                "Error calculating metric trends");
        }
        
        return trends;
    }

    private static ValidationSeverity MapSeverity(SeverityLevel level)
    {
        return level switch
        {
            SeverityLevel.Critical => ValidationSeverity.Critical,
            SeverityLevel.High => ValidationSeverity.Error,
            SeverityLevel.Medium => ValidationSeverity.Warning,
            SeverityLevel.Low => ValidationSeverity.Info,
            _ => ValidationSeverity.Info
        };
    }

    public async Task<double> CalculateComponentHealthAsync(DependencyNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        ThrowIfDisposed();

        try
        {
            // Calculate health score based on various metrics
            var metrics = await GetMetricsAsync(node.ComponentId);
            if (metrics == null)
            {
                return 0.0;
            }

            double healthScore = 0.0;
            // TODO: Implement health score calculation
            return healthScore;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(CalculateComponentHealthAsync)], ex, 
                "Error calculating component health for {ComponentId}", node.ComponentId);
            throw;
        }
    }

    public async Task<double> CalculateReliabilityAsync(DependencyNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        ThrowIfDisposed();

        try
        {
            // Calculate reliability score based on error history and metrics
            var metrics = await GetMetricsAsync(node.ComponentId);
            if (metrics == null)
            {
                return 0.0;
            }

            double reliabilityScore = 0.0;
            // TODO: Implement reliability score calculation
            return reliabilityScore;
        }
        catch (Exception ex)
        {
            _logger.LogError(_eventIds[nameof(CalculateReliabilityAsync)], ex, 
                "Error calculating reliability for {ComponentId}", node.ComponentId);
            throw;
        }
    }

    private void AddValidationMetric(ValidationResult result, string message, SeverityLevel severity)
    {
        result.AddWarning(new ValidationWarning
        {
            Message = message,
            Severity = severity.ToValidationSeverity()
        });
    }

    private void AddErrorMetric(ValidationResult result, string message, SeverityLevel severity)
    {
        result.AddError(new ValidationError
        {
            Message = message,
            Severity = severity.ToValidationSeverity()
        });
    }

    private void AddWarningMetric(ValidationResult result, string message, SeverityLevel severity)
    {
        result.AddWarning(new ValidationWarning
        {
            Message = message,
            Severity = severity.ToValidationSeverity()
        });
    }
} 

