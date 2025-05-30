using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Remediation.Factories;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Core.Remediation.Strategies
{
    /// <summary>
    /// Strategy for monitoring system health.
    /// </summary>
    public class MonitorStrategy : IRemediationStrategy
    {
        private readonly ILogger<MonitorStrategy> _logger;
        private readonly IMonitoringService _monitoringService;
        private readonly IRemediationActionResultFactory _resultFactory;
        private readonly List<RemediationAction> _actions = new();
        private readonly DateTime _createdAt = DateTime.UtcNow;

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
            Name = "Monitoring";
            Version = "1.0.0";
            IsEnabled = true;
            Priority = RemediationPriority.Low;
            RiskLevel = RiskLevel.Low;
            Description = "Monitors system performance after errors";
            Parameters = new Dictionary<string, object>();
            SupportedErrorTypes = new HashSet<string> { "System.OutOfMemoryException", "System.TimeoutException" };
            
            // Add default monitoring action
            _actions.Add(CreateMonitorAction("Monitor System Health", "Collects and analyzes system health metrics"));
        }

        /// <inheritdoc/>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <inheritdoc/>
        public string Name { get; set; } = "Monitoring";

        /// <inheritdoc/>
        public string Version { get; } = "1.0.0";

        /// <inheritdoc/>
        public bool IsEnabled { get; } = true;

        /// <inheritdoc/>
        public RemediationPriority Priority { get; set; } = RemediationPriority.Low;

        /// <inheritdoc/>
        public RiskLevel RiskLevel { get; set; } = RiskLevel.Low;

        /// <inheritdoc/>
        public string Description { get; set; } = "Monitors system performance after errors";

        /// <inheritdoc/>
        public Dictionary<string, object> Parameters { get; set; }

        /// <inheritdoc/>
        public ISet<string> SupportedErrorTypes { get; }

        /// <inheritdoc/>
        public List<RemediationAction> Actions => _actions;

        /// <inheritdoc/>
        public DateTime CreatedAt => _createdAt;

        /// <inheritdoc/>
        public async Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _logger.LogInformation("Executing monitoring strategy for error context {ErrorId}", context.Id);
            
            try
            {
                var monitoringResult = await _monitoringService.MonitorSystemAsync(context.ApplicationId);
                
                return new RemediationResult
                {
                    Success = monitoringResult.Success,
                    ErrorMessage = monitoringResult.Success ? null : monitoringResult.ErrorMessage,
                    Actions = new List<RemediationActionResult>
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
                    Actions = new List<RemediationActionResult>(),
                    StrategyId = Id,
                    StrategyName = Name,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow
                };
            }
        }

        /// <inheritdoc/>
        public Task<bool> CanHandleAsync(ErrorContext context)
        {
            if (context == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(SupportedErrorTypes.Contains(context.ErrorType));
        }

        /// <inheritdoc/>
        public Task<double> GetConfidenceAsync(ErrorContext context)
        {
            if (context == null)
            {
                return Task.FromResult(0.0);
            }

            // Higher confidence for explicitly supported error types
            if (SupportedErrorTypes.Contains(context.ErrorType))
            {
                return Task.FromResult(0.85);
            }

            // Lower confidence for other errors
            return Task.FromResult(0.4);
        }

        /// <inheritdoc/>
        public Task<List<RemediationSuggestion>> GetSuggestionsAsync(ErrorContext context)
        {
            if (context == null)
            {
                return Task.FromResult(new List<RemediationSuggestion>());
            }

            var suggestions = new List<RemediationSuggestion>
            {
                new RemediationSuggestion
                {
                    SuggestionId = Guid.NewGuid().ToString(),
                    StrategyId = Id,
                    StrategyName = Name,
                    Title = "Monitor System Health",
                    Description = $"Monitor system health metrics to analyze {context.ErrorType} error",
                    ConfidenceLevel = SupportedErrorTypes.Contains(context.ErrorType) ? 0.8 : 0.5,
                    Priority = Priority
                }
            };

            return Task.FromResult(suggestions);
        }

        /// <inheritdoc/>
        public Task<RemediationPlan> CreatePlanAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var plan = new RemediationPlan
            {
                PlanId = Guid.NewGuid().ToString(),
                Name = "Monitoring Plan",
                Description = $"Plan to monitor system health for {context.ErrorType} error",
                Context = context,
                CreatedAt = DateTime.UtcNow,
                Actions = new List<RemediationAction>(_actions)
            };

            return Task.FromResult(plan);
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
        public Task<RiskAssessmentModel> GetRiskAsync(ErrorContext context)
        {
            return Task.FromResult(new RiskAssessmentModel
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

