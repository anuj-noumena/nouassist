using System;
using System.Collections.Generic;
using NouAssist.Meetings.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.Meetings.ValueObjects
{
    public class MeetingAttendee : ValueObject
    {
        public Guid UserId { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
        public AttendeeRole Role { get; private set; }
        public AttendeeStatus Status { get; private set; }
        public bool IsRequired { get; private set; }
        public bool IsOrganizer { get; private set; }

        private MeetingAttendee() { /* ORM */ }

        public MeetingAttendee(
            Guid userId,
            string email,
            string name,
            AttendeeRole role,
            AttendeeStatus status,
            bool isRequired,
            bool isOrganizer
        )
        {
            UserId = userId;
            Email = email;
            Name = name;
            Role = role;
            Status = status;
            IsRequired = isRequired;
            IsOrganizer = isOrganizer;
        }

        public bool HasResponded => Status != AttendeeStatus.Pending;
        public bool HasAccepted => Status == AttendeeStatus.Accepted;
        public bool HasDeclined => Status == AttendeeStatus.Declined;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserId;
            yield return Role;
            yield return Status;
            yield return IsRequired;
        }
    }
}
