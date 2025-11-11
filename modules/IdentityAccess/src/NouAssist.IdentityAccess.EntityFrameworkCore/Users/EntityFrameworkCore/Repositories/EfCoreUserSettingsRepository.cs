using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Users.Aggregates;
using NouAssist.Users.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Users.EntityFrameworkCore.Repositories
{
    public class EfCoreUserSettingsRepository 
        : EfCoreRepository<IUsersDbContext, UserSettings, Guid>, IUserSettingsRepository
    {
        public EfCoreUserSettingsRepository(
            IDbContextProvider<IUsersDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<UserSettings?> FindByUserIdAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }
    }
}
