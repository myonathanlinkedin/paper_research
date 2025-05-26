using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Execution;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Base class for remediation strategies.
    /// </summary>
    public abstract class RemediationStrategy : IRemediationStrategy
    {
        protected readonly ILogger<RemediationStrategy> _logger;
        protected readonly IRemediationValidator _validator;

        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int Priority { get; protected set; }
        public Dictionary<string, string> Parameters { get; protected set; }

        protected RemediationStrategy(
            ILogger<RemediationStrategy> logger,
            IRemediationValidator validator,
            string name,
            string description,
            int priority)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(validator);
            
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Strategy name cannot be null or empty", nameof(name));
            }
            
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Strategy description cannot be null or empty", nameof(description));
            }
            
            if (priority < 1 || priority > 5)
            {
                throw new ArgumentException("Strategy priority must be between 1 and 5", nameof(priority));
            }

            _logger = logger;
            _validator = validator;
            Name = name;
            Description = description;
            Priority = priority;
            Parameters = new Dictionary<string, string>();
        }

        public virtual bool CanHandle(ErrorAnalysisResult analysisResult)
        {
            ArgumentNullException.ThrowIfNull(analysisResult);

            // Default implementation - override in derived classes
            return false;
        }

        public virtual async Task<RemediationExecution> ApplyAsync(ErrorAnalysisResult analysisResult)
        {
            ArgumentNullException.ThrowIfNull(analysisResult);

            var execution = new RemediationExecution
            {
                StartTime = DateTime.UtcNow,
                Status = RemediationExecutionStatus.Running
            };

            try
            {
                var result = await ExecuteAsync(analysisResult.Context);
                execution.EndTime = DateTime.UtcNow;
                execution.Status = result.Success ? RemediationExecutionStatus.Completed : RemediationExecutionStatus.Failed;
                execution.Error = result.Error;
                return execution;
            }
            catch (Exception ex)
            {
                execution.EndTime = DateTime.UtcNow;
                execution.Status = RemediationExecutionStatus.Failed;
                execution.Error = ex.Message;
                return execution;
            }
        }

        public virtual async Task<bool> ValidateAsync(ErrorAnalysisResult analysisResult)
        {
            ArgumentNullException.ThrowIfNull(analysisResult);

            try
            {
                var validationResult = await _validator.ValidateStrategyAsync(this, analysisResult.Context);
                return validationResult.IsValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating strategy {StrategyName}", Name);
                return false;
            }
        }

        public virtual async Task<RemediationImpact> GetEstimatedImpactAsync(ErrorAnalysisResult analysisResult)
        {
            ArgumentNullException.ThrowIfNull(analysisResult);

            // Default implementation - override in derived classes
            return new RemediationImpact
            {
                Scope = RemediationActionImpactScope.Unknown,
                Severity = RemediationActionSeverity.Unknown,
                Description = "Impact not estimated"
            };
        }

        public abstract Task<RemediationResult> ExecuteAsync(ErrorContext context);

        protected virtual async Task ValidateParametersAsync()
        {
            if (Parameters == null)
            {
                throw new InvalidOperationException("Strategy parameters cannot be null");
            }

            // Validate required parameters
            foreach (var requiredParam in GetRequiredParameters())
            {
                if (!Parameters.ContainsKey(requiredParam))
                {
                    throw new InvalidOperationException($"Required parameter '{requiredParam}' is missing");
                }
            }

            await Task.CompletedTask;
        }

        protected abstract IEnumerable<string> GetRequiredParameters();

        protected virtual async Task<RemediationResult> CreateSuccessResultAsync(string message)
        {
            return await Task.FromResult(new RemediationResult
            {
                Success = true,
                Message = message,
                Timestamp = DateTime.UtcNow
            });
        }

        protected virtual async Task<RemediationResult> CreateFailureResultAsync(string message, Exception? exception = null)
        {
            return await Task.FromResult(new RemediationResult
            {
                Success = false,
                Message = message,
                Error = exception?.Message,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Strategy for monitoring system components.
    /// </summary>
    public class MonitorStrategy : RemediationStrategy
    {
        public MonitorStrategy(
            ILogger<RemediationStrategy> logger,
            IRemediationValidator validator)
            : base(logger, validator, "Monitor", "Monitors system components for health and performance", 1)
        {
        }

        public override async Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            try
            {
                await ValidateParametersAsync();

                // Validate strategy before execution
                var validationResult = await ValidateAsync(context);
                if (!validationResult.IsValid)
                {
                    return await CreateFailureResultAsync(
                        $"Strategy validation failed: {string.Join(", ", validationResult.Errors)}");
                }

                // Execute monitoring logic
                var metric = Parameters["metric"];
                var threshold = double.Parse(Parameters["threshold"]);

                // Simulate monitoring check
                var isHealthy = await CheckMetricHealthAsync(metric, threshold);
                
                return await CreateSuccessResultAsync(
                    $"Monitoring check completed for metric '{metric}'. Health status: {(isHealthy ? "Healthy" : "Unhealthy")}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing monitor strategy");
                return await CreateFailureResultAsync("Failed to execute monitor strategy", ex);
            }
        }

        protected override IEnumerable<string> GetRequiredParameters()
        {
            return new[] { "metric", "threshold" };
        }

        private async Task<bool> CheckMetricHealthAsync(string metric, double threshold)
        {
            // Simulate metric check
            await Task.Delay(100);
            return new Random().NextDouble() < 0.9; // 90% chance of being healthy
        }
    }

    /// <summary>
    /// Strategy for alerting on system issues.
    /// </summary>
    public class AlertStrategy : RemediationStrategy
    {
        public AlertStrategy(
            ILogger<RemediationStrategy> logger,
            IRemediationValidator validator)
            : base(logger, validator, "Alert", "Sends alerts for system issues", 2)
        {
        }

        public override async Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            try
            {
                await ValidateParametersAsync();

                // Validate strategy before execution
                var validationResult = await ValidateAsync(context);
                if (!validationResult.IsValid)
                {
                    return await CreateFailureResultAsync(
                        $"Strategy validation failed: {string.Join(", ", validationResult.Errors)}");
                }

                // Execute alert logic
                var channel = Parameters["channel"];
                var severity = Parameters["severity"];

                // Simulate alert sending
                await SendAlertAsync(channel, severity, context);
                
                return await CreateSuccessResultAsync(
                    $"Alert sent to channel '{channel}' with severity '{severity}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing alert strategy");
                return await CreateFailureResultAsync("Failed to execute alert strategy", ex);
            }
        }

        protected override IEnumerable<string> GetRequiredParameters()
        {
            return new[] { "channel", "severity" };
        }

        private async Task SendAlertAsync(string channel, string severity, ErrorContext context)
        {
            // Simulate alert sending
            await Task.Delay(100);
            _logger.LogInformation(
                "Alert sent to {Channel} with severity {Severity} for error: {ErrorType}",
                channel, severity, context.ErrorType);
        }
    }

    /// <summary>
    /// Strategy for backing up system components.
    /// </summary>
    public class BackupStrategy : RemediationStrategy
    {
        public BackupStrategy(
            ILogger<RemediationStrategy> logger,
            IRemediationValidator validator)
            : base(logger, validator, "Backup", "Creates backups of system components", 3)
        {
        }

        public override async Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            try
            {
                await ValidateParametersAsync();

                // Validate strategy before execution
                var validationResult = await ValidateAsync(context);
                if (!validationResult.IsValid)
                {
                    return await CreateFailureResultAsync(
                        $"Strategy validation failed: {string.Join(", ", validationResult.Errors)}");
                }

                // Execute backup logic
                var target = Parameters["target"];
                var schedule = Parameters["schedule"];

                // Simulate backup creation
                await CreateBackupAsync(target, schedule);
                
                return await CreateSuccessResultAsync(
                    $"Backup created for target '{target}' according to schedule '{schedule}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing backup strategy");
                return await CreateFailureResultAsync("Failed to execute backup strategy", ex);
            }
        }

        protected override IEnumerable<string> GetRequiredParameters()
        {
            return new[] { "target", "schedule" };
        }

        private async Task CreateBackupAsync(string target, string schedule)
        {
            // Simulate backup creation
            await Task.Delay(100);
            _logger.LogInformation(
                "Backup created for target {Target} according to schedule {Schedule}",
                target, schedule);
        }
    }
} 