using System;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class ExperimentResultDataDto
    {
        public Guid Id { get; set; }
        public Guid VariantId { get; set; }
        public string MetricName { get; set; }
        public MetricType MetricType { get; set; }
        public ResultValueDto Value { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
