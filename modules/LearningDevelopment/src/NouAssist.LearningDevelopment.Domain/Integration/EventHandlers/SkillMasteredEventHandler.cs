using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.LearningDevelopment.Integration.Data;
using NouAssist.LearningDevelopment.Integration.Events;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.Learning.Data;
using NouAssist.SkillDevelopment.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.LearningDevelopment.Integration.EventHandlers
{
    /// <summary>
    /// Handles SkillMasteredEto to update learning path and topic
    /// </summary>
    public class SkillMasteredEventHandler :
        IDistributedEventHandler<SkillMasteredEto>,
        ITransientDependency
    {
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IDistributedEventBus _eventBus;
        private readonly ILogger<SkillMasteredEventHandler> _logger;

        public SkillMasteredEventHandler(
            ILearningPathRepository learningPathRepository,
            ITopicRepository topicRepository,
            IDistributedEventBus eventBus,
            ILogger<SkillMasteredEventHandler> logger)
        {
            _learningPathRepository = learningPathRepository;
            _topicRepository = topicRepository;
            _eventBus = eventBus;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(SkillMasteredEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Skill {SkillId} mastered by user {UserId}. Updating learning path integration.",
                    eventData.SkillId, eventData.UserId);

                // Find learning path associated with this skill
                var learningPath = await _learningPathRepository.FindBySkillIdAsync(eventData.SkillId);
                if (learningPath == null)
                {
                    _logger.LogDebug(
                        "No learning path found for skill {SkillId}. Skipping integration update.",
                        eventData.SkillId);
                    return;
                }

                // Complete practice phase
                learningPath.CompletePracticePhase();
                await _learningPathRepository.UpdateAsync(learningPath, autoSave: true);

                _logger.LogInformation(
                    "Practice phase completed for learning path {LearningPathId}.",
                    learningPath.Id);

                // Check if both theory and practice are completed
                if (learningPath.TheoryCompleted && learningPath.PracticeCompleted)
                {
                    // Publish integrated mastery event
                    await _eventBus.PublishAsync(new IntegratedMasteryAchievedEto
                    {
                        LearningPathId = learningPath.Id,
                        TopicId = learningPath.TopicId,
                        SkillId = learningPath.SkillId,
                        UserId = learningPath.UserId,
                        TenantId = learningPath.TenantId,
                        IntegratedMasteryScore = learningPath.IntegratedMasteryScore,
                        TheoryScore = learningPath.TheoryProgressPercentage,
                        PracticeScore = learningPath.PracticeProgressPercentage,
                        AchievedAt = DateTime.UtcNow
                    });

                    _logger.LogInformation(
                        "Integrated mastery achieved for learning path {LearningPathId}! Score: {Score}",
                        learningPath.Id, learningPath.IntegratedMasteryScore);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling SkillMasteredEto for skill {SkillId}",
                    eventData.SkillId);
                throw;
            }
        }
    }
}
