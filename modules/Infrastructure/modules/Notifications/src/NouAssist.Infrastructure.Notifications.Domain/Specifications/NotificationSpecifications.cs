using System;
using System.Linq.Expressions;
using NouAssist.Infrastructure.Notifications.Aggregates;
using NouAssist.Infrastructure.Notifications.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.Notifications.Specifications
{
    public class UnreadNotificationsSpecification : Specification<Notification>
    {
        private readonly Guid _userId;

        public UnreadNotificationsSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<Notification, bool>> ToExpression() =>
            n => n.UserId == _userId && !n.IsRead && !n.IsDismissed && !n.IsDeleted;
    }

    public class ExpiredNotificationsSpecification : Specification<Notification>
    {
        private readonly DateTime _now;

        public ExpiredNotificationsSpecification(DateTime now)
        {
            _now = now;
        }

        public override Expression<Func<Notification, bool>> ToExpression() =>
            n => n.ExpiresAt.HasValue && n.ExpiresAt.Value < _now;
    }

    public class PendingNotificationsSpecification : Specification<Notification>
    {
        public override Expression<Func<Notification, bool>> ToExpression() =>
            n => n.DeliveryStatus == NotificationDeliveryStatus.Pending && !n.IsDeleted;
    }

    public class HighPriorityNotificationsSpecification : Specification<Notification>
    {
        public override Expression<Func<Notification, bool>> ToExpression() =>
            n => n.Priority == NotificationPriority.High || n.Priority == NotificationPriority.Urgent;
    }
}
