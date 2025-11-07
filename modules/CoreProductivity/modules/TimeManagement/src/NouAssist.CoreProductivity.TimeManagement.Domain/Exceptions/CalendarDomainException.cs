using System;
using Volo.Abp;

namespace NouAssist.CoreProductivity.TimeManagement.Exceptions
{
    public class CalendarDomainException : BusinessException
    {
        public CalendarDomainException(string code, string message = null) 
            : base(code, message)
        {
        }
    }

    public class EventNotFoundException : CalendarDomainException
    {
        public EventNotFoundException(Guid eventId) 
            : base("Calendar:EventNotFound", $"Event with ID {eventId} was not found")
        {
            WithData("EventId", eventId);
        }
    }

    public class TimeblockValidationException : CalendarDomainException
    {
        public TimeblockValidationException(string message) 
            : base("Calendar:TimeblockValidation", message)
        {
        }
    }

    public class SchedulingConflictException : CalendarDomainException
    {
        public SchedulingConflictException(string message) 
            : base("Calendar:SchedulingConflict", message)
        {
        }
    }
}
