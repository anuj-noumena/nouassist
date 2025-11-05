using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Core.Enums;
using NouAssist.GTD.Aggregates;
using NouAssist.GTD.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.GTD.Data
{
    public interface IGtdHorizonAlignmentRepository : IRepository<GtdHorizonAlignment, Guid>
    {
        Task<GtdHorizonAlignment?> FindByHorizonAndEntityAsync(
            Guid higherHorizonId,
            string lowerEntityType,
            Guid lowerEntityId);

        Task<List<GtdHorizonAlignment>> GetByUserAndLevelAsync(
            Guid userId,
            GtdHorizonLevel level);

        Task<List<GtdHorizonAlignment>> GetWeakAlignmentsAsync(
            Guid userId,
            int weakThreshold = 2);

        Task<List<GtdHorizonAlignment>> GetByHorizonAsync(Guid horizonId);

        Task<List<GtdHorizonAlignment>> GetByEntityAsync(
            string entityType,
            Guid entityId);
    }
}
