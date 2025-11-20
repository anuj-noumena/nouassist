using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories
{
    public class AutomationWorkflowRepository : EfCoreRepository<CRMDbContext, AutomationWorkflow, Guid>, IAutomationWorkflowRepository
    {
        public AutomationWorkflowRepository(IDbContextProvider<CRMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<AutomationWorkflow>> GetByTriggerTypeAsync(
            AutomationTriggerType triggerType, 
            string entityType, 
            bool activeOnly = true, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = dbContext.AutomationWorkflows
                .Where(w => 
                    EF.Property<AutomationTriggerType>(w.TriggerCondition, "TriggerType") == triggerType &&
                    EF.Property<string>(w.TriggerCondition, "EntityType") == entityType);
                
            if (activeOnly)
            {
                query = query.Where(w => w.IsActive);
            }
            
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<AutomationWorkflow>> GetByFieldTriggerAsync(
            string entityType, 
            string fieldName, 
            string fieldValue = null, 
            bool activeOnly = true, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = dbContext.AutomationWorkflows
                .Where(w => 
                    (EF.Property<AutomationTriggerType>(w.TriggerCondition, "TriggerType") == AutomationTriggerType.CustomField ||
                     EF.Property<AutomationTriggerType>(w.TriggerCondition, "TriggerType") == AutomationTriggerType.LeadStatusChanged) &&
                    EF.Property<string>(w.TriggerCondition, "EntityType") == entityType &&
                    EF.Property<string>(w.TriggerCondition, "FieldName") == fieldName);
                
            if (fieldValue != null)
            {
                query = query.Where(w => 
                    EF.Property<string>(w.TriggerCondition, "ExpectedValue") == fieldValue);
            }
            
            if (activeOnly)
            {
                query = query.Where(w => w.IsActive);
            }
            
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<AutomationWorkflow>> GetScheduledWorkflowsAsync(
            bool activeOnly = true, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = dbContext.AutomationWorkflows
                .Where(w => 
                    EF.Property<AutomationTriggerType>(w.TriggerCondition, "TriggerType") == AutomationTriggerType.ScheduledTime &&
                    EF.Property<string>(w.TriggerCondition, "ScheduleExpression") != null);
                
            if (activeOnly)
            {
                query = query.Where(w => w.IsActive);
            }
            
            return await query.ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<AutomationWorkflow>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(w => w.Steps)
                .Include(w => w.ExecutionRecords);
        }
    }
}
