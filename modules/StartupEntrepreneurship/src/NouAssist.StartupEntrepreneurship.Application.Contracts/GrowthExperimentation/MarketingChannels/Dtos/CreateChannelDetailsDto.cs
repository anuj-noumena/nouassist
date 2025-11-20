using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos
{
    public class CreateChannelDetailsDto
    {
        [Required]
        [StringLength(100)]
        public string Platform { get; set; }
        
        [StringLength(100)]
        public string AccountId { get; set; }
        
        [Url]
        public string Url { get; set; }
        
        public string Settings { get; set; }
    }
}
