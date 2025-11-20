namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class StatisticalSignificanceDto
    {
        public double PValue { get; set; }
        public double ConfidenceLevel { get; set; }
        public bool IsSignificant { get; set; }
        public int SampleSize { get; set; }
        public double? Power { get; set; }
    }
}
