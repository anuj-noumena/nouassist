using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ProjectExecution.Policies
{
    /// <summary>
    /// Policy defining valid task status transitions
    /// </summary>
    public static class TaskStatusTransitionPolicy
    {
        private static readonly Dictionary<TaskStatus, HashSet<TaskStatus>> _allowedTransitions = new()
        {
            [TaskStatus.ToDo] = new HashSet<TaskStatus> { TaskStatus.InProgress, TaskStatus.Blocked },
            [TaskStatus.InProgress] = new HashSet<TaskStatus> { TaskStatus.ToDo, TaskStatus.Done, TaskStatus.Blocked },
            [TaskStatus.Done] = new HashSet<TaskStatus> { TaskStatus.ToDo, TaskStatus.InProgress },
            [TaskStatus.Blocked] = new HashSet<TaskStatus> { TaskStatus.ToDo, TaskStatus.InProgress }
        };
        
        /// <summary>
        /// Checks if a status transition is valid
        /// </summary>
        /// <param name="currentStatus">Current status</param>
        /// <param name="targetStatus">Target status</param>
        /// <returns>True if the transition is valid, false otherwise</returns>
        public static bool IsValidTransition(TaskStatus currentStatus, TaskStatus targetStatus)
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
