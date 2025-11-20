using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Specifications;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.EntityFrameworkCore.Repositories
{
    public class EfCoreFinancialModelRepository 
        : EfCoreRepository<IPerformanceFinancialModelingDbContext, FinancialModel, Guid>, IFinancialModelRepository
    {
        public EfCoreFinancialModelRepository(
            IDbContextProvider<IPerformanceFinancialModelingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<FinancialModel> FindByNameAsync(string name, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(m => m.Name == name && m.TenantId == tenantId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<FinancialModel>> GetActiveModelsAsync(Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var specification = new ActiveFinancialModelsSpecification();
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<FinancialModel>> GetByStatusAsync(ModelStatus status, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var specification = new ModelsByStatusSpecification(status);
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<FinancialModel>> GetCalculableModelsAsync(Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var specification = new CalculableModelsSpecification();
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<FinancialModel>> GetListAsync(FinancialModelSpecification specification, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .OrderBy(m => m.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(FinancialModelSpecification specification, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        // Override to ensure proper includes for aggregate
        public override async Task<FinancialModel> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                // Include child entities
                query = query
                    .Include(m => m.Scenarios)
                    .Include(m => m.Assumptions)
                    .Include(m => m.Projections);
            }
            
            var model = await query.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
            
            if (model == null)
            {
                throw new EntityNotFoundException(typeof(FinancialModel), id);
            }
            
            return model;
        }

        public override async Task<FinancialModel> FindAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query
                    .Include(m => m.Scenarios)
                    .Include(m => m.Assumptions)
                    .Include(m => m.Projections);
            }
            
            return await query.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public override async Task<IQueryable<FinancialModel>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(m => m.Scenarios)
                .Include(m => m.Assumptions)
                .Include(m => m.Projections);
        }
    }
}
