using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.ReflectionAndSupport.ActivityTracking.Data;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.BackgroundWorkers
{
    public class AuditLogArchiveWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public AuditLogArchiveWorker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory)
            : base(timer, serviceScopeFactory)
        {
            Timer.Period = 86400000; // Run once per day
        }

        [UnitOfWork]
        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("AuditLogArchiveWorker: Archiving old audit logs...");

            var auditLogRepository = workerContext.ServiceProvider.GetRequiredService<IAuditLogRepository>();

            // Archive logs older than 90 days
            var archiveDate = DateTime.UtcNow.AddDays(-90);

            var spec = new Specification<Aggregates.AuditLog>(a => a.Timestamp < archiveDate);

            var oldLogs = await auditLogRepository.GetListAsync(spec, 1000);

            Logger.LogInformation($"Found {oldLogs.Count} audit logs to archive");

            // TODO: Implement archiving logic (e.g., move to cold storage, compress, etc.)

            Logger.LogInformation("AuditLogArchiveWorker: Completed archiving");
        }
    }
}
