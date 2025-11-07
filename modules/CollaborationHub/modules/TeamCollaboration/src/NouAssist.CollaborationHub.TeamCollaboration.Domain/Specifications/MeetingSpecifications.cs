using System;
using System.Linq.Expressions;
using NouAssist.CollaborationHub.TeamCollaboration.Aggregates;
using NouAssist.CollaborationHub.TeamCollaboration.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.CollaborationHub.TeamCollaboration.Specifications
{
    public class UpcomingMeetingsSpecification : Specification<Meeting>
    {
        public override Expression<Func<Meeting, bool>> ToExpression() =>
            m => m.StartAt > DateTime.UtcNow && 
                 !m.IsCancelled &&
                 m.Status == MeetingStatus.Scheduled;
    }

    public class TodaysMeetingsSpecification : Specification<Meeting>
    {
        private readonly DateTime _today;

        public TodaysMeetingsSpecification()
        {
            _today = DateTime.UtcNow.Date;
        }

        public override Expression<Func<Meeting, bool>> ToExpression() =>
            m => m.StartAt.Date == _today && !m.IsCancelled;
    }

    public class MeetingsByTypeSpecification : Specification<Meeting>
    {
        private readonly MeetingType _meetingType;

        public MeetingsByTypeSpecification(MeetingType meetingType)
        {
            _meetingType = meetingType;
        }

        public override Expression<Func<Meeting, bool>> ToExpression() =>
            m => m.MeetingType == _meetingType;
    }

    public class RecurringMeetingsSpecification : Specification<Meeting>
    {
        public override Expression<Func<Meeting, bool>> ToExpression() =>
            m => m.Frequency != MeetingFrequency.OneTime;
    }

    public class MeetingsByProjectSpecification : Specification<Meeting>
    {
        private readonly Guid _projectId;

        public MeetingsByProjectSpecification(Guid projectId)
        {
            _projectId = projectId;
        }

        public override Expression<Func<Meeting, bool>> ToExpression() =>
            m => m.ProjectId.HasValue && m.ProjectId.Value == _projectId;
    }

    public class CompletedMeetingsSpecification : Specification<Meeting>
    {
        public override Expression<Func<Meeting, bool>> ToExpression() =>
            m => m.Status == MeetingStatus.Completed;
    }

    public class MeetingsInDateRangeSpecification : Specification<Meeting>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public MeetingsInDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<Meeting, bool>> ToExpression() =>
            m => m.StartAt >= _startDate && m.StartAt <= _endDate;
    }
}
