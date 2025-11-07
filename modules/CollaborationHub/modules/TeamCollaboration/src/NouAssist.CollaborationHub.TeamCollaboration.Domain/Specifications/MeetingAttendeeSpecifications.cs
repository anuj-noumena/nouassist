using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.CollaborationHub.TeamCollaboration.Aggregates;
using NouAssist.CollaborationHub.TeamCollaboration.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.CollaborationHub.TeamCollaboration.Specifications
{
    public class MeetingsRequiringResponseSpecification : Specification<Meeting>
    {
        private readonly Guid _userId;

        public MeetingsRequiringResponseSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<Meeting, bool>> ToExpression() =>
            m => m.IsUpcoming && 
                 !m.IsCancelled &&
                 m.Attendees.Any(a => a.UserId == _userId && 
                                     a.Status == AttendeeStatus.Pending);
    }

    public class MeetingsWithRecordingsSpecification : Specification<Meeting>
    {
        public override Expression<Func<Meeting, bool>> ToExpression() =>
            m => !string.IsNullOrEmpty(m.RecordingUrl);
    }

    public class MeetingsNeedingFollowUpSpecification : Specification<Meeting>
    {
        public override Expression<Func<Meeting, bool>> ToExpression() =>
            m => m.Status == MeetingStatus.Completed &&
                 m.ActionItems.Any(a => !a.IsCompleted);
    }
}
