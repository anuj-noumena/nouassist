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
    public class EfCoreElicitationSessionRepository 
        : EfCoreRepository<IElicitationDbContext, ElicitationSession, Guid>, IElicitationSessionRepository
    {
        public EfCoreElicitationSessionRepository(
            IDbContextProvider<IElicitationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ElicitationSession>> GetSessionsInTimeRangeAsync(DateTime startTime, DateTime endTime)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(s => s.Schedule.StartTime >= startTime && s.Schedule.EndTime <= endTime)
                .ToListAsync();
        }

        public async Task<List<ElicitationSession>> GetSessionsByTypeAsync(SessionType type)
        {
            var query = await GetQueryableAsync();
            return await query.Where(s => s.Type == type).ToListAsync();
        }

        public async Task<List<ElicitationSession>> GetSessionsByStatusAsync(SessionStatus status)
        {
            var query = await GetQueryableAsync();
            return await query.Where(s => s.Status == status).ToListAsync();
        }

        public async Task<List<ElicitationSession>> GetSessionsByOrganizerAsync(Guid organizerId)
        {
            var query = await GetQueryableAsync();
            return await query.Where(s => s.OrganizerId == organizerId).ToListAsync();
        }

        public async Task<List<ElicitationSession>> GetUpcomingSessionsAsync(int days = 7)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(days);
            var query = await GetQueryableAsync();
            return await query
                .Where(s => s.Schedule.StartTime >= DateTime.UtcNow && 
                           s.Schedule.StartTime <= cutoffDate &&
                           s.Status != SessionStatus.Cancelled)
                .OrderBy(s => s.Schedule.StartTime)
                .ToListAsync();
        }

        public async Task<ElicitationSession> GetWithParticipantsAsync(Guid id)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(s => s.Participants)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<ElicitationSession> GetWithArtifactsAsync(Guid id)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(s => s.GeneratedArtifacts)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // Override to include related entities when needed
        public override async Task<ElicitationSession> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query
                    .Include(s => s.Participants)
                    .Include(s => s.GeneratedArtifacts)
                    .Include(s => s.SurveyDefinition)
                        .ThenInclude(sd => sd.Responses)
                    .Include(s => s.InterviewRecord);
            }

            return await query.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
    }
}
