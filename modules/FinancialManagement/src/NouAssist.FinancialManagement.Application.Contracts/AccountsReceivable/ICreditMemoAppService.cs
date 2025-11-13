using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.AccountsReceivable
{
    public interface ICreditMemoAppService : IApplicationService
    {
        Task<CreditMemoDto> GetAsync(Guid id);
        Task<PagedResultDto<CreditMemoDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<CreditMemoDto> CreateAsync(CreateCreditMemoDto input);
        Task<CreditMemoDto> ApplyToInvoiceAsync(Guid creditMemoId, Guid invoiceId);
    }
}
