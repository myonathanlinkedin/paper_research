using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RuntimeErrorSage.Core.Extensions;
using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Examples.Models;
using RuntimeErrorSage.Examples.Services;
using RuntimeErrorSage.Examples.Exceptions;
using RuntimeErrorSage.Examples.Models.Responses;

var builder = WebApplication.CreateBuilder(args);

// Add RuntimeErrorSage services
builder.Services.AddRuntimeErrorSage(builder.Configuration);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register our services
builder.Services.AddScoped<IOperationSimulator, OperationSimulator>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Database operation endpoint
app.MapPost("/operations/database", async (ProcessRequest request, IOperationSimulator simulator) =>
{
    var response = await simulator.SimulateDatabaseOperationAsync(request);
    if (response.Status == OperationStatus.Failed)
    {
        throw new DatabaseOperationException(
            response.Error?.Message ?? "Database operation failed",
            response.Error?.Context
        );
    }
    return response;
});

// File upload endpoint
app.MapPost("/operations/file", async (FileUploadRequest request, IOperationSimulator simulator) =>
{
    var response = await simulator.SimulateFileUploadAsync(request);
    if (response.Status == OperationStatus.Failed)
    {
        throw new FileOperationException(
            response.Error?.Message ?? "File operation failed",
            response.Error?.Context
        );
    }
    return response;
});

// External service endpoint
app.MapPost("/operations/service", async (IntegrationRequest request, IOperationSimulator simulator) =>
{
    var response = await simulator.SimulateServiceCallAsync(request);
    if (response.Status == OperationStatus.Failed)
    {
        throw new ServiceIntegrationException(
            response.Error?.Message ?? "Service integration failed",
            response.Error?.Context
        );
    }
    return response;
});

// Resource allocation endpoint
app.MapPost("/operations/resource", async (ResourceRequest request, IOperationSimulator simulator) =>
{
    var response = await simulator.SimulateResourceAllocationAsync(request);
    if (response.Status == OperationStatus.Failed)
    {
        throw new ResourceAllocationException(
            response.Error?.Message ?? "Resource allocation failed",
            response.Error?.Context
        );
    }
    return response;
});

// Add a sample endpoint to test error analysis
app.MapPost("/analyze-error", async (ErrorContext context, IErrorAnalyzer analyzer) =>
{
    try
    {
        var result = await analyzer.AnalyzeErrorAsync(context);
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "Error analysis failed",
            detail: ex.Message,
            statusCode: 500);
    }
});

// Add health check endpoint
app.MapHealthChecks("/health");

// Run the application
await app.RunAsync();
