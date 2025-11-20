using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Repositories
{
    public class EfCorePitchDeckRepository : 
        EfCoreRepository<IFundraisingPipelineDbContext, PitchDeck, Guid>, 
        IPitchDeckRepository
    {
        public EfCorePitchDeckRepository(
            IDbContextProvider<IFundraisingPipelineDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<PitchDeck> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.PitchDecks
                .Include(p => p.Versions)
                .Include(p => p.Presentations)
                .Include(p => p.Feedbacks)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<PitchDeck>> GetByFundraisingRoundAsync(
            Guid fundraisingRoundId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.PitchDecks
                .Where(p => p.FundraisingRoundId == fundraisingRoundId)
                .OrderBy(p => p.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task<PitchDeck> GetLatestForFundraisingRoundAsync(
            Guid fundraisingRoundId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.PitchDecks
                .Where(p => p.FundraisingRoundId == fundraisingRoundId)
                .OrderByDescending(p => p.CreationTime)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<PitchPresentation>> GetPresentationsForProspectAsync(
            Guid prospectId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var pitchDecks = await dbContext.PitchDecks
                .Include(p => p.Presentations)
                .ToListAsync(cancellationToken);
            
            return pitchDecks
                .SelectMany(p => p.Presentations)
                .Where(p => p.ProspectId == prospectId)
                .OrderByDescending(p => p.Details.PresentationDate)
                .ToList();
        }

        public override async Task<IQueryable<PitchDeck>> WithDetailsAsync()
        {
            // Include relevant collections by default when requesting details
            return (await GetQueryableAsync())
                .Include(p => p.Versions)
                .Include(p => p.Presentations)
                .Include(p => p.Feedbacks);
        }
    }
}
