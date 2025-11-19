using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Core.Aggregates;
using NouAssist.Core.Data;
using NouAssist.Core.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Core.Tags
{
    [Authorize]
    public class TagAppService : ApplicationService, ITagAppService
    {
        private readonly ITagRepository _tagRepository;

        public TagAppService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<TagDto> GetAsync(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);
            return ObjectMapper.Map<Tag, TagDto>(tag);
        }

        public async Task<ListResultDto<TagDto>> GetMyTagsAsync()
        {
            var userId = CurrentUser.GetId();
            var tags = await _tagRepository.GetByUserAsync(userId);
            var dtos = ObjectMapper.Map<List<Tag>, List<TagDto>>(tags);
            return new ListResultDto<TagDto>(dtos);
        }

        public async Task<ListResultDto<TagDto>> GetPopularTagsAsync(int minUsageCount = 10)
        {
            var userId = CurrentUser.GetId();
            var tags = await _tagRepository.GetPopularTagsAsync(userId, minUsageCount);
            var dtos = ObjectMapper.Map<List<Tag>, List<TagDto>>(tags);
            return new ListResultDto<TagDto>(dtos);
        }

        public async Task<TagDto> CreateAsync(CreateTagDto input)
        {
            var userId = CurrentUser.GetId();
            var tag = new Tag(
                GuidGenerator.Create(),
                userId,
                input.Title,
                CurrentTenant.Id
            );

            if (!string.IsNullOrWhiteSpace(input.Color))
            {
                tag.Color = input.Color;
            }

            if (input.CategoryId.HasValue)
            {
                tag.AssignToCategory(input.CategoryId.Value);
            }

            await _tagRepository.InsertAsync(tag);
            return ObjectMapper.Map<Tag, TagDto>(tag);
        }

        public async Task<TagDto> UpdateAsync(Guid id, UpdateTagDto input)
        {
            var tag = await _tagRepository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(input.Title))
            {
                tag.Title = input.Title;
            }

            if (input.Color != null)
            {
                tag.Color = input.Color;
            }

            if (input.CategoryId.HasValue)
            {
                tag.AssignToCategory(input.CategoryId.Value);
            }
            else if (input.CategoryId == null)
            {
                tag.CategoryId = null;
            }

            await _tagRepository.UpdateAsync(tag);
            return ObjectMapper.Map<Tag, TagDto>(tag);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _tagRepository.DeleteAsync(id);
        }

        public async Task IncrementUsageAsync(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);
            tag.IncrementUsage();
            await _tagRepository.UpdateAsync(tag);
        }
    }
}
