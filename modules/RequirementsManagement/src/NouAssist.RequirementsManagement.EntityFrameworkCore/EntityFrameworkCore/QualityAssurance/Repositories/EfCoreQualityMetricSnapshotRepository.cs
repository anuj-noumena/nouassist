using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.QualityAssurance.Entities;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore.Repositories
{
    public class EfCoreQualityMetricSnapshotRepository : 
        EfCoreRepository<QADbContext, QualityMetricSnapshot, Guid>, 
        IQualityMetricSnapshotRepository
    {
        public EfCoreQualityMetricSnapshotRepository(
            IDbContextProvider<QADbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<QualityMetricSnapshot>> GetByTestPlanIdAsync(
            Guid testPlanId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.QualityMetricSnapshots
                .Where(m => m.TestPlanId == testPlanId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<QualityMetricSnapshot>> GetByReleaseVersionAsync(
            string releaseVersion, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.QualityMetricSnapshots
                .Where(m => m.ReleaseVersion == releaseVersion)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<QualityMetricSnapshot>> GetByMetricTypeAsync(
            MetricType metricType, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.QualityMetricSnapshots
                .Where(m => EF.Property<string>(m, "Metric_Type") == metricType.ToString())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<QualityMetricSnapshot>> GetByTimeRangeAsync(
            DateTime startTime, 
            DateTime endTime, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.QualityMetricSnapshots
                .Where(m => EF.Property<DateTime>(m, "Metric_Timestamp") >= startTime && 
                            EF.Property<DateTime>(m, "Metric_Timestamp") <= endTime)
                .ToListAsync(cancellationToken);
        }
    }
}
