using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Tests.TestSuite.Models;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Runs test scenarios and validates results
/// </summary>
public class TestScenarioRunner
{
    private readonly IErrorAnalyzer _errorAnalyzer;
    private readonly List<TestResult> _results;

    public TestScenarioRunner(IErrorAnalyzer errorAnalyzer)
    {
        _errorAnalyzer = errorAnalyzer;
        _results = new List<TestResult>();
    }

    /// <summary>
    /// Runs a test scenario and validates the results
    /// </summary>
    public async Task<TestResult> RunScenarioAsync(TestScenario scenario)
    {
        var result = new TestResult
        {
            ScenarioId = scenario.Id,
            ScenarioName = scenario.Name,
            StartTime = DateTime.UtcNow
        };

        try
        {
            if (scenario.SetupAction != null)
            {
                await scenario.SetupAction();
            }

            if (scenario.TriggerAction != null)
            {
                await scenario.TriggerAction();
            }

            var error = new RuntimeError(
                message: scenario.ExpectedErrorMessage,
                errorType: scenario.ExpectedErrorType,
                source: scenario.Name,
                stackTrace: string.Empty
            );

            var context = new ErrorContext(
                error: error,
                context: scenario.Name,
                timestamp: DateTime.UtcNow
            );

            var exception = new Exception(scenario.ExpectedErrorMessage);
            var analysisResult = await _errorAnalyzer.AnalyzeErrorAsync(exception, context);
            
            // Convert ErrorAnalysisResult to ErrorAnalysis for validation
            var analysis = new ErrorAnalysis(
                errorType: analysisResult.ErrorType ?? "Unknown",
                rootCause: analysisResult.RootCause ?? "Unknown",
                confidence: analysisResult.Confidence,
                remediationSteps: analysisResult.SuggestedActions?.Select(a => a.Description ?? "").ToList() ?? new List<string>()
            );
            
            result.Passed = ValidateResults(analysis, scenario);
            result.ErrorAnalysis = analysis;
        }
        catch (Exception ex)
        {
            result.Passed = false;
            result.ErrorMessage = ex.Message;
        }
        finally
        {
            if (scenario.CleanupAction != null)
            {
                await scenario.CleanupAction();
            }

            result.EndTime = DateTime.UtcNow;
            _results.Add(result);
        }

        return result;
    }

    /// <summary>
    /// Gets all test results
    /// </summary>
    public IReadOnlyList<TestResult> GetResults() => _results;

    private bool ValidateResults(ErrorAnalysis analysis, TestScenario scenario)
    {
        if (analysis == null) return false;

        var errorTypeMatch = analysis.ErrorType == scenario.ExpectedErrorType;
        var errorMessageMatch = analysis.ErrorMessage.Contains(scenario.ExpectedErrorMessage);
        // Compare RemediationAction description or name with expected remediation string
        var remediationMatch = analysis.RemediationAction != null && 
            (analysis.RemediationAction.Description?.Contains(scenario.ExpectedRemediation) == true ||
             analysis.RemediationAction.Name?.Contains(scenario.ExpectedRemediation) == true);

        return errorTypeMatch && errorMessageMatch && remediationMatch;
    }
} 
