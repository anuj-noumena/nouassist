using System;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos
{
    public class ConversionRatePerStageDto
    {
        public Guid StageId { get; set; }
        public int EnteredCount { get; set; }
        public int CompletedCount { get; set; }
        public double ConversionRate { get; set; }
        public DateTime MeasuredAt { get; set; }
    }
}
