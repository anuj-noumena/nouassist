using System;
using System.Linq.Expressions;
using NouAssist.Infrastructure.Reminders.Aggregates;
using NouAssist.Notification.Reminders.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.Reminders.Specifications
{
    public class DueRemindersSpecification : Specification<Reminder>
    {
        private readonly DateTime _upToDateTime;

        public DueRemindersSpecification(DateTime upToDateTime)
        {
            _upToDateTime = upToDateTime;
        }

        public override Expression<Func<Reminder, bool>> ToExpression() =>
            r => !r.IsSent && 
                 r.RemindAt <= _upToDateTime && 
                 r.Status != ReminderStatus.Cancelled &&
                 !r.IsDeleted;
    }

    public class OverdueRemindersSpecification : Specification<Reminder>
    {
        private readonly DateTime _now;

        public OverdueRemindersSpecification(DateTime now)
        {
            _now = now;
        }

        public override Expression<Func<Reminder, bool>> ToExpression() =>
            r => !r.IsSent && 
                 r.RemindAt < _now && 
                 r.Status == ReminderStatus.Pending &&
                 !r.IsDeleted;
    }

    public class PendingRemindersSpecification : Specification<Reminder>
    {
        private readonly Guid _userId;

        public PendingRemindersSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<Reminder, bool>> ToExpression() =>
            r => r.OwnerId == _userId && 
                 !r.IsSent && 
                 r.Status == ReminderStatus.Pending &&
                 !r.IsDeleted;
    }

    public class RemindersByEntitySpecification : Specification<Reminder>
    {
        private readonly ReminderEntityType _entityType;
        private readonly Guid _entityId;

        public RemindersByEntitySpecification(ReminderEntityType entityType, Guid entityId)
        {
            _entityType = entityType;
            _entityId = entityId;
        }

        public override Expression<Func<Reminder, bool>> ToExpression() =>
            r => r.RefEntityType == _entityType && 
                 r.RefEntityId == _entityId &&
                 !r.IsDeleted;
    }

    public class UpcomingRemindersSpecification : Specification<Reminder>
    {
        private readonly Guid _userId;
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;

        public UpcomingRemindersSpecification(Guid userId, DateTime startTime, DateTime endTime)
        {
            _userId = userId;
            _startTime = startTime;
            _endTime = endTime;
        }

        public override Expression<Func<Reminder, bool>> ToExpression() =>
            r => r.OwnerId == _userId &&
                 !r.IsSent &&
                 r.RemindAt >= _startTime &&
                 r.RemindAt <= _endTime &&
                 r.Status != ReminderStatus.Cancelled &&
                 !r.IsDeleted;
    }

    public class RecurringRemindersSpecification : Specification<Reminder>
    {
        public override Expression<Func<Reminder, bool>> ToExpression() =>
            r => r.Schedule != null && 
                 r.Schedule.RecurrencePattern != RecurrencePattern.None &&
                 !r.IsDeleted;
    }
}
