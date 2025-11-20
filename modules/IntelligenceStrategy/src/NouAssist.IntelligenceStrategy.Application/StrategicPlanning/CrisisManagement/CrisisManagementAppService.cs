using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagement.Dtos;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Permissions;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Services;
using NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagement
{
    /// <summary>
    /// Application service for Crisis Management
    /// </summary>
    [Authorize(StrategicPlanningPermissions.CrisisManagement.Default)]
    public class CrisisManagementAppService : ApplicationService, ICrisisManagementAppService
    {
        private readonly ICrisisManagementPlanRepository _crisisManagementPlanRepository;
        private readonly ICrisisManagementService _crisisManagementService;
        private readonly ICurrentUser _currentUser;

        public CrisisManagementAppService(
            ICrisisManagementPlanRepository crisisManagementPlanRepository,
            ICrisisManagementService crisisManagementService,
            ICurrentUser currentUser)
        {
            _crisisManagementPlanRepository = crisisManagementPlanRepository;
            _crisisManagementService = crisisManagementService;
            _currentUser = currentUser;
            
            LocalizationResource = typeof(StrategicPlanningResource);
        }

        [Authorize(StrategicPlanningPermissions.CrisisManagement.View)]
        public virtual async Task<CrisisManagementPlanDto> GetAsync(Guid id)
        {
            try
            {
                var plan = await _crisisManagementPlanRepository.GetAsync(id, includeDetails: true);
                return ObjectMapper.Map<CrisisManagementPlan, CrisisManagementPlanDto>(plan);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["StrategicPlanning:ErrorMessage:CrisisManagementPlanNotFound"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.CrisisManagement.View)]
        public virtual async Task<PagedResultDto<CrisisManagementPlanDto>> GetListAsync(PagedResultRequestDto input)
        {
            try
            {
                var queryable = await _crisisManagementPlanRepository.GetQueryableAsync();
                
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                var plans = await AsyncExecuter.ToListAsync(
                    queryable
                        .OrderBy("MaxCrisisLevel desc, Name")
                        .PageBy(input)
                );

                return new PagedResultDto<CrisisManagementPlanDto>(
                    totalCount,
                    ObjectMapper.Map<List<CrisisManagementPlan>, List<CrisisManagementPlanDto>>(plans)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.CrisisManagement.Create)]
        public virtual async Task<CrisisManagementPlanDto> CreateAsync(CreateCrisisManagementPlanDto input)
        {
            try
            {
                var plan = await _crisisManagementService.CreatePlanAsync(
                    input.Name,
                    input.Description,
                    input.MaxCrisisLevel,
                    input.NextReviewDate
                );

                return ObjectMapper.Map<CrisisManagementPlan, CrisisManagementPlanDto>(plan);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.CrisisManagement.Update)]
        public virtual async Task<CrisisManagementPlanDto> UpdateAsync(Guid id, CreateCrisisManagementPlanDto input)
        {
            try
            {
                var plan = await _crisisManagementPlanRepository.GetAsync(id);
                
                // Update plan properties - would need update methods in domain aggregate
                await _crisisManagementPlanRepository.UpdateAsync(plan);

                return ObjectMapper.Map<CrisisManagementPlan, CrisisManagementPlanDto>(plan);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.CrisisManagement.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _crisisManagementPlanRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.CrisisManagement.Activate)]
        public virtual async Task<CrisisManagementPlanDto> ActivateAsync(Guid id, ActivateCrisisPlanDto input)
        {
            try
            {
                await _crisisManagementService.ActivateCrisisPlanAsync(
                    id, 
                    _currentUser.GetId(), 
                    input.Reason
                );

                var plan = await _crisisManagementPlanRepository.GetAsync(id);
                return ObjectMapper.Map<CrisisManagementPlan, CrisisManagementPlanDto>(plan);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.CrisisManagement.Activate)]
        public virtual async Task<CrisisManagementPlanDto> DeactivateAsync(Guid id)
        {
            try
            {
                var plan = await _crisisManagementPlanRepository.GetAsync(id);
                plan.Deactivate();
                await _crisisManagementPlanRepository.UpdateAsync(plan);

                return ObjectMapper.Map<CrisisManagementPlan, CrisisManagementPlanDto>(plan);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.CrisisManagement.ManageProtocols)]
        public virtual async Task<CrisisManagementPlanDto> AddContingencyItemAsync(Guid planId, AddContingencyItemDto input)
        {
            try
            {
                var plan = await _crisisManagementPlanRepository.GetAsync(planId);
                
                var riskScenario = new RiskScenario(
                    input.RiskDescription,
                    input.RiskLevel,
                    input.ProbabilityScore,
                    input.ImpactScore
                );

                plan.AddContingencyItem(
                    input.Title,
                    input.Description,
                    riskScenario,
                    input.Priority
                );

                await _crisisManagementPlanRepository.UpdateAsync(plan);

                return ObjectMapper.Map<CrisisManagementPlan, CrisisManagementPlanDto>(plan);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.CrisisManagement.View)]
        public virtual async Task<ListResultDto<CrisisManagementPlanDto>> GetPlansDueForReviewAsync()
        {
            try
            {
                var plans = await _crisisManagementPlanRepository.GetPlansDueForReviewAsync();
                return new ListResultDto<CrisisManagementPlanDto>(
                    ObjectMapper.Map<List<CrisisManagementPlan>, List<CrisisManagementPlanDto>>(plans)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.CrisisManagement.View)]
        public virtual async Task<ListResultDto<CrisisManagementPlanDto>> GetPlansForCrisisLevelAsync(CrisisLevel level)
        {
            try
            {
                var plans = await _crisisManagementService.GetPlansForCrisisLevelAsync(level);
                return new ListResultDto<CrisisManagementPlanDto>(
                    ObjectMapper.Map<List<CrisisManagementPlan>, List<CrisisManagementPlanDto>>(plans)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(StrategicPlanningPermissions.CrisisManagement.View)]
        public virtual async Task<CrisisManagementPlanDto> GetActivePlanAsync()
        {
            try
            {
                var plan = await _crisisManagementPlanRepository.GetActivePlanAsync();
                if (plan == null)
                {
                    return null;
                }
                return ObjectMapper.Map<CrisisManagementPlan, CrisisManagementPlanDto>(plan);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, 
                "Error in {AppServiceName}: {ErrorMessage}", 
                GetType().FullName, exception.Message);
            await Task.CompletedTask;
        }
    }
}
