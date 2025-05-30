using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Interfaces;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Adapter class that adapts a model-based remediation strategy to the remediation interface.
    /// This helps solve namespace conflicts by providing a bridge between the two implementations.
    /// </summary>
    public class RemediationStrategyAdapter : IRemediationStrategy
    {
        private readonly IRemediationStrategy _strategy;

        public RemediationStrategyAdapter(IRemediationStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public string Id => _strategy.Id;

        public string Name 
        { 
            get => _strategy.Name; 
            set => _strategy.Name = value; 
        }

        public RemediationPriority Priority 
        { 
            get => _strategy.Priority; 
            set => _strategy.Priority = value; 
        }

        public RiskLevel RiskLevel
        {
            get => _strategy.RiskLevel;
            set => _strategy.RiskLevel = value;
        }

        public string Description 
        { 
            get => _strategy.Description; 
            set => _strategy.Description = value; 
        }

        public Dictionary<string, object> Parameters 
        { 
            get => _strategy.Parameters; 
            set => _strategy.Parameters = value; 
        }

        public string Version => _strategy.Version;

        public bool IsEnabled => _strategy.IsEnabled;

        public ISet<string> SupportedErrorTypes => _strategy.SupportedErrorTypes;

        public List<RemediationAction> Actions => _strategy.Actions;

        public DateTime CreatedAt => _strategy.CreatedAt;

        public async Task<bool> CanHandleAsync(ErrorContext context)
        {
            return await _strategy.CanHandleAsync(context);
        }

        public async Task<double> GetConfidenceAsync(ErrorContext context)
        {
            return await _strategy.GetConfidenceAsync(context);
        }

        public async Task<List<RemediationSuggestion>> GetSuggestionsAsync(ErrorContext context)
        {
            return await _strategy.GetSuggestionsAsync(context);
        }

        public async Task<RemediationPlan> CreatePlanAsync(ErrorContext context)
        {
            return await _strategy.CreatePlanAsync(context);
        }

        public async Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            return await _strategy.ExecuteAsync(context);
        }

        public async Task<bool> CanApplyAsync(ErrorContext context)
        {
            return await _strategy.CanApplyAsync(context);
        }

        public async Task<RemediationImpact> GetImpactAsync(ErrorContext context)
        {
            return await _strategy.GetImpactAsync(context);
        }

        public async Task<RiskAssessmentModel> GetRiskAsync(ErrorContext context)
        {
            return await _strategy.GetRiskAsync(context);
        }

        public async Task<bool> ValidateConfigurationAsync()
        {
            return await _strategy.ValidateConfigurationAsync();
        }

        public async Task<ValidationResult> ValidateAsync(ErrorContext context)
        {
            return await _strategy.ValidateAsync(context);
        }
    }
} 
