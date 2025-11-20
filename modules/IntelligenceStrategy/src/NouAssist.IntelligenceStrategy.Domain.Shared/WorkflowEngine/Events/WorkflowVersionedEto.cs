namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Events
{
    /// <summary>
    /// Event published when a workflow is versioned
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.WorkflowEngine.WorkflowVersioned")]
    public class WorkflowVersionedEto : WorkflowEventBase
    {
        /// <summary>
        /// The name of the workflow
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The version number
        /// </summary>
        public int VersionNumber { get; set; }
    }
}
