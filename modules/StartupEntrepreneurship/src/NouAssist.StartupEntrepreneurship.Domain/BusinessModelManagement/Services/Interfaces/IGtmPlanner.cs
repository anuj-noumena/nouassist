using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services.Interfaces
{
    /// <summary>
    /// Domain service for go-to-market strategy planning
    /// </summary>
    public interface IGtmPlanner
    {
        Task<GoToMarketStrategy> CreateGtmStrategyAsync(
            RevenueModel revenueModel,
            string name,
            string description,
            string targetMarket,
            DateTime startDate,
            DateTime? endDate = null);

        Task<List<GtmPlanElement>> GenerateRecommendedChannelsAsync(
            string targetMarket,
            RevenueModelType revenueModelType,
            decimal budget);

        Task<ValidationResult> ValidateGtmStrategyAsync(GoToMarketStrategy strategy);

        Task<decimal> EstimateChannelCostAsync(
            GtmChannelType channelType,
            string targetAudience,
            int duration);

        Task<List<string>> GetEffectivenessMetricsAsync(GtmChannelType channelType);
    }
}
