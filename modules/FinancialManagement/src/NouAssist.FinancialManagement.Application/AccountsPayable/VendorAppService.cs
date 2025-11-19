using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Dtos;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsPayable
{
    [Authorize(FinancialManagementPermissions.AccountsPayable.Default)]
    public class VendorAppService : ApplicationService, IVendorAppService
    {
        private readonly IRepository<Vendor, Guid> _vendorRepository;

        public VendorAppService(IRepository<Vendor, Guid> vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.ManageVendors)]
        public async Task<VendorDto> GetAsync(Guid id)
        {
            var vendor = await _vendorRepository.GetAsync(id);
            return ObjectMapper.Map<Vendor, VendorDto>(vendor);
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.ManageVendors)]
        public async Task<PagedResultDto<VendorDto>> GetListAsync(GetVendorListDto input)
        {
            var queryable = await _vendorRepository.GetQueryableAsync();

            if (input.ActiveOnly == true)
            {
                queryable = queryable.Where(v => v.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(input.SearchTerm))
            {
                queryable = queryable.Where(v => v.Name.Contains(input.SearchTerm));
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderBy(v => v.Name)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var vendors = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<VendorDto>(
                totalCount,
                ObjectMapper.Map<List<Vendor>, List<VendorDto>>(vendors)
            );
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.ManageVendors)]
        public async Task<VendorDto> CreateAsync(CreateVendorDto input)
        {
            var vendor = new Vendor(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.Name,
                new TaxId(input.TaxId),
                input.Email,
                input.PaymentTermsDays,
                input.PreferredPaymentMethod
            );

            vendor.UpdateContactInfo(input.Email, input.Phone, input.Address);

            await _vendorRepository.InsertAsync(vendor);
            return ObjectMapper.Map<Vendor, VendorDto>(vendor);
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.ManageVendors)]
        public async Task<VendorDto> UpdateAsync(Guid id, UpdateVendorDto input)
        {
            var vendor = await _vendorRepository.GetAsync(id);

            vendor.UpdateContactInfo(input.Email, input.Phone, input.Address);
            vendor.SetPaymentTerms(input.PaymentTermsDays, input.PreferredPaymentMethod);

            await _vendorRepository.UpdateAsync(vendor);
            return ObjectMapper.Map<Vendor, VendorDto>(vendor);
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.ManageVendors)]
        public async Task DeleteAsync(Guid id)
        {
            await _vendorRepository.DeleteAsync(id);
        }
    }
}
