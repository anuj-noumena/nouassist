using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Events
{
    /// <summary>
    /// Local event handlers for Product Discovery domain events
    /// </summary>
    public class ProductDiscoveryEventHandlers : 
        ILocalEventHandler<CustomerInterviewCompletedEto>,
        ILocalEventHandler<ProblemHypothesisValidatedEto>,
        ILocalEventHandler<FeatureDefinitionPrioritizedEto>,
        ITransientDependency
    {
        private readonly ICustomerSegmentRepository _segmentRepository;
        private readonly ILogger<ProductDiscoveryEventHandlers> _logger;

        public ProductDiscoveryEventHandlers(
            ICustomerSegmentRepository segmentRepository,
            ILogger<ProductDiscoveryEventHandlers> logger)
        {
            _segmentRepository = segmentRepository;
            _logger = logger;
        }

        public async Task HandleEventAsync(CustomerInterviewCompletedEto eventData)
        {
            try
            {
                _logger.LogInformation("Processing completed interview {InterviewId}", eventData.Id);

                // Update customer segment actual sample size
                if (eventData.TenantId.HasValue)
                {
                    // In a full implementation, we would:
                    // 1. Find the customer segment associated with this interview
                    // 2. Increment the actual sample size
                    // 3. Check if the segment target is met
                    // 4. Possibly trigger notifications

                    _logger.LogInformation("Interview {InterviewId} completed with {InsightCount} insights", 
                        eventData.Id, eventData.InsightCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling CustomerInterviewCompleted event for {InterviewId}", eventData.Id);
                throw;
            }
        }

        public async Task HandleEventAsync(ProblemHypothesisValidatedEto eventData)
        {
            try
            {
                _logger.LogInformation("Processing validated hypothesis {HypothesisId} with status {Status}", 
                    eventData.Id, eventData.Status);

                // In a full implementation, we would:
                // 1. Update related features based on validation results
                // 2. Trigger re-prioritization if needed
                // 3. Send notifications to stakeholders
                // 4. Update portfolio metrics

                if (eventData.Status == ValidationStatus.Validated)
                {
                    _logger.LogInformation("Hypothesis {HypothesisId} successfully validated", eventData.Id);
                }
                else if (eventData.Status == ValidationStatus.Invalidated)
                {
                    _logger.LogInformation("Hypothesis {HypothesisId} invalidated - consider pivot", eventData.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling ProblemHypothesisValidated event for {HypothesisId}", eventData.Id);
                throw;
            }
        }

        public async Task HandleEventAsync(FeatureDefinitionPrioritizedEto eventData)
        {
            try
            {
                _logger.LogInformation("Processing prioritized feature {FeatureId} with priority {Priority}", 
                    eventData.Id, eventData.Priority);

                // In a full implementation, we would:
                // 1. Update MVP recommendations if needed
                // 2. Notify development teams of priority changes
                // 3. Update roadmap planning
                // 4. Trigger dependency checks

                if (eventData.MvpStatus == MvpStatus.Planned && eventData.Priority >= FeaturePriority.High)
                {
                    _logger.LogInformation("High-priority feature {FeatureId} planned for MVP", eventData.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling FeatureDefinitionPrioritized event for {FeatureId}", eventData.Id);
                throw;
            }
        }
    }
}
