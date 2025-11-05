using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Aggregates;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.SharedKernel.Tags
{
    [Authorize]
    public class TagCategoryAppService : ApplicationService, ITagCategoryAppService
    {
        private readonly IRepository<TagCategory, Guid> _categoryRepository;

        public TagCategoryAppService(IRepository<TagCategory, Guid> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ListResultDto<TagCategoryDto>> GetMyCategoriesAsync()
        {
            var userId = CurrentUser.GetId();
            var queryable = await _categoryRepository.GetQueryableAsync();
            var categories = await AsyncExecuter.ToListAsync(
                queryable.Where(c => c.OwnerId == userId).OrderBy(c => c.Name));
            
            var dtos = ObjectMapper.Map<List<TagCategory>, List<TagCategoryDto>>(categories);
            return new ListResultDto<TagCategoryDto>(dtos);
        }

        public async Task<TagCategoryDto> CreateAsync(CreateTagCategoryDto input)
        {
            var userId = CurrentUser.GetId();
            
            var category = new TagCategory(
                GuidGenerator.Create(),
                userId,
                input.Name,
                CurrentTenant.Id
            );

            if (!string.IsNullOrWhiteSpace(input.Color))
                category.Color = input.Color;

            await _categoryRepository.InsertAsync(category);
            return ObjectMapper.Map<TagCategory, TagCategoryDto>(category);
        }

        public async Task<TagCategoryDto> UpdateAsync(Guid id, UpdateTagCategoryDto input)
        {
            var category = await _categoryRepository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(input.Name))
                category.Name = input.Name;

            if (input.Color != null)
                category.Color = input.Color;

            await _categoryRepository.UpdateAsync(category);
            return ObjectMapper.Map<TagCategory, TagCategoryDto>(category);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}
