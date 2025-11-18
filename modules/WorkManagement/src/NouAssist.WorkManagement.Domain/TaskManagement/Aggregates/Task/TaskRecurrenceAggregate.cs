using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Events;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates.Task
{
    /// <summary>
    /// Manages the recurrence pattern for a task blueprint.
    /// This aggregate now contains the recurrence properties and logic directly,
    /// instead of using a separate RecurrenceRule value object.
    /// </summary>
    public class TaskRecurrenceAggregate : FullAuditedAggregateRoot<Guid>
    {
        public Guid TaskId { get; private set; }

        // === RECURRENCE PROPERTIES (previously in RecurrenceRule Value Object) ===
        public RecurrenceFrequency? Frequency { get; private set; }
        public int Interval { get; private set; }
        public DateTime? EndDate { get; private set; }
        public int? Count { get; private set; }
        public List<DayOfWeek> DaysOfWeek { get; private set; }

        protected TaskRecurrenceAggregate() 
        {
            // Initialize collections to prevent null reference exceptions
            DaysOfWeek = new List<DayOfWeek>();
        }

        public TaskRecurrenceAggregate(Guid id, Guid taskId) : base(id)
        {
            TaskId = taskId;
            // Initialize collections to prevent null reference exceptions
            DaysOfWeek = new List<DayOfWeek>();
        }

        // === RECURRENCE MANAGEMENT ===

        /// <summary>
        /// Sets the recurrence pattern for the task by defining its properties directly on the aggregate.
        /// </summary>
        public void SetRecurrence(
            RecurrenceFrequency frequency,
            int interval = 1,
            DateTime? endDate = null,
            int? count = null,
            List<DayOfWeek> daysOfWeek = null)
        {
            // --- Validation Logic (previously in RecurrenceRule constructor) ---
            if (interval < 1)
            {
                throw new BusinessException("Tasks:InvalidRecurrenceInterval");
            }

            if (endDate.HasValue && count.HasValue)
            {
                throw new BusinessException("Tasks:CannotHaveBothEndDateAndCount");
            }
            // --- End Validation ---

            // Set the properties on the aggregate
            Frequency = frequency;
            Interval = interval;
            EndDate = endDate;
            Count = count;
            DaysOfWeek = daysOfWeek ?? new List<DayOfWeek>();

            AddDistributedEvent(new TaskRecurrenceSetEto 
            { 
                TaskRecurrenceId = Id, 
                RecurrencePattern = GetRecurrencePatternString() 
            });
        }

        /// <summary>
        /// Clears the recurrence pattern, resetting all properties to their default state.
        /// </summary>
        public void ClearRecurrence()
        {
            Frequency = null;
            Interval = 0; // Reset to an invalid state to signify no recurrence
            EndDate = null;
            Count = null;
            DaysOfWeek = new List<DayOfWeek>();

            AddDistributedEvent(new TaskRecurrenceClearedEto { TaskRecurrenceId = Id });
        }

        /// <summary>
        /// Calculates the next occurrence date based on the current recurrence settings.
        /// </summary>
        public DateTime CalculateNextOccurrence(DateTime currentDate)
        {
            if (Frequency == null)
            {
                throw new InvalidOperationException("Recurrence pattern is not set.");
            }

            return Frequency switch
            {
                RecurrenceFrequency.Daily => currentDate.AddDays(Interval),
                RecurrenceFrequency.Weekly => currentDate.AddDays(7 * Interval),
                RecurrenceFrequency.Monthly => currentDate.AddMonths(Interval),
                RecurrenceFrequency.Yearly => currentDate.AddYears(Interval),
                _ => throw new InvalidOperationException("Invalid frequency")
            };
        }

        private string GetRecurrencePatternString()
        {
            if (Frequency == null)
            {
                return null;
            }

            var pattern = $"{Frequency}, every {Interval} ";
            pattern += Frequency == RecurrenceFrequency.Daily ? "day" :
                      Frequency == RecurrenceFrequency.Weekly ? "week" :
                      Frequency == RecurrenceFrequency.Monthly ? "month" : "year";
            
            if (Interval > 1)
            {
                pattern += "s";
            }

            if (EndDate.HasValue)
            {
                pattern += $", ending on {EndDate:yyyy-MM-dd}";
            }
            else if (Count.HasValue)
            {
                pattern += $", for {Count} occurrence{(Count > 1 ? "s" : "")}";
            }

            return pattern;
        }
    }
}