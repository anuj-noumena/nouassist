using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects
{
    /// <summary>
    /// Value object that represents conversion rates for each stage in a funnel
    /// </summary>
    public class ConversionRatePerStage : ValueObject
    {
        /// <summary>
        /// Funnel stage ID
        /// </summary>
        public Guid StageId { get; private set; }
        
        /// <summary>
        /// Number of users who entered this stage
        /// </summary>
        public int EnteredCount { get; private set; }
        
        /// <summary>
        /// Number of users who completed this stage
        /// </summary>
        public int CompletedCount { get; private set; }
        
        /// <summary>
        /// Conversion rate for this stage
        /// </summary>
        public double ConversionRate => EnteredCount > 0 ? (double)CompletedCount / EnteredCount : 0;
        
        /// <summary>
        /// When this data was measured
        /// </summary>
        public DateTime MeasuredAt { get; private set; }
        
        protected ConversionRatePerStage() { }
        
        public ConversionRatePerStage(Guid stageId, int enteredCount, int completedCount, DateTime measuredAt)
        {
            StageId = stageId;
            SetEnteredCount(enteredCount);
            SetCompletedCount(completedCount, enteredCount);
            MeasuredAt = measuredAt;
        }
        
        private void SetEnteredCount(int enteredCount)
        {
            if (enteredCount < 0)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", enteredCount)
                    .WithData("property", "EnteredCount");
            }
            
            EnteredCount = enteredCount;
        }
        
        private void SetCompletedCount(int completedCount, int enteredCount)
        {
            if (completedCount < 0)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", completedCount)
                    .WithData("property", "CompletedCount");
            }
            
            if (completedCount > enteredCount)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("completedCount", completedCount)
                    .WithData("enteredCount", enteredCount)
                    .WithData("property", "CompletedCount");
            }
            
            CompletedCount = completedCount;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StageId;
            yield return EnteredCount;
            yield return CompletedCount;
            yield return MeasuredAt;
        }
    }
}
