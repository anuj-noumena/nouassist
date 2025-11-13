using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using NouAssist.FinancialManagement.AccountsReceivable.Dtos;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using NouAssist.FinancialManagement.Permissions;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.AccountsReceivable
{
    [Authorize(FinancialManagementPermissions.AccountsReceivable.ViewInvoices)]
    public class InvoiceAppService : ApplicationService, IInvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ICustomerRepository _customerRepository;

        public InvoiceAppService(
            IInvoiceRepository invoiceRepository,
            ICustomerRepository customerRepository)
        {
            _invoiceRepository = invoiceRepository;
            _customerRepository = customerRepository;
        }

        public async Task<InvoiceDto> GetAsync(Guid id)
        {
            var invoice = await _invoiceRepository.GetWithDetailsAsync(id);
            return await MapToDto(invoice);
        }

        public async Task<PagedResultDto<InvoiceDto>> GetListAsync(GetInvoiceListDto input)
        {
            var queryable = await _invoiceRepository.GetQueryableAsync();

            if (input.CustomerId.HasValue)
            {
                queryable = queryable.Where(i => i.CustomerId == input.CustomerId.Value);
            }

            if (input.Status.HasValue)
            {
                queryable = queryable.Where(i => i.Status == input.Status.Value);
            }

            if (input.IssueDateFrom.HasValue)
            {
                queryable = queryable.Where(i => i.IssueDate >= input.IssueDateFrom.Value);
            }

            if (input.IssueDateTo.HasValue)
            {
                queryable = queryable.Where(i => i.IssueDate <= input.IssueDateTo.Value);
            }

            if (input.OverdueOnly.GetValueOrDefault(false))
            {
                var today = DateTime.Today;
                queryable = queryable.Where(i => 
                    i.DueDate < today && 
                    i.Status != Enums.InvoiceStatus.Paid &&
                    i.Status != Enums.InvoiceStatus.Cancelled);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            queryable = queryable
                .OrderByDescending(i => i.IssueDate)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var invoices = await AsyncExecuter.ToListAsync(queryable);
            var dtos = new List<InvoiceDto>();

            foreach (var invoice in invoices)
            {
                dtos.Add(await MapToDto(invoice));
            }

            return new PagedResultDto<InvoiceDto>(totalCount, dtos);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.CreateInvoices)]
        public async Task<InvoiceDto> CreateAsync(CreateInvoiceDto input)
        {
            // Validate customer exists
            var customer = await _customerRepository.GetAsync(input.CustomerId);

            var invoiceNumber = await GenerateInvoiceNumber();

            var invoice = new Invoice(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                invoiceNumber,
                input.CustomerId,
                input.IssueDate,
                input.DueDate,
                input.ProjectId,
                input.Notes);

            // Add line items
            foreach (var lineDto in input.LineItems)
            {
                var unitPrice = new MonetaryValue(lineDto.UnitPrice, lineDto.Currency);
                var lineItem = new InvoiceLineItem(
                    invoice.Id,
                    lineDto.Description,
                    lineDto.Quantity,
                    unitPrice);

                invoice.AddLineItem(lineItem);
            }

            await _invoiceRepository.InsertAsync(invoice);

            return await MapToDto(invoice);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.SendInvoices)]
        public async Task<InvoiceDto> SendAsync(Guid id)
        {
            var invoice = await _invoiceRepository.GetWithDetailsAsync(id);
            var customer = await _customerRepository.GetAsync(invoice.CustomerId);

            invoice.Send(customer.Name);

            // Update customer balance
            customer.AddToBalance(invoice.TotalAmount);
            await _customerRepository.UpdateAsync(customer);

            await _invoiceRepository.UpdateAsync(invoice);

            return await MapToDto(invoice);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.ProcessPayments)]
        public async Task<InvoiceDto> ApplyPaymentAsync(Guid id, ApplyPaymentDto input)
        {
            var invoice = await _invoiceRepository.GetWithDetailsAsync(id);
            var amount = new MonetaryValue(input.Amount, input.Currency);

            invoice.ApplyPayment(input.PaymentId, amount, CurrentUser.Id.Value);

            // Update customer balance
            var customer = await _customerRepository.GetAsync(invoice.CustomerId);
            customer.SubtractFromBalance(amount);
            await _customerRepository.UpdateAsync(customer);

            await _invoiceRepository.UpdateAsync(invoice);

            return await MapToDto(invoice);
        }

        [Authorize(FinancialManagementPermissions.AccountsReceivable.CreateInvoices)]
        public async Task DeleteAsync(Guid id)
        {
            var invoice = await _invoiceRepository.GetAsync(id);
            invoice.Cancel("Deleted by user");
            await _invoiceRepository.UpdateAsync(invoice);
        }

        private async Task<InvoiceDto> MapToDto(Invoice invoice)
        {
            var dto = ObjectMapper.Map<Invoice, InvoiceDto>(invoice);

            // Load customer name
            var customer = await _customerRepository.GetAsync(invoice.CustomerId);
            dto.CustomerName = customer.Name;

            return dto;
        }

        private async Task<string> GenerateInvoiceNumber()
        {
            var now = Clock.Now;
            var prefix = $"INV-{now:yyyyMM}";
            
            var queryable = await _invoiceRepository.GetQueryableAsync();
            var count = await AsyncExecuter.CountAsync(
                queryable.Where(i => i.InvoiceNumber.StartsWith(prefix)));

            return $"{prefix}-{(count + 1):D4}";
        }
    }
}
