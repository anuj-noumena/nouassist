namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects
{
    /// <summary>
    /// Represents a threshold level for metric alerts
    /// </summary>
    public class ThresholdLevel : ValueObject
    {
        public decimal Value { get; private set; }
        public ThresholdType Type { get; private set; }
        public string AlertMessage { get; private set; }
        public bool IsEnabled { get; private set; }

        private ThresholdLevel() { }

        public ThresholdLevel(decimal value, ThresholdType type, string alertMessage = null, bool isEnabled = true)
        {
            Value = value;
            Type = type;
            AlertMessage = alertMessage?.Trim();
            IsEnabled = isEnabled;
        }

        public bool IsTriggered(MetricValue currentValue, MetricValue previousValue = null)
        {
            if (!IsEnabled) return false;

            return Type switch
            {
                ThresholdType.Above => currentValue.Value > Value,
                ThresholdType.Below => currentValue.Value < Value,
                ThresholdType.Equals => Math.Abs(currentValue.Value - Value) < 0.01m,
                _ => false
            };
        }

        public ThresholdLevel WithMessage(string message)
        {
            return new ThresholdLevel(Value, Type, message, IsEnabled);
        }

        public ThresholdLevel Enable()
        {
            return new ThresholdLevel(Value, Type, AlertMessage, true);
        }

        public ThresholdLevel Disable()
        {
            return new ThresholdLevel(Value, Type, AlertMessage, false);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Type;
            yield return AlertMessage;
            yield return IsEnabled;
        }
    }
}
