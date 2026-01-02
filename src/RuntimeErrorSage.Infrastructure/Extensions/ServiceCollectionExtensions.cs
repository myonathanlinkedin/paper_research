using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Health;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Application.LLM;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Application.LLM.Options;
using RuntimeErrorSage.Application.MCP;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Application.Options;
using RuntimeErrorSage.Application.Remediation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Application.Runtime.Interfaces;
using RuntimeErrorSage.Application.Storage;
using RuntimeErrorSage.Application.Storage.Interfaces;
using RuntimeErrorSage.Core.MCP;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Core.Examples;
using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Infrastructure.Services;
using StackExchange.Redis;

namespace RuntimeErrorSage.Infrastructure.Extensions
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

            // Configure Redis storage
            services.Configure<Application.Options.RedisPatternStorageOptions>(configuration.GetSection("RuntimeErrorSage:Redis"));
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<Application.Options.RedisPatternStorageOptions>>();
                return ConnectionMultiplexer.Connect(options.Value.ConnectionString);
            });

            // Configure LM Studio
            services.Configure<LMStudioOptions>(configuration.GetSection("RuntimeErrorSage:LMStudio"));
            services.AddHttpClient<ILMStudioClient, LMStudioClient>();

            // Register Qwen LLM client
            services.AddSingleton<ILLMClient>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<LLMClient>>();
                var options = sp.GetRequiredService<IOptions<LMStudioOptions>>();
                return new LLMClient(
                    logger,
                    options.Value.BaseUrl,
                    options.Value.ModelId);
            });

            // Register core services
            services.AddSingleton<IErrorAnalyzer, ErrorAnalyzer>();
            services.AddSingleton<IErrorContextAnalyzer, ErrorContextAnalyzer>();
            services.AddSingleton<IPatternStorage, RedisPatternStorage>();
            services.AddSingleton<IMCPClient, MCPClient>();
            services.AddSingleton<IRemediationMetricsCollector, RemediationMetricsCollector>();
            services.AddSingleton<IRemediationValidator, RemediationValidator>();
            services.AddSingleton<IRemediationTracker, RemediationTracker>();
            
            // Register RemediationStrategyProvider (needed by FixedRemediationExecutorExample)
            services.AddSingleton<IRemediationStrategyProvider, RemediationStrategyProvider>();
            services.AddSingleton<IRemediationRegistry, Core.Remediation.RemediationRegistry>();
            
            // Register FixedRemediationExecutorExample with a default strategy
            services.AddSingleton<IRemediationExecutor>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<FixedRemediationExecutorExample>>();
                var validator = sp.GetRequiredService<IRemediationValidator>();
                var metricsCollector = sp.GetRequiredService<IRemediationMetricsCollector>();
                var strategyProvider = sp.GetRequiredService<IRemediationStrategyProvider>();
                
                // Create a default strategy for the example executor
                var defaultStrategyModel = new Domain.Models.Remediation.RemediationStrategyModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Default Example Strategy",
                    Description = "Default strategy for FixedRemediationExecutorExample",
                    Priority = Domain.Enums.RemediationPriority.Medium
                };
                var defaultStrategy = new RemediationStrategyAdapter(defaultStrategyModel);
                
                return new FixedRemediationExecutorExample(
                    logger,
                    defaultStrategy,
                    validator,
                    metricsCollector,
                    strategyProvider);
            });
            
            services.AddSingleton<IRuntimeErrorSageService, RuntimeErrorSageService>();

            // Register health checks
            services.AddHealthChecks()
                .AddCheck<LMStudioHealthCheck>("lmstudio")
                .AddCheck<RedisHealthCheck>("redis");

            return services;
        }
    }
}




