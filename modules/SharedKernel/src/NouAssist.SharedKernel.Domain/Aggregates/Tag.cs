using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.SharedKernel.Aggregates
{
    public class Tag : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string? Color { get; set; }
        public Guid? CategoryId { get; set; }
        public int UsageCount { get; private set; }

        protected Tag() { }

        public Tag(Guid id, Guid ownerId, string title, Guid? tenantId = null) : base(id)
        {
            OwnerId = ownerId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 50);
            TenantId = tenantId;
            UsageCount = 0;
        }

        public void Rename(string newTitle)
        {
            Title = Check.NotNullOrWhiteSpace(newTitle, nameof(newTitle), maxLength: 50);
        }

        public void IncrementUsage()
        {
            UsageCount++;
        }

        public void DecrementUsage()
        {
            if (UsageCount > 0)
                UsageCount--;
        }

        public void AssignToCategory(Guid categoryId)
        {
            CategoryId = categoryId;
        }

        public bool IsPopular => UsageCount > 10;
    }

    public class TagCategory : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Name { get; private set; }
        public string? Color { get; set; }

        protected TagCategory() { }

        public TagCategory(Guid id, Guid ownerId, string name, Guid? tenantId = null) : base(id)
        {
            OwnerId = ownerId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 50);
            TenantId = tenantId;
        }

        public void Rename(string newName)
        {
            Name = Check.NotNullOrWhiteSpace(newName, nameof(newName), maxLength: 50);
        }
    }
}
