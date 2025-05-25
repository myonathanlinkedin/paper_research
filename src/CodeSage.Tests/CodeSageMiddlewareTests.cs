using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using CodeSage.Core;
using CodeSage.Middleware;

namespace CodeSage.Tests
{
    public class CodeSageMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_InterceptsException_EnrichesContextAndAppliesRemediation()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CodeSageMiddleware>>();
            var mockCodeSageService = new Mock<ICodeSageService>();
            var exception = new InvalidOperationException("Test exception");
            var errorContext = new ErrorContext 
            { 
                CorrelationId = "test-correlation-id", 
                Timestamp = DateTime.UtcNow,
                ServiceName = "TestService",
                OperationName = "TestOperation"
            };
            var analysis = new ErrorAnalysisResult 
            { 
                RemediationStrategy = "Test remediation", 
                Severity = SeverityLevel.Medium,
                CanAutoRemediate = true
            };
            var remediation = new RemediationResult 
            { 
                Success = true,
                ActionTaken = "Test remediation applied",
                RemediationTimestamp = DateTime.UtcNow
            };

            mockCodeSageService
                .Setup(x => x.ProcessExceptionAsync(It.IsAny<Exception>(), It.IsAny<ErrorContext>()))
                .ReturnsAsync(analysis);

            mockCodeSageService
                .Setup(x => x.ApplyRemediationAsync(It.IsAny<ErrorAnalysisResult>()))
                .ReturnsAsync(remediation);

            var middleware = new CodeSageMiddleware(
                next: (ctx) => throw exception,
                mockCodeSageService.Object,
                mockLogger.Object);

            var context = new DefaultHttpContext();
            context.TraceIdentifier = "test-correlation-id";

            // Act & Assert
            var thrownException = await Assert.ThrowsAsync<InvalidOperationException>(
                () => middleware.InvokeAsync(context));

            Assert.Equal("Test exception", thrownException.Message);

            // Verify that ProcessExceptionAsync was called with the expected parameters
            mockCodeSageService.Verify(
                x => x.ProcessExceptionAsync(
                    It.Is<Exception>(e => e.Message == "Test exception"),
                    It.Is<ErrorContext>(ec => ec.CorrelationId == "test-correlation-id")),
                Times.Once);

            // Verify that ApplyRemediationAsync was called with the expected analysis
            mockCodeSageService.Verify(
                x => x.ApplyRemediationAsync(
                    It.Is<ErrorAnalysisResult>(a => 
                        a.RemediationStrategy == "Test remediation" && 
                        a.Severity == SeverityLevel.Medium)),
                Times.Once);
        }
    }
} 