using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.LearningDevelopment.Integration.Aggregates;
using NouAssist.LearningDevelopment.Integration.Data;
using NouAssist.LearningDevelopment.Integration.Enums;
using NouAssist.LearningDevelopment.Integration.LearningPaths.Dtos;
using NouAssist.LearningDevelopment.Integration.Services;
using NouAssist.LearningDevelopment.Learning.Data;
using NouAssist.LearningDevelopment.SkillDevelopment.Data;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.LearningDevelopment.Integration.LearningPaths
{
    [Authorize]
    public class LearningPathAppService : ApplicationService, ILearningPathAppService
    {
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly LearningSkillIntegrationService _integrationService;
        private readonly ICurrentUser _currentUser;

        public LearningPathAppService(
            ILearningPathRepository learningPathRepository,
            ITopicRepository topicRepository,
            ISkillRepository skillRepository,
            LearningSkillIntegrationService integrationService,
            ICurrentUser currentUser)
        {
            _learningPathRepository = learningPathRepository;
            _topicRepository = topicRepository;
            _skillRepository = skillRepository;
            _integrationService = integrationService;
            _currentUser = currentUser;
        }

        public async Task<LearningPathDto> CreateAsync(CreateLearningPathDto input)
        {
            var userId = _currentUser.GetId();

            var learningPath = await _integrationService.CreateIntegratedLearningPathAsync(
                userId,
                input.Title,
                input.TopicId,
                input.SkillId,
                _currentUser.TenantId);

            return await MapToDto(learningPath);
        }

        public async Task<LearningPathDto> GetAsync(Guid id)
        {
            var learningPath = await _learningPathRepository.GetAsync(id);
            return await MapToDto(learningPath);
        }

        public async Task<List<LearningPathDto>> GetListAsync()
        {
            var userId = _currentUser.GetId();
            var learningPaths = await _learningPathRepository.GetByUserIdAsync(userId);

            var dtos = new List<LearningPathDto>();
            foreach (var path in learningPaths)
            {
                dtos.Add(await MapToDto(path));
            }

            return dtos;
        }

        public async Task<List<LearningPathDto>> GetByStatusAsync(LearningPathStatus status)
        {
            var userId = _currentUser.GetId();
            var learningPaths = await _learningPathRepository.GetByStatusAsync(userId, status);

            var dtos = new List<LearningPathDto>();
            foreach (var path in learningPaths)
            {
                dtos.Add(await MapToDto(path));
            }

            return dtos;
        }

        public async Task<LearningPathDto> StartAsync(Guid id)
        {
            var learningPath = await _learningPathRepository.GetAsync(id);

            await _integrationService.StartLearningPathAsync(id);

            // Reload to get updated state
            learningPath = await _learningPathRepository.GetAsync(id);
            return await MapToDto(learningPath);
        }

        public async Task<LearningPathDto> PutOnHoldAsync(Guid id)
        {
            var learningPath = await _learningPathRepository.GetAsync(id);
            learningPath.PutOnHold();
            await _learningPathRepository.UpdateAsync(learningPath);

            return await MapToDto(learningPath);
        }

        public async Task<LearningPathDto> ResumeAsync(Guid id)
        {
            var learningPath = await _learningPathRepository.GetAsync(id);
            learningPath.Resume();
            await _learningPathRepository.UpdateAsync(learningPath);

            return await MapToDto(learningPath);
        }

        public async Task<LearningPathDto> AbandonAsync(Guid id, string reason = null)
        {
            var learningPath = await _learningPathRepository.GetAsync(id);
            learningPath.Abandon(reason);
            await _learningPathRepository.UpdateAsync(learningPath);

            return await MapToDto(learningPath);
        }

        public async Task<LearningPathDto> EnterMaintenanceAsync(Guid id)
        {
            var learningPath = await _learningPathRepository.GetAsync(id);
            learningPath.EnterMaintenancePhase();
            await _learningPathRepository.UpdateAsync(learningPath);

            return await MapToDto(learningPath);
        }

        public async Task LinkTopicToSkillAsync(Guid topicId, Guid skillId)
        {
            await _integrationService.LinkTopicToSkillAsync(topicId, skillId);
        }

        public async Task UnlinkTopicFromSkillAsync(Guid topicId, Guid skillId)
        {
            await _integrationService.UnlinkTopicFromSkillAsync(topicId, skillId);
        }

        public async Task<IntegrationStatusDto> GetIntegrationStatusAsync(Guid topicId, Guid skillId)
        {
            var status = await _integrationService.GetIntegrationStatusAsync(topicId, skillId);

            return new IntegrationStatusDto
            {
                IsLinked = status.IsLinked,
                HasLearningPath = status.HasLearningPath,
                LearningPathId = status.LearningPathId,
                TopicProgress = status.TopicProgress,
                SkillProficiency = status.SkillProficiency,
                TopicStatus = status.TopicStatus,
                SkillStatus = status.SkillStatus,
                OverallProgress = status.OverallProgress
            };
        }

        public async Task<LearningPathDto> GetByTopicIdAsync(Guid topicId)
        {
            var learningPath = await _learningPathRepository.FindByTopicIdAsync(topicId);
            if (learningPath == null)
            {
                return null;
            }

            return await MapToDto(learningPath);
        }

        public async Task<LearningPathDto> GetBySkillIdAsync(Guid skillId)
        {
            var learningPath = await _learningPathRepository.FindBySkillIdAsync(skillId);
            if (learningPath == null)
            {
                return null;
            }

            return await MapToDto(learningPath);
        }

        private async Task<LearningPathDto> MapToDto(LearningPath learningPath)
        {
            var topic = await _topicRepository.GetAsync(learningPath.TopicId);
            var skill = await _skillRepository.GetAsync(learningPath.SkillId);

            return new LearningPathDto
            {
                Id = learningPath.Id,
                UserId = learningPath.UserId,
                Title = learningPath.Title,
                Description = learningPath.Description,
                Status = learningPath.Status,
                CurrentPhase = learningPath.CurrentPhase,
                TopicId = learningPath.TopicId,
                TopicTitle = topic.Title,
                LearningSessionId = learningPath.LearningSessionId,
                CurrentLearningPhase = learningPath.CurrentLearningPhase,
                TheoryCompleted = learningPath.TheoryCompleted,
                TheoryCompletedAt = learningPath.TheoryCompletedAt,
                SkillId = learningPath.SkillId,
                SkillTitle = skill.Title,
                SkillDevelopmentSessionId = learningPath.SkillDevelopmentSessionId,
                CurrentDevelopmentStep = learningPath.CurrentDevelopmentStep,
                PracticeCompleted = learningPath.PracticeCompleted,
                PracticeCompletedAt = learningPath.PracticeCompletedAt,
                TheoryProgressPercentage = learningPath.TheoryProgressPercentage,
                PracticeProgressPercentage = learningPath.PracticeProgressPercentage,
                OverallProgressPercentage = learningPath.OverallProgressPercentage,
                IntegratedMasteryScore = learningPath.IntegratedMasteryScore,
                CompletedAt = learningPath.CompletedAt,
                LastActivity = learningPath.LastActivity,
                Milestones = learningPath.Milestones.ToList(),
                CreationTime = learningPath.CreationTime,
                CreatorId = learningPath.CreatorId,
                LastModificationTime = learningPath.LastModificationTime,
                LastModifierId = learningPath.LastModifierId
            };
        }
    }
}
