using Microsoft.Extensions.DependencyInjection;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(CRMDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class CRMEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CRMDbContext>(options =>
            {
                /* Add custom repositories here */
                options.AddRepository<Lead, LeadRepository>();
                options.AddRepository<Contact, ContactRepository>();
                options.AddRepository<Organization, OrganizationRepository>();
                options.AddRepository<InteractionLog, InteractionLogRepository>();
                options.AddRepository<AutomationWorkflow, AutomationWorkflowRepository>();
                options.AddRepository<CommunicationTemplate, CommunicationTemplateRepository>();
            });
        }
    }
}
