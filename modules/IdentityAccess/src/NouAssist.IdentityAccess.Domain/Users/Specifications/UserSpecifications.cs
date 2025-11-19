using System;
using System.Linq.Expressions;
using NouAssist.IdentityAccess.Users.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.IdentityAccess.Users.Specifications
{
    public class ActiveUsersSpecification : Specification<User>
    {
        public override Expression<Func<User, bool>> ToExpression() =>
            u => u.IsActive;
    }

    public class UserByEmailSpecification : Specification<User>
    {
        private readonly string _email;

        public UserByEmailSpecification(string email)
        {
            _email = email;
        }

        public override Expression<Func<User, bool>> ToExpression() =>
            u => u.Email == _email;
    }

    public class UsersByNameSpecification : Specification<User>
    {
        private readonly string _searchTerm;

        public UsersByNameSpecification(string searchTerm)
        {
            _searchTerm = searchTerm?.ToLower() ?? string.Empty;
        }

        public override Expression<Func<User, bool>> ToExpression() =>
            u => (u.FirstName != null && u.FirstName.ToLower().Contains(_searchTerm)) ||
                 (u.LastName != null && u.LastName.ToLower().Contains(_searchTerm));
    }

    public class InactiveUsersSpecification : Specification<User>
    {
        public override Expression<Func<User, bool>> ToExpression() =>
            u => !u.IsActive;
    }

    public class RecentlyLoggedInUsersSpecification : Specification<User>
    {
        private readonly int _daysAgo;

        public RecentlyLoggedInUsersSpecification(int daysAgo = 7)
        {
            _daysAgo = daysAgo;
        }

        public override Expression<Func<User, bool>> ToExpression()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-_daysAgo);
            return u => u.LastLoginAt != null && u.LastLoginAt >= cutoffDate;
        }
    }
}
