using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsReceivable
{
    [Authorize(FinancialManagementPermissions.AccountsReceivable.Default)]
    public class InvoiceAppService : ApplicationService, IInvoiceAppService
    {
        private readonly IRepository<Invoice, Guid> _invoiceRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;

        public InvoiceAppService(
            IRepository<Invoice, Guid> invoiceRepository,
            IRepository<Customer, Guid> customerRepository)
        {
            _invoiceRepository = invoiceRepository;
            _customerRepository = customerRepository;
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ViewInvoices)]
        public async Task<InvoiceDto> GetAsync(Guid id)
        {
            var queryable = await _invoiceRepository.WithDetailsAsync();
            var invoice = await AsyncExecuter.FirstOrDefaultAsync(
                queryable.Where(i => i.Id == id));

            return ObjectMapper.Map<Invoice, InvoiceDto>(invoice);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ViewInvoices)]
        public async Task<PagedResultDto<InvoiceDto>> GetListAsync(GetInvoiceListDto input)
        {
            var queryable = await _invoiceRepository.GetQueryableAsync();

            if (input.CustomerFilter.HasValue)
            {
                queryable = queryable.Where(i => i.CustomerId == input.CustomerFilter.Value);
            }

            if (input.StatusFilter.HasValue)
            {
                queryable = queryable.Where(i => i.Status == input.StatusFilter.Value);
            }

            if (input.OverdueOnly == true)
            {
                var today = DateTime.UtcNow.Date;
                queryable = queryable.Where(i => i.DueDate < today && 
                    i.Status != AccountsReceivable.Enums.InvoiceStatus.Paid);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(i => i.IssueDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var invoices = await AsyncExecuter.ToListAsync(queryable);

            return new PagedResultDto<InvoiceDto>(
                totalCount,
                ObjectMapper.Map<List<Invoice>, List<InvoiceDto>>(invoices)
            );
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.CreateInvoices)]
        public async Task<InvoiceDto> CreateAsync(CreateInvoiceDto input)
        {
            var customer = await _customerRepository.GetAsync(input.CustomerId);

            var invoice = new Invoice(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                input.InvoiceNumber,
                input.CustomerId,
                customer.CustomerName,
                input.IssueDate,
                input.DueDate,
                input.Currency,
                input.ProjectId
            );

            foreach (var lineDto in input.LineItems)
            {
                var lineItem = new InvoiceLineItem(
                    GuidGenerator.Create(),
                    lineDto.Description,
                    lineDto.Quantity,
                    new MonetaryValue(lineDto.UnitPrice, input.Currency),
                    productId: lineDto.ProductId
                );
                invoice.AddLineItem(lineItem);
            }

            await _invoiceRepository.InsertAsync(invoice);
            return ObjectMapper.Map<Invoice, InvoiceDto>(invoice);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.SendInvoices)]
        public async Task<InvoiceDto> SendAsync(SendInvoiceDto input)
        {
            var invoice = await _invoiceRepository.GetAsync(input.InvoiceId);
            
            invoice.Send();

            await _invoiceRepository.UpdateAsync(invoice);
            return ObjectMapper.Map<Invoice, InvoiceDto>(invoice);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.RecordPayments)]
        public async Task<InvoiceDto> ApplyPaymentAsync(ApplyPaymentToInvoiceDto input)
        {
            var invoice = await _invoiceRepository.GetAsync(input.InvoiceId);
            
            invoice.ApplyPayment(
                new MonetaryValue(input.PaymentAmount, input.Currency),
                input.PaymentDate
            );

            await _invoiceRepository.UpdateAsync(invoice);
            return ObjectMapper.Map<Invoice, InvoiceDto>(invoice);
        }
    }
}
