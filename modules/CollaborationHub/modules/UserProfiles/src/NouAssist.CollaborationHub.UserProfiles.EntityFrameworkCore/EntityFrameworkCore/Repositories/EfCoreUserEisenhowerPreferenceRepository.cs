using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.CollaborationHub.UserProfiles.Aggregates;
using NouAssist.CollaborationHub.UserProfiles.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.UserProfiles.EntityFrameworkCore.Repositories
{
    public class EfCoreUserEisenhowerPreferenceRepository 
        : EfCoreRepository<IUsersDbContext, UserEisenhowerPreference, Guid>, 
          IUserEisenhowerPreferenceRepository
    {
        public EfCoreUserEisenhowerPreferenceRepository(
            IDbContextProvider<IUsersDbContext> dbContextProvider)
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
