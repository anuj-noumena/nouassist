using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.Application.Contracts.FinancialPlanning.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.Application.Contracts.FinancialPlanning
{
    public interface IExpenseAppService : IApplicationService
    {
        Task<ExpenseDto> GetAsync(Guid id);
        Task<PagedResultDto<ExpenseDto>> GetListAsync(GetExpenseListDto input);
        Task<ExpenseDto> CreateAsync(CreateExpenseDto input);
        Task<ExpenseDto> SubmitAsync(SubmitExpenseDto input);
        Task<ExpenseDto> ApproveAsync(ApproveExpenseDto input);
    }
}
