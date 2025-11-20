using System;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports
{
    /// <summary>
    /// Application service interface for generating reports
    /// </summary>
    public interface IReportAppService : IApplicationService
    {
        /// <summary>
        /// Generates a resource utilization report
        /// </summary>
        /// <param name="criteria">Report criteria</param>
        /// <returns>Resource utilization report</returns>
        Task<ResourceUtilizationReportDto> GetResourceUtilizationReportAsync(ReportCriteriaDto criteria);

        /// <summary>
        /// Generates an inventory stock report
        /// </summary>
        /// <param name="criteria">Report criteria</param>
        /// <returns>Inventory stock report</returns>
        Task<InventoryStockReportDto> GetInventoryStockReportAsync(ReportCriteriaDto criteria);

        /// <summary>
        /// Generates a booking activity report
        /// </summary>
        /// <param name="criteria">Report criteria</param>
        /// <returns>Booking activity report</returns>
        Task<BookingActivityReportDto> GetBookingActivityReportAsync(ReportCriteriaDto criteria);

        /// <summary>
        /// Generates an audit summary report
        /// </summary>
        /// <param name="criteria">Report criteria</param>
        /// <returns>Audit summary report</returns>
        Task<AuditSummaryReportDto> GetAuditSummaryReportAsync(ReportCriteriaDto criteria);

        /// <summary>
        /// Identifies resource bottlenecks (resources with high utilization)
        /// </summary>
        /// <param name="criteria">Report criteria</param>
        /// <param name="threshold">Utilization threshold percentage (default 80%)</param>
        /// <returns>Resource utilization report with bottlenecks highlighted</returns>
        Task<ResourceUtilizationReportDto> IdentifyResourceBottlenecksAsync(ReportCriteriaDto criteria, decimal threshold = 80);

        /// <summary>
        /// Generates purchase recommendations based on inventory levels
        /// </summary>
        /// <param name="criteria">Report criteria</param>
        /// <returns>Inventory stock report with purchase recommendations</returns>
        Task<InventoryStockReportDto> GetPurchaseRecommendationsAsync(ReportCriteriaDto criteria);

        /// <summary>
        /// Generates a consumption trend report for inventory items
        /// </summary>
        /// <param name="criteria">Report criteria</param>
        /// <returns>Inventory consumption report</returns>
        Task<InventoryConsumptionReportDto> GetInventoryConsumptionTrendsAsync(ReportCriteriaDto criteria);
    }
}
