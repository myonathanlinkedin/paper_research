using Xunit;
using FluentAssertions;
using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Core.MCP;
using Moq;
using System.Threading.Tasks;
using RuntimeErrorSage.Tests.Helpers;

namespace RuntimeErrorSage.Tests.Scenarios;

public class HttpClientErrorScenarios
{
    private readonly Mock<IMCPClient> _mcpClientMock;
    private readonly Mock<IRemediationExecutor> _remediationExecutorMock;
    private readonly RuntimeErrorSageService _service;

    public HttpClientErrorScenarios()
    {
        _mcpClientMock = TestHelper.CreateMCPClientMock();
        _remediationExecutorMock = TestHelper.CreateRemediationExecutorMock();
        _service = new RuntimeErrorSageService(_mcpClientMock.Object, _remediationExecutorMock.Object);
    }

    [Theory]
    [InlineData("ConnectionTimeout", "The HTTP request timed out after 30 seconds")]
    [InlineData("ConnectionRefused", "Unable to connect to the remote server")]
    [InlineData("DNSResolutionFailed", "Could not resolve host name 'api.example.com'")]
    [InlineData("SSLHandshakeFailed", "SSL handshake failed with remote server")]
    [InlineData("CertificateValidationFailed", "The remote certificate is invalid")]
    [InlineData("ProxyAuthenticationFailed", "Proxy authentication failed")]
    [InlineData("ProxyConnectionFailed", "Unable to connect to proxy server")]
    [InlineData("RequestTimeout", "The request timed out")]
    [InlineData("RequestCancelled", "The request was cancelled")]
    [InlineData("RequestAborted", "The request was aborted")]
    [InlineData("RequestFailed", "The request failed")]
    [InlineData("ResponseTimeout", "The response timed out")]
    [InlineData("ResponseAborted", "The response was aborted")]
    [InlineData("ResponseCancelled", "The response was cancelled")]
    [InlineData("ResponseFailed", "The response failed")]
    [InlineData("ContentLengthMismatch", "The content length does not match the expected length")]
    [InlineData("ContentTypeMismatch", "The content type does not match the expected type")]
    [InlineData("ContentEncodingMismatch", "The content encoding does not match the expected encoding")]
    [InlineData("ContentDispositionMismatch", "The content disposition does not match the expected disposition")]
    [InlineData("ContentRangeMismatch", "The content range does not match the expected range")]
    [InlineData("ContentLocationMismatch", "The content location does not match the expected location")]
    [InlineData("ContentLanguageMismatch", "The content language does not match the expected language")]
    [InlineData("ContentMD5Mismatch", "The content MD5 does not match the expected MD5")]
    [InlineData("ContentTransferEncodingMismatch", "The content transfer encoding does not match the expected encoding")]
    [InlineData("ContentDispositionFilenameMismatch", "The content disposition filename does not match the expected filename")]
    public async Task ShouldHandleHttpClientError(string errorType, string errorMessage)
    {
        // Arrange
        var errorContext = TestHelper.CreateErrorContext(errorType, errorMessage, "HttpClient");

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.RemediationPlan.Should().NotBeNull();
        result.RemediationPlan.Strategies.Should().NotBeEmpty();

        _mcpClientMock.Verify(x => x.PublishContextAsync(It.IsAny<ErrorContext>()), Times.Once);
        _remediationExecutorMock.Verify(x => x.ExecuteRemediationAsync(It.IsAny<RemediationPlan>()), Times.Once);
    }

    [Fact]
    public async Task ShouldHandleRealWorldHttpClientScenario()
    {
        // Arrange
        var additionalContext = new Dictionary<string, object>
        {
            { "Url", "https://api.example.com/v1/users" },
            { "Method", "GET" },
            { "Headers", new Dictionary<string, string>
                {
                    { "Authorization", "Bearer ****" },
                    { "Content-Type", "application/json" },
                    { "Accept", "application/json" }
                }
            },
            { "Timeout", 30000 },
            { "RetryCount", 3 },
            { "Proxy", "http://proxy.example.com:8080" },
            { "SSLProtocol", "Tls12" },
            { "RequestId", "req-123456" }
        };

        var errorContext = TestHelper.CreateErrorContext(
            "ConnectionTimeout",
            "The HTTP request timed out after 30 seconds",
            "HttpClient",
            additionalContext);

        // Act
        var result = await _service.AnalyzeErrorAsync(errorContext);

        // Assert
        result.Should().NotBeNull();
        result.IsAnalyzed.Should().BeTrue();
        result.RemediationPlan.Should().NotBeNull();
        result.RemediationPlan.Strategies.Should().NotBeEmpty();
        result.RemediationPlan.Strategies.Should().Contain(s => 
            s.Name == "RequestRetry" || 
            s.Name == "TimeoutAdjustment" ||
            s.Name == "ProxyFallback");

        _mcpClientMock.Verify(x => x.PublishContextAsync(It.IsAny<ErrorContext>()), Times.Once);
        _remediationExecutorMock.Verify(x => x.ExecuteRemediationAsync(It.IsAny<RemediationPlan>()), Times.Once);
    }
} 