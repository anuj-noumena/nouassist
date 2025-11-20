using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.LearningDevelopment.Integration.Aggregates;
using NouAssist.LearningDevelopment.Integration.Data;
using NouAssist.LearningDevelopment.Integration.Events;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.Learning.Data;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.Data;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;

namespace NouAssist.LearningDevelopment.Integration.Services
{
    /// <summary>
    /// Domain service for managing integration between Learning and SkillDevelopment
    /// </summary>
    public class LearningSkillIntegrationService : DomainService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly ILearningSessionRepository _learningSessionRepository;
        private readonly ISkillDevelopmentSessionRepository _skillSessionRepository;
        private readonly IDistributedEventBus _eventBus;
        private readonly ILogger<LearningSkillIntegrationService> _logger;

        public LearningSkillIntegrationService(
            ITopicRepository topicRepository,
            ISkillRepository skillRepository,
            ILearningPathRepository learningPathRepository,
            ILearningSessionRepository learningSessionRepository,
            ISkillDevelopmentSessionRepository skillSessionRepository,
            IDistributedEventBus eventBus,
            ILogger<LearningSkillIntegrationService> logger)
        {
            _topicRepository = topicRepository;
            _skillRepository = skillRepository;
            _learningPathRepository = learningPathRepository;
            _learningSessionRepository = learningSessionRepository;
            _skillSessionRepository = skillSessionRepository;
            _eventBus = eventBus;
            _logger = logger;
        }

        /// <summary>
        /// Link a Topic to a Skill (bi-directional)
        /// </summary>
        public async Task LinkTopicToSkillAsync(Guid topicId, Guid skillId)
        {
            var topic = await _topicRepository.GetAsync(topicId);
            var skill = await _skillRepository.GetAsync(skillId);

            if (topic.IsFolder)
            {
                throw new BusinessException("Integration:CannotLinkFolderToSkill");
            }

            // Validate same user
            if (topic.UserId != skill.UserId)
            {
                throw new BusinessException("Integration:TopicAndSkillMustBelongToSameUser");
            }

            // Link both ways
            topic.LinkToSkill(skillId);
            skill.LinkToTopic(topicId);

            await _topicRepository.UpdateAsync(topic, autoSave: false);
            await _skillRepository.UpdateAsync(skill, autoSave: true);

            // Publish event
            await _eventBus.PublishAsync(new TopicLinkedToSkillEto
            {
                TopicId = topicId,
                SkillId = skillId,
                UserId = topic.UserId,
                TenantId = topic.TenantId,
                LinkedAt = DateTime.UtcNow
            });

            _logger.LogInformation(
                "Linked Topic {TopicId} to Skill {SkillId} for user {UserId}",
                topicId, skillId, topic.UserId);
        }

        /// <summary>
        /// Unlink a Topic from a Skill (bi-directional)
        /// </summary>
        public async Task UnlinkTopicFromSkillAsync(Guid topicId, Guid skillId)
        {
            var topic = await _topicRepository.GetAsync(topicId);
            var skill = await _skillRepository.GetAsync(skillId);

            topic.UnlinkFromSkill();
            skill.UnlinkFromTopic();

            await _topicRepository.UpdateAsync(topic, autoSave: false);
            await _skillRepository.UpdateAsync(skill, autoSave: true);

            _logger.LogInformation(
                "Unlinked Topic {TopicId} from Skill {SkillId}",
                topicId, skillId);
        }

        /// <summary>
        /// Create a unified LearningPath that orchestrates both Topic and Skill
        /// </summary>
        public async Task<LearningPath> CreateIntegratedLearningPathAsync(
            Guid userId,
            string title,
            Guid topicId,
            Guid skillId,
            Guid? tenantId = null)
        {
            var topic = await _topicRepository.GetAsync(topicId);
            var skill = await _skillRepository.GetAsync(skillId);

            // Validate ownership
            if (topic.UserId != userId || skill.UserId != userId)
            {
                throw new BusinessException("Integration:TopicAndSkillMustBelongToUser");
            }

            if (topic.IsFolder)
            {
                throw new BusinessException("Integration:CannotCreatePathWithFolder");
            }

            // Check if learning path already exists
            var existingPath = await _learningPathRepository.FindByTopicIdAsync(topicId);
            if (existingPath != null)
            {
                throw new BusinessException("Integration:LearningPathAlreadyExists")
                    .WithData("ExistingPathId", existingPath.Id);
            }

            // Create learning path
            var learningPath = new LearningPath(
                GuidGenerator.Create(),
                userId,
                title,
                topicId,
                skillId,
                tenantId);

            await _learningPathRepository.InsertAsync(learningPath, autoSave: false);

            // Link topic and skill
            topic.LinkToSkill(skillId);
            skill.LinkToTopic(topicId);

            await _topicRepository.UpdateAsync(topic, autoSave: false);
            await _skillRepository.UpdateAsync(skill, autoSave: true);

            // Publish event
            await _eventBus.PublishAsync(new LearningPathCreatedEto
            {
                LearningPathId = learningPath.Id,
                UserId = userId,
                TenantId = tenantId,
                TopicId = topicId,
                SkillId = skillId,
                Title = title,
                CreatedAt = DateTime.UtcNow
            });

            _logger.LogInformation(
                "Created integrated learning path {PathId} for Topic {TopicId} and Skill {SkillId}",
                learningPath.Id, topicId, skillId);

            return learningPath;
        }

        /// <summary>
        /// Start a learning path by creating learning session
        /// </summary>
        public async Task<LearningSession> StartLearningPathAsync(Guid learningPathId)
        {
            var learningPath = await _learningPathRepository.GetAsync(learningPathId);

            if (learningPath.LearningSessionId.HasValue)
            {
                throw new BusinessException("Integration:LearningSessionAlreadyExists");
            }

            // Create learning session
            var learningSession = new LearningSession(
                GuidGenerator.Create(),
                learningPath.UserId,
                learningPath.TopicId,
                learningPath.TenantId);

            await _learningSessionRepository.InsertAsync(learningSession, autoSave: false);

            // Associate with learning path and start
            learningPath.AssociateLearningSession(learningSession.Id);
            learningPath.Start();

            // Update topic to associate session
            var topic = await _topicRepository.GetAsync(learningPath.TopicId);
            topic.AssociateLearningSession(learningSession.Id);
            topic.StartLearning();

            await _topicRepository.UpdateAsync(topic, autoSave: false);
            await _learningPathRepository.UpdateAsync(learningPath, autoSave: true);

            _logger.LogInformation(
                "Started learning path {PathId} with session {SessionId}",
                learningPathId, learningSession.Id);

            return learningSession;
        }

        /// <summary>
        /// Suggest creating a Skill for a Topic that has no linked skill
        /// </summary>
        public async Task<Skill> SuggestSkillForTopicAsync(Guid topicId)
        {
            var topic = await _topicRepository.GetAsync(topicId);

            if (topic.RelatedSkillId.HasValue)
            {
                throw new BusinessException("Integration:TopicAlreadyLinkedToSkill");
            }

            if (topic.IsFolder)
            {
                throw new BusinessException("Integration:CannotSuggestSkillForFolder");
            }

            // Create suggested skill based on topic
            var skill = new Skill(
                GuidGenerator.Create(),
                topic.UserId,
                $"{topic.Title} (Practice)",
                NouAssist.SkillDevelopment.Enums.ProficiencyLevel.Intermediate, // Default target
                topic.EstimatedHours, // Use same estimated hours
                topic.TenantId,
                null,
                false);

            skill.UpdateDescription($"Practical skill development for: {topic.Description}");
            skill.SetCategory(topic.Category);

            await _skillRepository.InsertAsync(skill, autoSave: true);

            _logger.LogInformation(
                "Suggested and created skill {SkillId} for topic {TopicId}",
                skill.Id, topicId);

            return skill;
        }

        /// <summary>
        /// Get integration status between a topic and skill
        /// </summary>
        public async Task<IntegrationStatus> GetIntegrationStatusAsync(Guid topicId, Guid skillId)
        {
            var topic = await _topicRepository.GetAsync(topicId);
            var skill = await _skillRepository.GetAsync(skillId);
            var learningPath = await _learningPathRepository.FindByTopicIdAsync(topicId);

            return new IntegrationStatus
            {
                IsLinked = topic.RelatedSkillId == skillId && skill.RelatedTopicId == topicId,
                HasLearningPath = learningPath != null,
                LearningPathId = learningPath?.Id,
                TopicProgress = topic.Progress,
                SkillProficiency = skill.CurrentProficiency.Score,
                TopicStatus = topic.Status.ToString(),
                SkillStatus = skill.Status.ToString(),
                OverallProgress = learningPath?.OverallProgressPercentage ?? 0
            };
        }
    }

    /// <summary>
    /// Result object for integration status
    /// </summary>
    public class IntegrationStatus
    {
        public bool IsLinked { get; set; }
        public bool HasLearningPath { get; set; }
        public Guid? LearningPathId { get; set; }
        public int TopicProgress { get; set; }
        public int SkillProficiency { get; set; }
        public string TopicStatus { get; set; }
        public string SkillStatus { get; set; }
        public int OverallProgress { get; set; }
    }
}
