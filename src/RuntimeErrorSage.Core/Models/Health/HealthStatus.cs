using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Health
{
    /// <summary>
    /// Represents the health status of a service or component.
    /// </summary>
    public class HealthStatus
    {
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, object> Data { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public void AddData(string key, object value)
        {
            Data[key] = value;
        }

        public void AddTag(string tag)
        {
            if (!Tags.Contains(tag))
            {
                Tags.Add(tag);
            }
        }

        public static HealthStatus Healthy(string description = "Service is healthy")
        {
            return new HealthStatus
            {
                Status = "Healthy",
                Description = description
            };
        }

        public static HealthStatus Unhealthy(string description)
        {
            return new HealthStatus
            {
                Status = "Unhealthy",
                Description = description
            };
        }

        public static HealthStatus Degraded(string description)
        {
            return new HealthStatus
            {
                Status = "Degraded",
                Description = description
            };
        }
    }
} 