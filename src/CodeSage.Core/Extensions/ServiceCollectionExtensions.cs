using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CodeSage.Core.Interfaces;
using CodeSage.Core.LLM;
using CodeSage.Core.Analysis;
using CodeSage.Core.Options;
using CodeSage.Core.LLM.Options;
using CodeSage.Core.Health;
using CodeSage.Core.MCP;
using CodeSage.Core.Remediation;
using CodeSage.Core.Remediation.Interfaces;

namespace CodeSage.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCodeSage(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<CodeSageOptions>? configureOptions = null)
        {
            // Configure options
            services.Configure<CodeSageOptions>(configuration.GetSection("CodeSage"));
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            // Configure LM Studio
            services.Configure<LMStudioOptions>(configuration.GetSection("CodeSage:LMStudio"));
            services.AddHttpClient<ILMStudioClient, LMStudioClient>();

            // Register core services
            services.AddSingleton<IErrorAnalyzer, ErrorAnalyzer>();
            services.AddSingleton<IDistributedStorage, DistributedStorage>();
            services.AddSingleton<IMCPClient, MCPClient>();
            services.AddSingleton<IRemediationMetricsCollector, RemediationMetricsCollector>();
            services.AddSingleton<IRemediationValidator, RemediationValidator>();
            services.AddSingleton<IRemediationTracker, RemediationTracker>();
            services.AddSingleton<IRemediationExecutor, RemediationExecutor>();
            services.AddSingleton<ICodeSageService, CodeSageService>();

            // Register health checks
            services.AddHealthChecks()
                .AddCheck<LMStudioHealthCheck>("lmstudio");
                // Add other health checks here

            return services;
        }
    }
} 