namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects
{
    /// <summary>
    /// Value object that represents a target metric for an experiment
    /// </summary>
    public class TargetMetric : ValueObject
    {
        /// <summary>
        /// Name of the metric
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Type of the metric
        /// </summary>
        public MetricType Type { get; private set; }
        
        /// <summary>
        /// Baseline value of the metric
        /// </summary>
        public double BaselineValue { get; private set; }
        
        /// <summary>
        /// Target value for the metric
        /// </summary>
        public double TargetValue { get; private set; }
        
        /// <summary>
        /// Whether this is the primary metric for the experiment
        /// </summary>
        public bool IsPrimary { get; private set; }
        
        protected TargetMetric() { }
        
        public TargetMetric(string name, MetricType type, double baselineValue, double targetValue, bool isPrimary = false)
        {
            SetName(name);
            Type = type;
            SetBaselineValue(baselineValue);
            SetTargetValue(targetValue);
            IsPrimary = isPrimary;
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: 100
            );
        }
        
        private void SetBaselineValue(double baselineValue)
        {
            if (Type == MetricType.ConversionRate || Type == MetricType.ClickThroughRate || 
                Type == MetricType.RetentionRate || Type == MetricType.BounceRate)
            {
                if (baselineValue < 0 || baselineValue > 1)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidMetricValue")
                        .WithData("value", baselineValue)
                        .WithData("metricType", Type);
                }
            }
            else if (baselineValue < 0 && Type != MetricType.Custom)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidMetricValue")
                    .WithData("value", baselineValue)
                    .WithData("metricType", Type);
            }
            
            BaselineValue = baselineValue;
        }
        
        private void SetTargetValue(double targetValue)
        {
            if (Type == MetricType.ConversionRate || Type == MetricType.ClickThroughRate || 
                Type == MetricType.RetentionRate || Type == MetricType.BounceRate)
            {
                if (targetValue < 0 || targetValue > 1)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidMetricValue")
                        .WithData("value", targetValue)
                        .WithData("metricType", Type);
                }
            }
            else if (targetValue < 0 && Type != MetricType.Custom)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidMetricValue")
                    .WithData("value", targetValue)
                    .WithData("metricType", Type);
            }
            
            TargetValue = targetValue;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Type;
            yield return BaselineValue;
            yield return TargetValue;
            yield return IsPrimary;
        }
    }
}
