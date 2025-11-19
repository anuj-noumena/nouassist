using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.FinancialPlanning
{
    public interface IExpenseReportAppService : IApplicationService
    {
        Task<ExpenseReportDto> GetAsync(Guid id);
        Task<PagedResultDto<ExpenseReportDto>> GetListAsync(GetExpenseReportListDto input);
        Task<ExpenseReportDto> CreateAsync(CreateExpenseReportDto input);
        Task<ExpenseReportDto> SubmitAsync(SubmitExpenseReportDto input);
        Task<ExpenseReportDto> ApproveAsync(ApproveExpenseReportDto input);
    }
}
