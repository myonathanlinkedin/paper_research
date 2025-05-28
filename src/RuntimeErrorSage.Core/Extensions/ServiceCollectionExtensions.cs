using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Core.Analysis.Interfaces;
using RuntimeErrorSage.Core.Health;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.LLM;
using RuntimeErrorSage.Core.LLM.Interfaces;
using RuntimeErrorSage.Core.LLM.Options;
using RuntimeErrorSage.Core.MCP;
using RuntimeErrorSage.Core.MCP.Interfaces;
using RuntimeErrorSage.Core.Options;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Runtime.Interfaces;
using RuntimeErrorSage.Core.Storage;
using RuntimeErrorSage.Core.Storage.Interfaces;
using StackExchange.Redis;

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

            // Configure Redis storage
            services.Configure<RedisPatternStorageOptions>(configuration.GetSection("RuntimeErrorSage:Redis"));
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<RedisPatternStorageOptions>>();
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
            services.AddSingleton<IPatternStorage, RedisPatternStorage>();
            services.AddSingleton<IMCPClient, MCPClient>();
            services.AddSingleton<IRemediationMetricsCollector, RemediationMetricsCollector>();
            services.AddSingleton<IRemediationValidator, RemediationValidator>();
            services.AddSingleton<IRemediationTracker, RemediationTracker>();
            services.AddSingleton<IRemediationExecutor, RemediationExecutor>();
            services.AddSingleton<IRuntimeErrorSageService, RuntimeErrorSageService>();

            // Register health checks
            services.AddHealthChecks()
                .AddCheck<LMStudioHealthCheck>("lmstudio")
                .AddCheck<RedisHealthCheck>("redis");

            return services;
        }
    }
} 

