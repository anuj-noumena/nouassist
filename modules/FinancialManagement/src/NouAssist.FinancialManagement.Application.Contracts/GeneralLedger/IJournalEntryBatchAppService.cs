using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.Application.Contracts.GeneralLedger.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.Application.Contracts.GeneralLedger
{
    public interface IJournalEntryBatchAppService : IApplicationService
    {
        Task<JournalEntryBatchDto> GetAsync(Guid id);
        Task<PagedResultDto<JournalEntryBatchDto>> GetListAsync(GetJournalEntryBatchListDto input);
        Task<JournalEntryBatchDto> CreateAsync(CreateJournalEntryBatchDto input);
        Task<JournalEntryBatchDto> PostAsync(PostJournalEntryBatchDto input);
        Task<JournalEntryBatchDto> ReverseAsync(ReverseJournalEntryBatchDto input);
    }
}
