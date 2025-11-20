namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects
{
    /// <summary>
    /// Value object representing a metric for hypothesis validation
    /// </summary>
    public class ValidationMetric : ValueObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal TargetValue { get; private set; }
        public decimal ActualValue { get; private set; }
        public string Unit { get; private set; }

        private ValidationMetric() { }

        public ValidationMetric(string name, string description, decimal targetValue, string unit, decimal actualValue = 0)
        {
            SetName(name);
            SetDescription(description);
            SetTargetValue(targetValue);
            SetUnit(unit);
            ActualValue = actualValue;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: 100
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: 500
            );
        }

        private void SetTargetValue(decimal targetValue)
        {
            if (targetValue < 0)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidValue", 
                    "Target value cannot be negative");
            }

            TargetValue = targetValue;
        }

        private void SetUnit(string unit)
        {
            Unit = Check.NotNullOrWhiteSpace(
                unit,
                nameof(unit),
                maxLength: 20
            );
        }

        public void UpdateActualValue(decimal actualValue)
        {
            if (actualValue < 0)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidValue", 
                    "Actual value cannot be negative");
            }

            ActualValue = actualValue;
        }

        public bool IsTargetMet()
        {
            return ActualValue >= TargetValue;
        }

        public decimal PercentageAchieved()
        {
            if (TargetValue == 0) return 0;
            return (ActualValue / TargetValue) * 100;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Description;
            yield return TargetValue;
            yield return ActualValue;
            yield return Unit;
        }
    }
}
