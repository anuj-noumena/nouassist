using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Repositories;
using NouAssist.RequirementsManagement.QualityAssurance.Specifications.Base;
using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore.Repositories
{
    public class EfCoreTestPlanRepository : 
        EfCoreRepository<QADbContext, TestPlan, Guid>, 
        ITestPlanRepository
    {
        public EfCoreTestPlanRepository(
            IDbContextProvider<QADbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TestPlan> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var testPlan = await dbContext.TestPlans
                .Include(tp => tp.TestCycles)
                    .ThenInclude(tc => tc.TestRuns)
                .FirstOrDefaultAsync(tp => tp.Id == id, cancellationToken);
                
            if (testPlan == null)
            {
                throw new NouAssist.RequirementsManagement.QualityAssurance.Exceptions.TestPlanNotFoundException(id);
            }
            
            return testPlan;
        }

        public async Task<List<TestPlan>> GetByReleaseVersionAsync(
            string releaseVersion, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.TestPlans
                .Where(tp => tp.ReleaseVersion == releaseVersion)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TestPlan>> GetByRequirementIdAsync(
            Guid requirementId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            // We need to handle the conversion from the stored comma-separated string
            return await dbContext.TestPlans
                .Where(tp => EF.Property<string>(tp, "InScopeRequirementIds").Contains(requirementId.ToString()))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TestPlan>> GetActiveTestPlansAsync(
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.TestPlans
                .Where(tp => tp.Status == Enums.TestPlanStatus.Active)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TestPlan>> GetListAsync(
            QASpecification<TestPlan> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<TestPlan>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(tp => tp.TestCycles);
        }
    }
}
