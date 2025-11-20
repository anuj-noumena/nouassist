using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Repositories;
using NouAssist.RequirementsManagement.QualityAssurance.Specifications.Base;
using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore.Repositories
{
    public class EfCoreTestCycleRepository : 
        EfCoreRepository<QADbContext, TestCycle, Guid>, 
        ITestCycleRepository
    {
        public EfCoreTestCycleRepository(
            IDbContextProvider<QADbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TestCycle> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var testCycle = await dbContext.TestCycles
                .Include(tc => tc.TestRuns)
                .FirstOrDefaultAsync(tc => tc.Id == id, cancellationToken);
                
            if (testCycle == null)
            {
                throw new NouAssist.RequirementsManagement.QualityAssurance.Exceptions.TestCycleNotFoundException(id);
            }
            
            return testCycle;
        }

        public async Task<List<TestCycle>> GetByTestPlanIdAsync(
            Guid testPlanId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.TestCycles
                .Where(tc => tc.TestPlanId == testPlanId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TestCycle>> GetActiveByAssigneeIdAsync(
            Guid assigneeId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.TestCycles
                .Where(tc => tc.AssigneeId == assigneeId && 
                             tc.Status == Enums.TestCycleStatus.InProgress)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TestCycle>> GetListAsync(
            QASpecification<TestCycle> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<TestCycle>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(tc => tc.TestRuns);
        }
    }
}
