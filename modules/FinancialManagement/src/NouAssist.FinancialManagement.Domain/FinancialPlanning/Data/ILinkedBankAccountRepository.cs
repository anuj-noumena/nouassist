using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning.Data
{
    public interface ILinkedBankAccountRepository : IRepository<LinkedBankAccount, Guid>
    {
        Task<List<LinkedBankAccount>> GetAccountsByUserAsync(Guid userId);
        Task<List<LinkedBankAccount>> GetActiveAccountsAsync(Guid userId);
        Task<List<LinkedBankAccount>> GetAccountsByTypeAsync(Guid userId, BankAccountType accountType);
        Task<List<LinkedBankAccount>> GetAccountsByStatusAsync(Guid userId, ConnectionStatus status);
        Task<LinkedBankAccount?> GetByAccountNumberAsync(string accountNumber, Guid userId);
        Task<decimal> GetTotalBalanceAsync(Guid userId);
    }
}
