using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using NouAssist.FinancialManagement.AccountsReceivable.ValueObjects;
using NouAssist.FinancialManagement.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.AccountsReceivable
{
    [Authorize(FinancialManagementPermissions.AccountsReceivable.Default)]
    public class CustomerAppService : ApplicationService, ICustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerAppService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDto> GetAsync(Guid id)
        {
            var customer = await _customerRepository.GetAsync(id);
            return MapToDto(customer);
        }

        public async Task<PagedResultDto<CustomerDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _customerRepository.GetQueryableAsync();

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderBy(c => c.Name)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var customers = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<CustomerDto>(
                totalCount,
                customers.Select(MapToDto).ToList()
            );
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ManageCustomers)]
        public async Task<CustomerDto> CreateAsync(CreateCustomerDto input)
        {
            // Check if customer already exists
            var exists = await _customerRepository.CustomerExistsAsync(input.Name);
            if (exists)
            {
                throw new Volo.Abp.UserFriendlyException($"Customer '{input.Name}' already exists");
            }

            var creditTerms = new CreditTerms(
                input.CreditTermsDueDays,
                input.CreditLimit,
                input.Currency);

            var customer = new Customer(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.Name,
                input.Email,
                input.Type,
                creditTerms,
                input.TaxId,
                input.Phone);

            await _customerRepository.InsertAsync(customer);

            return MapToDto(customer);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ManageCustomers)]
        public async Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto input)
        {
            var customer = await _customerRepository.GetAsync(id);

            // Update contact information (simplified for brevity)
            // In real implementation, you'd update all necessary fields

            await _customerRepository.UpdateAsync(customer);

            return MapToDto(customer);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ManageCustomers)]
        public async Task DeleteAsync(Guid id)
        {
            var customer = await _customerRepository.GetAsync(id);
            customer.Deactivate();
            await _customerRepository.UpdateAsync(customer);
        }

        private CustomerDto MapToDto(Customer customer)
        {
            var dto = ObjectMapper.Map<Customer, CustomerDto>(customer);
            dto.CreditLimit = customer.CreditTerms.CreditLimit;
            dto.AvailableCredit = customer.CreditTerms.CreditLimit - customer.CurrentBalance.Amount;
            return dto;
        }
    }
}
