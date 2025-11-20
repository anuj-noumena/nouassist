namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Specifications
{
    /// <summary>
    /// Base specification class for AnalyzedRequirement
    /// </summary>
    public abstract class AnalyzedRequirementSpecification : Specification<AnalyzedRequirement>
    {
    }

    /// <summary>
    /// Specification to find requirements with specific attribute types
    /// </summary>
    public class HasAttributeTypeSpecification : AnalyzedRequirementSpecification
    {
        private readonly AttributeType _attributeType;

        public HasAttributeTypeSpecification(AttributeType attributeType)
        {
            _attributeType = attributeType;
        }

        public override Expression<Func<AnalyzedRequirement, bool>> ToExpression()
        {
            return req => req.Attributes.Any(a => a.AttributeType == _attributeType);
        }
    }

    /// <summary>
    /// Specification to find requirements with specific relationship types
    /// </summary>
    public class HasRelationshipTypeSpecification : AnalyzedRequirementSpecification
    {
        private readonly RelationshipType _relationshipType;

        public HasRelationshipTypeSpecification(RelationshipType relationshipType)
        {
            _relationshipType = relationshipType;
        }

        public override Expression<Func<AnalyzedRequirement, bool>> ToExpression()
        {
            return req => req.Relationships.Any(r => r.RelationshipType == _relationshipType);
        }
    }

    /// <summary>
    /// Specification to find requirements with high impact assessment
    /// </summary>
    public class HighImpactRequirementSpecification : AnalyzedRequirementSpecification
    {
        private readonly decimal _minimumImpactScore;

        public HighImpactRequirementSpecification(decimal minimumImpactScore = 3.0m)
        {
            _minimumImpactScore = minimumImpactScore;
        }

        public override Expression<Func<AnalyzedRequirement, bool>> ToExpression()
        {
            return req => req.ImpactAssessment != null && 
                         req.ImpactAssessment.OverallScore >= _minimumImpactScore;
        }
    }

    /// <summary>
    /// Specification to find requirements analyzed within a time period
    /// </summary>
    public class AnalyzedWithinPeriodSpecification : AnalyzedRequirementSpecification
    {
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;

        public AnalyzedWithinPeriodSpecification(DateTime fromDate, DateTime toDate)
        {
            _fromDate = fromDate;
            _toDate = toDate;
        }

        public override Expression<Func<AnalyzedRequirement, bool>> ToExpression()
        {
            return req => req.LastAnalyzedAt.HasValue &&
                         req.LastAnalyzedAt >= _fromDate &&
                         req.LastAnalyzedAt <= _toDate;
        }
    }

    /// <summary>
    /// Specification to find requirements with specific attribute values
    /// </summary>
    public class HasAttributeValueSpecification : AnalyzedRequirementSpecification
    {
        private readonly string _attributeName;
        private readonly AttributeValue _expectedValue;

        public HasAttributeValueSpecification(string attributeName, AttributeValue expectedValue)
        {
            _attributeName = attributeName;
            _expectedValue = expectedValue;
        }

        public override Expression<Func<AnalyzedRequirement, bool>> ToExpression()
        {
            return req => req.Attributes.Any(a => 
                a.AttributeName == _attributeName && 
                a.AttributeValue.Equals(_expectedValue));
        }
    }

    /// <summary>
    /// Specification to find requirements that depend on specific requirements
    /// </summary>
    public class DependsOnRequirementSpecification : AnalyzedRequirementSpecification
    {
        private readonly Guid _targetRequirementId;

        public DependsOnRequirementSpecification(Guid targetRequirementId)
        {
            _targetRequirementId = targetRequirementId;
        }

        public override Expression<Func<AnalyzedRequirement, bool>> ToExpression()
        {
            return req => req.Relationships.Any(r => 
                r.RelationshipType == RelationshipType.Dependency &&
                r.TargetRequirementId == _targetRequirementId);
        }
    }

    /// <summary>
    /// Specification to find requirements without impact assessment
    /// </summary>
    public class MissingImpactAssessmentSpecification : AnalyzedRequirementSpecification
    {
        public override Expression<Func<AnalyzedRequirement, bool>> ToExpression()
        {
            return req => req.ImpactAssessment == null;
        }
    }
}
