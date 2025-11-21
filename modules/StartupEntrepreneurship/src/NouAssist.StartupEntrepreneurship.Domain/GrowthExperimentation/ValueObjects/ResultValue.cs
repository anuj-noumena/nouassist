using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects
{
    /// <summary>
    /// Value object that represents a metric result value
    /// </summary>
    public class ResultValue : ValueObject
    {
        /// <summary>
        /// The metric type
        /// </summary>
        public MetricType MetricType { get; private set; }
        
        /// <summary>
        /// The metric value
        /// </summary>
        public double Value { get; private set; }
        
        /// <summary>
        /// Sample size used to calculate the value
        /// </summary>
        public int SampleSize { get; private set; }
        
        /// <summary>
        /// Standard deviation, if applicable
        /// </summary>
        public double? StandardDeviation { get; private set; }
        
        /// <summary>
        /// Margin of error, if applicable
        /// </summary>
        public double? MarginOfError { get; private set; }
        
        protected ResultValue() { }
        
        public ResultValue(MetricType metricType, double value, int sampleSize, double? standardDeviation = null, double? marginOfError = null)
        {
            MetricType = metricType;
            SetValue(value);
            SetSampleSize(sampleSize);
            StandardDeviation = standardDeviation;
            MarginOfError = marginOfError;
        }
        
        private void SetValue(double value)
        {
            if (MetricType == MetricType.ConversionRate || MetricType == MetricType.ClickThroughRate || 
                MetricType == MetricType.RetentionRate || MetricType == MetricType.BounceRate)
            {
                if (value < 0 || value > 1)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidMetricValue")
                        .WithData("value", value)
                        .WithData("metricType", MetricType);
                }
            }
            else if (value < 0 && MetricType != MetricType.Custom)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidMetricValue")
                    .WithData("value", value)
                    .WithData("metricType", MetricType);
            }
            
            Value = value;
        }
        
        private void SetSampleSize(int sampleSize)
        {
            if (sampleSize <= 0)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", sampleSize)
                    .WithData("property", "SampleSize");
            }
            
            SampleSize = sampleSize;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MetricType;
            yield return Value;
            yield return SampleSize;
            
            if (StandardDeviation.HasValue)
            {
                yield return StandardDeviation.Value;
            }
            
            if (MarginOfError.HasValue)
            {
                yield return MarginOfError.Value;
            }
        }
    }
}
