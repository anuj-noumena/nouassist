using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.FinancialPlanning
{
    public interface IBudgetAppService : IApplicationService
    {
        Task<BudgetDto> GetAsync(Guid id);
        Task<PagedResultDto<BudgetDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<BudgetDto> CreateAsync(CreateBudgetDto input);
        Task<BudgetDto> ActivateAsync(Guid id);
        Task<BudgetDto> CloseAsync(Guid id);
        Task<BudgetDto> GetActiveBudgetAsync(Guid userId);
        Task DeleteAsync(Guid id);
    }
}
