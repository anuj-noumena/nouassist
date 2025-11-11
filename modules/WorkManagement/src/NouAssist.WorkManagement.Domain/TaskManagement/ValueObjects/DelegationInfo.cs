using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.TaskManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.TaskManagement.ValueObjects
{
    public class DelegationInfo : ValueObject
    {
        public Guid DelegatedBy { get; private set; }
        public Guid DelegatedTo { get; private set; }
        public DelegationStatus Status { get; private set; }
        public DateTime DelegatedAt { get; private set; }
        public DateTime? RespondedAt { get; private set; }
        public string Note { get; private set; }
        public string ResponseNote { get; private set; }

        private DelegationInfo() { }

        public DelegationInfo(
            Guid delegatedBy,
            Guid delegatedTo,
            string note = null)
        {
            if (delegatedBy == delegatedTo)
            {
                throw new BusinessException("Tasks:CannotDelegateToSelf");
            }

            DelegatedBy = Check.NotNull(delegatedBy, nameof(delegatedBy));
            DelegatedTo = Check.NotNull(delegatedTo, nameof(delegatedTo));
            Status = DelegationStatus.Delegated;
            DelegatedAt = DateTime.UtcNow;
            Note = note;
        }

        public DelegationInfo Accept(string responseNote = null)
        {
            if (Status != DelegationStatus.Delegated)
            {
                throw new BusinessException("Tasks:CanOnlyAcceptDelegatedTasks");
            }

            return new DelegationInfo
            {
                DelegatedBy = DelegatedBy,
                DelegatedTo = DelegatedTo,
                Status = DelegationStatus.Accepted,
                DelegatedAt = DelegatedAt,
                RespondedAt = DateTime.UtcNow,
                Note = Note,
                ResponseNote = responseNote
            };
        }

        public DelegationInfo Decline(string responseNote)
        {
            if (Status != DelegationStatus.Delegated)
            {
                throw new BusinessException("Tasks:CanOnlyDeclineDelegatedTasks");
            }

            Check.NotNullOrWhiteSpace(responseNote, nameof(responseNote));

            return new DelegationInfo
            {
                DelegatedBy = DelegatedBy,
                DelegatedTo = DelegatedTo,
                Status = DelegationStatus.Declined,
                DelegatedAt = DelegatedAt,
                RespondedAt = DateTime.UtcNow,
                Note = Note,
                ResponseNote = responseNote
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DelegatedBy;
            yield return DelegatedTo;
            yield return Status;
            yield return DelegatedAt;
            yield return RespondedAt;
        }
    }
}
