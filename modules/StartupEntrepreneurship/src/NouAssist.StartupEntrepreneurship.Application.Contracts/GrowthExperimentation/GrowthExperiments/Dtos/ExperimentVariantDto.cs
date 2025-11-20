using System;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class ExperimentVariantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public VariantType Type { get; set; }
        public VariantSetupDto Setup { get; set; }
    }
}
