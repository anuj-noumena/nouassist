using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories
{
    /// <summary>
    /// Repository interface for DueDiligenceDataRoom aggregate
    /// </summary>
    public interface IDueDiligenceDataRoomRepository : IRepository<DueDiligenceDataRoom, Guid>
    {
        Task<List<DueDiligenceDataRoom>> GetAccessibleDataRoomsAsync(
            Guid investorId,
            CancellationToken cancellationToken = default);
        
        Task<DueDiligenceDataRoom> GetWithDocumentsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
        
        Task<List<DueDiligenceDataRoom>> GetActiveDataRoomsAsync(
            CancellationToken cancellationToken = default);
        
        Task<List<DueDiligenceDataRoom>> GetExpiringDataRoomsAsync(
            DateTime beforeDate,
            CancellationToken cancellationToken = default);
    }
}
