using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Xunit;
using CodeSage.Core.Models;
using CodeSage.Core.Services;

namespace CodeSage.Tests.TestSuite
{
    /// <summary>
    /// Implements baseline comparison tests required by the research paper.
    /// Compares CodeSage against traditional error handling, static analysis, and manual debugging.
    /// </summary>
    public class BaselineComparisonTests
    {
        private readonly ICodeSageService _codeSageService;
        private readonly StandardizedErrorScenarios _standardizedScenarios;
        private readonly RealWorldErrorCases _realWorldScenarios;

        public BaselineComparisonTests(
            ICodeSageService codeSageService,
            StandardizedErrorScenarios standardizedScenarios,
            RealWorldErrorCases realWorldScenarios)
        {
            _codeSageService = codeSageService;
            _standardizedScenarios = standardizedScenarios;
            _realWorldScenarios = realWorldScenarios;
        }

        /// <summary>
        /// Compares CodeSage against traditional error handling methods.
        /// </summary>
        [Fact]
        public async Task CompareWithTraditionalErrorHandling()
        {
            var results = new ComparisonResults
            {
                Method = "Traditional Error Handling",
                Scenarios = new List<ComparisonScenario>()
            };

            // Test standardized scenarios
            foreach (var errorType in _standardizedScenarios.GetScenarios().Keys)
            {
                foreach (var scenario in _standardizedScenarios.GetScenarios()[errorType])
                {
                    var comparison = await CompareScenarioWithTraditionalHandling(scenario);
                    results.Scenarios.Add(comparison);
                }
            }

            // Test real-world scenarios
            foreach (var scenario in _realWorldScenarios.GetScenarios())
            {
                var comparison = await CompareScenarioWithTraditionalHandling(scenario);
                results.Scenarios.Add(comparison);
            }

            // Validate comparison results
            ValidateComparisonResults(results);
        }

        /// <summary>
        /// Compares CodeSage against static analysis tools.
        /// </summary>
        [Fact]
        public async Task CompareWithStaticAnalysis()
        {
            var results = new ComparisonResults
            {
                Method = "Static Analysis",
                Scenarios = new List<ComparisonScenario>()
            };

            // Test standardized scenarios
            foreach (var errorType in _standardizedScenarios.GetScenarios().Keys)
            {
                foreach (var scenario in _standardizedScenarios.GetScenarios()[errorType])
                {
                    var comparison = await CompareScenarioWithStaticAnalysis(scenario);
                    results.Scenarios.Add(comparison);
                }
            }

            // Test real-world scenarios
            foreach (var scenario in _realWorldScenarios.GetScenarios())
            {
                var comparison = await CompareScenarioWithStaticAnalysis(scenario);
                results.Scenarios.Add(comparison);
            }

            // Validate comparison results
            ValidateComparisonResults(results);
        }

        /// <summary>
        /// Compares CodeSage against manual debugging process.
        /// </summary>
        [Fact]
        public async Task CompareWithManualDebugging()
        {
            var results = new ComparisonResults
            {
                Method = "Manual Debugging",
                Scenarios = new List<ComparisonScenario>()
            };

            // Test standardized scenarios
            foreach (var errorType in _standardizedScenarios.GetScenarios().Keys)
            {
                foreach (var scenario in _standardizedScenarios.GetScenarios()[errorType])
                {
                    var comparison = await CompareScenarioWithManualDebugging(scenario);
                    results.Scenarios.Add(comparison);
                }
            }

            // Test real-world scenarios
            foreach (var scenario in _realWorldScenarios.GetScenarios())
            {
                var comparison = await CompareScenarioWithManualDebugging(scenario);
                results.Scenarios.Add(comparison);
            }

            // Validate comparison results
            ValidateComparisonResults(results);
        }

        /// <summary>
        /// Compares a scenario using traditional error handling methods.
        /// </summary>
        private async Task<ComparisonScenario> CompareScenarioWithTraditionalHandling(ErrorScenario scenario)
        {
            var stopwatch = Stopwatch.StartNew();
            var traditionalResult = await ExecuteTraditionalHandling(scenario);
            stopwatch.Stop();

            var codeSageResult = await _codeSageService.ProcessExceptionAsync(
                await scenario.ExecuteAsync(),
                new ErrorContext { ErrorType = scenario.ErrorType });

            return new ComparisonScenario
            {
                ScenarioId = scenario.Id,
                ErrorType = scenario.ErrorType,
                TraditionalHandling = new BaselineMetrics
                {
                    TimeToAnalyze = stopwatch.ElapsedMilliseconds,
                    RootCauseAccuracy = traditionalResult.RootCauseAccuracy,
                    RemediationAccuracy = traditionalResult.RemediationAccuracy,
                    FalsePositiveRate = traditionalResult.FalsePositiveRate,
                    FalseNegativeRate = traditionalResult.FalseNegativeRate
                },
                CodeSageMetrics = new BaselineMetrics
                {
                    TimeToAnalyze = codeSageResult.PerformanceMetrics.TotalProcessingTime.TotalMilliseconds,
                    RootCauseAccuracy = codeSageResult.RootCauseConfidence,
                    RemediationAccuracy = codeSageResult.RemediationConfidence,
                    FalsePositiveRate = codeSageResult.FalsePositiveRate,
                    FalseNegativeRate = codeSageResult.FalseNegativeRate
                }
            };
        }

        /// <summary>
        /// Compares a scenario using static analysis tools.
        /// </summary>
        private async Task<ComparisonScenario> CompareScenarioWithStaticAnalysis(ErrorScenario scenario)
        {
            var stopwatch = Stopwatch.StartNew();
            var staticAnalysisResult = await ExecuteStaticAnalysis(scenario);
            stopwatch.Stop();

            var codeSageResult = await _codeSageService.ProcessExceptionAsync(
                await scenario.ExecuteAsync(),
                new ErrorContext { ErrorType = scenario.ErrorType });

            return new ComparisonScenario
            {
                ScenarioId = scenario.Id,
                ErrorType = scenario.ErrorType,
                TraditionalHandling = new BaselineMetrics
                {
                    TimeToAnalyze = stopwatch.ElapsedMilliseconds,
                    RootCauseAccuracy = staticAnalysisResult.RootCauseAccuracy,
                    RemediationAccuracy = staticAnalysisResult.RemediationAccuracy,
                    FalsePositiveRate = staticAnalysisResult.FalsePositiveRate,
                    FalseNegativeRate = staticAnalysisResult.FalseNegativeRate
                },
                CodeSageMetrics = new BaselineMetrics
                {
                    TimeToAnalyze = codeSageResult.PerformanceMetrics.TotalProcessingTime.TotalMilliseconds,
                    RootCauseAccuracy = codeSageResult.RootCauseConfidence,
                    RemediationAccuracy = codeSageResult.RemediationConfidence,
                    FalsePositiveRate = codeSageResult.FalsePositiveRate,
                    FalseNegativeRate = codeSageResult.FalseNegativeRate
                }
            };
        }

        /// <summary>
        /// Compares a scenario using manual debugging process.
        /// </summary>
        private async Task<ComparisonScenario> CompareScenarioWithManualDebugging(ErrorScenario scenario)
        {
            var stopwatch = Stopwatch.StartNew();
            var manualDebugResult = await ExecuteManualDebugging(scenario);
            stopwatch.Stop();

            var codeSageResult = await _codeSageService.ProcessExceptionAsync(
                await scenario.ExecuteAsync(),
                new ErrorContext { ErrorType = scenario.ErrorType });

            return new ComparisonScenario
            {
                ScenarioId = scenario.Id,
                ErrorType = scenario.ErrorType,
                TraditionalHandling = new BaselineMetrics
                {
                    TimeToAnalyze = stopwatch.ElapsedMilliseconds,
                    RootCauseAccuracy = manualDebugResult.RootCauseAccuracy,
                    RemediationAccuracy = manualDebugResult.RemediationAccuracy,
                    FalsePositiveRate = manualDebugResult.FalsePositiveRate,
                    FalseNegativeRate = manualDebugResult.FalseNegativeRate
                },
                CodeSageMetrics = new BaselineMetrics
                {
                    TimeToAnalyze = codeSageResult.PerformanceMetrics.TotalProcessingTime.TotalMilliseconds,
                    RootCauseAccuracy = codeSageResult.RootCauseConfidence,
                    RemediationAccuracy = codeSageResult.RemediationConfidence,
                    FalsePositiveRate = codeSageResult.FalsePositiveRate,
                    FalseNegativeRate = codeSageResult.FalseNegativeRate
                }
            };
        }

        /// <summary>
        /// Validates the comparison results against research requirements.
        /// </summary>
        private void ValidateComparisonResults(ComparisonResults results)
        {
            // Calculate average improvements
            var avgTimeImprovement = results.Scenarios.Average(s => 
                (s.TraditionalHandling.TimeToAnalyze - s.CodeSageMetrics.TimeToAnalyze) / 
                s.TraditionalHandling.TimeToAnalyze);

            var avgRootCauseImprovement = results.Scenarios.Average(s => 
                (s.CodeSageMetrics.RootCauseAccuracy - s.TraditionalHandling.RootCauseAccuracy) / 
                s.TraditionalHandling.RootCauseAccuracy);

            var avgRemediationImprovement = results.Scenarios.Average(s => 
                (s.CodeSageMetrics.RemediationAccuracy - s.TraditionalHandling.RemediationAccuracy) / 
                s.TraditionalHandling.RemediationAccuracy);

            var avgFalsePositiveImprovement = results.Scenarios.Average(s => 
                (s.TraditionalHandling.FalsePositiveRate - s.CodeSageMetrics.FalsePositiveRate) / 
                s.TraditionalHandling.FalsePositiveRate);

            var avgFalseNegativeImprovement = results.Scenarios.Average(s => 
                (s.TraditionalHandling.FalseNegativeRate - s.CodeSageMetrics.FalseNegativeRate) / 
                s.TraditionalHandling.FalseNegativeRate);

            // Validate improvements against research requirements
            Assert.True(avgTimeImprovement >= 0.5, 
                $"Average time improvement {avgTimeImprovement:P2} below required 50% for {results.Method}");
            Assert.True(avgRootCauseImprovement >= 0.2, 
                $"Average root cause accuracy improvement {avgRootCauseImprovement:P2} below required 20% for {results.Method}");
            Assert.True(avgRemediationImprovement >= 0.2, 
                $"Average remediation accuracy improvement {avgRemediationImprovement:P2} below required 20% for {results.Method}");
            Assert.True(avgFalsePositiveImprovement >= 0.3, 
                $"Average false positive rate improvement {avgFalsePositiveImprovement:P2} below required 30% for {results.Method}");
            Assert.True(avgFalseNegativeImprovement >= 0.3, 
                $"Average false negative rate improvement {avgFalseNegativeImprovement:P2} below required 30% for {results.Method}");

            // Validate statistical significance
            var timeSignificance = CalculateStatisticalSignificance(
                results.Scenarios.Select(s => s.TraditionalHandling.TimeToAnalyze),
                results.Scenarios.Select(s => s.CodeSageMetrics.TimeToAnalyze));

            var accuracySignificance = CalculateStatisticalSignificance(
                results.Scenarios.Select(s => s.TraditionalHandling.RootCauseAccuracy),
                results.Scenarios.Select(s => s.CodeSageMetrics.RootCauseAccuracy));

            Assert.True(timeSignificance < 0.05, 
                $"Time improvement not statistically significant (p={timeSignificance:F4}) for {results.Method}");
            Assert.True(accuracySignificance < 0.05, 
                $"Accuracy improvement not statistically significant (p={accuracySignificance:F4}) for {results.Method}");
        }

        /// <summary>
        /// Calculates statistical significance using t-test.
        /// </summary>
        private double CalculateStatisticalSignificance(IEnumerable<double> baseline, IEnumerable<double> experimental)
        {
            // Implementation of t-test for statistical significance
            // Returns p-value (probability that the difference is due to chance)
            // p < 0.05 indicates statistical significance
            var baselineArray = baseline.ToArray();
            var experimentalArray = experimental.ToArray();

            var baselineMean = baselineArray.Average();
            var experimentalMean = experimentalArray.Average();

            var baselineVariance = baselineArray.Select(x => Math.Pow(x - baselineMean, 2)).Average();
            var experimentalVariance = experimentalArray.Select(x => Math.Pow(x - experimentalMean, 2)).Average();

            var pooledStdDev = Math.Sqrt((baselineVariance + experimentalVariance) / 2);
            var tStat = Math.Abs(baselineMean - experimentalMean) / 
                (pooledStdDev * Math.Sqrt(2.0 / baselineArray.Length));

            // Convert t-statistic to p-value using Student's t-distribution
            // This is a simplified approximation
            return 2 * (1 - NormalCDF(tStat));
        }

        /// <summary>
        /// Cumulative distribution function for normal distribution.
        /// </summary>
        private double NormalCDF(double x)
        {
            // Implementation of normal CDF using error function
            return 0.5 * (1 + Math.Erf(x / Math.Sqrt(2)));
        }
    }

    /// <summary>
    /// Represents the results of a baseline comparison.
    /// </summary>
    public class ComparisonResults
    {
        public string Method { get; set; }
        public List<ComparisonScenario> Scenarios { get; set; }
    }

    /// <summary>
    /// Represents a single scenario comparison.
    /// </summary>
    public class ComparisonScenario
    {
        public string ScenarioId { get; set; }
        public string ErrorType { get; set; }
        public BaselineMetrics TraditionalHandling { get; set; }
        public BaselineMetrics CodeSageMetrics { get; set; }
    }

    /// <summary>
    /// Represents metrics for baseline comparison.
    /// </summary>
    public class BaselineMetrics
    {
        public double TimeToAnalyze { get; set; }
        public double RootCauseAccuracy { get; set; }
        public double RemediationAccuracy { get; set; }
        public double FalsePositiveRate { get; set; }
        public double FalseNegativeRate { get; set; }
    }
} 