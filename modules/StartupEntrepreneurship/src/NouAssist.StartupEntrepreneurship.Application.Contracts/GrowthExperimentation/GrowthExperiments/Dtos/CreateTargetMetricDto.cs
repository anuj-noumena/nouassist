using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class CreateTargetMetricDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        public MetricType Type { get; set; }
        
        public double BaselineValue { get; set; }
        
        public double TargetValue { get; set; }
    }
}
