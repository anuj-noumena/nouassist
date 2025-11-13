using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.GeneralLedger
{
    public interface IAccountingPeriodAppService : IApplicationService
    {
        Task<AccountingPeriodDto> GetAsync(Guid id);
        Task<PagedResultDto<AccountingPeriodDto>> GetListAsync(GetAccountingPeriodListDto input);
        Task<AccountingPeriodDto> CreateAsync(CreateAccountingPeriodDto input);
        Task<AccountingPeriodDto> OpenAsync(Guid id);
        Task<AccountingPeriodDto> CloseAsync(Guid id, CloseAccountingPeriodDto input);
        Task<AccountingPeriodDto> GetCurrentPeriodAsync();
    }
}
