using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ProjectExecution.Entities
{
    /// <summary>
    /// Represents an assignment of a task to a user
    /// </summary>
    public class TaskAssignment : Entity
    {
        /// <summary>
        /// ID of the task
        /// </summary>
        public Guid TaskId { get; private set; }
        
        /// <summary>
        /// ID of the assigned user
        /// </summary>
        public Guid UserId { get; private set; }
        
        /// <summary>
        /// Date when the task was assigned
        /// </summary>
        public DateTime AssignedDate { get; private set; }
        
        /// <summary>
        /// Date when the task was unassigned, if applicable
        /// </summary>
        public DateTime? UnassignedDate { get; private set; }
        
        /// <summary>
        /// Indicates if the assignment is active
        /// </summary>
        public bool IsActive => !UnassignedDate.HasValue;
        
        protected TaskAssignment() { }
        
        internal TaskAssignment(
            Guid taskId,
            Guid userId,
            DateTime assignedDate)
        {
            TaskId = taskId;
            UserId = userId;
            AssignedDate = assignedDate;
        }
        
        internal void Unassign(DateTime unassignedDate)
        {
            if (unassignedDate < AssignedDate)
            {
                throw new BusinessException("Unassigned date cannot be earlier than assigned date");
            }
            
            UnassignedDate = unassignedDate;
        }
        
        public override object[] GetKeys()
        {
            return new object[] { TaskId, UserId, AssignedDate };
        }
    }
}
