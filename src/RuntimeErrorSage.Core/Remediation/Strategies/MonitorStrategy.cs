using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Remediation.Factories;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Validation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Remediation.Strategies
{
    /// <summary>
    /// Strategy for monitoring system health.
    /// </summary>
    public class MonitorStrategy : Models.Remediation.Interfaces.IRemediationStrategy
    {
        private readonly ILogger<MonitorStrategy> _logger;
        private readonly IMonitoringService _monitoringService;
        private readonly IRemediationActionResultFactory _resultFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorStrategy"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="monitoringService">The monitoring service.</param>
        /// <param name="resultFactory">The remediation action result factory.</param>
        public MonitorStrategy(
            ILogger<MonitorStrategy> logger,
            IMonitoringService monitoringService,
            IRemediationActionResultFactory resultFactory)
        {
            _logger = logger;
            _monitoringService = monitoringService;
            _resultFactory = resultFactory;
            Id = Guid.NewGuid().ToString();
            Name = "System Monitoring";
            Description = "Monitors system health metrics";
            Parameters = new Dictionary<string, object>();
            SupportedErrorTypes = new HashSet<string> { "System.OutOfMemoryException", "System.TimeoutException" };
            Priority = RemediationPriority.Low;
        }

        /// <inheritdoc/>
        public string Id { get; }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public RemediationPriority Priority { get; }

        /// <inheritdoc/>
        public string Description { get; }

        /// <inheritdoc/>
        public Dictionary<string, object> Parameters { get; set; }

        /// <inheritdoc/>
        public ISet<string> SupportedErrorTypes { get; }

        /// <inheritdoc/>
        public async Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            if (context == null)
            {
                ArgumentNullException.ThrowIfNull(nameof(context));
            }

            _logger.LogInformation("Executing monitoring strategy for error context {ErrorId}", context.Id);
            
            try
            {
                var monitoringResult = await _monitoringService.MonitorSystemAsync(context.ApplicationId);
                
                return new RemediationResult
                {
                    Success = monitoringResult.Success,
                    ErrorMessage = monitoringResult.Success ? null : monitoringResult.ErrorMessage,
                    Actions = new Collection<RemediationActionResult>
                    {
                        _resultFactory.Create(
                            "Monitor System",
                            monitoringResult.Success,
                            monitoringResult.Success ? null : monitoringResult.ErrorMessage
                        )
                    },
                    StrategyId = Id,
                    StrategyName = Name,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing monitoring strategy");
                return new RemediationResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Actions = new Collection<RemediationActionResult>(),
                    StrategyId = Id,
                    StrategyName = Name,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow
                };
            }
        }

        /// <inheritdoc/>
        public Task<bool> CanApplyAsync(ErrorContext context)
        {
            if (context == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(SupportedErrorTypes.Contains(context.ErrorType));
        }

        /// <inheritdoc/>
        public Task<RemediationImpact> GetImpactAsync(ErrorContext context)
        {
            return Task.FromResult(new RemediationImpact
            {
                Severity = RemediationActionSeverity.Low,
                Description = "Monitoring has minimal impact on system performance"
            });
        }

        /// <inheritdoc/>
        public Task<RiskAssessment> GetRiskAsync(ErrorContext context)
        {
            return Task.FromResult(new RiskAssessment
            {
                Level = RiskLevel.Low,
                Description = "Monitoring operations are low risk",
                Factors = new Dictionary<string, double>
                {
                    { "SystemPerformance", 0.1 }
                },
                Timestamp = DateTime.UtcNow
            });
        }

        /// <inheritdoc/>
        public Task<bool> ValidateConfigurationAsync()
        {
            return Task.FromResult(_monitoringService != null);
        }

        /// <inheritdoc/>
        public Task<ValidationResult> ValidateAsync(ErrorContext context)
        {
            if (context == null)
            {
                return Task.FromResult(new ValidationResult
                {
                    IsValid = false,
                    Messages = { "Error context cannot be null" }
                });
            }

            return Task.FromResult(new ValidationResult
            {
                IsValid = true,
                StrategyId = Id,
                StrategyName = Name
            });
        }

        private RemediationAction CreateMonitorAction(string name, string description)
        {
            return new RemediationAction
            {
                Name = name,
                Description = description,
                Type = RemediationActionType.Monitor,
                Severity = SeverityLevel.Low.ToRemediationActionSeverity(),
                Status = RemediationActionStatus.Pending
            };
        }
    }
} 






