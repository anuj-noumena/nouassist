using System;
using System.Linq.Expressions;
using NouAssist.Team.Aggregates;
using NouAssist.Team.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Team.Specifications
{
    public class ActiveTeamsSpecification : Specification<Aggregates.Team>
    {
        public override Expression<Func<Aggregates.Team, bool>> ToExpression() =>
            t => t.IsActive;
    }

    public class PublicTeamsSpecification : Specification<Aggregates.Team>
    {
        public override Expression<Func<Aggregates.Team, bool>> ToExpression() =>
            t => !t.IsPrivate;
    }

    public class TeamsByTypeSpecification : Specification<Aggregates.Team>
    {
        private readonly TeamType _teamType;

        public TeamsByTypeSpecification(TeamType teamType)
        {
            _teamType = teamType;
        }

        public override Expression<Func<Aggregates.Team, bool>> ToExpression() =>
            t => t.TeamType == _teamType;
    }

    public class TeamsByLeaderSpecification : Specification<Aggregates.Team>
    {
        private readonly Guid _leaderId;

        public TeamsByLeaderSpecification(Guid leaderId)
        {
            _leaderId = leaderId;
        }

        public override Expression<Func<Aggregates.Team, bool>> ToExpression() =>
            t => t.LeaderId.HasValue && t.LeaderId.Value == _leaderId;
    }

    public class ChildTeamsSpecification : Specification<Aggregates.Team>
    {
        private readonly Guid _parentTeamId;

        public ChildTeamsSpecification(Guid parentTeamId)
        {
            _parentTeamId = parentTeamId;
        }

        public override Expression<Func<Aggregates.Team, bool>> ToExpression() =>
            t => t.ParentTeamId.HasValue && t.ParentTeamId.Value == _parentTeamId;
    }
}
