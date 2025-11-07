using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Team.Aggregates
{
    public class Role : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid? OrganizationId { get; private set; }
        public bool IsSystem { get; private set; }
        public bool IsActive { get; private set; }
        public int? MaxUsers { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<string> _permissions = new();
        public IReadOnlyCollection<string> Permissions => _permissions.AsReadOnly();

        protected Role() { /* ORM */ }

        public Role(
            Guid id,
            string name,
            Guid? tenantId,
            string description = null,
            bool isSystem = false
        ) : base(id)
        {
            SetName(name);
            Description = description;
            TenantId = tenantId;
            IsSystem = isSystem;
            IsActive = true;
            Metadata = new Dictionary<string, object>();
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), 100);
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void AddPermission(string permission)
        {
            if (IsSystem)
            {
                throw new BusinessException("Team:CannotModifySystemRole");
            }

            if (!_permissions.Contains(permission))
            {
                _permissions.Add(permission);
            }
        }

        public void RemovePermission(string permission)
        {
            if (IsSystem)
            {
                throw new BusinessException("Team:CannotModifySystemRole");
            }

            _permissions.Remove(permission);
        }

        public bool HasPermission(string permission)
        {
            return _permissions.Contains(permission);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            if (IsSystem)
            {
                throw new BusinessException("Team:CannotDeactivateSystemRole");
            }
            IsActive = false;
        }
    }
}
