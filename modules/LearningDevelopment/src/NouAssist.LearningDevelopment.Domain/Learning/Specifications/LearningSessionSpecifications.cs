using System;
using System.Linq.Expressions;
using NouAssist.Learning.Enums;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.LearningDevelopment.Learning.Specifications
{
    public class ActiveSessionsSpecification : Specification<LearningSession>
    {
        public override Expression<Func<LearningSession, bool>> ToExpression() =>
            s => !s.Completed && !s.Paused;
    }

    public class PausedSessionsSpecification : Specification<LearningSession>
    {
        public override Expression<Func<LearningSession, bool>> ToExpression() =>
            s => s.Paused && !s.Completed;
    }

    public class CompletedSessionsSpecification : Specification<LearningSession>
    {
        public override Expression<Func<LearningSession, bool>> ToExpression() =>
            s => s.Completed;
    }

    public class SessionsInPhaseSpecification : Specification<LearningSession>
    {
        private readonly LearningPhase _phase;

        public SessionsInPhaseSpecification(LearningPhase phase)
        {
            _phase = phase;
        }

        public override Expression<Func<LearningSession, bool>> ToExpression() =>
            s => s.CurrentPhase == _phase && !s.Completed;
    }

    public class InactiveSessionsSpecification : Specification<LearningSession>
    {
        private readonly DateTime _cutoffDate;

        public InactiveSessionsSpecification(int daysInactive)
        {
            _cutoffDate = DateTime.UtcNow.AddDays(-daysInactive);
        }

        public override Expression<Func<LearningSession, bool>> ToExpression() =>
            s => s.LastActivity < _cutoffDate && !s.Completed;
    }
}
