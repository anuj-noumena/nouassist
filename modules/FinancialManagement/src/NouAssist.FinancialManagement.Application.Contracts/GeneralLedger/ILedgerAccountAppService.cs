using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.GeneralLedger
{
    public interface ILedgerAccountAppService : IApplicationService
    {
        Task<LedgerAccountDto> GetAsync(Guid id);
        Task<PagedResultDto<LedgerAccountDto>> GetListAsync(GetLedgerAccountListDto input);
        Task<LedgerAccountDto> CreateAsync(CreateLedgerAccountDto input);
        Task<LedgerAccountDto> UpdateAsync(Guid id, UpdateLedgerAccountDto input);
        Task DeleteAsync(Guid id);
    }
}
