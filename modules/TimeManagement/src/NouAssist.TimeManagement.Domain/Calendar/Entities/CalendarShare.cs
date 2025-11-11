using System;
using NouAssist.Calendar.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Calendar.Entities
{
    public class CalendarShare : Entity<Guid>
    {
        public Guid CalendarId { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid SharedWithUserId { get; private set; }
        public CalendarPermissionLevel PermissionLevel { get; private set; }
        public DateTime? AcceptedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string RevokedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected CalendarShare() { }

        public CalendarShare(
            Guid id,
            Guid calendarId,
            Guid ownerId,
            Guid sharedWithUserId,
            CalendarPermissionLevel permissionLevel)
            : base(id)
        {
            if (ownerId == sharedWithUserId)
                throw new BusinessException("Calendar:CannotShareWithSelf");

            CalendarId = calendarId;
            OwnerId = ownerId;
            SharedWithUserId = sharedWithUserId;
            PermissionLevel = permissionLevel;
            CreatedAt = DateTime.UtcNow;
        }

        public void Accept()
        {
            if (AcceptedAt.HasValue)
                return;

            AcceptedAt = DateTime.UtcNow;
        }

        public void Revoke(string revokedBy)
        {
            if (RevokedAt.HasValue)
                return;

            RevokedAt = DateTime.UtcNow;
            RevokedBy = Check.NotNullOrWhiteSpace(revokedBy, nameof(revokedBy));
        }

        public void UpgradeToWrite()
        {
            PermissionLevel = CalendarPermissionLevel.Write;
        }

        public void DowngradeToRead()
        {
            PermissionLevel = CalendarPermissionLevel.Read;
        }

        public bool IsAccepted => AcceptedAt.HasValue;
        public bool IsRevoked => RevokedAt.HasValue;
        public bool IsActive => IsAccepted && !IsRevoked;
        public bool CanWrite => PermissionLevel == CalendarPermissionLevel.Write;
    }
}
