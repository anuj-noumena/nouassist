using System;
using System.Linq.Expressions;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.Enums;

namespace NouAssist.RequirementsManagement.Elicitation.Specifications
{
    /// <summary>
    /// Base specification class for ElicitationArtifact aggregate
    /// </summary>
    public abstract class ArtifactSpecification : Specification<ElicitationArtifact>
    {
    }

    /// <summary>
    /// Specification that identifies approved artifacts ready for implementation
    /// </summary>
    public class ApprovedArtifactSpecification : ArtifactSpecification
    {
        public override Expression<Func<ElicitationArtifact, bool>> ToExpression()
        {
            return artifact => artifact.Status == ArtifactStatus.Approved;
        }
    }

    /// <summary>
    /// Specification that identifies artifacts by type and status
    /// </summary>
    public class ArtifactByTypeAndStatusSpecification : ArtifactSpecification
    {
        private readonly ArtifactType[] _types;
        private readonly ArtifactStatus[] _statuses;

        public ArtifactByTypeAndStatusSpecification(ArtifactType[] types, ArtifactStatus[] statuses)
        {
            _types = types ?? throw new ArgumentNullException(nameof(types));
            _statuses = statuses ?? throw new ArgumentNullException(nameof(statuses));
        }

        public override Expression<Func<ElicitationArtifact, bool>> ToExpression()
        {
            return artifact => _types.Contains(artifact.Type) &&
                              _statuses.Contains(artifact.Status);
        }
    }

    /// <summary>
    /// Specification that identifies artifacts created by a specific stakeholder
    /// </summary>
    public class ArtifactsByStakeholderSpecification : ArtifactSpecification
    {
        private readonly Guid _stakeholderId;

        public ArtifactsByStakeholderSpecification(Guid stakeholderId)
        {
            _stakeholderId = stakeholderId;
        }

        public override Expression<Func<ElicitationArtifact, bool>> ToExpression()
        {
            return artifact => artifact.CreatedByStakeholderId == _stakeholderId;
        }
    }

    /// <summary>
    /// Specification that identifies artifacts from specific sessions
    /// </summary>
    public class ArtifactsBySessionSpecification : ArtifactSpecification
    {
        private readonly Guid[] _sessionIds;

        public ArtifactsBySessionSpecification(params Guid[] sessionIds)
        {
            _sessionIds = sessionIds ?? throw new ArgumentNullException(nameof(sessionIds));
        }

        public override Expression<Func<ElicitationArtifact, bool>> ToExpression()
        {
            return artifact => _sessionIds.Contains(artifact.SessionId);
        }
    }
}
