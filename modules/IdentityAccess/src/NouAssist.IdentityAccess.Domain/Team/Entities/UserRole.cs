using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IdentityAccess.Team.Entities
{
    public class UserRole : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid RoleId { get; private set; }
        public Guid? AssignedBy { get; private set; }
        public DateTime AssignedAt { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public string Scope { get; private set; }
        public Guid? ScopeId { get; private set; }
        public bool IsActive { get; private set; }
        public Guid? TenantId { get; private set; }

        protected UserRole() { /* ORM */ }

        public UserRole(
            Guid id,
            Guid userId,
            Guid roleId,
            Guid? tenantId,
            Guid? assignedBy = null,
            DateTime? expiresAt = null,
            string scope = null,
            Guid? scopeId = null
        ) : base(id)
        {
            UserId = userId;
            RoleId = roleId;
            TenantId = tenantId;
            AssignedBy = assignedBy;
            AssignedAt = DateTime.UtcNow;
            ExpiresAt = expiresAt;
            Scope = scope;
            ScopeId = scopeId;
            IsActive = true;
        }

        public bool HasExpiry => ExpiresAt.HasValue;
        
        public bool IsExpired =>
            ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;

        public bool HasScope => !string.IsNullOrEmpty(Scope) && ScopeId.HasValue;

        public void ExtendExpiry(DateTime newExpiry)
        {
            if (newExpiry < DateTime.UtcNow)
            {
                throw new BusinessException("Team:ExpiryDateInPast");
            }
            ExpiresAt = newExpiry;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public bool IsValidAt(DateTime dateTime)
        {
            if (!IsActive)
            {
                return false;
            }

            if (dateTime < AssignedAt)
            {
                return false;
            }

            if (ExpiresAt.HasValue && dateTime > ExpiresAt.Value)
            {
                return false;
            }

            return true;
        }
    }
}
