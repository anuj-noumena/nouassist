using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.GeneralLedger.Services
{
    public class JournalEntryValidationService : DomainService
    {
        private readonly ILedgerAccountRepository _accountRepository;
        private readonly IAccountingPeriodRepository _periodRepository;

        public JournalEntryValidationService(
            ILedgerAccountRepository accountRepository,
            IAccountingPeriodRepository periodRepository)
        {
            _accountRepository = accountRepository;
            _periodRepository = periodRepository;
        }

        public async Task<ValidationResult> ValidateJournalEntryBatchAsync(
            JournalEntryBatch batch)
        {
            var errors = new List<string>();

            // Basic structure validation
            if (batch.Lines.Count < 2)
            {
                errors.Add("Journal entry batch must have at least 2 lines");
            }

            if (string.IsNullOrWhiteSpace(batch.Description))
            {
                errors.Add("Description is required");
            }

            // Validate account references
            var accountValidation = await ValidateAccountReferencesAsync(batch);
            errors.AddRange(accountValidation);

            // Validate balancing
            if (!batch.IsBalanced)
            {
                errors.Add("Journal entry batch is not balanced");
            }

            // Validate accounting period
            var periodValidation = await ValidateAccountingPeriodAsync(batch.PostingDate);
            errors.AddRange(periodValidation);

            return new ValidationResult(errors.Count == 0, errors);
        }

        private async Task<List<string>> ValidateAccountReferencesAsync(
            JournalEntryBatch batch)
        {
            var errors = new List<string>();
            var accountIds = batch.Lines.Select(l => l.AccountId).Distinct();

            foreach (var accountId in accountIds)
            {
                var account = await _accountRepository.FindAsync(accountId);
                if (account == null)
                {
                    errors.Add($"Account {accountId} does not exist");
                }
                else if (!account.IsActive)
                {
                    errors.Add($"Account {account.AccountNumber} is inactive");
                }
                else if (!account.AllowsPosting)
                {
                    errors.Add($"Account {account.AccountNumber} does not allow posting");
                }
            }

            return errors;
        }

        private async Task<List<string>> ValidateAccountingPeriodAsync(DateTime postingDate)
        {
            var errors = new List<string>();
            var period = await _periodRepository.GetPeriodForDateAsync(postingDate);

            if (period == null)
            {
                errors.Add($"No accounting period found for date {postingDate:yyyy-MM-dd}");
            }
            else if (period.Status != Enums.PeriodStatus.Open)
            {
                errors.Add($"Cannot post to closed period");
            }

            return errors;
        }
    }
}
