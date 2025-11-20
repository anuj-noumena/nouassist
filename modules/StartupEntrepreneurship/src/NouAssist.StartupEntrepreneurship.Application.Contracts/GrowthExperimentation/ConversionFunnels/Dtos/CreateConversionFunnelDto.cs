using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos
{
    public class CreateConversionFunnelDto
    {
        [Required]
        [StringLength(GrowthExperimentationConstants.ConversionFunnels.MaxFunnelNameLength)]
        public string Name { get; set; }
        
        [StringLength(GrowthExperimentationConstants.ConversionFunnels.MaxFunnelDescriptionLength)]
        public string Description { get; set; }
    }
}
