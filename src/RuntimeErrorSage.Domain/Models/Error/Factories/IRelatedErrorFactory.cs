using RuntimeErrorSage.Application.Models.Error;

namespace RuntimeErrorSage.Application.Models.Error.Factories
{
    /// <summary>
    /// Interface for creating RelatedError instances.
    /// </summary>
    public interface IRelatedErrorFactory
    {
        /// <summary>
        /// Creates a new RelatedError instance.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorType">The error type.</param>
        /// <param name="correlationId">The correlation ID.</param>
        /// <param name="relationshipType">The relationship type.</param>
        /// <returns>A new RelatedError instance.</returns>
        RelatedError Create(string errorMessage, string errorType, string correlationId, string relationshipType);
    }
} 