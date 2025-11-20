using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class ResultValueDto
    {
        public MetricType MetricType { get; set; }
        public double Value { get; set; }
        public int SampleSize { get; set; }
        public double? StandardDeviation { get; set; }
        public double? MarginOfError { get; set; }
    }
}

// Application Service Interface
