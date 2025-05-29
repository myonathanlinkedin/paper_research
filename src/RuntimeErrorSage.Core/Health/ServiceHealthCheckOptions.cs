using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Health
{
    public class ServiceHealthCheckOptions
    {
        public IReadOnlyCollection<ServiceEndpoints> ServiceEndpoints { get; } = new();
        public TimeSpan HealthCheckInterval { get; } = TimeSpan.FromSeconds(30);
        public TimeSpan HealthCheckTimeout { get; } = TimeSpan.FromSeconds(5);
        public int UnhealthyThreshold { get; } = 3;
        public int HealthyThreshold { get; } = 2;
        public bool EnablePredictiveAnalysis { get; } = true;
        public TimeSpan PredictionWindow { get; } = TimeSpan.FromHours(1);
        public int MinDataPointsForPrediction { get; } = 30;
        public double HealthScoreThreshold { get; } = 0.7;
        public Dictionary<string, double> MetricThresholds { get; set; } = new()
        {
            { "cpu.usage", 80.0 },
            { "memory.usage", 85.0 },
            { "disk.usage", 90.0 },
            { "network.latency", 100.0 },
            { "error.rate", 5.0 },
            { "response.time", 500.0 },
            { "thread.count", 1000.0 },
            { "connection.count", 10000.0 }
        };
        public IReadOnlyCollection<CustomHealthChecks> CustomHealthChecks { get; } = new();
    }
} 







