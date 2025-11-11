using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IdentityAccess.Team.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.IdentityAccess.Team.Data
{
    public interface IRoleRepository : IRepository<Role, Guid>
    {
        Task<List<Role>> GetActiveRolesAsync();
        Task<List<Role>> GetSystemRolesAsync();
        Task<List<Role>> GetUserRolesAsync(Guid userId);
        Task<Role> GetByNameAsync(string name);
        Task<List<Role>> GetListAsync(ISpecification<Role> spec);
    }
}
