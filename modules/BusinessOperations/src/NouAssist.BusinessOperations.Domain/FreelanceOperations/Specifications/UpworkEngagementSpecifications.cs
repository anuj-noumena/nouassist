using System;
using System.Linq.Expressions;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;

namespace NouAssist.BusinessOperations.FreelanceOperations.Specifications
{
    /// <summary>
    /// Base specification for UpworkEngagement aggregate
    /// </summary>
    public abstract class UpworkEngagementSpecification : Specification<UpworkEngagement>
    {
    }

    /// <summary>
    /// Specification for active Upwork engagements
    /// </summary>
    public class ActiveUpworkEngagementsSpecification : UpworkEngagementSpecification
    {
        public override Expression<Func<UpworkEngagement, bool>> ToExpression()
        {
            return engagement => engagement.Status == "Active" || 
                                engagement.Status == "In Progress";
        }
    }

    /// <summary>
    /// Specification for Upwork engagements linked to internal system
    /// </summary>
    public class LinkedUpworkEngagementsSpecification : UpworkEngagementSpecification
    {
        public override Expression<Func<UpworkEngagement, bool>> ToExpression()
        {
            return engagement => engagement.LinkedProjectId.HasValue || 
                                engagement.LinkedClientId.HasValue;
        }
    }

    /// <summary>
    /// Specification for unlinked Upwork engagements
    /// </summary>
    public class UnlinkedUpworkEngagementsSpecification : UpworkEngagementSpecification
    {
        public override Expression<Func<UpworkEngagement, bool>> ToExpression()
        {
            return engagement => !engagement.LinkedProjectId.HasValue && 
                                !engagement.LinkedClientId.HasValue;
        }
    }
}
