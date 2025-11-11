using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.TimeManagement.Meetings.Aggregates;
using NouAssist.TimeManagement.Meetings.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.TimeManagement.Meetings.Services
{
    public class MeetingSchedulerService : DomainService
    {
        public MeetingSchedulingResult ScheduleMeeting(
            string title,
            DateTime startAt,
            DateTime endAt,
            Guid organizerId,
            MeetingType meetingType,
            List<Meeting> existingMeetings,
            Guid? tenantId
        )
        {
            var result = new MeetingSchedulingResult();

            // Validate time slot
            if (startAt < Clock.Now)
            {
                result.AddError("Cannot schedule meeting in the past");
                return result;
            }

            if (endAt <= startAt)
            {
                result.AddError("End time must be after start time");
                return result;
            }

            // Check for conflicts
            var conflicts = DetectScheduleConflicts(startAt, endAt, existingMeetings);
            if (conflicts.Any())
            {
                result.Conflicts = conflicts;
                result.AddWarning($"Found {conflicts.Count} conflicting meetings");
            }

            // Create meeting
            try
            {
                var meeting = new Meeting(
                    Guid.NewGuid(),
                    organizerId,
                    title,
                    startAt,
                    endAt,
                    meetingType,
                    tenantId
                );

                result.Meeting = meeting;
                result.IsSuccessful = true;
                result.Message = "Meeting scheduled successfully";
            }
            catch (BusinessException ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }

        public List<Meeting> DetectScheduleConflicts(
            DateTime startAt,
            DateTime endAt,
            List<Meeting> existingMeetings
        )
        {
            return existingMeetings
                .Where(m => !m.IsCancelled && m.IsScheduled)
                .Where(m =>
                    (startAt >= m.StartAt && startAt < m.EndAt) ||
                    (endAt > m.StartAt && endAt <= m.EndAt) ||
                    (startAt <= m.StartAt && endAt >= m.EndAt))
                .ToList();
        }

        public Meeting CreateRecurringMeeting(
            string title,
            DateTime startAt,
            DateTime endAt,
            MeetingFrequency frequency,
            MeetingType meetingType,
            Guid organizerId,
            Guid? tenantId,
            int occurrences
        )
        {
            var seriesId = Guid.NewGuid();
            
            var meeting = new Meeting(
                Guid.NewGuid(),
                organizerId,
                title,
                startAt,
                endAt,
                meetingType,
                tenantId
            );

            meeting.SetRecurring(frequency, seriesId);

            return meeting;
        }

        public List<Meeting> GenerateRecurringOccurrences(
            Meeting templateMeeting,
            int numberOfOccurrences
        )
        {
            if (templateMeeting.Frequency == MeetingFrequency.OneTime)
            {
                throw new BusinessException("Meetings:NotRecurringMeeting");
            }

            var occurrences = new List<Meeting>();
            var duration = templateMeeting.Duration;

            for (int i = 1; i < numberOfOccurrences; i++)
            {
                var nextStart = CalculateNextOccurrence(
                    templateMeeting.StartAt,
                    templateMeeting.Frequency,
                    i
                );
                var nextEnd = nextStart.Add(duration);

                var occurrence = new Meeting(
                    Guid.NewGuid(),
                    templateMeeting.OwnerId,
                    templateMeeting.Title,
                    nextStart,
                    nextEnd,
                    templateMeeting.MeetingType,
                    templateMeeting.TenantId
                );

                occurrence.SetRecurring(
                    templateMeeting.Frequency,
                    templateMeeting.RecurringSeriesId
                );

                occurrences.Add(occurrence);
            }

            return occurrences;
        }

        private DateTime CalculateNextOccurrence(
            DateTime startDate,
            MeetingFrequency frequency,
            int occurrenceNumber
        )
        {
            return frequency switch
            {
                MeetingFrequency.Daily => startDate.AddDays(occurrenceNumber),
                MeetingFrequency.Weekly => startDate.AddDays(7 * occurrenceNumber),
                MeetingFrequency.Biweekly => startDate.AddDays(14 * occurrenceNumber),
                MeetingFrequency.Monthly => startDate.AddMonths(occurrenceNumber),
                MeetingFrequency.Quarterly => startDate.AddMonths(3 * occurrenceNumber),
                _ => startDate
            };
        }

        public bool CanUserEditMeeting(Meeting meeting, Guid userId)
        {
            // Organizer can always edit
            if (meeting.OrganizerId == userId)
            {
                return true;
            }

            // Owner can always edit
            if (meeting.OwnerId == userId)
            {
                return true;
            }

            return false;
        }
    }

    public class MeetingSchedulingResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public Meeting Meeting { get; set; }
        public List<Meeting> Conflicts { get; set; } = new();
        public List<string> Errors { get; private set; } = new();
        public List<string> Warnings { get; private set; } = new();

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public void AddWarning(string warning)
        {
            Warnings.Add(warning);
        }

        public bool HasErrors => Errors.Any();
        public bool HasWarnings => Warnings.Any();
        public bool HasConflicts => Conflicts.Any();
    }
}
