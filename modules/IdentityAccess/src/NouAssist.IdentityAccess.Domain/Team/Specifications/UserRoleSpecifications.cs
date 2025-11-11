using System;
using System.Linq.Expressions;
using NouAssist.IdentityAccess.Team.Entities;
using Volo.Abp.Specifications;

namespace NouAssist.IdentityAccess.Team.Specifications
{
    public class ActiveUserRolesSpecification : Specification<UserRole>
    {
        public override Expression<Func<UserRole, bool>> ToExpression() =>
            ur => ur.IsActive && (!ur.ExpiresAt.HasValue || ur.ExpiresAt.Value > DateTime.UtcNow);
    }

    public class UserRolesByScopeSpecification : Specification<UserRole>
    {
        private readonly string _scope;
        private readonly Guid _scopeId;

        public UserRolesByScopeSpecification(string scope, Guid scopeId)
        {
            _scope = scope;
            _scopeId = scopeId;
        }

        public override Expression<Func<UserRole, bool>> ToExpression() =>
            ur => ur.Scope == _scope && 
                  ur.ScopeId.HasValue && 
                  ur.ScopeId.Value == _scopeId &&
                  ur.IsActive;
    }

    public class ExpiringUserRolesSpecification : Specification<UserRole>
    {
        private readonly DateTime _expiryThreshold;

        public ExpiringUserRolesSpecification(int daysAhead = 7)
        {
            _expiryThreshold = DateTime.UtcNow.AddDays(daysAhead);
        }

        public override Expression<Func<UserRole, bool>> ToExpression() =>
            ur => ur.IsActive && 
                  ur.ExpiresAt.HasValue && 
                  ur.ExpiresAt.Value <= _expiryThreshold &&
                  ur.ExpiresAt.Value > DateTime.UtcNow;
    }
}
