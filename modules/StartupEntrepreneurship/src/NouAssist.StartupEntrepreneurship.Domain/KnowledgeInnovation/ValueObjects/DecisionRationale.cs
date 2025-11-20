namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects
{
    /// <summary>
    /// Value object representing the rationale for a decision
    /// </summary>
    public class DecisionRationale : ValueObject
    {
        public string Reasoning { get; private set; }
        public List<string> KeyFactors { get; private set; }
        public List<string> RisksIdentified { get; private set; }
        public List<string> AssumptionsMade { get; private set; }

        private DecisionRationale() 
        {
            KeyFactors = new List<string>();
            RisksIdentified = new List<string>();
            AssumptionsMade = new List<string>();
        }

        public DecisionRationale(
            string reasoning, 
            List<string> keyFactors = null, 
            List<string> risksIdentified = null, 
            List<string> assumptionsMade = null)
        {
            Reasoning = Check.NotNullOrWhiteSpace(
                reasoning, 
                nameof(reasoning), 
                maxLength: KnowledgeInnovationConsts.DecisionRecord.RationaleMaxLength);
            
            KeyFactors = keyFactors ?? new List<string>();
            RisksIdentified = risksIdentified ?? new List<string>();
            AssumptionsMade = assumptionsMade ?? new List<string>();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Reasoning;
            
            foreach (var factor in KeyFactors.OrderBy(f => f))
            {
                yield return factor;
            }
            
            foreach (var risk in RisksIdentified.OrderBy(r => r))
            {
                yield return risk;
            }
            
            foreach (var assumption in AssumptionsMade.OrderBy(a => a))
            {
                yield return assumption;
            }
        }
    }
}
