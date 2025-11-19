using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using NouAssist.FinancialManagement.CreditScoreMonitoring.CreditReports.Dtos;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Data;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Enums;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Permissions;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Services;
using NouAssist.FinancialManagement.InvestmentManagement.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.CreditReports
{
    [Authorize(CreditScoreMonitoringPermissions.CreditReports.Default)]
    public class CreditReportAppService : ApplicationService, ICreditReportAppService
    {
        private readonly ICreditReportRepository _creditReportRepository;
        private readonly CreditScoreAnalysisService _analysisService;

        public CreditReportAppService(
            ICreditReportRepository creditReportRepository,
            CreditScoreAnalysisService analysisService)
        {
            _creditReportRepository = creditReportRepository;
            _analysisService = analysisService;
        }

        [Authorize(CreditScoreMonitoringPermissions.CreditReports.ViewDetails)]
        public async Task<CreditReportDto> GetAsync(Guid id)
        {
            var report = await _creditReportRepository.GetWithDetailsAsync(id);
            return ObjectMapper.Map<CreditReport, CreditReportDto>(report);
        }

        [Authorize(CreditScoreMonitoringPermissions.CreditReports.View)]
        public async Task<PagedResultDto<CreditReportDto>> GetListAsync(GetCreditReportListDto input)
        {
            var userId = CurrentUser.GetId();
            var reports = await _creditReportRepository.GetUserReportsAsync(userId);

            if (input.Bureau.HasValue)
            {
                reports = reports.Where(r => r.Bureau == input.Bureau.Value).ToList();
            }

            if (input.FromDate.HasValue)
            {
                reports = reports.Where(r => r.ReportDate >= input.FromDate.Value).ToList();
            }

            if (input.ToDate.HasValue)
            {
                reports = reports.Where(r => r.ReportDate <= input.ToDate.Value).ToList();
            }

            var totalCount = reports.Count;
            var pagedReports = reports
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<CreditReportDto>(
                totalCount,
                ObjectMapper.Map<List<CreditReport>, List<CreditReportDto>>(pagedReports)
            );
        }

        [Authorize(CreditScoreMonitoringPermissions.CreditReports.View)]
        public async Task<CreditReportDto> GetLatestReportAsync(CreditBureau bureau)
        {
            var userId = CurrentUser.GetId();
            var report = await _creditReportRepository.GetLatestReportAsync(userId, bureau);

            if (report == null)
            {
                throw new Volo.Abp.BusinessException("CreditScoreMonitoring:NoReportsFound");
            }

            return ObjectMapper.Map<CreditReport, CreditReportDto>(report);
        }

        [Authorize(CreditScoreMonitoringPermissions.CreditReports.Import)]
        public async Task<CreditReportDto> ImportReportAsync(ImportCreditReportDto input)
        {
            var userId = CurrentUser.GetId();

            var report = new CreditReport(
                GuidGenerator.Create(),
                userId,
                CurrentTenant.Id,
                input.Bureau,
                input.ReportDate,
                input.CreditScore,
                input.PreviousScore
            );

            // Add accounts
            foreach (var accountDto in input.Accounts)
            {
                var assetClass = MapStringToAssetClass(accountDto.AccountType);
                
                report.AddAccount(
                    accountDto.AccountNumber,
                    accountDto.CreditorName,
                    accountDto.AccountType,
                    accountDto.Status,
                    accountDto.CreditLimit,
                    accountDto.CurrentBalance,
                    accountDto.OpenedDate
                );

                // Update payment history if provided
                if (accountDto.PaymentHistory30Days > 0 || 
                    accountDto.PaymentHistory60Days > 0 || 
                    accountDto.PaymentHistory90Days > 0)
                {
                    var account = report.Accounts.Last();
                    account.UpdatePaymentHistory(
                        accountDto.PaymentHistory30Days,
                        accountDto.PaymentHistory60Days,
                        accountDto.PaymentHistory90Days
                    );
                }
            }

            // Add inquiries
            foreach (var inquiryDto in input.Inquiries)
            {
                report.AddInquiry(
                    inquiryDto.Type,
                    inquiryDto.InquirerName,
                    inquiryDto.InquiryDate,
                    inquiryDto.Purpose
                );
            }

            report.RecalculateMetrics();

            await _creditReportRepository.InsertAsync(report);

            return ObjectMapper.Map<CreditReport, CreditReportDto>(report);
        }

        [Authorize(CreditScoreMonitoringPermissions.CreditReports.View)]
        public async Task<CreditScoreHistoryDto> GetScoreHistoryAsync(CreditBureau bureau, int months)
        {
            var userId = CurrentUser.GetId();
            var reports = await _creditReportRepository.GetReportHistoryAsync(userId, bureau, months);

            var scoreHistory = reports.Select(r => new CreditScorePointDto
            {
                Date = r.ReportDate,
                Score = r.CreditScore,
                Change = r.ScoreChange
            }).ToList();

            return new CreditScoreHistoryDto
            {
                Bureau = bureau,
                ScoreHistory = scoreHistory
            };
        }

        [Authorize(CreditScoreMonitoringPermissions.CreditReports.ViewDetails)]
        public async Task<CreditHealthReportDto> GetCreditHealthAnalysisAsync(Guid reportId)
        {
            var report = await _creditReportRepository.GetWithDetailsAsync(reportId);
            var healthReport = _analysisService.AnalyzeCreditHealth(report);

            return new CreditHealthReportDto
            {
                CreditScore = healthReport.CreditScore,
                ScoreRange = healthReport.ScoreRange,
                OverallHealthScore = healthReport.OverallHealthScore,
                HealthLevel = healthReport.HealthLevel,
                Factors = healthReport.Factors.Select(f => new CreditHealthFactorDto
                {
                    Name = f.Name,
                    Score = f.Score,
                    Impact = f.Impact,
                    Description = f.Description,
                    Weight = f.Weight
                }).ToList(),
                Recommendations = healthReport.Recommendations
            };
        }

        [Authorize(CreditScoreMonitoringPermissions.CreditReports.ViewDetails)]
        public async Task<ScoreProjectionDto> ProjectScoreImprovementAsync(
            Guid reportId,
            CreditImprovementPlanDto planDto)
        {
            var report = await _creditReportRepository.GetAsync(reportId);

            var plan = new CreditImprovementPlan
            {
                ReduceUtilizationTo = planDto.ReduceUtilizationTo,
                PayOffDelinquentAccounts = planDto.PayOffDelinquentAccounts,
                MonthsToWait = planDto.MonthsToWait
            };

            var projection = _analysisService.ProjectScoreImprovement(report, plan);

            return new ScoreProjectionDto
            {
                CurrentScore = projection.CurrentScore,
                ProjectedScore = projection.ProjectedScore,
                ProjectedGain = projection.ProjectedGain,
                TimelineMonths = projection.TimelineMonths,
                ProjectedAchievementDate = projection.ProjectedAchievementDate
            };
        }

        private AssetClass MapStringToAssetClass(string accountType)
        {
            // Simple mapping - adjust based on your needs
            return accountType.ToLower() switch
            {
                "credit card" => AssetClass.Cash,
                "mortgage" => AssetClass.RealEstate,
                _ => AssetClass.Cash
            };
        }
    }
}
