using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using NouAssist.FinancialManagement.GeneralLedger.Dtos;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.GeneralLedger
{
    [Authorize(FinancialManagementPermissions.GeneralLedger.ViewAccounts)]
    public class LedgerAccountAppService : ApplicationService, ILedgerAccountAppService
    {
        private readonly ILedgerAccountRepository _ledgerAccountRepository;

        public LedgerAccountAppService(ILedgerAccountRepository ledgerAccountRepository)
        {
            _ledgerAccountRepository = ledgerAccountRepository;
        }

        public async Task<LedgerAccountDto> GetAsync(Guid id)
        {
            var account = await _ledgerAccountRepository.GetAsync(id);
            return ObjectMapper.Map<LedgerAccount, LedgerAccountDto>(account);
        }

        public async Task<PagedResultDto<LedgerAccountDto>> GetListAsync(GetLedgerAccountListDto input)
        {
            var queryable = await _ledgerAccountRepository.GetQueryableAsync();

            if (input.Type.HasValue)
            {
                queryable = queryable.Where(a => a.Type == input.Type.Value);
            }

            if (input.ActiveOnly.GetValueOrDefault(true))
            {
                queryable = queryable.Where(a => a.IsActive);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = ApplySorting(queryable, input);
            queryable = ApplyPaging(queryable, input);

            var accounts = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<LedgerAccountDto>(
                totalCount,
                ObjectMapper.Map<List<LedgerAccount>, List<LedgerAccountDto>>(accounts)
            );
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ManageAccounts)]
        public async Task<LedgerAccountDto> CreateAsync(CreateLedgerAccountDto input)
        {
            // Check if account number already exists
            var exists = await _ledgerAccountRepository.AccountNumberExistsAsync(input.AccountNumber);
            if (exists)
            {
                throw new Volo.Abp.UserFriendlyException($"Account number {input.AccountNumber} already exists");
            }

            var account = new LedgerAccount(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.AccountNumber,
                input.Name,
                input.Type,
                input.Currency,
                input.ParentAccountId,
                input.Description);

            await _ledgerAccountRepository.InsertAsync(account);

            return ObjectMapper.Map<LedgerAccount, LedgerAccountDto>(account);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ManageAccounts)]
        public async Task<LedgerAccountDto> UpdateAsync(Guid id, UpdateLedgerAccountDto input)
        {
            var account = await _ledgerAccountRepository.GetAsync(id);

            account.UpdateName(input.Name);
            account.UpdateDescription(input.Description);

            await _ledgerAccountRepository.UpdateAsync(account);

            return ObjectMapper.Map<LedgerAccount, LedgerAccountDto>(account);
        }

        [Authorize(FinancialManagementPermissions.GeneralLedger.ManageAccounts)]
        public async Task DeleteAsync(Guid id)
        {
            var account = await _ledgerAccountRepository.GetAsync(id);
            account.Deactivate();
            await _ledgerAccountRepository.UpdateAsync(account);
        }

        private IQueryable<LedgerAccount> ApplySorting(IQueryable<LedgerAccount> queryable, GetLedgerAccountListDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                return queryable.OrderBy(a => a.AccountNumber);
            }

            return queryable.OrderBy(input.Sorting);
        }

        private IQueryable<LedgerAccount> ApplyPaging(IQueryable<LedgerAccount> queryable, GetLedgerAccountListDto input)
        {
            return queryable
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);
        }
    }
}
