using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Integration
{
    public class BusinessModelIntegrationService : ITransientDependency
    {
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly ILogger<BusinessModelIntegrationService> _logger;

        public BusinessModelIntegrationService(
            IDistributedEventBus distributedEventBus,
            ILogger<BusinessModelIntegrationService> logger)
        {
            _distributedEventBus = distributedEventBus;
            _logger = logger;
        }

        public async Task PublishCanvasValidationCompletedAsync(Guid canvasId, double validationProgress)
        {
            if (validationProgress >= 80.0) // Canvas is considered complete
            {
                await _distributedEventBus.PublishAsync(new BusinessModelCanvasCompletedEto
                {
                    Id = canvasId,
                    ValidationProgress = validationProgress,
                    CompletedAt = DateTime.UtcNow
                });

                _logger.LogInformation("Published canvas completion event for {CanvasId}", canvasId);
            }
        }
    }
}
