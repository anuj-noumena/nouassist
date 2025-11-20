using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos
{
    public class CreateMarketingChannelDto
    {
        [Required]
        [StringLength(GrowthExperimentationConstants.MarketingChannels.MaxChannelNameLength)]
        public string Name { get; set; }
        
        [StringLength(GrowthExperimentationConstants.MarketingChannels.MaxChannelDescriptionLength)]
        public string Description { get; set; }
        
        public MarketingChannelType Type { get; set; }
        
        [Required]
        public CreateChannelDetailsDto Details { get; set; }
    }
}
