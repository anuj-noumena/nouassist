namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Enums
{
    /// <summary>
    /// Represents the type of an activity within a workflow
    /// </summary>
    public enum ActivityType
    {
        /// <summary>
        /// A task to be performed by a user
        /// </summary>
        UserTask = 0,
        
        /// <summary>
        /// A task to be performed automatically by the system
        /// </summary>
        SystemTask = 1,
        
        /// <summary>
        /// A decision point in the workflow
        /// </summary>
        Gateway = 2,
        
        /// <summary>
        /// A starting point for the workflow
        /// </summary>
        StartEvent = 3,
        
        /// <summary>
        /// An ending point for the workflow
        /// </summary>
        EndEvent = 4,
        
        /// <summary>
        /// An intermediate event in the workflow
        /// </summary>
        IntermediateEvent = 5,
        
        /// <summary>
        /// A script to be executed as part of the workflow
        /// </summary>
        ScriptTask = 6,
        
        /// <summary>
        /// A service call to be made as part of the workflow
        /// </summary>
        ServiceTask = 7
    }
}
