using System;
using System.Threading.Tasks;
using NouAssist.Core.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Core.Tags
{
    public interface ITagAppService : IApplicationService
    {
        Task<TagDto> GetAsync(Guid id);
        Task<ListResultDto<TagDto>> GetMyTagsAsync();
        Task<ListResultDto<TagDto>> GetPopularTagsAsync(int minUsageCount = 10);
        Task<TagDto> CreateAsync(CreateTagDto input);
        Task<TagDto> UpdateAsync(Guid id, UpdateTagDto input);
        Task DeleteAsync(Guid id);
        Task IncrementUsageAsync(Guid id);
    }

    public interface ITagCategoryAppService : IApplicationService
    {
        Task<ListResultDto<TagCategoryDto>> GetMyCategor iesAsync();
        Task<TagCategoryDto> CreateAsync(CreateTagCategoryDto input);
        Task<TagCategoryDto> UpdateAsync(Guid id, UpdateTagCategoryDto input);
        Task DeleteAsync(Guid id);
    }
}
