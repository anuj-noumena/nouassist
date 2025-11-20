namespace NouAssist.BusinessOperations.ProjectExecution.Policies
{
    /// <summary>
    /// Policy defining valid project status transitions
    /// </summary>
    public static class ProjectStatusTransitionPolicy
    {
        private static readonly Dictionary<ProjectStatus, HashSet<ProjectStatus>> _allowedTransitions = new()
        {
            [ProjectStatus.Planning] = new HashSet<ProjectStatus> { ProjectStatus.Active, ProjectStatus.Cancelled },
            [ProjectStatus.Active] = new HashSet<ProjectStatus> { ProjectStatus.OnHold, ProjectStatus.Completed, ProjectStatus.Cancelled },
            [ProjectStatus.OnHold] = new HashSet<ProjectStatus> { ProjectStatus.Active, ProjectStatus.Cancelled },
            [ProjectStatus.Completed] = new HashSet<ProjectStatus>(),
            [ProjectStatus.Cancelled] = new HashSet<ProjectStatus>()
        };
        
        /// <summary>
        /// Checks if a status transition is valid
        /// </summary>
        /// <param name="currentStatus">Current status</param>
        /// <param name="targetStatus">Target status</param>
        /// <returns>True if the transition is valid, false otherwise</returns>
        public static bool IsValidTransition(ProjectStatus currentStatus, ProjectStatus targetStatus)
        {
            if (currentStatus == targetStatus)
            {
                return true;
            }
            
            return _allowedTransitions.ContainsKey(currentStatus) && 
                   _allowedTransitions[currentStatus].Contains(targetStatus);
        }
    }
}
