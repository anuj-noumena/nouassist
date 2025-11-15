using System;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.Shared.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.GeneralLedger.Services
{
    public class AccountBalanceCalculationService : DomainService
    {
        public MonetaryValue CalculateBalance(
            LedgerAccount account,
            JournalEntryBatch[] postedBatches,
            DateTime? asOfDate = null)
        {
            var startBalance = account.CurrentBalance;
            var linesForAccount = postedBatches
                .Where(b => !asOfDate.HasValue || b.PostingDate.Date <= asOfDate.Value)
                .SelectMany(b => b.Lines)
                .Where(l => l.AccountId == account.Id);

            var balance = startBalance;

            foreach (var line in linesForAccount.OrderBy(l => l.BatchId))
            {
                if (line.DebitAmount != null)
                {
                    balance = balance.Add(line.DebitAmount);
                }
                else if (line.CreditAmount != null)
                {
                    balance = balance.Subtract(line.CreditAmount);
                }
            }

            return balance;
        }

        public bool IsBalanceValid(LedgerAccount account, MonetaryValue calculatedBalance)
        {
            return Math.Abs(account.CurrentBalance.Amount - calculatedBalance.Amount) < 0.01m;
        }
    }
}
