namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Specifications
{
    /// <summary>
    /// Base specification class for PrioritizationActivity
    /// </summary>
    public abstract class PrioritizationActivitySpecification : Specification<PrioritizationActivity>
    {
    }

    /// <summary>
    /// Specification to find activities by status
    /// </summary>
    public class ActivityStatusSpecification : PrioritizationActivitySpecification
    {
        private readonly ActivityStatus _status;

        public ActivityStatusSpecification(ActivityStatus status)
        {
            _status = status;
        }

        public override Expression<Func<PrioritizationActivity, bool>> ToExpression()
        {
            return activity => activity.Status == _status;
        }
    }

    /// <summary>
    /// Specification to find activities by prioritization method
    /// </summary>
    public class PrioritizationMethodSpecification : PrioritizationActivitySpecification
    {
        private readonly PrioritizationMethod _method;

        public PrioritizationMethodSpecification(PrioritizationMethod method)
        {
            _method = method;
        }

        public override Expression<Func<PrioritizationActivity, bool>> ToExpression()
        {
            return activity => activity.Method == _method;
        }
    }

    /// <summary>
    /// Specification to find active activities (In Progress or Voting)
    /// </summary>
    public class ActiveActivitiesSpecification : PrioritizationActivitySpecification
    {
        public override Expression<Func<PrioritizationActivity, bool>> ToExpression()
        {
            return activity => activity.Status == ActivityStatus.In_Progress ||
                              activity.Status == ActivityStatus.Voting;
        }
    }

    /// <summary>
    /// Specification to find activities where a stakeholder is participating
    /// </summary>
    public class StakeholderParticipatingSpecification : PrioritizationActivitySpecification
    {
        private readonly Guid _stakeholderId;

        public StakeholderParticipatingSpecification(Guid stakeholderId)
        {
            _stakeholderId = stakeholderId;
        }

        public override Expression<Func<PrioritizationActivity, bool>> ToExpression()
        {
            return activity => activity.ParticipantIds.Contains(_stakeholderId);
        }
    }

    /// <summary>
    /// Specification to find activities with specific requirements
    /// </summary>
    public class ContainsRequirementSpecification : PrioritizationActivitySpecification
    {
        private readonly Guid _requirementId;

        public ContainsRequirementSpecification(Guid requirementId)
        {
            _requirementId = requirementId;
        }

        public override Expression<Func<PrioritizationActivity, bool>> ToExpression()
        {
            return activity => activity.RequirementIds.Contains(_requirementId);
        }
    }

    /// <summary>
    /// Specification to find activities with low voting progress
    /// </summary>
    public class LowVotingProgressSpecification : PrioritizationActivitySpecification
    {
        private readonly decimal _threshold;

        public LowVotingProgressSpecification(decimal threshold = 0.5m)
        {
            _threshold = threshold;
        }

        public override Expression<Func<PrioritizationActivity, bool>> ToExpression()
        {
            return activity => activity.Status == ActivityStatus.Voting &&
                              activity.GetVotingProgress() < _threshold;
        }
    }

    /// <summary>
    /// Specification to find overdue activities
    /// </summary>
    public class OverdueActivitiesSpecification : PrioritizationActivitySpecification
    {
        private readonly DateTime _currentDate;

        public OverdueActivitiesSpecification(DateTime currentDate)
        {
            _currentDate = currentDate;
        }

        public override Expression<Func<PrioritizationActivity, bool>> ToExpression()
        {
            return activity => activity.EndDate.HasValue &&
                              activity.EndDate < _currentDate &&
                              (activity.Status == ActivityStatus.In_Progress ||
                               activity.Status == ActivityStatus.Voting);
        }
    }
}
