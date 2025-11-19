using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.InvestmentManagement.InvestmentGoals.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.InvestmentManagement.InvestmentGoals
{
    public interface IInvestmentGoalAppService : IApplicationService
    {
        Task<InvestmentGoalDto> GetAsync(Guid id);
        Task<PagedResultDto<InvestmentGoalDto>> GetListAsync(GetInvestmentGoalListDto input);
        Task<InvestmentGoalDto> CreateAsync(CreateInvestmentGoalDto input);
        Task<InvestmentGoalDto> UpdateAsync(Guid id, UpdateInvestmentGoalDto input);
        Task DeleteAsync(Guid id);
        Task<InvestmentGoalDto> UpdateProgressAsync(Guid id, UpdateGoalProgressDto input);
    }
}
