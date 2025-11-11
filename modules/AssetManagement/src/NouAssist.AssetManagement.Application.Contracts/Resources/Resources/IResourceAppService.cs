using System;
using System.Threading.Tasks;
using NouAssist.AssetManagement.Resources.Resources.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.AssetManagement.Resources.Resources
{
    public interface IResourceAppService : IApplicationService
    {
        Task<ResourceDto> GetAsync(Guid id);
        Task<PagedResultDto<ResourceDto>> GetListAsync(GetResourceListDto input);
        Task<ResourceDto> UploadAsync(UploadResourceDto input);
        Task<ResourceDto> UpdateAsync(Guid id, UpdateResourceDto input);
        Task DeleteAsync(Guid id);
        Task<byte[]> DownloadAsync(Guid id);
        Task<ResourceDto> UploadNewVersionAsync(Guid id, UploadResourceDto input);
        Task<ResourceDto> GrantAccessAsync(Guid id, GrantAccessDto input);
        Task RevokeAccessAsync(Guid id, Guid userId);
        Task<long> GetTotalStorageUsedAsync();
    }
}
