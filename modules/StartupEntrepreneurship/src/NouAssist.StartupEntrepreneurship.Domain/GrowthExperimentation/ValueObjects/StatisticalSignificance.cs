using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects
{
    /// <summary>
    /// Value object that represents the statistical significance of experiment results
    /// </summary>
    public class StatisticalSignificance : ValueObject
    {
        /// <summary>
        /// The p-value of the statistical test
        /// </summary>
        public double PValue { get; private set; }
        
        /// <summary>
        /// The confidence level (e.g., 0.95 for 95% confidence)
        /// </summary>
        public double ConfidenceLevel { get; private set; }
        
        /// <summary>
        /// Whether the result is statistically significant
        /// </summary>
        public bool IsSignificant => PValue <= (1 - ConfidenceLevel);
        
        /// <summary>
        /// Sample size used in the analysis
        /// </summary>
        public int SampleSize { get; private set; }
        
        /// <summary>
        /// Statistical power of the test
        /// </summary>
        public double? Power { get; private set; }
        
        protected StatisticalSignificance() { }
        
        public StatisticalSignificance(double pValue, double confidenceLevel, int sampleSize, double? power = null)
        {
            SetPValue(pValue);
            SetConfidenceLevel(confidenceLevel);
            SetSampleSize(sampleSize);
            Power = power;
        }
        
        private void SetPValue(double pValue)
        {
            if (pValue < 0 || pValue > 1)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", pValue)
                    .WithData("property", "PValue");
            }
            
            PValue = pValue;
        }
        
        private void SetConfidenceLevel(double confidenceLevel)
        {
            if (confidenceLevel < 0 || confidenceLevel > 1)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", confidenceLevel)
                    .WithData("property", "ConfidenceLevel");
            }
            
            if (confidenceLevel < GrowthExperimentationConstants.Experiments.MinSignificanceThreshold)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", confidenceLevel)
                    .WithData("property", "ConfidenceLevel")
                    .WithData("minValue", GrowthExperimentationConstants.Experiments.MinSignificanceThreshold);
            }
            
            ConfidenceLevel = confidenceLevel;
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
            yield return PValue;
            yield return ConfidenceLevel;
            yield return SampleSize;
            if (Power.HasValue)
            {
                yield return Power.Value;
            }
        }
    }
}
