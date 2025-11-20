using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets
{
    public interface IKnowledgeAssetAppService : IApplicationService
    {
        Task<KnowledgeAssetDto> GetAsync(Guid id);
        
        Task<PagedResultDto<KnowledgeAssetListDto>> GetListAsync(GetKnowledgeAssetListDto input);
        
        Task<KnowledgeAssetDto> CreateAsync(CreateKnowledgeAssetDto input);
        
        Task<KnowledgeAssetDto> UpdateAsync(Guid id, UpdateKnowledgeAssetDto input);
        
        Task DeleteAsync(Guid id);
        
        Task<KnowledgeAssetDto> UpdateContentAsync(Guid id, UpdateContentDto input);
        
        Task<AssetContentDto> GetContentVersionAsync(Guid id, int version);
        
        Task<List<KnowledgeAssetListDto>> GetRecommendedAssetsAsync(int maxResults = 5);
        
        Task<List<KnowledgeAssetListDto>> GetByTypeAsync(KnowledgeAssetType assetType);
        
        Task<List<KnowledgeAssetListDto>> GetByTagAsync(string tag);
    }
}
