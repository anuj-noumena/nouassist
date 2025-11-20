namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Events
{
    /// <summary>
    /// Event published when a new workflow is designed
    /// </summary>
    [EventName("NouAssist.IntelligenceStrategy.WorkflowEngine.WorkflowDesigned")]
    public class WorkflowDesignedEto : WorkflowEventBase
    {
        /// <summary>
        /// The name of the designed workflow
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The version number of the workflow
        /// </summary>
        public int VersionNumber { get; set; }
    }
}
