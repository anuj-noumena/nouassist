using System;
using System.Collections.Generic;
using NouAssist.TimeManagement.Calendar.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.TimeManagement.Calendar.Aggregates
{
    public class Calendar : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Timezone { get; set; }
        public bool IsActive { get; private set; }
        public bool IsDefault { get; private set; }
        public bool IsShared { get; private set; }

        private readonly List<Entities.CalendarShare> _shares = new();
        public IReadOnlyCollection<Entities.CalendarShare> Shares => _shares.AsReadOnly();

        protected Calendar() { }

        public Calendar(
            Guid id,
            Guid ownerId,
            string title,
            Guid? tenantId = null,
            string description = null,
            string color = null,
            string timezone = "UTC",
            bool isDefault = false)
            : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            SetTitle(title);
            TenantId = tenantId;
            Description = description;
            Color = color;
            Timezone = timezone ?? "UTC";
            IsDefault = isDefault;
            IsActive = true;
            IsShared = false;
        }

        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 200);
        }

        public void MakeDefault()
        {
            IsDefault = true;
        }

        public void RemoveDefault()
        {
            IsDefault = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void ShareWith(Guid sharedWithUserId, CalendarPermissionLevel permissionLevel)
        {
            if (sharedWithUserId == OwnerId)
            {
                throw new BusinessException("Calendar:CannotShareWithSelf");
            }

            var existingShare = _shares.Find(s => s.SharedWithUserId == sharedWithUserId && !s.IsRevoked);
            if (existingShare != null)
            {
                return; // Already shared
            }

            var share = new Entities.CalendarShare(
                Guid.NewGuid(),
                Id,
                OwnerId,
                sharedWithUserId,
                permissionLevel);

            _shares.Add(share);
            IsShared = true;
        }

        public void RevokeShare(Guid sharedWithUserId, string revokedBy)
        {
            var share = _shares.Find(s => s.SharedWithUserId == sharedWithUserId && !s.IsRevoked);
            share?.Revoke(revokedBy);

            if (_shares.TrueForAll(s => s.IsRevoked))
            {
                IsShared = false;
            }
        }
    }
}
