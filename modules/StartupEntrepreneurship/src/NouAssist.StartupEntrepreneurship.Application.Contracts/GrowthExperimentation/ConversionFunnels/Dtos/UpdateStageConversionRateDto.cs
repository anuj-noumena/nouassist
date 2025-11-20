using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos
{
    public class UpdateStageConversionRateDto
    {
        [Range(0, int.MaxValue)]
        public int EnteredCount { get; set; }
        
        [Range(0, int.MaxValue)]
        public int CompletedCount { get; set; }
    }
}
