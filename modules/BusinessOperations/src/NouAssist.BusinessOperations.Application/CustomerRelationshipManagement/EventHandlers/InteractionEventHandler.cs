using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.EventHandlers
{
    /// <summary>
    /// Handles distributed events related to interactions
    /// </summary>
    public class InteractionEventHandler : 
        IDistributedEventHandler<InteractionLoggedEto>,
        ITransientDependency
    {
        private readonly ILogger<InteractionEventHandler> _logger;

        public InteractionEventHandler(ILogger<InteractionEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleEventAsync(InteractionLoggedEto eventData)
        {
            _logger.LogInformation("Interaction logged for {EntityType} {EntityId}: {InteractionType}", 
                eventData.RelatedEntityType, eventData.RelatedEntityId, eventData.InteractionType);
                
            // Could trigger follow-up workflows, notifications, analytics updates, etc.
            
            return Task.CompletedTask;
        }
    }
}
