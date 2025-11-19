using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsReceivable
{
    [Authorize(FinancialManagementPermissions.AccountsReceivable.Default)]
    public class CustomerPaymentAppService : ApplicationService, ICustomerPaymentAppService
    {
        private readonly IRepository<CustomerPayment, Guid> _paymentRepository;
        private readonly ICustomerPaymentRepository _customPaymentRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;

        public CustomerPaymentAppService(
            IRepository<CustomerPayment, Guid> paymentRepository,
            ICustomerPaymentRepository customPaymentRepository,
            IRepository<Customer, Guid> customerRepository)
        {
            _paymentRepository = paymentRepository;
            _customPaymentRepository = customPaymentRepository;
            _customerRepository = customerRepository;
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ViewPayments)]
        public async Task<CustomerPaymentDto> GetAsync(Guid id)
        {
            var payment = await _paymentRepository.GetAsync(id);
            return ObjectMapper.Map<CustomerPayment, CustomerPaymentDto>(payment);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ViewPayments)]
        public async Task<PagedResultDto<CustomerPaymentDto>> GetListAsync(GetCustomerPaymentListDto input)
        {
            var queryable = await _paymentRepository.GetQueryableAsync();

            if (input.CustomerFilter.HasValue)
            {
                queryable = queryable.Where(p => p.CustomerId == input.CustomerFilter.Value);
            }

            if (input.SourceFilter.HasValue)
            {
                queryable = queryable.Where(p => p.PaymentSource == input.SourceFilter.Value);
            }

            if (input.StartDate.HasValue)
            {
                queryable = queryable.Where(p => p.PaymentDate >= input.StartDate.Value);
            }

            if (input.EndDate.HasValue)
            {
                queryable = queryable.Where(p => p.PaymentDate <= input.EndDate.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(p => p.PaymentDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var payments = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<CustomerPaymentDto>(
                totalCount,
                ObjectMapper.Map<List<CustomerPayment>, List<CustomerPaymentDto>>(payments)
            );
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.CreatePayments)]
        public async Task<CustomerPaymentDto> CreateAsync(CreateCustomerPaymentDto input)
        {
            // Validate customer exists
            await _customerRepository.GetAsync(input.CustomerId);

            var payment = new CustomerPayment(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.CustomerId,
                new MonetaryValue(input.Amount, input.Currency),
                input.PaymentDate,
                input.PaymentMethod,
                input.Reference,
                input.PaymentSource
            );

            await _paymentRepository.InsertAsync(payment, autoSave: true);

            return ObjectMapper.Map<CustomerPayment, CustomerPaymentDto>(payment);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ViewPayments)]
        public async Task<decimal> GetTotalPaymentsForCustomerAsync(Guid customerId)
        {
            return await _customPaymentRepository.GetTotalPaymentsForCustomerAsync(customerId);
        }
    }
}
