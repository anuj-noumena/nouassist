using System;
using NouAssist.BusinessOperations.ProjectExecution.Events;
using NouAssist.BusinessOperations.ProjectExecution.ValueObjects;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.ProjectExecution.Aggregates
{
    /// <summary>
    /// TimeLog aggregate root representing time spent by resources on tasks/projects
    /// </summary>
    public class TimeLog : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        #region Properties
        
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// ID of the user who logged time
        /// </summary>
        public Guid UserId { get; private set; }
        
        /// <summary>
        /// ID of the project time was logged against
        /// </summary>
        public Guid ProjectId { get; private set; }
        
        /// <summary>
        /// Optional ID of the task time was logged against
        /// </summary>
        public Guid? TaskId { get; private set; }
        
        /// <summary>
        /// Details of the time log
        /// </summary>
        public TimeLogDetails Details { get; private set; }
        
        #endregion
        
        #region Constructors
        
        protected TimeLog() { }
        
        /// <summary>
        /// Creates a new instance of TimeLog aggregate
        /// </summary>
        public static TimeLog Create(
            Guid id,
            Guid userId,
            Guid projectId,
            double hours,
            DateTime logDate,
            Guid? taskId = null,
            string notes = null,
            Guid? resourceInstanceId = null,
            Guid? tenantId = null)
        {
            return new TimeLog(
                id,
                userId,
                projectId,
                hours,
                logDate,
                taskId,
                notes,
                resourceInstanceId,
                tenantId);
        }
        
        private TimeLog(
            Guid id,
            Guid userId,
            Guid projectId,
            double hours,
            DateTime logDate,
            Guid? taskId = null,
            string notes = null,
            Guid? resourceInstanceId = null,
            Guid? tenantId = null) : base(id)
        {
            UserId = userId;
            ProjectId = projectId;
            TaskId = taskId;
            Details = new TimeLogDetails(hours, logDate, notes, resourceInstanceId);
            TenantId = tenantId;
            
            AddDistributedEvent(new TimeLoggedEto
            {
                Id = Id,
                TenantId = TenantId,
                UserId = UserId,
                ProjectId = ProjectId,
                TaskId = TaskId,
                Hours = Details.Hours,
                LogDate = Details.LogDate
            });
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Updates the time log details
        /// </summary>
        public void UpdateDetails(
            double hours,
            DateTime logDate,
            string notes = null,
            Guid? resourceInstanceId = null)
        {
            Details = new TimeLogDetails(hours, logDate, notes, resourceInstanceId);
        }
        
        #endregion
    }
}
