using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects
{
    /// <summary>
    /// Represents the output of sensitivity analysis
    /// </summary>
    public class SensitivityOutput : ValueObject
    {
        public string VariableName { get; private set; }
        public decimal BaseValue { get; private set; }
        public decimal BaseResult { get; private set; }
        public Dictionary<decimal, decimal> SensitivityPoints { get; private set; }
        public decimal Elasticity { get; private set; }
        public decimal MaxImpact { get; private set; }
        public decimal MinImpact { get; private set; }
        public DateTime CalculatedAt { get; private set; }

        private SensitivityOutput() 
        {
            SensitivityPoints = new Dictionary<decimal, decimal>();
        }

        public SensitivityOutput(string variableName, decimal baseValue, decimal baseResult,
                               Dictionary<decimal, decimal> sensitivityPoints, 
                               decimal elasticity, decimal maxImpact, decimal minImpact)
        {
            VariableName = Check.NotNullOrWhiteSpace(variableName, nameof(variableName));
            BaseValue = baseValue;
            BaseResult = baseResult;
            SensitivityPoints = sensitivityPoints ?? new Dictionary<decimal, decimal>();
            Elasticity = elasticity;
            MaxImpact = maxImpact;
            MinImpact = minImpact;
            CalculatedAt = DateTime.UtcNow;
        }

        public bool IsHighSensitivity(decimal threshold = 1.0m)
        {
            return Math.Abs(Elasticity) > threshold;
        }

        public decimal GetImpactForVariance(decimal variancePercentage)
        {
            if (SensitivityPoints.ContainsKey(variancePercentage))
            {
                return SensitivityPoints[variancePercentage];
            }

            // Linear interpolation for intermediate values
            var lowerKey = SensitivityPoints.Keys.Where(k => k <= variancePercentage).Max();
            var upperKey = SensitivityPoints.Keys.Where(k => k >= variancePercentage).Min();

            if (lowerKey == upperKey)
            {
                return SensitivityPoints[lowerKey];
            }

            var lowerValue = SensitivityPoints[lowerKey];
            var upperValue = SensitivityPoints[upperKey];
            var ratio = (variancePercentage - lowerKey) / (upperKey - lowerKey);
            
            return lowerValue + (ratio * (upperValue - lowerValue));
        }

        public List<decimal> GetRiskRanges()
        {
            var impacts = SensitivityPoints.Values.OrderBy(v => v).ToList();
            return new List<decimal> { impacts.First(), impacts.Last() };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return VariableName;
            yield return BaseValue;
            yield return BaseResult;
            yield return string.Join(",", SensitivityPoints.Select(kv => $"{kv.Key}:{kv.Value}"));
            yield return Elasticity;
            yield return MaxImpact;
            yield return MinImpact;
            yield return CalculatedAt;
        }
    }
}
