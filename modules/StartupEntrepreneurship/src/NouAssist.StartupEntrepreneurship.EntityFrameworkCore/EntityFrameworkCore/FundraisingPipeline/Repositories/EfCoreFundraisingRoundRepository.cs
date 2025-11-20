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
    public class EfCoreFundraisingRoundRepository : 
        EfCoreRepository<IFundraisingPipelineDbContext, FundraisingRound, Guid>, 
        IFundraisingRoundRepository
    {
        public EfCoreFundraisingRoundRepository(
            IDbContextProvider<IFundraisingPipelineDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<FundraisingRound> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.FundraisingRounds
                .Include(r => r.PipelineStages)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<List<FundraisingRound>> GetActiveRoundsAsync(
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.FundraisingRounds
                .Where(r => r.Stage == RoundStage.Active)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FundraisingRound>> GetByStageAsync(
            RoundStage stage, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.FundraisingRounds
                .Where(r => r.Stage == stage)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FundraisingRound>> GetByTypeAsync(
            RoundType type, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.FundraisingRounds
                .Where(r => r.Type == type)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FundraisingRound>> GetBySpecificationAsync(
            FundraisingRoundSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .OrderByDescending(r => r.StartDate)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<FundraisingRound>> WithDetailsAsync()
        {
            // Include PipelineStages by default when requesting details
            return (await GetQueryableAsync()).Include(r => r.PipelineStages);
        }
    }
}
