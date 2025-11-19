using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsPayable.Data
{
    public interface IBillRepository : IRepository<Bill, Guid>
    {
        Task<Bill?> GetWithDetailsAsync(Guid id);
        Task<List<Bill>> GetBillsByVendorAsync(Guid vendorId);
        Task<List<Bill>> GetBillsByStatusAsync(Guid? tenantId, BillStatus status);
        Task<List<Bill>> GetOverdueBillsAsync(Guid? tenantId);
        Task<List<Bill>> GetBillsDueInPeriodAsync(Guid? tenantId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalOutstandingAsync(Guid? tenantId);
    }
}
