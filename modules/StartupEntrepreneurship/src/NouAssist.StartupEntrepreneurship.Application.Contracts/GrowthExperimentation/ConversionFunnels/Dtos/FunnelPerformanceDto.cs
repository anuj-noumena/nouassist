using System;
using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos
{
    public class FunnelPerformanceDto
    {
        public Guid FunnelId { get; set; }
        public string FunnelName { get; set; }
        public double OverallConversionRate { get; set; }
        public List<FunnelStageMetricDto> StageMetrics { get; set; }
    }
}
