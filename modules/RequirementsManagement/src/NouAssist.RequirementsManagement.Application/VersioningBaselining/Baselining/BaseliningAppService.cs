using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.Baselining.Dtos;
using NouAssist.RequirementsManagement.VersioningBaselining.Entities;
using NouAssist.RequirementsManagement.VersioningBaselining.Localization;
using NouAssist.RequirementsManagement.VersioningBaselining.Permissions;
using NouAssist.RequirementsManagement.VersioningBaselining.Repositories;
using NouAssist.RequirementsManagement.VersioningBaselining.Services.Interfaces;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Baselining
{
    [Authorize(VersionControlPermissions.Baselining.Default)]
    public class BaseliningAppService : ApplicationService, IBaseliningAppService
    {
        private readonly IBaseliningManager _baseliningManager;
        private readonly IBaselineDefinitionRepository _baselineRepository;
        private readonly ILogger<BaseliningAppService> _logger;

        public BaseliningAppService(
            IBaseliningManager baseliningManager,
            IBaselineDefinitionRepository baselineRepository,
            ILogger<BaseliningAppService> logger)
        {
            _baseliningManager = baseliningManager;
            _baselineRepository = baselineRepository;
            _logger = logger;
            
            LocalizationResource = typeof(VersionControlResource);
        }

        [Authorize(VersionControlPermissions.Baselining.Create)]
        public virtual async Task<BaselineDto> CreateAsync(CreateBaselineDto input)
        {
            try
            {
                var baseline = await _baseliningManager.CreateBaselineAsync(
                    input.Name,
                    input.Description,
                    CurrentTenant.Id);
                    
                return ObjectMapper.Map<BaselineDefinition, BaselineDto>(baseline);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Baselining.Update)]
        public virtual async Task<BaselineDto> UpdateAsync(Guid id, UpdateBaselineDto input)
        {
            try
            {
                var baseline = await _baselineRepository.GetAsync(id);
                
                baseline.Update(input.Name, input.Description);
                
                await _baselineRepository.UpdateAsync(baseline);
                
                return ObjectMapper.Map<BaselineDefinition, BaselineDto>(baseline);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Baselining.View)]
        public virtual async Task<BaselineDto> GetAsync(Guid id)
        {
            try
            {
                var baseline = await _baselineRepository.GetAsync(id);
                return ObjectMapper.Map<BaselineDefinition, BaselineDto>(baseline);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Baselining.View)]
        public virtual async Task<PagedResultDto<BaselineDto>> GetListAsync(GetBaselineListDto input)
        {
            try
            {
                var queryable = await _baselineRepository.GetQueryableAsync();
                
                // Apply filters
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    queryable = queryable.Where(b => b.Name.Contains(input.Filter));
                }
                
                if (input.Status.HasValue)
                {
                    queryable = queryable.Where(b => b.Status == input.Status.Value);
                }
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? nameof(BaselineDefinition.Name))
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);
                    
                // Execute query
                var baselines = await AsyncExecuter.ToListAsync(queryable);
                
                // Map to DTOs
                var dtoList = ObjectMapper.Map<List<BaselineDefinition>, List<BaselineDto>>(baselines);
                
                return new PagedResultDto<BaselineDto>(totalCount, dtoList);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Baselining.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _baselineRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Baselining.Update)]
        public virtual async Task<BaselineDto> AddRequirementAsync(Guid id, AddRequirementToBaselineDto input)
        {
            try
            {
                await _baseliningManager.AddRequirementToBaselineAsync(
                    id,
                    input.RequirementId,
                    input.Version);
                    
                var baseline = await _baselineRepository.GetAsync(id);
                return ObjectMapper.Map<BaselineDefinition, BaselineDto>(baseline);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Baselining.Update)]
        public virtual async Task<BaselineDto> RemoveRequirementAsync(Guid id, Guid requirementId)
        {
            try
            {
                await _baseliningManager.RemoveRequirementFromBaselineAsync(id, requirementId);
                
                var baseline = await _baselineRepository.GetAsync(id);
                return ObjectMapper.Map<BaselineDefinition, BaselineDto>(baseline);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Baselining.Update)]
        public virtual async Task<BaselineDto> UpdateRequirementAsync(Guid id, UpdateRequirementInBaselineDto input)
        {
            try
            {
                await _baseliningManager.UpdateRequirementVersionInBaselineAsync(
                    id,
                    input.RequirementId,
                    input.NewVersion);
                    
                var baseline = await _baselineRepository.GetAsync(id);
                return ObjectMapper.Map<BaselineDefinition, BaselineDto>(baseline);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Baselining.ManageStatus)]
        public virtual async Task<BaselineDto> ActivateAsync(Guid id)
        {
            try
            {
                await _baseliningManager.ActivateBaselineAsync(id);
                
                var baseline = await _baselineRepository.GetAsync(id);
                return ObjectMapper.Map<BaselineDefinition, BaselineDto>(baseline);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Baselining.ManageStatus)]
        public virtual async Task<BaselineDto> ArchiveAsync(Guid id)
        {
            try
            {
                await _baseliningManager.ArchiveBaselineAsync(id);
                
                var baseline = await _baselineRepository.GetAsync(id);
                return ObjectMapper.Map<BaselineDefinition, BaselineDto>(baseline);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        protected virtual async Task HandleErrorAsync(Exception exception)
        {
            _logger.LogError(exception, "An error occurred in Baselining App Service: {Message}", exception.Message);
            
            if (exception is BusinessException businessException)
            {
                throw new UserFriendlyException(
                    L[businessException.Code],
                    businessException.Code,
                    businessException.Data);
            }
        }
    }
}
