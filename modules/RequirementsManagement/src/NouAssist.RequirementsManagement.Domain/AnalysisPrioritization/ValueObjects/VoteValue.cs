namespace NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects
{
    /// <summary>
    /// Represents a vote value in prioritization activities
    /// </summary>
    public class VoteValue : ValueObject
    {
        public decimal NumericValue { get; private set; }
        public string? StringValue { get; private set; }
        public PrioritizationMethod Method { get; private set; }

        private VoteValue() { }

        public static VoteValue CreateMoSCoWVote(string moscowValue)
        {
            var validValues = new[] { "Must", "Should", "Could", "Won't" };
            if (!validValues.Contains(moscowValue))
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:InvalidMoSCoWValue");
            }

            var numericValue = moscowValue switch
            {
                "Must" => 4m,
                "Should" => 3m,
                "Could" => 2m,
                "Won't" => 1m,
                _ => 0m
            };

            return new VoteValue
            {
                StringValue = moscowValue,
                NumericValue = numericValue,
                Method = PrioritizationMethod.MoSCoW
            };
        }

        public static VoteValue CreateNumericVote(decimal value, PrioritizationMethod method)
        {
            if (value < 0 || value > 10)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:InvalidNumericVote");
            }

            return new VoteValue
            {
                NumericValue = value,
                Method = method
            };
        }

        public static VoteValue CreateWSJFVote(decimal businessValue, decimal timeCriticality, decimal riskReduction, decimal jobSize)
        {
            if (jobSize <= 0)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:InvalidJobSize");
            }

            var wsjfScore = (businessValue + timeCriticality + riskReduction) / jobSize;
            
            return new VoteValue
            {
                NumericValue = wsjfScore,
                Method = PrioritizationMethod.WSJF
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return NumericValue;
            yield return StringValue ?? string.Empty;
            yield return Method;
        }
    }
}
