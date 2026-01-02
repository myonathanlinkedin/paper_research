using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Core.Remediation;
using ApplicationStrategy = RuntimeErrorSage.Application.Interfaces.IRemediationStrategy;
using DomainStrategy = RuntimeErrorSage.Domain.Interfaces.IRemediationStrategy;

namespace RuntimeErrorSage.Core.Remediation.Mapping
{
    /// <summary>
    /// Implementation of IRemediationStrategyMapper that maps between Application and Domain strategy interfaces.
    /// Uses adapter pattern to maintain DDD layer separation.
    /// </summary>
    public class RemediationStrategyMapper : IRemediationStrategyMapper
    {
        /// <inheritdoc/>
        public DomainStrategy ToDomain(ApplicationStrategy appStrategy)
        {
            if (appStrategy == null)
                return null;

            // Use existing adapter extension method
            return appStrategy.ToDomainStrategy();
        }

        /// <inheritdoc/>
        public ApplicationStrategy ToApplication(DomainStrategy domainStrategy)
        {
            if (domainStrategy == null)
                return null;

            // Use existing adapter extension method
            return domainStrategy.ToApplicationStrategy();
        }

        /// <inheritdoc/>
        public DomainStrategy ToDomain(RemediationStrategyModel model)
        {
            if (model == null)
                return null;

            // Create a simple adapter that wraps the model
            return new RemediationStrategyModelAdapter(model);
        }

        /// <inheritdoc/>
        public ApplicationStrategy ToApplication(RemediationStrategyModel model)
        {
            if (model == null)
                return null;

            // Use existing adapter
            return new RemediationStrategyAdapter(model);
        }

        /// <summary>
        /// Adapter that wraps a RemediationStrategyModel as a Domain IRemediationStrategy.
        /// </summary>
        private class RemediationStrategyModelAdapter : DomainStrategy
        {
            private readonly RemediationStrategyModel _model;

            public RemediationStrategyModelAdapter(RemediationStrategyModel model)
            {
                _model = model ?? throw new ArgumentNullException(nameof(model));
            }

            public string Id => _model.Id;
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Version => _model.Version ?? "1.0.0";
            public bool IsEnabled => _model.IsEnabled;
            public RemediationPriority Priority { get; set; }
            public int? PriorityValue { get; set; }
            public RiskLevel RiskLevel { get; set; }
            public Dictionary<string, object> Parameters { get; set; } = new();
            public ISet<string> SupportedErrorTypes { get; } = new HashSet<string>();
            public List<RemediationAction> Actions { get; } = new();
            public DateTime CreatedAt => _model.CreatedAt;

            public RemediationStatusEnum Status { get; set; } = RemediationStatusEnum.NotStarted;
            
            public Task<RemediationPriority> GetPriorityAsync()
            {
                return Task.FromResult(Priority);
            }
            
            public bool AppliesTo(ErrorContext errorContext) => true;
            
            public Task<IEnumerable<RemediationAction>> CreateActionsAsync(ErrorContext errorContext)
            {
                return Task.FromResult<IEnumerable<RemediationAction>>(Actions);
            }
        }
    }
}

