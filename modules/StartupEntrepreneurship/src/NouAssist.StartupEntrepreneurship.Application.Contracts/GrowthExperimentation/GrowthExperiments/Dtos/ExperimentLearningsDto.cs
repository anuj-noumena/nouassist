using System;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class ExperimentLearningsDto
    {
        public string Insights { get; set; }
        public string UnexpectedOutcomes { get; set; }
        public string Recommendations { get; set; }
        public Guid DocumentedBy { get; set; }
        public DateTime DocumentedAt { get; set; }
    }
}
