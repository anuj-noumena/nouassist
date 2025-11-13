using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.GeneralLedger
{
    public interface IBankStatementAppService : IApplicationService
    {
        Task<BankStatementDto> GetAsync(Guid id);
        Task<PagedResultDto<BankStatementDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<BankStatementDto> ImportAsync(ImportBankStatementDto input);
        Task<BankStatementDto> MatchLineAsync(Guid statementId, MatchBankStatementLineDto input);
        Task<BankStatementDto> UnmatchLineAsync(Guid statementId, Guid lineId);
    }
}
