using System;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace NouAssist.WorkManagement.ProjectManagement.Entities
{
    /// <summary>
    /// ProjectDependency - Relationship between projects
    /// OPTIMIZED: Better validation, cleaner state
    /// </summary>
    public class ProjectDependency : FullAuditedEntity<Guid>, IMultiTenant, IHasConcurrencyStamp, IHasExtraProperties
    {
        public Guid ProjectId { get; private set; }
        public Guid DependsOnProjectId { get; private set; }
        public ProjectDependencyType DependencyType { get; private set; }
        public Guid? TenantId { get; private set; }
        
        // === TIMING ===
        public int LagDays { get; internal set; }
        
        // === IMPACT ===
        public bool IsBlocking { get; set; }
        public DependencyStrength Strength { get; private set; } // Hard, Soft, Informational
        
        // === STATUS ===
        public ProjectDependencyStatus Status { get; private set; }
        
        // === METADATA ===
        public string Description { get; set; }
        public string Notes { get; set; }
        public DateTime? ExpectedResolutionDate { get; set; }
        
        // === RESOLUTION ===
        public DateTime? ResolvedAt { get; private set; }
        public Guid? ResolvedBy { get; private set; }
        public string ResolutionNote { get; private set; }
        
        protected ProjectDependency() { }
        
        public ProjectDependency(
            Guid id,
            Guid projectId,
            Guid dependsOnProjectId,
            ProjectDependencyType dependencyType,
            Guid? tenantId,
            int lagDays = 0,
            bool isBlocking = true,
            DependencyStrength strength = DependencyStrength.Hard,
            string description = null) : base(id)
        {
            if (projectId == dependsOnProjectId)
            {
                throw new BusinessException("Projects:SelfDependency",
                    "Project cannot depend on itself.");
            }
            
            if (lagDays < 0)
            {
                throw new BusinessException("Projects:NegativeLag");
            }
            
            ProjectId = projectId;
            DependsOnProjectId = dependsOnProjectId;
            DependencyType = dependencyType;
            TenantId = tenantId;
            LagDays = lagDays;
            IsBlocking = isBlocking;
            Strength = strength;
            Description = description;
            Status = ProjectDependencyStatus.Active;
        }
        
        public void Update(string description, string notes, int lagDays, bool isBlocking, DateTime? expectedResolutionDate)
        {
            Description = description;
            Notes = notes;
        
            if (lagDays < 0)
            {
                throw new BusinessException("Projects:NegativeLag");
            }
        
            LagDays = lagDays;
            IsBlocking = isBlocking;
            ExpectedResolutionDate = expectedResolutionDate;
        }

        
        public void Resolve(Guid resolvedBy, string note = null)
        {
            if (Status == ProjectDependencyStatus.Resolved)
            {
                return;
            }
            
            Status = ProjectDependencyStatus.Resolved;
            ResolvedAt = DateTime.UtcNow;
            ResolvedBy = resolvedBy;
            ResolutionNote = note;
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
        
        // Computed
        public bool HasLag => LagDays > 0;
        public bool IsActive => Status == ProjectDependencyStatus.Active;
        public bool IsResolved => Status == ProjectDependencyStatus.Resolved;
        public bool IsOverdue => ExpectedResolutionDate.HasValue &&
                                 DateTime.UtcNow > ExpectedResolutionDate.Value &&
                                 !IsResolved;
        public TimeSpan LagDuration => TimeSpan.FromDays(LagDays);
        public string ConcurrencyStamp { get; set; }
        
        public ExtraPropertyDictionary ExtraProperties { get; }
    }
}