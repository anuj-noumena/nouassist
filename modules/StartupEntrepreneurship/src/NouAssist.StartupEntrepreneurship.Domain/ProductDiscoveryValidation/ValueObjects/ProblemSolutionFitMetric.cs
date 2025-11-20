namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects
{
    /// <summary>
    /// Value object representing problem-solution fit measurement
    /// </summary>
    public class ProblemSolutionFitMetric : ValueObject
    {
        public decimal ProblemClarity { get; private set; } // 0-100
        public decimal SolutionEffectiveness { get; private set; } // 0-100
        public decimal CustomerValidation { get; private set; } // 0-100
        public decimal MarketDemand { get; private set; } // 0-100
        public string Notes { get; private set; }

        private ProblemSolutionFitMetric() { }

        public ProblemSolutionFitMetric(
            decimal problemClarity,
            decimal solutionEffectiveness,
            decimal customerValidation,
            decimal marketDemand,
            string notes = null)
        {
            SetProblemClarity(problemClarity);
            SetSolutionEffectiveness(solutionEffectiveness);
            SetCustomerValidation(customerValidation);
            SetMarketDemand(marketDemand);
            Notes = notes;
        }

        private void SetProblemClarity(decimal value)
        {
            ValidatePercentage(value, nameof(ProblemClarity));
            ProblemClarity = value;
        }

        private void SetSolutionEffectiveness(decimal value)
        {
            ValidatePercentage(value, nameof(SolutionEffectiveness));
            SolutionEffectiveness = value;
        }

        private void SetCustomerValidation(decimal value)
        {
            ValidatePercentage(value, nameof(CustomerValidation));
            CustomerValidation = value;
        }

        private void SetMarketDemand(decimal value)
        {
            ValidatePercentage(value, nameof(MarketDemand));
            MarketDemand = value;
        }

        private void ValidatePercentage(decimal value, string fieldName)
        {
            if (value < 0 || value > 100)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidRange", 
                    $"{fieldName} must be between 0 and 100");
            }
        }

        public decimal CalculateOverallFit()
        {
            return (ProblemClarity + SolutionEffectiveness + CustomerValidation + MarketDemand) / 4;
        }

        public string GetFitLevel()
        {
            var overallFit = CalculateOverallFit();
            return overallFit switch
            {
                >= 80 => "Strong Fit",
                >= 60 => "Good Fit",
                >= 40 => "Moderate Fit",
                >= 20 => "Weak Fit",
                _ => "Poor Fit"
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ProblemClarity;
            yield return SolutionEffectiveness;
            yield return CustomerValidation;
            yield return MarketDemand;
            yield return Notes ?? string.Empty;
        }
    }
}
