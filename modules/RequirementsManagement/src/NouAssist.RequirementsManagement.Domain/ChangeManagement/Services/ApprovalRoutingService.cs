using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.ChangeManagement.Services
{
    /// <summary>
    /// Service for managing approval routing based on change impact and organizational rules
    /// </summary>
    public class ApprovalRoutingService : DomainService, IApprovalRoutingService
    {
        private readonly ILogger<ApprovalRoutingService> _logger;

        public ApprovalRoutingService(ILogger<ApprovalRoutingService> logger)
        {
            _logger = logger;
        }

        public async Task<List<ApprovalStepDefinition>> DetermineApprovalStepsAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Determining approval steps for change request {RequestId}", 
                changeRequest.Id);

            var approvalSteps = new List<ApprovalStepDefinition>();

            // Get total impact to determine approval levels required
            var (totalCost, totalTime) = changeRequest.GetTotalImpact();

            // Business Analyst is always required for initial review
            approvalSteps.Add(new ApprovalStepDefinition
            {
                ApproverRole = ApproverRole.BusinessAnalyst,
                ApproverId = await GetBusinessAnalystAsync(changeRequest, cancellationToken),
                SequenceOrder = 1,
                IsRequired = true
            });

            // Technical Lead required for technical changes
            if (HasTechnicalImpact(changeRequest))
            {
                approvalSteps.Add(new ApprovalStepDefinition
                {
                    ApproverRole = ApproverRole.TechnicalLead,
                    ApproverId = await GetTechnicalLeadAsync(changeRequest, cancellationToken),
                    SequenceOrder = 2,
                    IsRequired = true
                });
            }

            // Product Manager required for significant impact
            if (totalCost > 5000m || totalTime > 15)
            {
                approvalSteps.Add(new ApprovalStepDefinition
                {
                    ApproverRole = ApproverRole.ProductManager,
                    ApproverId = await GetProductManagerAsync(changeRequest, cancellationToken),
                    SequenceOrder = 3,
                    IsRequired = true
                });
            }

            // QA approval for quality-related changes
            if (HasQualityImpact(changeRequest))
            {
                approvalSteps.Add(new ApprovalStepDefinition
                {
                    ApproverRole = ApproverRole.QualityAssurance,
                    ApproverId = await GetQualityAssuranceLeadAsync(changeRequest, cancellationToken),
                    SequenceOrder = 4,
                    IsRequired = true
                });
            }

            // TODO: Add more complex routing rules based on:
            // - Affected requirement categories
            // - Stakeholder definitions
            // - Organizational hierarchy
            // - Change classification (critical, major, minor)

            // Sort by sequence order
            approvalSteps = approvalSteps.OrderBy(a => a.SequenceOrder).ToList();

            _logger.LogInformation("Determined {Count} approval steps for change request {RequestId}", 
                approvalSteps.Count, changeRequest.Id);

            return approvalSteps;
        }

        public async Task RouteForApprovalAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Routing change request {RequestId} for approval", changeRequest.Id);

            try
            {
                var approvalSteps = await DetermineApprovalStepsAsync(changeRequest, cancellationToken);

                foreach (var step in approvalSteps)
                {
                    changeRequest.AddApprovalStep(
                        step.ApproverRole,
                        step.ApproverId,
                        step.SequenceOrder
                    );
                }

                // Move to pending approval status if not already
                if (changeRequest.Status != RequestStatus.PendingApproval)
                {
                    changeRequest.MoveToPendingApproval();
                }

                _logger.LogInformation("Successfully routed change request {RequestId} with {Count} approval steps", 
                    changeRequest.Id, approvalSteps.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to route change request {RequestId} for approval", 
                    changeRequest.Id);
                throw;
            }
        }

        public async Task<Guid?> GetNextApproverAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken = default)
        {
            var nextStep = changeRequest.GetNextPendingApprovalStep();
            
            // TODO: Add logic to handle approver unavailability and delegation
            
            await Task.Delay(1, cancellationToken); // Simulate async operation
            
            return nextStep?.ApproverId;
        }

        public async Task<bool> CanUserApproveAsync(
            Guid userId,
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken = default)
        {
            // TODO: Implement proper authorization checks based on:
            // - User roles and permissions
            // - Organizational hierarchy
            // - Delegation rules
            // - Conflict of interest rules (e.g., submitter cannot approve own request)

            if (userId == changeRequest.SubmitterId)
            {
                return false; // Submitter cannot approve their own request
            }

            var nextStep = changeRequest.GetNextPendingApprovalStep();
            if (nextStep == null)
            {
                return false; // No pending approvals
            }

            await Task.Delay(1, cancellationToken); // Simulate async operation

            return nextStep.ApproverId == userId;
        }

        private bool HasTechnicalImpact(RequirementChangeRequest changeRequest)
        {
            // Check if the change has technical implications
            return changeRequest.ImpactStatements.Any(i => 
                i.ImpactDetails.Type == ImpactType.Resources ||
                i.ImpactDetails.Type == ImpactType.Quality ||
                i.Description.Contains("technical", StringComparison.OrdinalIgnoreCase) ||
                i.Description.Contains("architecture", StringComparison.OrdinalIgnoreCase));
        }

        private bool HasQualityImpact(RequirementChangeRequest changeRequest)
        {
            // Check if the change affects quality attributes
            return changeRequest.ImpactStatements.Any(i => 
                i.ImpactDetails.Type == ImpactType.Quality ||
                i.Description.Contains("quality", StringComparison.OrdinalIgnoreCase) ||
                i.Description.Contains("performance", StringComparison.OrdinalIgnoreCase) ||
                i.Description.Contains("security", StringComparison.OrdinalIgnoreCase));
        }

        private async Task<Guid> GetBusinessAnalystAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken)
        {
            // TODO: Implement logic to find appropriate business analyst
            // Based on requirement domain, workload, availability, etc.
            await Task.Delay(1, cancellationToken);
            return Guid.NewGuid(); // Placeholder
        }

        private async Task<Guid> GetTechnicalLeadAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken)
        {
            // TODO: Implement logic to find appropriate technical lead
            await Task.Delay(1, cancellationToken);
            return Guid.NewGuid(); // Placeholder
        }

        private async Task<Guid> GetProductManagerAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken)
        {
            // TODO: Implement logic to find appropriate product manager
            await Task.Delay(1, cancellationToken);
            return Guid.NewGuid(); // Placeholder
        }

        private async Task<Guid> GetQualityAssuranceLeadAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken)
        {
            // TODO: Implement logic to find appropriate QA lead
            await Task.Delay(1, cancellationToken);
            return Guid.NewGuid(); // Placeholder
        }
    }
}
