using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.CreditScoreMonitoring.CreditDisputes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.CreditDisputes
{
    public interface ICreditDisputeAppService : IApplicationService
    {
        Task<CreditDisputeDto> GetAsync(Guid id);
        Task<PagedResultDto<CreditDisputeDto>> GetListAsync(GetCreditDisputeListDto input);
        Task<CreditDisputeDto> CreateAsync(CreateCreditDisputeDto input);
        Task<CreditDisputeDto> UpdateStatusAsync(Guid id, UpdateDisputeStatusDto input);
        Task CancelAsync(Guid id);
    }
}
