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
        Task<PagedResultDto<FinancialGoalDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<FinancialGoalDto> CreateAsync(CreateFinancialGoalDto input);
        Task<FinancialGoalDto> AddContributionAsync(Guid id, AddGoalContributionDto input);
        Task<FinancialGoalDto> MarkAsAchievedAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
