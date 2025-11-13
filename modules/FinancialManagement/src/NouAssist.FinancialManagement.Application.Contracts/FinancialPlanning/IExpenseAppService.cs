using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.FinancialPlanning
{
    public interface IExpenseAppService : IApplicationService
    {
        Task<ExpenseDto> GetAsync(Guid id);
        Task<PagedResultDto<ExpenseDto>> GetListAsync(GetExpenseListDto input);
        Task<ExpenseDto> CreateAsync(CreateExpenseDto input);
        Task<ExpenseDto> SubmitAsync(Guid id);
        Task<ExpenseDto> ApproveAsync(Guid id, ApproveExpenseDto input);
        Task DeleteAsync(Guid id);
    }
}
