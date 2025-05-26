using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using CodeSage.Core.Health;
using CodeSage.Core.LLM;

namespace CodeSage.Tests.Health
{
    public class LMStudioHealthCheckTests
    {
        private readonly Mock<ILMStudioClient> _llmClientMock;
        private readonly Mock<IOptions<LMStudioOptions>> _optionsMock;
        private readonly Mock<IOptions<CodeSageOptions>> _codeSageOptionsMock;
        private readonly LMStudioHealthCheck _healthCheck;

        public LMStudioHealthCheckTests()
        {
            _llmClientMock = new Mock<ILMStudioClient>();
            _optionsMock = new Mock<IOptions<LMStudioOptions>>();
            _codeSageOptionsMock = new Mock<IOptions<CodeSageOptions>>();

            _optionsMock.Setup(x => x.Value).Returns(new LMStudioOptions
            {
                BaseUrl = "http://localhost:1234",
                ModelId = "test-model"
            });

            _codeSageOptionsMock.Setup(x => x.Value).Returns(new CodeSageOptions
            {
                EnableErrorAnalysis = true
            });

            _healthCheck = new LMStudioHealthCheck(
                _llmClientMock.Object,
                _optionsMock.Object,
                _codeSageOptionsMock.Object);
        }

        [Fact]
        public async Task CheckHealthAsync_WhenDisabled_ReturnsHealthy()
        {
            // Arrange
            _codeSageOptionsMock.Setup(x => x.Value).Returns(new CodeSageOptions
            {
                EnableErrorAnalysis = false
            });

            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext());

            // Assert
            Assert.Equal(HealthStatus.Healthy, result.Status);
            Assert.Equal("LM Studio integration is disabled", result.Description);
        }

        [Fact]
        public async Task CheckHealthAsync_WhenModelReady_ReturnsHealthy()
        {
            // Arrange
            _llmClientMock
                .Setup(x => x.IsModelReadyAsync())
                .ReturnsAsync(true);

            _llmClientMock
                .Setup(x => x.AnalyzeErrorAsync(It.IsAny<string>()))
                .ReturnsAsync("Test response");

            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext());

            // Assert
            Assert.Equal(HealthStatus.Healthy, result.Status);
            Assert.Contains("LM Studio is healthy", result.Description);
            Assert.Contains("test-model", result.Data["ModelId"].ToString());
            Assert.Contains("http://localhost:1234", result.Data["BaseUrl"].ToString());
            Assert.Contains("ResponseLength", result.Data.Keys);
        }

        [Fact]
        public async Task CheckHealthAsync_WhenModelNotReady_ReturnsUnhealthy()
        {
            // Arrange
            _llmClientMock
                .Setup(x => x.IsModelReadyAsync())
                .ReturnsAsync(false);

            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext());

            // Assert
            Assert.Equal(HealthStatus.Unhealthy, result.Status);
            Assert.Contains("LM Studio model is not ready", result.Description);
            Assert.Contains("test-model", result.Data["ModelId"].ToString());
            Assert.Contains("http://localhost:1234", result.Data["BaseUrl"].ToString());
        }

        [Fact]
        public async Task CheckHealthAsync_WhenEmptyResponse_ReturnsDegraded()
        {
            // Arrange
            _llmClientMock
                .Setup(x => x.IsModelReadyAsync())
                .ReturnsAsync(true);

            _llmClientMock
                .Setup(x => x.AnalyzeErrorAsync(It.IsAny<string>()))
                .ReturnsAsync(string.Empty);

            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext());

            // Assert
            Assert.Equal(HealthStatus.Degraded, result.Status);
            Assert.Contains("LM Studio returned empty response", result.Description);
            Assert.Contains("test-model", result.Data["ModelId"].ToString());
            Assert.Contains("http://localhost:1234", result.Data["BaseUrl"].ToString());
        }

        [Fact]
        public async Task CheckHealthAsync_WhenError_ReturnsUnhealthy()
        {
            // Arrange
            _llmClientMock
                .Setup(x => x.IsModelReadyAsync())
                .ThrowsAsync(new LMStudioException("Test error"));

            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext());

            // Assert
            Assert.Equal(HealthStatus.Unhealthy, result.Status);
            Assert.Contains("LM Studio health check failed", result.Description);
            Assert.Contains("Test error", result.Data["Error"].ToString());
            Assert.Contains("test-model", result.Data["ModelId"].ToString());
            Assert.Contains("http://localhost:1234", result.Data["BaseUrl"].ToString());
        }
    }
} 