using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.SharedKernel.Aggregates
{
    public class Area : FullAuditedAggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
        public List<string> StandardsToMaintain { get; private set; }
        public List<string> KeyResultAreas { get; private set; }
        public string? Purpose { get; set; }
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastReviewedAt { get; set; }
        public DateTime? ArchivedAt { get; private set; }
        public bool IsDeleted { get; set; }

        protected Area() 
        {
            StandardsToMaintain = new List<string>();
            KeyResultAreas = new List<string>();
        }

        public Area(Guid id, Guid ownerId, string title, Guid? tenantId = null) : base(id)
        {
            OwnerId = ownerId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 255);
            TenantId = tenantId;
            IsActive = true;
            StandardsToMaintain = new List<string>();
            KeyResultAreas = new List<string>();
        }

        public void UpdateTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 255);
        }

        public void SetParent(Guid parentId)
        {
            if (parentId == Id)
            {
                throw new BusinessException("Core:CannotSetSelfAsParent", 
                    "An area cannot be its own parent.");
            }
            ParentId = parentId;
        }

        public void AddStandard(string standard)
        {
            if (!StandardsToMaintain.Contains(standard))
            {
                StandardsToMaintain.Add(standard);
            }
        }

        public void RemoveStandard(string standard)
        {
            StandardsToMaintain.Remove(standard);
        }

        public void AddKeyResultArea(string kra)
        {
            if (!KeyResultAreas.Contains(kra))
            {
                KeyResultAreas.Add(kra);
            }
        }

        public void MarkAsReviewed()
        {
            LastReviewedAt = DateTime.UtcNow;
        }

        public void Archive()
        {
            IsActive = false;
            ArchivedAt = DateTime.UtcNow;
        }

        public void Unarchive()
        {
            IsActive = true;
            ArchivedAt = null;
        }

        public bool HasParent => ParentId.HasValue;
        public bool IsRootArea => !HasParent;
        public bool IsArchived => ArchivedAt.HasValue;
    }
}
