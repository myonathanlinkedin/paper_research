using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Interfaces;

namespace RuntimeErrorSage.Application.Remediation
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

        public string Id 
        { 
            get => _strategy.Id; 
            set => _strategy.Id = value; 
        }

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

        public ISet<string> SupportedErrorTypes => _strategy.SupportedErrorTypes;

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
