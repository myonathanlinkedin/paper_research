using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using CodeSage.Core.LLM;

namespace CodeSage.Tests.LLM
{
    public class LMStudioClientTests
    {
        private readonly Mock<ILogger<LMStudioClient>> _loggerMock;
        private readonly Mock<IOptions<LMStudioOptions>> _optionsMock;
        private readonly LMStudioOptions _options;
        private readonly HttpClient _httpClient;
        private readonly LMStudioClient _client;

        public LMStudioClientTests()
        {
            _loggerMock = new Mock<ILogger<LMStudioClient>>();
            _options = new LMStudioOptions
            {
                BaseUrl = "http://localhost:1234",
                ModelId = "test-model",
                TimeoutSeconds = 30,
                MaxTokens = 100,
                Temperature = 0.7f
            };
            _optionsMock = new Mock<IOptions<LMStudioOptions>>();
            _optionsMock.Setup(x => x.Value).Returns(_options);

            _httpClient = new HttpClient(new TestHttpMessageHandler());
            _client = new LMStudioClient(_httpClient, _optionsMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AnalyzeErrorAsync_ValidPrompt_ReturnsResponse()
        {
            // Arrange
            var prompt = "Test prompt";
            var expectedResponse = new LMStudioResponse
            {
                Choices = new List<Choice>
                {
                    new Choice { Text = "Test response" }
                }
            };

            TestHttpMessageHandler.SetResponse(
                "/v1/completions",
                HttpStatusCode.OK,
                JsonSerializer.Serialize(expectedResponse));

            // Act
            var result = await _client.AnalyzeErrorAsync(prompt);

            // Assert
            Assert.Equal("Test response", result);
        }

        [Fact]
        public async Task AnalyzeErrorAsync_ModelNotReady_ThrowsException()
        {
            // Arrange
            TestHttpMessageHandler.SetResponse(
                "/v1/models",
                HttpStatusCode.OK,
                JsonSerializer.Serialize(new LMStudioModelsResponse
                {
                    Data = new List<ModelInfo>
                    {
                        new ModelInfo { Id = "test-model", Status = "loading" }
                    }
                }));

            // Act & Assert
            await Assert.ThrowsAsync<LMStudioException>(
                () => _client.AnalyzeErrorAsync("Test prompt"));
        }

        [Fact]
        public async Task IsModelReadyAsync_ModelReady_ReturnsTrue()
        {
            // Arrange
            TestHttpMessageHandler.SetResponse(
                "/v1/models",
                HttpStatusCode.OK,
                JsonSerializer.Serialize(new LMStudioModelsResponse
                {
                    Data = new List<ModelInfo>
                    {
                        new ModelInfo { Id = "test-model", Status = "ready" }
                    }
                }));

            // Act
            var result = await _client.IsModelReadyAsync();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsModelReadyAsync_ModelNotReady_ReturnsFalse()
        {
            // Arrange
            TestHttpMessageHandler.SetResponse(
                "/v1/models",
                HttpStatusCode.OK,
                JsonSerializer.Serialize(new LMStudioModelsResponse
                {
                    Data = new List<ModelInfo>
                    {
                        new ModelInfo { Id = "test-model", Status = "loading" }
                    }
                }));

            // Act
            var result = await _client.IsModelReadyAsync();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsModelReadyAsync_HttpError_ReturnsFalse()
        {
            // Arrange
            TestHttpMessageHandler.SetResponse(
                "/v1/models",
                HttpStatusCode.InternalServerError,
                "Server error");

            // Act
            var result = await _client.IsModelReadyAsync();

            // Assert
            Assert.False(result);
        }

        private class TestHttpMessageHandler : HttpMessageHandler
        {
            private static readonly Dictionary<string, (HttpStatusCode StatusCode, string Content)> _responses
                = new Dictionary<string, (HttpStatusCode, string)>();

            public static void SetResponse(string path, HttpStatusCode statusCode, string content)
            {
                _responses[path] = (statusCode, content);
            }

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                System.Threading.CancellationToken cancellationToken)
            {
                var path = request.RequestUri.AbsolutePath;
                if (_responses.TryGetValue(path, out var response))
                {
                    return new HttpResponseMessage(response.StatusCode)
                    {
                        Content = new StringContent(response.Content, Encoding.UTF8, "application/json")
                    };
                }

                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
} 