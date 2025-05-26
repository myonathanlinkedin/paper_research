using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using Moq;
using RuntimeErrorSage.Examples.Models;
using RuntimeErrorSage.Examples.Services;
using RuntimeErrorSage.Examples.Exceptions;
using RuntimeErrorSage.Examples.Models.Responses;

namespace RuntimeErrorSage.Tests;

public class OperationSimulatorTests
{
    private readonly OperationSimulator _simulator;
    private readonly Random _random;

    public OperationSimulatorTests()
    {
        _simulator = new OperationSimulator();
        _random = new Random();
    }

    [Fact]
    public async Task SimulateDatabaseOperationAsync_Success_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new ProcessRequest
        {
            Data = "test data",
            Operation = "CREATE",
            Priority = 1,
            TimeoutSeconds = 30
        };

        // Act
        var response = await _simulator.SimulateDatabaseOperationAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(OperationStatus.Success, response.Status);
        Assert.Null(response.Error);
        Assert.True(response.DurationMs > 0);
    }

    [Fact]
    public async Task SimulateDatabaseOperationAsync_InvalidOperation_ThrowsValidationException()
    {
        // Arrange
        var request = new ProcessRequest
        {
            Data = "test data",
            Operation = "INVALID",
            Priority = 1
        };

        // Act & Assert
        var response = await _simulator.SimulateDatabaseOperationAsync(request);
        Assert.Equal(OperationStatus.Failed, response.Status);
        Assert.NotNull(response.Error);
        Assert.Equal("DB_OPERATION_FAILED", response.Error.Code);
    }

    [Fact]
    public async Task SimulateFileUploadAsync_Success_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new FileUploadRequest
        {
            FileName = "test.txt",
            FileSize = 1024,
            ContentType = "text/plain"
        };

        // Act
        var response = await _simulator.SimulateFileUploadAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(OperationStatus.Success, response.Status);
        Assert.Null(response.Error);
        Assert.True(response.DurationMs > 0);
    }

    [Fact]
    public async Task SimulateFileUploadAsync_InvalidFileSize_ThrowsValidationException()
    {
        // Arrange
        var request = new FileUploadRequest
        {
            FileName = "test.txt",
            FileSize = -1,
            ContentType = "text/plain"
        };

        // Act & Assert
        var response = await _simulator.SimulateFileUploadAsync(request);
        Assert.Equal(OperationStatus.Failed, response.Status);
        Assert.NotNull(response.Error);
        Assert.Equal("FILE_UPLOAD_FAILED", response.Error.Code);
    }

    [Fact]
    public async Task SimulateServiceCallAsync_Success_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new IntegrationRequest
        {
            ServiceUrl = "https://api.example.com",
            Method = "GET",
            TimeoutSeconds = 30
        };

        // Act
        var response = await _simulator.SimulateServiceCallAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(OperationStatus.Success, response.Status);
        Assert.Null(response.Error);
        Assert.True(response.DurationMs > 0);
    }

    [Fact]
    public async Task SimulateServiceCallAsync_InvalidUrl_ThrowsValidationException()
    {
        // Arrange
        var request = new IntegrationRequest
        {
            ServiceUrl = "invalid-url",
            Method = "GET"
        };

        // Act & Assert
        var response = await _simulator.SimulateServiceCallAsync(request);
        Assert.Equal(OperationStatus.Failed, response.Status);
        Assert.NotNull(response.Error);
        Assert.Equal("SERVICE_CALL_FAILED", response.Error.Code);
    }

    [Fact]
    public async Task SimulateResourceAllocationAsync_Success_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new ResourceRequest
        {
            ResourceType = "CPU",
            RequestedAmount = 2,
            Priority = 1
        };

        // Act
        var response = await _simulator.SimulateResourceAllocationAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(OperationStatus.Success, response.Status);
        Assert.Null(response.Error);
        Assert.True(response.DurationMs > 0);
    }

    [Fact]
    public async Task SimulateResourceAllocationAsync_InvalidAmount_ThrowsValidationException()
    {
        // Arrange
        var request = new ResourceRequest
        {
            ResourceType = "CPU",
            RequestedAmount = -1,
            Priority = 1
        };

        // Act & Assert
        var response = await _simulator.SimulateResourceAllocationAsync(request);
        Assert.Equal(OperationStatus.Failed, response.Status);
        Assert.NotNull(response.Error);
        Assert.Equal("RESOURCE_ALLOCATION_FAILED", response.Error.Code);
    }

    [Theory]
    [InlineData("CREATE")]
    [InlineData("UPDATE")]
    [InlineData("DELETE")]
    [InlineData("QUERY")]
    public async Task SimulateDatabaseOperationAsync_ValidOperations_ReturnsSuccessResponse(string operation)
    {
        // Arrange
        var request = new ProcessRequest
        {
            Data = "test data",
            Operation = operation,
            Priority = 1
        };

        // Act
        var response = await _simulator.SimulateDatabaseOperationAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(OperationStatus.Success, response.Status);
        Assert.Null(response.Error);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public async Task SimulateDatabaseOperationAsync_ValidPriorities_ReturnsSuccessResponse(int priority)
    {
        // Arrange
        var request = new ProcessRequest
        {
            Data = "test data",
            Operation = "CREATE",
            Priority = priority
        };

        // Act
        var response = await _simulator.SimulateDatabaseOperationAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(OperationStatus.Success, response.Status);
        Assert.Null(response.Error);
    }
} 
