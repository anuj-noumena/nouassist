using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.FinancialPlanning
{
    public interface ILinkedBankAccountAppService : IApplicationService
    {
        Task<LinkedBankAccountDto> GetAsync(Guid id);
        Task<PagedResultDto<LinkedBankAccountDto>> GetListAsync(GetLinkedBankAccountListDto input);
        Task<LinkedBankAccountDto> CreateAsync(CreateLinkedBankAccountDto input);
        Task<LinkedBankAccountDto> UpdateBalanceAsync(UpdateBankAccountBalanceDto input);
        Task<decimal> GetTotalBalanceAsync();
        Task DisconnectAsync(Guid id);
        Task ReconnectAsync(Guid id);
    }
}
