using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Enums;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Data;
using NouAssist.WorkManagement.GTD.Dtos;
using NouAssist.WorkManagement.GTD.Enums;
using NouAssist.WorkManagement.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.WorkManagement.GTD.GTD
{
    [Authorize(WorkManagementPermissions.GTD.Horizons.Default)]
    public class GtdHorizonAppService : ApplicationService, IGtdHorizonAppService
    {
        private readonly IGtdHorizonRepository _horizonRepository;

        public GtdHorizonAppService(IGtdHorizonRepository horizonRepository)
        {
            _horizonRepository = horizonRepository;
        }

        public async Task<GtdHorizonDto> GetAsync(Guid id)
        {
            var horizon = await _horizonRepository.GetAsync(id);
            return ObjectMapper.Map<GtdHorizon, GtdHorizonDto>(horizon);
        }

        public async Task<GtdHorizonDto?> GetByLevelAsync(GtdHorizonLevel level)
        {
            var userId = CurrentUser.GetId();
            var horizon = await _horizonRepository.FindByUserAndLevelAsync(userId, level);
            return horizon != null 
                ? ObjectMapper.Map<GtdHorizon, GtdHorizonDto>(horizon) 
                : null;
        }

        public async Task<ListResultDto<GtdHorizonDto>> GetMyHorizonsAsync()
        {
            var userId = CurrentUser.GetId();
            var horizons = await _horizonRepository.GetByUserAsync(userId);
            var dtos = ObjectMapper.Map<List<GtdHorizon>, List<GtdHorizonDto>>(horizons);
            return new ListResultDto<GtdHorizonDto>(dtos);
        }

        public async Task<ListResultDto<GtdHorizonDto>> GetHorizonsNeedingReviewAsync()
        {
            var userId = CurrentUser.GetId();
            var horizons = await _horizonRepository.GetNeedingReviewAsync(userId);
            var dtos = ObjectMapper.Map<List<GtdHorizon>, List<GtdHorizonDto>>(horizons);
            return new ListResultDto<GtdHorizonDto>(dtos);
        }

        [Authorize(WorkManagementPermissions.GTD.Horizons.Create)]
        public async Task<GtdHorizonDto> CreateAsync(CreateGtdHorizonDto input)
        {
            var userId = CurrentUser.GetId();

            // Check if horizon for this level already exists
            var existing = await _horizonRepository.FindByUserAndLevelAsync(userId, input.HorizonLevel);
            if (existing != null)
            {
                throw new UserFriendlyException($"A horizon for level {input.HorizonLevel} already exists.");
            }

            var horizon = new GtdHorizon(
                GuidGenerator.Create(),
                userId,
                input.HorizonLevel,
                input.Title,
                CurrentTenant.Id
            );

            if (!string.IsNullOrWhiteSpace(input.Description))
                horizon.Description = input.Description;

            await _horizonRepository.InsertAsync(horizon);
            return ObjectMapper.Map<GtdHorizon, GtdHorizonDto>(horizon);
        }

        [Authorize(WorkManagementPermissions.GTD.Horizons.Update)]
        public async Task<GtdHorizonDto> UpdateAsync(Guid id, UpdateGtdHorizonDto input)
        {
            var horizon = await _horizonRepository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(input.Title))
                horizon.Title = input.Title;

            if (input.Description != null)
                horizon.Description = input.Description;

            if (input.Content != null)
                horizon.Content = input.Content;

            if (input.LifePurpose != null)
                horizon.LifePurpose = input.LifePurpose;

            if (input.VisionStatement != null)
                horizon.VisionStatement = input.VisionStatement;

            await _horizonRepository.UpdateAsync(horizon);
            return ObjectMapper.Map<GtdHorizon, GtdHorizonDto>(horizon);
        }

        [Authorize(WorkManagementPermissions.GTD.Horizons.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _horizonRepository.DeleteAsync(id);
        }

        [Authorize(WorkManagementPermissions.GTD.Horizons.Review)]
        public async Task<GtdHorizonDto> MarkAsReviewedAsync(Guid id)
        {
            var horizon = await _horizonRepository.GetAsync(id);
            horizon.MarkAsReviewed();
            await _horizonRepository.UpdateAsync(horizon);
            return ObjectMapper.Map<GtdHorizon, GtdHorizonDto>(horizon);
        }

        [Authorize(WorkManagementPermissions.GTD.Horizons.Publish)]
        public async Task<GtdHorizonDto> PublishAsync(Guid id)
        {
            var horizon = await _horizonRepository.GetAsync(id);
            horizon.Publish();
            await _horizonRepository.UpdateAsync(horizon);
            return ObjectMapper.Map<GtdHorizon, GtdHorizonDto>(horizon);
        }

        [Authorize(WorkManagementPermissions.GTD.Horizons.Update)]
        public async Task<GtdHorizonDto> LinkGoalAsync(Guid id, Guid goalId)
        {
            var horizon = await _horizonRepository.GetAsync(id);
            horizon.LinkGoal(goalId);
            await _horizonRepository.UpdateAsync(horizon);
            return ObjectMapper.Map<GtdHorizon, GtdHorizonDto>(horizon);
        }

        [Authorize(WorkManagementPermissions.GTD.Horizons.Update)]
        public async Task<GtdHorizonDto> LinkAreaAsync(Guid id, Guid areaId)
        {
            var horizon = await _horizonRepository.GetAsync(id);
            horizon.LinkArea(areaId);
            await _horizonRepository.UpdateAsync(horizon);
            return ObjectMapper.Map<GtdHorizon, GtdHorizonDto>(horizon);
        }

        [Authorize(WorkManagementPermissions.GTD.Horizons.Update)]
        public async Task<GtdHorizonDto> LinkProjectAsync(Guid id, Guid projectId)
        {
            var horizon = await _horizonRepository.GetAsync(id);
            horizon.LinkProject(projectId);
            await _horizonRepository.UpdateAsync(horizon);
            return ObjectMapper.Map<GtdHorizon, GtdHorizonDto>(horizon);
        }

        [Authorize(WorkManagementPermissions.GTD.Horizons.Update)]
        public async Task<GtdHorizonDto> AddCoreValueAsync(Guid id, string value)
        {
            var horizon = await _horizonRepository.GetAsync(id);
            horizon.AddCoreValue(value);
            await _horizonRepository.UpdateAsync(horizon);
            return ObjectMapper.Map<GtdHorizon, GtdHorizonDto>(horizon);
        }

        [Authorize(WorkManagementPermissions.GTD.Horizons.Update)]
        public async Task<GtdHorizonDto> AddKeyThemeAsync(Guid id, string theme)
        {
            var horizon = await _horizonRepository.GetAsync(id);
            horizon.AddKeyTheme(theme);
            await _horizonRepository.UpdateAsync(horizon);
            return ObjectMapper.Map<GtdHorizon, GtdHorizonDto>(horizon);
        }
    }
}
