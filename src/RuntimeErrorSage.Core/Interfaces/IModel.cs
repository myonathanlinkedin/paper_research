using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Represents a model interface for error analysis and pattern recognition.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Gets the model identifier.
        /// </summary>
        string ModelId { get; }

        /// <summary>
        /// Gets the model version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the model type.
        /// </summary>
        string ModelType { get; }

        /// <summary>
        /// Gets whether the model is loaded.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Loads the model.
        /// </summary>
        Task LoadAsync();

        /// <summary>
        /// Unloads the model.
        /// </summary>
        Task UnloadAsync();

        /// <summary>
        /// Analyzes an error using the model.
        /// </summary>
        /// <param name="errorContext">The error context to analyze.</param>
        /// <returns>The analysis result.</returns>
        Task<ErrorAnalysisResult> AnalyzeErrorAsync(ErrorContext errorContext);

        /// <summary>
        /// Trains the model with new data.
        /// </summary>
        /// <param name="trainingData">The training data.</param>
        /// <returns>A task representing the training operation.</returns>
        Task TrainAsync(IEnumerable<ErrorContext> trainingData);

        /// <summary>
        /// Validates the model with test data.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns>The validation metrics.</returns>
        Task<Dictionary<string, double>> ValidateAsync(IEnumerable<ErrorContext> testData);

        /// <summary>
        /// Gets the model's performance metrics.
        /// </summary>
        /// <returns>The performance metrics.</returns>
        Task<Dictionary<string, double>> GetPerformanceMetricsAsync();

        /// <summary>
        /// Updates the model with new data.
        /// </summary>
        /// <param name="updateData">The update data.</param>
        /// <returns>A task representing the update operation.</returns>
        Task UpdateAsync(IEnumerable<ErrorContext> updateData);

        /// <summary>
        /// Saves the model to storage.
        /// </summary>
        /// <returns>A task representing the save operation.</returns>
        Task SaveAsync();

        /// <summary>
        /// Gets the model's metadata.
        /// </summary>
        /// <returns>The model metadata.</returns>
        Dictionary<string, object> GetMetadata();
    }
} 