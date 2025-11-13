using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.ProjectManagement.ValueObjects
{
    /// <summary>
    /// Sprint metrics value object - IMPROVED
    /// </summary>
    public class SprintMetrics : ValueObject
    {
        public int PlannedStoryPoints { get; private set; }
        public int CompletedStoryPoints { get; private set; }
        public int VelocityEstimate { get; private set; }
        public int ActualVelocity { get; private set; }
        
        private SprintMetrics() { }
        
        public static SprintMetrics Initialize(int velocityEstimate)
        {
            return new SprintMetrics
            {
                PlannedStoryPoints = 0,
                CompletedStoryPoints = 0,
                VelocityEstimate = velocityEstimate,
                ActualVelocity = 0
            };
        }
        
        public SprintMetrics Update(int? planned = null, int? completed = null)
        {
            var newPlanned = planned ?? this.PlannedStoryPoints;
            var newCompleted = completed ?? this.CompletedStoryPoints;
            
            if (newPlanned < 0)
            {
                throw new BusinessException("Sprints:InvalidStoryPoints");
            }
            
            if (newCompleted < 0 || newCompleted > newPlanned)
            {
                throw new BusinessException("Sprints:InvalidCompletedStoryPoints");
            }
            
            return new SprintMetrics
            {
                PlannedStoryPoints = newPlanned,
                CompletedStoryPoints = newCompleted,
                VelocityEstimate = this.VelocityEstimate,
                ActualVelocity = this.ActualVelocity
            };
        }
        
        public SprintMetrics Complete()
        {
            return new SprintMetrics
            {
                PlannedStoryPoints = this.PlannedStoryPoints,
                CompletedStoryPoints = this.CompletedStoryPoints,
                VelocityEstimate = this.VelocityEstimate,
                ActualVelocity = this.CompletedStoryPoints
            };
        }
        
        public double CompletionRate => PlannedStoryPoints == 0 
            ? 0 
            : (double)CompletedStoryPoints / PlannedStoryPoints * 100;
        
        public double VelocityAccuracy => VelocityEstimate == 0 
            ? 0 
            : (double)ActualVelocity / VelocityEstimate * 100;
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PlannedStoryPoints;
            yield return CompletedStoryPoints;
            yield return VelocityEstimate;
            yield return ActualVelocity;
        }
    }
}
