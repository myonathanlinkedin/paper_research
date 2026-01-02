using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Application.Options;
using RuntimeErrorSage.Application.Pattern.Interfaces;
using RuntimeErrorSage.Application.Storage.Interfaces;
using RuntimeErrorSage.Core.Storage.Utilities;
using RuntimeErrorSage.Domain.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;

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

        public async Task InitializeAsync()
        {
            try
            {
                // Initialize pattern recognition system
                await LoadPatternsAsync();
                await InitializeModelAsync();
                await ValidatePatternsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing pattern recognition");
                throw new RuntimeErrorSage.Application.Exceptions.PatternRecognitionException("Failed to initialize pattern recognition", ex);
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
                throw new RuntimeErrorSage.Application.Exceptions.PatternRecognitionException("Failed to detect patterns", ex);
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
                throw new RuntimeErrorSage.Application.Exceptions.PatternRecognitionException("Failed to find matching pattern", ex);
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
    }
} 

