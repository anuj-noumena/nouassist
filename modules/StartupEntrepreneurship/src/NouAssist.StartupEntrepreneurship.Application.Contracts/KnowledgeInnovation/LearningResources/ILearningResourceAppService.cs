using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources
{
    public interface ILearningResourceAppService : IApplicationService
    {
        Task<LearningResourceDto> GetAsync(Guid id);
        
        Task<PagedResultDto<LearningResourceListDto>> GetListAsync(GetLearningResourceListDto input);
        
        Task<LearningResourceDto> CreateAsync(CreateLearningResourceDto input);
        
        Task<LearningResourceDto> UpdateAsync(Guid id, UpdateLearningResourceDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<LearningResourceDto> PublishAsync(Guid id);
        
        Task<LearningResourceDto> UnpublishAsync(Guid id);
        
        Task<LearningResourceDto> SetFeaturedAsync(Guid id, bool isFeatured);
        
        Task<UserProgressDto> UpdateProgressAsync(Guid id, UpdateProgressDto input);
        
        Task<UserProgressDto> MarkCompletedAsync(Guid id);
        
        Task<UserProgressDto> MarkAbandonedAsync(Guid id);
        
        Task<List<LearningResourceListDto>> GetCompletedResourcesAsync();
        
        Task<List<LearningResourceListDto>> GetInProgressResourcesAsync();
        
        Task<List<LearningResourceListDto>> GetRecommendedResourcesAsync(int maxResults = 5);
        
        Task<LearningStatsDto> GetLearningStatsAsync();
    }
}
