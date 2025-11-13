using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IdentityAccess.Users.Aggregates;
using NouAssist.IdentityAccess.Users.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IdentityAccess.Users.EntityFrameworkCore.Repositories
{
    public class EfCoreUserRepository 
        : EfCoreRepository<IUsersDbContext, User, Guid>, IUserRepository
    {
        public EfCoreUserRepository(
            IDbContextProvider<IUsersDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(u => u.Email == email.ToLower());
        }

        public async Task<List<User>> GetActiveUsersAsync()
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(u => u.IsActive)
                .OrderBy(u => u.CreationTime)
                .ToListAsync();
        }
    }
}
