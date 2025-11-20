using System;
using System.Linq.Expressions;
using NouAssist.Notification.Notifications.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.Notifications.Specifications
{
    public class UnreadNotificationsSpecification : Specification<Aggregates.Notification>
    {
        private readonly Guid _userId;

        public UnreadNotificationsSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<Aggregates.Notification, bool>> ToExpression() =>
            n => n.UserId == _userId && !n.IsRead && !n.IsDismissed;
    }

    public class ExpiredNotificationsSpecification : Specification<Aggregates.Notification>
    {
        private readonly DateTime _now;

        public ExpiredNotificationsSpecification(DateTime now)
        {
            _now = now;
        }

        public override Expression<Func<Aggregates.Notification, bool>> ToExpression() =>
            n => n.ExpiresAt.HasValue && n.ExpiresAt.Value < _now;
    }

    public class PendingNotificationsSpecification : Specification<Aggregates.Notification>
    {
        public override Expression<Func<Aggregates.Notification, bool>> ToExpression() =>
            n => n.DeliveryStatus == NotificationDeliveryStatus.Pending;
    }

    public class HighPriorityNotificationsSpecification : Specification<Aggregates.Notification>
    {
        public override Expression<Func<Aggregates.Notification, bool>> ToExpression() =>
            n => n.Priority == NotificationPriority.High || n.Priority == NotificationPriority.Urgent;
    }
}
