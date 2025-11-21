using System;
using System.Linq.Expressions;
using NouAssist.RequirementsManagement.Traceability.Aggregates;
using NouAssist.RequirementsManagement.Traceability.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.RequirementsManagement.Traceability.Specifications
{
    /// <summary>
    /// Base specification class for TraceLink aggregate
    /// </summary>
    public abstract class TraceLinkSpecification : Specification<TraceLink>
    {
    }

    /// <summary>
    /// Specification that identifies suspect links needing verification
    /// </summary>
    public class SuspectLinkSpecification : TraceLinkSpecification
    {
        public override Expression<Func<TraceLink, bool>> ToExpression()
        {
            return link => link.IsSuspect;
        }
    }

    /// <summary>
    /// Specification that identifies links between artifacts of specific types
    /// </summary>
    public class ArtifactTypeLinksSpecification : TraceLinkSpecification
    {
        private readonly ArtifactType _sourceType;
        private readonly ArtifactType _targetType;

        public ArtifactTypeLinksSpecification(ArtifactType sourceType, ArtifactType targetType)
        {
            _sourceType = sourceType;
            _targetType = targetType;
        }

        public override Expression<Func<TraceLink, bool>> ToExpression()
        {
            return link => link.SourceArtifact.ArtifactIdentifier.Type == _sourceType &&
                           link.TargetArtifact.ArtifactIdentifier.Type == _targetType;
        }
    }

    /// <summary>
    /// Specification that identifies links of a specific relationship type
    /// </summary>
    public class LinkTypeSpecification : TraceLinkSpecification
    {
        private readonly LinkType _linkType;

        public LinkTypeSpecification(LinkType linkType)
        {
            _linkType = linkType;
        }

        public override Expression<Func<TraceLink, bool>> ToExpression()
        {
            return link => link.LinkType == _linkType;
        }
    }

    /// <summary>
    /// Specification that identifies links involving a specific artifact
    /// </summary>
    public class ArtifactInvolvementSpecification : TraceLinkSpecification
    {
        private readonly string _artifactId;
        private readonly bool _asSource;
        private readonly bool _asTarget;

        public ArtifactInvolvementSpecification(string artifactId, bool asSource = true, bool asTarget = true)
        {
            _artifactId = artifactId;
            _asSource = asSource;
            _asTarget = asTarget;
        }

        public override Expression<Func<TraceLink, bool>> ToExpression()
        {
            if (_asSource && _asTarget)
            {
                return link => link.SourceArtifact.ArtifactIdentifier.Id == _artifactId ||
                               link.TargetArtifact.ArtifactIdentifier.Id == _artifactId;
            }
            
            if (_asSource)
            {
                return link => link.SourceArtifact.ArtifactIdentifier.Id == _artifactId;
            }
            
            if (_asTarget)
            {
                return link => link.TargetArtifact.ArtifactIdentifier.Id == _artifactId;
            }
            
            // This would match nothing if neither asSource nor asTarget is true
            return link => false;
        }
    }
}
