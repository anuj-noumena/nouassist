using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Entities;
using NouAssist.FinancialManagement.Application.Contracts.AccountsPayable;
using NouAssist.FinancialManagement.Application.Contracts.AccountsPayable.Dtos;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.Application.AccountsPayable
{
    [Authorize(FinancialManagementPermissions.AccountsPayable.Default)]
    public class BillAppService : ApplicationService, IBillAppService
    {
        private readonly IRepository<Bill, Guid> _billRepository;
        private readonly IRepository<Vendor, Guid> _vendorRepository;

        public BillAppService(
            IRepository<Bill, Guid> billRepository,
            IRepository<Vendor, Guid> vendorRepository)
        {
            _billRepository = billRepository;
            _vendorRepository = vendorRepository;
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.ViewBills)]
        public async Task<BillDto> GetAsync(Guid id)
        {
            var queryable = await _billRepository.WithDetailsAsync();
            var bill = await AsyncExecuter.FirstOrDefaultAsync(
                queryable.Where(b => b.Id == id));

            return ObjectMapper.Map<Bill, BillDto>(bill);
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.ViewBills)]
        public async Task<PagedResultDto<BillDto>> GetListAsync(GetBillListDto input)
        {
            var queryable = await _billRepository.GetQueryableAsync();

            if (input.VendorFilter.HasValue)
            {
                queryable = queryable.Where(b => b.VendorId == input.VendorFilter.Value);
            }

            if (input.StatusFilter.HasValue)
            {
                queryable = queryable.Where(b => b.Status == input.StatusFilter.Value);
            }

            if (input.DueDateFrom.HasValue)
            {
                queryable = queryable.Where(b => b.DueDate >= input.DueDateFrom.Value);
            }

            if (input.DueDateTo.HasValue)
            {
                queryable = queryable.Where(b => b.DueDate <= input.DueDateTo.Value);
            }

            if (input.OverdueOnly == true)
            {
                var today = DateTime.UtcNow.Date;
                queryable = queryable.Where(b => b.DueDate < today && 
                    b.Status != AccountsPayable.Enums.BillStatus.Paid);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(b => b.DueDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var bills = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<BillDto>(
                totalCount,
                ObjectMapper.Map<List<Bill>, List<BillDto>>(bills)
            );
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.CreateBills)]
        public async Task<BillDto> CreateAsync(CreateBillDto input)
        {
            var vendor = await _vendorRepository.GetAsync(input.VendorId);

            var bill = new Bill(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.BillNumber,
                input.VendorId,
                vendor.Name,
                new MonetaryValue(input.Amount, input.Currency),
                input.IssueDate,
                input.DueDate,
                input.CategoryId,
                input.ProjectId,
                input.DocumentId
            );

            if (input.LineItems != null)
            {
                foreach (var lineDto in input.LineItems)
                {
                    var lineItem = new BillLineItem(
                        bill.Id,
                        lineDto.Description,
                        lineDto.Quantity,
                        new MonetaryValue(lineDto.UnitPrice, input.Currency),
                        lineDto.ProductId
                    );
                    bill.AddLineItem(lineItem);
                }
            }

            await _billRepository.InsertAsync(bill);
            return ObjectMapper.Map<Bill, BillDto>(bill);
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.ApproveBills)]
        public async Task<BillDto> ApproveAsync(ApproveBillDto input)
        {
            var bill = await _billRepository.GetAsync(input.BillId);
            
            bill.Approve(
                CurrentUser.Id.Value,
                input.Level,
                input.Comments
            );

            await _billRepository.UpdateAsync(bill);
            return ObjectMapper.Map<Bill, BillDto>(bill);
        }

        [Authorize(FinancialManagementPermissions.AccountsPayable.PayBills)]
        public async Task<BillDto> PayAsync(PayBillDto input)
        {
            var bill = await _billRepository.GetAsync(input.BillId);
            
            bill.MarkAsPaid(
                GuidGenerator.Create(),
                input.PaymentDate,
                input.ConfirmationNumber
            );

            await _billRepository.UpdateAsync(bill);
            return ObjectMapper.Map<Bill, BillDto>(bill);
        }
    }
}
