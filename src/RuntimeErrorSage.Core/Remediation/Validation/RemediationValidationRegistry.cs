using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Models.Validation;
using RemediationPlan = RuntimeErrorSage.Core.Models.Remediation.RemediationPlan;
using RuntimeErrorSage.Core.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Remediation.Validation;

/// <summary>
/// Registry for managing remediation validation rules with caching support.
/// </summary>
public class RemediationValidationRegistry : IRemediationValidationRegistry
{
    private readonly ILogger<RemediationValidationRegistry> _logger;
    private readonly IMemoryCache _cache;
    private readonly ConcurrentDictionary<string, RemediationValidationRule> _rules;
    private readonly MemoryCacheEntryOptions _defaultCacheOptions;

    public RemediationValidationRegistry(
        ILogger<RemediationValidationRegistry> logger,
        IMemoryCache cache)
    {
        _logger = logger;
        _cache = cache;
        _rules = new ConcurrentDictionary<string, RemediationValidationRule>();
        _defaultCacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));
    }

    public void RegisterRule(RemediationValidationRule rule)
    {
        if (rule == null)
            throw new ArgumentNullException(nameof(rule));

        if (_rules.TryAdd(rule.RuleId, rule))
        {
            _logger.LogInformation(
                "Registered validation rule {Rule} with priority {Priority}",
                rule.Name,
                rule.Priority);
        }
        else
        {
            _logger.LogWarning(
                "Validation rule {Rule} already registered",
                rule.Name);
        }
    }

    public void UnregisterRule(string ruleId)
    {
        if (_rules.TryRemove(ruleId, out var rule))
        {
            _logger.LogInformation(
                "Unregistered validation rule {Rule}",
                rule.Name);
        }
    }

    public async Task<ValidationResult> ValidateAsync(RemediationPlan plan, ErrorContext context)
    {
        var results = new List<ValidationResult>();
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            // Get rules ordered by priority
            var orderedRules = _rules.Values.OrderByDescending(r => r.Priority);

            foreach (var rule in orderedRules)
            {
                var result = await ValidateWithRuleAsync(rule, plan, context);
                results.Add(result);

                // Stop validation if a high-priority rule fails
                if (!result.IsSuccessful && rule.Priority >= 4)
                {
                    _logger.LogWarning(
                        "High-priority validation rule {Rule} failed: {Message}",
                        rule.Name,
                        result.Message);
                    break;
                }
            }

            // Aggregate results
            var aggregatedResult = new ValidationResult
            {
                IsSuccessful = results.All(r => r.IsSuccessful),
                Message = results.Any(r => !r.IsSuccessful)
                    ? string.Join("; ", results.Where(r => !r.IsSuccessful).Select(r => r.Message))
                    : "All validation rules passed",
                Details = new Dictionary<string, object>
                {
                    ["RuleResults"] = results,
                    ["TotalRules"] = results.Count,
                    ["FailedRules"] = results.Count(r => !r.IsSuccessful)
                },
                DurationMs = stopwatch.ElapsedMilliseconds
            };

            return aggregatedResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during validation");
            return new ValidationResult
            {
                IsSuccessful = false,
                Message = $"Validation failed: {ex.Message}",
                Details = new Dictionary<string, object>
                {
                    ["Exception"] = ex.ToString(),
                    ["DurationMs"] = stopwatch.ElapsedMilliseconds
                },
                DurationMs = stopwatch.ElapsedMilliseconds
            };
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    private async Task<ValidationResult> ValidateWithRuleAsync(
        RemediationValidationRule rule,
        RemediationPlan plan,
        ErrorContext context)
    {
        if (!rule.IsCacheable)
        {
            return await rule.ValidateAsync(plan, context);
        }

        var cacheKey = rule.GetCacheKey(plan, context);
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(rule.CacheDuration)
            .SetAbsoluteExpiration(rule.CacheDuration * 2);

        if (_cache.TryGetValue<ValidationResult>(cacheKey, out var cachedResult))
        {
            cachedResult.IsFromCache = true;
            return cachedResult;
        }

        var result = await rule.ValidateAsync(plan, context);
        _cache.Set(cacheKey, result, cacheOptions);
        return result;
    }

    public IEnumerable<RemediationValidationRule> GetRules()
    {
        return _rules.Values.OrderByDescending(r => r.Priority);
    }

    public RemediationValidationRule? GetRule(string ruleId)
    {
        return _rules.TryGetValue(ruleId, out var rule) ? rule : null;
    }

    public void ClearCache()
    {
        _cache.Compact(1.0);
        _logger.LogInformation("Validation rule cache cleared");
    }
} 