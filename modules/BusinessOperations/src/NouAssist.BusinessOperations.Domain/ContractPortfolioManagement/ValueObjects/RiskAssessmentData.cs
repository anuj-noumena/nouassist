namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing risk assessment data
    /// </summary>
    public class RiskAssessmentData : ValueObject
    {
        public RiskLevel Level { get; private set; }
        public string Description { get; private set; }
        public string MitigationPlan { get; private set; }
        public decimal Probability { get; private set; } // 0.0 to 1.0
        public decimal Impact { get; private set; } // 1-10
        public DateTime AssessmentDate { get; private set; }

        protected RiskAssessmentData() { }

        public RiskAssessmentData(
            RiskLevel level, 
            string description, 
            string mitigationPlan, 
            decimal probability, 
            decimal impact, 
            DateTime assessmentDate)
        {
            Level = level;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            MitigationPlan = mitigationPlan;
            
            if (probability < 0 || probability > 1)
            {
                throw new BusinessException("ContractPortfolioManagement:Validation:InvalidValue")
                    .WithData("propertyName", "Probability");
            }
            
            if (impact < 1 || impact > 10)
            {
                throw new BusinessException("ContractPortfolioManagement:Validation:InvalidValue")
                    .WithData("propertyName", "Impact");
            }
            
            Probability = probability;
            Impact = impact;
            AssessmentDate = assessmentDate;
        }

        public decimal CalculateRiskScore()
        {
            return Probability * Impact;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Level;
            yield return Description;
            yield return MitigationPlan ?? string.Empty;
            yield return Probability;
            yield return Impact;
            yield return AssessmentDate;
        }
    }
}
