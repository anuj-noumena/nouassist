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
    public class EfCoreElicitationArtifactRepository 
        : EfCoreRepository<IElicitationDbContext, ElicitationArtifact, Guid>, IElicitationArtifactRepository
    {
        public EfCoreElicitationArtifactRepository(
            IDbContextProvider<IElicitationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ElicitationArtifact>> GetByTypeAsync(ArtifactType type)
        {
            var query = await GetQueryableAsync();
            return await query.Where(a => a.Type == type).ToListAsync();
        }

        public async Task<List<ElicitationArtifact>> GetByStatusAsync(ArtifactStatus status)
        {
            var query = await GetQueryableAsync();
            return await query.Where(a => a.Status == status).ToListAsync();
        }

        public async Task<List<ElicitationArtifact>> GetBySessionAsync(Guid sessionId)
        {
            var query = await GetQueryableAsync();
            return await query.Where(a => a.SessionId == sessionId).ToListAsync();
        }

        public async Task<List<ElicitationArtifact>> GetByStakeholderAsync(Guid stakeholderId)
        {
            var query = await GetQueryableAsync();
            return await query.Where(a => a.CreatedByStakeholderId == stakeholderId).ToListAsync();
        }

        public async Task<List<ElicitationArtifact>> GetApprovedArtifactsAsync()
        {
            var query = await GetQueryableAsync();
            return await query.Where(a => a.Status == ArtifactStatus.Approved).ToListAsync();
        }

        public async Task<int> GetCountBySessionAndTypeAsync(Guid sessionId, ArtifactType type)
        {
            var query = await GetQueryableAsync();
            return await query.CountAsync(a => a.SessionId == sessionId && a.Type == type);
        }
    }
}
