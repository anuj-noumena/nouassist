using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel
{
    public interface IRevenueModelAppService : IApplicationService
    {
        Task<RevenueModelDto> GetAsync(Guid id);
        Task<PagedResultDto<RevenueModelDto>> GetListAsync(GetRevenueModelListDto input);
        Task<RevenueModelDto> CreateAsync(CreateRevenueModelDto input);
        Task<RevenueModelDto> UpdateAsync(Guid id, UpdateRevenueModelDto input);
        Task DeleteAsync(Guid id);
        Task<RevenueModelDto> ActivateAsync(Guid id);
        Task<RevenueModelDto> DeactivateAsync(Guid id);

        // Pricing Model Management
        Task<PricingModelDto> CreatePricingModelAsync(Guid revenueModelId, CreatePricingModelDto input);
        Task DeletePricingModelAsync(Guid revenueModelId, Guid pricingModelId);
        Task<List<PricingModelDto>> GetPricingModelsAsync(Guid revenueModelId);

        // Revenue Scenario Management
        Task<RevenueScenarioDto> CreateScenarioAsync(Guid revenueModelId, CreateRevenueScenarioDto input);
        Task<RevenueScenarioDto> UpdateScenarioParametersAsync(Guid revenueModelId, Guid scenarioId, ScenarioParametersDto parameters);
        Task DeleteScenarioAsync(Guid revenueModelId, Guid scenarioId);
        Task<List<RevenueScenarioDto>> GetScenariosAsync(Guid revenueModelId);
        Task<List<ProjectedRevenueDataDto>> SimulateRevenueAsync(Guid revenueModelId, Guid scenarioId, SimulateRevenueDto input);
        Task<List<ProjectedRevenueDataDto>> CompareScenarios(Guid revenueModelId, List<Guid> scenarioIds);
        Task<Dictionary<string, decimal>> GetKeyMetricsAsync(Guid revenueModelId, Guid scenarioId);

        // Go-to-Market Strategy Management
        Task<GoToMarketStrategyDto> CreateGtmStrategyAsync(Guid revenueModelId, CreateGoToMarketStrategyDto input);
        Task<GoToMarketStrategyDto> UpdateGtmStrategyAsync(Guid revenueModelId, Guid gtmStrategyId, CreateGoToMarketStrategyDto input);
        Task DeleteGtmStrategyAsync(Guid revenueModelId, Guid gtmStrategyId);
        Task<List<GoToMarketStrategyDto>> GetGtmStrategiesAsync(Guid revenueModelId);
        Task<List<GtmPlanElementDto>> GetRecommendedChannelsAsync(Guid revenueModelId, string targetMarket, decimal budget);

        // Analytics
        Task<decimal> CalculateProjectedRevenueAsync(Guid revenueModelId, int months);
    }
}
