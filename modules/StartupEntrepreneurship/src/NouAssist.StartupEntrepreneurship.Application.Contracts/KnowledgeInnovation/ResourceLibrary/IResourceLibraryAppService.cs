using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources.Dtos;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ResourceLibrary.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ResourceLibrary
{
    public interface IResourceLibraryAppService
    {
        Task<List<KnowledgeAssetListDto>> SearchKnowledgeAssetsAsync(SearchResourcesDto input);
        
        Task<List<LearningResourceListDto>> SearchLearningResourcesAsync(SearchResourcesDto input);
        
        Task<List<string>> AddTagsAsync(string entityType, Guid entityId, List<string> tags);
        
        Task<List<string>> GetAllTagsAsync();
        
        Task<List<string>> GetPopularTagsAsync(int maxTags = 20);
    }
}
