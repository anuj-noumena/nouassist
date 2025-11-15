using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.Approval.Data;
using NouAssist.WorkManagement.Approval.Enums;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace NouAssist.WorkManagement.Approval.BackgroundJobs
{
    public class ApprovalTimeoutCheckJob : AsyncBackgroundJob<ApprovalTimeoutCheckArgs>, ITransientDependency
    {
        private readonly IApprovalRequestRepository _requestRepository;
        private readonly IApprovalWorkflowRepository _workflowRepository;
        private readonly ILogger<ApprovalTimeoutCheckJob> _logger;

        public ApprovalTimeoutCheckJob(
            IApprovalRequestRepository requestRepository,
            IApprovalWorkflowRepository workflowRepository,
            ILogger<ApprovalTimeoutCheckJob> logger)
        {
            _requestRepository = requestRepository;
            _workflowRepository = workflowRepository;
            _logger = logger;
        }

        [UnitOfWork]
        public override async Task ExecuteAsync(ApprovalTimeoutCheckArgs args)
        {
            _logger.LogInformation("Starting approval timeout check job");

            var overdueRequests = await _requestRepository.GetOverdueRequestsAsync();
            var slaBreachedRequests = await _requestRepository.GetSlaBreachedRequestsAsync();

            _logger.LogInformation(
                $"Found {overdueRequests.Count} overdue requests and {slaBreachedRequests.Count} SLA breached requests");

            // Process overdue requests
            foreach (var request in overdueRequests)
            {
                try
                {
                    var workflow = await _workflowRepository.GetWithStepsAsync(request.WorkflowId);
                    
                    if (!string.IsNullOrEmpty(workflow.TimeoutAction))
                    {
                        await HandleTimeoutAction(request, workflow.TimeoutAction);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing timeout for request {request.Id}");
                }
            }

            // Log SLA breaches (notifications would be handled by event handlers)
            foreach (var request in slaBreachedRequests)
            {
                _logger.LogWarning($"SLA breached for request {request.Id}");
            }

            _logger.LogInformation("Completed approval timeout check job");
        }

        private async Task HandleTimeoutAction(
            Approval.Aggregates.ApprovalRequest request, 
            string timeoutAction)
        {
            switch (timeoutAction.ToLower())
            {
                case "auto-approve":
                    request.Approve(Guid.Empty); // System user
                    await _requestRepository.UpdateAsync(request);
                    break;
                    
                case "auto-reject":
                    request.Reject(Guid.Empty); // System user
                    await _requestRepository.UpdateAsync(request);
                    break;
                    
                case "escalate":
                    // Escalation logic would go here
                    _logger.LogInformation($"Escalating request {request.Id}");
                    break;
                    
                default:
                    _logger.LogWarning($"Unknown timeout action: {timeoutAction}");
                    break;
            }
        }
    }

    public class ApprovalTimeoutCheckArgs
    {
        // Can add tenant filtering or other parameters
    }
}
