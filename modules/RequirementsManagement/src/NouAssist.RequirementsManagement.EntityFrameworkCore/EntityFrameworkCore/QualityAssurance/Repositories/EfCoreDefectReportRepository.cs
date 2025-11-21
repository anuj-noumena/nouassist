using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.Repositories;
using NouAssist.RequirementsManagement.QualityAssurance.Specifications.Base;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore.Repositories
{
    public class EfCoreDefectReportRepository : 
        EfCoreRepository<QADbContext, DefectReport, Guid>, 
        IDefectReportRepository
    {
        public EfCoreDefectReportRepository(
            IDbContextProvider<QADbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<DefectReport> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var defect = await dbContext.DefectReports
                .Include("Comments") // Using string because it's an owned entity collection
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
                
            if (defect == null)
            {
                throw new NouAssist.RequirementsManagement.QualityAssurance.Exceptions.DefectNotFoundException(id);
            }
            
            return defect;
        }

        public async Task<List<DefectReport>> GetByTestCycleIdAsync(
            Guid testCycleId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.DefectReports
                .Where(d => d.TestCycleId == testCycleId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DefectReport>> GetByTestPlanIdAsync(
            Guid testPlanId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.DefectReports
                .Where(d => d.TestPlanId == testPlanId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DefectReport>> GetByAssigneeIdAsync(
            Guid assigneeId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.DefectReports
                .Where(d => d.AssigneeId == assigneeId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DefectReport>> GetByStatusAsync(
            DefectStatus status, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.DefectReports
                .Where(d => d.Status == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DefectReport>> GetBySeverityAsync(
            DefectSeverity severity, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.DefectReports
                .Where(d => d.Severity == severity)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DefectReport>> GetListAsync(
            QASpecification<DefectReport> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await DynamicQueryableExtensions.Where(query, specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<DefectReport>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include("Comments"); // Using string because it's an owned entity collection
        }
    }
}
