using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.FinancialPlanning
{
    public interface IFinancialGoalAppService : IApplicationService
    {
        Task<FinancialGoalDto> GetAsync(Guid id);
        Task<PagedResultDto<FinancialGoalDto>> GetListAsync(GetFinancialGoalListDto input);
        Task<FinancialGoalDto> CreateAsync(CreateFinancialGoalDto input);
        Task<FinancialGoalDto> UpdateProgressAsync(UpdateFinancialGoalProgressDto input);
        Task<FinancialGoalDto> CompleteGoalAsync(Guid id);
    }
}
