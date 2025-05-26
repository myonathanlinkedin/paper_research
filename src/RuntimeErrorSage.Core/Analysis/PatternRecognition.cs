using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.Exceptions;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Interfaces.MCP;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Options;
using RuntimeErrorSage.Core.Utilities;
using System.Collections.Concurrent;

namespace RuntimeErrorSage.Core.Analysis
{
    /// <summary>
    /// Implements pattern recognition for error analysis.
    /// </summary>
    public class PatternRecognition : IPatternRecognition
    {
        private readonly ILogger<PatternRecognition> _logger;
        private readonly IMCPClient _mcpClient;
        private readonly RuntimeErrorSageOptions _options;
        private readonly ConcurrentDictionary<string, List<ErrorPattern>> _patternCache;
        private List<ErrorPattern> _patterns;
        private readonly IModel _model;
        private readonly IStorage _storage;

        public PatternRecognition(
            ILogger<PatternRecognition> logger,
            IMCPClient mcpClient,
            IOptions<RuntimeErrorSageOptions> options,
            IModel model,
            IStorage storage)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(mcpClient);
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(storage);
            
            _logger = logger;
            _mcpClient = mcpClient;
            _options = options.Value;
            _patternCache = new ConcurrentDictionary<string, List<ErrorPattern>>();
            _patterns = new List<ErrorPattern>();
            _model = model;
            _storage = storage;
        }

        public async Task<ErrorPattern?> IdentifyPatternAsync(ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context)); // Validate parameter is non-null

            try
            {
                // Retrieve known patterns for the service
                var knownPatterns = await _mcpClient.GetErrorPatternsAsync(context.ServiceName).ConfigureAwait(false);

                // Find a matching pattern
                var matchingPattern = FindMatchingPattern(context, knownPatterns);

                if (matchingPattern != null)
                {
                    _logger.LogInformation(
                        "Found matching pattern for error type {ErrorType} in service {ServiceName}",
                        matchingPattern.ErrorType,
                        context.ServiceName);
                }

                return matchingPattern;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error identifying pattern");
                throw new PatternRecognitionException("Failed to identify pattern", ex);
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
                var patterns = await GetPatternsAsync(serviceName).ConfigureAwait(true);

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

        private ErrorPattern? FindMatchingPattern(ErrorContext context, List<ErrorPattern> patterns)
        {
            return patterns.FirstOrDefault(p => 
                p.ErrorType == context.ErrorType &&
                CompareAdditionalContext(p.Context, context.AdditionalContext));
        }

        public async Task InitializeAsync()
        {
            // Initialize pattern recognition system
            await LoadPatternsAsync();
            await InitializeModelAsync();
            await ValidatePatternsAsync();
        }

        private async Task LoadPatternsAsync()
        {
            // Load error patterns from storage
            _patterns = await _storage.LoadPatternsAsync();
        }

        private async Task InitializeModelAsync()
        {
            // Initialize the pattern recognition model
            await _model.InitializeAsync();
        }

        private async Task ValidatePatternsAsync()
        {
            // Validate loaded patterns
            foreach (var pattern in _patterns)
            {
                await ValidatePatternAsync(pattern);
            }
        }

        private async Task ValidatePatternAsync(ErrorPattern pattern)
        {
            // Validate individual pattern
            if (!await _model.ValidatePatternAsync(pattern))
            {
                throw new InvalidOperationException($"Invalid pattern: {pattern.Id}");
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
            Dictionary<string, string> currentContext)
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
                    existingPattern.LastUpdated = newPattern.LastUpdated;
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
