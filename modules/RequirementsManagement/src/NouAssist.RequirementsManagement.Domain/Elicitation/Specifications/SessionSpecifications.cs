namespace NouAssist.RequirementsManagement.Elicitation.Specifications
{
    /// <summary>
    /// Base specification class for ElicitationSession aggregate
    /// </summary>
    public abstract class SessionSpecification : Specification<ElicitationSession>
    {
    }

    /// <summary>
    /// Specification that identifies completed sessions
    /// </summary>
    public class CompletedSessionSpecification : SessionSpecification
    {
        public override Expression<Func<ElicitationSession, bool>> ToExpression()
        {
            return session => session.Status == SessionStatus.Completed;
        }
    }

    /// <summary>
    /// Specification that identifies sessions scheduled for a specific date range
    /// </summary>
    public class SessionsInDateRangeSpecification : SessionSpecification
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public SessionsInDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<ElicitationSession, bool>> ToExpression()
        {
            return session => session.Schedule.StartTime >= _startDate &&
                             session.Schedule.EndTime <= _endDate;
        }
    }

    /// <summary>
    /// Specification that identifies sessions by type and status
    /// </summary>
    public class SessionByTypeAndStatusSpecification : SessionSpecification
    {
        private readonly SessionType _type;
        private readonly SessionStatus[] _statuses;

        public SessionByTypeAndStatusSpecification(SessionType type, params SessionStatus[] statuses)
        {
            _type = type;
            _statuses = statuses ?? new[] { SessionStatus.Completed };
        }

        public override Expression<Func<ElicitationSession, bool>> ToExpression()
        {
            return session => session.Type == _type &&
                             _statuses.Contains(session.Status);
        }
    }

    /// <summary>
    /// Specification that identifies sessions with artifacts generated
    /// </summary>
    public class SessionsWithArtifactsSpecification : SessionSpecification
    {
        private readonly int _minimumArtifacts;

        public SessionsWithArtifactsSpecification(int minimumArtifacts = 1)
        {
            _minimumArtifacts = minimumArtifacts;
        }

        public override Expression<Func<ElicitationSession, bool>> ToExpression()
        {
            return session => session.Status == SessionStatus.Completed &&
                             session.GeneratedArtifacts.Count >= _minimumArtifacts;
        }
    }
}
