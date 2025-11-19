using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.GeneralLedger
{
    [Authorize(FinancialManagementPermissions.GeneralLedger.Default)]
    public class LedgerAccountAppService : ApplicationService, ILedgerAccountAppService
    {
        private readonly ILedgerAccountRepository _ledgerAccountRepository;

        public LedgerAccountAppService(ILedgerAccountRepository ledgerAccountRepository)
        {
            _ledgerAccountRepository = ledgerAccountRepository;
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ViewAccounts)]
        public async Task<LedgerAccountDto> GetAsync(Guid id)
        {
            var account = await _ledgerAccountRepository.GetAsync(id);
            return ObjectMapper.Map<LedgerAccount, LedgerAccountDto>(account);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ViewAccounts)]
        public async Task<PagedResultDto<LedgerAccountDto>> GetListAsync(GetLedgerAccountListDto input)
        {
            var queryable = await _ledgerAccountRepository.GetQueryableAsync();

            if (input.TypeFilter.HasValue)
            {
                queryable = queryable.Where(a => a.Type == input.TypeFilter.Value);
            }

            if (input.ActiveOnly == true)
            {
                queryable = queryable.Where(a => a.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(input.SearchTerm))
            {
                queryable = queryable.Where(a =>
                    a.Name.Contains(input.SearchTerm) ||
                    a.AccountNumber.Number.Contains(input.SearchTerm));
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderBy(a => a.AccountNumber.Number)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var accounts = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<LedgerAccountDto>(
                totalCount,
                ObjectMapper.Map<List<LedgerAccount>, List<LedgerAccountDto>>(accounts)
            );
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ManageAccounts)]
        public async Task<LedgerAccountDto> CreateAsync(CreateLedgerAccountDto input)
        {
            var accountNumber = new AccountNumber(input.AccountNumber);

            // Check uniqueness
            var isUnique = await _ledgerAccountRepository.IsAccountNumberUniqueAsync(accountNumber);
            if (!isUnique)
            {
                throw new BusinessException("GL:AccountNumberExists",
                    "Account number already exists");
            }

            var account = new LedgerAccount(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                accountNumber,
                input.Name,
                input.Type,
                input.Currency,
                input.ParentAccountId,
                input.Description
            );

            await _ledgerAccountRepository.InsertAsync(account);
            return ObjectMapper.Map<LedgerAccount, LedgerAccountDto>(account);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ManageAccounts)]
        public async Task<LedgerAccountDto> UpdateAsync(Guid id, UpdateLedgerAccountDto input)
        {
            var account = await _ledgerAccountRepository.GetAsync(id);

            account.UpdateName(input.Name);
            account.UpdateDescription(input.Description);

            if (input.IsActive && !account.IsActive)
            {
                account.Activate();
            }
            else if (!input.IsActive && account.IsActive)
            {
                account.Deactivate();
            }

            await _ledgerAccountRepository.UpdateAsync(account);
            return ObjectMapper.Map<LedgerAccount, LedgerAccountDto>(account);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ManageAccounts)]
        public async Task DeleteAsync(Guid id)
        {
            await _ledgerAccountRepository.DeleteAsync(id);
        }
    }
}
