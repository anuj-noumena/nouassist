using NouAssist.IntelligenceStrategy.WorkflowEngine.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Events
{
    /// <summary>
    /// Event published when workflow performance is analyzed
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.WorkflowEngine.WorkflowPerformanceAnalyzed")]
    public class WorkflowPerformanceAnalyzedEto : WorkflowEventBase
    {
        /// <summary>
        /// The name of the workflow
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The average cycle time in minutes
        /// </summary>
        public double AverageCycleTimeMinutes { get; set; }
        
        /// <summary>
        /// The bottleneck score
        /// </summary>
        public int BottleneckScore { get; set; }
    }
}
