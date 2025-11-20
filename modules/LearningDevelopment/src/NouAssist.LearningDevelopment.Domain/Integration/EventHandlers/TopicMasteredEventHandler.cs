using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Learning.Events;
using NouAssist.LearningDevelopment.Integration.Data;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.LearningDevelopment.Integration.EventHandlers
{
    /// <summary>
    /// Handles TopicMasteredEto to trigger skill development
    /// </summary>
    public class TopicMasteredEventHandler :
        IDistributedEventHandler<TopicMasteredEto>,
        ITransientDependency
    {
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly ISkillDevelopmentSessionRepository _skillSessionRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ILogger<TopicMasteredEventHandler> _logger;

        public TopicMasteredEventHandler(
            ILearningPathRepository learningPathRepository,
            ISkillDevelopmentSessionRepository skillSessionRepository,
            ISkillRepository skillRepository,
            ILogger<TopicMasteredEventHandler> logger)
        {
            _learningPathRepository = learningPathRepository;
            _skillSessionRepository = skillSessionRepository;
            _skillRepository = skillRepository;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(TopicMasteredEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Topic {TopicId} mastered by user {UserId}. Checking for learning path integration.",
                    eventData.TopicId, eventData.UserId);

                // Find learning path associated with this topic
                var learningPath = await _learningPathRepository.FindByTopicIdAsync(eventData.TopicId);
                if (learningPath == null)
                {
                    _logger.LogDebug(
                        "No learning path found for topic {TopicId}. Skipping skill development trigger.",
                        eventData.TopicId);
                    return;
                }

                // Complete theory phase
                learningPath.CompleteTheoryPhase();
                await _learningPathRepository.UpdateAsync(learningPath, autoSave: true);

                _logger.LogInformation(
                    "Theory phase completed for learning path {LearningPathId}. Ready for practice phase.",
                    learningPath.Id);

                // Check if skill development session already exists
                if (learningPath.SkillDevelopmentSessionId.HasValue)
                {
                    _logger.LogDebug(
                        "Skill development session already exists for learning path {LearningPathId}.",
                        learningPath.Id);
                    return;
                }

                // Auto-create skill development session
                var skill = await _skillRepository.GetAsync(learningPath.SkillId);
                var session = new SkillDevelopmentSession(
                    Guid.NewGuid(),
                    learningPath.UserId,
                    learningPath.SkillId,
                    learningPath.TenantId);

                await _skillSessionRepository.InsertAsync(session, autoSave: true);

                // Associate session with learning path
                learningPath.AssociateSkillDevelopmentSession(session.Id);
                await _learningPathRepository.UpdateAsync(learningPath, autoSave: true);

                // Start skill development
                skill.StartDevelopment(session.Id);
                await _skillRepository.UpdateAsync(skill, autoSave: true);

                _logger.LogInformation(
                    "Auto-created skill development session {SessionId} for skill {SkillId} after topic mastery.",
                    session.Id, skill.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling TopicMasteredEto for topic {TopicId}",
                    eventData.TopicId);
                throw;
            }
        }
    }
}
