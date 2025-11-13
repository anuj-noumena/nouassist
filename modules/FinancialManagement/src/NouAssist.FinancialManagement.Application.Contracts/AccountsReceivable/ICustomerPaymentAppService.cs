using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.AccountsReceivable
{
    public interface ICustomerPaymentAppService : IApplicationService
    {
        Task<CustomerPaymentDto> GetAsync(Guid id);
        Task<PagedResultDto<CustomerPaymentDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<CustomerPaymentDto> RecordAsync(RecordPaymentDto input);
    }
}
