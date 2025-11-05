using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Resources.Aggregates
{
    public class BookCollection : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public const int MaxNameLength = 200;

        public Guid OwnerId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid? CoverImageResourceId { get; private set; }
        public List<string> Tags { get; private set; }
        public bool IsPrivate { get; private set; }
        public int? SortOrder { get; private set; }
        public Guid? TenantId { get; private set; }

        protected BookCollection() { /* ORM */ }

        public BookCollection(
            Guid id,
            Guid ownerId,
            string name,
            Guid? tenantId,
            string description = null) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), MaxNameLength);
            TenantId = tenantId;
            Description = description;
            IsPrivate = false;
            Tags = new List<string>();
        }

        public void UpdateName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), MaxNameLength);
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetCoverImage(Guid coverImageResourceId)
        {
            CoverImageResourceId = coverImageResourceId;
        }

        public void MakePrivate()
        {
            IsPrivate = true;
        }

        public void MakePublic()
        {
            IsPrivate = false;
        }

        public void SetSortOrder(int sortOrder)
        {
            SortOrder = sortOrder;
        }

        public void AddTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return;
            if (Tags.Contains(tag)) return;

            Tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
        }

        public bool HasCoverImage => CoverImageResourceId.HasValue;
        public bool HasTags => Tags != null && Tags.Count > 0;
    }
}
