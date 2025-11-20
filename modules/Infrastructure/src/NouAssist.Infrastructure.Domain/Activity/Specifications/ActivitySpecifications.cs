using System;
using System.Linq.Expressions;
using NouAssist.Infrastructure.Activity.Aggregates;
using NouAssist.Infrastructure.Activity.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.Activity.Specifications
{
    public class ActivitiesSpecification : Specification<ActivityFeed>
    {
        private readonly Guid _userId;
        private readonly bool _showRead;
        private readonly DateTime? _fromDate;
        private readonly DateTime? _toDate;

        public ActivitiesSpecification(Guid userId, bool? showRead = false, DateTime? fromDate = null, DateTime? toDate = null)
        {
            _userId = userId;
            _showRead = showRead ?? false;
            _fromDate = fromDate;
            _toDate = toDate;
        }

        public override Expression<Func<ActivityFeed, bool>> ToExpression() =>
            a =>  // Condition 1: Filter by read status
                (_showRead || a.IsRead == false) &&

                // Condition 2: Filter by user involvement
                (a.ActorId == _userId || a.TargetUserId == _userId) &&

                // Condition 3: Filter by date range (fromDate)
                (_fromDate == null || a.OccurredAt >= _fromDate) &&

                // Condition 4: Filter by date range (toDate)
                (_toDate == null || a.OccurredAt <= _toDate);
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
