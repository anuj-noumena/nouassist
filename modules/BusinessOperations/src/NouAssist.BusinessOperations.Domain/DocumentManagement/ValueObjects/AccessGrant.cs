using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.DocumentManagement.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.DocumentManagement.ValueObjects
{
    /// <summary>
    /// Represents an access grant for a document to a user or role
    /// </summary>
    public class AccessGrant : ValueObject
    {
        public Guid PrincipalId { get; private set; }
        public bool IsRole { get; private set; }
        public AccessPermissionLevel PermissionLevel { get; private set; }
        public Guid GrantedBy { get; private set; }
        public DateTime GrantedAt { get; private set; }

        private AccessGrant() { }

        public AccessGrant(
            Guid principalId,
            bool isRole,
            AccessPermissionLevel permissionLevel,
            Guid grantedBy,
            DateTime grantedAt)
        {
            if (principalId == Guid.Empty)
            {
                throw new ArgumentException("Principal ID cannot be empty", nameof(principalId));
            }

            if (grantedBy == Guid.Empty)
            {
                throw new ArgumentException("Granter ID cannot be empty", nameof(grantedBy));
            }

            PrincipalId = principalId;
            IsRole = isRole;
            PermissionLevel = permissionLevel;
            GrantedBy = grantedBy;
            GrantedAt = grantedAt;
        }

        public AccessGrant UpdatePermissionLevel(
            AccessPermissionLevel newPermissionLevel,
            Guid updatedBy,
            DateTime updatedAt)
        {
            return new AccessGrant(
                PrincipalId,
                IsRole,
                newPermissionLevel,
                updatedBy,
                updatedAt
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PrincipalId;
            yield return IsRole;
            yield return PermissionLevel;
            yield return GrantedBy;
            yield return GrantedAt;
        }
    }
}
