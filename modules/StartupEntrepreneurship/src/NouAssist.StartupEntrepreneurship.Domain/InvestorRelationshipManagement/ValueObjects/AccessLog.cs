using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects
{
    /// <summary>
    /// Value object representing access log for documents
    /// </summary>
    public class AccessLog : ValueObject
    {
        public Guid UserId { get; private set; }
        public DateTime AccessedAt { get; private set; }
        public string Action { get; private set; }
        public string IpAddress { get; private set; }
        public string UserAgent { get; private set; }

        private AccessLog() { }

        public AccessLog(
            Guid userId,
            DateTime accessedAt,
            string action,
            string ipAddress = null,
            string userAgent = null)
        {
            UserId = userId;
            AccessedAt = accessedAt;
            Action = Check.NotNullOrWhiteSpace(action, nameof(action));
            IpAddress = ipAddress;
            UserAgent = userAgent;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserId;
            yield return AccessedAt;
            yield return Action;
            yield return IpAddress;
            yield return UserAgent;
        }
    }
}
