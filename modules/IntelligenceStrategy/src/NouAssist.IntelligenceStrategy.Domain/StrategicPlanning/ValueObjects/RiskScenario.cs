namespace NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects
{
    /// <summary>
    /// Value object representing a risk scenario for crisis planning
    /// </summary>
    public class RiskScenario : ValueObject
    {
        public string Description { get; private set; }
        public CrisisLevel Level { get; private set; }
        public decimal ProbabilityScore { get; private set; }
        public decimal ImpactScore { get; private set; }

        private RiskScenario() { }

        public RiskScenario(string description, CrisisLevel level, decimal probabilityScore, decimal impactScore)
        {
            SetDescription(description);
            Level = level;
            SetProbabilityScore(probabilityScore);
            SetImpactScore(impactScore);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: StrategicPlanningConsts.MaxContingencyDescriptionLength
            );
        }

        private void SetProbabilityScore(decimal score)
        {
            if (score < 0 || score > 1)
            {
                throw new ArgumentException("Probability score must be between 0 and 1");
            }
            ProbabilityScore = score;
        }

        private void SetImpactScore(decimal score)
        {
            if (score < 0 || score > 1)
            {
                throw new ArgumentException("Impact score must be between 0 and 1");
            }
            ImpactScore = score;
        }

        public decimal RiskScore => ProbabilityScore * ImpactScore;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Description;
            yield return Level;
            yield return ProbabilityScore;
            yield return ImpactScore;
        }
    }
}
