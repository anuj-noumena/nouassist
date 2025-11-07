using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Team.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Team.Data
{
    public interface IUserRoleRepository : IRepository<UserRole, Guid>
    {
        Task<List<UserRole>> GetUserRolesAsync(Guid userId);
        Task<List<UserRole>> GetRoleAssignmentsAsync(Guid roleId);
        Task<List<UserRole>> GetActiveUserRolesAsync(Guid userId);
        Task<bool> UserHasRoleAsync(Guid userId, Guid roleId);
    }
}
