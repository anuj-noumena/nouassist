namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Specifications
{
    /// <summary>
    /// Base specification class for DecisionRecord aggregate
    /// </summary>
    public abstract class DecisionRecordSpecification : Specification<DecisionRecord>
    {
    }

    /// <summary>
    /// Specification for decision records in a specific status
    /// </summary>
    public class DecisionStatusSpecification : DecisionRecordSpecification
    {
        private readonly DecisionStatus _status;

        public DecisionStatusSpecification(DecisionStatus status)
        {
            _status = status;
        }

        public override Expression<Func<DecisionRecord, bool>> ToExpression()
        {
            return decision => decision.Status == _status;
        }
    }

    /// <summary>
    /// Specification for recent decisions within a specific timeframe
    /// </summary>
    public class RecentDecisionsSpecification : DecisionRecordSpecification
    {
        private readonly DateTime _sinceDate;

        public RecentDecisionsSpecification(DateTime sinceDate)
        {
            _sinceDate = sinceDate;
        }

        public override Expression<Func<DecisionRecord, bool>> ToExpression()
        {
            return decision => decision.DecisionDate >= _sinceDate;
        }
    }

    /// <summary>
    /// Specification for decisions with specific tags
    /// </summary>
    public class DecisionWithTagSpecification : DecisionRecordSpecification
    {
        private readonly string _tag;

        public DecisionWithTagSpecification(string tag)
        {
            _tag = tag;
        }

        public override Expression<Func<DecisionRecord, bool>> ToExpression()
        {
            return decision => decision.Tags.Contains(_tag);
        }
    }

    /// <summary>
    /// Specification for decisions that can be implemented
    /// </summary>
    public class ImplementableDecisionSpecification : DecisionRecordSpecification
    {
        public override Expression<Func<DecisionRecord, bool>> ToExpression()
        {
            return decision => decision.Status == DecisionStatus.Made;
        }
    }
}
