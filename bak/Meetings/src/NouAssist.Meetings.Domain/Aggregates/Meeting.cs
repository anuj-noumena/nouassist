using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Meetings.Enums;
using NouAssist.Meetings.Events;
using NouAssist.Meetings.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Meetings.Aggregates;

public class Meeting : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid OwnerId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Objective { get; private set; }

    // Scheduling
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public string Location { get; private set; }
    public string MeetingUrl { get; private set; }
    public string CalendarEventId { get; private set; }

    // Meeting type
    public MeetingType MeetingType { get; private set; }
    public MeetingFrequency Frequency { get; private set; }
    public Guid? RecurringSeriesId { get; private set; }

    // Organizer
    public Guid? OrganizerId { get; private set; }
    public int? ExpectedAttendeesCount { get; private set; }

    // Execution
    public DateTime? ActualStartTime { get; private set; }
    public DateTime? ActualEndTime { get; private set; }
    public MeetingStatus Status { get; private set; }

    // Notes & Outcomes
    public string Notes { get; private set; }
    public string Decisions { get; private set; }
    public string Summary { get; private set; }
    public string AgendaNotes { get; private set; }

    // Recordings
    public string RecordingUrl { get; private set; }
    public string TranscriptUrl { get; private set; }

    // Related entities
    public Guid? ProjectId { get; private set; }
    public Guid? SprintId { get; private set; }

    // Cancellation
    public bool IsCancelled { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public string CancellationReason { get; private set; }

    public Dictionary<string, object> Metadata { get; private set; }
    public Guid? TenantId { get; private set; }

    private readonly List<MeetingAttendee> _attendees = new();
    public IReadOnlyCollection<MeetingAttendee> Attendees => _attendees.AsReadOnly();

    private readonly List<MeetingAgendaItem> _agendaItems = new();
    public IReadOnlyCollection<MeetingAgendaItem> AgendaItems => _agendaItems.AsReadOnly();

    private readonly List<MeetingActionItem> _actionItems = new();
    public IReadOnlyCollection<MeetingActionItem> ActionItems => _actionItems.AsReadOnly();

    private readonly List<Guid> _attachmentIds = new();
    public IReadOnlyCollection<Guid> AttachmentIds => _attachmentIds.AsReadOnly();

    private readonly List<Guid> _relatedTaskIds = new();
    public IReadOnlyCollection<Guid> RelatedTaskIds => _relatedTaskIds.AsReadOnly();

    protected Meeting()
    {
        /* ORM */
    }

    public Meeting(
        Guid id,
        Guid ownerId,
        string title,
        DateTime startAt,
        DateTime endAt,
        MeetingType meetingType,
        Guid? tenantId
    ) : base(id)
    {
        OwnerId = ownerId;
        SetTitle(title);
        SetSchedule(startAt, endAt);
        MeetingType = meetingType;
        TenantId = tenantId;
        Status = MeetingStatus.Scheduled;
        Frequency = MeetingFrequency.OneTime;
        IsCancelled = false;
        Metadata = new Dictionary<string, object>();
        OrganizerId = ownerId;

        AddLocalEvent(new MeetingScheduledEto
        {
            MeetingId = Id,
            Title = title,
            StartAt = startAt,
            EndAt = endAt,
            OrganizerId = ownerId,
            MeetingType = meetingType,
            AttendeeIds = new List<Guid>(),
            TenantId = tenantId
        });
    }

    public void SetTitle(string title)
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title), 200);
    }

    public void SetDescription(string description)
    {
        if (!string.IsNullOrEmpty(description) && description.Length > 2000)
        {
            throw new BusinessException("Meetings:DescriptionTooLong");
        }

        Description = description;
    }

    public void SetObjective(string objective)
    {
        Objective = objective;
    }

    public void SetSchedule(DateTime startAt, DateTime endAt)
    {
        if (startAt >= endAt)
        {
            throw new BusinessException("Meetings:InvalidSchedule")
                .WithData("Reason", "Start time must be before end time");
        }

        if (startAt < DateTime.UtcNow.AddMinutes(-5) && Status == MeetingStatus.Scheduled)
        {
            throw new BusinessException("Meetings:CannotScheduleInPast");
        }

        StartAt = startAt;
        EndAt = endAt;
    }

    public void SetLocation(string location)
    {
        Location = location;
    }

    public void SetMeetingUrl(string url)
    {
        MeetingUrl = url;
    }

    public void SetCalendarEventId(string eventId)
    {
        CalendarEventId = eventId;
    }

    public void SetRecurring(MeetingFrequency frequency, Guid? seriesId = null)
    {
        Frequency = frequency;
        RecurringSeriesId = seriesId ?? Guid.NewGuid();
    }

    // Status checks
    public bool IsScheduled => Status == MeetingStatus.Scheduled;
    public bool IsInProgress => Status == MeetingStatus.InProgress;
    public bool IsCompleted => Status == MeetingStatus.Completed;
    public bool IsVirtual => !string.IsNullOrEmpty(MeetingUrl);
    public bool HasRecording => !string.IsNullOrEmpty(RecordingUrl);
    public bool HasTranscript => !string.IsNullOrEmpty(TranscriptUrl);

    public TimeSpan Duration => EndAt - StartAt;

    public TimeSpan? ActualDuration =>
        ActualStartTime.HasValue && ActualEndTime.HasValue
            ? ActualEndTime.Value - ActualStartTime.Value
            : null;

    public bool IsUpcoming => StartAt > DateTime.UtcNow;
    public bool IsPast => EndAt < DateTime.UtcNow;
    public bool IsHappeningNow => DateTime.UtcNow >= StartAt && DateTime.UtcNow <= EndAt;

    // State transitions
    public void Start()
    {
        if (Status != MeetingStatus.Scheduled)
        {
            throw new BusinessException("Meetings:CanOnlyStartScheduledMeetings");
        }

        Status = MeetingStatus.InProgress;
        ActualStartTime = DateTime.UtcNow;
    }

    public void Complete(string notes = null, string decisions = null, string summary = null)
    {
        if (Status == MeetingStatus.Completed)
        {
            throw new BusinessException("Meetings:AlreadyCompleted");
        }

        Status = MeetingStatus.Completed;
        ActualEndTime = DateTime.UtcNow;
        Notes = notes;
        Decisions = decisions;
        Summary = summary;
    }

    public void Cancel(string reason)
    {
        if (IsCancelled)
        {
            return;
        }

        IsCancelled = true;
        Status = MeetingStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        CancellationReason = Check.NotNullOrWhiteSpace(reason, nameof(reason));
        AddDistributedEvent(new MeetingCancelledEto
        {
            MeetingId = Id,
            Title = Title,
            Reason = reason,
            CancelledAt = CancelledAt.Value,
            TenantId = TenantId
        });
    }

    // Attendee management
    public void AddAttendee(MeetingAttendee attendee)
    {
        if (_attendees.Any(a => a.UserId == attendee.UserId))
        {
            throw new BusinessException("Meetings:AttendeeAlreadyAdded");
        }

        _attendees.Add(attendee);
    }

    public void RemoveAttendee(Guid userId)
    {
        var attendee = _attendees.FirstOrDefault(a => a.UserId == userId);
        if (attendee != null)
        {
            _attendees.Remove(attendee);
        }
    }

    public void UpdateAttendeeStatus(Guid userId, AttendeeStatus status)
    {
        var attendee = _attendees.FirstOrDefault(a => a.UserId == userId);
        if (attendee == null)
        {
            throw new BusinessException("Meetings:AttendeeNotFound");
        }

        // Create updated attendee (value object is immutable)
        var updatedAttendee = new MeetingAttendee(
            attendee.UserId,
            attendee.Email,
            attendee.Name,
            attendee.Role,
            status,
            attendee.IsRequired,
            attendee.IsOrganizer
        );

        _attendees.Remove(attendee);
        _attendees.Add(updatedAttendee);
    }

    public int GetAttendeeCount() => _attendees.Count;
    public int GetConfirmedCount() => _attendees.Count(a => a.Status == AttendeeStatus.Accepted);
    public int GetDeclinedCount() => _attendees.Count(a => a.Status == AttendeeStatus.Declined);

    // Agenda management
    public void AddAgendaItem(MeetingAgendaItem item)
    {
        _agendaItems.Add(item);
    }

    public void RemoveAgendaItem(Guid itemId)
    {
        var item = _agendaItems.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            _agendaItems.Remove(item);
        }
    }

    public void SetAgendaNotes(string notes)
    {
        AgendaNotes = notes;
    }

    // Action items
    public void AddActionItem(MeetingActionItem item)
    {
        _actionItems.Add(item);
    }

    public void CompleteActionItem(Guid actionItemId)
    {
        var item = _actionItems.FirstOrDefault(a => a.Id == actionItemId);
        if (item == null)
        {
            throw new BusinessException("Meetings:ActionItemNotFound");
        }

        var completed = item.Complete();
        _actionItems.Remove(item);
        _actionItems.Add(completed);
    }

    public int GetOpenActionItemsCount() => _actionItems.Count(a => !a.IsCompleted);

    // Recording & Transcript
    public void AddRecording(string url)
    {
        RecordingUrl = Check.NotNullOrWhiteSpace(url, nameof(url));
    }

    public void AddTranscript(string url)
    {
        TranscriptUrl = Check.NotNullOrWhiteSpace(url, nameof(url));
    }

    // Related entities
    public void LinkToProject(Guid projectId)
    {
        ProjectId = projectId;
    }

    public void LinkToSprint(Guid sprintId)
    {
        SprintId = sprintId;
    }

    public void AddRelatedTask(Guid taskId)
    {
        if (!_relatedTaskIds.Contains(taskId))
        {
            _relatedTaskIds.Add(taskId);
        }
    }

    public void AddAttachment(Guid attachmentId)
    {
        if (!_attachmentIds.Contains(attachmentId))
        {
            _attachmentIds.Add(attachmentId);
        }
    }

    public MeetingMetrics GetMetrics()
    {
        return new MeetingMetrics
        {
            Status = Status.ToString(),
            AttendeeCount = GetAttendeeCount(),
            ConfirmedCount = GetConfirmedCount(),
            AcceptanceRate = GetAttendeeCount() > 0
                ? (double)GetConfirmedCount() / GetAttendeeCount() * 100
                : 0,
            DurationMinutes = Duration.Minutes,
            HasAgenda = _agendaItems.Any(),
            AgendaItemCount = _agendaItems.Count,
            ActionItemCount = _actionItems.Count,
            OpenActionItems = GetOpenActionItemsCount(),
            HasRecording = HasRecording
        };
    }
}


public class MeetingMetrics
{
    public string Status { get; set; }
    public int AttendeeCount { get; set; }
    public int ConfirmedCount { get; set; }
    public double AcceptanceRate { get; set; }
    public int? DurationMinutes { get; set; }
    public bool HasAgenda { get; set; }
    public int AgendaItemCount { get; set; }
    public int ActionItemCount { get; set; }
    public int OpenActionItems { get; set; }
    public bool HasRecording { get; set; }
}