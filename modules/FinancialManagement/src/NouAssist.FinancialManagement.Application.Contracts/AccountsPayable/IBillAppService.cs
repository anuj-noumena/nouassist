using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.Application.Contracts.AccountsPayable.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.Application.Contracts.AccountsPayable
{
    public interface IBillAppService : IApplicationService
    {
        Task<BillDto> GetAsync(Guid id);
        Task<PagedResultDto<BillDto>> GetListAsync(GetBillListDto input);
        Task<BillDto> CreateAsync(CreateBillDto input);
        Task<BillDto> ApproveAsync(ApproveBillDto input);
        Task<BillDto> PayAsync(PayBillDto input);
    }
}
