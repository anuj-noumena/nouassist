using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.SharedKernel.Aggregates
{
    public class Context : FullAuditedAggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; internal set; }
        public string? Notes { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? Location { get; set; }
        public List<string> Tools { get; private set; }
        public int? EnergyLevel { get; set; }
        public bool IsActive { get; set; }
        public int UsageCount { get; private set; }
        public DateTime? LastUsedAt { get; private set; }
        public DateTime? ArchivedAt { get; private set; }
        public bool IsDeleted { get; set; }

        protected Context() 
        {
            Tools = new List<string>();
        }

        public Context(Guid id, Guid ownerId, string title, Guid? tenantId = null) : base(id)
        {
            OwnerId = ownerId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 255);
            TenantId = tenantId;
            IsActive = true;
            UsageCount = 0;
            Tools = new List<string>();
        }

        public void UpdateTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 255);
        }

        public void AddTool(string tool)
        {
            if (!Tools.Contains(tool))
            {
                Tools.Add(tool);
            }
        }

        public void RemoveTool(string tool)
        {
            Tools.Remove(tool);
        }

        public void SetLocation(string location)
        {
            Location = location;
        }

        public void SetEnergyLevel(int energyLevel)
        {
            if (energyLevel < 1 || energyLevel > 10)
            {
                throw new BusinessException("Core:InvalidEnergyLevel",
                    "Energy level must be between 1 and 10.");
            }
            EnergyLevel = energyLevel;
        }

        public void RecordUsage()
        {
            UsageCount++;
            LastUsedAt = DateTime.UtcNow;
        }

        public void Archive()
        {
            IsActive = false;
            ArchivedAt = DateTime.UtcNow;
        }

        public void Reactivate()
        {
            IsActive = true;
            ArchivedAt = null;
        }

        public bool IsArchived => ArchivedAt.HasValue;
    }
}
