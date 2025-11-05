using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Aggregates;
using NouAssist.SharedKernel.Data;
using NouAssist.SharedKernel.Tags.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.SharedKernel.Tags
{
    [Authorize]
    public class TagAppService : ApplicationService, ITagAppService
    {
        private readonly ITagRepository _tagRepository;

        public TagAppService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<ListResultDto<TagDto>> GetMyTagsAsync()
        {
            var userId = CurrentUser.GetId();
            var tags = await _tagRepository.GetByUserAsync(userId);
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
                tag.Color = input.Color;

            if (input.CategoryId.HasValue)
                tag.AssignToCategory(input.CategoryId.Value);

            await _tagRepository.InsertAsync(tag);
            return ObjectMapper.Map<Tag, TagDto>(tag);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _tagRepository.DeleteAsync(id);
        }
    }
}
