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
        Task<PagedResultDto<BankStatementDto>> GetListAsync(GetBankStatementListDto input);
        Task<BankStatementDto> CreateAsync(CreateBankStatementDto input);
        Task<BankStatementDto> ImportAsync(ImportBankStatementDto input);
        Task<BankStatementDto> MatchLineAsync(MatchStatementLineDto input);
        Task<BankStatementDto> UnmatchLineAsync(Guid statementId, Guid lineId);
        Task<BankStatementDto> ReconcileAsync(Guid id);
    }
}
