using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using RuntimeErrorSage.Application;
using RuntimeErrorSage.Middleware;
using FluentAssertions;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Application.Remediation;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Application.Validation;
using RuntimeErrorSage.Application.Graph;
using RuntimeErrorSage.Application.Runtime.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using ErrorAnalysisResult = RuntimeErrorSage.Domain.Models.Error.ErrorAnalysisResult;
using RuntimeErrorSage.Core.Remediation.Base;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Tests
{
    public class RuntimeErrorSageMiddlewareTests
    {
        private readonly Mock<IRuntimeErrorSageService> _serviceMock;
        private readonly Mock<ILogger<RuntimeErrorSageMiddleware>> _loggerMock;
        private readonly RuntimeErrorSageMiddleware _middleware;

        public RuntimeErrorSageMiddlewareTests()
        {
            _serviceMock = new Mock<IRuntimeErrorSageService>();
            _loggerMock = new Mock<ILogger<RuntimeErrorSageMiddleware>>();
            _middleware = new RuntimeErrorSageMiddleware(
                next: (innerHttpContext) => throw new Exception("Test exception"),
                service: _serviceMock.Object,
                logger: _loggerMock.Object);
        }

        [Fact]
        public async Task ShouldAnalyzeAndRemediateError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/test";
            context.Request.Method = "GET";
            context.Response.StatusCode = 500;

            var analysisResult = new ErrorAnalysisResult
            {
                IsAnalyzed = true,
                RemediationPlan = new RemediationPlan(
                    name: "Test Remediation Plan",
                    description: "Test plan for middleware",
                    actions: new List<RemediationAction>(),
                    parameters: new Dictionary<string, object>(),
                    estimatedDuration: TimeSpan.FromMinutes(5))
                {
                    Strategies = new List<RuntimeErrorSage.Domain.Interfaces.IRemediationStrategy>()
                }
            };

            _serviceMock.Setup(x => x.AnalyzeErrorAsync(It.IsAny<ErrorContext>()))
                .ReturnsAsync(analysisResult);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _middleware.InvokeAsync(context));

            _serviceMock.Verify(x => x.AnalyzeErrorAsync(It.Is<ErrorContext>(c =>
                c.ErrorType == "Exception" &&
                c.ErrorMessage == "Test exception" &&
                c.Source == "Middleware" &&
                c.AdditionalContext["RequestPath"].ToString() == "/api/test" &&
                c.AdditionalContext["RequestMethod"].ToString() == "GET" &&
                c.AdditionalContext["StatusCode"].ToString() == "500")), Times.Once);

            _serviceMock.Verify(x => x.RemediateErrorAsync(It.IsAny<ErrorContext>()), Times.Once);
        }

        [Fact]
        public async Task ShouldHandleAnalysisError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _serviceMock.Setup(x => x.AnalyzeErrorAsync(It.IsAny<ErrorContext>()))
                .ThrowsAsync(new Exception("Analysis failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _middleware.InvokeAsync(context));

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error analyzing or remediating exception in middleware")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
} 

