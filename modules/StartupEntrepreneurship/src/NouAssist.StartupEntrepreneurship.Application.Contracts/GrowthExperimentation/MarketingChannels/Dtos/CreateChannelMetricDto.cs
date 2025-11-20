using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos
{
    public class CreateChannelMetricDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        public MetricType Type { get; set; }
        
        public double InitialValue { get; set; }
        
        public double TargetValue { get; set; }
    }
}
