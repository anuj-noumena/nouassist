using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Localization;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Permissions;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel
{
    [Authorize(BusinessModelManagementPermissions.RevenueModel.Default)]
    public class RevenueModelAppService : ApplicationService, IRevenueModelAppService
    {
        private readonly IRevenueModelRepository _revenueModelRepository;
        private readonly IRevenueSimulationEngine _simulationEngine;
        private readonly IGtmPlanner _gtmPlanner;
        private readonly ICurrentUser _currentUser;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public RevenueModelAppService(
            IRevenueModelRepository revenueModelRepository,
            IRevenueSimulationEngine simulationEngine,
            IGtmPlanner gtmPlanner,
            ICurrentUser currentUser,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _revenueModelRepository = revenueModelRepository;
            _simulationEngine = simulationEngine;
            _gtmPlanner = gtmPlanner;
            _currentUser = currentUser;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(BusinessModelManagementResource);
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.View)]
        public virtual async Task<RevenueModelDto> GetAsync(Guid id)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetCompleteAsync(id);
                await CheckRevenueModelAccessAsync(revenueModel);

                return ObjectMapper.Map<Aggregates.RevenueModel, RevenueModelDto>(revenueModel);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.View)]
        public virtual async Task<PagedResultDto<RevenueModelDto>> GetListAsync(GetRevenueModelListDto input)
        {
            try
            {
                var query = await _revenueModelRepository.GetQueryableAsync();

                // Apply filters
                query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(input.Filter) || x.Description.Contains(input.Filter));

                if (input.OwnerId.HasValue)
                {
                    query = query.Where(x => x.OwnerId == input.OwnerId.Value);
                }

                if (input.Type.HasValue)
                {
                    query = query.Where(x => x.Type == input.Type.Value);
                }

                if (input.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == input.IsActive.Value);
                }

                var totalCount = await AsyncExecuter.CountAsync(query);

                var sorting = input.Sorting.IsNullOrWhiteSpace() ? nameof(Aggregates.RevenueModel.CreationTime) + " desc" : input.Sorting;
                var items = await AsyncExecuter.ToListAsync(
                    query.OrderBy(sorting).PageBy(input));

                return new PagedResultDto<RevenueModelDto>(
                    totalCount,
                    ObjectMapper.Map<List<Aggregates.RevenueModel>, List<RevenueModelDto>>(items)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.Create)]
        public virtual async Task<RevenueModelDto> CreateAsync(CreateRevenueModelDto input)
        {
            try
            {
                var revenueModel = Aggregates.RevenueModel.Create(
                    _guidGenerator.Create(),
                    input.Name,
                    input.Description,
                    input.Type,
                    _currentUser.GetId(),
                    CurrentTenant.Id
                );

                await _revenueModelRepository.InsertAsync(revenueModel);

                return ObjectMapper.Map<Aggregates.RevenueModel, RevenueModelDto>(revenueModel);
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

        [Authorize(BusinessModelManagementPermissions.RevenueModel.Update)]
        public virtual async Task<RevenueModelDto> UpdateAsync(Guid id, UpdateRevenueModelDto input)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetAsync(id);
                await CheckRevenueModelAccessAsync(revenueModel);

                revenueModel.UpdateName(input.Name);
                revenueModel.UpdateDescription(input.Description);
                revenueModel.UpdateType(input.Type);

                await _revenueModelRepository.UpdateAsync(revenueModel);

                return ObjectMapper.Map<Aggregates.RevenueModel, RevenueModelDto>(revenueModel);
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

        [Authorize(BusinessModelManagementPermissions.RevenueModel.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetAsync(id);
                await CheckRevenueModelAccessAsync(revenueModel);

                await _revenueModelRepository.DeleteAsync(revenueModel);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.Update)]
        public virtual async Task<RevenueModelDto> ActivateAsync(Guid id)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetAsync(id);
                await CheckRevenueModelAccessAsync(revenueModel);

                revenueModel.Activate();
                await _revenueModelRepository.UpdateAsync(revenueModel);

                return ObjectMapper.Map<Aggregates.RevenueModel, RevenueModelDto>(revenueModel);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.Update)]
        public virtual async Task<RevenueModelDto> DeactivateAsync(Guid id)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetAsync(id);
                await CheckRevenueModelAccessAsync(revenueModel);

                revenueModel.Deactivate();
                await _revenueModelRepository.UpdateAsync(revenueModel);

                return ObjectMapper.Map<Aggregates.RevenueModel, RevenueModelDto>(revenueModel);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.ManagePricing)]
        public virtual async Task<PricingModelDto> CreatePricingModelAsync(Guid revenueModelId, CreatePricingModelDto input)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithPricingModelsAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                var pricingModel = revenueModel.AddPricingModel(input.Name, input.Strategy, input.Description);
                await _revenueModelRepository.UpdateAsync(revenueModel);

                return ObjectMapper.Map<Entities.PricingModel, PricingModelDto>(pricingModel);
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

        [Authorize(BusinessModelManagementPermissions.RevenueModel.ManagePricing)]
        public virtual async Task DeletePricingModelAsync(Guid revenueModelId, Guid pricingModelId)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithPricingModelsAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                revenueModel.RemovePricingModel(pricingModelId);
                await _revenueModelRepository.UpdateAsync(revenueModel);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.View)]
        public virtual async Task<List<PricingModelDto>> GetPricingModelsAsync(Guid revenueModelId)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithPricingModelsAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                return revenueModel.PricingModels
                    .Select(pm => ObjectMapper.Map<Entities.PricingModel, PricingModelDto>(pm))
                    .ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.ManageScenarios)]
        public virtual async Task<RevenueScenarioDto> CreateScenarioAsync(Guid revenueModelId, CreateRevenueScenarioDto input)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithScenariosAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                var parameters = ObjectMapper.Map<ScenarioParametersDto, ScenarioParameters>(input.Parameters);
                
                var scenario = revenueModel.CreateScenario(
                    input.Name,
                    input.Description,
                    parameters,
                    _currentUser.GetId(),
                    _clock.Now
                );

                await _revenueModelRepository.UpdateAsync(revenueModel);

                return ObjectMapper.Map<Entities.RevenueScenario, RevenueScenarioDto>(scenario);
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

        [Authorize(BusinessModelManagementPermissions.RevenueModel.ManageScenarios)]
        public virtual async Task<RevenueScenarioDto> UpdateScenarioParametersAsync(Guid revenueModelId, Guid scenarioId, ScenarioParametersDto parameters)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithScenariosAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                var scenario = revenueModel.GetScenario(scenarioId);
                if (scenario == null)
                {
                    throw new UserFriendlyException(L["ScenarioNotFound"]);
                }

                var newParameters = ObjectMapper.Map<ScenarioParametersDto, ScenarioParameters>(parameters);
                scenario.UpdateParameters(newParameters);

                await _revenueModelRepository.UpdateAsync(revenueModel);

                return ObjectMapper.Map<Entities.RevenueScenario, RevenueScenarioDto>(scenario);
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

        [Authorize(BusinessModelManagementPermissions.RevenueModel.ManageScenarios)]
        public virtual async Task DeleteScenarioAsync(Guid revenueModelId, Guid scenarioId)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithScenariosAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                revenueModel.RemoveScenario(scenarioId);
                await _revenueModelRepository.UpdateAsync(revenueModel);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.View)]
        public virtual async Task<List<RevenueScenarioDto>> GetScenariosAsync(Guid revenueModelId)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithScenariosAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                return revenueModel.Scenarios
                    .Select(s => ObjectMapper.Map<Entities.RevenueScenario, RevenueScenarioDto>(s))
                    .OrderByDescending(s => s.CreatedAt)
                    .ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.SimulateRevenue)]
        public virtual async Task<List<ProjectedRevenueDataDto>> SimulateRevenueAsync(Guid revenueModelId, Guid scenarioId, SimulateRevenueDto input)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithScenariosAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                var scenario = revenueModel.GetScenario(scenarioId);
                if (scenario == null)
                {
                    throw new UserFriendlyException(L["ScenarioNotFound"]);
                }

                var simulationDate = input.SimulationDate ?? _clock.Now;
                var projections = await _simulationEngine.SimulateRevenueAsync(scenario, simulationDate);

                await _revenueModelRepository.UpdateAsync(revenueModel);

                return projections
                    .Select(p => ObjectMapper.Map<ProjectedRevenueData, ProjectedRevenueDataDto>(p))
                    .ToList();
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

        [Authorize(BusinessModelManagementPermissions.RevenueModel.SimulateRevenue)]
        public virtual async Task<List<ProjectedRevenueDataDto>> CompareScenarios(Guid revenueModelId, List<Guid> scenarioIds)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithScenariosAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                var scenarios = scenarioIds
                    .Select(id => revenueModel.GetScenario(id))
                    .Where(s => s != null)
                    .ToList();

                if (!scenarios.Any())
                {
                    throw new UserFriendlyException(L["NoValidScenariosFound"]);
                }

                var projections = await _simulationEngine.CompareScenarios(scenarios, _clock.Now, 12);

                return projections
                    .Select(p => ObjectMapper.Map<ProjectedRevenueData, ProjectedRevenueDataDto>(p))
                    .ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.View)]
        public virtual async Task<Dictionary<string, decimal>> GetKeyMetricsAsync(Guid revenueModelId, Guid scenarioId)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithScenariosAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                var scenario = revenueModel.GetScenario(scenarioId);
                if (scenario == null)
                {
                    throw new UserFriendlyException(L["ScenarioNotFound"]);
                }

                return await _simulationEngine.CalculateKeyMetricsAsync(scenario);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.ManageGtmStrategies)]
        public virtual async Task<GoToMarketStrategyDto> CreateGtmStrategyAsync(Guid revenueModelId, CreateGoToMarketStrategyDto input)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithGtmStrategiesAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                var strategy = await _gtmPlanner.CreateGtmStrategyAsync(
                    revenueModel,
                    input.Name,
                    input.Description,
                    input.TargetMarket,
                    input.StartDate,
                    input.EndDate
                );

                await _revenueModelRepository.UpdateAsync(revenueModel);

                return ObjectMapper.Map<Entities.GoToMarketStrategy, GoToMarketStrategyDto>(strategy);
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

        [Authorize(BusinessModelManagementPermissions.RevenueModel.ManageGtmStrategies)]
        public virtual async Task<GoToMarketStrategyDto> UpdateGtmStrategyAsync(Guid revenueModelId, Guid gtmStrategyId, CreateGoToMarketStrategyDto input)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithGtmStrategiesAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                var strategy = revenueModel.GetGtmStrategy(gtmStrategyId);
                if (strategy == null)
                {
                    throw new UserFriendlyException(L["GtmStrategyNotFound"]);
                }

                strategy.UpdateDescription(input.Description);
                strategy.UpdateTargetMarket(input.TargetMarket);
                strategy.UpdateTimeline(input.StartDate, input.EndDate);

                await _revenueModelRepository.UpdateAsync(revenueModel);

                return ObjectMapper.Map<Entities.GoToMarketStrategy, GoToMarketStrategyDto>(strategy);
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

        [Authorize(BusinessModelManagementPermissions.RevenueModel.ManageGtmStrategies)]
        public virtual async Task DeleteGtmStrategyAsync(Guid revenueModelId, Guid gtmStrategyId)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithGtmStrategiesAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                revenueModel.RemoveGtmStrategy(gtmStrategyId);
                await _revenueModelRepository.UpdateAsync(revenueModel);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.View)]
        public virtual async Task<List<GoToMarketStrategyDto>> GetGtmStrategiesAsync(Guid revenueModelId)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetWithGtmStrategiesAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                return revenueModel.GtmStrategies
                    .Select(gtm => ObjectMapper.Map<Entities.GoToMarketStrategy, GoToMarketStrategyDto>(gtm))
                    .ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.View)]
        public virtual async Task<List<GtmPlanElementDto>> GetRecommendedChannelsAsync(Guid revenueModelId, string targetMarket, decimal budget)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                var recommendations = await _gtmPlanner.GenerateRecommendedChannelsAsync(
                    targetMarket,
                    revenueModel.Type,
                    budget
                );

                return recommendations
                    .Select(r => ObjectMapper.Map<GtmPlanElement, GtmPlanElementDto>(r))
                    .ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(BusinessModelManagementPermissions.RevenueModel.View)]
        public virtual async Task<decimal> CalculateProjectedRevenueAsync(Guid revenueModelId, int months)
        {
            try
            {
                var revenueModel = await _revenueModelRepository.GetCompleteAsync(revenueModelId);
                await CheckRevenueModelAccessAsync(revenueModel);

                return revenueModel.CalculateProjectedRevenue(months);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        // Helper methods
        private async Task CheckRevenueModelAccessAsync(Aggregates.RevenueModel revenueModel)
        {
            if (revenueModel.OwnerId != _currentUser.GetId() && revenueModel.TenantId != CurrentTenant.Id)
            {
                throw new UserFriendlyException(L["AccessDenied"]);
            }
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in {ServiceName}: {ErrorMessage}", 
                GetType().FullName, exception.Message);
            await Task.CompletedTask;
        }
    }
}
