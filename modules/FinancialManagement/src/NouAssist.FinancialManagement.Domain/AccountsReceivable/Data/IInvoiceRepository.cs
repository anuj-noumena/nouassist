using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsReceivable.Data
{
    public interface IInvoiceRepository : IRepository<Invoice, Guid>
    {
        Task<Invoice?> GetWithDetailsAsync(Guid id);
        Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber, Guid? tenantId);
        Task<List<Invoice>> GetInvoicesByCustomerAsync(Guid customerId);
        Task<List<Invoice>> GetInvoicesByStatusAsync(Guid? tenantId, InvoiceStatus status);
        Task<List<Invoice>> GetOverdueInvoicesAsync(Guid? tenantId);
        Task<List<Invoice>> GetInvoicesDueInPeriodAsync(Guid? tenantId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalReceivablesAsync(Guid? tenantId);
        Task<string> GetNextInvoiceNumberAsync(Guid? tenantId);
    }
}
