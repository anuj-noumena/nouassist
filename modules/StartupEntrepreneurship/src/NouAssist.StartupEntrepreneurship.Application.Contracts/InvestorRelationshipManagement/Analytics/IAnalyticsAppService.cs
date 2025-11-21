using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Analytics.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Analytics
{
    /// <summary>
    /// Application service interface for analytics and reporting
    /// </summary>
    public interface IAnalyticsAppService : IApplicationService
    {
        // Engagement Analytics
        Task<InvestorEngagementReportDto> GenerateEngagementReportAsync(GenerateReportDto input);
        Task<InvestorPerformanceReportDto> GeneratePerformanceReportAsync(Guid investorId, GenerateReportDto input);
        Task<UpdateEffectivenessReportDto> GenerateUpdateEffectivenessReportAsync(GenerateReportDto input);

        // Trend Analysis
        Task<List<InvestorTrendDto>> AnalyzeInvestorTrendsAsync(TimeSpan period);

        // Export Functions
        Task<byte[]> ExportEngagementReportAsync(GenerateReportDto input);
        Task<byte[]> ExportPerformanceReportAsync(Guid investorId, GenerateReportDto input);
        Task<byte[]> ExportUpdateEffectivenessReportAsync(GenerateReportDto input);
    }

    public class InvestorTrendDto
    {
        public string TrendType { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public decimal Change { get; set; }
        public string Direction { get; set; }
    }
}
