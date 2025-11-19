using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.Localization;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.GeneralLedger.Services
{
    public class JournalEntryValidationService : DomainService
    {
        private readonly IStringLocalizer<FinancialManagementResource> _localizer;
        
        public JournalEntryValidationService(IStringLocalizer<FinancialManagementResource> localizer)
        {
            _localizer = localizer;
        }
        
        public ValidationResult ValidateForPosting(JournalEntryBatch batch)
        {
            var errors = new List<string>();

            // Basic structure validation
            if (batch.Lines.Count < 2)
            {
                errors.Add(_localizer["GL:MinimumTwoLines"]);
            }

            // Balance validation
            if (!batch.IsBalanced())
            {
                errors.Add(_localizer["GL:BatchNotBalanced"]);
            }

            // Line validation
            foreach (var line in batch.Lines)
            {
                if (line.DebitAmount == null && line.CreditAmount == null)
                {
                    errors.Add(_localizer["GL:LineNeedsAmount"]);
                }

                if (line.DebitAmount != null && line.CreditAmount != null)
                {
                    errors.Add(_localizer["GL:LineBothAmounts"]);
                }
            }

            return new ValidationResult(errors.Count == 0, errors);
        }

        public Task<ValidationResult> ValidateAccountStructure(
            LedgerAccount account,
            LedgerAccount parentAccount)
        {
            var errors = new List<string>();

            if (parentAccount != null)
            {
                // Parent must be same account type or allow children
                if (parentAccount.Type != account.Type)
                {
                    errors.Add(_localizer["GL:ParentTypeMismatch"]);
                }

                // Check for circular references (would be done via repository in real scenario)
                // This is simplified - in production, check full parent chain
            }

            return Task.FromResult(new ValidationResult(errors.Count == 0, errors));
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; }
        public List<string> Errors { get; }

        public ValidationResult(bool isValid, List<string> errors)
        {
            IsValid = isValid;
            Errors = errors ?? new List<string>();
        }
    }
}
