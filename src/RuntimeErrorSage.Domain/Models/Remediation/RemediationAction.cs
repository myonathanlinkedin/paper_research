// Git: fix application & domain & push
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Interfaces;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents a remediation action that can be executed to fix an error.
    /// </summary>
    public class RemediationAction : IRemediationAction
    {
        private readonly RemediationActionCore _core;
        private readonly RemediationActionValidation _validation;
        private readonly RemediationActionExecution _execution;
        private readonly RemediationActionResult _result;
        private readonly List<string> _prerequisites = new();
        private readonly List<string> _dependencies = new();
        private readonly List<ValidationRule> _validationRules = new();
        private readonly Dictionary<string, object> _parameters = new();
        private Dictionary<string, object> _metadata = new();
        private RemediationActionImpactScope _impactScope = RemediationActionImpactScope.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationAction"/> class.
        /// </summary>
        public RemediationAction(IValidationRuleProvider ruleProvider)
        {
            _core = new RemediationActionCore();
            var defaultContext = new ValidationContext();
            _validation = new RemediationActionValidation(this, defaultContext);
            _execution = new RemediationActionExecution();
            _result = new RemediationActionResult();
        }

        /// <summary>
        /// Gets or sets the unique identifier for this action.
        /// </summary>
        public string Id
        {
            get => _core.Id;
            set => _core.Id = value;
        }

        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        public string Name
        {
            get => _core.Name;
            set => _core.Name = value;
        }

        /// <summary>
        /// Gets or sets the action description.
        /// </summary>
        public string Description
        {
            get => _core.Description;
            set => _core.Description = value;
        }

        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        public string ActionType
        {
            get => _core.ActionType;
            set => _core.ActionType = value;
        }

        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public ErrorContext Context
        {
            get => _core.Context;
            set => _core.Context = value;
        }

        /// <summary>
        /// Gets or sets the priority of the action.
        /// </summary>
        public RemediationPriority Priority
        {
            get => _core.Priority;
            set => _core.Priority = value;
        }

        /// <summary>
        /// Gets or sets the impact level of the action.
        /// </summary>
        public ImpactLevel Impact
        {
            get => _core.Impact;
            set => _core.Impact = value;
        }

        /// <summary>
        /// Gets or sets the risk level of the action.
        /// </summary>
        public RiskLevel RiskLevel
        {
            get => _core.RiskLevel;
            set => _core.RiskLevel = value;
        }

        /// <summary>
        /// Gets or sets the status of the action.
        /// </summary>
        public RemediationStatusEnum Status
        {
            get => _core.Status;
            set => _core.Status = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the action requires manual approval.
        /// </summary>
        public bool RequiresManualApproval
        {
            get => _core.RequiresManualApproval;
            set => _core.RequiresManualApproval = value;
        }

        /// <summary>
        /// Gets or sets the rollback action.
        /// </summary>
        public IRemediationAction RollbackAction
        {
            get => _core.RollbackAction;
            set => _core.RollbackAction = value;
        }

        /// <summary>
        /// Gets or sets whether the action can be rolled back.
        /// </summary>
        public bool CanRollback
        {
            get => _core.CanRollback;
            set => _core.CanRollback = value;
        }

        /// <summary>
        /// Executes the remediation action.
        /// </summary>
        /// <returns>The result of the action execution.</returns>
        async Task IRemediationAction.ExecuteAsync()
        {
            if (Context == null)
                throw new InvalidOperationException("Cannot execute action without context");

            await _execution.ExecuteAsync(this, Context);
        }

        /// <summary>
        /// Executes the remediation action.
        /// </summary>
        /// <returns>The result of the action execution.</returns>
        public async Task<RemediationResult> ExecuteAsync()
        {
            if (Context == null)
                throw new InvalidOperationException("Cannot execute action without context");

            return await _execution.ExecuteAsync(this, Context);
        }

        /// <summary>
        /// Validates the remediation action.
        /// </summary>
        /// <returns>The validation result.</returns>
        public async Task<ValidationResult> ValidateAsync()
        {
            if (Context == null)
                throw new InvalidOperationException("Cannot validate action without context");

            return await _validation.ValidateAsync();
        }

        /// <summary>
        /// Rolls back the remediation action.
        /// </summary>
        /// <returns>The rollback status.</returns>
        async Task<Domain.Enums.RollbackStatusEnum> IRemediationAction.RollbackAsync()
        {
            if (Context == null)
                throw new InvalidOperationException("Cannot rollback action without context");

            var result = await _execution.RollbackAsync(this, Context);
            return (Domain.Enums.RollbackStatusEnum)(int)result.Status;
        }

        /// <summary>
        /// Rolls back the remediation action.
        /// </summary>
        /// <returns>The rollback status.</returns>
        public async Task<RollbackStatus> RollbackAsync()
        {
            if (Context == null)
                throw new InvalidOperationException("Cannot rollback action without context");

            return await _execution.RollbackAsync(this, Context);
        }

        /// <summary>
        /// Gets or sets the parameters for the action.
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            get => _parameters;
            set
            {
                if (value != null)
                {
                    _parameters.Clear();
                    foreach (var kvp in value)
                    {
                        _parameters.Add(kvp.Key, kvp.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the validation rules.
        /// </summary>
        public List<string> ValidationRules
        {
            get => _validationRules.ConvertAll(r => r.RuleId);
            set
            {
                _validationRules.Clear();
                if (value != null)
                {
                    foreach (var ruleId in value)
                    {
                        _validationRules.Add(new ValidationRule(ruleId));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the error type this action addresses.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the scope of impact for this remediation action.
        /// </summary>
        public RemediationActionImpactScope ImpactScope
        {
            get => _impactScope;
            set => _impactScope = value;
        }

        /// <summary>
        /// Gets the estimated impact of this action.
        /// </summary>
        /// <returns>The estimated impact.</returns>
        public async Task<RemediationImpact> GetEstimatedImpactAsync()
        {
            return new RemediationImpact();
        }
    }
} 



