using System;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos
{
    public class ImpactValueDto
    {
        public string MetricName { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public DateTime MeasuredAt { get; set; }
        public string MeasurementMethod { get; set; }
        public string Notes { get; set; }
    }
}
