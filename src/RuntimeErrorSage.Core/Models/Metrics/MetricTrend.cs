namespace RuntimeErrorSage.Core.Health.Models
{
    public class MetricTrend
    {
        public double Slope { get; set; }
        public double Intercept { get; set; }
        public double CurrentValue { get; set; }
        public double ChangeRate { get; set; }
    }
} 

