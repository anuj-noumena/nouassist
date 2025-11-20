namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Enums
{
    /// <summary>
    /// Represents the type of assignee for a task
    /// </summary>
    public enum AssigneeType
    {
        /// <summary>
        /// Task is assigned to a specific user
        /// </summary>
        User = 0,
        
        /// <summary>
        /// Task is assigned to a team
        /// </summary>
        Team = 1,
        
        /// <summary>
        /// Task is assigned to a role
        /// </summary>
        Role = 2
    }
}
