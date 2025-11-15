using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Data;
using NouAssist.Learning.Mindmaps.Dtos;
using NouAssist.Learning.Permissions;
using Volo.Abp.Application.Services;

namespace NouAssist.Learning.Mindmaps
{
    [Authorize(LearningPermissions.Mindmaps.Default)]
    public class MindmapAppService : ApplicationService, IMindmapAppService
    {
        private readonly IMindmapRepository _mindmapRepository;

        public MindmapAppService(IMindmapRepository mindmapRepository)
        {
            _mindmapRepository = mindmapRepository;
        }

        public async Task<MindmapDto> GetAsync(Guid id)
        {
            var mindmap = await _mindmapRepository.GetAsync(id);
            return ObjectMapper.Map<Mindmap, MindmapDto>(mindmap);
        }

        public async Task<MindmapDto> GetByTopicIdAsync(Guid topicId)
        {
            var mindmap = await _mindmapRepository.GetByTopicIdAsync(topicId);
            return ObjectMapper.Map<Mindmap, MindmapDto>(mindmap);
        }

        [Authorize(LearningPermissions.Mindmaps.Create)]
        public async Task<MindmapDto> CreateAsync(CreateMindmapDto input)
        {
            var mindmap = new Mindmap(
                GuidGenerator.Create(),
                CurrentUser.GetId(),
                input.TopicId,
                CurrentTenant.Id
            );

            mindmap.UpdateData(input.Data);
            mindmap.UpdateUrl(input.Url);

            await _mindmapRepository.InsertAsync(mindmap);
            return ObjectMapper.Map<Mindmap, MindmapDto>(mindmap);
        }

        [Authorize(LearningPermissions.Mindmaps.Update)]
        public async Task<MindmapDto> UpdateAsync(Guid id, UpdateMindmapDto input)
        {
            var mindmap = await _mindmapRepository.GetAsync(id);

            if (input.Data != null)
            {
                mindmap.UpdateData(input.Data);
            }

            if (!string.IsNullOrWhiteSpace(input.Url))
            {
                mindmap.UpdateUrl(input.Url);
            }

            await _mindmapRepository.UpdateAsync(mindmap);
            return ObjectMapper.Map<Mindmap, MindmapDto>(mindmap);
        }

        [Authorize(LearningPermissions.Mindmaps.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _mindmapRepository.DeleteAsync(id);
        }
    }
}
