using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using NouAssist.FinancialManagement.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning
{
    [Authorize(FinancialManagementPermissions.FinancialPlanning.Default)]
    public class ExpenseReportAppService : ApplicationService, IExpenseReportAppService
    {
        private readonly IRepository<ExpenseReport, Guid> _reportRepository;
        private readonly IExpenseReportRepository _customReportRepository;

        public ExpenseReportAppService(
            IRepository<ExpenseReport, Guid> reportRepository,
            IExpenseReportRepository customReportRepository)
        {
            _reportRepository = reportRepository;
            _customReportRepository = customReportRepository;
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ViewReports)]
        public async Task<ExpenseReportDto> GetAsync(Guid id)
        {
            var report = await _customReportRepository.GetWithExpensesAsync(id);
            return ObjectMapper.Map<ExpenseReport, ExpenseReportDto>(report);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ViewReports)]
        public async Task<PagedResultDto<ExpenseReportDto>> GetListAsync(GetExpenseReportListDto input)
        {
            var queryable = await _reportRepository.GetQueryableAsync();

            if (input.SubmitterFilter.HasValue)
            {
                queryable = queryable.Where(r => r.SubmitterId == input.SubmitterFilter.Value);
            }

            if (input.StatusFilter.HasValue)
            {
                queryable = queryable.Where(r => r.Status == input.StatusFilter.Value);
            }

            if (input.StartDate.HasValue)
            {
                queryable = queryable.Where(r => r.PeriodStart >= input.StartDate.Value);
            }

            if (input.EndDate.HasValue)
            {
                queryable = queryable.Where(r => r.PeriodEnd <= input.EndDate.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(r => r.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var reports = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<ExpenseReportDto>(
                totalCount,
                ObjectMapper.Map<List<ExpenseReport>, List<ExpenseReportDto>>(reports)
            );
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.CreateReports)]
        public async Task<ExpenseReportDto> CreateAsync(CreateExpenseReportDto input)
        {
            var report = new ExpenseReport(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                CurrentUser.Id.Value,
                input.ReportName,
                input.PeriodStart,
                input.PeriodEnd,
                input.Currency
            );

            await _reportRepository.InsertAsync(report, autoSave: true);

            return ObjectMapper.Map<ExpenseReport, ExpenseReportDto>(report);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.SubmitReports)]
        public async Task<ExpenseReportDto> SubmitAsync(SubmitExpenseReportDto input)
        {
            var report = await _reportRepository.GetAsync(input.ReportId);
            report.Submit();
            await _reportRepository.UpdateAsync(report, autoSave: true);

            return ObjectMapper.Map<ExpenseReport, ExpenseReportDto>(report);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ApproveReports)]
        public async Task<ExpenseReportDto> ApproveAsync(ApproveExpenseReportDto input)
        {
            var report = await _reportRepository.GetAsync(input.ReportId);
            report.Approve(CurrentUser.Id.Value);
            await _reportRepository.UpdateAsync(report, autoSave: true);

            return ObjectMapper.Map<ExpenseReport, ExpenseReportDto>(report);
        }
    }
}
