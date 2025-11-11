using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Tasks.ValueObjects;

public class TaskSchedule : ValueObject
{
    public DateTime? PlannedStartDate { get; private set; }
    public DateTime? PlannedDueDate { get; private set; }
    public DateTime? ActualStartDate { get; private set; }
    public DateTime? ActualDueDate { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // Constructor
    private TaskSchedule()
    {
    }

    public TaskSchedule(
        DateTime? plannedStartDate,
        DateTime? plannedDueDate,
        DateTime? actualStartDate = null,
        DateTime? actualDueDate = null,
        DateTime? completedAt = null)
    {
        PlannedStartDate = plannedStartDate;
        PlannedDueDate = plannedDueDate;
        ActualStartDate = actualStartDate;
        ActualDueDate = actualDueDate;
        CompletedAt = completedAt;
    }

    // Factory methods for immutability and clarity
    public TaskSchedule WithPlannedDates(DateTime? start = null, DateTime? due = null)
    {
        return new TaskSchedule(start?? PlannedStartDate, due ?? PlannedDueDate, ActualStartDate, ActualDueDate, CompletedAt);
    }

    public TaskSchedule WithActualDates(DateTime? start = null, DateTime? due = null)
    {
        return new TaskSchedule(PlannedStartDate, PlannedDueDate, start ?? ActualStartDate, due ?? ActualDueDate, CompletedAt);
    }

    public TaskSchedule MarkCompleted(DateTime completedAt)
    {
        return new TaskSchedule(PlannedStartDate, PlannedDueDate, ActualStartDate, ActualDueDate, completedAt);
    }
    
    public TaskSchedule MarkIncompleted()
    {
        return new TaskSchedule(PlannedStartDate, PlannedDueDate, ActualStartDate, ActualDueDate, null);
    }
    public bool IsCompleted => CompletedAt.HasValue;
    public bool IsOverdue => !IsCompleted && ActualDueDate.HasValue &&
                             DateTime.UtcNow > ActualDueDate.Value;
    public TimeSpan? TimeRemaining => ActualDueDate.HasValue && !IsCompleted
        ? ActualDueDate.Value - DateTime.UtcNow
        : null;
    public bool HasStarted => ActualStartDate.HasValue;
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return PlannedStartDate;
        yield return PlannedDueDate;
        yield return ActualStartDate;
        yield return ActualDueDate;
        yield return CompletedAt;
    }
}