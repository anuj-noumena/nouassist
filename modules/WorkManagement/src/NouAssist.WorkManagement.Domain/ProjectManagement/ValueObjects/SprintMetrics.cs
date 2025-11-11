using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.ProjectManagement.ValueObjects
{
    public class SprintMetrics : ValueObject
    {
        public int PlannedStoryPoints { get; private set; }
        public int CompletedStoryPoints { get; private set; }
        public int VelocityEstimate { get; private set; }
        public int ActualVelocity { get; private set; }

        private SprintMetrics() { }

        public SprintMetrics(
            int plannedStoryPoints,
            int completedStoryPoints,
            int velocityEstimate,
            int actualVelocity)
        {
            PlannedStoryPoints = plannedStoryPoints;
            CompletedStoryPoints = completedStoryPoints;
            VelocityEstimate = velocityEstimate;
            ActualVelocity = actualVelocity;
        }

        public double CompletionRate => PlannedStoryPoints == 0 ? 0 : 
            (double)CompletedStoryPoints / PlannedStoryPoints * 100;

        public double VelocityAccuracy => VelocityEstimate == 0 ? 0 : 
            (double)ActualVelocity / VelocityEstimate * 100;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PlannedStoryPoints;
            yield return CompletedStoryPoints;
            yield return VelocityEstimate;
            yield return ActualVelocity;
        }
    }
}
