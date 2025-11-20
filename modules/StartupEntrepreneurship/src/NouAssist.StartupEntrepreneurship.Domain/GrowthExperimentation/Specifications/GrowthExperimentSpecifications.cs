namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Specifications
{
    /// <summary>
    /// Base specification for GrowthExperiment entity
    /// </summary>
    public abstract class GrowthExperimentSpecification : Specification<GrowthExperiment>
    {
    }
    
    /// <summary>
    /// Specification for experiments that are ready to start
    /// </summary>
    public class ReadyToStartExperimentSpecification : GrowthExperimentSpecification
    {
        public override Expression<Func<GrowthExperiment, bool>> ToExpression()
        {
            return e => e.Status == ExperimentStatus.Ready;
        }
    }
    
    /// <summary>
    /// Specification for experiments that are running
    /// </summary>
    public class RunningExperimentSpecification : GrowthExperimentSpecification
    {
        public override Expression<Func<GrowthExperiment, bool>> ToExpression()
        {
            return e => e.Status == ExperimentStatus.Running;
        }
    }
    
    /// <summary>
    /// Specification for experiments that have been running for a specific duration
    /// </summary>
    public class ExperimentRunningForDurationSpecification : GrowthExperimentSpecification
    {
        private readonly int _minDaysRunning;
        
        public ExperimentRunningForDurationSpecification(int minDaysRunning)
        {
            _minDaysRunning = minDaysRunning;
        }
        
        public override Expression<Func<GrowthExperiment, bool>> ToExpression()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-_minDaysRunning);
            return e => e.Status == ExperimentStatus.Running && 
                       e.StartDate.HasValue && 
                       e.StartDate.Value <= cutoffDate;
        }
    }
    
    /// <summary>
    /// Specification for experiments that have completed analysis
    /// </summary>
    public class CompletedExperimentSpecification : GrowthExperimentSpecification
    {
        public override Expression<Func<GrowthExperiment, bool>> ToExpression()
        {
            return e => e.Status == ExperimentStatus.Completed;
        }
    }
    
    /// <summary>
    /// Specification for experiments with significant results
    /// </summary>
    public class SignificantResultsExperimentSpecification : GrowthExperimentSpecification
    {
        public override Expression<Func<GrowthExperiment, bool>> ToExpression()
        {
            return e => e.Status == ExperimentStatus.Completed && 
                       e.Significance != null && 
                       e.Significance.IsSignificant;
        }
    }
}
