namespace NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects
{
    /// <summary>
    /// Represents the result of impact analysis on a requirement
    /// </summary>
    public class ImpactAssessmentResult : ValueObject
    {
        public ImpactLevel TechnicalImpact { get; private set; }
        public ImpactLevel BusinessImpact { get; private set; }
        public ImpactLevel UserImpact { get; private set; }
        public ImpactLevel RiskImpact { get; private set; }
        public string Description { get; private set; }
        public decimal OverallScore { get; private set; }

        private ImpactAssessmentResult() { }

        public ImpactAssessmentResult(
            ImpactLevel technicalImpact,
            ImpactLevel businessImpact,
            ImpactLevel userImpact,
            ImpactLevel riskImpact,
            string description)
        {
            TechnicalImpact = technicalImpact;
            BusinessImpact = businessImpact;
            UserImpact = userImpact;
            RiskImpact = riskImpact;
            SetDescription(description);
            CalculateOverallScore();
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: AnalysisConsts.MaxImpactDescriptionLength
            );
        }

        private void CalculateOverallScore()
        {
            // Weighted calculation: Technical 30%, Business 40%, User 20%, Risk 10%
            OverallScore = ((int)TechnicalImpact * 0.3m) +
                          ((int)BusinessImpact * 0.4m) +
                          ((int)UserImpact * 0.2m) +
                          ((int)RiskImpact * 0.1m);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TechnicalImpact;
            yield return BusinessImpact;
            yield return UserImpact;
            yield return RiskImpact;
            yield return Description;
            yield return OverallScore;
        }
    }
}
