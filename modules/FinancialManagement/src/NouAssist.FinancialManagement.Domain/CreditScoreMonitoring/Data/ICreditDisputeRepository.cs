using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.CreditScoreMonitoring.Enums;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.Data
{
    public interface ICreditDisputeRepository : IRepository<CreditDispute, Guid>
    {
        Task<List<CreditDispute>> GetUserDisputesAsync(Guid userId);
        Task<List<CreditDispute>> GetActiveDisputesAsync(Guid userId);
        Task<List<CreditDispute>> GetDisputesByStatusAsync(Guid userId, DisputeStatus status);
    }
}
