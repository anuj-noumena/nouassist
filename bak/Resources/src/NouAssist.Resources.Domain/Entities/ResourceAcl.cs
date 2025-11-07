using System;
using NouAssist.Resources.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Resources.Entities
{
    public class ResourceAcl : Entity
    {
        public Guid ResourceId { get; private set; }
        public Guid GrantedToUserId { get; private set; }
        public ResourcePermission Permission { get; private set; }
        public Guid? GrantedBy { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected ResourceAcl() { /* ORM */ }

        public ResourceAcl(
            Guid resourceId,
            Guid grantedToUserId,
            ResourcePermission permission,
            Guid? grantedBy = null,
            DateTime? expiresAt = null)
        {
            ResourceId = Check.NotNull(resourceId, nameof(resourceId));
            GrantedToUserId = Check.NotNull(grantedToUserId, nameof(grantedToUserId));
            Permission = permission;
            GrantedBy = grantedBy;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpgradePermission(ResourcePermission newPermission)
        {
            Permission = newPermission;
        }

        public void ExtendExpiry(DateTime newExpiry)
        {
            ExpiresAt = newExpiry;
        }

        public bool HasExpiry => ExpiresAt.HasValue;
        public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
        public bool IsActive => !IsExpired;
        public bool CanRead => Permission == ResourcePermission.Read || Permission == ResourcePermission.Write || Permission == ResourcePermission.Admin;
        public bool CanWrite => Permission == ResourcePermission.Write || Permission == ResourcePermission.Admin;
        public bool CanAdmin => Permission == ResourcePermission.Admin;

        public override object[] GetKeys() => new object[] { ResourceId, GrantedToUserId };
    }
}
