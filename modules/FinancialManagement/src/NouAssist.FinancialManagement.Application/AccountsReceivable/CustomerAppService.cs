using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsReceivable
{
    [Authorize(FinancialManagementPermissions.AccountsReceivable.Default)]
    public class CustomerAppService : ApplicationService, ICustomerAppService
    {
        private readonly IRepository<Customer, Guid> _customerRepository;

        public CustomerAppService(IRepository<Customer, Guid> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ManageCustomers)]
        public async Task<CustomerDto> GetAsync(Guid id)
        {
            var customer = await _customerRepository.GetAsync(id);
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ManageCustomers)]
        public async Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomerListDto input)
        {
            var queryable = await _customerRepository.GetQueryableAsync();

            if (input.ActiveOnly == true)
            {
                queryable = queryable.Where(c => c.IsActive);
            }

            if (input.TypeFilter.HasValue)
            {
                queryable = queryable.Where(c => c.CustomerType == input.TypeFilter.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.SearchTerm))
            {
                queryable = queryable.Where(c => c.CustomerName.Contains(input.SearchTerm));
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderBy(c => c.CustomerName)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var customers = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<CustomerDto>(
                totalCount,
                ObjectMapper.Map<List<Customer>, List<CustomerDto>>(customers)
            );
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ManageCustomers)]
        public async Task<CustomerDto> CreateAsync(CreateCustomerDto input)
        {
            var customer = new Customer(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.Name,
                new TaxId(input.TaxId),
                input.Email,
                input.Type,
                input.CreditTermsDays
            );

            await _customerRepository.InsertAsync(customer);
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ManageCustomers)]
        public async Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto input)
        {
            var customer = await _customerRepository.GetAsync(id);

            // Update logic here

            await _customerRepository.UpdateAsync(customer);
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ManageCustomers)]
        public async Task DeleteAsync(Guid id)
        {
            var customer = await _customerRepository.GetAsync(id);
            customer.Deactivate();
            await _customerRepository.UpdateAsync(customer);
        }
    }
}
