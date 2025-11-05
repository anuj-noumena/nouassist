using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Aggregates;
using NouAssist.SharedKernel.Data;
using NouAssist.SharedKernel.Tags.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.SharedKernel.Areas
{
    [Authorize]
    public class AreaAppService : ApplicationService, IAreaAppService
    {
        private readonly IAreaRepository _areaRepository;

        public AreaAppService(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }

        public async Task<AreaDto> GetAsync(Guid id)
        {
            var area = await _areaRepository.GetAsync(id);
            return ObjectMapper.Map<Area, AreaDto>(area);
        }

        public async Task<AreaDto> GetWithChildrenAsync(Guid id)
        {
            var area = await _areaRepository.GetAsync(id);
            var children = await _areaRepository.GetChildAreasAsync(id);
            
            var dto = ObjectMapper.Map<Area, AreaWithChildrenDto>(area);
            dto.Children = ObjectMapper.Map<List<Area>, List<AreaDto>>(children);
            
            return dto;
        }

        public async Task<ListResultDto<AreaDto>> GetMyAreasAsync()
        {
            var userId = CurrentUser.GetId();
            var areas = await _areaRepository.GetByUserAsync(userId);
            var dtos = ObjectMapper.Map<List<Area>, List<AreaDto>>(areas);
            return new ListResultDto<AreaDto>(dtos);
        }

        public async Task<ListResultDto<AreaDto>> GetRootAreasAsync()
        {
            var userId = CurrentUser.GetId();
            var areas = await _areaRepository.GetRootAreasAsync(userId);
            var dtos = ObjectMapper.Map<List<Area>, List<AreaDto>>(areas);
            return new ListResultDto<AreaDto>(dtos);
        }

        public async Task<ListResultDto<AreaDto>> GetChildAreasAsync(Guid parentId)
        {
            var areas = await _areaRepository.GetChildAreasAsync(parentId);
            var dtos = ObjectMapper.Map<List<Area>, List<AreaDto>>(areas);
            return new ListResultDto<AreaDto>(dtos);
        }

        public async Task<AreaDto> CreateAsync(CreateAreaDto input)
        {
            var userId = CurrentUser.GetId();
            
            var area = new Area(
                GuidGenerator.Create(),
                userId,
                input.Title,
                CurrentTenant.Id
            );

            if (!string.IsNullOrWhiteSpace(input.Description))
                area.Description = input.Description;

            if (input.ParentId.HasValue)
                area.AssignParent(input.ParentId.Value);

            if (!string.IsNullOrWhiteSpace(input.Purpose))
                area.Purpose = input.Purpose;

            await _areaRepository.InsertAsync(area);
            return ObjectMapper.Map<Area, AreaDto>(area);
        }

        public async Task<AreaDto> UpdateAsync(Guid id, UpdateAreaDto input)
        {
            var area = await _areaRepository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(input.Title))
                area.Title = input.Title;

            if (input.Description != null)
                area.Description = input.Description;

            if (input.Purpose != null)
                area.Purpose = input.Purpose;

            await _areaRepository.UpdateAsync(area);
            return ObjectMapper.Map<Area, AreaDto>(area);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _areaRepository.DeleteAsync(id);
        }

        public async Task<AreaDto> ArchiveAsync(Guid id)
        {
            var area = await _areaRepository.GetAsync(id);
            area.Archive();
            await _areaRepository.UpdateAsync(area);
            return ObjectMapper.Map<Area, AreaDto>(area);
        }

        public async Task<AreaDto> ReactivateAsync(Guid id)
        {
            var area = await _areaRepository.GetAsync(id);
            area.Reactivate();
            await _areaRepository.UpdateAsync(area);
            return ObjectMapper.Map<Area, AreaDto>(area);
        }

        public async Task<AreaDto> MarkAsReviewedAsync(Guid id)
        {
            var area = await _areaRepository.GetAsync(id);
            area.MarkAsReviewed();
            await _areaRepository.UpdateAsync(area);
            return ObjectMapper.Map<Area, AreaDto>(area);
        }

        public async Task<AreaDto> AddStandardAsync(Guid id, string standard)
        {
            var area = await _areaRepository.GetAsync(id);
            area.AddStandard(standard);
            await _areaRepository.UpdateAsync(area);
            return ObjectMapper.Map<Area, AreaDto>(area);
        }

        public async Task<AreaDto> AddKeyResultAreaAsync(Guid id, string kra)
        {
            var area = await _areaRepository.GetAsync(id);
            area.AddKeyResultArea(kra);
            await _areaRepository.UpdateAsync(area);
            return ObjectMapper.Map<Area, AreaDto>(area);
        }
    }
}
