using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Health
{
    public class ServiceHealthCheckOptions
    {
        public List<string> ServiceEndpoints { get; set; } = new();
        public TimeSpan HealthCheckInterval { get; set; } = TimeSpan.FromSeconds(30);
        public TimeSpan HealthCheckTimeout { get; set; } = TimeSpan.FromSeconds(5);
        public int UnhealthyThreshold { get; set; } = 3;
        public int HealthyThreshold { get; set; } = 2;
        public bool EnablePredictiveAnalysis { get; set; } = true;
        public TimeSpan PredictionWindow { get; set; } = TimeSpan.FromHours(1);
        public int MinDataPointsForPrediction { get; set; } = 30;
        public double HealthScoreThreshold { get; set; } = 0.7;
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
        public List<string> CustomHealthChecks { get; set; } = new();
    }
} 

