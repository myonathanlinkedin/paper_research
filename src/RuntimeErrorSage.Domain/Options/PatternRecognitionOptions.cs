using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Options
{
    /// <summary>
    /// Configuration options for pattern recognition.
    /// </summary>
    public class PatternRecognitionOptions
    {
        /// <summary>
        /// Gets or sets the service name for pattern recognition.
        /// </summary>
        public string ServiceName { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the similarity threshold for pattern matching.
        /// </summary>
        public float SimilarityThreshold { get; } = 0.8f;

        /// <summary>
        /// Gets or sets the minimum number of occurrences for a pattern to be recognized.
        /// </summary>
        public int MinPatternOccurrences { get; } = 3;

        /// <summary>
        /// Gets or sets the time window for pattern recognition.
        /// </summary>
        public TimeSpan PatternWindow { get; } = TimeSpan.FromHours(24);

        /// <summary>
        /// Gets or sets the temporal window for burst detection.
        /// </summary>
        public TimeSpan TemporalWindow { get; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Gets or sets the maximum number of features for pattern recognition.
        /// </summary>
        public int MaxPatternFeatures { get; } = 100;

        /// <summary>
        /// Gets or sets whether temporal analysis is enabled.
        /// </summary>
        public bool EnableTemporalAnalysis { get; } = true;

        /// <summary>
        /// Gets or sets whether cross-service pattern detection is enabled.
        /// </summary>
        public bool EnableCrossServicePatterns { get; } = true;
    }
} 







