using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing an audit trail entry
    /// </summary>
    public class AuditTrailEntry : ValueObject
    {
        public Guid EntryId { get; private set; }
        public string Action { get; private set; }
        public string Description { get; private set; }
        public DateTime Timestamp { get; private set; }
        public Guid UserId { get; private set; }
        public string UserName { get; private set; }

        protected AuditTrailEntry() { }

        public AuditTrailEntry(
            Guid entryId,
            string action, 
            string description, 
            DateTime timestamp, 
            Guid userId, 
            string userName)
        {
            EntryId = entryId;
            Action = Check.NotNullOrWhiteSpace(action, nameof(action));
            Description = description;
            Timestamp = timestamp;
            UserId = userId;
            UserName = Check.NotNullOrWhiteSpace(userName, nameof(userName));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return EntryId;
            yield return Action;
            yield return Description ?? string.Empty;
            yield return Timestamp;
            yield return UserId;
            yield return UserName;
        }
    }
}
