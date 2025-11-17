using System;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.Learning.Topics.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.LearningDevelopment.Learning.Topics
{
    public interface ITopicAppService : IApplicationService
    {
        Task<TopicDto> GetAsync(Guid id);
        Task<TopicDto> GetWithChildrenAsync(Guid id);
        Task<PagedResultDto<TopicDto>> GetListAsync(GetTopicListDto input);
        Task<ListResultDto<TopicDto>> GetRootTopicsAsync();
        Task<ListResultDto<TopicDto>> GetChildrenAsync(Guid parentId);
        Task<TopicDto> CreateAsync(CreateTopicDto input);
        Task<TopicDto> UpdateAsync(Guid id, UpdateTopicDto input);
        Task<TopicDto> UpdateStatusAsync(Guid id, UpdateTopicStatusDto input);
        Task<TopicDto> UpdateProgressAsync(Guid id, UpdateTopicProgressDto input);
        Task DeleteAsync(Guid id);
        Task<TopicDto> AddChildAsync(Guid parentId, CreateTopicDto input);
        Task RemoveChildAsync(Guid parentId, Guid childId);
        Task<TopicMetricsDto> GetMetricsAsync(Guid id);
    }
}
