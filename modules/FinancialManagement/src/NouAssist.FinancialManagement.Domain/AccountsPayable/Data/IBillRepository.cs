using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsPayable.Data
{
    public interface IBillRepository : IRepository<Bill, Guid>
    {
        Task<Bill> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<Bill>> GetByVendorAsync(
            Guid vendorId,
            BillStatus? status = null,
            CancellationToken cancellationToken = default);

        Task<List<Bill>> GetOverdueBillsAsync(
            CancellationToken cancellationToken = default);

        Task<Bill> GetByBillNumberAsync(
            Guid vendorId,
            string billNumber,
            CancellationToken cancellationToken = default);
    }
}
