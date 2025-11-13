using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using NouAssist.FinancialManagement.GeneralLedger.Services;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.GeneralLedger
{
    [Authorize(FinancialManagementPermissions.GeneralLedger.Default)]
    public class JournalEntryAppService : ApplicationService, IJournalEntryAppService
    {
        private readonly IJournalEntryRepository _journalEntryRepository;
        private readonly ILedgerAccountRepository _ledgerAccountRepository;
        private readonly JournalEntryValidationService _validationService;

        public JournalEntryAppService(
            IJournalEntryRepository journalEntryRepository,
            ILedgerAccountRepository ledgerAccountRepository,
            JournalEntryValidationService validationService)
        {
            _journalEntryRepository = journalEntryRepository;
            _ledgerAccountRepository = ledgerAccountRepository;
            _validationService = validationService;
        }

        public async Task<JournalEntryBatchDto> GetAsync(Guid id)
        {
            var batch = await _journalEntryRepository.GetWithLinesAsync(id);
            return await MapToDto(batch);
        }

        public async Task<PagedResultDto<JournalEntryBatchDto>> GetListAsync(GetJournalEntryListDto input)
        {
            var queryable = await _journalEntryRepository.GetQueryableAsync();

            if (input.StartDate.HasValue)
            {
                queryable = queryable.Where(b => b.PostingDate >= input.StartDate.Value);
            }

            if (input.EndDate.HasValue)
            {
                queryable = queryable.Where(b => b.PostingDate <= input.EndDate.Value);
            }

            if (input.Status.HasValue)
            {
                queryable = queryable.Where(b => b.Status == input.Status.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(b => b.PostingDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var batches = await AsyncExecuter.ToListAsync(queryable);
            var dtos = new List<JournalEntryBatchDto>();

            foreach (var batch in batches)
            {
                dtos.Add(await MapToDto(batch));
            }

            return new PagedResultDto<JournalEntryBatchDto>(totalCount, dtos);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.PostJournalEntries)]
        public async Task<JournalEntryBatchDto> CreateAsync(CreateJournalEntryBatchDto input)
        {
            var batchNumber = await GenerateBatchNumber();

            var batch = new JournalEntryBatch(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                batchNumber,
                input.PostingDate,
                input.Description,
                input.Reference);

            foreach (var lineDto in input.Lines)
            {
                var debitAmount = lineDto.DebitAmount.HasValue
                    ? new MonetaryValue(lineDto.DebitAmount.Value, lineDto.Currency)
                    : null;

                var creditAmount = lineDto.CreditAmount.HasValue
                    ? new MonetaryValue(lineDto.CreditAmount.Value, lineDto.Currency)
                    : null;

                var line = new JournalEntryLine(
                    batch.Id,
                    lineDto.AccountId,
                    debitAmount,
                    creditAmount,
                    lineDto.Memo,
                    lineDto.Reference);

                batch.AddLine(line);
            }

            // Validate before saving
            var validationResult = await _validationService.ValidateJournalEntryBatchAsync(batch);
            if (!validationResult.IsValid)
            {
                throw new Volo.Abp.UserFriendlyException(
                    string.Join("; ", validationResult.Errors));
            }

            await _journalEntryRepository.InsertAsync(batch);

            return await MapToDto(batch);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.PostJournalEntries)]
        public async Task<JournalEntryBatchDto> PostAsync(Guid id)
        {
            var batch = await _journalEntryRepository.GetWithLinesAsync(id);

            // Validate before posting
            var validationResult = await _validationService.ValidateJournalEntryBatchAsync(batch);
            if (!validationResult.IsValid)
            {
                throw new Volo.Abp.UserFriendlyException(
                    string.Join("; ", validationResult.Errors));
            }

            batch.Post(CurrentUser.Id.Value);

            // Update account balances
            foreach (var line in batch.Lines)
            {
                var account = await _ledgerAccountRepository.GetAsync(line.AccountId);
                var amount = line.DebitAmount ?? line.CreditAmount;
                var indicator = line.DebitAmount != null 
                    ? Enums.DebitCreditIndicator.Debit 
                    : Enums.DebitCreditIndicator.Credit;

                account.UpdateBalance(amount, indicator);
                await _ledgerAccountRepository.UpdateAsync(account);
            }

            await _journalEntryRepository.UpdateAsync(batch);

            return await MapToDto(batch);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ReverseJournalEntries)]
        public async Task<JournalEntryBatchDto> ReverseAsync(Guid id, string reason)
        {
            var originalBatch = await _journalEntryRepository.GetWithLinesAsync(id);
            var reversalBatch = originalBatch.Reverse(CurrentUser.Id.Value, Clock.Now, reason);

            await _journalEntryRepository.UpdateAsync(originalBatch);
            await _journalEntryRepository.InsertAsync(reversalBatch);

            // Auto-post the reversal
            reversalBatch.Post(CurrentUser.Id.Value);

            // Update account balances for reversal
            foreach (var line in reversalBatch.Lines)
            {
                var account = await _ledgerAccountRepository.GetAsync(line.AccountId);
                var amount = line.DebitAmount ?? line.CreditAmount;
                var indicator = line.DebitAmount != null 
                    ? Enums.DebitCreditIndicator.Debit 
                    : Enums.DebitCreditIndicator.Credit;

                account.UpdateBalance(amount, indicator);
                await _ledgerAccountRepository.UpdateAsync(account);
            }

            await _journalEntryRepository.UpdateAsync(reversalBatch);

            return await MapToDto(reversalBatch);
        }

        private async Task<JournalEntryBatchDto> MapToDto(JournalEntryBatch batch)
        {
            var dto = ObjectMapper.Map<JournalEntryBatch, JournalEntryBatchDto>(batch);

            // Load account information for each line
            var accountIds = batch.Lines.Select(l => l.AccountId).Distinct();
            var accounts = await _ledgerAccountRepository.GetListAsync(a => accountIds.Contains(a.Id));

            foreach (var lineDto in dto.Lines)
            {
                var account = accounts.FirstOrDefault(a => a.Id == lineDto.AccountId);
                if (account != null)
                {
                    lineDto.AccountNumber = account.AccountNumber;
                    lineDto.AccountName = account.Name;
                }
            }

            return dto;
        }

        private async Task<string> GenerateBatchNumber()
        {
            var now = Clock.Now;
            var prefix = $"JE-{now:yyyyMM}";
            
            var queryable = await _journalEntryRepository.GetQueryableAsync();
            var count = await AsyncExecuter.CountAsync(
                queryable.Where(b => b.BatchNumber.StartsWith(prefix)));

            return $"{prefix}-{(count + 1):D4}";
        }
    }
}
