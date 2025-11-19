using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.GeneralLedger
{
    [Authorize(FinancialManagementPermissions.GeneralLedger.Default)]
    public class BankStatementAppService : ApplicationService, IBankStatementAppService
    {
        private readonly IRepository<BankStatement, Guid> _statementRepository;
        private readonly IRepository<LedgerAccount, Guid> _ledgerAccountRepository;

        public BankStatementAppService(
            IRepository<BankStatement, Guid> statementRepository,
            IRepository<LedgerAccount, Guid> ledgerAccountRepository)
        {
            _statementRepository = statementRepository;
            _ledgerAccountRepository = ledgerAccountRepository;
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ViewStatements)]
        public async Task<BankStatementDto> GetAsync(Guid id)
        {
            var queryable = await _statementRepository.WithDetailsAsync();
            var statement = await AsyncExecuter.FirstOrDefaultAsync(
                queryable.Where(s => s.Id == id));

            return ObjectMapper.Map<BankStatement, BankStatementDto>(statement);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ViewStatements)]
        public async Task<PagedResultDto<BankStatementDto>> GetListAsync(GetBankStatementListDto input)
        {
            var queryable = await _statementRepository.GetQueryableAsync();

            if (input.BankAccountFilter.HasValue)
            {
                queryable = queryable.Where(s => s.BankAccountId == input.BankAccountFilter.Value);
            }

            if (input.StatusFilter.HasValue)
            {
                queryable = queryable.Where(s => s.Status == input.StatusFilter.Value);
            }

            if (input.StartDate.HasValue)
            {
                queryable = queryable.Where(s => s.StatementDate >= input.StartDate.Value);
            }

            if (input.EndDate.HasValue)
            {
                queryable = queryable.Where(s => s.StatementDate <= input.EndDate.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(s => s.StatementDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var statements = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<BankStatementDto>(
                totalCount,
                ObjectMapper.Map<List<BankStatement>, List<BankStatementDto>>(statements)
            );
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.CreateStatements)]
        public async Task<BankStatementDto> CreateAsync(CreateBankStatementDto input)
        {
            var statement = new BankStatement(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.BankAccountId,
                input.StatementDate,
                input.StartDate,
                input.EndDate,
                new MonetaryValue(input.OpeningBalance, input.Currency),
                new MonetaryValue(input.ClosingBalance, input.Currency)
            );

            await _statementRepository.InsertAsync(statement, autoSave: true);

            return ObjectMapper.Map<BankStatement, BankStatementDto>(statement);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ImportStatements)]
        public async Task<BankStatementDto> ImportAsync(ImportBankStatementDto input)
        {
            // Note: This is a simplified implementation
            // In production, you'd parse the file content based on format
            throw new NotImplementedException(
                "Bank statement import requires file parsing logic for the specified format");
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ReconcileStatements)]
        public async Task<BankStatementDto> MatchLineAsync(MatchStatementLineDto input)
        {
            var statement = await _statementRepository.GetAsync(input.StatementId);
            statement.MatchLine(input.LineId, input.JournalBatchId, CurrentUser.Id.Value);
            await _statementRepository.UpdateAsync(statement, autoSave: true);

            return ObjectMapper.Map<BankStatement, BankStatementDto>(statement);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ReconcileStatements)]
        public async Task<BankStatementDto> UnmatchLineAsync(Guid statementId, Guid lineId)
        {
            var statement = await _statementRepository.GetAsync(statementId);
            statement.UnmatchLine(lineId);
            await _statementRepository.UpdateAsync(statement, autoSave: true);

            return ObjectMapper.Map<BankStatement, BankStatementDto>(statement);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ReconcileStatements)]
        public async Task<BankStatementDto> ReconcileAsync(Guid id)
        {
            var statement = await _statementRepository.GetAsync(id);
            statement.Reconcile(CurrentUser.Id.Value);
            await _statementRepository.UpdateAsync(statement, autoSave: true);

            return ObjectMapper.Map<BankStatement, BankStatementDto>(statement);
        }
    }
}
