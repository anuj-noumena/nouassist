using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.BackgroundWorkers
{
    /// <summary>
    /// Background worker that checks data quality
    /// </summary>
    public class DataQualityChecker : AsyncPeriodicBackgroundWorkerBase
    {
        public DataQualityChecker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory)
            : base(timer, serviceScopeFactory)
        {
            Timer.Period = 24 * 60 * 60 * 1000; // 24 hours
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("DataQualityChecker is working...");
            
            try
            {
                var leadRepository = workerContext.ServiceProvider.GetRequiredService<ILeadRepository>();
                var contactRepository = workerContext.ServiceProvider.GetRequiredService<IContactRepository>();
                var organizationRepository = workerContext.ServiceProvider.GetRequiredService<IOrganizationRepository>();
                
                // Simplified implementation - in a real system, we would have more sophisticated data quality checks
                // Here we just log counts
                
                var leadCount = await leadRepository.CountAsync();
                var contactCount = await contactRepository.CountAsync();
                var orgCount = await organizationRepository.CountAsync();
                
                Logger.LogInformation("Data quality check completed. Counts: Leads={LeadCount}, Contacts={ContactCount}, Organizations={OrgCount}",
                    leadCount, contactCount, orgCount);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in DataQualityChecker: {Message}", ex.Message);
            }
        }
    }
}
