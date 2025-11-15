using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.Application.Contracts.AccountsReceivable.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.Application.Contracts.AccountsReceivable
{
    public interface IInvoiceAppService : IApplicationService
    {
        Task<InvoiceDto> GetAsync(Guid id);
        Task<PagedResultDto<InvoiceDto>> GetListAsync(GetInvoiceListDto input);
        Task<InvoiceDto> CreateAsync(CreateInvoiceDto input);
        Task<InvoiceDto> SendAsync(SendInvoiceDto input);
        Task<InvoiceDto> ApplyPaymentAsync(ApplyPaymentToInvoiceDto input);
    }
}
