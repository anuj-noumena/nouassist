using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using NouAssist.FinancialManagement.DebtManagement.Data;
using NouAssist.FinancialManagement.DebtManagement.DebtAccounts.Dtos;
using NouAssist.FinancialManagement.DebtManagement.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.DebtManagement.DebtAccounts
{
    [Authorize(DebtManagementPermissions.DebtAccounts.Default)]
    public class DebtAccountAppService : ApplicationService, IDebtAccountAppService
    {
        private readonly IDebtAccountRepository _debtAccountRepository;

        public DebtAccountAppService(IDebtAccountRepository debtAccountRepository)
        {
            _debtAccountRepository = debtAccountRepository;
        }

        public async Task<DebtAccountDto> GetAsync(Guid id)
        {
            var debtAccount = await _debtAccountRepository.GetWithPaymentsAsync(id);
            return ObjectMapper.Map<DebtAccount, DebtAccountDto>(debtAccount);
        }

        public async Task<PagedResultDto<DebtAccountDto>> GetListAsync(GetDebtAccountListDto input)
        {
            var userId = CurrentUser.GetId();
            
            var query = await _debtAccountRepository.GetQueryableAsync();
            query = query.Where(d => d.UserId == userId);

            if (input.Type.HasValue)
            {
                query = query.Where(d => d.Type == input.Type.Value);
            }

            if (input.Status.HasValue)
            {
                query = query.Where(d => d.Status == input.Status.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = query
                .OrderBy(input.Sorting ?? nameof(DebtAccount.CurrentBalance) + " desc")
                .PageBy(input.SkipCount, input.MaxResultCount);

            var debtAccounts = await AsyncExecuter.ToListAsync(query);

            return new PagedResultDto<DebtAccountDto>(
                totalCount,
                ObjectMapper.Map<List<DebtAccount>, List<DebtAccountDto>>(debtAccounts)
            );
        }

        [Authorize(DebtManagementPermissions.DebtAccounts.Create)]
        public async Task<DebtAccountDto> CreateAsync(CreateDebtAccountDto input)
        {
            var userId = CurrentUser.GetId();

            var debtAccount = new DebtAccount(
                GuidGenerator.Create(),
                userId,
                CurrentTenant.Id,
                input.DebtName,
                input.Type,
                input.OriginalAmount,
                input.CurrentBalance,
                input.InterestRate,
                input.MinimumPayment,
                input.PaymentFrequency
            );

            if (!string.IsNullOrWhiteSpace(input.AccountNumber))
            {
                // Set account number via reflection or add property setter
            }

            if (!string.IsNullOrWhiteSpace(input.CreditorName))
            {
                debtAccount.UpdateDebtDetails(creditorName: input.CreditorName);
            }

            await _debtAccountRepository.InsertAsync(debtAccount);

            return ObjectMapper.Map<DebtAccount, DebtAccountDto>(debtAccount);
        }

        [Authorize(DebtManagementPermissions.DebtAccounts.Update)]
        public async Task<DebtAccountDto> UpdateAsync(Guid id, UpdateDebtAccountDto input)
        {
            var debtAccount = await _debtAccountRepository.GetAsync(id);

            debtAccount.UpdateDebtDetails(
                input.DebtName,
                input.CreditorName,
                input.MinimumPayment
            );

            await _debtAccountRepository.UpdateAsync(debtAccount);

            return ObjectMapper.Map<DebtAccount, DebtAccountDto>(debtAccount);
        }

        [Authorize(DebtManagementPermissions.DebtAccounts.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _debtAccountRepository.DeleteAsync(id);
        }

        [Authorize(DebtManagementPermissions.DebtAccounts.RecordPayment)]
        public async Task<DebtAccountDto> RecordPaymentAsync(Guid id, RecordDebtPaymentDto input)
        {
            var debtAccount = await _debtAccountRepository.GetWithPaymentsAsync(id);

            debtAccount.RecordPayment(
                input.PaymentDate,
                input.PaymentAmount,
                input.TransactionReference
            );

            await _debtAccountRepository.UpdateAsync(debtAccount);

            return ObjectMapper.Map<DebtAccount, DebtAccountDto>(debtAccount);
        }

        public async Task<PayoffProjectionDto> CalculatePayoffProjectionAsync(
            Guid id, 
            decimal monthlyPayment)
        {
            var debtAccount = await _debtAccountRepository.GetAsync(id);
            var projection = debtAccount.CalculatePayoffProjection(monthlyPayment);

            return new PayoffProjectionDto
            {
                ProjectedPayoffDate = projection.ProjectedPayoffDate,
                MonthsToPayoff = projection.MonthsToPayoff,
                TotalInterestToPay = projection.TotalInterestToPay,
                TotalAmountToPay = projection.TotalAmountToPay,
                MonthlyPayment = projection.MonthlyPayment
            };
        }

        public async Task<decimal> GetTotalDebtBalanceAsync()
        {
            var userId = CurrentUser.GetId();
            return await _debtAccountRepository.GetTotalDebtBalanceAsync(userId);
        }
    }
}
