using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsPayable.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.AccountsPayable
{
    public interface IBillAppService : IApplicationService
    {
        Task<BillDto> GetAsync(Guid id);
        Task<PagedResultDto<BillDto>> GetListAsync(GetBillListDto input);
        Task<BillDto> CreateAsync(CreateBillDto input);
        Task<BillDto> SubmitAsync(Guid id);
        Task<BillDto> ApproveAsync(Guid id, ApproveBillDto input);
        Task DeleteAsync(Guid id);
    }
}
