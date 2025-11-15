using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.Application.Contracts.AccountsReceivable.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.Application.Contracts.AccountsReceivable
{
    public interface ICustomerAppService : IApplicationService
    {
        Task<CustomerDto> GetAsync(Guid id);
        Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomerListDto input);
        Task<CustomerDto> CreateAsync(CreateCustomerDto input);
        Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto input);
        Task DeleteAsync(Guid id);
    }
}
