using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CodeSage.Core.Interfaces;
using CodeSage.Core.Models;
using CodeSage.Core.Exceptions;
using System.Collections.Concurrent;
using CodeSage.Core.Models.Error;
using CodeSage.Core.Interfaces.MCP;
using CodeSage.Core.Utilities;

namespace CodeSage.Core.Analysis
{
    /// <summary>
    /// Implements pattern recognition for error analysis.
    /// </summary>
    public class PatternRecognition : IPatternRecognition
    {
        private readonly ILogger<PatternRecognition> _logger;
        private readonly IMCPClient _mcpClient;

        public PatternRecognition(
            ILogger<PatternRecognition> logger,
            IMCPClient mcpClient)
        {
            _logger = logger;
            _mcpClient = mcpClient;
        }

        public async Task<ErrorPattern?> IdentifyPatternAsync(ErrorContext context)
        {
            try
            {
                // Retrieve known patterns for the service
                var knownPatterns = await _mcpClient.GetErrorPatternsAsync(context.ServiceName);

                // Find a matching pattern
                var matchingPattern = FindMatchingPattern(context, knownPatterns);

                if (matchingPattern != null)
                {
                    _logger.LogInformation(
                // Initialize pattern recognition system
                await LoadPatternsAsync();
                _logger.LogInformation("Pattern recognition system initialized");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing pattern recognition system");
                throw new PatternRecognitionException("Failed to initialize pattern recognition system", ex);
            }
        }

        public async Task<List<ErrorPattern>> DetectPatternsAsync(List<ErrorContext> contexts, string serviceName)
        {
            try
            {
                var patterns = new List<ErrorPattern>();
                foreach (var context in contexts)
                {
                    var pattern = await FindMatchingPatternAsync(context, serviceName);
                    if (pattern != null)
                    {
                        patterns.Add(pattern);
                    }
                }
                return patterns;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting patterns");
                throw new PatternRecognitionException("Failed to detect patterns", ex);
            }
        }

        public async Task<ErrorPattern?> FindMatchingPatternAsync(ErrorContext context, string serviceName)
        {
            try
            {
                // Get patterns for the service
                var patterns = await GetPatternsAsync(serviceName);

                // Find matching pattern
                return patterns.FirstOrDefault(p => 
                    p.ErrorType == context.ErrorType &&
                    CompareAdditionalContext(p.Context, context.AdditionalContext));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding matching pattern");
                throw new PatternRecognitionException("Failed to find matching pattern", ex);
            }
        }

        private async Task LoadPatternsAsync()
        {
            try
            {
                // Load patterns from MCP
                var patterns = await _mcpClient.GetErrorPatternsAsync(_options.ServiceName);
                _patternCache[_options.ServiceName] = patterns;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading patterns");
                throw new PatternRecognitionException("Failed to load patterns", ex);
            }
        }

        private async Task<List<ErrorPattern>> GetPatternsAsync(string serviceName)
        {
            if (!_patternCache.TryGetValue(serviceName, out var patterns))
            {
                patterns = await _mcpClient.GetErrorPatternsAsync(serviceName);
                _patternCache[serviceName] = patterns;
            }
            return patterns;
        }

        private bool CompareAdditionalContext(
            Dictionary<string, object> patternContext,
            Dictionary<string, object> currentContext)
        {
            return ContextComparer.CompareAdditionalContext(patternContext, currentContext);
        }

        private List<ErrorPattern> MergePatterns(
            List<ErrorPattern> existing,
            List<ErrorPattern> newPatterns)
        {
            var merged = new List<ErrorPattern>(existing);

            foreach (var newPattern in newPatterns)
            {
                var existingPattern = merged.FirstOrDefault(p =>
                    p.ErrorType == newPattern.ErrorType &&
                    p.OperationName == newPattern.OperationName);

                if (existingPattern != null)
                {
                    existingPattern.LastOccurrence = newPattern.LastOccurrence;
                    existingPattern.OccurrenceCount += newPattern.OccurrenceCount;
                    existingPattern.Context = newPattern.Context;
                    existingPattern.Confidence = Math.Max(existingPattern.Confidence, newPattern.Confidence);
                }
                else
                {
                    merged.Add(newPattern);
                }
            }

            return merged;
        }
    }
} 