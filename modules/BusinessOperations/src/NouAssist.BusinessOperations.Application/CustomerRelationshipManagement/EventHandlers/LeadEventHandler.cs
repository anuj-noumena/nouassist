using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.EventHandlers
{
    /// <summary>
    /// Handles distributed events related to leads
    /// </summary>
    public class LeadEventHandler : 
        IDistributedEventHandler<LeadCreatedEto>,
        IDistributedEventHandler<LeadStatusChangedEto>,
        IDistributedEventHandler<LeadConvertedEto>,
        ITransientDependency
    {
        private readonly ILogger<LeadEventHandler> _logger;

        public LeadEventHandler(ILogger<LeadEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleEventAsync(LeadCreatedEto eventData)
        {
            _logger.LogInformation("Lead {LeadId} created: {LeadName} ({LeadEmail})", 
                eventData.Id, eventData.Name, eventData.Email);
                
            // Could trigger automation workflows, notifications, etc.
            
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(LeadStatusChangedEto eventData)
        {
            _logger.LogInformation("Lead {LeadId} status changed: {PreviousStatus} -> {NewStatus}", 
                eventData.Id, eventData.PreviousStatus, eventData.NewStatus);
                
            // Could trigger automation workflows, notifications, etc. based on status
            
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(LeadConvertedEto eventData)
        {
            _logger.LogInformation("Lead {LeadId} ({LeadName}) converted to contact {ContactId}", 
                eventData.Id, eventData.LeadName, eventData.ContactId);
                
            // Could trigger follow-up workflows, notifications, etc.
            
            return Task.CompletedTask;
        }
    }
}
