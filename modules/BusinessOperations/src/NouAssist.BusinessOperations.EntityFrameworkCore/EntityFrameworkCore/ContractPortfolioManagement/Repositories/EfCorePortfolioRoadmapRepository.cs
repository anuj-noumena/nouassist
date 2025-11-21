using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Repositories;
using NouAssist.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.ContractPortfolioManagement.Entities.Portfolio;
using NouAssist.ContractPortfolioManagement.EntityFrameworkCore;
using NouAssist.ContractPortfolioManagement.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ContractPortfolioManagement.Infrastructure.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IPortfolioRoadmapRepository
    /// </summary>
    public class EfCorePortfolioRoadmapRepository : 
        EfCoreRepository<ContractPortfolioManagementDbContext, PortfolioRoadmapAggregate, Guid>,
        IPortfolioRoadmapRepository
    {
        public EfCorePortfolioRoadmapRepository(
            IDbContextProvider<ContractPortfolioManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<PortfolioRoadmapAggregate>> GetByPortfolioIdAsync(
            Guid portfolioId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.PortfolioRoadmaps
                .Where(r => r.PortfolioId == portfolioId)
                .ToListAsync(cancellationToken);
        }

        public async Task<PortfolioRoadmapAggregate> GetWithMilestonesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.PortfolioRoadmaps
                .Include(r => r.Milestones)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<PortfolioRoadmapAggregate> GetWithDependenciesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.PortfolioRoadmaps
                .Include(r => r.Dependencies)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<PortfolioRoadmapAggregate> GetWithAllDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.PortfolioRoadmaps
                .Include(r => r.Milestones)
                .Include(r => r.Dependencies)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<List<RoadmapMilestone>> GetMilestonesInDateRangeAsync(
            Guid roadmapId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.RoadmapMilestones
                .Where(m => m.RoadmapId == roadmapId &&
                            m.Date.PlannedDate >= startDate &&
                            m.Date.PlannedDate <= endDate)
                .OrderBy(m => m.Date.PlannedDate)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<PortfolioRoadmapAggregate>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(r => r.Milestones)
                .Include(r => r.Dependencies);
        }
    }
}
