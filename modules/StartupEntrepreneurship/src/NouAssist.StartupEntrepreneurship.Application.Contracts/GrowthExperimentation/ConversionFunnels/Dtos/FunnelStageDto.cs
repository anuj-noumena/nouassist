using System;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos
{
    public class FunnelStageDto
    {
        public Guid Id { get; set; }
        public StageDefinitionDto Definition { get; set; }
        public ConversionRatePerStageDto ConversionRate { get; set; }
        public bool IsEntryStage { get; set; }
        public bool IsExitStage { get; set; }
    }
}
