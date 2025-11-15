using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.AuditTrail.ValueObjects
{
    public class UserContext : ValueObject
    {
        public Guid UserId { get; init; }
        public string UserName { get; init; }
        public string IPAddress { get; init; }
        public string UserAgent { get; init; }
        public string SessionId { get; init; }

        protected UserContext() { }

        public UserContext(
            Guid userId, 
            string userName, 
            string ipAddress = null, 
            string userAgent = null,
            string sessionId = null)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            UserName = Check.NotNullOrWhiteSpace(userName, nameof(userName));
            IPAddress = ipAddress;
            UserAgent = userAgent;
            SessionId = sessionId;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return UserId;
            yield return UserName;
            yield return IPAddress ?? string.Empty;
            yield return UserAgent ?? string.Empty;
            yield return SessionId ?? string.Empty;
        }
    }
}
