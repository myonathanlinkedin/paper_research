# CodeSage LLM Integration

This module provides integration with LM Studio for local LLM-based error analysis in .NET applications.

## Overview

The LLM integration enables real-time error analysis using local language models through LM Studio. This provides privacy-preserving error analysis without requiring cloud connectivity.

## Features

- Local LLM inference through LM Studio
- Configurable model parameters (temperature, tokens, etc.)
- Health monitoring and status checks
- Comprehensive error analysis with confidence scoring
- Support for various error types (database, HTTP, filesystem, etc.)
- Performance metrics collection

## Prerequisites

1. [LM Studio](https://lmstudio.ai/) installed and running
2. A compatible language model loaded in LM Studio
3. .NET 9.0 or later

## Configuration

Add the following to your `appsettings.json`:

```json
{
  "CodeSage": {
    "EnableErrorAnalysis": true,
    "LMStudio": {
      "BaseUrl": "http://localhost:1234",
      "ModelId": "your-model-id",
      "TimeoutSeconds": 30,
      "MaxTokens": 500,
      "Temperature": 0.7,
      "TopP": 0.9,
      "FrequencyPenalty": 0.0,
      "PresencePenalty": 0.0,
      "StopSequences": []
    }
  }
}
```

## Usage

1. Add the CodeSage services to your application:

```csharp
builder.Services.AddCodeSage(builder.Configuration);
```

2. Inject and use the error analyzer:

```csharp
public class YourService
{
    private readonly IErrorAnalyzer _errorAnalyzer;

    public YourService(IErrorAnalyzer errorAnalyzer)
    {
        _errorAnalyzer = errorAnalyzer;
    }

    public async Task HandleError(Exception ex)
    {
        var context = new ErrorContext
        {
            ServiceName = "YourService",
            OperationName = "YourOperation",
            CorrelationId = Guid.NewGuid().ToString(),
            Timestamp = DateTime.UtcNow,
            Exception = ex,
            AdditionalContext = new Dictionary<string, string>
            {
                ["Key"] = "Value"
            }
        };

        var analysis = await _errorAnalyzer.AnalyzeErrorAsync(context);
        
        // Use the analysis results
        Console.WriteLine($"Root Cause: {analysis.RootCause}");
        Console.WriteLine($"Confidence: {analysis.Confidence}");
        Console.WriteLine($"Accuracy: {analysis.Accuracy}");
        foreach (var step in analysis.RemediationSteps)
        {
            Console.WriteLine($"Step: {step}");
        }
    }
}
```

## Health Monitoring

The integration includes a health check that monitors the LM Studio connection and model status. Add the health check endpoint to your application:

```csharp
app.MapHealthChecks("/health");
```

The health check will report:
- Model readiness
- Connection status
- Response validation
- Performance metrics

## Error Types

The analyzer supports various error types with specialized handling:

1. Database Errors
   - Connection issues
   - Query failures
   - Timeout errors

2. HTTP Errors
   - Connection failures
   - Status code errors
   - Timeout issues

3. File System Errors
   - Permission issues
   - Access violations
   - I/O errors

4. Resource Errors
   - Memory allocation
   - CPU usage
   - Thread pool exhaustion

## Performance Considerations

- The analyzer includes performance metrics for each analysis
- Latency is measured for the entire analysis process
- Memory usage is tracked during analysis
- CPU usage is monitored for resource impact

## Limitations

1. Requires LM Studio to be running locally
2. Model performance depends on local hardware
3. Response time varies based on model size and complexity
4. Limited to text-based analysis (no image or binary analysis)

## Troubleshooting

1. Check LM Studio Status
   - Verify LM Studio is running
   - Confirm model is loaded
   - Check model status in LM Studio UI

2. Connection Issues
   - Verify BaseUrl in configuration
   - Check network connectivity
   - Validate port availability

3. Model Issues
   - Confirm model ID matches loaded model
   - Check model compatibility
   - Verify model parameters

4. Performance Issues
   - Monitor system resources
   - Adjust model parameters
   - Consider model size vs. hardware

## Contributing

1. Fork the repository
2. Create a feature branch
3. Add tests for new functionality
4. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details. 