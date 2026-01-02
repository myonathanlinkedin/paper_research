using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Interfaces;
using RuntimeErrorSage.Application.Options;
using RuntimeErrorSage.Application.Storage.Interfaces;
using RuntimeErrorSage.Core.Analysis;
using Xunit;

namespace RuntimeErrorSage.Tests.Analysis
{
    public class PatternRecognitionTests
    {
        private readonly Mock<ILogger<PatternRecognition>> _loggerMock;
        private readonly Mock<IMCPClient> _mcpClientMock;
        private readonly Mock<IModel> _modelMock;
        private readonly Mock<IStorage> _storageMock;
        private readonly PatternRecognitionOptions _options;
        private readonly PatternRecognition _patternRecognition;

        public PatternRecognitionTests()
        {
            _loggerMock = new Mock<ILogger<PatternRecognition>>();
            _mcpClientMock = new Mock<IMCPClient>();
            _modelMock = new Mock<IModel>();
            _storageMock = new Mock<IStorage>();
            _options = new PatternRecognitionOptions
            {
                ServiceName = "TestService",
                SimilarityThreshold = 0.8f,
                MinPatternOccurrences = 3,
                PatternWindow = TimeSpan.FromHours(24),
                TemporalWindow = TimeSpan.FromMinutes(5),
                MaxPatternFeatures = 100,
                EnableTemporalAnalysis = true,
                EnableCrossServicePatterns = true
            };

            var runtimeErrorSageOptions = new RuntimeErrorSageOptions();
            _patternRecognition = new PatternRecognition(
                _loggerMock.Object,
                _mcpClientMock.Object,
                Options.Create(runtimeErrorSageOptions),
                _modelMock.Object,
                _storageMock.Object);
        }

        [Fact]
        public async Task InitializeAsync_Success()
        {
            // Arrange
            var patterns = new List<ErrorPattern>();
            _storageMock.Setup(x => x.LoadPatternsAsync())
                .ReturnsAsync(patterns);
            _modelMock.Setup(x => x.InitializeAsync())
                .Returns(Task.CompletedTask);
            _modelMock.Setup(x => x.ValidatePatternAsync(It.IsAny<ErrorPattern>()))
                .ReturnsAsync(true);

            // Act
            await _patternRecognition.InitializeAsync();

            // Assert
            _storageMock.Verify(x => x.LoadPatternsAsync(), Times.Once);
            _modelMock.Verify(x => x.InitializeAsync(), Times.Once);
        }

        [Fact]
        public async Task InitializeAsync_ThrowsOnError()
        {
            // Arrange
            _storageMock.Setup(x => x.LoadPatternsAsync())
                .ThrowsAsync(new Exception("Test error"));

            // Act & Assert
            await Assert.ThrowsAsync<RuntimeErrorSage.Application.Exceptions.PatternRecognitionException>(
                () => _patternRecognition.InitializeAsync());
        }

        [Fact]
        public async Task DetectPatternsAsync_ReturnsMatchingPatterns()
        {
            // Arrange
            var contexts = new List<ErrorContext>
            {
                CreateTestContext("Error1", new Dictionary<string, object>
                {
                    { "DatabaseName", "TestDB" },
                    { "FilePath", "test.txt" }
                }),
                CreateTestContext("Error2", new Dictionary<string, object>
                {
                    { "DatabaseName", "TestDB" },
                    { "ServiceEndpoint", "http://test" }
                })
            };

            var patterns = new List<ErrorPattern>
            {
                CreateTestPattern("Error1", new Dictionary<string, object>
                {
                    { "DatabaseName", "TestDB" },
                    { "FilePath", "test.txt" }
                })
            };

            _mcpClientMock.Setup(x => x.GetErrorPatternsAsync(_options.ServiceName))
                .ReturnsAsync(patterns);
            _modelMock.Setup(x => x.ValidatePatternAsync(It.IsAny<ErrorPattern>()))
                .ReturnsAsync(true);

            // Act
            var result = await _patternRecognition.DetectPatternsAsync(contexts, _options.ServiceName);

            // Assert
            Assert.Single(result);
            Assert.Equal("Error1", result[0].ErrorType);
        }

        [Fact]
        public async Task FindMatchingPatternAsync_ReturnsMatchingPattern()
        {
            // Arrange
            var context = CreateTestContext("Error1", new Dictionary<string, object>
            {
                { "DatabaseName", "TestDB" },
                { "FilePath", "test.txt" }
            });

            var patterns = new List<ErrorPattern>
            {
                CreateTestPattern("Error1", new Dictionary<string, object>
                {
                    { "DatabaseName", "TestDB" },
                    { "FilePath", "test.txt" }
                })
            };

            _mcpClientMock.Setup(x => x.GetErrorPatternsAsync(_options.ServiceName))
                .ReturnsAsync(patterns);
            _modelMock.Setup(x => x.ValidatePatternAsync(It.IsAny<ErrorPattern>()))
                .ReturnsAsync(true);

            // Act
            var result = await _patternRecognition.FindMatchingPatternAsync(context, _options.ServiceName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Error1", result.ErrorType);
        }

        [Fact]
        public async Task FindMatchingPatternAsync_ReturnsNullWhenNoMatch()
        {
            // Arrange
            var context = CreateTestContext("Error1", new Dictionary<string, object>
            {
                { "DatabaseName", "TestDB" },
                { "FilePath", "test.txt" }
            });

            var patterns = new List<ErrorPattern>
            {
                CreateTestPattern("Error2", new Dictionary<string, object>
                {
                    { "DatabaseName", "TestDB" },
                    { "FilePath", "test.txt" }
                })
            };

            _mcpClientMock.Setup(x => x.GetErrorPatternsAsync(_options.ServiceName))
                .ReturnsAsync(patterns);

            // Act
            var result = await _patternRecognition.FindMatchingPatternAsync(context, _options.ServiceName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DetectPatternsAsync_HandlesEmptyContextList()
        {
            // Arrange
            var contexts = new List<ErrorContext>();
            var patterns = new List<ErrorPattern>();

            _mcpClientMock.Setup(x => x.GetErrorPatternsAsync(_options.ServiceName))
                .ReturnsAsync(patterns);

            // Act
            var result = await _patternRecognition.DetectPatternsAsync(contexts, _options.ServiceName);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task DetectPatternsAsync_ThrowsOnError()
        {
            // Arrange
            var contexts = new List<ErrorContext>
            {
                CreateTestContext("Error1", new Dictionary<string, object>())
            };

            _mcpClientMock.Setup(x => x.GetErrorPatternsAsync(_options.ServiceName))
                .ThrowsAsync(new Exception("Test error"));

            // Act & Assert
            await Assert.ThrowsAsync<RuntimeErrorSage.Application.Exceptions.PatternRecognitionException>(
                () => _patternRecognition.DetectPatternsAsync(contexts, _options.ServiceName));
        }

        private static ErrorContext CreateTestContext(string errorType, Dictionary<string, object> additionalContext)
        {
            var error = new RuntimeError(
                message: $"Test error of type {errorType}",
                errorType: errorType,
                source: "TestService",
                stackTrace: string.Empty
            );

            var context = new ErrorContext(
                error: error,
                context: "TestService",
                timestamp: DateTime.UtcNow
            );

            foreach (var kvp in additionalContext)
            {
                context.AddMetadata(kvp.Key, kvp.Value);
            }

            return context;
        }

        private static ErrorPattern CreateTestPattern(string errorType, Dictionary<string, object> context)
        {
            return new ErrorPattern
            {
                ErrorType = errorType,
                Context = context,
                PatternType = "TestPattern",
                Confidence = 0.9f,
                LastOccurrence = DateTime.UtcNow,
                OccurrenceCount = 1
            };
        }
    }
} 

