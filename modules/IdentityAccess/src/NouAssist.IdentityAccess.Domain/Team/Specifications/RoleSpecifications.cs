using System;
using System.Linq.Expressions;
using NouAssist.IdentityAccess.Team.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.IdentityAccess.Team.Specifications
{
    public class ActiveRolesSpecification : Specification<Role>
    {
        public override Expression<Func<Role, bool>> ToExpression() =>
            r => r.IsActive;
    }

    public class CustomRolesSpecification : Specification<Role>
    {
        public override Expression<Func<Role, bool>> ToExpression() =>
            r => !r.IsSystem;
    }

    public class SystemRolesSpecification : Specification<Role>
    {
        public override Expression<Func<Role, bool>> ToExpression() =>
            r => r.IsSystem;
    }

    public class RolesByOrganizationSpecification : Specification<Role>
    {
        private readonly Guid _organizationId;

        public RolesByOrganizationSpecification(Guid organizationId)
        {
            _organizationId = organizationId;
        }

        public override Expression<Func<Role, bool>> ToExpression() =>
            r => r.OrganizationId.HasValue && r.OrganizationId.Value == _organizationId;
    }
}
