using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.Integration.SubModuleIntegration
{
    /// <summary>
    /// Application service for Phase 1 sub-module integration
    /// Provides APIs for GeneralLedger and Budget integration status
    /// </summary>
    public interface IFinancialSubModuleIntegrationAppService : IApplicationService
    {
        /// <summary>
        /// Get integration dashboard with GL and budget metrics
        /// </summary>
        Task<IntegrationDashboardDto> GetIntegrationDashboardAsync(DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Get general ledger integration status
        /// </summary>
        Task<GeneralLedgerIntegrationStatusDto> GetGeneralLedgerStatusAsync();

        /// <summary>
        /// Get budget integration status
        /// </summary>
        Task<BudgetIntegrationStatusDto> GetBudgetStatusAsync();

        /// <summary>
        /// Get recent journal entries created from AP/AR
        /// </summary>
        Task<RecentJournalEntriesDto> GetRecentAutoJournalEntriesAsync(int count = 10);

        /// <summary>
        /// Get recent budget variance alerts
        /// </summary>
        Task<BudgetVarianceAlertsDto> GetRecentBudgetVariancesAsync(int count = 10);

        /// <summary>
        /// Get integration health check
        /// </summary>
        Task<IntegrationHealthDto> GetIntegrationHealthAsync();
    }
}
