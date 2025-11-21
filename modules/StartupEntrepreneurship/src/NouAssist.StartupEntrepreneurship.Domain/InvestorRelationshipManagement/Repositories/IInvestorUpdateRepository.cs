using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories
{
    /// <summary>
    /// Repository interface for InvestorUpdate aggregate
    /// </summary>
    public interface IInvestorUpdateRepository : IRepository<InvestorUpdate, Guid>
    {
        Task<List<InvestorUpdate>> GetListAsync(
            Specification<InvestorUpdate> specification,
            CancellationToken cancellationToken = default);
        
        Task<List<InvestorUpdate>> GetScheduledUpdatesAsync(
            DateTime cutoffDate,
            CancellationToken cancellationToken = default);
        
        Task<List<InvestorUpdate>> GetByFrequencyAsync(
            UpdateFrequency frequency,
            CancellationToken cancellationToken = default);
        
        Task<InvestorUpdate> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
        
        Task<List<InvestorUpdate>> GetEngagementReportAsync(
            DateTime fromDate,
            DateTime toDate,
            CancellationToken cancellationToken = default);
    }
}
