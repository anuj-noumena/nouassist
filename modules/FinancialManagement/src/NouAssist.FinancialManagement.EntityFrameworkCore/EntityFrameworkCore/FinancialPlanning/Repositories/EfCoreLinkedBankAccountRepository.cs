using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.FinancialPlanning.Repositories
{
    public class EfCoreLinkedBankAccountRepository
        : EfCoreRepository<IFinancialManagementDbContext, LinkedBankAccount, Guid>,
          ILinkedBankAccountRepository
    {
        public EfCoreLinkedBankAccountRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<LinkedBankAccount>> GetAccountsByUserAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.LinkedBankAccounts
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.AccountName)
                .ToListAsync();
        }

        public async Task<List<LinkedBankAccount>> GetActiveAccountsAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.LinkedBankAccounts
                .Where(a => a.UserId == userId && a.IsActive)
                .OrderBy(a => a.AccountName)
                .ToListAsync();
        }

        public async Task<List<LinkedBankAccount>> GetAccountsByTypeAsync(Guid userId, BankAccountType accountType)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.LinkedBankAccounts
                .Where(a => a.UserId == userId && a.AccountType == accountType)
                .OrderBy(a => a.AccountName)
                .ToListAsync();
        }

        public async Task<List<LinkedBankAccount>> GetAccountsByStatusAsync(Guid userId, ConnectionStatus status)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.LinkedBankAccounts
                .Where(a => a.UserId == userId && a.Status == status)
                .OrderBy(a => a.AccountName)
                .ToListAsync();
        }

        public async Task<LinkedBankAccount?> GetByAccountNumberAsync(string accountNumber, Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.LinkedBankAccounts
                .FirstOrDefaultAsync(a => a.UserId == userId && a.AccountNumber == accountNumber);
        }

        public async Task<decimal> GetTotalBalanceAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.LinkedBankAccounts
                .Where(a => a.UserId == userId && a.IsActive)
                .SumAsync(a => a.CurrentBalance.Amount);
        }
    }
}
