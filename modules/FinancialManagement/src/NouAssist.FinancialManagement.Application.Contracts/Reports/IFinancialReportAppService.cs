using System.Threading.Tasks;
using NouAssist.FinancialManagement.Reports.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.Reports
{
    public interface IFinancialReportAppService : IApplicationService
    {
        Task<BalanceSheetDto> GetBalanceSheetAsync(GetReportInputDto input);
        Task<IncomeStatementDto> GetIncomeStatementAsync(GetReportInputDto input);
        Task<CashFlowStatementDto> GetCashFlowStatementAsync(GetReportInputDto input);
        Task<AgingReportDto> GetAccountsReceivableAgingAsync(GetReportInputDto input);
        Task<AgingReportDto> GetAccountsPayableAgingAsync(GetReportInputDto input);
        Task<BudgetVarianceReportDto> GetBudgetVarianceReportAsync(GetReportInputDto input);
    }
}
