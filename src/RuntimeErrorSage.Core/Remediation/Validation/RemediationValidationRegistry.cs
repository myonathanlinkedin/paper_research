using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Validation;
using RemediationPlan = RuntimeErrorSage.Model.Models.Remediation.RemediationPlan;
using RuntimeErrorSage.Model.Remediation.Interfaces;
using CoreValidationResult = RuntimeErrorSage.Model.Models.Validation.ValidationResult;
using DataAnnotationsValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;
using RuntimeErrorSage.Model.Models.Remediation;

namespace RuntimeErrorSage.Model.Remediation.Validation;

/// <summary>
/// Registry for managing remediation validation rules with caching support.
/// </summary>
public class RemediationValidationRegistry : IRemediationValidationRegistry
{
    private readonly ILogger<RemediationValidationRegistry> _logger;
    private readonly IMemoryCache _cache;
    private readonly Dictionary<string, Models.Remediation.RemediationValidationRule> _rules;
    private readonly MemoryCacheEntryOptions _defaultCacheOptions;

    public RemediationValidationRegistry(
        ILogger<RemediationValidationRegistry> logger,
        IMemoryCache cache)
    {
        _logger = logger;
        _cache = cache;
        _rules = new Dictionary<string, Models.Remediation.RemediationValidationRule>();
        _defaultCacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));
    }

    /// <summary>
    /// Registers a validation rule.
    /// </summary>
    /// <param name="rule">The rule to register.</param>
    public void RegisterRule(Models.Remediation.RemediationValidationRule rule)
    {
        if (rule == null)
            throw new ArgumentNullException(nameof(rule));

        _rules[rule.Name] = rule;
    }

    /// <summary>
    /// Gets all registered rules.
    /// </summary>
    /// <returns>An enumerable of all registered rules.</returns>
    public IEnumerable<Models.Remediation.RemediationValidationRule> GetRules()
    {
        return _rules.Values;
    }

    /// <summary>
    /// Gets a rule by its name.
    /// </summary>
    /// <param name="name">The name of the rule to get.</param>
    /// <returns>The rule with the specified name, or null if not found.</returns>
    public Models.Remediation.RemediationValidationRule GetRule(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Rule name cannot be null or empty.", nameof(name));

        return _rules.TryGetValue(name, out var rule) ? rule : null;
    }

    public void UnregisterRule(string ruleId)
    {
        if (_rules.TryGetValue(ruleId, out var rule))
        {
            _rules.Remove(ruleId);
            _logger.LogInformation(
                "Unregistered validation rule {Rule}",
                rule.Name);
        }
    }

    public async Task<DataAnnotationsValidationResult> ValidateAsync(RemediationPlan plan, ErrorContext context)
    {
        var results = new List<CoreValidationResult>();
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

            // Convert to System.ComponentModel.DataAnnotations.ValidationResult
            if (results.Any(r => !r.IsSuccessful))
            {
                var errorMessages = results
                    .Where(r => !r.IsSuccessful)
                    .Select(r => r.Message);
                return new DataAnnotationsValidationResult(string.Join("; ", errorMessages));
            }

            return DataAnnotationsValidationResult.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during validation");
            return new DataAnnotationsValidationResult($"Validation failed: {ex.Message}");
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    private async Task<CoreValidationResult> ValidateWithRuleAsync(
        Models.Remediation.RemediationValidationRule rule,
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

        if (_cache.TryGetValue<CoreValidationResult>(cacheKey, out var cachedResult))
        {
            cachedResult.IsFromCache = true;
            return cachedResult;
        }

        var result = await rule.ValidateAsync(plan, context);
        _cache.Set(cacheKey, result, cacheOptions);
        return result;
    }

    public void ClearCache()
    {
        _cache.Compact(1.0);
        _logger.LogInformation("Validation rule cache cleared");
    }
} 
