using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.DebtManagement.DebtAccounts.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.DebtManagement.DebtAccounts
{
    public interface IDebtAccountAppService : IApplicationService
    {
        Task<DebtAccountDto> GetAsync(Guid id);
        Task<PagedResultDto<DebtAccountDto>> GetListAsync(GetDebtAccountListDto input);
        Task<DebtAccountDto> CreateAsync(CreateDebtAccountDto input);
        Task<DebtAccountDto> UpdateAsync(Guid id, UpdateDebtAccountDto input);
        Task DeleteAsync(Guid id);
        Task<DebtAccountDto> RecordPaymentAsync(Guid id, RecordDebtPaymentDto input);
        Task<PayoffProjectionDto> CalculatePayoffProjectionAsync(Guid id, decimal monthlyPayment);
        Task<decimal> GetTotalDebtBalanceAsync();
    }
}
