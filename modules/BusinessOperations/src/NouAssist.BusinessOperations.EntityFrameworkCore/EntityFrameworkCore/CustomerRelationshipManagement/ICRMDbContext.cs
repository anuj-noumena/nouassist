using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.EntityFrameworkCore
{
    [ConnectionStringName("CRM")]
    public interface ICRMDbContext : IEfCoreDbContext
    {
        DbSet<Lead> Leads { get; }
        DbSet<Contact> Contacts { get; }
        DbSet<Organization> Organizations { get; }
        DbSet<InteractionLog> InteractionLogs { get; }
        DbSet<AutomationWorkflow> AutomationWorkflows { get; }
        DbSet<CommunicationTemplate> CommunicationTemplates { get; }
    }
}
