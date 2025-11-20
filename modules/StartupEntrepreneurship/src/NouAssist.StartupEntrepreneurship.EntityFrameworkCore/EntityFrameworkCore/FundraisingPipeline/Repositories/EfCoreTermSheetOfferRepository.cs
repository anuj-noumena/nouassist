using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Specifications;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Repositories
{
    public class EfCoreTermSheetOfferRepository : 
        EfCoreRepository<IFundraisingPipelineDbContext, TermSheetOffer, Guid>, 
        ITermSheetOfferRepository
    {
        public EfCoreTermSheetOfferRepository(
            IDbContextProvider<IFundraisingPipelineDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TermSheetOffer> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.TermSheetOffers
                .Include(t => t.Clauses)
                .Include(t => t.NegotiationNotes)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<TermSheetOffer>> GetByProspectAsync(
            Guid prospectId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.TermSheetOffers
                .Where(t => t.ProspectId == prospectId)
                .OrderByDescending(t => t.ReceivedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TermSheetOffer>> GetByFundraisingRoundAsync(
            Guid fundraisingRoundId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.TermSheetOffers
                .Where(t => t.FundraisingRoundId == fundraisingRoundId)
                .OrderByDescending(t => t.ReceivedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TermSheetOffer>> GetByStatusAsync(
            TermSheetStatus status, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.TermSheetOffers
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.ReceivedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TermSheetOffer>> GetBySpecificationAsync(
            TermSheetSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            query = query
                .Include(t => t.Clauses)
                .Include(t => t.NegotiationNotes);
            
            return await query
                .Where(specification.ToExpression())
                .OrderByDescending(t => t.ReceivedDate)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<TermSheetOffer>> WithDetailsAsync()
        {
            // Include relevant collections by default when requesting details
            return (await GetQueryableAsync())
                .Include(t => t.Clauses)
                .Include(t => t.NegotiationNotes);
        }
    }
}
