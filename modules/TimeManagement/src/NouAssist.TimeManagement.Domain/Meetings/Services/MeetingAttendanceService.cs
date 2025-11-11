using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.TimeManagement.Meetings.Aggregates;
using NouAssist.TimeManagement.Meetings.Enums;
using NouAssist.TimeManagement.Meetings.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.TimeManagement.Meetings.Services
{
    public class MeetingAttendanceService : DomainService
    {
        public AttendanceResult AddAttendees(
            Meeting meeting,
            List<MeetingAttendee> attendees
        )
        {
            var result = new AttendanceResult();

            if (meeting.IsCancelled)
            {
                result.AddError("Cannot add attendees to cancelled meeting");
                return result;
            }

            foreach (var attendee in attendees)
            {
                try
                {
                    meeting.AddAttendee(attendee);
                    result.AddedCount++;
                }
                catch (BusinessException ex)
                {
                    result.AddError($"User {attendee.UserId}: {ex.Message}");
                }
            }

            result.IsSuccessful = result.AddedCount > 0;
            result.Message = $"Added {result.AddedCount} attendees";
            return result;
        }

        public AttendanceResult UpdateAttendeeResponse(
            Meeting meeting,
            Guid userId,
            AttendeeStatus status
        )
        {
            var result = new AttendanceResult();

            try
            {
                meeting.UpdateAttendeeStatus(userId, status);
                result.IsSuccessful = true;
                result.Message = $"Attendance status updated to {status}";
            }
            catch (BusinessException ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }

        public double CalculateAttendanceRate(Meeting meeting)
        {
            if (meeting.GetAttendeeCount() == 0)
            {
                return 0;
            }

            var acceptedCount = meeting.GetConfirmedCount();
            return (double)acceptedCount / meeting.GetAttendeeCount() * 100;
        }

        public List<MeetingAttendee> GetRequiredAttendees(Meeting meeting)
        {
            return meeting.Attendees
                .Where(a => a.IsRequired)
                .ToList();
        }

        public List<MeetingAttendee> GetPendingResponses(Meeting meeting)
        {
            return meeting.Attendees
                .Where(a => a.Status == AttendeeStatus.Pending)
                .ToList();
        }

        public bool AllRequiredAttendeesAccepted(Meeting meeting)
        {
            var requiredAttendees = GetRequiredAttendees(meeting);
            if (!requiredAttendees.Any())
            {
                return true;
            }

            return requiredAttendees.All(a => a.HasAccepted);
        }
    }

    public class AttendanceResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public int AddedCount { get; set; }
        public List<string> Errors { get; private set; } = new();

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public bool HasErrors => Errors.Any();
    }
}
