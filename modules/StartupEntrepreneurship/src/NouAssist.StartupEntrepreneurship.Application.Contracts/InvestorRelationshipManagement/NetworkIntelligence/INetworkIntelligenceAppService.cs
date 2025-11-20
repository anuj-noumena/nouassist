using System;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.NetworkIntelligence.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.NetworkIntelligence
{
    /// <summary>
    /// Application service interface for network intelligence and mapping
    /// </summary>
    public interface INetworkIntelligenceAppService : IApplicationService
    {
        // Network Mapping
        Task<NetworkMapDto> GenerateNetworkMapAsync(Guid investorId);
        Task<List<InvestorConnectionDto>> FindPotentialConnectionsAsync(Guid investorId);

        // Network Analysis
        Task<NetworkInsightsDto> AnalyzeNetworkInsightsAsync(List<Guid> investorIds);
        Task<List<InvestorInfluenceDto>> CalculateInfluenceScoresAsync();

        // Recommendations
        Task<List<string>> GetNetworkRecommendationsAsync(List<Guid> investorIds);
    }
}
