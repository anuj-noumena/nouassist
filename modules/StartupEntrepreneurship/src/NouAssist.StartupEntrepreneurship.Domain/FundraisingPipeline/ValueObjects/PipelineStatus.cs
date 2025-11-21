using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Exceptions;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects
{
    /// <summary>
    /// Value object representing a prospect's status in the pipeline with additional metadata
    /// </summary>
    public class PipelineStatus : ValueObject
    {
        public ProspectStatus Status { get; private set; }
        public DateTime ChangedAt { get; private set; }
        public int DaysInCurrentStatus { get; private set; }
        public double ConversionProbability { get; private set; } // 0.0 to 1.0
        
        private PipelineStatus() { }
        
        public PipelineStatus(ProspectStatus status, DateTime changedAt, double conversionProbability = 0)
        {
            Status = status;
            ChangedAt = changedAt;
            SetConversionProbability(conversionProbability);
            UpdateDaysInCurrentStatus(DateTime.Now);
        }
        
        public void UpdateStatus(ProspectStatus newStatus, DateTime changeTime)
        {
            ValidateStatusTransition(newStatus);
            Status = newStatus;
            ChangedAt = changeTime;
            UpdateDaysInCurrentStatus(DateTime.Now);
        }
        
        public void UpdateDaysInCurrentStatus(DateTime referenceDate)
        {
            DaysInCurrentStatus = (int)(referenceDate - ChangedAt).TotalDays;
        }
        
        public void SetConversionProbability(double probability)
        {
            if (probability < 0 || probability > 1.0)
            {
                throw new ArgumentException("Conversion probability must be between 0 and 1.");
            }
            
            ConversionProbability = probability;
        }
        
        private void ValidateStatusTransition(ProspectStatus newStatus)
        {
            // Implement status transition rules
            // This is simplified; in a real implementation, you might have more complex rules
            if (Status == ProspectStatus.Closed || Status == ProspectStatus.Declined)
            {
                throw new InvalidProspectStatusTransitionException(Status, newStatus);
            }
            
            // Prevent backwards movement in the pipeline (simplified)
            if ((int)newStatus < (int)Status && newStatus != ProspectStatus.Declined)
            {
                throw new InvalidProspectStatusTransitionException(Status, newStatus);
            }
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Status;
            yield return ChangedAt;
            yield return DaysInCurrentStatus;
            yield return ConversionProbability;
        }
    }
}
