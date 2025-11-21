using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects
{
    /// <summary>
    /// Value object for forecasting potential investment values
    /// </summary>
    public class OpportunityForecastValue : ValueObject
    {
        public decimal MinAmount { get; private set; }
        public decimal MaxAmount { get; private set; }
        public decimal ExpectedAmount { get; private set; }
        public double Probability { get; private set; } // 0.0 to 1.0
        public decimal WeightedAmount => ExpectedAmount * (decimal)Probability;
        public DateTime ForecastedClosingDate { get; private set; }
        
        private OpportunityForecastValue() { }
        
        public OpportunityForecastValue(
            decimal minAmount, 
            decimal maxAmount, 
            decimal expectedAmount, 
            double probability, 
            DateTime forecastedClosingDate)
        {
            ValidateAmounts(minAmount, maxAmount, expectedAmount);
            SetProbability(probability);
            
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            ExpectedAmount = expectedAmount;
            ForecastedClosingDate = forecastedClosingDate;
        }
        
        private void ValidateAmounts(decimal min, decimal max, decimal expected)
        {
            if (min < 0 || max < 0 || expected < 0)
            {
                throw new ArgumentException("Amounts cannot be negative.");
            }
            
            if (min > max)
            {
                throw new ArgumentException("Minimum amount cannot be greater than maximum amount.");
            }
            
            if (expected < min || expected > max)
            {
                throw new ArgumentException("Expected amount must be between minimum and maximum amounts.");
            }
        }
        
        private void SetProbability(double probability)
        {
            if (probability < 0 || probability > 1.0)
            {
                throw new ArgumentException("Probability must be between 0 and 1.");
            }
            
            Probability = probability;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MinAmount;
            yield return MaxAmount;
            yield return ExpectedAmount;
            yield return Probability;
            yield return ForecastedClosingDate;
        }
    }
}
