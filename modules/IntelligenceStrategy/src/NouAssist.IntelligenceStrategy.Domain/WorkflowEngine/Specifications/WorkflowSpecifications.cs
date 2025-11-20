namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Specifications
{
    /// <summary>
    /// Base specification class for workflow definitions
    /// </summary>
    public abstract class WorkflowSpecification : Specification<WorkflowDefinition>
    {
    }

    /// <summary>
    /// Specification for published workflows
    /// </summary>
    public class PublishedWorkflowSpecification : WorkflowSpecification
    {
        public override Expression<Func<WorkflowDefinition, bool>> ToExpression()
        {
            return workflow => workflow.IsPublished;
        }
    }

    /// <summary>
    /// Specification for workflows in a specific category
    /// </summary>
    public class WorkflowCategorySpecification : WorkflowSpecification
    {
        private readonly string _category;

        public WorkflowCategorySpecification(string category)
        {
            _category = category;
        }

        public override Expression<Func<WorkflowDefinition, bool>> ToExpression()
        {
            return workflow => workflow.Category == _category;
        }
    }

    /// <summary>
    /// Specification for workflows with performance metrics above a threshold
    /// </summary>
    public class PerformanceMetricsSpecification : WorkflowSpecification
    {
        private readonly double _minCycleTime;
        private readonly double _maxCycleTime;
        private readonly int _maxBottleneckScore;

        public PerformanceMetricsSpecification(
            double minCycleTime = 0,
            double maxCycleTime = double.MaxValue,
            int maxBottleneckScore = 100)
        {
            _minCycleTime = minCycleTime;
            _maxCycleTime = maxCycleTime;
            _maxBottleneckScore = maxBottleneckScore;
        }

        public override Expression<Func<WorkflowDefinition, bool>> ToExpression()
        {
            return workflow => 
                workflow.PerformanceMetrics != null && 
                workflow.PerformanceMetrics.AverageCycleTimeMinutes >= _minCycleTime &&
                workflow.PerformanceMetrics.AverageCycleTimeMinutes <= _maxCycleTime &&
                workflow.PerformanceMetrics.BottleneckScore <= _maxBottleneckScore;
        }
    }
}
