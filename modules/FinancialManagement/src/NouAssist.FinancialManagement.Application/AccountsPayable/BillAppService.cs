using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Data;
using NouAssist.FinancialManagement.AccountsPayable.Dtos;
using NouAssist.FinancialManagement.AccountsPayable.Entities;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.AccountsPayable
{
    [Authorize(FinancialManagementPermissions.AccountsPayable.ViewBills)]
    public class BillAppService : ApplicationService, IBillAppService
    {
        private readonly IBillRepository _billRepository;
        private readonly IVendorRepository _vendorRepository;

        public BillAppService(
            IBillRepository billRepository,
            IVendorRepository vendorRepository)
        {
            _billRepository = billRepository;
            _vendorRepository = vendorRepository;
        }

        public async Task<BillDto> GetAsync(Guid id)
        {
            var bill = await _billRepository.GetWithDetailsAsync(id);
            return await MapToDto(bill);
        }

        public async Task<PagedResultDto<BillDto>> GetListAsync(GetBillListDto input)
        {
            var queryable = await _billRepository.GetQueryableAsync();

            if (input.VendorId.HasValue)
            {
                queryable = queryable.Where(b => b.VendorId == input.VendorId.Value);
            }

            if (input.Status.HasValue)
            {
                queryable = queryable.Where(b => b.Status == input.Status.Value);
            }

            if (input.DueDateFrom.HasValue)
            {
                queryable = queryable.Where(b => b.DueDate >= input.DueDateFrom.Value);
            }

            if (input.DueDateTo.HasValue)
            {
                queryable = queryable.Where(b => b.DueDate <= input.DueDateTo.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(b => b.IssueDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var bills = await AsyncExecuter.ToListAsync(queryable);
            var dtos = new List<BillDto>();

            foreach (var bill in bills)
            {
                dtos.Add(await MapToDto(bill));
            }

            return new PagedResultDto<BillDto>(totalCount, dtos);
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.CreateBills)]
        public async Task<BillDto> CreateAsync(CreateBillDto input)
        {
            // Validate vendor exists
            var vendor = await _vendorRepository.GetAsync(input.VendorId);

            // Check for duplicate bill number
            var existingBill = await _billRepository.GetByBillNumberAsync(
                input.VendorId,
                input.BillNumber);

            if (existingBill != null)
            {
                throw new Volo.Abp.UserFriendlyException(
                    $"Bill number '{input.BillNumber}' already exists for this vendor");
            }

            var amount = new MonetaryValue(input.Amount, input.Currency);

            var bill = new Bill(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.BillNumber,
                input.VendorId,
                amount,
                input.IssueDate,
                input.DueDate,
                input.CategoryId,
                input.ProjectId,
                null,
                input.Notes);

            // Add line items
            foreach (var lineDto in input.LineItems)
            {
                var unitPrice = new MonetaryValue(lineDto.UnitPrice, lineDto.Currency);
                var lineItem = new BillLineItem(
                    bill.Id,
                    lineDto.Description,
                    lineDto.Quantity,
                    unitPrice);

                bill.AddLineItem(lineItem);
            }

            await _billRepository.InsertAsync(bill);

            return await MapToDto(bill);
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.CreateBills)]
        public async Task<BillDto> SubmitAsync(Guid id)
        {
            var bill = await _billRepository.GetWithDetailsAsync(id);
            bill.Submit();
            await _billRepository.UpdateAsync(bill);
            return await MapToDto(bill);
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.ApproveBills)]
        public async Task<BillDto> ApproveAsync(Guid id, ApproveBillDto input)
        {
            var bill = await _billRepository.GetWithDetailsAsync(id);

            if (input.IsApproved)
            {
                bill.Approve(
                    CurrentUser.Id.Value,
                    Enums.ApprovalLevel.Manager,
                    input.Comments);
            }
            else
            {
                bill.Reject(
                    CurrentUser.Id.Value,
                    Enums.ApprovalLevel.Manager,
                    input.Comments);
            }

            await _billRepository.UpdateAsync(bill);

            return await MapToDto(bill);
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.CreateBills)]
        public async Task DeleteAsync(Guid id)
        {
            await _billRepository.DeleteAsync(id);
        }

        private async Task<BillDto> MapToDto(Bill bill)
        {
            var dto = ObjectMapper.Map<Bill, BillDto>(bill);

            // Load vendor name
            var vendor = await _vendorRepository.GetAsync(bill.VendorId);
            dto.VendorName = vendor.Name;

            return dto;
        }
    }
}
