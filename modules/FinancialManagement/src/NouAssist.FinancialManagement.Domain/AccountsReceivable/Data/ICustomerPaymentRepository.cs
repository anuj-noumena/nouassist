using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AccountsReceivable.Data
{
    public interface ICustomerPaymentRepository : IRepository<CustomerPayment, Guid>
    {
        Task<CustomerPayment?> GetByReferenceAsync(string reference, Guid? tenantId);
        Task<List<CustomerPayment>> GetPaymentsByCustomerAsync(Guid customerId);
        Task<List<CustomerPayment>> GetPaymentsBySourceAsync(Guid? tenantId, PaymentSource source);
        Task<List<CustomerPayment>> GetPaymentsInPeriodAsync(Guid? tenantId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalPaymentsForCustomerAsync(Guid customerId);
    }
}
