namespace NouAssist.WorkManagement.Workflow.Enums
{
    /// <summary>
    /// Defines the purpose/category of a workflow transition
    /// </summary>
    public enum TransitionPurpose
    {
        /// <summary>
        /// Standard workflow transition
        /// </summary>
        Standard = 0,
        
        /// <summary>
        /// Transition for approval workflows
        /// </summary>
        Approval = 1,
        
        /// <summary>
        /// Transition for rejection/decline
        /// </summary>
        Rejection = 2,
        
        /// <summary>
        /// Transition for delegation to another user
        /// </summary>
        Delegation = 3,
        
        /// <summary>
        /// Transition for escalation
        /// </summary>
        Escalation = 4,
        
        /// <summary>
        /// Transition for cancellation
        /// </summary>
        Cancellation = 5,
        
        /// <summary>
        /// Transition for completion/resolution
        /// </summary>
        Completion = 6
    }
}