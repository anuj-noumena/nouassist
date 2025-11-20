using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos
{
    public class CreateFunnelStageDto
    {
        [Required]
        [StringLength(GrowthExperimentationConstants.ConversionFunnels.MaxStageNameLength)]
        public string Name { get; set; }
        
        [StringLength(GrowthExperimentationConstants.ConversionFunnels.MaxFunnelDescriptionLength)]
        public string Description { get; set; }
        
        [Range(1, GrowthExperimentationConstants.ConversionFunnels.MaxFunnelStages)]
        public int Order { get; set; }
        
        [StringLength(500)]
        public string Criteria { get; set; }
        
        public bool IsExitStage { get; set; }
    }
}
