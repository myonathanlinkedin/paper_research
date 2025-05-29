using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Remediation.Factories;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Remediation.Strategies
{
    /// <summary>
    /// Strategy for backing up system state.
    /// </summary>
    public class BackupStrategy : Models.Remediation.Interfaces.IRemediationStrategy
    {
        private readonly ILogger<BackupStrategy> _logger;
        private readonly IBackupService _backupService;
        private readonly ILLMClient _llmClient;
        private readonly IRemediationActionResultFactory _resultFactory;

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
            Name = "Backup Strategy";
            Description = "Creates a backup before attempting remediation";
            Parameters = new Dictionary<string, object>();
            SupportedErrorTypes = new HashSet<string> { "System.Exception", "System.IO.IOException" };
            Priority = RemediationPriority.High;
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

            _logger.LogInformation("Executing backup strategy for error context {ErrorId}", context.Id);
            
            try
            {
                var backupResult = await _backupService.CreateBackupAsync(context.ApplicationId);
                
                return new RemediationResult
                {
                    Success = backupResult.Success,
                    ErrorMessage = backupResult.Success ? null : backupResult.ErrorMessage,
                    Actions = new Collection<RemediationActionResult>
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
                Description = "Creating a backup has minimal impact on system performance"
            });
        }

        /// <inheritdoc/>
        public Task<RiskAssessment> GetRiskAsync(ErrorContext context)
        {
            return Task.FromResult(new RiskAssessment
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






