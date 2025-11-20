using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.EventHandlers
{
    /// <summary>
    /// Handles distributed events related to automation workflows
    /// </summary>
    public class AutomationEventHandler : 
        IDistributedEventHandler<AutomationWorkflowTriggeredEto>,
        ITransientDependency
    {
        private readonly ILogger<AutomationEventHandler> _logger;

        public AutomationEventHandler(ILogger<AutomationEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleEventAsync(AutomationWorkflowTriggeredEto eventData)
        {
            _logger.LogInformation("Automation workflow {WorkflowId} ({WorkflowName}) triggered by {EntityType} {EntityId}", 
                eventData.Id, eventData.WorkflowName, eventData.TriggeredByEntityType, eventData.TriggeredByEntityId);
                
            // Could trigger notifications, logging, etc.
            
            return Task.CompletedTask;
        }
    }
}
