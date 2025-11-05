using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Team.Aggregates;
using NouAssist.Team.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Team.Data
{
    public interface ITeamRepository : IRepository<Aggregates.Team, Guid>
    {
        Task<Aggregates.Team> GetWithMembersAsync(Guid id);
        Task<List<Aggregates.Team>> GetActiveTeamsAsync();
        Task<List<Aggregates.Team>> GetTeamsByTypeAsync(TeamType teamType);
        Task<List<Aggregates.Team>> GetTeamsByLeaderAsync(Guid leaderId);
        Task<List<Aggregates.Team>> GetUserTeamsAsync(Guid userId);
        Task<List<Aggregates.Team>> GetListAsync(ISpecification<Aggregates.Team> spec);
    }
}
