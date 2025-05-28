using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Xunit;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Services;
using RuntimeErrorSage.Tests.TestSuite.Models;
using RuntimeErrorSage.Core.Analysis.Interfaces;

namespace RuntimeErrorSage.Tests.TestSuite
{
    /// <summary>
    /// Implements baseline comparison tests required by the research paper.
    /// Compares RuntimeErrorSage against traditional error handling, static analysis, and manual debugging.
    /// </summary>
    public class BaselineComparisonTests
    {
        private readonly IRuntimeErrorSageService _RuntimeErrorSageService;
        private readonly StandardizedErrorScenarios _standardizedScenarios;
        private readonly RealWorldErrorCases _realWorldScenarios;
        private readonly IErrorAnalyzer _errorAnalyzer;
        private readonly List<ComparisonResults> _results;

        public BaselineComparisonTests(
            IRuntimeErrorSageService RuntimeErrorSageService,
            StandardizedErrorScenarios standardizedScenarios,
            RealWorldErrorCases realWorldScenarios,
            IErrorAnalyzer errorAnalyzer)
        {
            _RuntimeErrorSageService = RuntimeErrorSageService;
            _standardizedScenarios = standardizedScenarios;
            _realWorldScenarios = realWorldScenarios;
            _errorAnalyzer = errorAnalyzer;
            _results = new List<ComparisonResults>();
        }

        /// <summary>
        /// Compares RuntimeErrorSage against traditional error handling methods.
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
        /// Compares RuntimeErrorSage against static analysis tools.
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
        /// Compares RuntimeErrorSage against manual debugging process.
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

            var RuntimeErrorSageResult = await _RuntimeErrorSageService.ProcessExceptionAsync(
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
                RuntimeErrorSageMetrics = new BaselineMetrics
                {
                    TimeToAnalyze = RuntimeErrorSageResult.PerformanceMetrics.TotalProcessingTime.TotalMilliseconds,
                    RootCauseAccuracy = RuntimeErrorSageResult.RootCauseConfidence,
                    RemediationAccuracy = RuntimeErrorSageResult.RemediationConfidence,
                    FalsePositiveRate = RuntimeErrorSageResult.FalsePositiveRate,
                    FalseNegativeRate = RuntimeErrorSageResult.FalseNegativeRate
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

            var RuntimeErrorSageResult = await _RuntimeErrorSageService.ProcessExceptionAsync(
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
                RuntimeErrorSageMetrics = new BaselineMetrics
                {
                    TimeToAnalyze = RuntimeErrorSageResult.PerformanceMetrics.TotalProcessingTime.TotalMilliseconds,
                    RootCauseAccuracy = RuntimeErrorSageResult.RootCauseConfidence,
                    RemediationAccuracy = RuntimeErrorSageResult.RemediationConfidence,
                    FalsePositiveRate = RuntimeErrorSageResult.FalsePositiveRate,
                    FalseNegativeRate = RuntimeErrorSageResult.FalseNegativeRate
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

            var RuntimeErrorSageResult = await _RuntimeErrorSageService.ProcessExceptionAsync(
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
                RuntimeErrorSageMetrics = new BaselineMetrics
                {
                    TimeToAnalyze = RuntimeErrorSageResult.PerformanceMetrics.TotalProcessingTime.TotalMilliseconds,
                    RootCauseAccuracy = RuntimeErrorSageResult.RootCauseConfidence,
                    RemediationAccuracy = RuntimeErrorSageResult.RemediationConfidence,
                    FalsePositiveRate = RuntimeErrorSageResult.FalsePositiveRate,
                    FalseNegativeRate = RuntimeErrorSageResult.FalseNegativeRate
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
                (s.TraditionalHandling.TimeToAnalyze - s.RuntimeErrorSageMetrics.TimeToAnalyze) / 
                s.TraditionalHandling.TimeToAnalyze);

            var avgRootCauseImprovement = results.Scenarios.Average(s => 
                (s.RuntimeErrorSageMetrics.RootCauseAccuracy - s.TraditionalHandling.RootCauseAccuracy) / 
                s.TraditionalHandling.RootCauseAccuracy);

            var avgRemediationImprovement = results.Scenarios.Average(s => 
                (s.RuntimeErrorSageMetrics.RemediationAccuracy - s.TraditionalHandling.RemediationAccuracy) / 
                s.TraditionalHandling.RemediationAccuracy);

            var avgFalsePositiveImprovement = results.Scenarios.Average(s => 
                (s.TraditionalHandling.FalsePositiveRate - s.RuntimeErrorSageMetrics.FalsePositiveRate) / 
                s.TraditionalHandling.FalsePositiveRate);

            var avgFalseNegativeImprovement = results.Scenarios.Average(s => 
                (s.TraditionalHandling.FalseNegativeRate - s.RuntimeErrorSageMetrics.FalseNegativeRate) / 
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
                results.Scenarios.Select(s => s.RuntimeErrorSageMetrics.TimeToAnalyze));

            var accuracySignificance = CalculateStatisticalSignificance(
                results.Scenarios.Select(s => s.TraditionalHandling.RootCauseAccuracy),
                results.Scenarios.Select(s => s.RuntimeErrorSageMetrics.RootCauseAccuracy));

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

        /// <summary>
        /// Compares baseline and experimental test results
        /// </summary>
        public async Task<ComparisonResults> CompareResultsAsync(
            string testName,
            Func<Task> baselineAction,
            Func<Task> experimentalAction)
        {
            var baselineMethods = new BaselineExecutionMethods(_errorAnalyzer);
            var experimentalMethods = new BaselineExecutionMethods(_errorAnalyzer);

            await baselineMethods.ExecuteBaselineAsync(testName, baselineAction);
            await experimentalMethods.ExecuteBaselineAsync(testName, experimentalAction);

            var baselineResult = baselineMethods.GetResults().First();
            var experimentalResult = experimentalMethods.GetResults().First();

            var comparison = new ComparisonResults
            {
                Name = testName,
                Baseline = baselineResult,
                Experimental = experimentalResult,
                StatisticalSignificance = CalculateStatisticalSignificance(baselineResult, experimentalResult),
                IsSignificantlyBetter = IsSignificantlyBetter(baselineResult, experimentalResult)
            };

            _results.Add(comparison);
            return comparison;
        }

        /// <summary>
        /// Gets all comparison results
        /// </summary>
        public IReadOnlyList<ComparisonResults> GetResults() => _results;

        private bool IsSignificantlyBetter(BaselineResult baseline, BaselineResult experimental)
        {
            const double significanceThreshold = 0.05;
            var significance = CalculateStatisticalSignificance(baseline, experimental);
            return significance > significanceThreshold && experimental.DurationMs < baseline.DurationMs;
        }
    }

    /// <summary>
    /// Represents the results of a baseline comparison.
    /// </summary>
    public class ComparisonResults
    {
        public string Method { get; set; }
        public List<ComparisonScenario> Scenarios { get; set; }
        public string Name { get; set; }
        public BaselineResult Baseline { get; set; }
        public BaselineResult Experimental { get; set; }
        public double StatisticalSignificance { get; set; }
        public bool IsSignificantlyBetter { get; set; }
    }

    /// <summary>
    /// Represents a single scenario comparison.
    /// </summary>
    public class ComparisonScenario
    {
        public string ScenarioId { get; set; }
        public string ErrorType { get; set; }
        public BaselineMetrics TraditionalHandling { get; set; }
        public BaselineMetrics RuntimeErrorSageMetrics { get; set; }
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

