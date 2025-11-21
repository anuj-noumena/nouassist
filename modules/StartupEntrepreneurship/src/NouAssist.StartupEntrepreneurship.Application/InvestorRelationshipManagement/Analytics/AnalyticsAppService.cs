using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Analytics.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Permissions;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Services;
using System.Text.Json;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Localization;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Analytics
{
    /// <summary>
    /// Application service for analytics and reporting
    /// </summary>
    [Authorize(IRMPermissions.Analytics.Default)]
    public class AnalyticsAppService : ApplicationService, IAnalyticsAppService
    {
        private readonly IInvestorAnalyticsService _analyticsService;
        private readonly IInvestorProfileRepository _investorRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;

        public AnalyticsAppService(
            IInvestorAnalyticsService analyticsService,
            IInvestorProfileRepository investorRepository,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock)
        {
            _analyticsService = analyticsService;
            _investorRepository = investorRepository;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;

            LocalizationResource = typeof(IRMResource);
        }

        [Authorize(IRMPermissions.Analytics.ViewEngagement)]
        public async Task<InvestorEngagementReportDto> GenerateEngagementReportAsync(GenerateReportDto input)
        {
            try
            {
                var report = await _analyticsService.GenerateEngagementReportAsync(input.FromDate, input.ToDate);
                return ObjectMapper.Map<InvestorEngagementReport, InvestorEngagementReportDto>(report);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Analytics.ViewPerformance)]
        public async Task<InvestorPerformanceReportDto> GeneratePerformanceReportAsync(Guid investorId, GenerateReportDto input)
        {
            try
            {
                // Verify investor exists and belongs to current tenant
                var investor = await _investorRepository.GetAsync(investorId);
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                var report = await _analyticsService.GeneratePerformanceReportAsync(investorId, input.FromDate, input.ToDate);
                return ObjectMapper.Map<InvestorPerformanceReport, InvestorPerformanceReportDto>(report);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Analytics.ViewPerformance)]
        public async Task<UpdateEffectivenessReportDto> GenerateUpdateEffectivenessReportAsync(GenerateReportDto input)
        {
            try
            {
                var report = await _analyticsService.GenerateUpdateEffectivenessReportAsync(input.FromDate, input.ToDate);
                return ObjectMapper.Map<UpdateEffectivenessReport, UpdateEffectivenessReportDto>(report);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Analytics.ViewNetworkInsights)]
        public async Task<List<InvestorTrendDto>> AnalyzeInvestorTrendsAsync(TimeSpan period)
        {
            try
            {
                var trends = await _analyticsService.AnalyzeInvestorTrendsAsync(period);
                return ObjectMapper.Map<List<InvestorTrend>, List<InvestorTrendDto>>(trends);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Analytics.ExportReports)]
        public async Task<byte[]> ExportEngagementReportAsync(GenerateReportDto input)
        {
            try
            {
                var report = await _analyticsService.GenerateEngagementReportAsync(input.FromDate, input.ToDate);
                var reportDto = ObjectMapper.Map<InvestorEngagementReport, InvestorEngagementReportDto>(report);
                
                // Convert to JSON and then to bytes
                var json = JsonSerializer.Serialize(reportDto, new JsonSerializerOptions { WriteIndented = true });
                return Encoding.UTF8.GetBytes(json);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Analytics.ExportReports)]
        public async Task<byte[]> ExportPerformanceReportAsync(Guid investorId, GenerateReportDto input)
        {
            try
            {
                // Verify investor exists and belongs to current tenant
                var investor = await _investorRepository.GetAsync(investorId);
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                var report = await _analyticsService.GeneratePerformanceReportAsync(investorId, input.FromDate, input.ToDate);
                var reportDto = ObjectMapper.Map<InvestorPerformanceReport, InvestorPerformanceReportDto>(report);
                
                // Convert to JSON and then to bytes
                var json = JsonSerializer.Serialize(reportDto, new JsonSerializerOptions { WriteIndented = true });
                return Encoding.UTF8.GetBytes(json);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Analytics.ExportReports)]
        public async Task<byte[]> ExportUpdateEffectivenessReportAsync(GenerateReportDto input)
        {
            try
            {
                var report = await _analyticsService.GenerateUpdateEffectivenessReportAsync(input.FromDate, input.ToDate);
                var reportDto = ObjectMapper.Map<UpdateEffectivenessReport, UpdateEffectivenessReportDto>(report);
                
                // Convert to JSON and then to bytes
                var json = JsonSerializer.Serialize(reportDto, new JsonSerializerOptions { WriteIndented = true });
                return Encoding.UTF8.GetBytes(json);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in AnalyticsAppService: {ErrorMessage}", exception.Message);
        }
    }
}
