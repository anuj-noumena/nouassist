using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ProjectExecution.Entities
{
    /// <summary>
    /// Represents a member assigned to a project
    /// </summary>
    public class ProjectMember : Entity
    {
        /// <summary>
        /// ID of the project this member belongs to
        /// </summary>
        public Guid ProjectId { get; private set; }
        
        /// <summary>
        /// ID of the user
        /// </summary>
        public Guid UserId { get; private set; }
        
        /// <summary>
        /// Role of the member in the project
        /// </summary>
        public string Role { get; private set; }
        
        /// <summary>
        /// Date when the member joined the project
        /// </summary>
        public DateTime JoinDate { get; private set; }
        
        /// <summary>
        /// Date when the member left the project, if applicable
        /// </summary>
        public DateTime? LeaveDate { get; private set; }
        
        /// <summary>
        /// Indicates if the member is active in the project
        /// </summary>
        public bool IsActive => !LeaveDate.HasValue;
        
        protected ProjectMember() { }
        
        internal ProjectMember(
            Guid projectId,
            Guid userId,
            string role,
            DateTime joinDate)
        {
            ProjectId = projectId;
            UserId = userId;
            SetRole(role);
            JoinDate = joinDate;
        }
        
        internal void SetRole(string role)
        {
            Role = Check.NotNullOrWhiteSpace(
                role,
                nameof(role),
                maxLength: 100
            );
        }
        
        internal void Leave(DateTime leaveDate)
        {
            if (leaveDate < JoinDate)
            {
                throw new BusinessException("Leave date cannot be earlier than join date");
            }
            
            LeaveDate = leaveDate;
        }
        
        internal void Rejoin(DateTime rejoinDate)
        {
            if (!LeaveDate.HasValue)
            {
                return;
            }
            
            if (rejoinDate < LeaveDate.Value)
            {
                throw new BusinessException("Rejoin date cannot be earlier than leave date");
            }
            
            LeaveDate = null;
        }
        
        public override object[] GetKeys()
        {
            return new object[] { ProjectId, UserId };
        }
    }
}
