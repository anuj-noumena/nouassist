using Microsoft.Extensions.DependencyInjection;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.EntityFrameworkCore.ExternalSystems;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces;
using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(CRMDomainModule)
    )]
    public class CRMInfrastructureModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IExternalSystemIntegrationService, ExternalSystemIntegrationService>();
        }
    }
}
