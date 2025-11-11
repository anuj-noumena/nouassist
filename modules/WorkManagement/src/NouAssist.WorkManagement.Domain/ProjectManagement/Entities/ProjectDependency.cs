using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.ProjectManagement.Entities
{
    public class ProjectDependency : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid ProjectId { get; private set; }
        public Guid DependsOnProjectId { get; private set; }
        public ProjectDependencyType DependencyType { get; private set; }
        public Guid? TenantId { get; private set; }

        public int LagDays { get; private set; }
        public bool IsBlocking { get; set; }
        public ProjectDependencyStatus Status { get; private set; }

        public string Description { get; set; }
        public string Notes { get; set; }

        public DateTime? ExpectedResolutionDate { get; set; }
        public DateTime? ActualResolutionDate { get; private set; }
        public string ResolvedBy { get; private set; }

        public Guid? ApprovalRequestId { get; set; }
        
        private Dictionary<string, object> _metadata;
        public IReadOnlyDictionary<string, object> Metadata => 
            _metadata?.AsReadOnly();


        protected ProjectDependency()
        {
            _metadata = new Dictionary<string, object>();
        }

        public ProjectDependency(
            Guid id,
            Guid projectId,
            Guid dependsOnProjectId,
            ProjectDependencyType dependencyType,
            Guid? tenantId,
            int lagDays = 0,
            bool isBlocking = true,
            string description = null) : base(id)
        {
            if (projectId == dependsOnProjectId)
            {
                throw new BusinessException("Projects:SelfDependency", 
                    "Project cannot depend on itself.");
            }

            ProjectId = projectId;
            DependsOnProjectId = dependsOnProjectId;
            DependencyType = dependencyType;
            TenantId = tenantId;
            LagDays = lagDays;
            IsBlocking = isBlocking;
            Description = description;
            Status = ProjectDependencyStatus.Active;
        }

        public void Resolve(string resolvedBy)
        {
            if (Status == ProjectDependencyStatus.Resolved)
            {
                return;
            }

            Status = ProjectDependencyStatus.Resolved;
            ActualResolutionDate = DateTime.UtcNow;
            ResolvedBy = Check.NotNullOrWhiteSpace(resolvedBy, nameof(resolvedBy));
        }

        public void MarkAsObsolete()
        {
            Status = ProjectDependencyStatus.Obsolete;
        }

        public void Block()
        {
            Status = ProjectDependencyStatus.Blocked;
        }

        public void Reactivate()
        {
            if (Status == ProjectDependencyStatus.Resolved)
            {
                throw new BusinessException("Projects:CannotReactivateResolved");
            }

            Status = ProjectDependencyStatus.Active;
        }
        
        public void SetMetadata(string key, object value)
        {
            if (_metadata == null)
            {
                _metadata = new Dictionary<string, object>();
            }

            _metadata[key] = value;
        }

        public object GetMetadata(string key)
        {
            return _metadata?.GetValueOrDefault(key);
        }

        // Computed Properties
        public bool HasLag => LagDays > 0;
        public bool IsActive => Status == ProjectDependencyStatus.Active;
        public bool IsResolved => Status == ProjectDependencyStatus.Resolved;
        public bool IsObsolete => Status == ProjectDependencyStatus.Obsolete;
        public bool IsBlocked => Status == ProjectDependencyStatus.Blocked;

        public bool IsOverdue => ExpectedResolutionDate.HasValue && 
                                 DateTime.UtcNow > ExpectedResolutionDate.Value && 
                                 !IsResolved;

        public TimeSpan LagDuration => TimeSpan.FromDays(LagDays);

        public string DependencyTypeDescription => DependencyType switch
        {
            ProjectDependencyType.FinishToStart => "Finish-to-Start",
            ProjectDependencyType.StartToStart => "Start-to-Start",
            ProjectDependencyType.FinishToFinish => "Finish-to-Finish",
            ProjectDependencyType.StartToFinish => "Start-to-Finish",
            ProjectDependencyType.Deliverable => "Deliverable Dependency",
            ProjectDependencyType.Resource => "Resource Dependency",
            ProjectDependencyType.Approval => "Approval Dependency",
            ProjectDependencyType.Information => "Information Dependency",
            _ => "Unknown"
        };
    }
}
