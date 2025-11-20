namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects
{
    /// <summary>
    /// Value object representing feature prioritization inputs (Impact, Effort, Confidence)
    /// </summary>
    public class PriorityInput : ValueObject
    {
        public int Impact { get; private set; } // 1-5
        public int Effort { get; private set; } // 1-5
        public int Confidence { get; private set; } // 1-5
        public string Rationale { get; private set; }

        private PriorityInput() { }

        public PriorityInput(int impact, int effort, int confidence, string rationale = null)
        {
            SetImpact(impact);
            SetEffort(effort);
            SetConfidence(confidence);
            Rationale = rationale;
        }

        private void SetImpact(int impact)
        {
            if (impact < 1 || impact > 5)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidRange", 
                    "Impact must be between 1 and 5");
            }

            Impact = impact;
        }

        private void SetEffort(int effort)
        {
            if (effort < 1 || effort > 5)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidRange", 
                    "Effort must be between 1 and 5");
            }

            Effort = effort;
        }

        private void SetConfidence(int confidence)
        {
            if (confidence < 1 || confidence > 5)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidRange", 
                    "Confidence must be between 1 and 5");
            }

            Confidence = confidence;
        }

        public decimal CalculatePriorityScore()
        {
            // ICE Score: Impact * Confidence / Effort
            return (decimal)(Impact * Confidence) / Effort;
        }

        public FeaturePriority DeterminePriority()
        {
            var score = CalculatePriorityScore();
            return score switch
            {
                >= 15 => FeaturePriority.Critical,
                >= 10 => FeaturePriority.High,
                >= 5 => FeaturePriority.Medium,
                _ => FeaturePriority.Low
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Impact;
            yield return Effort;
            yield return Confidence;
            yield return Rationale ?? string.Empty;
        }
    }
}
