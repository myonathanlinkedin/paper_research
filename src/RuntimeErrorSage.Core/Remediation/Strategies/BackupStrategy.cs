using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Remediation.Factories;
using RuntimeErrorSage.Domain.Models.Validation;

namespace RuntimeErrorSage.Core.Remediation.Strategies
{
    /// <summary>
    /// Strategy for backing up system state.
    /// </summary>
    public class BackupStrategy : IRemediationStrategy
    {
        private readonly ILogger<BackupStrategy> _logger;
        private readonly IBackupService _backupService;
        private readonly ILLMClient _llmClient;
        private readonly IRemediationActionResultFactory _resultFactory;
        private readonly List<RemediationAction> _actions = new();
        private readonly DateTime _createdAt = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupStrategy"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="backupService">The backup service.</param>
        /// <param name="llmClient">The LLM client.</param>
        /// <param name="resultFactory">The remediation action result factory.</param>
        public BackupStrategy(
            ILogger<BackupStrategy> logger,
            IBackupService backupService,
            ILLMClient llmClient,
            IRemediationActionResultFactory resultFactory)
        {
            _logger = logger;
            _backupService = backupService;
            _llmClient = llmClient;
            _resultFactory = resultFactory;
            Id = Guid.NewGuid().ToString();
            Name = "Backup";
            Version = "1.0.0";
            IsEnabled = true;
            Priority = RemediationPriority.Medium;
            RiskLevel = RiskLevel.Low;
            Description = "Creates a backup before remediation";
            Parameters = new Dictionary<string, object>();
            SupportedErrorTypes = new HashSet<string> { "System.Exception", "System.IO.IOException" };
            
            // Add default backup action
            _actions.Add(CreateBackupAction("Create System Backup", "Creates a system backup before remediation"));
        }

        /// <inheritdoc/>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <inheritdoc/>
        public string Name { get; set; } = "Backup";

        /// <inheritdoc/>
        public string Version { get; } = "1.0.0";

        /// <inheritdoc/>
        public bool IsEnabled { get; } = true;

        /// <inheritdoc/>
        public RemediationPriority Priority { get; set; } = RemediationPriority.Medium;

        /// <inheritdoc/>
        public RiskLevel RiskLevel { get; set; } = RiskLevel.Low;

        /// <inheritdoc/>
        public string Description { get; set; } = "Creates a backup before remediation";

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

            _logger.LogInformation("Executing backup strategy for error context {ErrorId}", context.Id);
            
            try
            {
                var backupResult = await _backupService.CreateBackupAsync(context.ApplicationId);
                
                return new RemediationResult
                {
                    Success = backupResult.Success,
                    ErrorMessage = backupResult.Success ? null : backupResult.ErrorMessage,
                    Actions = new List<RemediationActionResult>
                    {
                        _resultFactory.Create(
                            "Create Backup",
                            backupResult.Success,
                            backupResult.Success ? null : backupResult.ErrorMessage
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
                _logger.LogError(ex, "Error executing backup strategy");
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

            // Return confidence based on error type
            if (SupportedErrorTypes.Contains(context.ErrorType))
            {
                return Task.FromResult(0.8);
            }

            return Task.FromResult(0.3);
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
                    Title = "Create System Backup",
                    Description = "Create a backup before attempting any remediation actions",
                    ConfidenceLevel = 0.9,
                    Priority = RemediationPriority.High
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
                Name = "Backup Plan",
                Description = "Plan to create a system backup",
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
                Description = "Creating a backup has minimal impact on system performance"
            });
        }

        /// <inheritdoc/>
        public Task<RiskAssessmentModel> GetRiskAsync(ErrorContext context)
        {
            return Task.FromResult(new RiskAssessmentModel
            {
                Level = RiskLevel.Low,
                Description = "Backup operations are low risk",
                Factors = new Dictionary<string, double>
                {
                    { "DataLoss", 0.1 },
                    { "SystemPerformance", 0.2 }
                },
                Timestamp = DateTime.UtcNow
            });
        }

        /// <inheritdoc/>
        public Task<bool> ValidateConfigurationAsync()
        {
            return Task.FromResult(_backupService != null);
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

        private RemediationAction CreateBackupAction(string name, string description)
        {
            return new RemediationAction
            {
                Name = name,
                Description = description,
                Type = RemediationActionType.Backup,
                Severity = SeverityLevel.Low.ToRemediationActionSeverity(),
                Status = RemediationActionStatus.Pending
            };
        }
    }
} 

