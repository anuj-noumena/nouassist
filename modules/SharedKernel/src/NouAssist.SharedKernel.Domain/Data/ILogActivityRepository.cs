using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.SharedKernel.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.SharedKernel.Data
{
    public interface ILogActivityRepository : IRepository<LogActivity, Guid>
    {
        Task<List<LogActivity>> GetByUserAsync(Guid userId, int maxCount = 100);
        Task<List<LogActivity>> GetByEntityAsync(string entityType, Guid entityId);
        Task<List<LogActivity>> GetByDateRangeAsync(
            Guid userId, 
            DateTime startDate, 
            DateTime endDate);
    }
}
