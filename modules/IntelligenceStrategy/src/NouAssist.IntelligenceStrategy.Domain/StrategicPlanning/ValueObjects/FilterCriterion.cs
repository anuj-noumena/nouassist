namespace NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects
{
    /// <summary>
    /// Value object representing opportunity filter criteria
    /// </summary>
    public class FilterCriterion : ValueObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal MinThreshold { get; private set; }
        public decimal MaxThreshold { get; private set; }
        public int Weight { get; private set; }

        private FilterCriterion() { }

        public FilterCriterion(string name, string description, decimal minThreshold, decimal maxThreshold, int weight)
        {
            SetName(name);
            SetDescription(description);
            SetThresholds(minThreshold, maxThreshold);
            SetWeight(weight);
        }

        public bool Evaluate(decimal value)
        {
            return value >= MinThreshold && value <= MaxThreshold;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 100);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 500);
        }

        private void SetThresholds(decimal min, decimal max)
        {
            if (min > max)
                throw new ArgumentException("Minimum threshold cannot be greater than maximum threshold");
            
            MinThreshold = min;
            MaxThreshold = max;
        }

        private void SetWeight(int weight)
        {
            if (weight < 1 || weight > 10)
                throw new ArgumentException("Weight must be between 1 and 10");
            
            Weight = weight;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Description;
            yield return MinThreshold;
            yield return MaxThreshold;
            yield return Weight;
        }
    }
}
