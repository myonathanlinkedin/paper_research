using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Application.Health.Models
{
    public class MetricTrend
    {
        public double Slope { get; }
        public double Intercept { get; }
        public double CurrentValue { get; }
        public double ChangeRate { get; }
    }
} 







