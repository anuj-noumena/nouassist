using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Learning.Enums;
using NouAssist.Learning.Events;
using NouAssist.LearningDevelopment.Integration.Data;
using NouAssist.LearningDevelopment.Integration.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.LearningDevelopment.Integration.EventHandlers
{
    /// <summary>
    /// Handles PhaseCompletedEto to sync learning path progress
    /// Special handling for BloomAssessment phase completion
    /// </summary>
    public class PhaseCompletedEventHandler :
        IDistributedEventHandler<PhaseCompletedEto>,
        ITransientDependency
    {
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly IDistributedEventBus _eventBus;
        private readonly ILogger<PhaseCompletedEventHandler> _logger;

        public PhaseCompletedEventHandler(
            ILearningPathRepository learningPathRepository,
            IDistributedEventBus eventBus,
            ILogger<PhaseCompletedEventHandler> logger)
        {
            _learningPathRepository = learningPathRepository;
            _eventBus = eventBus;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(PhaseCompletedEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Learning phase {Phase} completed for topic {TopicId}. Syncing with learning path.",
                    eventData.Phase, eventData.TopicId);

                // Find learning path associated with this topic
                var learningPath = await _learningPathRepository.FindByTopicIdAsync(eventData.TopicId);
                if (learningPath == null)
                {
                    _logger.LogDebug(
                        "No learning path found for topic {TopicId}. Skipping sync.",
                        eventData.TopicId);
                    return;
                }

                // Calculate progress percentage based on phase
                var progressPercentage = CalculateProgressPercentage(eventData.Phase);
                learningPath.UpdateTheoryProgress(eventData.Phase, progressPercentage);

                // Special handling for BloomAssessment completion
                if (eventData.Phase == LearningPhase.BloomAssessment)
                {
                    _logger.LogInformation(
                        "Bloom Assessment completed for learning path {LearningPathId}. Signaling readiness for practice.",
                        learningPath.Id);

                    // Publish event to signal theory phase completion and readiness for practice
                    await _eventBus.PublishAsync(new TheoryPhaseCompletedEto
                    {
                        LearningPathId = learningPath.Id,
                        TopicId = learningPath.TopicId,
                        SkillId = learningPath.SkillId,
                        UserId = learningPath.UserId,
                        TenantId = learningPath.TenantId,
                        TheoryMasteryScore = learningPath.TheoryProgressPercentage,
                        CompletedAt = DateTime.UtcNow
                    });
                }

                await _learningPathRepository.UpdateAsync(learningPath, autoSave: true);

                _logger.LogInformation(
                    "Learning path {LearningPathId} synced with phase completion. Progress: {Progress}%",
                    learningPath.Id, learningPath.TheoryProgressPercentage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling PhaseCompletedEto for topic {TopicId}, phase {Phase}",
                    eventData.TopicId, eventData.Phase);
                throw;
            }
        }

        private int CalculateProgressPercentage(LearningPhase phase)
        {
            // Each phase represents ~16.67% progress
            // Phase 1 = 17%, Phase 2 = 33%, Phase 3 = 50%, Phase 4 = 67%, Phase 5 = 83%, Phase 6 = 100%
            return (int)phase * 17;
        }
    }
}
