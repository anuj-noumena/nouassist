using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsReceivable
{
    [Authorize(FinancialManagementPermissions.AccountsReceivable.ProcessPayments)]
    public class CustomerPaymentAppService : ApplicationService, ICustomerPaymentAppService
    {
        private readonly IRepository<CustomerPayment, Guid> _paymentRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public CustomerPaymentAppService(
            IRepository<CustomerPayment, Guid> paymentRepository,
            IInvoiceRepository invoiceRepository)
        {
            _paymentRepository = paymentRepository;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CustomerPaymentDto> GetAsync(Guid id)
        {
            var payment = await _paymentRepository.GetAsync(id);
            return ObjectMapper.Map<CustomerPayment, CustomerPaymentDto>(payment);
        }

        public async Task<PagedResultDto<CustomerPaymentDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _paymentRepository.GetQueryableAsync();

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(p => p.PaymentDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var payments = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<CustomerPaymentDto>(
                totalCount,
                ObjectMapper.Map<System.Collections.Generic.List<CustomerPayment>, System.Collections.Generic.List<CustomerPaymentDto>>(payments)
            );
        }

        public async Task<CustomerPaymentDto> RecordAsync(RecordPaymentDto input)
        {
            var amount = new MonetaryValue(input.Amount, input.Currency);

            var payment = new CustomerPayment(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.CustomerId,
                amount,
                input.PaymentDate,
                input.PaymentMethod,
                input.Reference,
                PaymentSource.Direct);

            // Apply payments to invoices
            if (input.Applications != null && input.Applications.Any())
            {
                foreach (var application in input.Applications)
                {
                    var invoice = await _invoiceRepository.GetAsync(application.InvoiceId);
                    var appliedAmount = new MonetaryValue(application.Amount, input.Currency);
                    
                    payment.ApplyToInvoice(application.InvoiceId, appliedAmount);
                    invoice.ApplyPayment(payment.Id, appliedAmount, CurrentUser.Id.Value);
                    
                    await _invoiceRepository.UpdateAsync(invoice);
                }
            }

            await _paymentRepository.InsertAsync(payment);

            return ObjectMapper.Map<CustomerPayment, CustomerPaymentDto>(payment);
        }
    }
}
