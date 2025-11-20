using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Permissions;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels
{
    [Authorize(GrowthExperimentationPermissions.ConversionFunnel.Default)]
    public class ConversionFunnelAppService : ApplicationService, IConversionFunnelAppService
    {
        private readonly IConversionFunnelDefinitionRepository _funnelRepository;
        private readonly IFunnelAnalysisService _funnelAnalysisService;

        public ConversionFunnelAppService(
            IConversionFunnelDefinitionRepository funnelRepository,
            IFunnelAnalysisService funnelAnalysisService)
        {
            _funnelRepository = funnelRepository;
            _funnelAnalysisService = funnelAnalysisService;
        }

        [Authorize(GrowthExperimentationPermissions.ConversionFunnel.View)]
        public async Task<ConversionFunnelDto> GetAsync(Guid id)
        {
            try
            {
                var funnel = await _funnelRepository.GetWithStagesAsync(id);
                return ObjectMapper.Map<ConversionFunnelDefinition, ConversionFunnelDto>(funnel);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:FunnelNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting conversion funnel with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.ConversionFunnel.View)]
        public async Task<PagedResultDto<ConversionFunnelDto>> GetListAsync(GetConversionFunnelListDto input)
        {
            try
            {
                var queryable = await _funnelRepository.GetQueryableAsync();

                // Apply filtering
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    queryable = queryable.Where(f => f.Name.Contains(input.Filter) || 
                                                    f.Description.Contains(input.Filter));
                }

                if (input.IsActive.HasValue)
                {
                    queryable = queryable.Where(f => f.IsActive == input.IsActive.Value);
                }

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? nameof(ConversionFunnelDefinition.Name))
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);

                var funnels = await AsyncExecuter.ToListAsync(queryable);
                var funnelDtos = ObjectMapper.Map<List<ConversionFunnelDefinition>, List<ConversionFunnelDto>>(funnels);

                return new PagedResultDto<ConversionFunnelDto>(totalCount, funnelDtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting conversion funnel list");
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.ConversionFunnel.Create)]
        public async Task<ConversionFunnelDto> CreateAsync(CreateConversionFunnelDto input)
        {
            try
            {
                // Create funnel using domain service
                var funnel = await _funnelAnalysisService.CreateFunnelAsync(
                    input.Name,
                    input.Description);

                return ObjectMapper.Map<ConversionFunnelDefinition, ConversionFunnelDto>(funnel);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error creating funnel: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating conversion funnel {Name}", input.Name);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.ConversionFunnel.Update)]
        public async Task<ConversionFunnelDto> UpdateAsync(Guid id, UpdateConversionFunnelDto input)
        {
            try
            {
                var funnel = await _funnelRepository.GetAsync(id);
                
                // Update basic properties
                funnel.Name = input.Name;
                funnel.Description = input.Description;
                funnel.IsActive = input.IsActive;
                
                await _funnelRepository.UpdateAsync(funnel);
                
                return ObjectMapper.Map<ConversionFunnelDefinition, ConversionFunnelDto>(funnel);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:FunnelNotFound"]);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error updating funnel: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating conversion funnel with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.ConversionFunnel.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _funnelRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException)
            {
                // If entity not found, consider it as deleted
                return;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting conversion funnel with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.ConversionFunnel.Update)]
        public async Task<ConversionFunnelDto> SetActiveAsync(Guid id, bool isActive)
        {
            try
            {
                var funnel = await _funnelRepository.GetAsync(id);
                
                funnel.SetActive(isActive);
                await _funnelRepository.UpdateAsync(funnel);
                
                return ObjectMapper.Map<ConversionFunnelDefinition, ConversionFunnelDto>(funnel);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:FunnelNotFound"]);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error setting funnel active state: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error setting active state for funnel with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.ConversionFunnel.ManageStages)]
        public async Task<FunnelStageDto> AddStageAsync(Guid funnelId, CreateFunnelStageDto input)
        {
            try
            {
                // Add stage using domain service
                var stage = await _funnelAnalysisService.AddFunnelStageAsync(
                    funnelId,
                    input.Name,
                    input.Description,
                    input.Order,
                    input.Criteria,
                    input.IsExitStage);
                
                return ObjectMapper.Map<FunnelStage, FunnelStageDto>(stage);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:FunnelNotFound"]);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error adding stage: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding stage to funnel with ID {FunnelId}", funnelId);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.ConversionFunnel.ManageStages)]
        public async Task<FunnelStageDto> UpdateStageConversionRateAsync(Guid funnelId, Guid stageId, UpdateStageConversionRateDto input)
        {
            try
            {
                // Update conversion rate using domain service
                await _funnelAnalysisService.UpdateStageConversionRateAsync(
                    funnelId,
                    stageId,
                    input.EnteredCount,
                    input.CompletedCount);
                
                // Get updated stage
                var stage = await _funnelRepository.GetStageAsync(funnelId, stageId);
                
                return ObjectMapper.Map<FunnelStage, FunnelStageDto>(stage);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:StageNotFound"]);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error updating stage conversion rate: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating conversion rate for stage {StageId} in funnel {FunnelId}", stageId, funnelId);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.ConversionFunnel.View)]
        public async Task<List<FunnelStageDto>> GetFunnelStagesAsync(Guid funnelId)
        {
            try
            {
                var funnel = await _funnelRepository.GetWithStagesAsync(funnelId);
                
                if (funnel == null)
                {
                    throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:FunnelNotFound"]);
                }
                
                var stages = funnel.Stages.OrderBy(s => s.Definition.Order).ToList();
                return ObjectMapper.Map<List<FunnelStage>, List<FunnelStageDto>>(stages);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:FunnelNotFound"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting stages for funnel {FunnelId}", funnelId);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.ConversionFunnel.View)]
        public async Task<FunnelPerformanceDto> GetFunnelPerformanceAsync(Guid funnelId)
        {
            try
            {
                var performanceData = await _funnelAnalysisService.GetFunnelPerformanceAsync(funnelId);
                
                var performance = new FunnelPerformanceDto
                {
                    FunnelId = funnelId,
                    FunnelName = performanceData["funnelName"].ToString(),
                    OverallConversionRate = Convert.ToDouble(performanceData["overallConversionRate"]),
                    StageMetrics = new List<FunnelStageMetricDto>()
                };
                
                var stageMetrics = performanceData["stageMetrics"] as List<object>;
                if (stageMetrics != null)
                {
                    foreach (var metricObj in stageMetrics)
                    {
                        // Cast to dynamic for easier property access
                        dynamic metric = metricObj;
                        
                        performance.StageMetrics.Add(new FunnelStageMetricDto
                        {
                            StageId = metric.stageId,
                            StageName = metric.stageName,
                            Order = metric.order,
                            EnteredCount = metric.enteredCount,
                            CompletedCount = metric.completedCount,
                            ConversionRate = metric.conversionRate,
                            IsEntryStage = metric.isEntryStage,
                            IsExitStage = metric.isExitStage
                        });
                    }
                }
                
                return performance;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:FunnelNotFound"]);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error getting funnel performance: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting performance for funnel {FunnelId}", funnelId);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.ConversionFunnel.View)]
        public async Task<List<ConversionFunnelDto>> GetActiveFunnelsAsync()
        {
            try
            {
                var funnels = await _funnelRepository.GetActiveFunnelsAsync();
                return ObjectMapper.Map<List<ConversionFunnelDefinition>, List<ConversionFunnelDto>>(funnels);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting active funnels");
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.ConversionFunnel.ManageStages)]
        public async Task ValidateFunnelAsync(Guid funnelId)
        {
            try
            {
                await _funnelAnalysisService.ValidateFunnelAsync(funnelId);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:FunnelNotFound"]);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error validating funnel: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error validating funnel {FunnelId}", funnelId);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }
    }
}
