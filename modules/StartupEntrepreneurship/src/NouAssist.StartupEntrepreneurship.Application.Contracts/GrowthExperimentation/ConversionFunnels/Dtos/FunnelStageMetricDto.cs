using System;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos
{
    public class FunnelStageMetricDto
    {
        public Guid StageId { get; set; }
        public string StageName { get; set; }
        public int Order { get; set; }
        public int EnteredCount { get; set; }
        public int CompletedCount { get; set; }
        public double ConversionRate { get; set; }
        public bool IsEntryStage { get; set; }
        public bool IsExitStage { get; set; }
    }
}
