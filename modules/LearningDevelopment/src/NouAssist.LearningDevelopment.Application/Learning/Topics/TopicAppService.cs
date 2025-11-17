using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.Learning.Data;
using NouAssist.LearningDevelopment.Learning.Permissions;
using NouAssist.LearningDevelopment.Learning.Services;
using NouAssist.LearningDevelopment.Learning.Specifications;
using NouAssist.LearningDevelopment.Learning.Topics.Dtos;
using NouAssist.LearningDevelopment.Learning.ValueObjects;
using NouAssist.SharedKernel.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Specifications;
using Volo.Abp.Users;

namespace NouAssist.LearningDevelopment.Learning.Topics
{
    [Authorize(LearningPermissions.Topics.Default)]
    public class TopicAppService : ApplicationService, ITopicAppService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ILearningSessionRepository _sessionRepository;
        private readonly LearningProgressService _progressService;

        public TopicAppService(
            ITopicRepository topicRepository,
            ILearningSessionRepository sessionRepository,
            LearningProgressService progressService)
        {
            _topicRepository = topicRepository;
            _sessionRepository = sessionRepository;
            _progressService = progressService;
        }

        public async Task<TopicDto> GetAsync(Guid id)
        {
            var topic = await _topicRepository.GetAsync(id);
            return ObjectMapper.Map<Topic, TopicDto>(topic);
        }

        public async Task<TopicDto> GetWithChildrenAsync(Guid id)
        {
            var topic = await _topicRepository.GetWithChildrenAsync(id);
            return ObjectMapper.Map<Topic, TopicDto>(topic);
        }

        public async Task<PagedResultDto<TopicDto>> GetListAsync(GetTopicListDto input)
        {
            var spec = CreateListSpecification(input);

            var topics = await _topicRepository.GetUserTopicsAsync(
                CurrentUser.GetId(), spec);

            var totalCount = topics.Count; // For simplicity, not implementing separate count query

            // Apply paging
            var pagedTopics = topics
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<TopicDto>(
                totalCount,
                ObjectMapper.Map<List<Topic>, List<TopicDto>>(pagedTopics)
            );
        }

        public async Task<ListResultDto<TopicDto>> GetRootTopicsAsync()
        {
            var topics = await _topicRepository.GetRootTopicsAsync(CurrentUser.GetId());
            return new ListResultDto<TopicDto>(
                ObjectMapper.Map<List<Topic>, List<TopicDto>>(topics)
            );
        }

        public async Task<ListResultDto<TopicDto>> GetChildrenAsync(Guid parentId)
        {
            var topics = await _topicRepository.GetChildrenAsync(parentId);
            return new ListResultDto<TopicDto>(
                ObjectMapper.Map<List<Topic>, List<TopicDto>>(topics)
            );
        }

        [Authorize(LearningPermissions.Topics.Create)]
        public async Task<TopicDto> CreateAsync(CreateTopicDto input)
        {
            // Validate parent if specified
            if (input.ParentId.HasValue)
            {
                var parent = await _topicRepository.GetAsync(input.ParentId.Value);
                if (!parent.IsFolder)
                {
                    throw new BusinessException("Learning:CannotAddChildToNonFolder");
                }
            }

            var topic = new Topic(
                GuidGenerator.Create(),
                CurrentUser.GetId(),
                input.Title,
                input.IsFolder,
                input.Difficulty,
                input.EstimatedHours,
                CurrentTenant.Id,
                input.ParentId
            );

            topic.UpdateDescription(input.Description);
            topic.UpdateCategory(input.Category);
            topic.UpdateLearningGoals(input.LearningGoals);

            foreach (var tag in input.Tags ?? new List<string>())
            {
                topic.AddTag(tag);
            }

            await _topicRepository.InsertAsync(topic);
            return ObjectMapper.Map<Topic, TopicDto>(topic);
        }

        [Authorize(LearningPermissions.Topics.Update)]
        public async Task<TopicDto> UpdateAsync(Guid id, UpdateTopicDto input)
        {
            var topic = await _topicRepository.GetAsync(id);

            topic.UpdateTitle(input.Title);
            topic.UpdateDescription(input.Description);
            topic.UpdateCategory(input.Category);
            topic.UpdateDifficulty(input.Difficulty);
            topic.UpdateEstimatedHours(input.EstimatedHours);
            topic.UpdateLearningGoals(input.LearningGoals);

            // Update tags (clear and re-add)
            // Note: In production, you'd need a more sophisticated approach
            foreach (var tag in input.Tags ?? new List<string>())
            {
                topic.AddTag(tag);
            }

            await _topicRepository.UpdateAsync(topic);
            return ObjectMapper.Map<Topic, TopicDto>(topic);
        }

        [Authorize(LearningPermissions.Topics.Update)]
        public async Task<TopicDto> UpdateStatusAsync(Guid id, UpdateTopicStatusDto input)
        {
            var topic = await _topicRepository.GetAsync(id);
            topic.UpdateStatus(input.Status);
            await _topicRepository.UpdateAsync(topic);
            return ObjectMapper.Map<Topic, TopicDto>(topic);
        }

        [Authorize(LearningPermissions.Topics.Update)]
        public async Task<TopicDto> UpdateProgressAsync(Guid id, UpdateTopicProgressDto input)
        {
            var topic = await _topicRepository.GetAsync(id);
            topic.UpdateProgress(input.Progress);
            await _topicRepository.UpdateAsync(topic);
            return ObjectMapper.Map<Topic, TopicDto>(topic);
        }

        [Authorize(LearningPermissions.Topics.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var topic = await _topicRepository.GetAsync(id);

            // Check if topic has children
            var children = await _topicRepository.GetChildrenAsync(id);
            if (children.Any())
            {
                throw new BusinessException("Learning:CannotDeleteTopicWithChildren",
                    "Cannot delete a topic that has child topics.");
            }

            await _topicRepository.DeleteAsync(topic);
        }

        [Authorize(LearningPermissions.Topics.ManageChildren)]
        public async Task<TopicDto> AddChildAsync(Guid parentId, CreateTopicDto input)
        {
            var parent = await _topicRepository.GetAsync(parentId);

            if (!parent.IsFolder)
            {
                throw new BusinessException("Learning:CannotAddChildToNonFolder");
            }

            var child = new Topic(
                GuidGenerator.Create(),
                CurrentUser.GetId(),
                input.Title,
                input.IsFolder,
                input.Difficulty,
                input.EstimatedHours,
                CurrentTenant.Id,
                parentId
            );

            child.UpdateDescription(input.Description);
            child.UpdateCategory(input.Category);
            child.UpdateLearningGoals(input.LearningGoals);

            parent.AddChild(child);

            await _topicRepository.InsertAsync(child);
            await _topicRepository.UpdateAsync(parent);

            return ObjectMapper.Map<Topic, TopicDto>(child);
        }

        [Authorize(LearningPermissions.Topics.ManageChildren)]
        public async Task RemoveChildAsync(Guid parentId, Guid childId)
        {
            var parent = await _topicRepository.GetWithChildrenAsync(parentId);
            parent.RemoveChild(childId);
            await _topicRepository.UpdateAsync(parent);
        }

        public async Task<TopicMetricsDto> GetMetricsAsync(Guid id)
        {
            var topic = await _topicRepository.GetWithChildrenAsync(id);
            
            LearningSession session = null;
            if (topic.LearningSessionId.HasValue)
            {
                session = await _sessionRepository.GetAsync(topic.LearningSessionId.Value);
            }

            var metrics = _progressService.CalculateTopicMetrics(topic, session);
            return ObjectMapper.Map<TopicMetrics, TopicMetricsDto>(metrics);
        }

        private ISpecification<Topic> CreateListSpecification(GetTopicListDto input)
        {
            ISpecification<Topic> spec = new BaseSpecification<Topic>();

            if (input.Status.HasValue)
            {
                spec = spec.And(new BaseSpecification<Topic>(t => t.Status == input.Status.Value));
            }

            if (input.Difficulty.HasValue)
            {
                spec = spec.And(new TopicsByDifficultySpecification(input.Difficulty.Value));
            }

            if (!string.IsNullOrWhiteSpace(input.Category))
            {
                spec = spec.And(new TopicsByCategorySpecification(input.Category));
            }

            if (input.IsFolder.HasValue)
            {
                spec = spec.And(new BaseSpecification<Topic>(t => t.IsFolder == input.IsFolder.Value));
            }

            if (input.ParentId.HasValue)
            {
                spec = spec.And(new ChildTopicsSpecification(input.ParentId.Value));
            }

            if (input.MinProgress.HasValue)
            {
                spec = spec.And(new TopicsWithProgressAboveSpecification(input.MinProgress.Value));
            }

            if (input.MaxProgress.HasValue)
            {
                spec = spec.And(new BaseSpecification<Topic>(t => t.Progress <= input.MaxProgress.Value));
            }

            if (!string.IsNullOrWhiteSpace(input.SearchKeyword))
            {
                spec = spec.And(new BaseSpecification<Topic>(t => 
                    t.Title.Contains(input.SearchKeyword) || 
                    t.Description.Contains(input.SearchKeyword)));
            }

            return spec;
        }
    }
}
