using System;
using System.Threading.Tasks;
using NouAssist.DebtManagement.PayoffPlans.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.DebtManagement.PayoffPlans
{
    public interface IDebtPayoffPlanAppService : IApplicationService
    {
        Task<DebtPayoffPlanDto> GetAsync(Guid id);
        Task<PagedResultDto<DebtPayoffPlanDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<DebtPayoffPlanDto> CreateAsync(CreateDebtPayoffPlanDto input);
        Task<DebtPayoffPlanDto> UpdateAsync(Guid id, UpdateDebtPayoffPlanDto input);
        Task DeleteAsync(Guid id);
        Task<ConsolidationAnalysisDto> AnalyzeConsolidationAsync(
            decimal consolidationRate,
            int termMonths,
            List<Guid> debtAccountIds);
    }
}
