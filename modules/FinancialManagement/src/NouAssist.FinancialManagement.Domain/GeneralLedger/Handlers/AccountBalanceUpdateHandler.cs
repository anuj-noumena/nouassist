using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace NouAssist.FinancialManagement.GeneralLedger.Handlers
{
    public class AccountBalanceUpdateHandler :
        ILocalEventHandler<EntityCreatedEventData<JournalEntryBatch>>,
        ITransientDependency
    {
        private readonly ILedgerAccountRepository _accountRepository;

        public AccountBalanceUpdateHandler(ILedgerAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<JournalEntryBatch> eventData)
        {
            var batch = eventData.Entity;
            
            if (batch.Status != Enums.BatchStatus.Posted)
                return;

            foreach (var line in batch.Lines)
            {
                var account = await _accountRepository.GetAsync(line.AccountId);
                
                if (line.DebitAmount != null)
                {
                    account.UpdateBalance(line.DebitAmount, Shared.Enums.DebitCreditIndicator.Debit);
                }
                else if (line.CreditAmount != null)
                {
                    account.UpdateBalance(line.CreditAmount, Shared.Enums.DebitCreditIndicator.Credit);
                }
                
                await _accountRepository.UpdateAsync(account);
            }
        }
    }
}
