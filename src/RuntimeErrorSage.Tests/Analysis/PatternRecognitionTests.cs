using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Interfaces;
using RuntimeErrorSage.Application.Options;
using RuntimeErrorSage.Application.Storage.Interfaces;
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

            _patternRecognition = new PatternRecognition(
                _loggerMock.Object,
                _mcpClientMock.Object,
                Options.Create(_options),
                _modelMock.Object,
                _storageMock.Object);
        }

        [Fact]
        public async Task InitializeAsync_Success()
        {
            // Arrange
            var patterns = new Collection<ErrorPattern>();
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
            await Assert.ThrowsAsync<PatternRecognitionException>(
                () => _patternRecognition.InitializeAsync());
        }

        [Fact]
        public async Task DetectPatternsAsync_ReturnsMatchingPatterns()
        {
            // Arrange
            var contexts = new Collection<ErrorContext>
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

            var patterns = new Collection<ErrorPattern>
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

            var patterns = new Collection<ErrorPattern>
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

            var patterns = new Collection<ErrorPattern>
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
            var contexts = new Collection<ErrorContext>();
            var patterns = new Collection<ErrorPattern>();

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
            var contexts = new Collection<ErrorContext>
            {
                CreateTestContext("Error1", new Dictionary<string, object>())
            };

            _mcpClientMock.Setup(x => x.GetErrorPatternsAsync(_options.ServiceName))
                .ThrowsAsync(new Exception("Test error"));

            // Act & Assert
            await Assert.ThrowsAsync<PatternRecognitionException>(
                () => _patternRecognition.DetectPatternsAsync(contexts, _options.ServiceName));
        }

        private static ErrorContext CreateTestContext(string errorType, Dictionary<string, object> additionalContext)
        {
            return new ErrorContext
            {
                ErrorType = errorType,
                AdditionalContext = additionalContext,
                Timestamp = DateTime.UtcNow
            };
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




