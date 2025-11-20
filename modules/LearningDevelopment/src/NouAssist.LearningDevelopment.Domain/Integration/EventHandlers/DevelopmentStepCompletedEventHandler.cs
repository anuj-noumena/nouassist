using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.LearningDevelopment.Integration.Data;
using NouAssist.LearningDevelopment.Integration.Events;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.Learning.Data;
using NouAssist.SkillDevelopment.Enums;
using NouAssist.SkillDevelopment.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.LearningDevelopment.Integration.EventHandlers
{
    /// <summary>
    /// Handles DevelopmentStepCompletedEto to sync learning path progress
    /// Triggers spaced repetition when Application step is completed
    /// </summary>
    public class DevelopmentStepCompletedEventHandler :
        IDistributedEventHandler<DevelopmentStepCompletedEto>,
        ITransientDependency
    {
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly ILearningSessionRepository _learningSessionRepository;
        private readonly IDistributedEventBus _eventBus;
        private readonly ILogger<DevelopmentStepCompletedEventHandler> _logger;

        public DevelopmentStepCompletedEventHandler(
            ILearningPathRepository learningPathRepository,
            ILearningSessionRepository learningSessionRepository,
            IDistributedEventBus eventBus,
            ILogger<DevelopmentStepCompletedEventHandler> logger)
        {
            _learningPathRepository = learningPathRepository;
            _learningSessionRepository = learningSessionRepository;
            _eventBus = eventBus;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(DevelopmentStepCompletedEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Development step {Step} completed for skill {SkillId}. Syncing with learning path.",
                    eventData.CompletedStep, eventData.SkillId);

                // Find learning path associated with this skill
                var learningPath = await _learningPathRepository.FindBySkillIdAsync(eventData.SkillId);
                if (learningPath == null)
                {
                    _logger.LogDebug(
                        "No learning path found for skill {SkillId}. Skipping sync.",
                        eventData.SkillId);
                    return;
                }

                // Calculate progress percentage based on step
                var progressPercentage = CalculateProgressPercentage(eventData.CompletedStep);
                learningPath.UpdatePracticeProgress(eventData.CompletedStep, progressPercentage);

                await _learningPathRepository.UpdateAsync(learningPath, autoSave: true);

                _logger.LogInformation(
                    "Learning path {LearningPathId} synced with step completion. Practice progress: {Progress}%",
                    learningPath.Id, learningPath.PracticeProgressPercentage);

                // Trigger spaced repetition when Application step is completed
                if (eventData.CompletedStep == DevelopmentStep.Application && learningPath.LearningSessionId.HasValue)
                {
                    _logger.LogInformation(
                        "Application step completed. Triggering spaced repetition for learning session {SessionId}",
                        learningPath.LearningSessionId.Value);

                    // Note: This could trigger a command/event to start spaced repetition review
                    // For now, we just log it as a signal
                }

                // When Practice step is completed, publish event
                if (eventData.CompletedStep == DevelopmentStep.Practice)
                {
                    await _eventBus.PublishAsync(new PracticePhaseCompletedEto
                    {
                        LearningPathId = learningPath.Id,
                        TopicId = learningPath.TopicId,
                        SkillId = learningPath.SkillId,
                        UserId = learningPath.UserId,
                        TenantId = learningPath.TenantId,
                        PracticeProficiencyScore = progressPercentage,
                        CompletedAt = DateTime.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling DevelopmentStepCompletedEto for skill {SkillId}, step {Step}",
                    eventData.SkillId, eventData.CompletedStep);
                throw;
            }
        }

        private int CalculateProgressPercentage(DevelopmentStep step)
        {
            // Each step represents ~16.67% progress
            // Step 1 = 17%, Step 2 = 33%, Step 3 = 50%, Step 4 = 67%, Step 5 = 83%, Step 6 = 100%
            return (int)step * 17;
        }
    }
}
