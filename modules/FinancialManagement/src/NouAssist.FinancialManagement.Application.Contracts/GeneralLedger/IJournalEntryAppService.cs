using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.GeneralLedger
{
    public interface IJournalEntryAppService : IApplicationService
    {
        Task<JournalEntryBatchDto> GetAsync(Guid id);
        Task<PagedResultDto<JournalEntryBatchDto>> GetListAsync(GetJournalEntryListDto input);
        Task<JournalEntryBatchDto> CreateAsync(CreateJournalEntryBatchDto input);
        Task<JournalEntryBatchDto> PostAsync(Guid id);
        Task<JournalEntryBatchDto> ReverseAsync(Guid id, string reason);
    }
}
