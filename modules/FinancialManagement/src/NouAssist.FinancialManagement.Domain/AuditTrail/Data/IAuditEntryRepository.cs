using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AuditTrail.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.AuditTrail.Data
{
    public interface IAuditEntryRepository : IRepository<AuditEntry, Guid>
    {
        Task<List<AuditEntry>> SearchAsync(AuditSearchCriteria criteria);
        
        Task<long> GetCountAsync(AuditSearchCriteria criteria);
        
        Task<List<AuditEntry>> GetEntriesForArchivalAsync(DateTime currentDate);
        
        Task<List<AuditEntry>> GetExpiredEntriesAsync(DateTime currentDate);
        
        Task<List<AuditEntry>> GetByEntityAsync(
            string entityType, 
            string entityId, 
            DateTime? since = null);
        
        Task<List<AuditEntry>> GetByUserAsync(
            Guid userId, 
            DateTime startDate, 
            DateTime endDate);
    }
}
