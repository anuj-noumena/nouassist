using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.AccountsReceivable
{
    public interface IInvoiceAppService : IApplicationService
    {
        Task<InvoiceDto> GetAsync(Guid id);
        Task<PagedResultDto<InvoiceDto>> GetListAsync(GetInvoiceListDto input);
        Task<InvoiceDto> CreateAsync(CreateInvoiceDto input);
        Task<InvoiceDto> SendAsync(Guid id);
        Task<InvoiceDto> ApplyPaymentAsync(Guid id, ApplyPaymentDto input);
        Task DeleteAsync(Guid id);
    }
}
