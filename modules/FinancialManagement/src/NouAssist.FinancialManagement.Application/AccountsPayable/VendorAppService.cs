using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Data;
using NouAssist.FinancialManagement.AccountsPayable.Dtos;
using NouAssist.FinancialManagement.AccountsPayable.ValueObjects;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.AccountsPayable
{
    [Authorize(FinancialManagementPermissions.AccountsPayable.Default)]
    public class VendorAppService : ApplicationService, IVendorAppService
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorAppService(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<VendorDto> GetAsync(Guid id)
        {
            var vendor = await _vendorRepository.GetAsync(id);
            return ObjectMapper.Map<Vendor, VendorDto>(vendor);
        }

        public async Task<PagedResultDto<VendorDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _vendorRepository.GetQueryableAsync();

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
            // Check if vendor already exists
            var exists = await _vendorRepository.VendorExistsAsync(input.Name);
            if (exists)
            {
                throw new Volo.Abp.UserFriendlyException($"Vendor '{input.Name}' already exists");
            }

            var contactInfo = new ContactInfo(
                input.Email,
                input.Phone,
                input.Address,
                input.City,
                input.State,
                input.ZipCode,
                input.Country);

            var paymentTerms = new PaymentTerms(input.PaymentTermsNetDays);

            var vendor = new Vendor(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.Name,
                contactInfo,
                paymentTerms,
                Enums.PaymentMethod.ACH,
                null,
                input.TaxId);

            await _vendorRepository.InsertAsync(vendor);

            return ObjectMapper.Map<Vendor, VendorDto>(vendor);
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.ManageVendors)]
        public async Task<VendorDto> UpdateAsync(Guid id, UpdateVendorDto input)
        {
            var vendor = await _vendorRepository.GetAsync(id);

            var contactInfo = new ContactInfo(
                input.Email,
                input.Phone,
                input.Address,
                null,
                null,
                null,
                null);

            vendor.UpdateContactInfo(contactInfo);

            var paymentTerms = new PaymentTerms(input.PaymentTermsNetDays);
            vendor.UpdatePaymentTerms(paymentTerms);

            await _vendorRepository.UpdateAsync(vendor);

            return ObjectMapper.Map<Vendor, VendorDto>(vendor);
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.ManageVendors)]
        public async Task DeleteAsync(Guid id)
        {
            var vendor = await _vendorRepository.GetAsync(id);
            vendor.Deactivate();
            await _vendorRepository.UpdateAsync(vendor);
        }
    }
}
