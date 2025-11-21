using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IdentityAccess.Users.Aggregates;
using NouAssist.IdentityAccess.Users.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IdentityAccess.EntityFrameworkCore.Users.Repositories
{
    public class EfCoreUserEisenhowerPreferenceRepository 
        : EfCoreRepository<IdentityAccessDbContext, UserEisenhowerPreference, Guid>, 
          IUserEisenhowerPreferenceRepository
    {
        public EfCoreUserEisenhowerPreferenceRepository(
            IDbContextProvider<IdentityAccessDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<UserEisenhowerPreference?> FindByUserIdAsync(Guid userId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }
    }
}
