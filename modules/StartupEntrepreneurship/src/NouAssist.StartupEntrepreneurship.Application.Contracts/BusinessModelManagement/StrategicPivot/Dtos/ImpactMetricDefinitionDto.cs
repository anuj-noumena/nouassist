using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos
{
    public class ImpactMetricDefinitionDto : EntityDto<Guid>
    {
        public Guid PivotId { get; set; }
        public string MetricName { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public string MeasurementMethod { get; set; }
        public decimal? BaselineValue { get; set; }
        public decimal? TargetValue { get; set; }
        public List<ImpactValueDto> Measurements { get; set; }
        public decimal? LatestValue { get; set; }
        public double? ProgressToTarget { get; set; }

        public ImpactMetricDefinitionDto()
        {
            Measurements = new List<ImpactValueDto>();
        }
    }
}
