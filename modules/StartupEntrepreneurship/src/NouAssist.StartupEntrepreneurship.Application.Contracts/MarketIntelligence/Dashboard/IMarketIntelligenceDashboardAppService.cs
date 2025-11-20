using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Dashboard.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Dashboard
{
    /// <summary>
    /// Interface for market intelligence dashboard application service
    /// </summary>
    public interface IMarketIntelligenceDashboardAppService : IApplicationService
    {
        /// <summary>
        /// Gets dashboard data
        /// </summary>
        Task<MarketIntelligenceDashboardDto> GetDashboardDataAsync();
    }
}
