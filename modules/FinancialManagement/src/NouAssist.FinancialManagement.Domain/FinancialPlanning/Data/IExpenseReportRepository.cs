using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning.Data
{
    public interface IExpenseReportRepository : IRepository<ExpenseReport, Guid>
    {
        Task<ExpenseReport?> GetWithExpensesAsync(Guid id);
        Task<List<ExpenseReport>> GetReportsBySubmitterAsync(Guid submitterId);
        Task<List<ExpenseReport>> GetReportsByStatusAsync(Guid? tenantId, ReportStatus status);
        Task<List<ExpenseReport>> GetPendingApprovalsAsync(Guid? tenantId);
        Task<List<ExpenseReport>> GetReportsInPeriodAsync(Guid? tenantId, DateTime startDate, DateTime endDate);
    }
}
