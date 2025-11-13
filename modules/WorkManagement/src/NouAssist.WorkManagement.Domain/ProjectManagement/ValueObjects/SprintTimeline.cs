using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.ProjectManagement.ValueObjects;

/// <summary>
/// Sprint timeline - enforces duration constraints
/// </summary>
public class SprintTimeline : ValueObject
{
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime? ActualStartDate { get; private set; }
    public DateTime? ActualEndDate { get; private set; }

    private SprintTimeline()
    {
    }

    public static SprintTimeline Create(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
        {
            throw new BusinessException("Sprints:InvalidDates");
        }

        var duration = (endDate - startDate).Days;
        if (duration < 7 || duration > 30)
        {
            throw new BusinessException("Sprints:InvalidDuration",
                "Sprint duration must be between 7 and 30 days.");
        }

        return new SprintTimeline
        {
            StartDate = startDate,
            EndDate = endDate
        };
    }

    public SprintTimeline Update(DateTime startDate, DateTime endDate)
    {
        if (ActualStartDate.HasValue)
        {
            throw new BusinessException("Sprints:CannotChangeDatesAfterStart");
        }

        if (endDate <= startDate)
        {
            throw new BusinessException("Sprints:InvalidDates");
        }

        var duration = (endDate - startDate).Days;
        if (duration < 7 || duration > 30)
        {
            throw new BusinessException("Sprints:InvalidDuration");
        }

        return new SprintTimeline
        {
            StartDate = startDate,
            EndDate = endDate,
            ActualStartDate = this.ActualStartDate,
            ActualEndDate = this.ActualEndDate
        };
    }

    public SprintTimeline MarkStarted(DateTime actualStart)
    {
        return new SprintTimeline
        {
            StartDate = this.StartDate,
            EndDate = this.EndDate,
            ActualStartDate = actualStart,
            ActualEndDate = this.ActualEndDate
        };
    }

    public SprintTimeline MarkCompleted(DateTime actualEnd)
    {
        return new SprintTimeline
        {
            StartDate = this.StartDate,
            EndDate = this.EndDate,
            ActualStartDate = this.ActualStartDate,
            ActualEndDate = actualEnd
        };
    }

    public int DurationInDays => (EndDate - StartDate).Days;
    public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    public bool IsPast => DateTime.UtcNow > EndDate;
    public bool IsFuture => DateTime.UtcNow < StartDate;
    public bool HasStarted => ActualStartDate.HasValue;
    public bool IsCompleted => ActualEndDate.HasValue;

    public TimeSpan? TimeRemaining => IsCompleted || IsPast
        ? null
        : EndDate - DateTime.UtcNow;

    public int? DaysRemaining => TimeRemaining?.Days;

    public double TimeProgress
    {
        get
        {
            if (IsFuture)
            {
                return 0;
            }

            if (IsPast || IsCompleted)
            {
                return 1;
            }

            var totalDuration = (EndDate - StartDate).TotalDays;
            var elapsed = (DateTime.UtcNow - StartDate).TotalDays;

            return Math.Clamp(elapsed / totalDuration, 0, 1);
        }
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return StartDate;
        yield return EndDate;
        yield return ActualStartDate;
        yield return ActualEndDate;
    }
}