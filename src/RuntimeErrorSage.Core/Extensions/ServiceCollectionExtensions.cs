using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.LLM;
using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Core.Options;
using RuntimeErrorSage.Core.LLM.Options;
using RuntimeErrorSage.Core.Health;
using RuntimeErrorSage.Core.MCP;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Core.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRuntimeErrorSage(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<RuntimeErrorSageOptions>? configureOptions = null)
        {
            // Configure options
            services.Configure<RuntimeErrorSageOptions>(configuration.GetSection("RuntimeErrorSage"));
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            // Configure LM Studio
            services.Configure<LMStudioOptions>(configuration.GetSection("RuntimeErrorSage:LMStudio"));
            services.AddHttpClient<ILMStudioClient, LMStudioClient>();

            // Register core services
            services.AddSingleton<IErrorAnalyzer, ErrorAnalyzer>();
            services.AddSingleton<IDistributedStorage, DistributedStorage>();
            services.AddSingleton<IMCPClient, MCPClient>();
            services.AddSingleton<IRemediationMetricsCollector, RemediationMetricsCollector>();
            services.AddSingleton<IRemediationValidator, RemediationValidator>();
            services.AddSingleton<IRemediationTracker, RemediationTracker>();
            services.AddSingleton<IRemediationExecutor, RemediationExecutor>();
            services.AddSingleton<IRuntimeErrorSageService, RuntimeErrorSageService>();

            // Register health checks
            services.AddHealthChecks()
                .AddCheck<LMStudioHealthCheck>("lmstudio");
                // Add other health checks here

            return services;
        }
    }
} 
