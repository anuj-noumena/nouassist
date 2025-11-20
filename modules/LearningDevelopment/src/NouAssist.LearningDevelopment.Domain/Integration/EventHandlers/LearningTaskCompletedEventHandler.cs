using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.LearningDevelopment.Integration.Data;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.Learning.Data;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.Data;
using NouAssist.WorkManagement.TaskManagement.Events;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.LearningDevelopment.Integration.EventHandlers
{
    /// <summary>
    /// Handles TaskCompletedEto to sync learning progress
    /// </summary>
    public class LearningTaskCompletedEventHandler :
        IDistributedEventHandler<TaskCompletedEto>,
        ITransientDependency
    {
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly ILearningSessionRepository _learningSessionRepository;
        private readonly ISkillDevelopmentSessionRepository _skillSessionRepository;
        private readonly ILogger<LearningTaskCompletedEventHandler> _logger;

        public LearningTaskCompletedEventHandler(
            ILearningPathRepository learningPathRepository,
            ILearningSessionRepository learningSessionRepository,
            ISkillDevelopmentSessionRepository skillSessionRepository,
            ILogger<LearningTaskCompletedEventHandler> logger)
        {
            _learningPathRepository = learningPathRepository;
            _learningSessionRepository = learningSessionRepository;
            _skillSessionRepository = skillSessionRepository;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(TaskCompletedEto eventData)
        {
            try
            {
                // Check if this is a learning task via custom fields
                // Note: Would need to query TaskInstance to get custom fields
                // For now, assume the event contains the data or we query it

                _logger.LogDebug(
                    "Processing task completion {TaskInstanceId} for potential learning sync",
                    eventData.TaskInstanceId);

                // This is a simplified version - in reality, you'd query the task to get custom fields
                // var taskInstance = await _taskInstanceRepo.GetAsync(eventData.TaskInstanceId);
                // var customFields = taskInstance.CustomFields;

                // For demonstration, assuming we can check if it's a learning task
                // In real implementation, you'd include this in the event or query it

                // Example: If we had the LearningPathId in the event
                // var learningPathId = eventData.CustomFields["LearningPathId"];
                // var phase = eventData.CustomFields["Phase"];

                _logger.LogInformation(
                    "Task {TaskInstanceId} completed - would sync to learning path if applicable",
                    eventData.TaskInstanceId);

                // Implementation would:
                // 1. Get LearningPath by custom field
                // 2. Determine if theory or practice task
                // 3. Advance appropriate session phase/step
                // 4. Update learning path progress
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error syncing task completion {TaskInstanceId} to learning progress",
                    eventData.TaskInstanceId);
                // Don't throw - this is optional sync
            }
        }
    }
}
