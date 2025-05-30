using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Application.LLM;

namespace RuntimeErrorSage.Tests.Analysis
{
    public class ErrorAnalyzerTests
    {
        private readonly Mock<ILMStudioClient> _llmClientMock;
        private readonly Mock<ILogger<ErrorAnalyzer>> _loggerMock;
        private readonly ErrorAnalyzer _analyzer;

        public ErrorAnalyzerTests()
        {
            _llmClientMock = new Mock<ILMStudioClient>();
            _loggerMock = new Mock<ILogger<ErrorAnalyzer>>();
            _analyzer = new ErrorAnalyzer(_llmClientMock.Object);
        }

        [Fact]
        public async Task AnalyzeErrorAsync_ValidContext_ReturnsAnalysis()
        {
            // Arrange
            var error = new RuntimeError(
                message: "Test error",
                errorType: "InvalidOperationError",
                source: "TestService",
                stackTrace: string.Empty
            );

            var context = new ErrorContext(
                error: error,
                context: "TestService",
                timestamp: DateTime.UtcNow
            );

            var llmResponse = @"Root Cause: Test root cause
Confidence: 0.8
Step 1: First remediation step
Step 2: Second remediation step";

            _llmClientMock
                .Setup(x => x.AnalyzeErrorAsync(It.IsAny<string>()))
                .ReturnsAsync(llmResponse);

            // Act
            var result = await _analyzer.AnalyzeErrorAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test root cause", result.RootCause);
            Assert.Equal(0.8f, result.Confidence);
            Assert.Equal(2, result.RemediationSteps.Count);
            Assert.Equal("First remediation step", result.RemediationSteps[0]);
            Assert.Equal("Second remediation step", result.RemediationSteps[1]);
            Assert.True(result.Accuracy > 0);
            Assert.True(result.Latency > 0);
            Assert.True(result.MemoryUsage > 0);
        }

        [Fact]
        public async Task AnalyzeErrorAsync_DatabaseError_ReturnsAnalysis()
        {
            // Arrange
            var error = new RuntimeError(
                message: "Database error",
                errorType: "DatabaseError",
                source: "TestService",
                stackTrace: string.Empty
            );

            var context = new ErrorContext(
                error: error,
                context: "TestService",
                timestamp: DateTime.UtcNow
            );

            context.AddMetadata("DatabaseName", "TestDB");
            context.AddMetadata("Query", "SELECT * FROM Test");

            var llmResponse = @"Root Cause: Database connection timeout
Confidence: 0.9
Step 1: Check database server status
Step 2: Verify connection string
Step 3: Check network connectivity";

            _llmClientMock
                .Setup(x => x.AnalyzeErrorAsync(It.IsAny<string>()))
                .ReturnsAsync(llmResponse);

            // Act
            var result = await _analyzer.AnalyzeErrorAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Database connection timeout", result.RootCause);
            Assert.Equal(0.9f, result.Confidence);
            Assert.Equal(3, result.RemediationSteps.Count);
            Assert.Contains("database", result.RootCause.ToLower());
            Assert.True(result.Accuracy > 0.7f);
        }

        [Fact]
        public async Task AnalyzeErrorAsync_HttpError_ReturnsAnalysis()
        {
            // Arrange
            var error = new RuntimeError(
                message: "HTTP error",
                errorType: "HttpRequestError",
                source: "TestService",
                stackTrace: string.Empty
            );

            var context = new ErrorContext(
                error: error,
                context: "TestService",
                timestamp: DateTime.UtcNow
            );

            context.AddMetadata("Url", "http://test.com");
            context.AddMetadata("StatusCode", "500");

            var llmResponse = @"Root Cause: Internal server error
Confidence: 0.85
Step 1: Check server logs
Step 2: Verify endpoint availability
Step 3: Test with different parameters";

            _llmClientMock
                .Setup(x => x.AnalyzeErrorAsync(It.IsAny<string>()))
                .ReturnsAsync(llmResponse);

            // Act
            var result = await _analyzer.AnalyzeErrorAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Internal server error", result.RootCause);
            Assert.Equal(0.85f, result.Confidence);
            Assert.Equal(3, result.RemediationSteps.Count);
            Assert.Contains("server", result.RootCause.ToLower());
            Assert.True(result.Accuracy > 0.7f);
        }

        [Fact]
        public async Task AnalyzeErrorAsync_LLMError_ReturnsErrorResult()
        {
            // Arrange
            var error = new RuntimeError(
                message: "Test error",
                errorType: "TestError",
                source: "TestService",
                stackTrace: string.Empty
            );

            var context = new ErrorContext(
                error: error,
                context: "TestService",
                timestamp: DateTime.UtcNow
            );

            _llmClientMock
                .Setup(x => x.AnalyzeErrorAsync(It.IsAny<string>()))
                .ThrowsAsync(new LMStudioException("LLM error"));

            // Act
            var result = await _analyzer.AnalyzeErrorAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Error during analysis", result.RootCause);
            Assert.Equal(0, result.Confidence);
            Assert.Equal(0, result.Accuracy);
            Assert.NotNull(result.Error);
            Assert.Equal("LLM error", result.Error.Message);
        }

        [Fact]
        public async Task AnalyzeErrorAsync_InvalidResponse_ReturnsLowAccuracy()
        {
            // Arrange
            var error = new RuntimeError(
                message: "Test error",
                errorType: "TestError",
                source: "TestService",
                stackTrace: string.Empty
            );

            var context = new ErrorContext(
                error: error,
                context: "TestService",
                timestamp: DateTime.UtcNow
            );

            var llmResponse = "Invalid response format";

            _llmClientMock
                .Setup(x => x.AnalyzeErrorAsync(It.IsAny<string>()))
                .ReturnsAsync(llmResponse);

            // Act
            var result = await _analyzer.AnalyzeErrorAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.RootCause));
            Assert.Equal(0, result.Confidence);
            Assert.Equal(0, result.RemediationSteps.Count);
        }
    }
} 

