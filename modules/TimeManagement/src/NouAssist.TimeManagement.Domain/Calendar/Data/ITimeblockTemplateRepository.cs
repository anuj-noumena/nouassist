using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.TimeManagement.Calendar.Data
{
    public interface ITimeblockTemplateRepository : IRepository<Aggregates.TimeblockTemplate, Guid>
    {
        Task<List<Aggregates.TimeblockTemplate>> GetActiveTemplatesAsync(
            Guid ownerId, 
            CancellationToken cancellationToken = default);
        
        Task<List<Aggregates.TimeblockTemplate>> GetTemplatesForDayAsync(
            Guid ownerId, 
            int dayOfWeek, 
            CancellationToken cancellationToken = default);
    }
}
