using System;
using System.Linq.Expressions;
using NouAssist.SkillDevelopment.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.SkillDevelopment.Specifications
{
    public class ActiveSessionsSpecification : Specification<SkillDevelopmentSession>
    {
        public override Expression<Func<SkillDevelopmentSession, bool>> ToExpression() =>
            s => !s.Completed && !s.Paused;
    }

    public class PausedSessionsSpecification : Specification<SkillDevelopmentSession>
    {
        public override Expression<Func<SkillDevelopmentSession, bool>> ToExpression() =>
            s => s.Paused && !s.Completed;
    }

    public class StaleSessionsSpecification : Specification<SkillDevelopmentSession>
    {
        private readonly DateTime _staleDateThreshold;

        public StaleSessionsSpecification(int daysWithoutActivity = 14)
        {
            _staleDateThreshold = DateTime.UtcNow.AddDays(-daysWithoutActivity);
        }

        public override Expression<Func<SkillDevelopmentSession, bool>> ToExpression() =>
            s => !s.Completed &&
                 !s.Paused &&
                 s.LastActivity < _staleDateThreshold;
    }
}
