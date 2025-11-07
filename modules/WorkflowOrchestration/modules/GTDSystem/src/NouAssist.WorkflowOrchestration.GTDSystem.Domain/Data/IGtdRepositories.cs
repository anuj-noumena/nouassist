using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Core.Enums;
using NouAssist.WorkflowOrchestration.GTDSystem.Aggregates;
using NouAssist.WorkflowOrchestration.GTDSystem.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkflowOrchestration.GTDSystem.Data
{
    public interface IGtdHorizonRepository : IRepository<GtdHorizon, Guid>
    {
        Task<GtdHorizon?> FindByUserAndLevelAsync(Guid userId, GtdHorizonLevel level);
        Task<List<GtdHorizon>> GetByUserAsync(Guid userId);
        Task<List<GtdHorizon>> GetNeedingReviewAsync(Guid userId);
    }

    public interface ISomedayMaybeRepository : IRepository<SomedayMaybeItem, Guid>
    {
        Task<List<SomedayMaybeItem>> GetActiveByUserAsync(Guid userId);
        Task<List<SomedayMaybeItem>> GetNeedingReviewAsync(Guid userId);
    }

    public interface ITicklerRepository : IRepository<TicklerItem, Guid>
    {
        Task<List<TicklerItem>> GetDueTicklersAsync(Guid userId, DateTime currentDate);
        Task<List<TicklerItem>> GetSurfacedTicklersAsync(Guid userId);
        Task<List<TicklerItem>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
    }

    public interface IWaitingForRepository : IRepository<WaitingFor, Guid>
    {
        Task<List<WaitingFor>> GetActiveByUserAsync(Guid userId);
        Task<List<WaitingFor>> GetFollowUpDueAsync(Guid userId, DateTime currentDate);
    }
}
