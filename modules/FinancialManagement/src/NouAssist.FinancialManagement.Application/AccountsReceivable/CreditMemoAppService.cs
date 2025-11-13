using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using NouAssist.FinancialManagement.AccountsReceivable.Services;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsReceivable
{
    [Authorize(FinancialManagementPermissions.AccountsReceivable.Default)]
    public class CreditMemoAppService : ApplicationService, ICreditMemoAppService
    {
        private readonly IRepository<CreditMemo, Guid> _creditMemoRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly CreditManagementService _creditManagementService;

        public CreditMemoAppService(
            IRepository<CreditMemo, Guid> creditMemoRepository,
            ICustomerRepository customerRepository,
            IInvoiceRepository invoiceRepository,
            CreditManagementService creditManagementService)
        {
            _creditMemoRepository = creditMemoRepository;
            _customerRepository = customerRepository;
            _invoiceRepository = invoiceRepository;
            _creditManagementService = creditManagementService;
        }

        public async Task<CreditMemoDto> GetAsync(Guid id)
        {
            var creditMemo = await _creditMemoRepository.GetAsync(id);
            return await MapToDtoAsync(creditMemo);
        }

        public async Task<PagedResultDto<CreditMemoDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _creditMemoRepository.GetQueryableAsync();

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(cm => cm.IssueDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var creditMemos = await AsyncExecuter.ToListAsync(queryable);
            var dtos = new List<CreditMemoDto>();

            foreach (var memo in creditMemos)
            {
                dtos.Add(await MapToDtoAsync(memo));
            }

            return new PagedResultDto<CreditMemoDto>(totalCount, dtos);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.CreateInvoices)]
        public async Task<CreditMemoDto> CreateAsync(CreateCreditMemoDto input)
        {
            // Validate customer exists
            var customer = await _customerRepository.GetAsync(input.CustomerId);

            // Generate credit memo number
            var creditMemoNumber = await GenerateCreditMemoNumberAsync();

            var amount = new MonetaryValue(input.Amount, input.Currency);

            var creditMemo = new CreditMemo(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                creditMemoNumber,
                input.CustomerId,
                amount,
                input.IssueDate,
                input.Reason,
                input.Description,
                input.OriginalInvoiceId);

            // Update customer balance (reduce what they owe)
            customer.SubtractFromBalance(amount);
            await _customerRepository.UpdateAsync(customer);

            await _creditMemoRepository.InsertAsync(creditMemo);

            return await MapToDtoAsync(creditMemo);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ProcessPayments)]
        public async Task<CreditMemoDto> ApplyToInvoiceAsync(Guid creditMemoId, Guid invoiceId)
        {
            var creditMemo = await _creditMemoRepository.GetAsync(creditMemoId);
            var invoice = await _invoiceRepository.GetWithDetailsAsync(invoiceId);
            var customer = await _customerRepository.GetAsync(creditMemo.CustomerId);

            // Validate
            if (creditMemo.IsApplied)
            {
                throw new Volo.Abp.UserFriendlyException("Credit memo has already been applied");
            }

            if (invoice.CustomerId != creditMemo.CustomerId)
            {
                throw new Volo.Abp.UserFriendlyException(
                    "Credit memo and invoice must be for the same customer");
            }

            if (invoice.Status == Enums.InvoiceStatus.Paid || 
                invoice.Status == Enums.InvoiceStatus.Cancelled)
            {
                throw new Volo.Abp.UserFriendlyException(
                    $"Cannot apply credit memo to invoice with status {invoice.Status}");
            }

            // Apply credit memo to invoice
            var amountToApply = new MonetaryValue(
                Math.Min(creditMemo.Amount.Amount, invoice.Balance.Amount),
                creditMemo.Amount.Currency);

            invoice.ApplyPayment(creditMemo.Id, amountToApply, CurrentUser.Id.Value);
            creditMemo.Apply();

            await _invoiceRepository.UpdateAsync(invoice);
            await _creditMemoRepository.UpdateAsync(creditMemo);

            return await MapToDtoAsync(creditMemo);
        }

        private async Task<CreditMemoDto> MapToDtoAsync(CreditMemo creditMemo)
        {
            var dto = ObjectMapper.Map<CreditMemo, CreditMemoDto>(creditMemo);

            // Load customer name
            var customer = await _customerRepository.GetAsync(creditMemo.CustomerId);
            dto.CustomerName = customer.Name;

            return dto;
        }

        private async Task<string> GenerateCreditMemoNumberAsync()
        {
            var now = Clock.Now;
            var prefix = $"CM-{now:yyyyMM}";
            
            var queryable = await _creditMemoRepository.GetQueryableAsync();
            var count = await AsyncExecuter.CountAsync(
                queryable.Where(cm => cm.CreditMemoNumber.StartsWith(prefix)));

            return $"{prefix}-{(count + 1):D4}";
        }
    }
}
