using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class CreateVariantSetupDto
    {
        [Range(0, 1)]
        public double TrafficAllocation { get; set; }
        
        public string Configuration { get; set; }
        
        [Url]
        public string Url { get; set; }
    }
}
