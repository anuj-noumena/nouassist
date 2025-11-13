using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsPayable.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.AccountsPayable
{
    public interface IVendorAppService : IApplicationService
    {
        Task<VendorDto> GetAsync(Guid id);
        Task<PagedResultDto<VendorDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<VendorDto> CreateAsync(CreateVendorDto input);
        Task<VendorDto> UpdateAsync(Guid id, UpdateVendorDto input);
        Task DeleteAsync(Guid id);
    }
}
