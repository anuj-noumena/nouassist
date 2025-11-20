using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using NouAssist.RequirementsManagement.Elicitation.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore.Repositories
{
    public class EfCoreStakeholderProfileRepository 
        : EfCoreRepository<IElicitationDbContext, StakeholderProfile, Guid>, IStakeholderProfileRepository
    {
        public EfCoreStakeholderProfileRepository(
            IDbContextProvider<IElicitationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<StakeholderProfile> FindByEmailAsync(string email)
        {
            var query = await GetQueryableAsync();
            return await query.FirstOrDefaultAsync(s => s.ContactInfo.Email == email);
        }

        public async Task<List<StakeholderProfile>> GetActiveStakeholdersAsync()
        {
            var query = await GetQueryableAsync();
            return await query.Where(s => s.IsActive).ToListAsync();
        }

        public async Task<List<StakeholderProfile>> GetByRoleAsync(StakeholderRole role)
        {
            var query = await GetQueryableAsync();
            return await query.Where(s => s.IsActive && s.Role == role).ToListAsync();
        }

        public async Task<List<StakeholderProfile>> GetByOrganizationAsync(string organization)
        {
            var query = await GetQueryableAsync();
            return await query.Where(s => s.IsActive && s.Organization == organization).ToListAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null)
        {
            var query = await GetQueryableAsync();
            var existingStakeholder = await query
                .Where(s => s.ContactInfo.Email == email)
                .FirstOrDefaultAsync();

            if (existingStakeholder == null)
                return true;

            return excludeId.HasValue && existingStakeholder.Id == excludeId.Value;
        }
    }
}
