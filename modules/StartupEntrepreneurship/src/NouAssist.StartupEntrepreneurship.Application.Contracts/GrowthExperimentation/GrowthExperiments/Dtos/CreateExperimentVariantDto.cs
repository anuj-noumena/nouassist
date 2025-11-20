using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class CreateExperimentVariantDto
    {
        [Required]
        [StringLength(GrowthExperimentationConstants.Experiments.MaxVariantNameLength)]
        public string Name { get; set; }
        
        [StringLength(GrowthExperimentationConstants.Experiments.MaxDescriptionLength)]
        public string Description { get; set; }
        
        public VariantType Type { get; set; }
        
        [Required]
        public CreateVariantSetupDto Setup { get; set; }
    }
}
