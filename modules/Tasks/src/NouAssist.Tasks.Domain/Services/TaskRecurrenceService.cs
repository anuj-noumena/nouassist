using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NouAssist.Tasks.Aggregates;
using NouAssist.Tasks.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.Tasks.Services
{
    public class TaskRecurrenceService : DomainService
    {
        private readonly ILogger<TaskRecurrenceService> _logger;

        public TaskRecurrenceService(ILogger<TaskRecurrenceService> logger)
        {
            _logger = logger;
        }

        public List<DateTime> CalculateNextOccurrences(
            RecurrenceRule recurrenceRule,
            DateTime startDate,
            int maxOccurrences = 10)
        {
            if (recurrenceRule == null)
            {
                throw new BusinessException("Tasks:RecurrenceRuleRequired");
            }

            var occurrences = new List<DateTime>();
            var currentDate = startDate;
            var count = 0;

            while (count < maxOccurrences)
            {
                // Check if we've reached the end date
                if (recurrenceRule.EndDate.HasValue && currentDate > recurrenceRule.EndDate.Value)
                {
                    break;
                }

                // Check if we've reached the count limit
                if (recurrenceRule.Count.HasValue && count >= recurrenceRule.Count.Value)
                {
                    break;
                }

                // Calculate next occurrence
                currentDate = recurrenceRule.CalculateNextOccurrence(currentDate);
                
                // For weekly recurrence with specific days
                if (recurrenceRule.Frequency == RecurrenceFrequency.Weekly && 
                    recurrenceRule.DaysOfWeek.Any())
                {
                    // Adjust to match specified days of week
                    var targetDay = recurrenceRule.DaysOfWeek
                        .OrderBy(d => d)
                        .FirstOrDefault(d => d >= currentDate.DayOfWeek);
                    
                    if (targetDay != default)
                    {
                        var daysToAdd = ((int)targetDay - (int)currentDate.DayOfWeek + 7) % 7;
                        currentDate = currentDate.AddDays(daysToAdd);
                    }
                }

                occurrences.Add(currentDate);
                count++;
            }

            return occurrences;
        }

        public bool ShouldCreateOccurrence(
            Task taskBlueprint,
            DateTime checkDate,
            List<TaskInstance> existingInstances)
        {
            if (!taskBlueprint.IsRecurring || taskBlueprint.RecurrenceRule == null)
            {
                return false;
            }

            // Check if an instance already exists for this date
            var existingForDate = existingInstances.Any(ti =>
                ti.TaskId == taskBlueprint.Id &&
                ti.PlannedStartDate.HasValue &&
                ti.PlannedStartDate.Value.Date == checkDate.Date);

            if (existingForDate)
            {
                _logger.LogDebug(
                    "Task instance already exists for {Date} for task {TaskId}",
                    checkDate, taskBlueprint.Id);
                return false;
            }

            // Check if the date is within the recurrence pattern
            var nextOccurrence = taskBlueprint.RecurrenceRule.CalculateNextOccurrence(
                taskBlueprint.CreationTime);

            // If check date matches or is past the next occurrence, create it
            return checkDate.Date >= nextOccurrence.Date;
        }
    }
}
