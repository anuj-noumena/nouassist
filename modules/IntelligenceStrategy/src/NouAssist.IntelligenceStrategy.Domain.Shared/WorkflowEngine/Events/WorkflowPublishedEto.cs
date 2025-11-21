using NouAssist.IntelligenceStrategy.WorkflowEngine.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Events
{
    /// <summary>
    /// Event published when a workflow is published
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.WorkflowEngine.WorkflowPublished")]
    public class WorkflowPublishedEto : WorkflowEventBase
    {
        /// <summary>
        /// The name of the workflow
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The version number that was published
        /// </summary>
        public int VersionNumber { get; set; }
    }
}
