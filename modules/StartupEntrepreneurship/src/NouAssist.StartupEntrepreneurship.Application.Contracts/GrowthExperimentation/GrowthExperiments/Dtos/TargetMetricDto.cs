using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class TargetMetricDto
    {
        public string Name { get; set; }
        public MetricType Type { get; set; }
        public double BaselineValue { get; set; }
        public double TargetValue { get; set; }
        public bool IsPrimary { get; set; }
    }
}
