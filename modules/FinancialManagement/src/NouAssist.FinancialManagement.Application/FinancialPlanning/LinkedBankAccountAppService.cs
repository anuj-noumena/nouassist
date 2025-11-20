using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Dtos;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning
{
    [Authorize(FinancialManagementPermissions.FinancialPlanning.Default)]
    public class LinkedBankAccountAppService : ApplicationService, ILinkedBankAccountAppService
    {
        private readonly IRepository<LinkedBankAccount, Guid> _accountRepository;
        private readonly ILinkedBankAccountRepository _customAccountRepository;

        public LinkedBankAccountAppService(
            IRepository<LinkedBankAccount, Guid> accountRepository,
            ILinkedBankAccountRepository customAccountRepository)
        {
            _accountRepository = accountRepository;
            _customAccountRepository = customAccountRepository;
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ViewAccounts)]
        public async Task<LinkedBankAccountDto> GetAsync(Guid id)
        {
            var account = await _accountRepository.GetAsync(id);
            return ObjectMapper.Map<LinkedBankAccount, LinkedBankAccountDto>(account);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ViewAccounts)]
        public async Task<PagedResultDto<LinkedBankAccountDto>> GetListAsync(GetLinkedBankAccountListDto input)
        {
            var queryable = await _accountRepository.GetQueryableAsync();

            queryable = queryable.Where(a => a.UserId == CurrentUser.Id.Value);

            if (input.TypeFilter.HasValue)
            {
                queryable = queryable.Where(a => a.AccountType == input.TypeFilter.Value);
            }

            if (input.StatusFilter.HasValue)
            {
                queryable = queryable.Where(a => a.ConnectionStatus == input.StatusFilter.Value);
            }

            if (input.ActiveOnly == true)
            {
                queryable = queryable.Where(a => a.IsActive);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderBy(a => a.AccountName)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var accounts = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<LinkedBankAccountDto>(
                totalCount,
                ObjectMapper.Map<List<LinkedBankAccount>, List<LinkedBankAccountDto>>(accounts)
            );
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.CreateAccounts)]
        public async Task<LinkedBankAccountDto> CreateAsync(CreateLinkedBankAccountDto input)
        {
            var account = new LinkedBankAccount(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                CurrentUser.Id.Value,
                input.BankName,
                input.AccountName,
                input.AccountNumber,
                input.AccountType,
                input.Currency
            );

            await _accountRepository.InsertAsync(account, autoSave: true);

            return ObjectMapper.Map<LinkedBankAccount, LinkedBankAccountDto>(account);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.UpdateAccounts)]
        public async Task<LinkedBankAccountDto> UpdateBalanceAsync(UpdateBankAccountBalanceDto input)
        {
            var account = await _accountRepository.GetAsync(input.AccountId);
            var newBalance = new MonetaryValue(input.NewBalance, account.CurrentBalance.Currency);
            account.UpdateBalance(newBalance);
            await _accountRepository.UpdateAsync(account, autoSave: true);

            return ObjectMapper.Map<LinkedBankAccount, LinkedBankAccountDto>(account);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.ViewAccounts)]
        public async Task<decimal> GetTotalBalanceAsync()
        {
            return await _customAccountRepository.GetTotalBalanceAsync(CurrentUser.Id.Value);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.UpdateAccounts)]
        public async Task DisconnectAsync(Guid id)
        {
            var account = await _accountRepository.GetAsync(id);
            account.Disconnect();
            await _accountRepository.UpdateAsync(account, autoSave: true);
        }

        [Authorize(FinancialManagementPermissions.FinancialPlanning.UpdateAccounts)]
        public async Task ReconnectAsync(Guid id)
        {
            var account = await _accountRepository.GetAsync(id);
            account.Reconnect();
            await _accountRepository.UpdateAsync(account, autoSave: true);
        }
    }
}
