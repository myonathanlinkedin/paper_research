using System;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Error.Factories
{
    /// <summary>
    /// Factory for creating RelatedError instances.
    /// </summary>
    public class RelatedErrorFactory : IRelatedErrorFactory
    {
        private readonly IRuntimeErrorFactory _runtimeErrorFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedErrorFactory"/> class.
        /// </summary>
        /// <param name="runtimeErrorFactory">The runtime error factory.</param>
        public RelatedErrorFactory(IRuntimeErrorFactory runtimeErrorFactory)
        {
            _runtimeErrorFactory = runtimeErrorFactory ?? throw new ArgumentNullException(nameof(runtimeErrorFactory));
        }

        /// <summary>
        /// Creates a new RelatedError instance.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorType">The error type.</param>
        /// <param name="correlationId">The correlation ID.</param>
        /// <param name="relationshipType">The relationship type.</param>
        /// <returns>A new RelatedError instance.</returns>
        public RelatedError Create(string errorMessage, string errorType, string correlationId, string relationshipType)
        {
            return new RelatedError
            {
                ErrorId = _runtimeErrorFactory.Create(errorMessage, errorType).Id,
                CorrelationId = correlationId,
                RelationshipType = Enum.Parse<ErrorRelationshipType>(relationshipType),
                Timestamp = DateTime.UtcNow
            };
        }

        public RelatedError CreateRelatedError(RuntimeError error, string relationshipType)
        {
            var relatedError = new RelatedError
            {
                ErrorId = error.Id,
                RelationshipType = Enum.Parse<ErrorRelationshipType>(relationshipType)
            };
            return relatedError;
        }
    }
} 
