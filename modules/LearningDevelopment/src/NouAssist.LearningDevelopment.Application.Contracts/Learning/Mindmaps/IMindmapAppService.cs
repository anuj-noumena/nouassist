using System;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.Learning.Mindmaps.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.LearningDevelopment.Learning.Mindmaps
{
    public interface IMindmapAppService : IApplicationService
    {
        Task<MindmapDto> GetAsync(Guid id);
        Task<MindmapDto> GetByTopicIdAsync(Guid topicId);
        Task<MindmapDto> CreateAsync(CreateMindmapDto input);
        Task<MindmapDto> UpdateAsync(Guid id, UpdateMindmapDto input);
        Task DeleteAsync(Guid id);
    }
}
