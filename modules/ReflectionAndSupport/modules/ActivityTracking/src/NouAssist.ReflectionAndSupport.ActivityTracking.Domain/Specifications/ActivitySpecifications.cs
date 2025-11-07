using System;
using System.Linq.Expressions;
using NouAssist.ReflectionAndSupport.ActivityTracking.Aggregates;
using NouAssist.ReflectionAndSupport.ActivityTracking.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.Specifications
{
    public class UnreadActivitiesSpecification : Specification<ActivityFeed>
    {
        private readonly Guid _userId;

        public UnreadActivitiesSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<ActivityFeed, bool>> ToExpression() =>
            a => !a.IsRead && (a.ActorId == _userId || a.TargetUserId == _userId);
    }

    public class ActivitiesByEntitySpecification : Specification<ActivityFeed>
    {
        private readonly ActivityEntityType _entityType;
        private readonly Guid _entityId;

        public ActivitiesByEntitySpecification(ActivityEntityType entityType, Guid entityId)
        {
            _entityType = entityType;
            _entityId = entityId;
        }

        public override Expression<Func<ActivityFeed, bool>> ToExpression() =>
            a => a.EntityType == _entityType && a.EntityId == _entityId;
    }

    public class RecentActivitiesSpecification : Specification<ActivityFeed>
    {
        private readonly DateTime _since;

        public RecentActivitiesSpecification(DateTime since)
        {
            _since = since;
        }

        public override Expression<Func<ActivityFeed, bool>> ToExpression() =>
            a => a.OccurredAt >= _since;
    }
}
