using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Metrics;

namespace RuntimeErrorSage.Tests.TestSuite;

/// <summary>
/// Generates reports for test suite results.
/// </summary>
public class TestSuiteReporter
{
    private readonly TestSuiteResult _result;
    private readonly string _outputPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSuiteReporter"/> class.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="outputPath">The output path.</param>
    public TestSuiteReporter(TestSuiteResult result, string outputPath)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
        _outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
    }

    /// <summary>
    /// Gets the result.
    /// </summary>
    public TestSuiteResult Result => _result;

    /// <summary>
    /// Gets the output path.
    /// </summary>
    public string OutputPath => _outputPath;

    /// <summary>
    /// Generates a report.
    /// </summary>
    /// <returns>The report.</returns>
    public async Task<string> GenerateReportAsync()
    {
        var report = new StringBuilder();

        report.AppendLine("# Test Suite Report");
        report.AppendLine();
        report.AppendLine($"## Summary");
        report.AppendLine();
        report.AppendLine($"- Start Time: {_result.StartTime:yyyy-MM-dd HH:mm:ss}");
        report.AppendLine($"- End Time: {_result.EndTime:yyyy-MM-dd HH:mm:ss}");
        report.AppendLine($"- Duration: {_result.Duration.TotalSeconds:F2} seconds");
        report.AppendLine($"- Status: {(_result.Passed ? "Passed" : "Failed")}");
        if (!_result.Passed)
            report.AppendLine($"- Error: {_result.ErrorMessage}");
        report.AppendLine();

        report.AppendLine("## Scenarios");
        report.AppendLine();
        foreach (var scenario in _result.Scenarios)
        {
            report.AppendLine($"### {scenario.Name}");
            report.AppendLine();
            report.AppendLine($"- Error Type: {scenario.ErrorType}");
            report.AppendLine($"- Error Message: {scenario.ErrorMessage}");
            report.AppendLine($"- Source: {scenario.Source}");
            report.AppendLine($"- Analysis: {scenario.Analysis?.Summary ?? "N/A"}");
            report.AppendLine();
        }

        report.AppendLine("## Metrics");
        report.AppendLine();
        foreach (var metric in _result.Metrics)
        {
            report.AppendLine($"### {metric.Name}");
            report.AppendLine();
            report.AppendLine($"- Duration: {metric.Duration.TotalMilliseconds:F2} ms");
            report.AppendLine($"- Memory Usage: {metric.MemoryUsage / 1024:F2} KB");
            report.AppendLine($"- CPU Usage: {metric.CpuUsage:F2}%");
            report.AppendLine();
        }

        report.AppendLine("## Metadata");
        report.AppendLine();
        foreach (var (key, value) in _result.Metadata)
        {
            report.AppendLine($"- {key}: {value}");
        }

        return report.ToString();
    }

    /// <summary>
    /// Saves the report.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SaveReportAsync()
    {
        var report = await GenerateReportAsync().ConfigureAwait(false);
        await File.WriteAllTextAsync(_outputPath, report).ConfigureAwait(false);
    }

    /// <summary>
    /// Generates a JSON report.
    /// </summary>
    /// <returns>The JSON report.</returns>
    public async Task<string> GenerateJsonReportAsync()
    {
        var report = new
        {
            Summary = new
            {
                StartTime = _result.StartTime,
                EndTime = _result.EndTime,
                Duration = _result.Duration.TotalSeconds,
                Passed = _result.Passed,
                ErrorMessage = _result.ErrorMessage
            },
            Scenarios = _result.Scenarios.Select(s => new
            {
                s.Name,
                s.ErrorType,
                s.ErrorMessage,
                s.Source,
                Analysis = s.Analysis?.Summary
            }),
            Metrics = _result.Metrics.Select(m => new
            {
                m.Name,
                Duration = m.Duration.TotalMilliseconds,
                MemoryUsage = m.MemoryUsage,
                m.CpuUsage
            }),
            Metadata = _result.Metadata
        };

        return JsonSerializer.Serialize(report, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    /// <summary>
    /// Saves the JSON report.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SaveJsonReportAsync()
    {
        var report = await GenerateJsonReportAsync().ConfigureAwait(false);
        var jsonPath = Path.ChangeExtension(_outputPath, ".json");
        await File.WriteAllTextAsync(jsonPath, report).ConfigureAwait(false);
    }

    /// <summary>
    /// Generates a summary.
    /// </summary>
    /// <returns>The summary.</returns>
    public string GenerateSummary()
    {
        var summary = new StringBuilder();

        summary.AppendLine($"Test Suite Summary");
        summary.AppendLine($"-----------------");
        summary.AppendLine($"Status: {(_result.Passed ? "Passed" : "Failed")}");
        summary.AppendLine($"Duration: {_result.Duration.TotalSeconds:F2} seconds");
        summary.AppendLine($"Scenarios: {_result.Scenarios.Count}");
        summary.AppendLine($"Metrics: {_result.Metrics.Count}");
        if (!_result.Passed)
            summary.AppendLine($"Error: {_result.ErrorMessage}");

        return summary.ToString();
    }
} 
