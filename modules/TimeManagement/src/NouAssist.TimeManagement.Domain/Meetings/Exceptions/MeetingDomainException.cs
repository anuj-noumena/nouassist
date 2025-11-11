using System;
using Volo.Abp;

namespace NouAssist.Meetings.Exceptions
{
    public class MeetingException : BusinessException
    {
        public MeetingException(string message, string code = null) 
            : base(code ?? "Meeting:Error", message)
        {
        }
    }

    public class MeetingSchedulingException : BusinessException
    {
        public MeetingSchedulingException(string message, string code = null) 
            : base(code ?? "Meeting:SchedulingError", message)
        {
        }
    }

    public class MeetingConflictException : BusinessException
    {
        public MeetingConflictException(int conflictCount) 
            : base("Meeting:ScheduleConflict", 
                  $"Meeting conflicts with {conflictCount} existing meeting(s)")
        {
            WithData("ConflictCount", conflictCount);
        }
    }
}
