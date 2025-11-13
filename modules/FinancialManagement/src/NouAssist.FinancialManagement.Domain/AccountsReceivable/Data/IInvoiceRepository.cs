using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsReceivable.Data
{
    public interface IInvoiceRepository : IRepository<Invoice, Guid>
    {
        Task<Invoice> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<Invoice>> GetByCustomerAsync(
            Guid customerId,
            InvoiceStatus? status = null,
            CancellationToken cancellationToken = default);

        Task<List<Invoice>> GetOverdueInvoicesAsync(
            int? daysOverdue = null,
            CancellationToken cancellationToken = default);
    }
}
