using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RuntimeErrorSage.Core.Analysis.Interfaces;
using RuntimeErrorSage.Core.MCP;
using RuntimeErrorSage.Core.MCP.Interfaces;
using RuntimeErrorSage.Core.Models.Context;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.MCP;
using RuntimeErrorSage.Core.Storage.Interfaces;
using Xunit;

namespace RuntimeErrorSage.Core.Tests.MCP
{
    public class MCPClientTests
    {
        private readonly Mock<ILogger<MCPClient>> _loggerMock;
        private readonly Mock<IOptions<MCPClientOptions>> _optionsMock;
        private readonly Mock<IPatternStorage> _storageMock;
        private readonly Mock<IErrorAnalyzer> _errorAnalyzerMock;
        private readonly MCPClient _client;

        public MCPClientTests()
        {
            _loggerMock = new Mock<ILogger<MCPClient>>();
            _optionsMock = new Mock<IOptions<MCPClientOptions>>();
            _storageMock = new Mock<IPatternStorage>();
            _errorAnalyzerMock = new Mock<IErrorAnalyzer>();

            _optionsMock.Setup(x => x.Value).Returns(new MCPClientOptions());

            _client = new MCPClient(
                _loggerMock.Object,
                _optionsMock.Object,
                _storageMock.Object,
                _errorAnalyzerMock.Object);
        }

        [Fact]
        public async Task ConnectAsync_ShouldConnectSuccessfully()
        {
            // Arrange
            _storageMock.Setup(x => x.ValidateConnectionAsync())
                .ReturnsAsync(true);

            // Act
            await _client.ConnectAsync();

            // Assert
            Assert.True(_client.IsConnected);
            Assert.Equal(ConnectionState.Connected, _client.ConnectionStatus.State);
        }

        [Fact]
        public async Task AnalyzeErrorAsync_ShouldAnalyzeContextCorrectly()
        {
            // Arrange
            var context = new ErrorContext
            {
                ErrorId = "test-error-1",
                ErrorType = "RuntimeError",
                Message = "Test error message",
                ComponentGraph = new List<ComponentNode>()
            };

            _errorAnalyzerMock.Setup(x => x.AnalyzeContextAsync(It.IsAny<ErrorContext>()))
                .ReturnsAsync(new Dictionary<string, object>());

            // Act
            var result = await _client.AnalyzeErrorAsync(context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(context.ErrorId, result.ErrorId);
            Assert.True(result.Metadata.ContainsKey("AnalysisResult"));
        }

        [Fact]
        public async Task GetContextHistoryAsync_ShouldReturnHistoryForTimeRange()
        {
            // Arrange
            var contextId = "test-context-1";
            var range = new TimeRange
            {
                Start = DateTime.UtcNow.AddHours(-1),
                End = DateTime.UtcNow
            };

            // Act
            var history = await _client.GetContextHistoryAsync(contextId, range);

            // Assert
            Assert.NotNull(history);
            Assert.Empty(history); // Initially empty
        }

        [Fact]
        public async Task PublishContextAsync_ShouldUpdateCacheAndNotifySubscribers()
        {
            // Arrange
            var context = new ErrorContext
            {
                ErrorId = "test-error-2",
                ErrorType = "RuntimeError",
                Message = "Test error message",
                ComponentGraph = new List<ComponentNode>()
            };

            var callbackCalled = false;
            await _client.SubscribeToContextUpdatesAsync(context.ErrorId, async ctx =>
            {
                callbackCalled = true;
                await Task.CompletedTask;
            });

            // Act
            await _client.PublishContextAsync(context);

            // Assert
            Assert.True(callbackCalled);
        }

        [Fact]
        public async Task GetAvailableModelsAsync_ShouldReturnQwenModel()
        {
            // Act
            var models = await _client.GetAvailableModelsAsync();

            // Assert
            Assert.Single(models);
            Assert.Contains("Qwen2.5-7B-Instruct-1M", models);
        }

        [Fact]
        public async Task GetModelMetadataAsync_ShouldReturnCorrectMetadata()
        {
            // Arrange
            var modelId = "Qwen2.5-7B-Instruct-1M";

            // Act
            var metadata = await _client.GetModelMetadataAsync(modelId);

            // Assert
            Assert.NotNull(metadata);
            Assert.Equal(modelId, metadata["ModelId"]);
            Assert.Contains("ErrorAnalysis", (string[])metadata["Capabilities"]);
            Assert.Contains("ContextEnrichment", (string[])metadata["Capabilities"]);
            Assert.Contains("RemediationPlanning", (string[])metadata["Capabilities"]);
        }

        [Fact]
        public async Task GetServerStatisticsAsync_ShouldReturnCorrectStats()
        {
            // Act
            var stats = await _client.GetServerStatisticsAsync();

            // Assert
            Assert.NotNull(stats);
            Assert.Equal(1, stats["ConnectedClients"]);
            Assert.Equal(0, stats["ActiveContexts"]);
            Assert.Equal(0, stats["TotalAnalyses"]);
        }

        [Fact]
        public async Task GetClientStatisticsAsync_ShouldReturnCorrectStats()
        {
            // Act
            var stats = await _client.GetClientStatisticsAsync();

            // Assert
            Assert.NotNull(stats);
            Assert.Equal(_client.ClientId, stats["ClientId"]);
            Assert.Equal(0, stats["ActiveSubscriptions"]);
            Assert.Equal(0, stats["CachedContexts"]);
            Assert.Equal(0, stats["TotalHistoryEntries"]);
        }
    }
} 
