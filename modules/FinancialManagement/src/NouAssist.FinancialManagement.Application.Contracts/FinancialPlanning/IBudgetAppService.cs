using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.Application.Contracts.FinancialPlanning.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.Application.Contracts.FinancialPlanning
{
    public interface IBudgetAppService : IApplicationService
    {
        Task<BudgetDto> GetAsync(Guid id);
        Task<PagedResultDto<BudgetDto>> GetListAsync(GetBudgetListDto input);
        Task<BudgetDto> CreateAsync(CreateBudgetDto input);
    }
}
