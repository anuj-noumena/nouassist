using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.EntityFrameworkCore
{
    [ConnectionStringName("CRM")]
    public class CRMDbContext : AbpDbContext<CRMDbContext>, ICRMDbContext
    {
        /* Define DbSets for aggregates and entities */
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<InteractionLog> InteractionLogs { get; set; }
        public DbSet<AutomationWorkflow> AutomationWorkflows { get; set; }
        public DbSet<CommunicationTemplate> CommunicationTemplates { get; set; }

        public CRMDbContext(DbContextOptions<CRMDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure the entities */
            builder.ConfigureCRM();
        }
    }
}
